using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionDropFix : MonoBehaviour
{
    [SerializeField]
    EventSystem eventSystem = null;

    GameObject current = null;
    
    private void Update()
    {
        if (eventSystem != null)
        {
            if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != current)
                current = eventSystem.currentSelectedGameObject;
            if (eventSystem.currentSelectedGameObject == null && current != null)
                eventSystem.SetSelectedGameObject(current);
        }
    }
}
