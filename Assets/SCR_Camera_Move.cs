using UnityEngine;

public class SCR_Camera_Move : MonoBehaviour
{
    public float speed = 2f;  

    void Update() {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
