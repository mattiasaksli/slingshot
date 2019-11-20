using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public String LevelToLoad;

    private bool inputUnlocked = false;
    private SpriteRenderer keySprite;
    private PlayerController Player;
    private UIFade Fade;

    void Start()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        Player = PlayerObject.GetComponent<PlayerController>();
        keySprite = PlayerObject.GetComponentsInChildren<SpriteRenderer>()[1];
        keySprite.enabled = false;
        Fade = FindObjectOfType<UIFade>();
    }


    void Update()
    {
        if (inputUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.IsInputLocked = true;
                keySprite.enabled = false;
                Fade.AddCallback(() => SceneManager.LoadScene(LevelToLoad));
                Fade.FadeIn(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inputUnlocked = true;
            keySprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inputUnlocked = false;
            keySprite.enabled = false;
        }
    }
}
