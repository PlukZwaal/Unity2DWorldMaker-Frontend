using System;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public Object2DApiClient object2DApiClient;
    public GameObject prefab;
    private GameObject currentInstance;

    public void CreateInstanceOfPrefab()
    {
        // Instantieer het prefab zonder gegevensinstellingen
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        currentInstance = Instantiate(prefab, mousePos, Quaternion.identity);
    }

    public void SaveObjectData()
    {
        if (currentInstance != null)
        {
            // Stel de gegevens in voor het Object2D bij het opslaan
            Object2D object2D = new Object2D
            {
                id = Guid.NewGuid().ToString(),
                environmentId = "env_123456", // Vervang dit door de daadwerkelijke environment ID
                prefabId = "prefab_654321",   // Vervang dit door de daadwerkelijke prefab ID
                positionX = currentInstance.transform.position.x,
                positionY = currentInstance.transform.position.y,
                scaleX = currentInstance.transform.localScale.x,
                scaleY = currentInstance.transform.localScale.y,
                rotationZ = currentInstance.transform.rotation.eulerAngles.z,
                sortingLayer = currentInstance.GetComponent<Renderer>().sortingLayerID
            };

            // Roep de methode aan om het object daadwerkelijk aan te maken met de API
            CreateObject2D(object2D);
        }
        else
        {
            Debug.LogWarning("Geen instantie beschikbaar om op te slaan.");
        }
    }

    public async void CreateObject2D(Object2D object2D)
    {
        Debug.Log("Verzenden van Object2D naar API...");
        IWebRequestReponse webRequestResponse = await object2DApiClient.CreateObject2D(object2D);

        // Verwerk de respons van de API
        switch (webRequestResponse)
        {
            case WebRequestData<Object2D> dataResponse:
                object2D.id = dataResponse.Data.id;
                Debug.Log("Object2D succesvol aangemaakt! ID: " + object2D.id);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Fout bij het aanmaken van Object2D: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("Onverwacht webRequestResponse-type: " + webRequestResponse.GetType());
        }
    }
}
