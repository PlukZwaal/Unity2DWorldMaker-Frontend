using System;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public Object2D object2D;
    public Object2DApiClient object2DApiClient;
    public GameObject prefab;

    public void CreateInstanceOfPrefab()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        GameObject instance = Instantiate(prefab, mousePos, Quaternion.identity);
        CreateObject2D();
    }

    public async void CreateObject2D()
    {
        object2D = new Object2D
        {
            id = Guid.NewGuid().ToString(),
            environmentId = "env_123456",
            prefabId = "prefab_654321",
            positionX = UnityEngine.Random.Range(-10f, 10f),
            positionY = UnityEngine.Random.Range(-10f, 10f),
            scaleX = UnityEngine.Random.Range(0.5f, 2f),
            scaleY = UnityEngine.Random.Range(0.5f, 2f),
            rotationZ = UnityEngine.Random.Range(0f, 360f),
            sortingLayer = UnityEngine.Random.Range(0, 5)
        };

        Debug.Log("e");
        IWebRequestReponse webRequestResponse = await object2DApiClient.CreateObject2D(object2D);

        switch (webRequestResponse)
        {
            case WebRequestData<Object2D> dataResponse:
                object2D.id = dataResponse.Data.id;
                Debug.Log("Object2D succesvol aangemaakt! ID: " + object2D.id);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create Object2D error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
}
