using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SCR_PostProcessController : MonoBehaviour
{
    [SerializeField] private Volume volume; 
    private bool isEnabled = false;

    private URPGlitch.AnalogGlitchVolume analogGlitchVolume;
    private URPGlitch.DigitalGlitchVolume digitalGlitchVolume;
    
    [SerializeField] private Timer glitchTimer;
    private bool _glitchEffectRunning;

    [Header("Effect Parameters")] 
    [SerializeField] private float scanLineJitter = 0.4f;
    [SerializeField] private float horizontalShake = 0.2f;
    [SerializeField] private float colorDrift = 0.4f;
    [SerializeField] private float digitalIntensity = 0.44f;

    private void Start()
    {
        volume.profile.TryGet(out analogGlitchVolume);
        volume.profile.TryGet(out digitalGlitchVolume);
        
    }

    private void OnEnable()
    {
        PlayerHealth.onPlayerDamaged += FireGlitch;
        glitchTimer.timerDone += ReadyForGlitch;
    }

    private void OnDisable()
    {
        PlayerHealth.onPlayerDamaged -= FireGlitch;
        glitchTimer.timerDone -= ReadyForGlitch;
    }

    private void FireGlitch(int damage)
    {
        if (_glitchEffectRunning) return;

        StartCoroutine(HitGlitch());

    }

    private void ReadyForGlitch()
    {
        _glitchEffectRunning = false;
    }
    
    public void ToggleGlitchEffects()
    {
        
        isEnabled = !isEnabled;
        analogGlitchVolume.active = isEnabled;
        digitalGlitchVolume.active = isEnabled;
    }

    IEnumerator HitGlitch()
    {
        _glitchEffectRunning = true;
        ToggleGlitchEffects();
        DOTween.To(() => analogGlitchVolume.scanLineJitter.value,
            x => analogGlitchVolume.scanLineJitter.value = x,
            scanLineJitter, glitchTimer.Length/2).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);
        
        DOTween.To(() => analogGlitchVolume.horizontalShake.value,
            x => analogGlitchVolume.horizontalShake.value = x,
            horizontalShake, glitchTimer.Length/2).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);
        
        DOTween.To(() => analogGlitchVolume.colorDrift.value,
            x => analogGlitchVolume.colorDrift.value = x,
            colorDrift, glitchTimer.Length/2).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);
        
        DOTween.To(() => digitalGlitchVolume.intensity.value,
            x => digitalGlitchVolume.intensity.value = x,
            digitalIntensity, glitchTimer.Length/2).SetEase(Ease.OutCirc).SetLoops(2, LoopType.Yoyo);

        
        yield return new WaitForSeconds(glitchTimer.Length);
        
        
        analogGlitchVolume.scanLineJitter.value = Random.Range(0f, 0.4f);
        analogGlitchVolume.verticalJump.value = Random.Range(0f, 0f);
        analogGlitchVolume.horizontalShake.value = Random.Range(0f, 0.2f);
        analogGlitchVolume.colorDrift.value = Random.Range(0f, 0.4f);

        digitalGlitchVolume.intensity.value = Random.Range(0f, 0.44f);
        
    
        ToggleGlitchEffects();
        _glitchEffectRunning = false;
        yield break;
    }


    // Update is called once per frame
    void Update()
    {
        
    } 
}
