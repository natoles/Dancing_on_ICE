using UnityEditor;
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
        SongName = serializedObject.FindProperty("SongNameText");
        Artist = serializedObject.FindProperty("ArtistText");
        Difficulty = serializedObject.FindProperty("DifficultyText");
        Duration = serializedObject.FindProperty("DurationText");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();

        EditorGUILayout.PropertyField(Thumbnail);
        EditorGUILayout.PropertyField(SongName);
        EditorGUILayout.PropertyField(Artist);
        EditorGUILayout.PropertyField(Difficulty);
        EditorGUILayout.PropertyField(Duration);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }
}
