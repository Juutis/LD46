using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DogUI : MonoBehaviour
{
    [SerializeField]
    GameObject Dead;

    [SerializeField]
    GameObject Menu;

    [SerializeField]
    GameObject SausageLove;

    [SerializeField]
    GameObject CakeLove;

    [SerializeField]
    GameObject Win;

    [SerializeField]
    AudioClip plop;

    AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Menu.activeInHierarchy || Dead.activeInHierarchy)
            {
                Restart();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Win.activeInHierarchy)
            {
                Application.Quit();
            }
            else if (Menu.activeInHierarchy)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Menu.activeInHierarchy)
            {
                Application.Quit();
            }
        }
    }

    public void ShowCakeLove()
    {
        if (!Menu.activeInHierarchy && !Dead.activeInHierarchy)
        {
            CakeLove.SetActive(true);
            Invoke("HideCakeLove", 3.0f);
            audioSrc.PlayOneShot(plop);
        }
    }

    public void HideCakeLove()
    {
        CakeLove.SetActive(false);
    }

    public void ShowSausageLove()
    {
        if (!Menu.activeInHierarchy && !Dead.activeInHierarchy)
        {
            SausageLove.SetActive(true);
            Invoke("HideSausageLove", 3.0f);
            audioSrc.PlayOneShot(plop);
        }
    }

    public void HideSausageLove()
    {
        SausageLove.SetActive(false);
    }

    public void ShowDead()
    {
        Dead.SetActive(true);
        HideMenu();
        HideCakeLove();
        HideSausageLove();
        audioSrc.PlayOneShot(plop);
    }

    public void HideDead()
    {
        Dead.SetActive(false);
    }

    public void ShowMenu()
    {
        Menu.SetActive(true);
        HideCakeLove();
        HideSausageLove();
        HideDead();
        audioSrc.PlayOneShot(plop);
    }

    public void HideMenu()
    {
        Menu.SetActive(false);
    }

    public void ShowWin()
    {
        if (!Win.activeInHierarchy)
        {
            Win.SetActive(true);
            HideCakeLove();
            HideSausageLove();
            HideDead();
            HideMenu();
            audioSrc.PlayOneShot(plop);
        }
    }

    public void HideWin()
    {
        Win.SetActive(false);
    }
}
