using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;

    [SerializeField]
    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        // Cuando se pulsa el botón izquierdo
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPosition = Mouse.current.position.ReadValue();

            // Para seleccionar las unidades
            selectionBox = new Rect();
        }

        // Mientras se mantiene pulsado
        if (Mouse.current.leftButton.isPressed)
        {
            if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0)
            {
                UnitSelectionManager.Instance.DeselectAll();
                SelectUnits();
            }
            
            endPosition = Mouse.current.position.ReadValue();

            DrawVisual();
            DrawSelection();
        }

        // Cuando se suelta
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SelectUnits();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;

            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2f;

        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(
            Mathf.Abs(boxStart.x - boxEnd.x),
            Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mousePos.x < startPosition.x)
        {
            selectionBox.xMin = mousePos.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = mousePos.x;
        }

        if (mousePos.y < startPosition.y)
        {
            selectionBox.yMin = mousePos.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = mousePos.y;
        }
    }

    void SelectUnits()
    {
        foreach (GameObject unit in UnitSelectionManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}