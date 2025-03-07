using UnityEngine;
using System.Collections;


public class SCR_FloatingAmmo : MonoBehaviour {
    public Transform target; 
    public Vector3 offset = new Vector3(0.2f, 0.3f, 0f); 
    public float floatStrength = 0.1f; 
    public float floatSpeed = 2f; 

    [Range(0, 100)] public float ammo = 100f; 
    public float minScale = 0.2f; 
    public float maxScale = 1f; 
    private SCR_HeadsUpDisplay hud;

    private Vector3 initialOffset;
    private Vector3 originalScale;
    void Start()
    {
        hud = SCR_HeadsUpDisplay.Instance;
        initialOffset = offset;
        originalScale = transform.localScale;
    }

    void Update() {
        if (target == null) return;

        // Floating effect
        float floatY = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        float floatX = Mathf.Cos(Time.time * floatSpeed * 0.7f) * floatStrength * 0.5f;
        float floatZ = Mathf.Sin(Time.time * floatSpeed * 0.5f) * floatStrength * 0.5f;
        Vector3 dynamicOffset = initialOffset + new Vector3(floatX, floatY, floatZ);

        // Update position relative to the target
        transform.position = target.position + target.right * dynamicOffset.x
                                             + target.up * dynamicOffset.y
                                             + target.forward * dynamicOffset.z;

        ScaleOrb();
    }

    void ScaleOrb()
    {
        float ammoPercent = hud.AmmoPercent;
        float scalePercentage = Mathf.Clamp(ammoPercent, 0f, 1f);
        float newScale = Mathf.Lerp(minScale, maxScale, scalePercentage);
        transform.localScale = originalScale * newScale;
    }

    public void StartReload(float reloadTime) {
        StopAllCoroutines(); 
        StartCoroutine(ScaleToOriginal(reloadTime));
    }

    private IEnumerator ScaleToOriginal(float reloadSpeed) {
        float elapsedTime = 0f;
        Vector3 currentScale = transform.localScale;

        while (elapsedTime < reloadSpeed)
        {
            transform.localScale = Vector3.Lerp(currentScale, originalScale, elapsedTime / reloadSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; 
    }
}