using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DancingICE.Modes
{
    [CustomEditor(typeof(ModeManager), true)]
    public class ModeManagerEditor : Editor
    {
        SerializedProperty Modes;
        SerializedProperty Current;

        ReorderableList ModesList;

        protected void OnEnable()
        {
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

                    SerializedProperty Name = element.FindPropertyRelative("name");
                    SerializedProperty Slider = element.FindPropertyRelative("showDifficultySlider");
                    SerializedProperty Buttons = element.FindPropertyRelative("buttonsToShow");
                    SerializedProperty SceneToLoad = element.FindPropertyRelative("sceneToLoad");

                    if (EditorGUI.Toggle(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Current", Current.intValue == index, EditorStyles.radioButton))
                    {
                        Current.intValue = index;
                    }
                    rect.y += EditorGUIUtility.singleLineHeight;

                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Name);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Slider);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), Buttons);
                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), SceneToLoad);
                },
                elementHeight = EditorGUIUtility.singleLineHeight * 5.5f,
                onAddCallback = (list) =>
                {
                    var index = list.serializedProperty.arraySize;
                    list.serializedProperty.arraySize++;
                    list.index = index;

                    var element = list.serializedProperty.GetArrayElementAtIndex(index);

                    element.FindPropertyRelative("name").stringValue = null;
                    element.FindPropertyRelative("showDifficultySlider").boolValue = false;
                    element.FindPropertyRelative("buttonsToShow").objectReferenceValue = null;
                },
                onReorderCallbackWithDetails = (list, oldindex, newindex) =>
                {
                    if (Current.intValue == oldindex)
                        Current.intValue = newindex;
                    else
                    if (Current.intValue < oldindex && Current.intValue >= newindex)
                        Current.intValue++;
                    else
                    if (Current.intValue > oldindex && Current.intValue <= newindex)
                        Current.intValue--;
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ModesList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}