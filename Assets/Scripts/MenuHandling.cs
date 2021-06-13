using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandling : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas creditsCanvas;

    public void PressStartButton()
    {
        //load game scene
        SceneManager.LoadScene("GameScene");
    }

    public void PressCreditsButton()
    {
        //hide main canvas, show credits canvas
        mainCanvas.enabled = false;
        creditsCanvas.enabled = true;
    }

    public void PressExitButton()
    {
        //quit application
        Application.Quit();
    }

    public void PressReturnButton()
    {
        //hide credits canvas, show main canvas
        creditsCanvas.enabled = false;
        mainCanvas.enabled = true;
    }

    public void PressMainMenuButton()
    {
        //load start scene
        SceneManager.LoadScene("StartMenu");
    }
}
