using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_SummonedSwords : Surv_PlayerAttack
{
    public List<Surv_Sword> swords;
    public List<float> rotateSpeedAtLevel;
    private float rotateSpeed { get => rotateSpeedAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1]; }
    [Tooltip("Upgrade to 4 swords at which level")]public int levelFourSwords = 4;

    protected override void Start()
    {
        base.Start();

        foreach (var s in swords)
        {
            s.transform.parent = null;
            s.gameObject.SetActive(true);
            s.GetComponent<Surv_FollowTarget>().SetTarget(player.transform);
        }

        // level 1
        swords[0].Show();
        swords[1].Show();
        swords[2].Hide();
        swords[3].Hide();
    }

    private void OnEnable()
    {
        foreach (var s in swords)
        {
            s.transform.parent = null;
            s.gameObject.SetActive(true);
            s.Show();
        }

        if (level < levelFourSwords)
        {
            swords[2].Hide();
            swords[3].Hide();
        }
    }

    private void OnDisable()
    {
        foreach (var s in swords)
        {
            if (s == null) continue;
            s.Hide();
        }
    }

    private void Update()
    {
        foreach(var s in swords)
        {
            s.hitbox.SetDamage(damage);
            s.transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }

    public override void AttackLevelUp()
    {
        base.AttackLevelUp();

        if (level >= levelFourSwords)
        {
            swords[2].Show();
            swords[3].Show();
        }
    }

    public override void OnValidate()
    {
        base.OnValidate();

        if (rotateSpeedAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = rotateSpeedAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                rotateSpeedAtLevel.Add(90f);
            }
        }
    }
}
