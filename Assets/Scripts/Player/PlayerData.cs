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
    public List<ActiveAbility> equippedAbilities;

    public bool isDead = false;
    public PlayerController pc;

    [Header("UI elements")]
    public HpPanel hpPanel;
    public GameObject pfDmgPopup;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        stats.hp = stats.maxHp;
        isDead = false;

        hpPanel.UpdateFillAmount((1f * stats.hp) / stats.maxHp);
    }

    public void CheckHp()
    {
        isDead = (stats.hp <= 0);

        if (isDead)
        {
            pc.Die();
        }
    }

    public void ReceiveDamage(int value)
    {
        stats.hp = Mathf.Clamp(stats.hp - value, 0, stats.maxHp);

        hpPanel.UpdateFillAmount((1f * stats.hp) / stats.maxHp);
        CreatePopupText(value, Color.red);
        CheckHp();
    }

    public void ReceiveHp(int value)
    {
        stats.hp = Mathf.Clamp(stats.hp + value, 0, stats.maxHp);

        hpPanel.UpdateFillAmount((1f * stats.hp) / stats.maxHp);
        CreatePopupText(value, Color.green);
        CheckHp();
    }

    public void CreatePopupText(int value, Color color)
    {
        Vector3 spriteExtents = pc.GetComponent<SpriteRenderer>().bounds.extents;
        Vector3 pos = new Vector3(pc.transform.position.x, pc.transform.position.y + spriteExtents.y, pc.transform.position.z);
        GameObject popupText = Instantiate(pfDmgPopup, pos, Quaternion.identity);
        popupText.GetComponent<PopupText>().Setup(value.ToString(), color);
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

        // Resize equipped abilities list if size != 4
        if (equippedAbilities.Count < 4)
        {
            while (equippedAbilities.Count < 4)
            {
                equippedAbilities.Add(new ActiveAbility());
            }
        }
        else if (equippedAbilities.Count > 4)
        {
            for (int i = equippedAbilities.Count - 1; i > 3; --i)
            {
                equippedAbilities.RemoveAt(i);
            }
        }
    }
}
