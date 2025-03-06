using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public User user;
    public UserApiClient userApiClient;
    public GetEnvironments getEnvironments;
    public InputField emailInput;
    public InputField passwordInput;
    public Text feedbackText;

    public async void PerformLogin()
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
                getEnvironments = FindObjectOfType<GetEnvironments>();
                getEnvironments.ReadEnvironment2Ds();
                break;

            case WebRequestError errorResponse:
                feedbackText.text = "Geen account gevonden met deze gegevens!";
                break;

            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
}
