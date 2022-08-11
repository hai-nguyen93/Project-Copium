using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MyAssets/Ability")]
public class Ability : ScriptableObject
{
    public string abilityName = "New Ability";
    public float cooldown = 1f;
    public int cost = 1; // mp cost or stamina cost

    public virtual void Activate(GameObject user)
    {
        Debug.Log(user.name + " Activates " + abilityName);
    }
}
