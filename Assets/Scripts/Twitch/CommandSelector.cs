using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSelector : MonoBehaviour
{
    InputField commandSelector = null;

    [SerializeField]
    public ChatCommands.CommandType type = ChatCommands.CommandType.AudienceTime;

    private void Start()
    {
        commandSelector = GetComponent<InputField>();
        commandSelector.text = ChatCommands.commands[type];
        commandSelector.onValueChanged.AddListener(UpdateCommand);
    }

    private void UpdateCommand(string arg0)
    {
        ChatCommands.commands[type] = arg0;
    }
}
