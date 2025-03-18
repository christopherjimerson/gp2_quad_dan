using Unity.VisualScripting;
using UnityEngine;

public class SCR_OutOfBoundsTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider player) {
        SCR_GameController.Instance.OutOfBoundsRespawn(player.gameObject);
    }
}
