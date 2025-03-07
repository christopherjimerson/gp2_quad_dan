using UnityEngine;

public class SCR_ProjectilePool : SCR_ObjectPool
{
    public static SCR_ProjectilePool Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateObjects();
    }

    public GameObject GetProjectile(Transform transform)
    {
        var projectile = base.GetObject();

        projectile.SetActive(true);

        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;

        return projectile;
    }

}
