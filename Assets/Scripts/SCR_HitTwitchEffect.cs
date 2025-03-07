using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_HitTwitchEffect : MonoBehaviour
{
    public float twitchDuration = 0.5f; // How long the twitch lasts
    public float twitchIntensity = 0.1f; // How much the twitch moves or rotates

    private bool isTwitching = false;

    public void TriggerTwitch() {
        if (!isTwitching)
            StartCoroutine(Twitch());
    }

    private IEnumerator Twitch() {
        Debug.Log("Twitch Twitch");
        isTwitching = true;
        Vector3 originalPosition = transform.localPosition;

        float elapsedTime = 0f;
        while (elapsedTime < twitchDuration)
        {
            Vector3 twitchOffset = new Vector3(
                Random.Range(-twitchIntensity, twitchIntensity),
                Random.Range(-twitchIntensity, twitchIntensity),
                Random.Range(-twitchIntensity, twitchIntensity)
            );

            transform.localPosition = originalPosition + twitchOffset;

            yield return null;

            elapsedTime += Time.deltaTime;
        }

        transform.localPosition = originalPosition;
        isTwitching = false;
    }
}
