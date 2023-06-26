using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour
{
    private int spawnedLoopNumber;

    public int SpawnedLoopNumber { get { return spawnedLoopNumber; } }

    public void SetSpawnedLoopNumber(int spawnedLoopNumber) {
        this.spawnedLoopNumber = spawnedLoopNumber;
    }

}
