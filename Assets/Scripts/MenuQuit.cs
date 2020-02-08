using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MenuQuit : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadScene()
    {
        Debug.Log("Load");
        SceneManager.LoadScene("Teste");
    }
}
