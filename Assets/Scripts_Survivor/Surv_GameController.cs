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
    public Surv_EnemySpawner spawner;
    public Surv_PlayerController player;

    [Header("Game Stats")]
    [Tooltip("the number of seconds player has survived.")] public float elapsedTime;

    [Header("UI elements")]
    public TextMeshProUGUI timeText;

    private void Start()
    {
        ResetVariables();
        UpdateElapsedTimeUI();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateElapsedTimeUI();
    }

    public void ResetVariables()
    {
        elapsedTime = 0f;
    }

    public void UpdateElapsedTimeUI()
    {
        string text = string.Format("{0}:{1:D2}", (int)(elapsedTime / 60), (int)(elapsedTime % 60));
        timeText.text = text;
    }
}
