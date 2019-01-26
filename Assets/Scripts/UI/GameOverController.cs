
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameOverController : MonoBehaviour
{
    public void OnPlayAgain()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
