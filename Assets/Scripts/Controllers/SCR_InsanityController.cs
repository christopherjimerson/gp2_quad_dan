using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SCR_InsanityController : MonoBehaviour
{
    public static SCR_InsanityController Instance;
#region Variables
    //Insanity is triggered when above the RAM. 
    [SerializeField]
    float maxRam = 160f;
    public float _usedRam = 0f;
    [SerializeField]
    float extraRam = 0f;


    [Header("First Threshold")] [SerializeField]
    public float firstThresholdPercentage = 0f;
    [SerializeField]
    List<SCR_SO_InsanityEffect> firstThresholdEffects = new();
    
    [Header("Second Threshold")]
    [SerializeField]
    public float secondThresholdPercentage = 25f;
    [SerializeField]
    List<SCR_SO_InsanityEffect> secondThresholdEffects = new();
    
    [Header("Third Threshold")]
    [SerializeField]
    public float thirdThresholdPercentage = 50f;
    [SerializeField]
    List<SCR_SO_InsanityEffect> thirdThresholdEffects = new();
    
    [Header("Fourth Threshold")]
    [SerializeField]
    public float fourthThresholdPercentage = 100f;
    [SerializeField]
    List<SCR_SO_InsanityEffect> fourthThresholdEffects = new();

    float _lastInsanity = 0f;
    bool _firstInsanityTriggered = false;
    bool _secondInsanityTriggered = false;
    bool _thirdInsanityTriggered = false;
    bool _fourthInsanityTriggered = false;
    
#endregion
#region Properties
    public float MaxRam
    {
        get { return maxRam; }
        set { maxRam = value; }
    }
    
    public float UsedRam
    {
        set { _usedRam = value; }
    }
    
    public float ExtraInsanity
    {
        get { return extraRam; }
        set { extraRam = value; }
    }
        
    public float Insanity 
    {
        get { return ((_usedRam + extraRam)/maxRam) * 100f; }
    }
#endregion
#region Events
    public delegate void OnInsanityChange(float oldInsanity, float newInsanity);
    public static event OnInsanityChange onInsanityChange;
    #endregion
    #region Unity Methods

    public void OnEnable() {

        //SCR_GameController.Instance.OnEquipRam += OnRamAdded;
        //SCR_GameController.Instance.OnUnEquipRam += OnRamRemoved;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void OnDisable()
    {
        SCR_GameController.Instance.OnEquipRam -= OnRamAdded ;
        SCR_GameController.Instance.OnUnEquipRam -= OnRamRemoved ;
    }

    public void OnValidate()
    {
        RecalculateInsanity();
    }

    private void Start()
    {
        //SCR_GameController.Instance.OnEquipRam += OnRamAdded;
        //SCR_GameController.Instance.OnUnEquipRam += OnRamRemoved;
    }

#endregion
#region Private Methods

    void OnRamAdded(SCR_SO_Ram ram)
    {
        var oldRam = _usedRam;
        _usedRam += ram.cost;
        RecalculateInsanity();
        onInsanityChange?.Invoke(oldRam, _usedRam);
        Debug.Log("Ram Added: " + oldRam + " -> " + _usedRam);
    }

    void OnRamRemoved(SCR_SO_Ram ram)
    {
        var oldRam = _usedRam;
        _usedRam -= ram.cost;
        RecalculateInsanity();
        onInsanityChange?.Invoke(oldRam, _usedRam);
        Debug.Log("Ram Removed: " + oldRam + " -> " + _usedRam);
    }

    //Forgive me... I had to do what I had to do...
    void RecalculateInsanity()
    {
        //Debug.Log("Insanity Recalculated at: " + Insanity + "%");
        
        if ((Insanity) > (100f + fourthThresholdPercentage - 0.0002f))
        {
            Debug.Log("Insanity fourth stage: " + Insanity + " - " +  (fourthThresholdPercentage + 100f));
            if (!_firstInsanityTriggered)
            {
                ActivateList(firstThresholdEffects);
                _firstInsanityTriggered = true;
            }
            if (!_secondInsanityTriggered)
            {   
                ActivateList(secondThresholdEffects);
                _secondInsanityTriggered = true;
            }
            if (!_thirdInsanityTriggered)
            {
                ActivateList(thirdThresholdEffects);
                _thirdInsanityTriggered = true;
            }
            if (!_fourthInsanityTriggered)
            {
                ActivateList(fourthThresholdEffects);
                _fourthInsanityTriggered = true;
            }
        }
        else if ((Insanity) > (100f + thirdThresholdPercentage - 0.0002f))
        {
            Debug.Log("Insanity third stage: " + Insanity + " - " + (thirdThresholdPercentage + 100f));
            if (!_firstInsanityTriggered)
            {
                ActivateList(firstThresholdEffects);
                _firstInsanityTriggered = true;
            }
            if (!_secondInsanityTriggered)
            {
                ActivateList(secondThresholdEffects);
                _secondInsanityTriggered = true;
            }
            if (!_thirdInsanityTriggered)
            {
                ActivateList(thirdThresholdEffects);
                _thirdInsanityTriggered = true;
            }
            if (_fourthInsanityTriggered)
            {
                DeactivateList(fourthThresholdEffects);
                _fourthInsanityTriggered = false;
            }
        }
        else if ((Insanity) > (100f + secondThresholdPercentage - 0.0002f))
        {
            Debug.Log("Insanity second stage: " + Insanity + " - " + (secondThresholdPercentage + 100f));
            if (!_firstInsanityTriggered)
            {
                ActivateList(firstThresholdEffects);
                _firstInsanityTriggered = true;
            }
            if (!_secondInsanityTriggered)
            {
                ActivateList(secondThresholdEffects);
                _secondInsanityTriggered = true;
            }
            if (_thirdInsanityTriggered)
            {
                DeactivateList(thirdThresholdEffects);
                _thirdInsanityTriggered = false;
            }
            if (_fourthInsanityTriggered)
            {
                DeactivateList(fourthThresholdEffects);
                _fourthInsanityTriggered = false;
            }
        }
        else if ((Insanity) > (100f + firstThresholdPercentage - 0.0002f))
        {
            Debug.Log("Insanity first stage: " + Insanity + " - " + (firstThresholdPercentage + 100f));
            if (!_firstInsanityTriggered)
            {
                ActivateList(firstThresholdEffects);
                _firstInsanityTriggered = true;
            }
            if (_secondInsanityTriggered)
            {
                DeactivateList(secondThresholdEffects);
                _secondInsanityTriggered = false;
            }
            if (_thirdInsanityTriggered)
            {
                DeactivateList(thirdThresholdEffects);
                _thirdInsanityTriggered = false;
            }
            if (_fourthInsanityTriggered)
            {
                DeactivateList(fourthThresholdEffects);
                _fourthInsanityTriggered = false;
            }
        }
        else
        {
            if (_firstInsanityTriggered)
            {
                DeactivateList(firstThresholdEffects);
                _firstInsanityTriggered = false;
            }
            if (_secondInsanityTriggered)
            {
                DeactivateList(secondThresholdEffects);
                _secondInsanityTriggered = false;
            }
            if (_thirdInsanityTriggered)
            {
                DeactivateList(thirdThresholdEffects);
                _thirdInsanityTriggered = false;
            }
            if (_fourthInsanityTriggered)
            {
                DeactivateList(fourthThresholdEffects);
                _fourthInsanityTriggered = false;
            }
        }
        
        
    }

    void ActivateList(List<SCR_SO_InsanityEffect> effectList)
    {
        foreach (var effect in effectList)
        {
            effect.Activate();
        }    
    }
    
    void DeactivateList(List<SCR_SO_InsanityEffect> effectList)
    {
        foreach (var effect in effectList) { effect.Deactivate();}          
    }

    private void OnDestroy()
    {
        DeactivateList(firstThresholdEffects);
        DeactivateList(secondThresholdEffects);
        DeactivateList(thirdThresholdEffects);
        DeactivateList(fourthThresholdEffects);
    }
    

#endregion

}
