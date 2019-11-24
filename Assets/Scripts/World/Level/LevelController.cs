﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;
using System;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    private static PlayerController player;
    public SpawnPoint SpawnPoint;
    public UIView LevelEnd;
    public int Deaths;
    public float CompletionTime;

    public TextMeshProUGUI DeathText;
    public TextMeshProUGUI TimeText;

    private RoomFollower roomFollower;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
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
        if (player.state != player.states[3])
        {
            SpawnPoint = Room.GetClosestSpawnPoint();
        }
    }

    void OnPlayerRespawn()
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.transform.position = SpawnPoint.transform.position;
        player.Sprite.enabled = true;
        player.Sprite.transform.localRotation = Quaternion.Euler(0,0,0);
        player.body.Movement = new Vector2(0, 0);
        player.body.TargetMovement = new Vector2(0, 0);
        player.state = player.states[0];
        player.RecallOrb();
        player.IsOrbAvailable = true;
        Deaths++;
    }

    private void Update()
    {
        CompletionTime += Time.deltaTime;
        if (SpawnPoint)
        {
            Debug.DrawLine(SpawnPoint.transform.position, SpawnPoint.transform.position + new Vector3(0, 1, 0));
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            LevelCompleted();
        }
    }

    public void LevelCompleted()
    {
        LevelEnd.Show();
        roomFollower.TargetZoom = 2;
        player.disablePlayer();
        DeathText.text = "Deaths: " + Deaths;
        TimeText.text = "Time: " + TimeSpan.FromSeconds((int)CompletionTime).ToString(@"mm\:ss");
    }

    public void Fade()
    {
        GameObject.FindGameObjectWithTag("FadeView").GetComponent<UIView>().Hide();
    }
}
