using UnityEditor;
using DancingICE.Modes;

namespace DancingICEEditor
{
    [CustomEditor(typeof(Mode), true)]
    public class ModeEditor : Editor
    {
        SerializedProperty useCustomName;
        SerializedProperty customName;
        SerializedProperty gameScene;
        SerializedProperty analyzeAudioSpectrum;
        SerializedProperty mesureCalories;

        protected void OnEnable()
        {
            useCustomName = serializedObject.FindProperty("useCustomName");
            customName = serializedObject.FindProperty("customName");
            gameScene = serializedObject.FindProperty("gameScene");
            analyzeAudioSpectrum = serializedObject.FindProperty("analyzeAudioSpectrum");
            mesureCalories = serializedObject.FindProperty("mesureCalories");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(useCustomName);
            if (useCustomName.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(customName);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.PropertyField(gameScene);
            EditorGUILayout.PropertyField(analyzeAudioSpectrum);
            EditorGUILayout.PropertyField(mesureCalories);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
