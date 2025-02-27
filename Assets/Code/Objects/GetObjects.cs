using System;
using UnityEngine;
using System.Collections.Generic;

public class GetObjects : MonoBehaviour
{
    public Object2DApiClient object2DApiClient;
    public GameObject prefab; // Prefab die wordt gebruikt om objecten te maken

    public async void ReadObject2Ds(string id)
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(id);

        switch (webRequestResponse)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                List<Object2D> object2Ds = dataResponse.Data;
                Debug.Log("Aantal objecten opgehaald: " + object2Ds.Count);

                foreach (Object2D obj in object2Ds)
                {
                    SpawnObject(obj);
                }
                break;

            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.LogError("Fout bij ophalen objecten: " + errorMessage);
                break;

            default:
                throw new NotImplementedException("Geen implementatie voor webRequestResponse van type: " + webRequestResponse.GetType());
        }
    }

    private void SpawnObject(Object2D objData)
    {
        Vector3 position = new Vector3(objData.positionX, objData.positionY, 0);
        Quaternion rotation = Quaternion.Euler(0, 0, objData.rotationZ);
        GameObject newObject = Instantiate(prefab, position, rotation);
        newObject.transform.localScale = new Vector3(objData.scaleX, objData.scaleY, 1);
    }
}
