using System;
using DG.Tweening;
using UnityEngine;

public class EnemyOrb : AEnemy
{

    [SerializeField]
    GameObject visual;
    [SerializeField]
    GameObject iris;


    void Start()
    {
        //visual.transform.DOLocalMoveY(transform.position.y + 0.02f, 4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        //avisual.transform.DOLocalMoveX(transform.position.x + 0.02f, 0.7f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        iris.transform.DOScaleX(iris.transform.localScale.x + 0.03f, 1.6f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
        iris.transform.DOScaleZ(iris.transform.localScale.z + 0.04f, 1.6f).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
    }

}
