using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Surv_GameController : MonoBehaviour
{
    #region Singleton Pattern
    private static Surv_GameController _instance;
    public static Surv_GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("null Singleton Surv_GameController");
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

    [Header("Game Systems")]
    public bool useMultiThread;
    public Surv_EnemySpawner spawner;
    public Surv_PlayerController player;

    [Header("Game Stats")]
    [Tooltip("the number of seconds player has survived.")] public float elapsedTime;
    public GameState state;

    [Header("UI elements")]
    public TextMeshProUGUI timeText;
    public GameObject bgPanel;
    public GameObject lvUpPanel;

    private void Start()
    {
        CloseAllMenuPanel();
        ResetVariables();
        UpdateElapsedTimeUI();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Gameplay:
                elapsedTime += Time.deltaTime;
                UpdateElapsedTimeUI();
                break;

            case GameState.Gameover:
                break;

            case GameState.Pause:
                break;
        }
    }

    public void PlayerLevelUp()
    {
        Time.timeScale = 0f;
        bgPanel.SetActive(true);
        lvUpPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        CloseAllMenuPanel();
        Time.timeScale = 1f;
    }

    public void CloseAllMenuPanel()
    {
        lvUpPanel.SetActive(false);
        bgPanel.SetActive(false);
    }

    public void GameOver()
    {
        state = GameState.Gameover;
    }

    public void ResetVariables()
    {
        state = GameState.Gameplay;
        elapsedTime = 0f;
    }

    public void UpdateElapsedTimeUI()
    {
        string text = string.Format("{0}:{1:D2}", (int)(elapsedTime / 60), (int)(elapsedTime % 60));
        timeText.text = text;
    }
}
