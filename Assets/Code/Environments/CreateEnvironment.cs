using System;
using UnityEngine;

public class Create : MonoBehaviour
{
    public Environment2DApiClient enviroment2DApiClient;
    public Environment2D environment2D;

    [ContextMenu("Environment2D/Create")]
    public async void CreateEnvironment2D()
    {
        Debug.Log("Creating environment2D: " + "environment2D.name");
        environment2D.name = "New Environment";
        environment2D.maxLength = 10;
        environment2D.maxHeight = 10;
        environment2D.id = Guid.Empty.ToString(); 

        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.CreateEnvironment(environment2D);

        switch (webRequestResponse)
        {
            case WebRequestData<Environment2D> dataResponse:
                environment2D.id = dataResponse.Data.id;
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create environment2D error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
}
