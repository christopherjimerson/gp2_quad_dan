using System;
using DG.Tweening;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] 
    protected GameObject pivot;
    [SerializeField] 
    protected GameObject shell;
    [SerializeField] 
    protected GameObject core;
    
    public void OnEnable()
    {
        AEnemy.Targets.Add(this.gameObject);
    }

    public void OnDisable()
    {
        AEnemy.Targets.Remove(this.gameObject);
    }

    public void Start()
    {
        pivot.transform.DOLocalMoveY( 0.2f, 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        shell.transform.DORotate(new Vector3(0,360,0), 2f,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        core.transform.DORotate(new Vector3(0,-360,0), 3.5f,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    
}
