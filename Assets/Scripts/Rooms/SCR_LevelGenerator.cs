using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SCR_LevelGenerator : MonoBehaviour
{
    public static SCR_LevelGenerator Instance;

    public bool AllowLevelGeneration = true;

    public int chestsToSpawn = 1;
    public GameObject chestPrefab;
    private GameObject currentChest;


    public int RoomsToCreate = 5;
    bool spawnRoomHasSpawned = false;
    public GameObject SpawnRoom;

    public SCR_RoomInfo currentSpawnRoom { get; private set; }

    [SerializeField]
    private List<GameObject> Rooms;

    [SerializeField]
    private List<SCR_RoomInfo> SpawnedRooms;
    [SerializeField]
    private float distanceBetweenRooms;

    private RoomType[,] gameMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int CurrentSpawnedRoomsInFloor()
    {
        return SpawnedRooms.Count;
    }

    public void GenerateLevel()
    {
        if(!AllowLevelGeneration) return;
        StartCoroutine(GenerateGridLayout());
    }

    public void DestroyLevel()
    {
        if (!AllowLevelGeneration) return;

        foreach (var room in SpawnedRooms)
        {
            Destroy(room.gameObject);
        }

        SpawnedRooms.Clear();
        Destroy(currentChest);
        EventManager.Instance.FloorDestroyed();
    }

    private IEnumerator GenerateGridLayout()
    {
        PlaceNodesOnGrid();
        PlaceSpawnNodeOnGrid();
        AddRoomsToWorld();

        yield return new WaitForSeconds(0.2f);
        RotateRooms();
        yield return null;
        SpawnChests();
    }

    void SpawnChests()
    {
        for (int i = 0; i < chestsToSpawn; i++)
        {
            int roomNumber = Random.Range(0, SpawnedRooms.Count());

            currentChest = Instantiate(chestPrefab, SpawnedRooms[roomNumber].GetRandomChestLocation().position, Quaternion.identity);
        }
    }

    private void PlaceNodesOnGrid()
    {
        if(RoomsToCreate > 10)
            RoomsToCreate = 10;

        gameMap = RoomsToCreate < 10 ? new RoomType[10, 10] : new RoomType[RoomsToCreate, RoomsToCreate];

        List<Vector2Int> allowed_movements = new List<Vector2Int>
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down
        };

        var rnd = new System.Random();
        Vector2Int curr_pos = new Vector2Int(rnd.Next(RoomsToCreate), rnd.Next(RoomsToCreate));

        gameMap[curr_pos.x, curr_pos.y] = RoomType.One;

        for (int idStep = 0; idStep < RoomsToCreate; idStep++)
        {
            while (true)
            {
                Vector2Int new_pos = curr_pos + allowed_movements[rnd.Next(allowed_movements.Count)];
                // check if the new position is in the grid
                if (new_pos.x >= 0 && new_pos.x < RoomsToCreate && new_pos.y >= 0 && new_pos.y < RoomsToCreate)
                {
                    //If the position is already a room, go next
                    if (gameMap[new_pos.x, new_pos.y] != RoomType.Zero)
                    {
                        curr_pos = new_pos;
                        continue;
                    }

                    // this is a valid position, we set it in the grid
                    gameMap[new_pos.x, new_pos.y] = RoomType.One;

                    // replace curr_pos with new_pos
                    curr_pos = new_pos;

                    // and finally break of the infinite loop
                    break;
                }
            }
        }

    }

    private void PlaceSpawnNodeOnGrid()
    {
        for(int x = 0; x < RoomsToCreate; x++)
        for (int y = 0; y < RoomsToCreate; y++)
        {
            if (gameMap[x,y] != RoomType.One) continue;

            var connections = 0;
            for (int i = -1; i <= 1; i++)
            {
                if (x + i < 0 || x + i >= RoomsToCreate || i == 0) continue;

                if (gameMap[x + i, y] == RoomType.One)
                    connections++;
            }

            for (int i = -1; i <= 1; i++)
            {
                if (y + i < 0 || y + i >= RoomsToCreate || i == 0) continue;

                if (gameMap[x, y + i] == RoomType.One)
                    connections++;
            }

            if (connections != 1) continue;
            gameMap[x, y] = RoomType.Spawn;
            goto Finish;
        }

        Finish:
        return;
    }

    public void AddRoomsToWorld()
    {
        List<GameObject> roomsOfType = new List<GameObject>();

        for (int x = 0; x < RoomsToCreate; x++)
            for (int y = 0; y < RoomsToCreate; y++)
            {
                if (gameMap[x, y] == RoomType.Zero) continue;

                int connections = 0;
                bool isIShape = false;

                for (int i = -1; i <= 1; i++)
                {
                    if (x + i < 0 || x + i >= RoomsToCreate || i == 0) continue;


                    if (gameMap[x + i, y] != RoomType.Zero)
                    {
                        gameMap[x, y] += 1;
                        connections++;
                    }
                }

                if (connections == 2) isIShape = true;
                else connections = 0;


                for (int i = -1; i <= 1; i++)
                {
                    if (y + i < 0 || y + i >= RoomsToCreate || i == 0) continue;

                    if (gameMap[x, y + i] != RoomType.Zero)
                    {
                        gameMap[x, y] += 1;
                        connections++;
                    }
                }

                if (connections == 2) isIShape = true;
                else connections = 0;

                if (gameMap[x,y] != RoomType.One)
                    gameMap[x, y]--;

                roomsOfType.Clear();

                foreach (var room in Rooms)
                {
                    if(room.GetComponent<SCR_RoomInfo>().GetRoomType() != gameMap[x,y]) continue;

                    switch (room.GetComponent<SCR_RoomInfo>().GetRoomType())
                    {
                        case RoomType.Two:
                            if (room.GetComponent<SCR_RoomInfo>().GetTwoRoomShape() == TwoRoomShape.I && isIShape)
                                roomsOfType.Add(room);

                            if (room.GetComponent<SCR_RoomInfo>().GetTwoRoomShape() == TwoRoomShape.L && !isIShape)
                                roomsOfType.Add(room);
                            break;
                        case RoomType.Spawn:
                            roomsOfType.Add(SpawnRoom);
                            break;
                        case RoomType.One:
                            roomsOfType.Add(room);
                            break;
                        case RoomType.Zero:
                            roomsOfType.Add(room);
                            break;
                        case RoomType.Three:
                            roomsOfType.Add(room);
                            break;
                        case RoomType.Four:
                            roomsOfType.Add(room);
                            break;
                        default:
                            Debug.Log("WHY GOD WHYYYYY");
                            break;
                    }
                }

                GameObject roomToSpawn = !roomsOfType.Any() ? SpawnRoom : roomsOfType[Mathf.FloorToInt(UnityEngine.Random.Range(0, roomsOfType.Count()-1))];

                if (roomToSpawn == null)
                {
                    Debug.Log("SPawnroom was null");
                    continue;
                } 

                roomToSpawn = Instantiate(roomToSpawn, new Vector3(x * distanceBetweenRooms, 0, y * distanceBetweenRooms), Quaternion.identity);

                SCR_RoomInfo ri = roomToSpawn.GetComponent<SCR_RoomInfo>();
                ri.Position = new Vector2Int(x, y);
                SpawnedRooms.Add(ri);
            }
    }

    private void RotateRooms()
    {
        foreach (var room in SpawnedRooms)
        {
            room.CheckDoors();
        }

        foreach (var room in SpawnedRooms)
        {
            room.TurnOffAllDoors();
        }
    } 
}
