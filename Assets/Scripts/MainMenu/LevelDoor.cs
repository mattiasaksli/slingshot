using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public string LevelToLoad;
    public AudioClipGroup AudioLevelSelect;

    private bool inputUnlocked = false;
    private SpriteRenderer keySprite;
    private SceneLoader sceneLoader;
    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        keySprite = Player.GetComponentsInChildren<SpriteRenderer>()[1];
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        keySprite.enabled = false;
    }


    void Update()
    {
        if (inputUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.E) && !Player.GetComponentInChildren<PlayerController>().IsInputLocked)
            {
                sceneLoader.SceneName = LevelToLoad;
                sceneLoader.LoadSceneAsync();

                GameEventMessage.SendEvent("GoToLevel");
                keySprite.enabled = false;
                Player.GetComponentInChildren<PlayerController>().LockInput();
                AudioLevelSelect?.Play();
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
