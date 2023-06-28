using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    [SerializeField] Transform[] spawnPoints;
    private GameObject[] pool;

    private int poolSize = 5;

    private void Awake() {
        PopulatePool();
    }

    private void Start() {
        LoopManager.Instance.OnPlaybackStarted += LoopManager_OnPlaybackStarted;
        LoopManager.Instance.OnRecordingStarted += LoopManager_OnRecordingStarted;
    }

    private void LoopManager_OnRecordingStarted(object sender, LoopManager.OnRecordingEventArgs e) {
        pool[e.loopNumber].SetActive(true);
        pool[e.loopNumber].GetComponent<Champion>().SetSpawnedLoopNumber(e.loopNumber);

        // Reset champion positions to spawn points
        for (int i = 0; i < poolSize; i++) {
            pool[i].transform.position = spawnPoints[i].transform.position;
        }
    }

    private void LoopManager_OnPlaybackStarted(object sender, LoopManager.OnRecordingEventArgs e) {
        // Reset champion positions to spawn points
        for (int i = 0; i < poolSize; i++) {
            pool[i].transform.position = spawnPoints[i].transform.position;
        }
        }

    private void PopulatePool() {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++) {
            pool[i] = Instantiate(champion, spawnPoints[i].transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }
}
