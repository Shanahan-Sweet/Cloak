using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public enum AudioChanel { Game, SFX, Music }

    AudioSource[] musicSources;//num 2 is ambience
    int activeMusicSourceIndex;

    //static reference
    public static AudioManager instance;

    //IEnumerator
    private IEnumerator crossFadeCoroutine;//, fadeAmbienceRoutine;

    //variables
    [SerializeField] float spatialBlend = .4f;

    //volume refrence
    public int gameVolumeInt;
    public float gameVolumePercent;
    public float musicVolumePercent;
    public float sfxVolumePercent;

    //Mixers
    [SerializeField] AudioMixer masterMixer;

    //mixergroups
    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerGroup sfxMixer;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //load
            if (!PlayerPrefs.HasKey("GameVolumeInt"))
            {
                //PlayerPrefs.SetFloat("GameVolume", .7f);
                PlayerPrefs.SetInt("GameVolumeInt", 3);//0 1 2 3 4
                PlayerPrefs.SetFloat("MusicVolume", 1);
                PlayerPrefs.SetFloat("SfxVolume", 1);
            }

            gameVolumeInt = PlayerPrefs.GetInt("GameVolumeInt");
            musicVolumePercent = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolumePercent = PlayerPrefs.GetFloat("SfxVolume");
            //force set
            /*
            SetVolume(gameVolumePercent, AudioChanel.Game);
            SetVolume(musicVolumePercent, AudioChanel.Music);
            SetVolume(sfxVolumePercent, AudioChanel.SFX);
            */



            //___________

            musicSources = new AudioSource[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject newMusicSource = new GameObject("MusicSource" + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                musicSources[i].loop = true;
                musicSources[i].transform.parent = transform;
                musicSources[i].outputAudioMixerGroup = musicMixer;
                musicSources[i].priority = 0;
            }

            //coroutines
            crossFadeCoroutine = MusicCrossFade(1, false);

        }
    }

    private void Start()
    {
        //set
        SetVolume(gameVolumeInt, AudioChanel.Game);
        SetVolume(musicVolumePercent, AudioChanel.Music);
        SetVolume(sfxVolumePercent, AudioChanel.SFX);
    }

    public void SetVolume(float volumePercent, AudioChanel channel)//change volume through sliders
    {
        //print("set volume" + channel);
        switch (channel)
        {
            case AudioChanel.Game:
                gameVolumeInt = (int)volumePercent;//0 1 2 3 4
                gameVolumePercent = (float)gameVolumeInt * .25f;
                masterMixer.SetFloat("MasterVolume", Mathf.Max(-79, Mathf.Log(gameVolumePercent) * 20));
                PlayerPrefs.SetInt("GameVolumeInt", gameVolumeInt);//save
                break;
            case AudioChanel.SFX:
                sfxVolumePercent = volumePercent;
                masterMixer.SetFloat("SfxVolume", Mathf.Max(-79, Mathf.Log(volumePercent) * 20));
                PlayerPrefs.SetFloat("SfxVolume", volumePercent);//save
                break;
            case AudioChanel.Music:
                musicVolumePercent = volumePercent;
                masterMixer.SetFloat("MusicVolume", Mathf.Max(-79, Mathf.Log(volumePercent) * 20));
                PlayerPrefs.SetFloat("MusicVolume", volumePercent);//save
                break;
        }
    }

    public void StartMusic(AudioClip clip)
    {
        PlayMusic(clip, .1f, false);
    }

    public void PlayMusic(AudioClip clip, float fadeSpd, bool syncMusic)
    {
        if (clip != null)
        {
            if (clip != musicSources[activeMusicSourceIndex].clip)//not same clip
            {
                activeMusicSourceIndex = 1 - activeMusicSourceIndex;

                musicSources[activeMusicSourceIndex].volume = 0;
                musicSources[activeMusicSourceIndex].clip = clip;

                if (musicSources[activeMusicSourceIndex].isPlaying == false)
                {
                    //musicSources[activeMusicSourceIndex].timeSamples = 0;
                    musicSources[activeMusicSourceIndex].Play();
                }
                musicSources[activeMusicSourceIndex].volume = 0;

                //coroutines
                StopCoroutine(crossFadeCoroutine);
                crossFadeCoroutine = MusicCrossFade(fadeSpd, syncMusic);
                StartCoroutine(crossFadeCoroutine);
            }
        }
        else//stop music
        {
            activeMusicSourceIndex = 1 - activeMusicSourceIndex;
            musicSources[activeMusicSourceIndex].clip = null;
            musicSources[activeMusicSourceIndex].Stop();
            musicSources[activeMusicSourceIndex].volume = 0;

            //coroutines
            StopCoroutine(crossFadeCoroutine);
            crossFadeCoroutine = MusicCrossFade(fadeSpd, syncMusic);
            StartCoroutine(crossFadeCoroutine);
        }
    }

    /*public void PlayAmbience(float targetVolume, AudioClip clip = null, float spd = 2)
    {
        if (clip != null)//change clip
        {
            musicSources[2].clip = clip;
        }

        //coroutines
        if (fadeDrumsRoutine != null)
        {
            StopCoroutine(fadeDrumsRoutine);
        }
        fadeDrumsRoutine = FadeDrumsRoutine(targetVolume, spd);//fade to new volume level
        StartCoroutine(fadeDrumsRoutine);
    }*/


    //play
    public void PlaySound(AudioClip clip, Vector3 pos, int priority, float pitch = 1, float volume = 1, Transform newParent = null)//spawn audio object
    {
        if (clip == null) return;

        //create audio source
        GameObject temp = new GameObject();//create audio source
        temp.AddComponent<AudioSource>();
        AudioSource tempAudio = temp.GetComponent<AudioSource>();

        //position and parent
        tempAudio.transform.position = pos + new Vector3(0, 0, -8);//set position
        if (newParent != null)
        {
            temp.transform.parent = newParent;
        }

        //set clip
        tempAudio.clip = clip;

        //effects
        tempAudio.priority = priority;
        tempAudio.spatialBlend = spatialBlend;//make 3D
        tempAudio.pitch = pitch;
        tempAudio.volume = volume;

        //volume
        tempAudio.outputAudioMixerGroup = sfxMixer;

        //play sound
        tempAudio.Play();//play
        Destroy(temp, clip.length * (1.1f + pitch));//destroy

    }

    IEnumerator MusicCrossFade(float fadeSpd, bool syncMusic)
    {
        float percent = 0;
        float startValue = musicSources[1 - activeMusicSourceIndex].volume;

        while (percent < 1)
        {
            percent += Time.unscaledDeltaTime * fadeSpd;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, 1, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(startValue, 0, percent);

            if (syncMusic == true && musicSources[1 - activeMusicSourceIndex].clip != null)//play together
            {
                musicSources[activeMusicSourceIndex].timeSamples = musicSources[1 - activeMusicSourceIndex].timeSamples;
            }

            yield return null;
        }
    }
}
