using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLoginScene()
    {
        Debug.Log("LoadLoginScene");
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
}