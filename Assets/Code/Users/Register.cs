using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public User user;
    public UserApiClient userApiClient;
    public InputField emailInput;
    public InputField passwordInput;
    public Text feedbackText;
    public Login login;

    public async void PerformRegister()
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
                login = FindObjectOfType<Login>();
                login.PerformLogin();
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
}
