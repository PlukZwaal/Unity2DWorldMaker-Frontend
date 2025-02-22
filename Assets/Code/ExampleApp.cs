using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleApp : MonoBehaviour
{
    [Header("Test data")]
    public User user;
    public Environment2D environment2D;
    public Object2D object2D;

    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public Environment2DApiClient enviroment2DApiClient;
    public Object2DApiClient object2DApiClient;

    [Header("UI Elements")]
    public InputField emailInput;   
    public InputField passwordInput;
    public Text feedbackText;       
    #region Login

    [ContextMenu("User/Register")]
    public async void Register()
    {
        // Wijs de waarden direct toe aan de bestaande variabelen
        user.email = emailInput.text;    // E-mailadres
        user.password = passwordInput.text; // Wachtwoord

        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                feedbackText.text = "Register success!"; // Geef feedback
                Debug.Log("Register success!");
                // TODO: Handle success scenario. Bijvoorbeeld, navigeer naar een ander scherm.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                feedbackText.text = "Register error: " + errorMessage; // Geef feedback
                Debug.Log("Register error: " + errorMessage);
                // TODO: Handle error scenario. Toon de foutmelding aan de gebruiker.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("User/Login")]
    public async void Login()
    {
        // Wijs de waarden direct toe aan de bestaande variabelen
        user.email = emailInput.text;    // Gebruikersnaam
        user.password = passwordInput.text; // Wachtwoord

        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                feedbackText.text = "Login succes!"; // Geef feedback
                Debug.Log("Login succes!");
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                feedbackText.text = "Login error: " + errorMessage; // Geef feedback
                Debug.Log("Login error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion

    #region Environment

    [ContextMenu("Environment2D/Read all")]
    public async void ReadEnvironment2Ds()
    {
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.ReadEnvironment2Ds();

        switch (webRequestResponse)
        {
            case WebRequestData<List<Environment2D>> dataResponse:
                List<Environment2D> environment2Ds = dataResponse.Data;
                Debug.Log("List of environment2Ds: ");
                environment2Ds.ForEach(environment2D => Debug.Log(environment2D.id));
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read environment2Ds error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Environment2D/Create")]
    public async void CreateEnvironment2D()
    {
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.CreateEnvironment(environment2D);

        switch (webRequestResponse)
        {
            case WebRequestData<Environment2D> dataResponse:
                environment2D.id = dataResponse.Data.id;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create environment2D error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Environment2D/Delete")]
    public async void DeleteEnvironment2D()
    {
        IWebRequestReponse webRequestResponse = await enviroment2DApiClient.DeleteEnvironment(environment2D.id);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                string responseData = dataResponse.Data;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Delete environment error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion Environment

    #region Object2D

    [ContextMenu("Object2D/Read all")]
    public async void ReadObject2Ds()
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(object2D.environmentId);

        switch (webRequestResponse)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                List<Object2D> object2Ds = dataResponse.Data;
                Debug.Log("List of object2Ds: " + object2Ds);
                object2Ds.ForEach(object2D => Debug.Log(object2D.id));
                // TODO: Succes scenario. Show the enviroments in the UI
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read object2Ds error: " + errorMessage);
                // TODO: Error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Object2D/Create")]
    public async void CreateObject2D()
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.CreateObject2D(object2D);

        switch (webRequestResponse)
        {
            case WebRequestData<Object2D> dataResponse:
                object2D.id = dataResponse.Data.id;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create Object2D error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("Object2D/Update")]
    public async void UpdateObject2D()
    {
        IWebRequestReponse webRequestResponse = await object2DApiClient.UpdateObject2D(object2D);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                string responseData = dataResponse.Data;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Update object2D error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion

}
