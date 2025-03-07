using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProjectileReturning : Projectile
{


    // Update is called once per frame
    public override void FixedUpdate()
    {
        //This needs to be here so the regular one doesn't fire.
        //transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.fixedDeltaTime);
    }

    public void SetUpAndFire(Vector3 direction, float speed, float lifetime)
    {
        StartCoroutine(FireAndReturn(direction, speed, lifetime));

    }

    IEnumerator FireAndReturn(Vector3 direction, float speed, float lifetime)
    {
        
        _direction = direction.normalized;
        _speed = speed;
        _target = transform.position + (_direction * (speed * (lifetime/2)));
        transform.LookAt(_target);
        //DOTween
        transform.DOMove(endValue:_target, lifetime/2).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
