using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public                      List <AudioSettings> AudioBasa;
    public                      List <EnvironmentSounds> AudioPlayer;
    private static              List <AudioSettings> ShadowAudioBasa;
    private static              List <EnvironmentSounds> ShadowAudioPlayer;
    private static              Transform MyPos;
    /////////////////////////////////////////////////////////////
    [SerializeField] private    Transform ZombiePos;
    private                     AudioSource ZombieSounds;
    /////////////////////////////////////////////////////////////
    public static               bool CheckSoundState;

    void Awake()
    {
        CheckSoundState = AudioListener.pause;
        ShadowAudioBasa = AudioBasa;
        ShadowAudioPlayer = AudioPlayer;
        MyPos = transform;
        if(CheckSoundState)
        {
            for (int i = 0; i < ShadowAudioPlayer.Count; i++)
            {

                ShadowAudioPlayer[i].AudioPlayer.Stop();
            }
        }
        ZombieSounds = AudioManager.GetAudioPlayer("ZombieSound");
    }
    public static void PlayEnvironment(string SoundName, bool Switch)
    {
        for (int i = 0; i < ShadowAudioPlayer.Count; i++)
        {
            if(ShadowAudioPlayer[i].Name == SoundName)
            {
                if(Switch)
                {
                    ShadowAudioPlayer[i].AudioPlayer.Play();
                }
                else
                {
                    ShadowAudioPlayer[i].AudioPlayer.Stop();
                }
            }
        }
    }

    public static AudioSource GetAudioPlayer(string Name)
    {
        AudioSource NewPlayer = new AudioSource();

        for (int i = 0; i < ShadowAudioPlayer.Count; i++)
        {
            if(ShadowAudioPlayer[i].Name == Name)
            {
                NewPlayer = ShadowAudioPlayer[i].AudioPlayer;
            }
        }

        return NewPlayer;
    }
    public static void PlaySound(string SoundName)
    {
        if(!CheckSoundState)
        {
            GameObject SoundMaster = new GameObject();
            SoundMaster.name = SoundName;
            SoundMaster.transform.SetParent(MyPos);
            var NewSound = SoundMaster.AddComponent<AudioSource>();
            for (int i = 0; i < ShadowAudioBasa.Count; i++)
            {
                if(ShadowAudioBasa[i].Name == SoundName)
                {
                    NewSound.volume = ShadowAudioBasa[i].Volume;
                    NewSound.clip = ShadowAudioBasa[i].AudioClips[Random.Range(0, ShadowAudioBasa[i].AudioClips.Length)];
                    NewSound.Play();
                    Destroy(SoundMaster, NewSound.clip.length);
                    break;
                }
            }
        }
    }
}


[System.Serializable]
public class AudioSettings
{
    public string Name;
    [Range(0,1) ]public float Volume; 
    public AudioClip[] AudioClips;
}

[System.Serializable]
public class EnvironmentSounds
{
    public string Name;
    public AudioSource AudioPlayer;
}