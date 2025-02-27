using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool isDragging = false;
    private CreateObjects createObjects;

    public void StartDragging()
    {
        isDragging = true;
    }

    private void Update()
    {
        if (isDragging)
            transform.position = GetMousePosition();
    }

    private void OnMouseUpAsButton()
    {
        if (isDragging)
        {
            isDragging = false;
            SavePosition();
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    private void SavePosition()
    {
        Debug.Log($"Positie opgeslagen: {transform.position}");
        createObjects ??= FindObjectOfType<CreateObjects>();
        createObjects.SaveObjectData();
    }
}