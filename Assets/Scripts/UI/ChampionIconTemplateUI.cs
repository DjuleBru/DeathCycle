using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ChampionIconTemplateUI : MonoBehaviour
{
    [SerializeField] ChampionSO championSO;

    [SerializeField] GameObject visualSelected;
    [SerializeField] GameObject visualHovered;
    [SerializeField] GameObject visualExhausted;
    [SerializeField] GameObject visualGreyed;

    private ObjectPool objectPool;
    private LoopManager loopManager;

    private bool loopOnPause;
    private bool isExhausted;

    public bool IsExhausted { get { return isExhausted; } }
    public ChampionSO ChampionSO {  get { return championSO; } }

    private void Awake() {
        objectPool = GetComponentInParent<ObjectPool>();
    }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;

        visualSelected.SetActive(false);
        visualHovered.SetActive(false);
    }

    private void Update() {
        if (loopOnPause) {
            if (objectPool.SelectedChampionIconTemplateUI != this) {
                // The selected spawn point is another spawn point
                visualSelected.SetActive(false);
            }
        }
    }

    public void SetSelectedChampionIcon() {
        visualSelected.SetActive(true);
        objectPool.SetSelectedChampion(championSO.Champion);
        objectPool.SetSelectedChampionIconTemplateUI(this);
    }

    public void SetHoveredChampionIcon() {
        visualHovered.SetActive(true);
    }

    public void UnhoveredChampionIcon() {
        visualHovered.SetActive(false);
    }

    public void SetExhaustedChampionIcon() {
        visualExhausted.SetActive(true);
        isExhausted = true;
    }

    private void DeactivateVisuals() {
        visualHovered.SetActive(false);
        visualGreyed.SetActive(false);
        visualSelected.SetActive(false);
        visualExhausted.SetActive(false);
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            visualGreyed.SetActive(true);

            if (isExhausted) {
                visualExhausted.SetActive(true);
            }

            loopOnPause = true;
        } else {
            DeactivateVisuals();
        }
    }
}
