using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(VotePanelDifficultyCursorController), true)]
public class VotePanelDifficultyCursorControllerEditor : Editor
{
    SerializedProperty Cursor;
    SerializedProperty MinDifficulty;
    SerializedProperty MaxDifficulty;

    float minVal = 1f;
    float maxVal = 5f;

    float minLimit = 0f;
    float maxLimit = 10f;

    protected void OnEnable()
    {
        Cursor = serializedObject.FindProperty("cursor");
        MinDifficulty = serializedObject.FindProperty("minDifficulty");
        MaxDifficulty = serializedObject.FindProperty("maxDifficulty");

        minVal = MinDifficulty.intValue;
        maxVal = MaxDifficulty.intValue;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(GetType().ToString(), EditorStyles.centeredGreyMiniLabel);

        serializedObject.Update();

        EditorGUILayout.PropertyField(Cursor);

        EditorGUILayout.LabelField("Min Val:", minVal.ToString());
        EditorGUILayout.LabelField("Max Val:", maxVal.ToString());
        EditorGUILayout.MinMaxSlider(ref minVal, ref maxVal, minLimit, maxLimit);

        minVal = Mathf.Max(Mathf.RoundToInt(minVal), Mathf.CeilToInt(minLimit));
        maxVal = Mathf.Min(Mathf.RoundToInt(maxVal), Mathf.FloorToInt(maxLimit));

        MinDifficulty.intValue = (int)minVal;
        MaxDifficulty.intValue = (int)maxVal;

        serializedObject.ApplyModifiedProperties();
    }
}
