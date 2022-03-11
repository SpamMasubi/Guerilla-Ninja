using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClip levelSong, bossSong;

    private AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        PlaySong(levelSong);
    }

    private void Update()
    {
        
        if (PauseMenu.isPause)
        {
            audioS.Pause();
        }
        else
        {
            audioS.UnPause();
        }
    }

    public void StopSong()
    {
        audioS.Stop();
    }

    public void ResumeMusic()
    {
        if (BossStart.startBoss)
        {
            PlaySong(bossSong);
        }
        else
        {
            PlaySong(levelSong);
        }
    }

    public void PlaySong(AudioClip clip)
    {
        audioS.clip = clip;
        audioS.Play();
    }

}
