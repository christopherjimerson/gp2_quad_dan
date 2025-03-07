using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;


public class SCR_FirstPersonController : MonoBehaviour {
    [Header("Movement Parameters")]
    [SerializeField] public float walkSpeed = 3.0f;
    [SerializeField] private float stepInterval = 0.5f;
    private float stepTimer = 0f;

    [Header("Sprint Parameters")]
    [SerializeField] private float sprintMultiplier = 2.0f;
    [SerializeField] private float maxStamina = 25f;
    [SerializeField] private float staminaDepletionRate = 5f;
    [SerializeField] private float staminaRegenRate = 3f;
    [SerializeField] private float staminaDrainResetValue = .75f;
    [SerializeField] public bool sprintSOEquipped = false;
    private float currentStamina;
    private bool canSprint;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.00f;
    [SerializeField] private float gravityMultiplier = 1.0f;
    [SerializeField] public bool jumpSOEquipped = false;


    [Header("Double Jump Parameters")]
    [SerializeField] private bool doubleJumpEnabled = false;
    [SerializeField] private float doubleJumpForce = 5.0f;
    [SerializeField] private float doubleJumpGravityMultiplier = 1.0f;
    public bool _canDoubleJump = false;

    [Header("Dash Parameters")]
    [SerializeField] private float dashDistance = 10.0f;
    [SerializeField] private float dashSpeed = 50.0f;
    [SerializeField] private float dashCooldown = 2.0f;
    [SerializeField] public bool dashSOEquipped = false;
    [SerializeField] private ParticleSystem dashLines;


    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    private enum PowerUps
    {
        Shield,
        Virus,
        Repel,
        Stun,

    }

    [Header("Shield Parameters")]
    [SerializeField] public bool shieldEquipped = false;
    [SerializeField] private float shieldPowerUpLength;
    [SerializeField] private float shieldCoolDown;
    [SerializeField] private GameObject shieldPrefab;
    public bool _isShieldActive;
    private bool _shieldOnCoolDown;
    private bool _shieldSelected;

    [Header("Virus Parameters")]
    [SerializeField] public bool virusEquipped = false;
    [SerializeField] public float virusDamage;
    [SerializeField] public float virusCoolDown;
    private bool _virusOnCoolDown;
    private bool _virusSelected;

    [Header("Stun Parameters")]
    [SerializeField] public bool stunEquipped = false;
    [SerializeField] private float stunLength;
    [SerializeField] private float stunCoolDown;
    private bool _stunOnCoolDown;
    private bool _stunSelected;


    [Header("Repel Settings")]
    [SerializeField] public bool repelEquipped = false;
    [SerializeField] public float repelRadius = 10f;
    [SerializeField] public float repelForce = 10f;
    [SerializeField] private int repelDamage;
    [SerializeField] private float repelCoolDown;
    private bool _repelOnCoolDown;
    private bool _repelSelected;

    private PowerUps currentPower;
    
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera weaponsCamOverlay;
    [SerializeField] private Camera HUDCamOverlay;
    [SerializeField] private SCR_PlayerInputHandler playerInputHandler;
    [SerializeField] private SCR_HeadsUpDisplay hud;

    public bool isUIActive => SCR_GameController.Instance.UpgradeUI.activeInHierarchy;
    private bool isMovementDisabled => isUIActive;
    Vector3 _currentMovement;
    float _verticalRotation;
    bool _isDashing = false;
    bool _dashOnCooldown = false;
    private bool _isSprinting => playerInputHandler.SprintTriggered && canSprint && sprintSOEquipped;
    private float CurrentSpeed => _isSprinting ? walkSpeed * sprintMultiplier : walkSpeed;

    public float DamageMultiplier = 0;
    float tick = 0.5f;
    float totalTick = 10f;

    private Canvas _HUDCanvas;
    private Canvas _modRamUI;
    private Canvas _buffUI;

    public bool isPaused;
    
    void OnEnable()
    {
        AEnemy.Targets.Add(gameObject);
        mainCamera.enabled = true;
    }

    void OnDisable() {  
        AEnemy.Targets.Remove(gameObject);
        mainCamera.enabled = false;
    }


