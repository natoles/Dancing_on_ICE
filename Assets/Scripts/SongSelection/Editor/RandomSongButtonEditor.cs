using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(RandomSongButton), true)]
public class RandomSongButtonEditor : ButtonEditor
{
    SerializedProperty Scroll;

    protected override void OnEnable()
    {
        base.OnEnable();
        Scroll = serializedObject.FindProperty("Scroll");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(Scroll);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}
