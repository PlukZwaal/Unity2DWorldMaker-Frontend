using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GetEnvironments environmentUIHandler; 

    [Header("UI Elements")]
    public InputField emailInput;
    public InputField passwordInput;
    public Text feedbackText;

    #region Login

    [ContextMenu("User/Register")]
    public async void Register()
    {
        user.email = emailInput.text;
        user.password = passwordInput.text;

        if (!IsEmailValid(user.email))
        {
            feedbackText.text = "Ongeldig e-mailadres";
            return;
        }

        string validationFeedback = GetPasswordValidationFeedback(user.password);
        if (!string.IsNullOrEmpty(validationFeedback))
        {
            feedbackText.text = validationFeedback;
            return;
        }

        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Login();
                break;
            case WebRequestError errorResponse:
                feedbackText.text = "Er bestaat al een gebruiker met deze naam";
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    private bool IsEmailValid(string email)
    {
        if (!email.Contains("@")) return false;

        string[] parts = email.Split('@');
        return parts.Length == 2 && parts[0].Length > 0 && parts[1].Length > 0;
    }

    private string GetPasswordValidationFeedback(string password)
    {
        var feedbackMessages = new List<string>();

        if (password.Length < 10) feedbackMessages.Add("minimaal 10 karakters lang");
        if (!Regex.IsMatch(password, "[a-z]")) feedbackMessages.Add("minstens 1 kleine letter");
        if (!Regex.IsMatch(password, "[A-Z]")) feedbackMessages.Add("minstens 1 hoofdletter");
        if (!Regex.IsMatch(password, "[0-9]")) feedbackMessages.Add("minstens 1 cijfer");
        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]")) feedbackMessages.Add("minstens 1 niet-alfanumeriek karakter");

        return feedbackMessages.Count > 0 ? "Wachtwoord moet " + string.Join(", ", feedbackMessages) + " bevatten." : "";
    }

    [ContextMenu("User/Login")]
    [Obsolete]
    public async void Login()
    {
        user.email = emailInput.text;
        user.password = passwordInput.text;

        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                PlayerPrefs.SetString("Token", dataResponse.Data);
                PlayerPrefs.Save(); 

                await SceneManager.LoadSceneAsync("EnvironmentsScene");
                await Task.Yield();
                environmentUIHandler = FindObjectOfType<GetEnvironments>();
                environmentUIHandler.ReadEnvironment2Ds();
                break;

            case WebRequestError errorResponse:
                feedbackText.text = "Geen account gevonden met deze gegevens!";
                break;

            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    #endregion

    #region Environment


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
