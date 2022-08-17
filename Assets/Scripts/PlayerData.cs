using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStats
{
    public int maxHp;
    public int hp;
    public int atk;
}

public class PlayerData : MonoBehaviour
{
    #region Singleton Patter
    private static PlayerData _instance;
    public static PlayerData Instance {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("null Singleton PlayerData");
            }
            return _instance;
        }
    }
    #endregion

    public CharacterStats stats;
    public List<Ability> abilities;

    public bool isDead = false;
    public PlayerController pc;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        stats.hp = stats.maxHp;
        isDead = false;
    }

    public void CheckHp()
    {
        isDead = (stats.hp <= 0);
    }

    public void ReceiveDamage(int value)
    {
        stats.hp = Mathf.Clamp(stats.hp - value, 0, stats.maxHp);
        CheckHp();
    }

    public void ReceiveHp(int value)
    {
        stats.hp = Mathf.Clamp(stats.hp + value, 0, stats.maxHp);
        CheckHp();
    }

    public void SetPlayerController(PlayerController _playerController)
    {
        pc = _playerController;
    }


    // Validate value in Unity Inspector
    private void OnValidate()
    {
        if (stats.maxHp <= 0) stats.maxHp = 1;
        stats.atk = Mathf.Abs(stats.atk);
    }

}
