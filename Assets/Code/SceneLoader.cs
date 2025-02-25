using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
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

    public void LoadEnvironmentsScene()
    {
        SceneManager.LoadScene("EnvironmentsScene");
    }

    public void LoadCreateEnvironmentScene()
    {
        SceneManager.LoadScene("CreateEnvironmentScene");
    }
}