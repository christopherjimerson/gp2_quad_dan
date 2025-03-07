using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class SCR_Cutscene : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer player;

    [SerializeField] private GameObject videoScreen;

    [SerializeField] private TMP_Text text;

    private bool allowSkipText = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TurnOffCutscene());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            allowSkipText = true;
            text.gameObject.SetActive(true);
            //Display skip text;
        }

        if (!allowSkipText) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TurnOffCutscene()
    {
        yield return new WaitForSeconds((float)player.length-9);
        videoScreen.SetActive(false);
    }
}
