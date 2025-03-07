using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_GameController : MonoBehaviour
{
    public static SCR_GameController Instance;

    public GameObject hud, buffUI, NewGameCutscene, DeathScreen;

    public GameObject PlayerPrefab, CurrentPlayer;
    public GameObject spawnRoom;

    public event Action<SCR_SO_Ram> OnEquipRam, OnUnEquipRam;

    public List<SCR_SO_Ram> CurrentEquippedRam, StartingRam, BackPackRam, AllRamNotEquipped, AllRam;
    public List<SCR_SO_Buff> AvalibleBuffs, PlayerBuffs;
    public List<SCR_SO_Ram> EquippedInInsanity = new List<SCR_SO_Ram>();

    public SCR_SO_Ram latestRam;

    public GameObject UpgradeUI,LoadPlayerScreen; 

    public int currentFloor = 0, clearedRooms = 0;

    public List<SCR_SO_Ram> DemoEquippedRam, DemoBackPackRam;
    private int demoCurrentFloor = 0;

    [SerializeField] private bool playerHasWeapon = false, playerHasRam = false;

    [SerializeField]
    SCR_InsanityController insanityController;
    public WeaponDataStorage weaponDataStorage;

    public GameObject PauseMenuUI;


    public SCR_InsanityController InsanityController
    {
        get => insanityController;
        
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    private void OnEnable()
    {
        EventManager.Instance.OnFloorCompleted += ToggleBuffUI;
        EventManager.Instance.OnPlayerDeath += PlayerDeath;

    }

    private void OnDisable()
    {
        EventManager.Instance.OnFloorCompleted -= ToggleBuffUI;
        EventManager.Instance.OnPlayerDeath -= PlayerDeath;
        EventManager.Instance.OnSpawnRoomSpawned -= LockDoorsFirstPlaythrough;

    }

    private void Start()
    {
        switch (GameManager.Instance.LoadState)
        {
            case LoadState.CONTINUE:
            {
                CurrentEquippedRam.Clear();
                BackPackRam.Clear();
                    if(SaveManager.GetEquippedRam() != null)
                        if(SaveManager.GetEquippedRam().Length > 0)
                            foreach (var ram in from ram in AllRam from i in SaveManager.GetEquippedRam() where ram.Id == i select ram)
                            {
                                CurrentEquippedRam.Add(ram);
                            }
                    if (SaveManager.GetBackPackRam() != null)
                        if (SaveManager.GetBackPackRam().Length > 0)
                            foreach (var ram in from ram in AllRam from i in SaveManager.GetBackPackRam() where ram.Id == i select ram)
                            {
                                BackPackRam.Add(ram);
                            }

                currentFloor = SaveManager.GetFloorNumber();

                NewGameCutscene.SetActive(false);
                    break;
            }
            case LoadState.DEMO:
            {
                if (DemoEquippedRam.Any())
                {
                    CurrentEquippedRam.Clear();
                    CurrentEquippedRam = DemoEquippedRam;
                }

                if (DemoBackPackRam.Any())
                {
                    BackPackRam.Clear();
                    BackPackRam = DemoBackPackRam;
                }
                if(demoCurrentFloor >= 1)
                    currentFloor = demoCurrentFloor-1;
                else
                    currentFloor = 0;

                NewGameCutscene.SetActive(false);
                    break;
            }
            case LoadState.NEW:
                NewGameCutscene.SetActive(true);
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        EventManager.Instance.OnSpawnRoomSpawned += LockDoorsFirstPlaythrough;

        ProgressLevel();

        //Turn off main menu music
        AudioManager.Instance.Stop("Music_Static");
        AudioManager.Instance.Play("Music_Main");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("O was pressed");
            EventManager.Instance.FloorCompleted();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SaveManager.SaveGame();
        }

    }

    public void SetLatestRam(SCR_SO_Ram ram)
    {
        latestRam = ram;
    }

    public void RemoveLatestRam()
    {
        latestRam = null;
    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }


    public void CheckIfFloorClear()
    {
        clearedRooms++;

        if (clearedRooms >= SCR_LevelGenerator.Instance.CurrentSpawnedRoomsInFloor()-1)
        {
            EventManager.Instance.FloorCompleted();
        }
    }

    public void TogglePauseMenuUI(bool enabled, int time)
    {
        if (!PauseMenuUI) return;
        PauseMenuUI.SetActive(enabled);
        Time.timeScale = time;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToggleUpgradeUI(bool enabled)
    {
        if (!UpgradeUI) return;
            UpgradeUI.SetActive(enabled);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }

    public void ToggleBuffUI() {
        if (!buffUI) return;

        buffUI.SetActive(true);

        if (CurrentPlayer)
        {
            Destroy(CurrentPlayer);
            CurrentPlayer = null;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    

    public void CreateLevel()
    {
        SCR_LevelGenerator.Instance.GenerateLevel();
    }

    public void ProgressLevel()
    {
        if (CurrentPlayer)
        {
            Destroy(CurrentPlayer);
            CurrentPlayer = null;
        }

        SCR_LevelGenerator.Instance.RoomsToCreate++;
        SCR_LevelGenerator.Instance.DestroyLevel();
        SCR_LevelGenerator.Instance.GenerateLevel();
        currentFloor++;
        clearedRooms = 0;



        if(GameManager.Instance.LoadState != LoadState.NEW || currentFloor > 1) 
            LoadPlayerScreen.SetActive(true);

        //    StartCoroutine(SlowSpawnPlayer());
    }

    public void PlayerDeath()
    {
        if (CurrentPlayer)
        {
            Destroy(CurrentPlayer);
            CurrentPlayer = null;
        }

        DeathScreen.SetActive(true);
    }

    IEnumerator LockDoorsInSpawn()
    {
        yield return null;

        foreach (var door in spawnRoom.GetComponentsInChildren<SCR_EnemyRoomSpawner>()[0].lockedDoorsList)
        {
            door.SetActive(true);
        }

        Debug.Log("FINISHED RUNNING");
    }

    public void LockDoorsFirstPlaythrough()
    {
        Debug.Log("RUNNING");

        playerHasWeapon = false;

        if(currentFloor == 0)
            playerHasRam = false;

        StartCoroutine(LockDoorsInSpawn());

        

        
    }

    public void RespawnLevelAndPlayer()
    {
        SCR_LevelGenerator.Instance.DestroyLevel();
        SCR_LevelGenerator.Instance.GenerateLevel();
        clearedRooms = 0;
        currentFloor = 1;
        Debug.Log("Current Floor" + currentFloor);
        weaponDataStorage.weaponSelected = false;
        weaponDataStorage.pistolSelected = false;
        weaponDataStorage.shotgunSelected = false;
        weaponDataStorage.rifleSelected = false;

        LoadPlayerScreen.SetActive(true);

        //StartCoroutine(SlowSpawnPlayer());
    }

    public IEnumerator SlowSpawnPlayer() {
        yield return new WaitForSeconds(0.2f);
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        var spawnPoint = GameObject.FindGameObjectWithTag("Spawn");


        if (!spawnPoint)
        {
            Debug.LogWarning("Spawn not found, Aborting player spawning");
            SCR_LevelGenerator.Instance.DestroyLevel();
            SCR_LevelGenerator.Instance.GenerateLevel();
            StartCoroutine(SlowSpawnPlayer());
            return;
        }
        AudioManager.Instance.Play("Music_Main");
        AudioManager.Instance.Stop("CRT_Static");
        CurrentPlayer = Instantiate(PlayerPrefab, spawnPoint.transform.position, Quaternion.identity);
        if (currentFloor > 1)
        {
            Debug.Log(currentFloor);
            weaponDataStorage.EquipStorageWeapon();
            PlayerChoseWeapon();
            PlayerHasRam();
            AllowPlayerToLeave();
            AddAllEquippedRamToPlayer();
        }
        if (currentFloor == 1)
        {
            playerHasRam = false;
        }


        SaveManager.SaveGame();
    }


    /// <summary>
    /// Provides the caller with a ram upgrade from the not equipped ram list
    /// </summary>
    /// <returns>A random ram upgrade</returns>
    public SCR_SO_Ram GetRandomNotEquippedRam()
    {
        return AllRamNotEquipped[UnityEngine.Random.Range(0, AllRamNotEquipped.Count - 1)];
    }

    private float InsanityCost = 0;

    public void AddAllEquippedRamToPlayer()
    {
        foreach (var ram in CurrentEquippedRam)
        {
            //if (EquippedInInsanity.Contains(ram)) {  }

            EquipRam(ram);
        }

        foreach (var ram in BackPackRam)
        {
            //if (EquippedInInsanity.Contains(ram)) {  }

            UnEquipRam(ram);

        }

        SCR_InsanityController.Instance._usedRam = 0f;
        foreach (var scrSoRam in CurrentEquippedRam)
        {
            SCR_InsanityController.Instance._usedRam += scrSoRam.cost;
        }
    }

    /// <summary>
    /// Gives you a random buff
    /// </summary>
    /// <returns>Buff type. ex: speed, damage, hp</returns>
    public SCR_SO_Buff GetRandomBuff() {
        var buff = AvalibleBuffs[UnityEngine.Random.Range(0, AvalibleBuffs.Count)];
        buff.RandomizeQuality();
        return buff;
    }


    /// <summary>
    /// Adds a ram from the allRamNotEquipped list to the players backpack
    /// </summary>
    /// <param name="upgrade"></param>
    public void AddRamToBackpack(SCR_SO_Ram upgrade)
    {
        if(!AllRamNotEquipped.Contains(upgrade)) return;

        AllRamNotEquipped.Remove(upgrade);
        BackPackRam.Add(upgrade);
    }

    /// <summary>
    /// Equip a ram from the allRamNotEquipped list
    /// </summary>
    /// <param name="upgrade"></param>
    public void EquipNonPlayerRam(SCR_SO_Ram upgrade)
    {
        if (!AllRamNotEquipped.Contains(upgrade)) return;

        AllRamNotEquipped.Remove(upgrade);
        CurrentEquippedRam.Add(upgrade);
    }
    /// <summary>
    /// Takes an upgrade that the player has equipped and places it in their backpack
    /// </summary>
    /// <param name="upgrade"></param>
    public void UnequipEquippedRam(SCR_SO_Ram upgrade)
    {
        if (!CurrentEquippedRam.Contains(upgrade)) return;

        CurrentEquippedRam.Remove(upgrade);
        BackPackRam.Add(upgrade);
    }



    public void EquipRam(SCR_SO_Ram ram) {
        if (!ram) return;
        //CurrentEquippedRam.Add(ram);
        if (!EquippedInInsanity.Contains(ram))
        {
            EquippedInInsanity.Add(ram);
        }

        ;
        ram.OnEquipRam();
        OnEquipRam?.Invoke(ram);
    }

    public void UnEquipRam(SCR_SO_Ram ram) {
        //CurrentEquippedRam.Remove(ram);
        EquippedInInsanity.Remove(ram);
        ram.OnUnEquipRam();
        OnUnEquipRam?.Invoke(ram);
    }
    public void AddBuff(SCR_SO_Buff buff) {
        var boof = buff;
        PlayerBuffs.Add(boof);
    }

    public void PlayerChoseWeapon()
    {
        playerHasWeapon = true;
        AllowPlayerToLeave();
    }

    public void PlayerHasRam()
    {
        playerHasRam = true;
        AllowPlayerToLeave();
    }

    public void AllowPlayerToLeave()
    {
        if(playerHasRam && playerHasWeapon)
            foreach (var door in spawnRoom.GetComponentsInChildren<SCR_EnemyRoomSpawner>()[0].lockedDoorsList)
            {
                door.SetActive(false);
            }
    }
}
