using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnvironmentUIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public Transform contentParent;
    [SerializeField] public GameObject listItemPrefab;

    public void PopulateList(List<Environment2D> environments)
    {
        foreach (var environment in environments)
        {
            GameObject newItem = Instantiate(listItemPrefab, contentParent);
            if (newItem.TryGetComponent(out TMP_Text textComponent))
            {
                textComponent.text = environment.name;
            }
        }
    }

    public void ClearList()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    public void DebugLogEnvironments(List<Environment2D> environments)
    {
        Debug.Log("Gevonden environments:");
        foreach (var env in environments)
        {
            Debug.Log($"- {env.name} (ID: {env.id})");
        }
    }
}