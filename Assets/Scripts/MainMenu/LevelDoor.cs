using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using Doozy.Engine.UI;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    public string LevelToLoad;

    private bool inputUnlocked = false;
    private SpriteRenderer keySprite;
    private UIView fadeView;
    private SceneLoader sceneLoader;

    void Start()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        keySprite = PlayerObject.GetComponentsInChildren<SpriteRenderer>()[1];

        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        keySprite.enabled = false;

        GameEventMessage.SendEvent("LockInput");
    }


    void Update()
    {
        if (inputUnlocked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                sceneLoader.SceneName = LevelToLoad;
                sceneLoader.LoadSceneAsync();

                GameEventMessage.SendEvent("LockInput");
                fadeView.Hide();
                keySprite.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            fadeView = GameObject.FindGameObjectWithTag("FadeView").GetComponent<UIView>();
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
