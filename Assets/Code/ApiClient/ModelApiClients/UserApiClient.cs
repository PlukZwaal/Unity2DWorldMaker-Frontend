using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> Register(User user)
    {
        string route = "/account/register";
        string data = JsonUtility.ToJson(user);

        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Login(User user)
    {
        string route = "/account/login";
        string data = JsonUtility.ToJson(user);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);
        return ProcessLoginResponse(response);
    }

    private IWebRequestReponse ProcessLoginResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                //Debug.Log("Response data raw: " + data.Data);

                var json = JsonUtility.FromJson<TokenResponse>(data.Data);
                if (!string.IsNullOrEmpty(json.accessToken))
                {
                    webClient.SetToken(json.accessToken);
                    return new WebRequestData<string>(json.accessToken);
                }
                else
                {
                    return new WebRequestData<string>("Fout: Geen geldige token");
                }
            default:
                return webRequestResponse;
        }
    }

    public class TokenResponse
    {
        public string accessToken;
        public string tokenType;
        public int expiresIn;
        public string refreshToken;
    }

}