    void Start()
    {
        isPaused = false;
        SCR_ModRamUI ramUI = FindFirstObjectByType<SCR_ModRamUI>(FindObjectsInactive.Include);
        _modRamUI = ramUI.modRamUI;
        _modRamUI.renderMode = RenderMode.ScreenSpaceCamera;
        _modRamUI.worldCamera = HUDCamOverlay;
        _modRamUI.sortingOrder = 20;
        _modRamUI.planeDistance = 0.2f;
        /*
        _buffUI = FindFirstObjectByType<SCR_BuffUI>(FindObjectsInactive.Include).GetComponent<Canvas>();
        _buffUI.renderMode = RenderMode.ScreenSpaceCamera;
        _buffUI.worldCamera = HUDCamOverlay;
        _buffUI.sortingOrder = 22;
        _buffUI.planeDistance= 0.2f;
        */
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina = maxStamina;
        if (!hud) { hud = SCR_HeadsUpDisplay.Instance; }

        _HUDCanvas = hud.GetComponent<Canvas>();
        _HUDCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _HUDCanvas.worldCamera = HUDCamOverlay;
        _HUDCanvas.sortingOrder = 10;
        _HUDCanvas.planeDistance = 0.4f;
        currentPower = PowerUps.Shield;

        var gameController = SCR_GameController.Instance;
        
        for (int i = 0; i < gameController.StartingRam.Count; i++)
        {
            gameController.EquipRam(gameController.StartingRam[i]);
        }

        if (gameController.weaponDataStorage.weaponSelected)
        {
            SCR_WeaponVisibility.Instance.DisableWeaponSelect();
        }
        else if(!gameController.weaponDataStorage.weaponSelected)
        {
            SCR_WeaponVisibility.Instance.EnableWeaponSelect();
        }
        LoopBuffList();
        if (shieldEquipped) { hud.shieldIcon.SetActive(true); hud.shieldNotEquipped.gameObject.SetActive(false); }
        if (stunEquipped) { hud.stunIcon.SetActive(true); hud.stunNotEquipped.gameObject.SetActive(false); }
        if (repelEquipped) { hud.repelIcon.SetActive(true); hud.repelNotEquipped.gameObject.SetActive(false); }
        if (virusEquipped) { hud.virusIcon.SetActive(true); hud.virusNotEquipped.gameObject.SetActive(false); }

    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            Debug.Log("Should pause");
            SCR_GameController.Instance.TogglePauseMenuUI(true, 0);
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            Debug.Log("Should unpause");
            SCR_GameController.Instance.TogglePauseMenuUI(false, 1);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
        }

