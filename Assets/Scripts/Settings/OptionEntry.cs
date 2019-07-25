using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionEntry : MonoBehaviour
{
    #region Label
    [SerializeField]
    private Text Label = null;

    public string OptionName { get { return Label?.text; } set { if (Label != null) { Label.text = value; } } }
    #endregion

    #region String Value

    [SerializeField]
    private InputField StringValueInput = null;

    public string StringValue
    {
        get
        {
            return StringValueInput?.text;
        }
        set
        {
            if (StringValueInput != null)
                StringValueInput.text = value;
        }
    }

    #endregion

    #region Bool Value

    [SerializeField]
    private Toggle BoolValueInput = null;
    
    public bool BoolValue
    {
        get
        {
            return (bool)BoolValueInput?.isOn;
        }
        set
        {
            if (BoolValueInput != null)
                BoolValueInput.isOn = value;
        }
    }

    #endregion

    #region Number Value

    [SerializeField]
    private Slider NumberValueInput = null;
    
    private float numberStep = 1;
    private bool numberRoundValues = false;

    public int IntValue
    {
        get
        {
            return (int)NumberValueInput?.value;
        }
        set
        {
            if (NumberValueInput != null)
                NumberValueInput.value = value;
        }
    }

    public float FloatValue
    {
        get
        {
            return (float)NumberValueInput?.value;
        }
        set
        {
            if (NumberValueInput != null)
                NumberValueInput.value = value;
        }
    }

    public float MinNumberValue
    {
        get
        {
            return (float)NumberValueInput.minValue;
        }
        set
        {
            if (NumberValueInput != null)
                NumberValueInput.minValue = value;
        }
    }

    public float MaxNumberValue 
    {
        get
        {
            return (float)NumberValueInput.minValue;
        }
        set
        {
            if (NumberValueInput != null)
                NumberValueInput.minValue = value;
        }
    }
    public float NumberValueStep
    {
        get 
        {
            return (float)numberStep;
        }
        set
        {
            if (NumberValueInput != null)
                numberStep = value > 0f ? value : Mathf.Epsilon;
        }
    }

    public bool RoundNumberValues
    {
        get
        {
            return (bool)numberRoundValues;
        }
        set
        {
            if (NumberValueInput != null)
                numberRoundValues = value;
        }
    }

    #endregion

    #region Controller
    private void HideAllFields()
    {
        StringValueInput.gameObject.SetActive(false);
        BoolValueInput.gameObject.SetActive(false);
        NumberValueInput.gameObject.SetActive(false);
    }

    public void SetLayout(Type type)
    {
        HideAllFields();
        if (type == typeof(string))
            StringValueInput.gameObject.SetActive(true);
        else if (type == typeof(bool))
            BoolValueInput.gameObject.SetActive(true);
        else if (type == typeof(int) || type == typeof(float))
            NumberValueInput.gameObject.SetActive(true);
    }
    #endregion
}
