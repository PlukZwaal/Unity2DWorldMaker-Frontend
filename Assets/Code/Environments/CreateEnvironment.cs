using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class Create : MonoBehaviour
{
    public Environment2DApiClient enviroment2DApiClient;
    public GetEnvironments getEnvironments;
    public Environment2D environment2D;
    public InputField nameInput;
    public InputField heightInput;
    public InputField lenghtInput;
    public Text feedbackText;

    [ContextMenu("Environment2D/Create")]
    public async void CreateEnvironment2D()
    {
        feedbackText.text = "";

        if (!int.TryParse(lenghtInput.text, out int length) || !int.TryParse(heightInput.text, out int height))
        {
            feedbackText.text = "Lengte en hoogte moeten cijfers zijn!";
            return;
        }

        if (length < 20 || length > 200)
        {
            feedbackText.text = "Lengte moet tussen 20-200 zijn!";
            return;
        }

        if (height < 10 || height > 100)
        {
            feedbackText.text = "Hoogte moet tussen 10-100 zijn!";
            return;
        }

        string newName = nameInput.text.Trim();
        if (newName.Length < 1 || newName.Length > 25)
        {
            feedbackText.text = "Naam moet tussen 1-25 tekens zijn!";
            return;
        }

        IWebRequestReponse readResponse = await enviroment2DApiClient.ReadEnvironment2Ds();

        if (readResponse is WebRequestError readError)
        {
            feedbackText.text = "Fout bij ophalen environments: " + readError.ErrorMessage;
            return;
        }

        if (readResponse is WebRequestData<List<Environment2D>> existingData)
        {
            if (existingData.Data.Count >= 5)
            {
                feedbackText.text = "Maximaal 5 environments toegestaan!";
                return;
            }

            if (existingData.Data.Any(e => e.name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                feedbackText.text = "Naam bestaat al!";
                return;
            }
        }

        environment2D.id = Guid.NewGuid().ToString();
        environment2D.name = newName;
        environment2D.maxLength = length;
        environment2D.maxHeight = height;

        IWebRequestReponse createResponse = await enviroment2DApiClient.CreateEnvironment(environment2D);

        switch (createResponse)
        {
            case WebRequestData<Environment2D> dataResponse:
                environment2D.id = dataResponse.Data.id;
                await SceneManager.LoadSceneAsync("EnvironmentsScene");
                await Task.Yield();
                getEnvironments = FindObjectOfType<GetEnvironments>();
                getEnvironments.ReadEnvironment2Ds();
                break;
            case WebRequestError errorResponse:
                feedbackText.text = "Creatie mislukt: " + errorResponse.ErrorMessage;
                break;
            default:
                feedbackText.text = "Onbekende serverreactie";
                throw new NotImplementedException();
        }
    }
}