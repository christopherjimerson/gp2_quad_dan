using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    protected void OnDrawGizmos()
    {
        Gizmos.color = new Color(.2f, .2f, 1f, 0.8f);
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
    
    public void OnEnable()
    {
        AEnemy.Waypoints.Add(this.gameObject);
    }

    public void OnDisable()
    {
        AEnemy.Waypoints.Remove(this.gameObject);
    }
    
    
}
