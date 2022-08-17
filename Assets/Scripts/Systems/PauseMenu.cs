using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    public void Resume()
    {
        gc.ResumeGame();
    }

    public void Quit()
    {
        gc.QuitGame();
    }
}
