﻿using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(VoteEntry), true)]
public class VoteEntryEditor : ImageEditor
{
    SerializedProperty ID;
    SerializedProperty SongName;

    protected override void OnEnable()
    {
        base.OnEnable();
        ID = serializedObject.FindProperty("IDText");
        SongName = serializedObject.FindProperty("SongNameText");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(ID);
        EditorGUILayout.PropertyField(SongName);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}
