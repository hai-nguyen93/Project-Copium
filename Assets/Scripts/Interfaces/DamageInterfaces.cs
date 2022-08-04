using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int dmg);
}

public interface IKillable
{
    public void Kill();
}
