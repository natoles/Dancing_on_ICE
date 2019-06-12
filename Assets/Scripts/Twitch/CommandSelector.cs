using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelector : MonoBehaviour
{
    InputField commandSelector = null;

    [SerializeField]
    public string type = "AudienceTimeCommand";

    private void Start()
    {
        commandSelector = GetComponent<InputField>();
        commandSelector.text = SettingsManager.Instance.config["ChatCommands"][type].StringValue;
        commandSelector.onValueChanged.AddListener(UpdateCommand);
    }

    private void UpdateCommand(string arg0)
    {
        SettingsManager.Instance.config["ChatCommands"][type].StringValue = arg0;
    }
}
