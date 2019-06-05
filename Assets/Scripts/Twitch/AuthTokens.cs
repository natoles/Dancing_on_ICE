using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAuthTokens
{
    string CLIENT_ID { get; }
    string CLIENT_SECRET { get; }
    string BOT_ACCESS_TOKEN { get; }
    string BOT_REFRESH_TOKEN { get; }
}
