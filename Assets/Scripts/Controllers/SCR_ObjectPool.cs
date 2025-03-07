using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SCR_ObjectPool : MonoBehaviour
{
    [SerializeField]
    protected GameObject ObjectPrefab;
    
    [SerializeField]
    [Header("Just info, no touch")]
    protected List<GameObject> ObjectPool;

    public GameObject GetObject()
    {
        if (!ObjectPrefab) return null;
        while (true)
        {
            foreach (var objectInPool in ObjectPool.Where(objectInPool => !objectInPool.activeSelf))
            {
                return objectInPool;
            }

            InstantiateObjects();
        }
    }

    public void InstantiateObjects()
    {
        if (!ObjectPrefab) return;

        for (int i = 0; i < 10; i++)
        {
            var projectile = Instantiate(ObjectPrefab, gameObject.transform);
            projectile.transform.parent = null;
            projectile.SetActive(false);
            ObjectPool.Add(projectile);
        }
    }

    public void DisableAllActiveObjects()
    {
        if (!ObjectPrefab) return;

        foreach (var objectInPool in ObjectPool.Where(objectInPool => objectInPool.activeSelf))
        {
            objectInPool.SetActive(false);
        }
    }
}
