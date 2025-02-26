using System;
using UnityEngine;

/*
* Het GameObject moet ook een collider hebben; anders kan OnMouseUp() niet worden gedetecteerd.
*/
public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    public CreateObjects createObjects;

    private void OnMouseDown()
    {
        // Start met slepen wanneer de muisknop wordt ingedrukt
        isDragging = true;
    }

    private void Update()
    {
        if (isDragging)
        {
            // Verplaats het object naar de muispositie tijdens het slepen
            Vector3 mousePosition = GetMousePosition();
            transform.position = mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            // Stop met slepen wanneer de muisknop wordt losgelaten
            isDragging = false;

            // Roep de methode aan om de positie op te slaan
            SaveObjectPosition();
        }
    }

    private Vector3 GetMousePosition()
    {
        // Haal de muispositie op in wereldcoördinaten
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Stel de Z-positie in op 0 voor 2D-omgevingen
        return mousePosition;
    }

    [Obsolete]
    private void SaveObjectPosition()
    {
        // Verkrijg de huidige positie van het object
        Vector3 currentPosition = transform.position;

        // Hier kun je de logica toevoegen om de positie op te slaan
        // Bijvoorbeeld, stuur de positie naar een API of sla deze lokaal op
        Debug.Log($"Objectpositie opgeslagen: {currentPosition}");
        createObjects = FindObjectOfType<CreateObjects>();
        createObjects.SaveObjectData();

    }
}
