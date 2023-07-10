using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    [SerializeField] Player player;
    SpawnPoint[] spawnPoints;
    SpawnPoint selectedSpawnPoint;
    private GameObject[] pool;

    private InputManager inputManager;

    private float moveInput;
    private int spawnPointNumber = 3;
    private int selectedSpawnPointInt = 0;

    private bool inputPressed;
    private bool loopOnPause;
    public SpawnPoint SelectedSpawnPoint { get { return selectedSpawnPoint; } }

    private void Awake() { 
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
        inputManager.OnInteractHeld += InputManager_OnInteractHeld;

        InitializeSpawnPoints();
        PopulatePool();

        loopOnPause = true;
    }


    private void Update() {
        if (loopOnPause) {
            HandleSpawnPointSelection();
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

            foreach (SpawnPoint spawnPoint in spawnPoints) {
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

    private void InitializeSpawnPoints() {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        spawnPoints[0].SetHoveredSpawnPoint();
    }

    private void PopulatePool() {
        pool = new GameObject[spawnPointNumber];

        for (int i = 0; i < spawnPointNumber; i++) {
            pool[i] = Instantiate(champion, spawnPoints[i].transform.position, Quaternion.identity, spawnPoints[i].transform);
            pool[i].GetComponent<Champion>().SetObjectPoolParent(this);
            pool[i].SetActive(false);
        }
    }

    private void ResetPool() {
        // Reset champion positions to spawn points, reset direction, reset health, reset velocity to zero, remove flag children
        for (int i = 0; i < spawnPointNumber; i++) {
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
        for (int i = 0; i < spawnPointNumber; i++) {
            pool[i].GetComponent<IChampionAttack>().EnableAttacks();
            pool[i].GetComponent<ChampionMovement>().enabled = true;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = true;
            pool[i].GetComponent<Collider2D>().enabled = true;
        }
    }

    private void DeactivatePool() {
        // Deactivate all controller components on all champions
        for (int i = 0; i < spawnPointNumber; i++) {
            pool[i].GetComponent<IChampionAttack>().DisableAttacks();
            pool[i].GetComponent<ChampionMovement>().enabled = false;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = false;
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
            
            spawnPoints[selectedSpawnPointInt].SetHoveredSpawnPoint();
        }

        if (moveInput == -1 && !inputPressed) {
            inputPressed = true;
            spawnPoints[selectedSpawnPointInt].UnhoveredSpawnPoint();


            if (selectedSpawnPointInt == 0) {
                selectedSpawnPointInt = spawnPointNumber;
            }
            selectedSpawnPointInt--;

            spawnPoints[selectedSpawnPointInt].SetHoveredSpawnPoint();
        }

        if (moveInput == 0) {
            inputPressed = false;
        }
    }

    private void InputManager_OnInteractHeld(object sender, System.EventArgs e) {
        if (loopOnPause) {

        }
    }

    public void SetSelectedSpawnPoint(SpawnPoint spawnPoint) {
        selectedSpawnPoint = spawnPoint;
    }


    public Player GetPlayer() {
        return player;
    }
}
