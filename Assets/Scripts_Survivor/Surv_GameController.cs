using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public EnemyDiePSPool diePsPool;
    public Surv_PlayerController player;
    public Surv_PlayerAttackDatabase playerAttackDB;

    [Header("Game Stats")]
    [Tooltip("the number of seconds player has survived.")] public float elapsedTime;
    public int killCount = 0;
    public GameState state;

    [Header("UI elements")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killCountText;
    public GameObject bgPanel;
    public GameObject lvUpPanel;

    private void Start()
    {
        CloseAllMenuPanel();
        ResetVariables();
        UpdateElapsedTimeUI();
        UpdateKillCountUI();
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

            case GameState.CannotPause:
                break;

            case GameState.Pause:
                break;
        }
    }

    public void PlayerLevelUp()
    {
        Time.timeScale = 0f;
        state = GameState.Pause;
        bgPanel.SetActive(true);
        lvUpPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        CloseAllMenuPanel();
        state = GameState.Gameplay;
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
        killCount = 0;
        elapsedTime = 0f;
    }

    public void OnEnemyKilled(Surv_Enemy enemy)
    {
        killCount++;
        UpdateKillCountUI();

        var ps = diePsPool.Get();
        ps.transform.position = enemy.transform.position + new Vector3(0, 0.5f, 0);
        var m = ps.main;
        m.startColor = enemy.dieParticleColor;
        ps.Play();
    }

    private void UpdateKillCountUI()
    {
        killCountText.text = killCount.ToString();
    }

    public void UpdateElapsedTimeUI()
    {
        string text = string.Format("{0}:{1:D2}", (int)(elapsedTime / 60), (int)(elapsedTime % 60));
        timeText.text = text;
    }
}
