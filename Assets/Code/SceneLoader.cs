using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GetEnvironments getEnvironments;
    public GetObjects getObjects;

    public void LoadLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public async void LoadEnvironmentsScene()
    {
        await SceneManager.LoadSceneAsync("EnvironmentsScene");
        await Task.Yield();
        getEnvironments = FindObjectOfType<GetEnvironments>();
        getEnvironments.ReadEnvironment2Ds();
    }

    public async void LoadCreateEnvironmentScene()
    {
        await SceneManager.LoadSceneAsync("CreateEnvironmentScene");
        await Task.Yield();
    }
}