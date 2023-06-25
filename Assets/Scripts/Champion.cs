using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour
{
    private ChampionRecPlaybackManager loopManager;

    private ChampionBehaviour championBehaviour;

    private void Start() {
        loopManager = GetComponent<ChampionRecPlaybackManager>();
        championBehaviour = GetComponent<ChampionBehaviour>();
    }
    void Update()
    {
        if (loopManager.IsRecording) {
            // Enable player inputs
        }
    }

}
