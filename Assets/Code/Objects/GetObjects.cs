using System;
using UnityEngine;
using System.Collections.Generic;

public class GetObjects : MonoBehaviour
{
    public Object2D object2D;
    public Object2DApiClient object2DApiClient;

    public async void ReadObject2Ds(string id)
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(id);

        switch (webRequestResponse)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                List<Object2D> object2Ds = dataResponse.Data;
                Debug.Log("List of object2Ds: " + object2Ds);
                object2Ds.ForEach(object2D => Debug.Log(object2D.id));
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read object2Ds error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
}
