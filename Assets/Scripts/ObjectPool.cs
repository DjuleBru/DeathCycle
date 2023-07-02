using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    [SerializeField] Transform[] spawnPoints;
    private GameObject[] pool;

    private int poolSize = 5;

<<<<<<< HEAD
    private void Awake() {
        PopulatePool();
    }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
=======
    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
        PopulatePool();
>>>>>>> parent of e2851f3 (Revert "Initial commit")
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            ResetPool();
        }
        if (e.state == LoopManager.State.Recording) {
            pool[e.loopNumber].SetActive(true);
            pool[e.loopNumber].GetComponent<Champion>().SetSpawnedLoopNumber(e.loopNumber);
        }
        if (e.state == LoopManager.State.Playbacking) {
            ResetPool();
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
        // Reset champion positions to spawn points, activate all components, reset health
        for (int i = 0; i < poolSize; i++) {
            pool[i].transform.position = spawnPoints[i].transform.position;
<<<<<<< HEAD
            pool[i].GetComponent<ChampionAim>().enabled = true;
=======
            pool[i].GetComponent<ChampionAimMelee>().enabled = true;
>>>>>>> parent of e2851f3 (Revert "Initial commit")
            pool[i].GetComponent<ChampionMovement>().enabled = true;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = true;
            pool[i].GetComponent<Collider2D>().enabled = true;
            pool[i].GetComponent<Champion>().ResetChampionHealth();
        }
    }
}
