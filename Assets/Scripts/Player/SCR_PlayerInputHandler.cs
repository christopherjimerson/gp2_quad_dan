using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SCR_PlayerInputHandler : MonoBehaviour {
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;


    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    public event System.Action OnInteract;

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string swapWeapon = "SwapWeapon";
    [SerializeField] private string dash = "Dash";
    [SerializeField] private string shoot = "Shoot";
    [SerializeField] private string reload = "Reload";
    [SerializeField] private string interact = "Interact";


    [Header("Weapon References")]
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject weapon3;

    //You're welcome :3 ~Dani
    public GameObject Weapon1 { get => weapon1; }
    public GameObject Weapon2 { get => weapon2; }
    public GameObject Weapon3 { get => weapon3; }

    private Coroutine _autoFireCoroutine;


    InputAction _movementAction;
    InputAction _rotationAction;
    InputAction _jumpAction;
    InputAction _dashAction;
    InputAction _sprintAction;
    InputAction _swapWeaponAction;
    InputAction _shootAction;
    InputAction _reloadAction;
    InputAction _interactAction;


    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool DashTriggered { get; private set; }
    public bool ShotTriggered { get; private set; }
    public bool InteractTriggered { get; private set; }




    private void Awake() {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);


        _movementAction = mapReference.FindAction(movement);
        _rotationAction = mapReference.FindAction(rotation);
        _jumpAction = mapReference.FindAction(jump);
        _dashAction = mapReference.FindAction(dash);
        _sprintAction = mapReference.FindAction(sprint);
        _swapWeaponAction = mapReference.FindAction(swapWeapon);
        _shootAction = mapReference.FindAction(shoot);
        _reloadAction = mapReference.FindAction(reload);
        _interactAction = mapReference.FindAction(interact);

        SubscribeActionValuesToInputEvents();

        //Enables and disables important components


        weapon1.SetActive(false);
        weapon2.SetActive(false);
        weapon3.SetActive(false);
    }

    private void OnDestroy() {

    }

    private void SubscribeActionValuesToInputEvents() {
        _movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        _movementAction.canceled += inputInfo => MovementInput = Vector2.zero;


        _rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        _rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;


        _jumpAction.performed += inputInfo => JumpTriggered = true;
        _jumpAction.canceled += inputInfo => JumpTriggered = false;

        _dashAction.performed += inputInfo => DashTriggered = true;
        _dashAction.canceled += inputInfo => DashTriggered = false;


        _sprintAction.performed += inputInfo => SprintTriggered = true;
        _sprintAction.canceled += inputInfo => SprintTriggered = false;

        _interactAction.performed += inputInfo => { InteractTriggered = true; OnInteract?.Invoke();};
        _interactAction.canceled += inputInfo => InteractTriggered = false;

        _shootAction.started += inputInfo => FireActiveWeapon();
        _shootAction.canceled += inputInfo => StopFiring();

        _reloadAction.performed += inputInfo => ReloadActiveWeapon();

        _swapWeaponAction.performed += inputInfo => SwapWeapons();
    }

    private void SwapWeapons() {
        if (weapon1.activeSelf)
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
            weapon3.SetActive(false);
        }
        else if (weapon2.activeSelf)
        {
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            weapon3.SetActive(true);
        }
        else if (weapon3.activeSelf)
        {
            weapon1.SetActive(true);
            weapon2.SetActive(false);
            weapon3.SetActive(false);
        }
    }

    private void OnEnable() {
        playerControls.FindActionMap(actionMapName).Enable();

    }


    private void OnDisable() {
        playerControls.FindActionMap(actionMapName).Disable();
    }

    private void FireActiveWeapon() {
        if (weapon1.activeSelf) {
            weapon1.GetComponent<SCR_Shoot_Hitscan>().FireWeapon();
        }else if (weapon2.activeSelf) {
            _autoFireCoroutine = StartCoroutine(AutoFireCoroutine());
        }
        else if (weapon3.activeSelf) {
            weapon3.GetComponent<SCR_Shotgun>().FireWeapon();
        }
    }

    private void StopFiring() {
        if (_autoFireCoroutine != null)
        {
            StopCoroutine(_autoFireCoroutine);
            _autoFireCoroutine = null;
            weapon2.GetComponent<SCR_AssaultRifle>().StopMuzzleFlash();
        }
    }

    private IEnumerator AutoFireCoroutine() {
        SCR_AssaultRifle rifle = weapon2.GetComponent<SCR_AssaultRifle>();
        while (true)
        {
            rifle.FireWeapon();
            yield return new WaitForSeconds(0.1f); // Adjust this based on weapon's fire rate
        }
    }

    private void ReloadActiveWeapon() {
        if (weapon1.activeSelf) {
            weapon1.GetComponent<SCR_Shoot_Hitscan>().ReloadWeapon();
        }
    }
}