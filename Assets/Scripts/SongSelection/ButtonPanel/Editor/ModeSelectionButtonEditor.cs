using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(ModeSelectionButton), true)]
public class ModeSelectionButtonEditor : ButtonEditor
{
    SerializedProperty TargetText;
    SerializedProperty Slider;
    SerializedProperty Modes;
    SerializedProperty Current;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        TargetText = serializedObject.FindProperty("TextComponent");
        Slider = serializedObject.FindProperty("DifficultySlider");
        Modes = serializedObject.FindProperty("Modes");
        Current = serializedObject.FindProperty("current");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();
        
        EditorGUILayout.PropertyField(TargetText);
        EditorGUILayout.PropertyField(Slider);

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
        GUIStyle centeredLabel = new GUIStyle("wordWrappedLabel") { alignment = TextAnchor.MiddleCenter, stretchWidth = false };

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(20f));
        EditorGUILayout.LabelField(new GUIContent("Name", "Name displayed by targeted Text component"), centeredLabel, GUILayout.Width((Screen.width - 106f) * 30 / 100));
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField(new GUIContent("Slider", "Should this mode use the default difficulty slider ?"), centeredLabel, GUILayout.Width(46f));
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField(new GUIContent("Buttons", "Buttons displayed when this mode is selected"), centeredLabel, GUILayout.Width((Screen.width - 106f) * 45 / 100));
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
            SerializedProperty Slider = element.FindPropertyRelative("useSlider");
            SerializedProperty Buttons = element.FindPropertyRelative("buttons");

            Name.stringValue = EditorGUILayout.TextField(Name.stringValue, GUILayout.Width((Screen.width - 76f) * 30 / 100));
            GUILayout.FlexibleSpace();
            Slider.boolValue = EditorGUILayout.Toggle(Slider.boolValue, GUILayout.Width(16f));
            GUILayout.FlexibleSpace();
            Buttons.objectReferenceValue = EditorGUILayout.ObjectField(Buttons.objectReferenceValue, typeof(GameObject), true, GUILayout.Width((Screen.width - 76f) * 45 / 100));

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
            element.FindPropertyRelative("useSlider").boolValue = false;
            element.FindPropertyRelative("buttons").objectReferenceValue = null;
        }
    }
}
