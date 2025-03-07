using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RoomType {Zero, One, Two, Three, Four, Spawn}
public enum TwoRoomShape {No, L, I}

public class SCR_RoomInfo : MonoBehaviour
{
    public bool IsSpawnRoom;
    public GameObject SpawnPoint;

    public List<GameObject> chestLocations;

    public Vector2Int Position;

    public List<Door> ExitDoors;
    [SerializeField]
    private List<GameObject> doors;
    private float doorCheckRadius = 3f;
    [SerializeField] private LayerMask doorLayer;

    public int rotationValue = 0;


    [SerializeField]
    private RoomType RoomType;
    [SerializeField]
    private TwoRoomShape twoRoomShape;
    public RoomType GetRoomType() => RoomType;
    public TwoRoomShape GetTwoRoomShape() => twoRoomShape;

    private void Start()
    {
        if (IsSpawnRoom)
        {
            SCR_GameController.Instance.spawnRoom = gameObject;
            EventManager.Instance.SpawnRoomSpawned();
            //SCR_GameController.Instance.LockDoorsFirstPlaythrough();

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
            Rotate();
    }

    private void Rotate()
    {
        switch (rotationValue)
        {
            case 0:
                transform.Rotate(0, 0, 0);
                rotationValue++;
                break;
            case 1:
                transform.Rotate(0, 90, 0);
                rotationValue++;
                break;
            case 2:
                transform.Rotate(0, 180, 0);
                rotationValue++;
                break;
            case 3:
                transform.Rotate(0, -90, 0);
                rotationValue = 0;
                break;
        }

        
    }

    public Transform GetRandomChestLocation()
    {
        return chestLocations[Random.Range(0, chestLocations.Count() - 1)].transform;
    }

    public void CheckDoors()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (var door in ExitDoors)
            {
                Collider[] col = Physics.OverlapSphere(door.GameObject.transform.position, doorCheckRadius, doorLayer);
                door.connected = col.Length > 1;
            }

            int connected = ExitDoors.Count(door => door.connected);

            if (connected >= ExitDoors.Count)
                break;
            

            Rotate();
        }
    }

    public void TurnOffAllDoors()
    {
        foreach(var door in doors)
            door.SetActive(false);
    }
}

[System.Serializable]
public class Door
{
    public GameObject GameObject;
    public bool connected;
}
