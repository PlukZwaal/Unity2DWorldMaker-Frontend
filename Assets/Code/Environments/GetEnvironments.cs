using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

public class GetEnvironments : MonoBehaviour
{
    [SerializeField] public Transform contentParent;
    [SerializeField] public GameObject listItemPrefab;
    public Environment2DApiClient enviroment2DApiClient;
    public Environment2D environment2D;

    public async void ReadEnvironment2Ds()
    {
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.ReadEnvironment2Ds();

        switch (webRequestResponse)
        {
            case WebRequestData<List<Environment2D>> dataResponse:
                ClearList();
                PopulateList(dataResponse.Data);
                break;
            case WebRequestError errorResponse:
                Debug.LogError($"Error: {errorResponse.ErrorMessage}");
                break;
            default:
                throw new NotImplementedException("Geen implementatie voor: " + webRequestResponse.GetType());
        }
    }

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
}