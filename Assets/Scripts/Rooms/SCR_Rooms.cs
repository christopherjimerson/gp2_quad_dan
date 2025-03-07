using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Obsolete("This class is not in use anymore, use: SCR_RoomInfo.cs", false)]
public class SCR_Rooms : MonoBehaviour
{
    public Transform spawnPoint;
    public List<GameObject> roomEnemies;
    public List<Doors> Doors;

    public float doorCheckRadius = 3f;
    public LayerMask doorLayer;

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Rotate();
        }
    }

    public void SpawnEnemies()
    {
        foreach (var enemy in roomEnemies)
        {
            enemy.SetActive(true);
        }
    }

    /// <summary>
    /// Rotates the room 90 degrees to the left
    /// </summary>
    public void Rotate()
    {
        int rotation = (int)((transform.rotation.y + 90f) / 10) * 10;
        transform.Rotate(0, rotation, 0);
    }

    public void TurnOffDoors()
    {
        foreach (var door in Doors)
        {
            door.door.SetActive(false);
        }
    }

    public void ResetConnections()
    {
        foreach (var door in Doors)
        {
            door.connected = false;
        }
    }

    public bool CheckDoors()
    {
        foreach (var door in Doors.Where(door => door.door.gameObject))
        {
            Collider[] col = Physics.OverlapSphere(door.door.transform.position, doorCheckRadius, doorLayer);

            door.connected = col.Length > 1;
        }

        int connected = Doors.Count(door => door.connected);

        return connected >= Doors.Count;
    }
}
[System.Serializable]
public class Doors
{
    public GameObject door;
    public bool connected;
}