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
        SongEntryGameObject = serializedObject.FindProperty("SongEntryGameObject");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(SongEntryGameObject);

        serializedObject.ApplyModifiedProperties();
    }
}
