using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(ModeSelectionButton), true)]
public class ModeSelectionButtonEditor : ButtonEditor
{
    SerializedProperty TargetText;
    SerializedProperty Slider;
    SerializedProperty ModeManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        TargetText = serializedObject.FindProperty("TextComponent");
        Slider = serializedObject.FindProperty("DifficultySlider");
        ModeManager = serializedObject.FindProperty("ModeManager");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();
        
        EditorGUILayout.PropertyField(TargetText);
        EditorGUILayout.PropertyField(Slider);
        EditorGUILayout.PropertyField(ModeManager);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }
}
