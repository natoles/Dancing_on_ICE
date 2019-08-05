using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLava : MonoBehaviour
{
    public bool touchedLava = false;
    bool armed = false;

    void Start(){
        StartCoroutine(DisableAtAwake());
    }

    IEnumerator DisableAtAwake(){
        yield return new WaitForSeconds(0.2f); //To prevent Boom on spawn
        armed = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(armed) touchedLava = true;
    }



}
