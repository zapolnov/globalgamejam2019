
using UnityEngine;
using UnityEngine.UI;

public sealed class HomeSweetHome : MonoBehaviour
{
    public SpriteRenderer Empty;
    public SpriteRenderer Full;
    public Player Player;
    public CameraController CameraController;
    public GameObject CameraTarget;
    public Button PauseButton;
    public Button PlayAgainButton;

    public float TransitionTime = 0.5f;

    private bool mTransitioning;
    private float mTransitionTime;

    void Update()
    {
        if (!mTransitioning)
            return;

        mTransitionTime += TimeManager.unscaledDeltaTime;
        if (mTransitionTime >= TransitionTime) {
            Time.timeScale = 1.0f;
            mTransitioning = false;
            Full.gameObject.SetActive(true);
            Empty.gameObject.SetActive(false);
            Player.gameObject.SetActive(false);
            PlayAgainButton.gameObject.SetActive(true);
        } else {
            Time.timeScale = 0.0f;
            Full.gameObject.SetActive(true);
            Empty.gameObject.SetActive(true);
            Player.gameObject.SetActive(true);

            var color = Full.color;
            color.a = mTransitionTime / TransitionTime;
            Full.color = color;

            color = Empty.color;
            color.a = 1.0f - mTransitionTime / TransitionTime;
            Empty.color = color;

            Player.SetAlpha(1.0f - Mathf.Clamp(2.0f * mTransitionTime / TransitionTime, 0.0f, 1.0f));
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player") {
            mTransitioning = true;
            CameraController.Target = CameraTarget;
            CameraController.Speed = 4.0f;
            Player.IgnoreInput = true;
            PauseButton.gameObject.SetActive(false);
            PlayAgainButton.gameObject.SetActive(false);
            SoundManager.Instance.SetMusic(SoundManager.AudioTrack.Win);
        }
    }
}
