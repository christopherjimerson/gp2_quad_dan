using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SCR_ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private int bulletDamage;
    [SerializeField] public float projectileSpeed = 10f;

    public void SetDamage(int damage) {
        bulletDamage = damage;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            bool notDamagable = other.gameObject.GetComponent<SCR_FirstPersonController>()._isShieldActive;
            if (!notDamagable)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);

            }

            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Enemy"))
        {
            //Ignore the collision.
        }
        else
        {
            gameObject.SetActive(false);
            Debug.Log("Projectile Hit Something! " + other.tag);
        }

    }

    private void OnEnable()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward.normalized * projectileSpeed, ForceMode.Impulse);
        StartCoroutine(TurnOffBullet());
    }

    private IEnumerator TurnOffBullet()
    {
        yield return new WaitForSeconds(2f);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
