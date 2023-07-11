using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject[] champions;
    [SerializeField] Player player;

    SpawnPoint[] spawnPoints;
    SpawnPoint selectedSpawnPoint;

    ChampionIconTemplateUI[] championsIconTemplateUI;
    ChampionIconTemplateUI selectedChampionIconTemplateUI;
    Champion selectedChampion;

    private GameObject[] pool;

    private InputManager inputManager;

    private float moveInput;
    private int spawnPointNumber = 3;
    private int selectedSpawnPointInt = 0;

    private int championNumber = 4;
    private int selectedChampionInt = 0;

    private bool inputPressed;
    private bool loopOnPause;
    public SpawnPoint SelectedSpawnPoint { get { return selectedSpawnPoint; } }
    public ChampionIconTemplateUI SelectedChampionIconTemplateUI { get { return selectedChampionIconTemplateUI; } }

    private void Awake() { 
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
        inputManager.OnAttackPressed += InputManager_OnSelectPressed;
        inputManager.OnBackPressed += InputManager_OnBackPressed;

        InitializeSpawnPoints();
        PopulatePool();

        loopOnPause = true;
    }

    private void Update() {
        if (loopOnPause) {
            if (selectedSpawnPoint != null) {
                HandleChampionSelection(selectedSpawnPoint);
            } else {
                HandleSpawnPointSelection();
            }
        }
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

            selectedSpawnPoint = spawnPoints[selectedSpawnPointInt];
            SpawnSelectedChampion(selectedSpawnPoint, e.loopNumber);

            selectedChampion = null;
            selectedSpawnPoint = null;

        }
        if (e.state == LoopManager.State.Playbacking) {
            ActivatePool();
            ResetPool();
        }
        if (e.state == LoopManager.State.PlaybackEndBuffer || e.state == LoopManager.State.RecordingEndBuffer) {
            DeactivatePool();
        }
    }

    private void InitializeSpawnPoints() {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        spawnPoints[0].SetHoveredSpawnPoint();
    }

    private void PopulatePool() {
        pool = new GameObject[spawnPointNumber];

        for (int i = 0; i < spawnPointNumber; i++) {
            foreach (GameObject champion in champions) {
                pool[i] = Instantiate(champion, spawnPoints[i].transform.position, Quaternion.identity, spawnPoints[i].transform);
                pool[i].GetComponent<Champion>().SetObjectPoolParent(this);
                pool[i].SetActive(false);
            }
        }
    }

    private void ResetPool() {
        // Reset champion positions to spawn points, reset direction, reset health, reset velocity to zero, remove flag children
        for (int i = 0; i < spawnPointNumber; i++) {
            foreach(Champion champion in spawnPoints[i].GetComponentsInChildren<Champion>()) {

                champion.transform.position = spawnPoints[i].transform.position;
                champion.transform.localScale = Vector3.one;
                champion.GetComponent<Champion>().ResetChampionHealth();
                champion.GetComponent<Champion>().RemoveFlagChildren();
                champion.GetComponent<IChampionAttack>().ResetAttacks();
                champion.GetComponent<Animator>().Play("Idle");
                champion.GetComponent<ChampionMovement>().SetVelocity(Vector3.zero);
            }
        }
    }

    private void ActivatePool() {
        // Activate all components on all champions
        for (int i = 0; i < spawnPointNumber; i++) {
            foreach (Champion champion in spawnPoints[i].GetComponentsInChildren<Champion>()) {
                champion.GetComponent<IChampionAttack>().EnableAttacks();
                champion.GetComponent<ChampionMovement>().enabled = true;
                champion.GetComponent<ChampionRecPlaybackManager>().enabled = true;
                champion.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    private void DeactivatePool() {
        // Deactivate all controller components on all champions
        for (int i = 0; i < spawnPointNumber; i++) {
            foreach (Champion champion in spawnPoints[i].GetComponentsInChildren<Champion>()) {
                champion.GetComponent<IChampionAttack>().DisableAttacks();
                champion.GetComponent<ChampionMovement>().enabled = false;
                champion.GetComponent<ChampionRecPlaybackManager>().enabled = false;
            }
        }
    }

    private void HandleSpawnPointSelection() {
        moveInput = inputManager.GetMoveInput();

        if (moveInput == 1 && !inputPressed) {
            inputPressed = true;
            spawnPoints[selectedSpawnPointInt].UnhoveredSpawnPoint();
            selectedSpawnPointInt++;

            if (selectedSpawnPointInt == spawnPointNumber) {
                selectedSpawnPointInt = 0;
            }
        }

        if (moveInput == -1 && !inputPressed) {
            inputPressed = true;
            spawnPoints[selectedSpawnPointInt].UnhoveredSpawnPoint();


            if (selectedSpawnPointInt == 0) {
                selectedSpawnPointInt = spawnPointNumber;
            }
            selectedSpawnPointInt--;
        }

        spawnPoints[selectedSpawnPointInt].SetHoveredSpawnPoint();

        if (moveInput == 0) {
            inputPressed = false;
        }
    }

    private void HandleChampionSelection(SpawnPoint spawnpoint) {
        championsIconTemplateUI = spawnpoint.GetComponentInChildren<ChampionSelectionUI>().GetChampionIconTemplateUIArray();
        moveInput = inputManager.GetMoveInput();

        if (moveInput == 1 && !inputPressed) {
            inputPressed = true;
            championsIconTemplateUI[selectedChampionInt].UnhoveredChampionIcon();
            selectedChampionInt++;

            if (selectedChampionInt == championNumber) {
                selectedChampionInt = 0;
            }
        }

        if (moveInput == -1 && !inputPressed) {
            inputPressed = true;
            championsIconTemplateUI[selectedChampionInt].UnhoveredChampionIcon();

            if (selectedChampionInt == 0) {
                selectedChampionInt = championNumber;
            }
            selectedChampionInt--;
        }
        championsIconTemplateUI[selectedChampionInt].SetHoveredChampionIcon();

        if (moveInput == 0) {
            inputPressed = false;
        }
    }

    private void SpawnSelectedChampion(SpawnPoint selectedSpawnPoint, int loopNumber) {
        foreach (SpawnPoint spawnPoint in spawnPoints) {
            if (spawnPoint == selectedSpawnPoint) {

                foreach (Champion champion in spawnPoint.GetComponentsInChildren<Champion>(true)) {
                    if (champion.ChampionSO == selectedChampion.ChampionSO) {
                        champion.gameObject.SetActive(true);
                        champion.GetComponent<Champion>().SetSpawnedLoopNumber(loopNumber);
                    }
                }
            }
        }
    }

    private void InputManager_OnSelectPressed(object sender, System.EventArgs e) {
        if (selectedSpawnPoint != null && selectedChampion == null) {
            championsIconTemplateUI[selectedChampionInt].SetSelectedChampionIcon();
        }

        if (selectedSpawnPoint == null) {
            spawnPoints[selectedSpawnPointInt].SetSelectedSpawnPoint();
        }
    }

    private void InputManager_OnBackPressed(object sender, System.EventArgs e) {
        if (selectedSpawnPoint != null && selectedChampion == null) {
            selectedSpawnPoint = null;
        }
        if (selectedChampion != null) {
            selectedChampion = null;
        }
     }

    public void SetSelectedSpawnPoint(SpawnPoint spawnPoint) {
        selectedSpawnPoint = spawnPoint;
    }

    public void SetSelectedChampionIconTemplateUI(ChampionIconTemplateUI championIconTemplateUI) {
        selectedChampionIconTemplateUI = championIconTemplateUI;
    }

    public void SetSelectedChampion(Champion champion) {
        selectedChampion = champion;
    }

    public Player GetPlayer() {
        return player;
    }
}
