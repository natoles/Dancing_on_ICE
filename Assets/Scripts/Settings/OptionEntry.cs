using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class OptionEntry : MonoBehaviour
{
    [SerializeField]
    private Text label = null;

    [SerializeField]
    private InputField inputField = null;

    [SerializeField]
    private Toggle toggleButton = null;
    
    public PropertyInfo Property
    {
        set
        {
            SetLayout(value);
        }
    }

    public Action Apply = () => { };

    private void SetLayout(PropertyInfo property)
    {
        Type type = property.PropertyType;

        label.text = property.Name.SplitCamelCase();

        bool isString = type == typeof(string);
        bool isBool = type == typeof(bool);
        bool isInt = type == typeof(int);
        bool isFloat = type == typeof(float);

        if (isString || isInt || isFloat)
        {
            inputField.gameObject.SetActive(true);
            if (isString)
            {
                inputField.contentType = InputField.ContentType.Standard;
                inputField.text = (string)property.GetValue(null);
                Apply = () => { property.SetValue(null, inputField.text); };
            }
            else if (isInt)
            {
                inputField.contentType = InputField.ContentType.IntegerNumber;
                inputField.text = ((int)property.GetValue(null)).ToString();
                Apply = () => { property.SetValue(null, int.Parse(inputField.text)); };
            }
            else if (isFloat)
            {
                inputField.contentType = InputField.ContentType.DecimalNumber;
                inputField.text = ((float)property.GetValue(null)).ToString();
                Apply = () => { property.SetValue(null, float.Parse(inputField.text)); };
            }
        }
        else
            inputField.gameObject.SetActive(false);

        if (isBool)
        {
            toggleButton.gameObject.SetActive(true);
            toggleButton.isOn = (bool)property.GetValue(null);
            Apply = () => { property.SetValue(null, toggleButton.isOn); };
        }
        else
            toggleButton.gameObject.SetActive(false);
    }
}
