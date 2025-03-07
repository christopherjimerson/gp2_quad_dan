using JetBrains.Annotations;
using UnityEngine;

public class SCR_SpawnEffect : MonoBehaviour
{
    public GameObject SpawnCube; // Reference to the SpawnCube GameObject
    private Animator spawnCubeAnimator; // Animator component on the SpawnCube

    void Start() {
        // Ensure SpawnCube is assigned and has an Animator component
        if (SpawnCube != null)
        {
            spawnCubeAnimator = SpawnCube.GetComponent<Animator>();
            if (spawnCubeAnimator != null)
            {
                PlaySpawnAnimation();
            }
            else
            {
                Debug.LogWarning("Animator component not found on SpawnCube!");
            }
        }
        else
        {
            Debug.LogWarning("SpawnCube is not assigned in the Inspector!");
        }
    }

    private void PlaySpawnAnimation() {
        // Play the animation (replace "SpawnAnimation" with your animation state name)
        spawnCubeAnimator.Play("SpawnShrink");
    }
}
