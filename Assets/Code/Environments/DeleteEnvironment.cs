using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteEnvironment : MonoBehaviour
{
    public Environment2DApiClient enviroment2DApiClient;
    public GetEnvironments getEnvironments;
    public Environment2D environment2D;

    [Obsolete]
    public async void DeleteEnvironment2D(string id)
    {
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.DeleteEnvironment(id);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                string responseData = dataResponse.Data;
                await SceneManager.LoadSceneAsync("EnvironmentsScene");
                await Task.Yield();
                getEnvironments = FindObjectOfType<GetEnvironments>();
                getEnvironments.ReadEnvironment2Ds();
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                //Debug.Log("Delete environment error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
}
