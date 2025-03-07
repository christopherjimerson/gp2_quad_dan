using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SCR_EnemyController : MonoBehaviour
{
    public static SCR_EnemyController Instance;
    [SerializeField] public List<GameObject> activeEnemies;
    [SerializeField] public  List<GameObject> deadEnemies;
    [SerializeField]float _enemyHealthMultiplier = 1.0f;
    public float EnemyHealthMultipler
    {
        get
        {
            return _enemyHealthMultiplier;
        }
        set
        {
            _enemyHealthMultiplier = value;
            onEnemyHealthMultChange?.Invoke();
        }
    }
    
    public delegate void OnEnemyHealthChange();
    public static event OnEnemyHealthChange onEnemyHealthMultChange;
    
    void Start()
    {
        EventManager.Instance.OnFloorDestroyed += ClearEnemies;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CheckEnemies(GameObject lastEnemy)
    {
        if (activeEnemies.Count == 0)
        {
            Debug.Log("All enemies dead");
            EventManager.Instance.RoomCompleted(lastEnemy);
        }
    }

    public void DefeatEnemy(GameObject enemyObject)
    {
        if (!deadEnemies.Contains(enemyObject))
        {
            activeEnemies.Remove(enemyObject);
            deadEnemies.Add(enemyObject);
            enemyObject.SetActive(false);
            CheckEnemies(enemyObject);

        }
    }

    private void ClearEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Destroy(activeEnemies[i]);
            activeEnemies[i] = null;

        }

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if(activeEnemies[i] == null) {activeEnemies.RemoveAt(i);}

        }
        activeEnemies.Clear();
    }

    //Anything under this comment is Temp code and might be changed depending on future implementation
    public void ActivateEnemies()
    {

    }

}
