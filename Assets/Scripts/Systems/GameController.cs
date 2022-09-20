using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameState
{
    Gameplay,
    Pause,
    Gameover
}

public class GameController : MonoBehaviour
{
    #region Singleton Pattern
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
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public static GameState state;

    public string pauseScene = "PauseScene";
    public string playerMenuScene = "PlayerMenuScene";
    public string activeMenu;


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Gameplay;

        activeMenu = "";
    }

    public void PauseGame()
    {
        activeMenu = pauseScene;
        LoadMenuScene(pauseScene);
    }

    public void ResumeGame()
    {
        UnloadMenuScene(activeMenu);
        activeMenu = "";
    }

    public void OpenPlayerMenu()
    {
        if (PlayerData.Instance.isDead) return;

        activeMenu = playerMenuScene;
        LoadMenuScene(playerMenuScene);
    }

    public void LoadMenuScene(string sceneToLoad)
    {
        PlayerData.Instance.pc.DisableInput();
        Time.timeScale = 0f;
        state = GameState.Pause;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    public void UnloadMenuScene(string sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
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
                if (activeMenu != "")
                    ResumeGame();
            }
        }
    }

    public void OnPlayerMenu(InputValue value)
    {
        if (PlayerData.Instance.isDead) return;

        float input = value.Get<float>();
        if (input > 0.5f)
        {
            if (state == GameState.Gameplay)
            {
                OpenPlayerMenu();
            }
            else if (state == GameState.Pause)
            {
                if (activeMenu == playerMenuScene)
                    ResumeGame();
            }
        }
    }
}
