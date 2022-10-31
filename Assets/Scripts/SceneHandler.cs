using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void start_game()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void main_menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quit_game()
    {
        Application.Quit();
    }
}
