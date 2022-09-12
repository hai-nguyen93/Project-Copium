using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MyAssets/Key Item")]
public class KeyItem : ScriptableObject
{
    public string iName = "New key item";
    public string description = "item description";
    public Sprite icon;
}
