using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTokens : IAuthTokens
{
    public string CLIENT_ID         { get; } = "";
    public string CLIENT_SECRET     { get; } = "";
    public string BOT_ACCESS_TOKEN  { get; } = "";
    public string BOT_REFRESH_TOKEN { get; } = "";
}
