using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    void Awake()
    {
        instance = this;
    }

    //Sound Effects
    public AudioClip jump, run, attack, pickup,
        shootAR, shootHandgun, hurt, dead, respawn;
    //Music
    public AudioClip music;
    //Current Music Object
    public GameObject currentMusicObject;

    //Sound Object
    public GameObject soundObject;

    public void PlaySFX(string sfxName)
    {
        switch (sfxName)
        {
            case "jump":
                SoundObjectCreation(jump);
                break;
            case "pickup":
                SoundObjectCreation(pickup);
                break;
            case "running":
                SoundObjectCreation(run);
                break;
            default:
                break;
        }
    }

    void SoundObjectCreation(AudioClip clip)
    {
        //Create SoundObject gameobject
        GameObject newObject = Instantiate(soundObject, transform);
        //Assign aduioclip to its audiosource
        newObject.GetComponent<AudioSource>().clip = clip;
        //Play the audio
        newObject.GetComponent<AudioSource>().Play();
    }

    public void PlayMusic(string musicName)
    {
        switch (musicName)
        {
            case "Level1":
                MusicObjectCreation(music);
                break;
            default:
                break;
        }
    }

    void MusicObjectCreation(AudioClip clip)
    {
        //Check if there is an existing music object, if so delete it
        if (currentMusicObject)
        {
            Destroy(currentMusicObject);
        }
        //Create SoundObject gameobject
        currentMusicObject = Instantiate(soundObject, transform);
        //Assign aduioclip to its audiosource
        currentMusicObject.GetComponent<AudioSource>().clip = clip;
        //Make the audio source looping
        currentMusicObject.GetComponent<AudioSource>().loop = true;
        //Play the audio
        currentMusicObject.GetComponent<AudioSource>().Play();
    }

}
