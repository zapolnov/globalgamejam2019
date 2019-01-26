
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuController : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
