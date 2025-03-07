using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SCR_PlayerInputHandler : MonoBehaviour {
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;


    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";


    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string swapWeapon = "SwapWeapon";
    [SerializeField] private string dash = "Dash";


    [Header("Weapon References")]
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject weapon3;

    //You're welcome :3 ~Dani
    public GameObject Weapon1 { get => weapon1; }
    public GameObject Weapon2 { get => weapon2; }
    public GameObject Weapon3 { get => weapon3; }


    InputAction _movementAction;
    InputAction _rotationAction;
    InputAction _jumpAction;
    InputAction _dashAction;
    InputAction _sprintAction;
    InputAction _swapWeaponAction;


    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool DashTriggered { get; private set; }



    private void Awake() {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);


        _movementAction = mapReference.FindAction(movement);
        _rotationAction = mapReference.FindAction(rotation);
        _jumpAction = mapReference.FindAction(jump);
        _dashAction = mapReference.FindAction(dash);
        _sprintAction = mapReference.FindAction(sprint);
        _swapWeaponAction = mapReference.FindAction(swapWeapon);

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
}