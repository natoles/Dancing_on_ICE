using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ModeSelectionButton), true)]
public class ModeSelectionButtonEditor : ButtonEditor
{
    SerializedProperty TextComponent;
    SerializedProperty Modes;


    protected override void OnEnable()
    {
        base.OnEnable();
        TextComponent = serializedObject.FindProperty("TextComponent");
        Modes = serializedObject.FindProperty("Modes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(TextComponent);
        //EditorGUILayout.PropertyField(Modes);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}
