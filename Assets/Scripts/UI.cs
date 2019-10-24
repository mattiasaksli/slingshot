using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance;
    public bool Win = false;
    public GameObject EndGamePanel;
    public Button EndGameButton;

    private void Awake()
    {
        Instance = this;
        EndGamePanel.gameObject.SetActive(false);
    }

    public void ShowEndGamePanel()
    {
        TextMeshProUGUI[] texts = EndGamePanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        if (Win)
        {
            texts[0].SetText("You win!");
            texts[1].SetText("Play again");
        }
        else
        {
            texts[0].SetText("You lose!");
            texts[1].SetText("Restart");
        }
        EndGameButton.onClick.AddListener(OnEndGameButtonClick);
        EndGamePanel.SetActive(true);
    }

    public void OnEndGameButtonClick()
    {
        if (Win)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
