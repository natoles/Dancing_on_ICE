using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(SettingField)  , true)]  
public class SettingFieldEditor : InputFieldEditor
{
    SerializedProperty m_SettingType;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_SettingType = serializedObject.FindProperty("m_SettingType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_SettingType);

        EditorGUILayout.Space();

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}