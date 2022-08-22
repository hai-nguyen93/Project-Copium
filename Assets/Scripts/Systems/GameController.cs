using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameState
{
    Gameplay,
    Pause
}

public class GameController : MonoBehaviour
{
    #region Singleton Patter
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("null Singleton GameController");
            }
            return _instance;
        }
    }
    #endregion

    public static GameState state;

    public string pauseScene = "PauseScene";


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Gameplay;
    }

    public void PauseGame()
    {
        PlayerData.Instance.pc.DisableInput();
        Time.timeScale = 0f;
        state = GameState.Pause;
        SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync(pauseScene);
        Time.timeScale = 1f;
        state = GameState.Gameplay;
        if (!PlayerData.Instance.isDead) PlayerData.Instance.pc.EnableInput();
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit Game.");
        Application.Quit();
    }

    public void OnPause(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            if (state == GameState.Gameplay)
            {
                PauseGame();
            }
            else if (state == GameState.Pause)
            {
                ResumeGame();
            }
        }
    }
}
