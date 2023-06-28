using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    
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
    }

    private void LoopManager_OnPlaybackStarted(object sender, LoopManager.OnRecordingEventArgs e) {

    }

    private void PopulatePool() {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++) {
            pool[i] = Instantiate(champion, transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }
}
