using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controlGuide_K;
    public GameObject controlGuide_C;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Controls()
    {
        controlGuide_K.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitConrols()
    {
        controlGuide_C.SetActive(false);
        controlGuide_K.SetActive(false);
    }

    public void SwitchGuideC()
    {
        controlGuide_C.SetActive(true);
        controlGuide_K.SetActive(false);
    }

    public void SwitchGuideK()
    {
        controlGuide_K.SetActive(true);
        controlGuide_C.SetActive(false);
    }
}
