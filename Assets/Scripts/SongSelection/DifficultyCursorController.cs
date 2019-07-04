using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyCursorController : MonoBehaviour
{
    [SerializeField]
    private RectTransform cursor = null;

    private RectTransform cursorBounds = null;

    [SerializeField]
    private int minDifficulty = 1;

    [SerializeField]
    private int maxDifficulty = 5;

    private float tmpDifficulty = 0;
    private float difficulty = 0;

    private float updateDelay = 0.1f;

    public float Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = Mathf.Clamp(value, minDifficulty, maxDifficulty);
        }
    }

    protected void Start()
    {
        cursorBounds = cursor.parent.GetComponent<RectTransform>();
        difficulty = (maxDifficulty + minDifficulty) / 2f;
        tmpDifficulty = Difficulty;
        UpdateCursorPosition(Difficulty);
    }

    private void Update()
    {
        tmpDifficulty = Mathf.Lerp(tmpDifficulty, difficulty, updateDelay);
        UpdateCursorPosition(tmpDifficulty);
    }

    private void UpdateCursorPosition(float cursorDifficulty)
    {
        cursor.anchoredPosition = Vector3.right * (cursorDifficulty - minDifficulty) / (maxDifficulty - minDifficulty) * cursorBounds.rect.width;
    }
}
