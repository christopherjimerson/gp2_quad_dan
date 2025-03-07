using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_RamUnlocked : MonoBehaviour
{
    public GameObject border;
    public float blinkTime = 1f;
    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            border.SetActive(true);
            yield return new WaitForSeconds(blinkTime);
            border.SetActive(false);
            yield return new WaitForSeconds(blinkTime);
        }
        
    }
}
