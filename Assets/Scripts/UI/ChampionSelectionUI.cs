using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ChampionSelectionUI : MonoBehaviour {

    private ChampionIconTemplateUI[] championUI;

    private void Start () {
        championUI = GetComponentsInChildren<ChampionIconTemplateUI>();
    }

    public ChampionIconTemplateUI[] GetChampionIconTemplateUIArray() {
        return championUI; 
    }

}
