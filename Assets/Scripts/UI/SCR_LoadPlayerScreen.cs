using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_LoadPlayerScreen : MonoBehaviour
{
    [SerializeField] private float loadTime = 3f; 
    [SerializeField] private Image loadObject;    
    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    private void OnEnable() {
        loadObject.fillAmount = 0f;
        StartCoroutine(LoadProgress());
    }

    private IEnumerator LoadProgress() {
        float elapsedTime = 0f;

        while (elapsedTime < loadTime)
        {
            elapsedTime += Time.deltaTime;
            loadObject.fillAmount = Mathf.Clamp01(elapsedTime / loadTime);
            yield return null;
        }
        SCR_GameController.Instance.SpawnPlayer();
        gameObject.SetActive(false); 
    }
}
