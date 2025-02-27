using UnityEngine;
using UnityEngine.UI;

public class CreateObjects : MonoBehaviour
{
    public Object2DApiClient object2DApiClient;
    public GameObject prefabCar;
    public GameObject prefabPoppetje;
    public GameObject prefabFlag;
    private GameObject currentInstance;
    private string prefabId;

    private void Start()
    {
        // Standaard Unity Button configuratie
        GetComponent<Button>().onClick.AddListener(() =>
        {
            CreateInstanceOfPrefabCar();
        });
    }

    public void CreateInstanceOfPrefabCar()
    {
        Vector3 mousePos = GetMousePosition();
        currentInstance = Instantiate(prefabCar, mousePos, Quaternion.identity);
        currentInstance.GetComponent<Draggable>().StartDragging();
        prefabId = "car"; 
    }

    public void CreateInstanceOfPrefabPoppetje()
    {
        Vector3 mousePos = GetMousePosition();
        currentInstance = Instantiate(prefabPoppetje, mousePos, Quaternion.identity);
        currentInstance.GetComponent<Draggable>().StartDragging();
        prefabId = "poppetje";
    }

    public void CreateInstanceOfPrefabFlag()
    {
        Vector3 mousePos = GetMousePosition();
        currentInstance = Instantiate(prefabFlag, mousePos, Quaternion.identity);
        currentInstance.GetComponent<Draggable>().StartDragging();
        prefabId = "flag";
    }

    public void SaveObjectData()
    {
        if (currentInstance != null)
        {
            Object2D object2D = new Object2D
            {
                id = System.Guid.NewGuid().ToString(),
                environmentId = PlayerPrefs.GetString("CurrentEnvironmentId"),
                prefabId = prefabId, 
                positionX = currentInstance.transform.position.x,
                positionY = currentInstance.transform.position.y,
                scaleX = currentInstance.transform.localScale.x,
                scaleY = currentInstance.transform.localScale.y,
                rotationZ = currentInstance.transform.rotation.eulerAngles.z,
                sortingLayer = currentInstance.GetComponent<Renderer>().sortingLayerID
            };

            CreateObject2D(object2D);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    public async void CreateObject2D(Object2D object2D)
    {
        Debug.Log("Verzenden naar API...");
        IWebRequestReponse response = await object2DApiClient.CreateObject2D(object2D);

        switch (response)
        {
            case WebRequestData<Object2D> data:
                Debug.Log($"Aangemaakt met ID: {data.Data.id}");
                break;
            case WebRequestError error:
                Debug.LogError($"Fout: {error.ErrorMessage}");
                break;
        }
    }
}