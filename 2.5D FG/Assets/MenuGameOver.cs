using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void VoltarAoMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!!!");
        Application.Quit();
    }

}
