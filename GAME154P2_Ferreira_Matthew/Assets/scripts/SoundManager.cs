using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    static SoundManager _instance = null;

    public AudioClip Music;
    public AudioClip GameOverMusic;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    bool musicchange;
    // Use this for initialization
    void Start()
    {

        if (_instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Title" && musicchange == false)
        {
            musicSource.clip = Music;
            musicSource.Play();
            musicchange = true;
        }

        if(SceneManager.GetActiveScene().name == "Score Screen" && musicchange == true)
        {
            musicSource.clip = GameOverMusic;
            musicSource.Play();
            musicchange = false;
        }
    }

    public void playSingleSound(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public static SoundManager instance
    {
        get { return _instance; }
    }
}
