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

                PlayerPrefs.SetInt("EnvironmentCount", dataResponse.Data.Count);
                Debug.Log($"Aantal opgeslagen environments: {PlayerPrefs.GetInt("EnvironmentCount")}");

                string[] names = new string[dataResponse.Data.Count];
                for (int i = 0; i < dataResponse.Data.Count; i++)
                {
                    names[i] = dataResponse.Data[i].name;
                }
                PlayerPrefsX.SetStringArray("EnvironmentNames", names);

                string[] loadedNames = PlayerPrefsX.GetStringArray("EnvironmentNames");
                Debug.Log("Alle namen: " + string.Join(", ", loadedNames));
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

    // PlayerPrefsX helper class voor array-opslag
    public static class PlayerPrefsX
    {
        public static void SetStringArray(string key, string[] array)
        {
            PlayerPrefs.SetString(key, string.Join("|", array));
        }

        public static string[] GetStringArray(string key)
        {
            string data = PlayerPrefs.GetString(key, "");
            return data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}