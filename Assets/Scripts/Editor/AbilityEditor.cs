using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ability), true)]
public class AbilityEditor : Editor
{
    Ability ability;

    private void OnEnable()
    {
        ability = target as Ability;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (ability.abilityIcon == null) return;

        Texture2D texture = AssetPreview.GetAssetPreview(ability.abilityIcon);
        GUILayout.Label("Icon Preview", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
