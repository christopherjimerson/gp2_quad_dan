using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_HealthOrb : MonoBehaviour
{
    [SerializeField]
    private Vector3 movePosition;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float moveTimer = 1f;

    [SerializeField]
    private float moveDistance = 3f;

    [SerializeField]
    private int healAmount = 10;

    private bool allowMovement = false;

    // Update is called once per frame
    void Update()
    {
        //if (!allowMovement) return;
        //var step = moveSpeed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, movePosition, step);
    }

    private void OnEnable()
    {
        //movePosition = new Vector3(transform.position.x + Random.Range(-moveDistance, moveDistance), moveDistance, transform.position.z + Random.Range(-moveDistance,moveDistance));
        //StartCoroutine(AllowMovement());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        SCR_GameController.Instance.CurrentPlayer.GetComponent<PlayerHealth>().HealUnit(healAmount);
        gameObject.SetActive(false);
    }

    //IEnumerator AllowMovement()
    //{
        //allowMovement = true;
        //yield return new WaitForSeconds(moveTimer);
        //allowMovement = false;
    //}
}
