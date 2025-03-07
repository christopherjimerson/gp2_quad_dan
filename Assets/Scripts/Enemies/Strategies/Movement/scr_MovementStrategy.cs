using UnityEngine;

public abstract class MovementStrategy : MonoBehaviour
{

    public abstract void Execute(AEnemy enemy);
    
}
