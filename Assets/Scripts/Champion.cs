using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour
{
    private int spawnedLoopNumber;

    public int SpawnedLoopNumber { get { return spawnedLoopNumber; } }

    private bool ispawnedLoopNumberSet;

    private void Start() {
        LoopManager.Instance.OnRecordingStarted += LoopManager_OnRecordingStarted;
    }
    private void LoopManager_OnRecordingStarted(object sender, LoopManager.OnRecordingEventArgs e) {
        if (!ispawnedLoopNumberSet) {
            spawnedLoopNumber = e.loopNumber;
            ispawnedLoopNumberSet = true;
        }
    }

}
