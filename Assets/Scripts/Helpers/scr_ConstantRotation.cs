using DG.Tweening;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    
    [SerializeField]
    float rotationSpeed = 2f;
    [SerializeField]
    bool invert = true;

    void Start()
    {
        
        int degrees = 360;
        if (invert) degrees = -360;
        transform.DORotate(new Vector3(0,degrees,0), rotationSpeed,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        
    }


}
