using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
{
    public static WebRequestHandler Instance;

    [SerializeField] private TextMeshProUGUI loadingDataText;

    public bool isGettingData { get; private set; } = false;

    private string url = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(GetClientsData());
        }
    }

    private IEnumerator GetClientsData()
    {
        loadingDataText.text = "Loading Data...";
        isGettingData = true;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonString = webRequest.downloadHandler.text.Trim();

                Debug.Log("Fetched Json : " + jsonString);

                Root dataObject = JsonConvert.DeserializeObject<Root>(jsonString);
                UIManager.Instance.clientDataRoot = dataObject;

                isGettingData = false;
            }
            else
            {
                loadingDataText.text = "Error loading data!";
                Debug.Log("Unable to fetch! " + webRequest.error + webRequest.downloadHandler.text);
            }
        }
    }
}
public class Root
{
    public List<Client> clients;
    public Dictionary<string, Data> data;
    public string label;
}
public class Client
{
    public bool isManager;
    public string id;
    public string label;
}
public class Data
{
    public string address;
    public string name;
    public int points;
}