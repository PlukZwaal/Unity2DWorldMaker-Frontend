using System;
using UnityEngine;
using System.Collections.Generic;

public class GetObjects : MonoBehaviour
{
    public Object2DApiClient object2DApiClient;
    public GameObject prefab; 
    public GameObject Prefab1;
    public GameObject Prefab2;
    public GameObject Prefab3;

    public async void ReadObject2Ds(string id)
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(id);

        switch (webRequestResponse)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                List<Object2D> object2Ds = dataResponse.Data;
                //Debug.Log("Aantal objecten opgehaald: " + object2Ds.Count);

                foreach (Object2D obj in object2Ds)
                {
                    SpawnObject(obj);
                }
                break;

            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                //Debug.LogError("Fout bij ophalen objecten: " + errorMessage);
                break;

            default:
                throw new NotImplementedException("Geen implementatie voor webRequestResponse van type: " + webRequestResponse.GetType());
        }
    }

    private void SpawnObject(Object2D objData)
    {
        // Kies het juiste prefab gebaseerd op het ID
        GameObject prefabToSpawn = objData.prefabId switch
        {
            "1" => Prefab1,
            "2" => Prefab2,
            "3" => Prefab3,
            _ => null
        };

        if (prefabToSpawn != null)
        {
            Vector3 position = new Vector3(objData.positionX, objData.positionY, 0);
            Quaternion rotation = Quaternion.Euler(0, 0, objData.rotationZ);

            GameObject newObject = Instantiate(prefabToSpawn, position, rotation);
            newObject.transform.localScale = new Vector3(objData.scaleX, objData.scaleY, 1);
        }
        else
        {
            //Debug.LogError($"Kon geen prefab vinden voor ID: {objData.prefabId}");
        }
    }
}
