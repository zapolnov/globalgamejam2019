
using UnityEngine;

public sealed class SoundManager : MonoBehaviour
{
    public enum AudioTrack
    {
        None,
        Main,
        Lose,
        Win,
    }

    public static SoundManager Instance { get; private set; }

    public AudioTrack Music = AudioTrack.None;
    public AudioSource MainMusic;
    public AudioSource LoseMusic;
    public AudioSource WinMusic;

    private AudioTrack mCurrentMusic = AudioTrack.None;

    void Awake()
    {
        if (Instance != null) {
            Instance.SetMusic(Music);
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetMusic(Music);
        }
    }

    public void SetMusic(AudioTrack music)
    {
        if (music != mCurrentMusic) {
            mCurrentMusic = music;
            MainMusic.Stop();
            LoseMusic.Stop();
            WinMusic.Stop();
            switch (music) {
                case AudioTrack.None: break;
                case AudioTrack.Main: MainMusic.Play(); break;
                case AudioTrack.Lose: LoseMusic.Play(); break;
                case AudioTrack.Win: WinMusic.Play(); break;
            }
        }
    }
}
