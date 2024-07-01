using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public event Action<WorldPosition> LeftMouseClicked;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse click is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else
            {
                WorldPosition mousePosition = GetMouseWorldPosition();
                LeftMouseClicked?.Invoke(mousePosition);
            }
        }
    }

    private WorldPosition GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new(mousePosition);
    }
}
