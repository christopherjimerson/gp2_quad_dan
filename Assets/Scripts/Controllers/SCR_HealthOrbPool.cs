using UnityEngine;

public class SCR_HealthOrbPool : SCR_ObjectPool
{
    public static SCR_HealthOrbPool Instance;

    [SerializeField]
    private int minHealthOrbsToSpawn = 3, maxHealthOrbsToSpawn = 5;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnHealthOrbs(Vector3 position)
    {
        int orbsToSpawn = (int)Random.Range(minHealthOrbsToSpawn, maxHealthOrbsToSpawn);
        for (int i = 0; i < orbsToSpawn; i++)
        {
            var orb = GetObject();
            float range = Random.Range(0.5f, 2f);
            Vector3 newPos = new Vector3(position.x + range, position.y + 1f, position.z + range);
            orb.transform.position = newPos;
            orb.SetActive(true);
        }
    }
}
