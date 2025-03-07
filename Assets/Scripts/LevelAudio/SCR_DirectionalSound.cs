using UnityEngine;

public class SCR_DirectionalSound : MonoBehaviour {
    public AudioSource audioSource; 
    public AudioClip soundClip; 
    public bool playOnce = true; 

    public bool hasPlayed = false; 

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) 
        {
            if (!playOnce || !hasPlayed)
            {
                PlaySound();
                hasPlayed = true;
            }
        }
    }

    void PlaySound() {
        if (audioSource && soundClip)
        {
            Debug.Log("Play");
            //audioSource.clip = soundClip;
            audioSource.Play();
        }
    }
}