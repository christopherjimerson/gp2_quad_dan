using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerRadar : MonoBehaviour
{
    private Collider[] enemiesInCollider;

    [SerializeField] private float radious = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateRadar());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator UpdateRadar()
    {
        yield return new WaitForSeconds(0.25f);

        Physics.OverlapSphereNonAlloc(transform.position, radious, enemiesInCollider);

        RaycastHit hit;

        foreach (var enemy in enemiesInCollider)
        {
            Physics.Raycast(transform.position, (enemy.transform.position - transform.position).normalized, radious,
                LayerMask.GetMask("Enemy"));
        }
    }
}
