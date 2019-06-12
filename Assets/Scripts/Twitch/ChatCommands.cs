using System.Collections;
using System.Collections.Generic;

public static class ChatCommands
{
    public enum CommandType
    {
        AudienceTime,
        IncreaseDifficulty,
        DecreaseDifficulty,
        Congratulate
    }

    public static Dictionary<CommandType, string> commands = new Dictionary<CommandType, string>
    {
        { CommandType.AudienceTime, "gambatte" },
        { CommandType.IncreaseDifficulty, "increase" },
        { CommandType.DecreaseDifficulty, "decrease" },
        { CommandType.Congratulate, "gg" }
    };
}
