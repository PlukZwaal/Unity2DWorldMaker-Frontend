using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GetEnvironments : MonoBehaviour
{
    [SerializeField] public Transform contentParent;
    [SerializeField] public GameObject listItemPrefab;
    public Environment2DApiClient enviroment2DApiClient;

    public GetObjects getObjects;
    public Environment2D environment2D;
    public DeleteEnvironment deleteEnvironment;


    public async void ReadEnvironment2Ds()
    {
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.ReadEnvironment2Ds();

        switch (webRequestResponse)
        {
            case WebRequestData<List<Environment2D>> dataResponse:
                ClearList();
                PopulateList(dataResponse.Data);
                PlayerPrefs.SetInt("EnvironmentCount", dataResponse.Data.Count);
                string[] names = new string[dataResponse.Data.Count];
                for (int i = 0; i < dataResponse.Data.Count; i++) names[i] = dataResponse.Data[i].name;
                PlayerPrefsX.SetStringArray("EnvironmentNames", names);
                break;
            case WebRequestError errorResponse:
                //Debug.LogError($"Error: {errorResponse.ErrorMessage}");
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void PopulateList(List<Environment2D> environments)
    {
        foreach (var environment in environments)
        {
            GameObject newItem = Instantiate(listItemPrefab, contentParent);

            // 1. NAAMTEKST INSTELLEN
            Transform nameButton = newItem.transform.Find("NameButton");
            if (nameButton != null)
            {
                TMP_Text nameText = nameButton.GetComponentInChildren<TMP_Text>(true);
                if (nameText != null) nameText.text = environment.name;

                // 2. KLIK OP NAMEBUTTON
                Button button = nameButton.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                       SeeEnvironment(environment.id)
                    );
                }
            }

            // 3. PRULLENBAK
            Transform trashButton = newItem.transform.Find("TrashButton");
            if (trashButton != null)
            {
                Button button = trashButton.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                        deleteEnvironment.DeleteEnvironment2D(environment.id)
                    );
                }
            }
        }
    }

    public async void SeeEnvironment(string id)
    {
        PlayerPrefs.SetString("CurrentEnvironmentId", id);
        PlayerPrefs.Save();
        await SceneManager.LoadSceneAsync("SeeEnvironmentScene");
        await Task.Yield();
        getObjects = FindObjectOfType<GetObjects>();
        getObjects.ReadObject2Ds(id);
    }
    
    public void ClearList()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    public static class PlayerPrefsX
    {
        public static void SetStringArray(string key, string[] array) =>
            PlayerPrefs.SetString(key, string.Join("|", array));

        public static string[] GetStringArray(string key) =>
            PlayerPrefs.GetString(key, "").Split(
                new[] { '|' },
                StringSplitOptions.RemoveEmptyEntries
            );
    }
}