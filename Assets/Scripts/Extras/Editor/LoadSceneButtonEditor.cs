using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.UI;
using UnityEngine.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(LoadSceneButton), true)]
public class LoadSceneButtonEditor : ButtonEditor
{
    SerializedProperty ScenePath;
    int selected = 0;
    List<string> options = new List<string>();

    protected override void OnEnable()
    {
        base.OnEnable();
        ScenePath = serializedObject.FindProperty("ScenePath");        
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            options.Add(scene.path);
        }
        selected = options.FindIndex((string str) => str == ScenePath.stringValue);
        selected = selected >= 0 ? selected : 0;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();

        selected = EditorGUILayout.Popup("Scene To Load", selected, options.ToArray());
        ScenePath.stringValue = options[selected];

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }
}
