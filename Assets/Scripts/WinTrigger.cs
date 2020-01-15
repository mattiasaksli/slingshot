using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneLoader sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (SceneManager.sceneCountInBuildSettings - 1 != currentSceneIndex) // if we are not on the last level
            {
                int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
                PlayerPrefs.SetInt("highest", nextLevelIndex);

                sceneLoader.SceneBuildIndex = nextLevelIndex;
                sceneLoader.AllowSceneActivation = false;
                sceneLoader.LoadSceneAsync();

                GameEventMessage.SendEvent("GoToNextLevel");
            }
            else    // if we are on the last level
            {
                sceneLoader.SceneBuildIndex = 0;
                sceneLoader.AllowSceneActivation = false;
                sceneLoader.LoadSceneAsync();

                GameEventMessage.SendEvent("GoToNextLevel");
            }
        }
    }
}