        if (!isMovementDisabled)
        {
            if (!_isDashing)
            {
                Movement();
                Rotation();

                if (playerInputHandler.DashTriggered && !_dashOnCooldown && dashSOEquipped)
                {
                    StartCoroutine(PerformDash());
                }
            }

            HandleStamina();
        }
        else
        {
            return;
        }

        

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            CyclePower(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            CyclePower(-1);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ActivatePower();
        }


    }

    


    private Vector3 CalculateWorldDirection() {
        Vector3 _inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 _worldDirection = transform.TransformDirection(_inputDirection);
        return _worldDirection.normalized;
    }


    private void HandleJumping() {


        if (characterController.isGrounded && jumpSOEquipped)
        {
            
            _canDoubleJump = doubleJumpEnabled;
            _currentMovement.y = -0.5f;

            if (playerInputHandler.JumpTriggered)
            {
                AudioManager.Instance.Play("Player_Jump");
                _currentMovement.y = jumpForce;
            }
        }
        else
        {
            _currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;

            if (_canDoubleJump && playerInputHandler.JumpTriggered)
            {
                _currentMovement.y = doubleJumpForce;
                _canDoubleJump = false;
            }
        }
    }


    private void Movement() {
        _currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        Vector3 _worldDirection = CalculateWorldDirection();
        if (characterController.isGrounded || _worldDirection != Vector3.zero)
        {
            _currentMovement.x = _worldDirection.x * CurrentSpeed;
            _currentMovement.z = _worldDirection.z * CurrentSpeed;
            if (_worldDirection.magnitude > 0.1f)
            {
                stepTimer += Time.deltaTime;
                if (stepTimer >= stepInterval)
                {
                    AudioManager.Instance.Play("Player_Footsteps");
                    stepTimer = 0f;
                }
            }
        }


        HandleJumping();
        characterController.Move(_currentMovement * Time.deltaTime);
    }


    private void HorizontalRotate(float rotationAmount) {
        transform.Rotate(0, rotationAmount, 0);
    }


    private void VerticalRotate(float rotationAmount) {
        _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }


    private void Rotation() {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;


        HorizontalRotate(mouseXRotation);
        VerticalRotate(mouseYRotation);
    }

    private void HandleStamina() {
        if (_isSprinting && currentStamina > 0)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            if (currentStamina <= 0) canSprint = false;
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina >= maxStamina * staminaDrainResetValue) canSprint = true;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        hud.UpdateStaminaBar(currentStamina / maxStamina);
    }


    private IEnumerator PerformDash() {
        dashLines.gameObject.SetActive(true);
        _isDashing = true;
        _dashOnCooldown = true;
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0, playerInputHandler.MovementInput.y);
        Vector3 dashDirection = inputDirection != Vector3.zero
            ? transform.TransformDirection(inputDirection.normalized)
            : transform.forward; // Default to forward if there isn't a direciton pressed
        float dashTravelled = 0f;
        while (dashTravelled < dashDistance)
        {
            AudioManager.Instance.Play("Player_Dash");
            float dashStep = dashSpeed * Time.deltaTime;
            dashTravelled += dashStep;
            characterController.Move(dashDirection * dashStep);
            if (characterController.collisionFlags == CollisionFlags.CollidedSides)
            {
                break;
            }

            yield return null;
        }

        _isDashing = false;
        dashLines.gameObject.SetActive(false);

        yield return new WaitForSeconds(dashCooldown);
        _dashOnCooldown = false;
    }

    private Coroutine ImageRoutine = null;
    void CyclePower(int direction) {
        int powerCount = System.Enum.GetValues(typeof(PowerUps)).Length;
        int newPowerIndex = ((int)currentPower + direction + powerCount) % powerCount;
        currentPower = (PowerUps)newPowerIndex;

        if(ImageRoutine != null) StopCoroutine(ImageRoutine); 
        ImageRoutine = StartCoroutine(DisplayImage());

        Debug.Log("Current Power: " + currentPower);
    }

    IEnumerator DisplayImage()
    {
        TurnOffPowerUpImages();

        switch (currentPower)
        {
            case PowerUps.Shield:
                hud.shieldArea.gameObject.SetActive(true);
                hud.shieldArea.color = Color.cyan;
                break;
            case PowerUps.Virus:
                hud.virusArea.gameObject.SetActive(true);
                hud.virusArea.color = Color.cyan;
                break;
            case PowerUps.Repel:
                hud.repelArea.gameObject.SetActive(true);
                hud.repelArea.color = Color.cyan;
                break;
            case PowerUps.Stun:
                hud.stunArea.gameObject.SetActive(true);
                hud.stunArea.color = Color.cyan;
                break;
        }

        yield return new WaitForSeconds(2f);

        TurnOffPowerUpImages();
    }

    private void TurnOffPowerUpImages()
    {
        hud.shieldArea.gameObject.SetActive(false);
        hud.stunArea.gameObject.SetActive(false);
        hud.repelArea.gameObject.SetActive(false);
        hud.virusArea.gameObject.SetActive(false);

        hud.shieldArea.color = Color.white;
        hud.stunArea.color = Color.white;
        hud.repelArea.color = Color.white;
        hud.virusArea.color = Color.white;
    }



    private void Shield() {
        hud.shieldEffect.gameObject.SetActive(true);
        _isShieldActive = true;
        StartCoroutine(StartShield(shieldPowerUpLength));
    }

    private IEnumerator StartShield(float powerUpLength) {
        yield return new WaitForSeconds(powerUpLength);
        hud.shieldEffect.gameObject.SetActive(false);
        _isShieldActive = false;
        StartCoroutine(ShieldCoolDown());
    }

    private void Stun()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
            if (hit.collider.TryGetComponent<SCR_Enemy>(out SCR_Enemy enemy))
            {
                enemy.Stunned(5f);
                StartCoroutine(StunCoolDown());
            }

    }

    public void RepelEnemies() {
        Collider[] enemies = Physics.OverlapSphere(transform.position, repelRadius);
        foreach (Collider col in enemies)
        {
            if (col.TryGetComponent<SCR_Enemy>(out SCR_Enemy enemy))
            {
                Debug.Log("Found Enemy Collider");
                enemy.TakeDamage(repelDamage);
                if (enemy.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
                {
                    Debug.Log("Found Enemy Nav Agent");
                    Vector3 pushDirection = (enemy.transform.position - transform.position).normalized;
                    Vector3 targetPosition = enemy.transform.position + pushDirection * repelForce;

                    if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, repelForce, NavMesh.AllAreas))
                    {
                        Debug.Log("This works");
                        enemy.transform.position = hit.position;
                        Debug.Log(agent.Warp(hit.position));
                        agent.Warp(hit.position);
                    }
                }
            }
        }
        StartCoroutine(RepelCoolDown());
    }

    void PlayerSpawnEquipment() {

    }

    private void VirusPower()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
        {
            if (hit.collider.GetComponent<SCR_Enemy>().canBeInfect == true)
            {
                hit.collider.GetComponent<SCR_Enemy>().infectDamage = 2;
                hit.collider.GetComponent<SCR_Enemy>().isInfected = true;
                hit.collider.GetComponent<SCR_Enemy>().InfectionDamage();
                Debug.Log(hit.collider.GetComponent<SCR_Enemy>().infectDamage);
            }
        }

        StartCoroutine(VirusCoolDown());
    }

    private IEnumerator TestCode() {
        float runFor = totalTick;
        if (totalTick >= 0)
        {
            Debug.Log(totalTick);
            totalTick -= tick;
            yield return new WaitForSeconds(tick);
            StartCoroutine(TestCode());
        }
        else
        {
            totalTick = 15;
            Debug.Log("Total Tick:" + totalTick);
        }

    }

    private IEnumerator ReenableNavMesh(NavMeshAgent agent, float delay) {
        yield return new WaitForSeconds(delay);
        if (agent != null)
        {
            agent.enabled = true;
        }
    }

    void ActivatePower() {
        switch (currentPower)
        {
            case PowerUps.Shield:
                if (!_shieldOnCoolDown && shieldEquipped) Shield();
                break;
            case PowerUps.Stun:
                if (!_stunOnCoolDown && stunEquipped) Stun();
                break;
            case PowerUps.Virus:
                if (!_virusOnCoolDown && virusEquipped) VirusPower();
                break;
            case PowerUps.Repel:
                if (!_repelOnCoolDown && repelEquipped) RepelEnemies();
                break;
        }
    }

    void LoopBuffList() {
        foreach (var buff in SCR_GameController.Instance.PlayerBuffs)
        {
            switch (buff.Buff)
            {
                case BuffType.SPD:
                    walkSpeed += (walkSpeed * (buff.GetPercentValue() / 100f));
                    break;
                case BuffType.HP:
                    GetComponent<PlayerHealth>().currentHealth += (GetComponent<PlayerHealth>().currentHealth * (buff.GetPercentValue() / 100));
                    GetComponent<PlayerHealth>().maxHealth += (GetComponent<PlayerHealth>().maxHealth * buff.GetPercentValue() / 100);
                    break;
                case BuffType.DMG:
                    DamageMultiplier += buff.GetPercentValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private IEnumerator ShieldCoolDown()
    {
        _shieldOnCoolDown = true;
        float elapsedTime = 0f;
        while (elapsedTime < shieldCoolDown)
        {
            hud.shieldCooldown.gameObject.SetActive(true);
            hud.shieldCooldown.fillAmount =  (1.0f - (elapsedTime / shieldCoolDown));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hud.shieldCooldown.gameObject.SetActive(false);
        _shieldOnCoolDown = false;
    }

    private IEnumerator StunCoolDown() {
        _stunOnCoolDown = true;
        float elapsedTime = 0f;
        while (elapsedTime < stunCoolDown)
        {
            hud.stunCooldown.gameObject.SetActive(true);
            hud.stunCooldown.fillAmount = (1.0f - (elapsedTime  / stunCoolDown));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hud.stunCooldown.gameObject.SetActive(false);
        _stunOnCoolDown = false;
    }

    private IEnumerator RepelCoolDown() {
        _repelOnCoolDown = true;
        float elapsedTime = 0f;
        while (elapsedTime < repelCoolDown)
        {
            hud.repelCooldown.gameObject.SetActive(true);
            hud.repelCooldown.fillAmount = (1.0f - (elapsedTime / repelCoolDown));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hud.repelCooldown.gameObject.SetActive(false);
        _repelOnCoolDown = false;
    }

    private IEnumerator VirusCoolDown() {
        _virusOnCoolDown = true;
        float elapsedTime = 0f;
        while (elapsedTime < virusCoolDown)
        {
            hud.virusCooldown.gameObject.SetActive(true);
            hud.virusCooldown.fillAmount = (1.0f - (elapsedTime  / virusCoolDown));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hud.virusCooldown.gameObject.SetActive(false);
        _virusOnCoolDown = false;
    }

}

