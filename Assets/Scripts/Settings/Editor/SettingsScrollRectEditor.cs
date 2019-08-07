using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SettingsScrollRect), true)]
public class SettingsScrollRectEditor : ScrollRectEditor
{
    SerializedProperty prefab;
    SerializedProperty sectionTypeName;

    string[] typesName = null;
    string[] typesAssemblyQualifiedName = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        prefab = serializedObject.FindProperty("prefab");
        sectionTypeName = serializedObject.FindProperty("sectionTypeName");
        Type[] types = Array.FindAll(typeof(SettingsManager).GetNestedTypes(), type => type.GetCustomAttributes(typeof(SettingSectionAttribute), false).Length > 0);
        typesName = types.Select(type => type.Name).ToArray();
        typesAssemblyQualifiedName = types.Select(type => type.AssemblyQualifiedName).ToArray();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();

        EditorGUILayout.PropertyField(prefab);

        if (typesAssemblyQualifiedName.Length > 0)
        {
            int i = Mathf.Max(Array.IndexOf(typesAssemblyQualifiedName, sectionTypeName.stringValue), 0);
            i = EditorGUILayout.Popup("Section", i, typesName);
            sectionTypeName.stringValue = typesAssemblyQualifiedName[i];
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }
}
