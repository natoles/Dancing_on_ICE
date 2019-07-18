using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(SongScrollView), true)]
public class SongScrollViewEditor : ScrollRectEditor
{
    SerializedProperty SongEntryGameObject;

    protected override void OnEnable()
    {
        base.OnEnable();
        SongEntryGameObject = serializedObject.FindProperty("SongEntryModel");
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);
        
        serializedObject.Update();

        EditorGUILayout.PropertyField(SongEntryGameObject);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }
}
