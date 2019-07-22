using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ModeSelectionButton), true)]
public class ModeSelectionButtonEditor : ButtonEditor
{
    SerializedProperty TextComponent;
    SerializedProperty Modes;
    SerializedProperty Current;

    protected override void OnEnable()
    {
        base.OnEnable();
        TextComponent = serializedObject.FindProperty("TextComponent");
        Modes = serializedObject.FindProperty("Modes");
        Current = serializedObject.FindProperty("current");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();

        EditorGUILayout.PropertyField(TextComponent);
        ShowList(Modes);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }

    private void ShowList(SerializedProperty list)
    {
        EditorGUILayout.LabelField(list.name);

        SerializedProperty size = list.FindPropertyRelative("Array.size");
        if (size.hasMultipleDifferentValues)
            EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
        else
            ShowElements(list);
    }

    private void ShowElements(SerializedProperty list)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(20f));
        EditorGUILayout.LabelField("Name", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("Buttons", EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("", GUILayout.Width(40f));
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < list.arraySize; i++)
        {
            SerializedProperty element = list.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.Toggle(Current.intValue == i, EditorStyles.radioButton, GUILayout.Width(20f)))
            {
                Current.intValue = i;
                ((ModeSelectionButton)target).UpdateModeDisplay();
            }

            SerializedProperty Name = element.FindPropertyRelative("name");
            SerializedProperty Buttons = element.FindPropertyRelative("buttons");

            Name.stringValue = EditorGUILayout.TextField(Name.stringValue);
            Buttons.objectReferenceValue = EditorGUILayout.ObjectField(Buttons.objectReferenceValue, typeof(GameObject), true);

            if (GUILayout.Button(new GUIContent("-", "Delete entry"), EditorStyles.miniButton, GUILayout.Width(40f)))
            {
                list.DeleteArrayElementAtIndex(i);
                if (i < Current.intValue)
                    Current.intValue -= 1;
                else if (i == Current.intValue)
                    Current.intValue = 0;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button(new GUIContent("+", "Add entry")))
        {
            list.InsertArrayElementAtIndex(list.arraySize);
            SerializedProperty element = list.GetArrayElementAtIndex(list.arraySize - 1);
            element.FindPropertyRelative("name").stringValue = null;
            element.FindPropertyRelative("buttons").objectReferenceValue = null;

        }
    }
}
