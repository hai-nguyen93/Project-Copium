using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CastType { instant, channeling};

[CreateAssetMenu(menuName ="MyAssets/AbilityBase")]
public class Ability : ScriptableObject
{
    public string abilityName = "New Ability";
    public float cooldown = 1f;
    public int cost = 1; // mp cost or stamina cost
    public CastType castType = CastType.instant;
    public float castTime = 1f;
    public Sprite abilityIcon;

    public bool selfAnimation = false;
    public string animationName;

    public virtual void Activate(GameObject user)
    {
        Debug.Log(user.name + " Activates " + abilityName);

        if (selfAnimation)
        {
            user.GetComponent<Animator>().Play(animationName);
        }
    }
}
