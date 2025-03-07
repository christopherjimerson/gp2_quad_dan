using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_HeadsUpDisplay : MonoBehaviour
{
    //[SerializeField] private Image ammoCount;
    [SerializeField] public Image health;
    //[SerializeField] public bool hpSOEquiped = false;
    [SerializeField] private Image Insanity;
    [SerializeField] public Image  staminaBar;
    [SerializeField] public GameObject miniMap, radarMap;
    //[SerializeField] public bool miniMapSOEquiped = false; 
    [SerializeField] public GameObject pointReticle;
    [SerializeField] public GameObject shotgunReticle;
    [SerializeField] public GameObject scopeReticle;
    [SerializeField] public bool reticalOn = true;
    //[SerializeField] public bool crosshairSOEquiped = false;
    [SerializeField] private SCR_FirstPersonController player;
    [Header("Power Up Icons")]
    [SerializeField] public Image shieldArea;
    [SerializeField] public GameObject shieldIcon;
    [SerializeField] public Image shieldCooldown;
    [SerializeField] public Image shieldNotEquipped;
    [SerializeField] public GameObject shieldEffect;

    [SerializeField] public Image stunArea;
    [SerializeField] public GameObject stunIcon;
    [SerializeField] public Image stunCooldown;
    [SerializeField] public Image stunNotEquipped;

    [SerializeField] public Image repelArea;
    [SerializeField] public GameObject repelIcon;
    [SerializeField] public Image repelCooldown;
    [SerializeField] public Image repelNotEquipped;

    [SerializeField] public Image virusArea;
    [SerializeField] public GameObject virusIcon;
    [SerializeField] public Image virusCooldown;
    [SerializeField] public Image virusNotEquipped;



    public float AmmoPercent = 0;

    public static SCR_HeadsUpDisplay Instance { get; private set; }

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


        //SCR_GameController.Instance.OnEquipRam += EnableReticle;
        //SCR_GameController.Instance.OnUnEquipRam += EnableReticle;
    }


    void Start()
    {
        UpdateStaminaBar(1f);
        if (reticalOn)
        {
            pointReticle.gameObject.SetActive(false);
            shotgunReticle.gameObject.SetActive(false);
            scopeReticle.gameObject.SetActive(false);

        }
        shieldCooldown.gameObject.SetActive(false);
        stunCooldown.gameObject.SetActive(false);
        repelCooldown.gameObject.SetActive(false);
        virusCooldown.gameObject.SetActive(false); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmoCount(int currentAmmo, int maxAmmo)
    {
        //if (!ammoCount) return;
        AmmoPercent = ((((float)maxAmmo)- (float)currentAmmo) / (float)maxAmmo);
        //AmmoPercent = Mathf.Clamp01(AmmoPercent);

        //ammoCount.fillAmount = AmmoPercent;
        if (AmmoPercent <= 0) {
            miniMap.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            miniMap.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            miniMap.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            miniMap.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        else {
            miniMap.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        }
        
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if(!health) return;
        float healthPercent = ((float)currentHealth / (float)maxHealth);
        healthPercent = Mathf.Clamp01(healthPercent);

        health.fillAmount = healthPercent;
    }

    public void UpdateStaminaBar(float staminaFraction)
    {
        if(!staminaBar) return;

        staminaFraction = Mathf.Clamp01(staminaFraction);
        staminaBar.fillAmount = staminaFraction;
    }

    public void UpdateInsanity(int currentInsanity, int maxInsanity)
    {
        if (!Insanity) return;
        float InsanityPercent = ((float)currentInsanity / (float)maxInsanity);
        InsanityPercent = Mathf.Clamp01(InsanityPercent);

        Insanity.fillAmount = InsanityPercent;
    }

}
