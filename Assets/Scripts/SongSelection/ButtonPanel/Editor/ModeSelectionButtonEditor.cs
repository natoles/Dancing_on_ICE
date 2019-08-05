using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;

[CustomEditor(typeof(ModeSelectionButton), true)]
public class ModeSelectionButtonEditor : ButtonEditor
{
    SerializedProperty TargetText;
    SerializedProperty Slider;
    SerializedProperty Modes;
    SerializedProperty Current;

    ReorderableList ModesList;

    protected override void OnEnable()
    {
        base.OnEnable();
        TargetText = serializedObject.FindProperty("textComponent");
        Slider = serializedObject.FindProperty("difficultySlider");

        Modes = serializedObject.FindProperty("modes");
        Current = serializedObject.FindProperty("current");
        ModesList = new ReorderableList(serializedObject, Modes)
        {
            draggable = true,
            displayAdd = true,
            displayRemove = true,
            drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Game modes"),
            drawElementCallback = (rect, index, active, focused) =>
            {
                SerializedProperty element = Modes.GetArrayElementAtIndex(index);

                SerializedProperty Mode = element.FindPropertyRelative("mode");
                SerializedProperty Slider = element.FindPropertyRelative("showDifficultySlider");
                SerializedProperty Buttons = element.FindPropertyRelative("buttonsToShow");
                SerializedProperty PreviousButtonState = element.FindPropertyRelative("previousButtonsState");

                rect.y += 0.125f * EditorGUIUtility.singleLineHeight;

                bool oldEnabled = GUI.enabled;
                Color oldColor = GUI.backgroundColor;
                string buttonText = "Set active";
                if (index == Current.intValue)
                {
                    GUI.enabled = false;
                    buttonText = "Active";
                    GUI.backgroundColor = Color.green;
                }

                if (GUI.Button(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), buttonText))
                {
                    Current.intValue = index;
                }
                GUI.enabled = oldEnabled;
                GUI.backgroundColor = oldColor;
                rect.y += EditorGUIUtility.singleLineHeight;

                rect.y += 0.125f * EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Mode);
                rect.y += EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Slider);
                rect.y += EditorGUIUtility.singleLineHeight;

                Object previousGameObject = Buttons.objectReferenceValue;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Buttons);
                if (Buttons.objectReferenceValue != previousGameObject)
                {
                    if (previousGameObject != null)
                        (previousGameObject as GameObject).SetActive(PreviousButtonState.boolValue);

                    if (Buttons.objectReferenceValue != null)
                        PreviousButtonState.boolValue = (Buttons.objectReferenceValue as GameObject).activeSelf;
                }
            },
            elementHeight = EditorGUIUtility.singleLineHeight * 4.5f,
            onAddCallback = (list) =>
            {
                list.serializedProperty.arraySize++;

                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);

                element.FindPropertyRelative("mode").objectReferenceValue = null;
                element.FindPropertyRelative("showDifficultySlider").boolValue = false;
                element.FindPropertyRelative("buttonsToShow").objectReferenceValue = null;
            },
            onRemoveCallback = (list) =>
            {
                if ((list.count > 1) && (list.index < Current.intValue || (list.index == Current.intValue && Current.intValue == list.count - 1)))
                    Current.intValue--;
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            },
            onReorderCallbackWithDetails = (list, oldindex, newindex) =>
            {
                if (Current.intValue == oldindex)
                    Current.intValue = newindex;
                else if (Current.intValue < oldindex && Current.intValue >= newindex)
                    Current.intValue++;
                else if (Current.intValue > oldindex && Current.intValue <= newindex)
                    Current.intValue--;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();
        
        EditorGUILayout.PropertyField(TargetText);
        EditorGUILayout.PropertyField(Slider);

        ModesList.DoLayoutList();

        if (serializedObject.ApplyModifiedProperties())
            (serializedObject.targetObject as ModeSelectionButton).UpdateModeDisplay();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField(GetType().BaseType.ToString(), EditorStyles.centeredGreyMiniLabel);

        base.OnInspectorGUI();
    }
}
