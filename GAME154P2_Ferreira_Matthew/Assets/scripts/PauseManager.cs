using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour {
    CanvasGroup cg;
    public AudioClip pause;
    // Use this for initialization
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        if (!cg)
            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0.0f;
        cg.interactable = false;
    }
        // Update is called once per frame
        void Update () {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }
        }
    void PauseGame()
    {
        if (cg.alpha == 0)
        {
            SoundManager.instance.playSingleSound(pause);
            cg.alpha = 1;
            Time.timeScale = 0;
            cg.interactable = true;
        }
        else
        {
            SoundManager.instance.playSingleSound(pause);
            cg.alpha = 0;
            Time.timeScale = 1;
            cg.interactable = false;
        }


    }
}
