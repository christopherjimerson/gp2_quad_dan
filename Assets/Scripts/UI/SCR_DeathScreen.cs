using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_DeathScreen : MonoBehaviour
{
    [SerializeField] private float deatBufferTime = 1f;
    private bool allowSpawning = false;

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if(!allowSpawning) return;

        if (Input.anyKeyDown)
        {
            SCR_GameController.Instance.RespawnLevelAndPlayer();
            gameObject.SetActive(false);
        }
    }

    IEnumerator AllowRespawn()
    {
        yield return new WaitForSeconds(deatBufferTime);
        allowSpawning = true;
    }

    private void OnEnable()
    {
        AudioManager.Instance.Stop("Music_Main");
        AudioManager.Instance.Play("CRT_Static");
        StartCoroutine(AllowRespawn());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        allowSpawning = false;
    }
}
