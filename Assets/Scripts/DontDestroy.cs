using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    bool mute = false;

    AudioSource music;

    float origVolume;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
        music = GetComponent<AudioSource>();
        origVolume = music.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            if (mute)
            {
                music.volume = origVolume;
            }
            else
            {
                music.volume = 0.0f;
            }
            mute = !mute;
        }
    }
}
