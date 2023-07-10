using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    [SerializeField] Player player;
    SpawnPoint[] spawnPoints;
    SpawnPoint selectedSpawnPoint;

    private GameObject[] pool;

    private int poolSize = 3;

    private bool loopOnPause;
    public SpawnPoint SelectedSpawnPoint { get { return selectedSpawnPoint; } }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;

        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        PopulatePool();

        loopOnPause = true;
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            loopOnPause = true;
            ActivatePool();
            ResetPool();
        }
        if (e.state == LoopManager.State.Recording) {
            loopOnPause = false;
            ActivatePool();
            ResetPool();

            foreach(SpawnPoint spawnPoint in spawnPoints) {
                if (spawnPoint == selectedSpawnPoint) {
                    spawnPoint.GetComponentInChildren<Champion>(true).gameObject.SetActive(true);
                    spawnPoint.GetComponentInChildren<Champion>().SetSpawnedLoopNumber(e.loopNumber);
                }
            }
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
            pool[i] = Instantiate(champion, spawnPoints[i].transform.position, Quaternion.identity, spawnPoints[i].transform);
            pool[i].GetComponent<Champion>().SetObjectPoolParent(this);
            pool[i].SetActive(false);
        }
    }

    private void ResetPool() {
        // Reset champion positions to spawn points, reset direction, reset health, reset velocity to zero, remove flag children
        for (int i = 0; i < poolSize; i++) {
            pool[i].transform.position = spawnPoints[i].transform.position;
            pool[i].transform.localScale = Vector3.one;
            pool[i].GetComponent<Champion>().ResetChampionHealth();
            pool[i].GetComponent<Champion>().RemoveFlagChildren();
            pool[i].GetComponent<IChampionAttack>().ResetAttacks();
            pool[i].GetComponent<Animator>().Play("Idle");
            pool[i].GetComponent<ChampionMovement>().SetVelocity(Vector3.zero);
        }
    }

    private void ActivatePool() {
        // Activate all components on all champions
        for (int i = 0; i < poolSize; i++) {
            pool[i].GetComponent<IChampionAttack>().EnableAttacks();
            pool[i].GetComponent<ChampionMovement>().enabled = true;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = true;
            pool[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    private void DeactivatePool() {
        // Deactivate all controller components on all champions
        for (int i = 0; i < poolSize; i++) {
            pool[i].GetComponent<IChampionAttack>().DisableAttacks();
            pool[i].GetComponent<ChampionMovement>().enabled = false;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = false;
        }
    }

    public void SetSelectedSpawnPoint(SpawnPoint spawnPoint) {
        selectedSpawnPoint = spawnPoint;
    }

    public Player GetPlayer() {
        return player;
    }
}
