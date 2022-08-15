using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    public List<GameObject> UI_abilitySlots;

    private PlayerCombat pCombat;

    private void Start()
    {
        pCombat = PlayerData.Instance.pc.GetComponent<PlayerCombat>();
    }

    public void UpdateAbilityUI()
    {
        
    }
}
