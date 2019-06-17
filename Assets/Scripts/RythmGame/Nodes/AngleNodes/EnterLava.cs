using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLava : MonoBehaviour
{
    public bool touchedLava = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        touchedLava = true;
    }
}
