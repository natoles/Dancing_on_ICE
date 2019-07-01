﻿using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(SongEntry), true)]
public class SongEntryEditor : ButtonEditor
{
    SerializedProperty Thumbnail;
    SerializedProperty SongName;
    SerializedProperty Artist;
    SerializedProperty Difficulty;
    SerializedProperty Duration;

    protected override void OnEnable()
    {
        base.OnEnable();
        Thumbnail = serializedObject.FindProperty("Thumbnail");
        SongName = serializedObject.FindProperty("SongName");
        Artist = serializedObject.FindProperty("Artist");
        Difficulty = serializedObject.FindProperty("Difficulty");
        Duration = serializedObject.FindProperty("Duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(Thumbnail);
        EditorGUILayout.PropertyField(SongName);
        EditorGUILayout.PropertyField(Artist);
        EditorGUILayout.PropertyField(Difficulty);
        EditorGUILayout.PropertyField(Duration);

        EditorGUILayout.Space();

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}
