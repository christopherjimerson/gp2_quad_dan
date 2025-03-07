using System.Collections.Generic;
using UnityEngine;

public class OldLevelController : MonoBehaviour
{
    public static OldLevelController Instance;

    public Vector2Int LayoutGridSize;
    public GameObject[] Rooms;
    public int RoomsToSpawn = 3;
    bool[,] roomMap = null;
    public List<Vector3> PossiblePlayerSpawns = new List<Vector3>();

    public List<GameObject> CurrentSpawnedRooms;

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

    public void CreateFloor()
    {
        roomMap = GenerateMap(LayoutGridSize.x * LayoutGridSize.y);

        for (int x = 0; x < LayoutGridSize.x; x++)
            for (int y = 0; y < LayoutGridSize.y; y++)
                if (roomMap[x, y] == true)
                {
                    CurrentSpawnedRooms.Add(Instantiate(Rooms[Random.Range(0, Rooms.Length)], new Vector3((x * 10) * 10f, 0, (y * 10) * 10f), Quaternion.identity));
                    PossiblePlayerSpawns.Add(new Vector3((x * 10) * 10f, 1, (y * 10) * 10f));
                }
    }

    public void DestroyCurrentLevel()
    {
        foreach (var room in CurrentSpawnedRooms)
        {
            Destroy(room);
        }
        PossiblePlayerSpawns.Clear();
        CurrentSpawnedRooms.Clear();
    }

    /// <summary>
    /// Generates the room layout and spawns all rooms into the world
    /// </summary>
    private bool[,] GenerateMap(int nb_steps)
    {
        int currentPlacedRooms = 0;
        bool[,] grid = new bool[LayoutGridSize.x, LayoutGridSize.y];

        System.Random rnd = new System.Random();
        Vector2Int curr_pos = new Vector2Int(rnd.Next(LayoutGridSize.x), rnd.Next(LayoutGridSize.y));

        grid[curr_pos.x, curr_pos.y] = true;

        List<Vector2Int> allowed_movements = new List<Vector2Int>
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down
        };

        // iterate on the number of steps and move around
        for (int id_step = 0; id_step < nb_steps; id_step++)
        {
            while (true)
            {
                Vector2Int new_pos = curr_pos + allowed_movements[rnd.Next(allowed_movements.Count)];
                // check if the new position is in the grid
                if (new_pos.x >= 0 && new_pos.x < LayoutGridSize.x && new_pos.y >= 0 && new_pos.y < LayoutGridSize.y)
                {
                    //if position is empty, we increment current room (trying to get it to total rooms)
                    if (grid[new_pos.x, new_pos.y] == false)
                        currentPlacedRooms++;

                    // this is a valid position, we set it in the grid
                    grid[new_pos.x, new_pos.y] = true;

                    // replace curr_pos with new_pos
                    curr_pos = new_pos;

                    // and finally break of the infinite loop
                    break;
                }
            }
            if (currentPlacedRooms >= RoomsToSpawn)
                break;
        }

        return grid;
    }
}
