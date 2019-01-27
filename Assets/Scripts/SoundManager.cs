
using System.Collections.Generic;
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
    private readonly List<AudioSource> mSources = new List<AudioSource>();

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

    void Update()
    {
        int n = mSources.Count;
        while (n-- > 0) {
            var source = mSources[n];
            if (!source.isPlaying) {
                mSources.RemoveAt(n);
                Destroy(source);
            }
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

    public void PlaySound(AudioClip clip)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.PlayOneShot(clip);
        mSources.Add(source);
    }
}
