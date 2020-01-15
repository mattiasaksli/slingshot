using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public AudioClipGroup AudioWin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioWin?.Play();

            SceneLoader sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            //Debug.Log("currentsceneindex:" + currentSceneIndex);
            if (SceneManager.sceneCountInBuildSettings - 1 != currentSceneIndex) // if we are not on the last level
            {
                int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
                PlayerPrefs.SetInt("highest", nextLevelIndex);
                //Debug.Log("nextsceneindex:" + nextLevelIndex);

                sceneLoader.SceneBuildIndex = nextLevelIndex;
                sceneLoader.AllowSceneActivation = false;
                sceneLoader.LoadSceneAsync();

                GameEventMessage.SendEvent("GoToNextLevel");
            }
            else    // if we are on the last level
            {
                //Debug.Log("going to main menu");
                sceneLoader.SceneBuildIndex = 0;
                sceneLoader.AllowSceneActivation = false;
                sceneLoader.LoadSceneAsync();

                GameEventMessage.SendEvent("GoToNextLevel");
            }
        }
    }
}
