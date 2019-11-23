using Doozy.Engine.SceneManagement;
using Doozy.Engine.UI;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        sceneLoader.LoadSceneAsync();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("FadeView").GetComponent<UIView>().Hide();
        }
    }
}
