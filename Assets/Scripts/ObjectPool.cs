using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    [SerializeField] Transform[] spawnPoints;
    private GameObject[] pool;

    private int poolSize = 5;

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
        PopulatePool();
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            ActivatePool();
            ResetPool();
        }
        if (e.state == LoopManager.State.Recording) {
            ActivatePool();
            ResetPool();
            pool[e.loopNumber].SetActive(true);
            pool[e.loopNumber].GetComponent<Champion>().SetSpawnedLoopNumber(e.loopNumber);
        }
        if (e.state == LoopManager.State.Playbacking) {
            ActivatePool();
            ResetPool();
        }
        if (e.state == LoopManager.State.PlaybackEndBuffer || e.state == LoopManager.State.RecordingEndBuffer) {
            DeactivatePool();
        }
    }

    private void PopulatePool() {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++) {
            pool[i] = Instantiate(champion, spawnPoints[i].transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }

    private void ResetPool() {
        // Reset champion positions to spawn points, reset direction, reset health, reset velocity to zero
        for (int i = 0; i < poolSize; i++) {
            pool[i].transform.position = spawnPoints[i].transform.position;
            pool[i].transform.localScale = Vector3.one;
            pool[i].GetComponent<Champion>().ResetChampionHealth();
            pool[i].GetComponent<IChampionAttack>().ResetAttacks();
            pool[i].GetComponent<Animator>().Play("Idle");
            pool[i].GetComponent<ChampionMovement>().SetVelocity(Vector3.zero);
        }
    }

    private void ActivatePool() {
        // Activate all components
        for (int i = 0; i < poolSize; i++) {
            pool[i].GetComponent<IChampionAttack>().EnableAttacks();
            pool[i].GetComponent<ChampionMovement>().enabled = true;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = true;
            pool[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    private void DeactivatePool() {
        // Deactivate all controller components
        for (int i = 0; i < poolSize; i++) {
            pool[i].GetComponent<IChampionAttack>().DisableAttacks();
            pool[i].GetComponent<ChampionMovement>().enabled = false;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = false;
        }
    }
}
