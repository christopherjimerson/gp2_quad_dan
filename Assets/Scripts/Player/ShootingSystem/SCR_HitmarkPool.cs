using UnityEngine;

public class SCR_HitmarkPool : SCR_ObjectPool
{
    public static SCR_HitmarkPool Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetHitmark(Transform PositionTransform)
    {
        var hitmark = base.GetObject();

        hitmark.SetActive(true);

        hitmark.transform.position = transform.position;
        hitmark.transform.rotation = transform.rotation;

        return hitmark;
    }
}
