
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameUIController : MonoBehaviour
{
    public GameObject PauseMenu;
    private float mSavedTimeScale = 1.0f;

    void Awake()
    {
        TimeManager.IsPaused = false;
        Time.timeScale = 1.0f;
        mSavedTimeScale = 1.0f;
        PauseMenu.SetActive(false);
    }

    public void OnPause()
    {
        TimeManager.IsPaused = true;
        mSavedTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
        PauseMenu.SetActive(true);
    }

    public void OnResume()
    {
        TimeManager.IsPaused = false;
        Time.timeScale = mSavedTimeScale;
        mSavedTimeScale = 1.0f;
        PauseMenu.SetActive(false);
    }

    public void OnPlayAgain()
    {
        if (TimeManager.IsPaused) {
            TimeManager.IsPaused = false;
            Time.timeScale = mSavedTimeScale;
            mSavedTimeScale = 1.0f;
        }
        SceneManager.LoadScene("GamePlay");
    }
}
