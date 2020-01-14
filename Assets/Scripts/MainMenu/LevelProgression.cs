using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelProgression : MonoBehaviour
{
    [SerializeField]
    private LevelDoor[] levelDoors;
    [SerializeField]
    private List<LevelDoor> openLevels;
    [SerializeField]
    private int fileHighestLevelReached = -1;

    private void Start()
    {
        levelDoors = GetComponentsInChildren<LevelDoor>();
        DisableAllLevelDoors();

        //initialize from file
        fileHighestLevelReached = PlayerPrefs.GetInt("highest", 1);

        for (int i = 0; i < fileHighestLevelReached; i++)
        {
            openLevels.Add(levelDoors[i]);
        }

        ShowAllOpenLevelDoors();

        if (openLevels.Count > 1)
        {
            Vector3 newPosition = openLevels[openLevels.Count - 1].transform.position;
            newPosition.y += 2;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = newPosition;
        }
    }

    private void ShowAllOpenLevelDoors()
    {
        foreach (LevelDoor door in openLevels)
        {
            door.gameObject.SetActive(true);
        }
    }

    private void DisableAllLevelDoors()
    {
        foreach (LevelDoor door in levelDoors)
        {
            door.gameObject.SetActive(false);
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("highest", 1);
        SceneManager.LoadScene(0);
    }
}
