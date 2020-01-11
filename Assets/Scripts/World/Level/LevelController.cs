using Doozy.Engine;
using System;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    private static PlayerController player;
    public SpawnPoint SpawnPoint;
    public int Deaths;
    public float CompletionTime;

    public TextMeshProUGUI DeathText;
    public TextMeshProUGUI TimeText;

    private RoomFollower roomFollower;


    void Start()
    {
        Instance = this;
        this.enabled = true;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        roomFollower = gameObject.GetComponent<RoomFollower>();
    }

    private void Awake()
    {
        LevelEvents.OnRoomChange += OnRoomChange;
        LevelEvents.OnPlayerRespawn += OnPlayerRespawn;
    }

    private void OnDestroy()
    {
        LevelEvents.OnRoomChange -= OnRoomChange;
        LevelEvents.OnPlayerRespawn -= OnPlayerRespawn;
    }

    void OnRoomChange(RoomBoundsManager Room)
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (player.state != player.states[3])
        {
            SpawnPoint = Room.GetClosestSpawnPoint();
        }
    }

    void OnPlayerRespawn()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.Respawn();
        Deaths++;
    }

    private void Update()
    {
        CompletionTime += Time.deltaTime;
        if (SpawnPoint)
        {
            Debug.DrawLine(SpawnPoint.transform.position, SpawnPoint.transform.position + new Vector3(0, 1, 0));
        }
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    GameEventMessage.SendEvent("GoToMainMenu");
        //}
    }

    public void LevelCompleted()
    {
        roomFollower.TargetZoom = 2;
        player.disablePlayer();
        DeathText.text = "Deaths: " + Deaths;
        TimeText.text = "Time: " + TimeSpan.FromSeconds((int)CompletionTime).ToString(@"mm\:ss");
    }
}
