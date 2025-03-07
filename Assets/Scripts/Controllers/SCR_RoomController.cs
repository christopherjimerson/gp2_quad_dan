using UnityEngine;

public class SCR_RoomController : MonoBehaviour
{
    Vector3 _lastEnemyPosition;
    [SerializeField] GameObject _roomEndPreFab;
    void Start()
    {
        EventManager.Instance.OnRoomCompleted += RoomComplete;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RoomComplete(GameObject lastEnemy)
    {
        _lastEnemyPosition = lastEnemy.transform.position;
        _roomEndPreFab = Instantiate(_roomEndPreFab, _lastEnemyPosition, Quaternion.identity);
        Debug.Log("Done");
    }

    void OnDisable()
    {
        EventManager.Instance.OnRoomCompleted -= RoomComplete;
    }
}
