using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    SpawnPoint SpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        LevelEvents.OnRoomChange += OnRoomChange;
    }

    private void OnDestroy()
    {
        LevelEvents.OnRoomChange -= OnRoomChange;
    }

    void OnRoomChange(RoomBoundsManager Room)
    {
        SpawnPoint = Room.GetClosestSpawnPoint();
    }

    private void Update()
    {
        if (SpawnPoint)
        {
            Debug.DrawLine(SpawnPoint.transform.position, SpawnPoint.transform.position + new Vector3(0, 1, 0));
            Debug.Log(SpawnPoint);
        }
    }
}
