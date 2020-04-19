using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [SerializeField]
    AudioClip EndSound;

    AudioSource audioSrc;

    [SerializeField]
    Image FadeOut;

    float triggered = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered > 0.0f)
        {
            float alpha = (Time.time - triggered) * 5.0f;
            alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
            FadeOut.color = new Color(FadeOut.color.r, FadeOut.color.g, FadeOut.color.b, alpha);
        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlaySound()
    {
        audioSrc.PlayOneShot(EndSound);
    }

    public void Trigger()
    {
        triggered = Time.time;
        Invoke("NextLevel", 3.0f);
        Invoke("PlaySound", 0.0f);
        FadeOut.gameObject.SetActive(true);
    }
}
