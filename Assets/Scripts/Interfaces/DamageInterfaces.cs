using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void ReceiveDamage(int dmg);
}

public interface IKillable
{
    public void Kill();
}

public interface ISpeedChange
{
    public void ChangeSpeedModifier(float duration, float amount);
}

public interface IBossAttack
{
    public void Activate();
}