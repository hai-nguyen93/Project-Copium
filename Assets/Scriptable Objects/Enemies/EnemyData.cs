using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "MyAssets/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "New Enemy";
    public int maxHp = 5;
    public int atk = 1;
}
