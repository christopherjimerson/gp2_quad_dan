using UnityEngine;

public class SCR_EnemyHitEffect : MonoBehaviour
{
    public ParticleSystem hitEffect; 

    public void SpawnEffect(Vector3 hitPosition) {
        ParticleSystem effect = Instantiate(hitEffect, hitPosition, Quaternion.identity);
        effect.Play();
        Destroy(effect.gameObject, 1f); 
    }
}
