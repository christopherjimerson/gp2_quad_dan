using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    protected float _speed;
    protected Vector3 _direction;
    protected Vector3 _target;
    

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Vector3 Direction
    {
        get => _direction;
        set {
            _direction = value.normalized;
            _target = transform.position + (_direction * 200f);
        }
    }
    
    public void OnEnable()
    {
        _direction = Vector3.zero;
        _speed = 0f;
        _target = Vector3.zero;
        transform.LookAt(_target);
    }

    public virtual void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.fixedDeltaTime);
    }
    
    
    //TODO: On Collision enter.
}
