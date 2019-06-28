using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(SongScrollView), true)]
public class SongScrollViewEditor : ScrollRectEditor
{
    SerializedProperty m_SongEntry;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_SongEntry = serializedObject.FindProperty("m_SongEntry");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_SongEntry);

        serializedObject.ApplyModifiedProperties();
    }
}
