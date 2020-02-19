using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    [System.Serializable]
    public class Stem
    {
        public AudioSource source;
        public AudioClip clip;
        public float startingSpeedRatio;
    }
    #region Singleton
    static protected MusicPlayer s_Instance;
    static public MusicPlayer instance { get { return s_Instance; } }
    #endregion
    public UnityEngine.Audio.AudioMixer mixer;
    public Stem[] stems;
    public float maxVolume = 0.1f;
    private int numberSoundWinGame = 1;
    private int numberSoundOverGame = 2;

    void Awake()
    {
        #region Singleton
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
        #endregion
        Application.targetFrameRate = 30;
        AudioListener.pause = false;
    }

    void Start()
    {
        SetVolumes(SaveLoad.GetInstance().pData.musicValue);
        AudioListener.pause = SaveLoad.GetInstance().pData.musicMute;
    }

    public void PlayFirstSound()
    {
        Play(0);
    }
    public void PlayWinSound()
    {
        Play(numberSoundWinGame);
    }
    public void PlayOverGameSound()
    {
        Play(numberSoundOverGame);
    }

    private void Play(int numberSound)
    {
        if (stems[0] != null && stems[0].source != null)
        {
            stems[0].source.clip = stems[numberSound].clip;
            stems[0].source.Play();
        }
    }


    public void StopFirstSound()
    {
        if (stems[0] != null && stems[0].source != null)
        {
            stems[0].source.Stop();
        }
    }

    public void SetStem(int index, AudioClip clip)
    {
        if (stems.Length <= index)
        {
            return;
        }

        stems[index].clip = clip;
    }

    public AudioClip GetStem(int index)
    {
        return stems.Length <= index ? null : stems[index].clip;
    }

    public IEnumerator RestartAllStems()
    {
        for (int i = 0; i < stems.Length; ++i)
        {
            stems[i].source.clip = stems[i].clip;
            stems[i].source.volume = 0.0f;
            stems[i].source.Play();
        }

        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < stems.Length; ++i)
        {
            stems[i].source.volume = stems[i].startingSpeedRatio <= 0.0f ? maxVolume : 0.0f;
        }
    }

    public void UpdateVolumes(float currentSpeedRatio)
    {
        const float fadeSpeed = 0.5f;

        for (int i = 0; i < stems.Length; ++i)
        {
            float target = currentSpeedRatio >= stems[i].startingSpeedRatio ? maxVolume : 0.0f;
            stems[i].source.volume = Mathf.MoveTowards(stems[i].source.volume, target, fadeSpeed * Time.deltaTime);
        }
    }

    public void SetVolumes(float currentSpeedRatio)
    {
        for (int i = 0; i < stems.Length; ++i)
        {
            stems[i].source.volume = currentSpeedRatio;
        }
    }
}
