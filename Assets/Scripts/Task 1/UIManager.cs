using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Clients Details UIs")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_Dropdown clientsDropDown;
    [SerializeField] private GameObject clientDataUITemplate;
    [SerializeField] private Transform contentTransform;

    [Header("Window UI")]
    [SerializeField] private GameObject clientDetailPanel;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI clientLabelText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI addressText;

    [SerializeField] private float itemSpawnTime = 0.1f;
    private WaitForSeconds waitTime;

    public Root clientDataRoot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private IEnumerator Start()
    {
        Debug.Log("Getting Data..");
        waitTime = new WaitForSeconds(itemSpawnTime);
        yield return new WaitUntil(() => !WebRequestHandler.Instance.isGettingData);

        clientsDropDown.onValueChanged.RemoveAllListeners();
        clientsDropDown.onValueChanged.AddListener(OnClientsDropDownChanged);

        titleText.text = clientDataRoot.label;

        // We will show all clients by default
        OnClientsDropDownChanged(0);

        Debug.Log("UI initialized");
    }
    private void OnClientsDropDownChanged(int value)
    {
        ClearUI();

        switch (value)
        {
            case 0:
                StartCoroutine(ShowAllClientsUI());
                break;
            case 1:
                StartCoroutine(ShowManagersUI());
                break;
            case 2:
                StartCoroutine(ShowNonManagersUI());
                break;
        }
    }

    private IEnumerator ShowAllClientsUI()
    {
        for (int i = 0; i < clientDataRoot.clients.Count; i++)
        {
            CreateClientUI(i);

            yield return waitTime;
        }
    }
    private IEnumerator ShowManagersUI()
    {
        for (int i = 0; i < clientDataRoot.clients.Count; i++)
        {
            if (clientDataRoot.clients[i].isManager)
            {
                CreateClientUI(i);

                yield return waitTime;
            }
        }
    }
    private IEnumerator ShowNonManagersUI()
    {
        for (int i = 0; i < clientDataRoot.clients.Count; i++)
        {
            if (!clientDataRoot.clients[i].isManager)
            {
                CreateClientUI(i);

                yield return waitTime;
            }
        }
    }

    private void ClearUI()
    {
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }

    private void CreateClientUI(int index)
    {
        GameObject clientObject = Instantiate(clientDataUITemplate, contentTransform);
        Animations.Instance.ItemOpen(clientObject);

        TextMeshProUGUI clientLabelText = clientObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI clientPointsText = clientObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        clientLabelText.text = clientDataRoot.clients[index].label;

        string clientDataKey = clientDataRoot.clients[index].id;

        if (clientDataRoot.data.ContainsKey(clientDataKey))
        {
            clientPointsText.text = clientDataRoot.data[clientDataKey].points.ToString();
        }
        else
        {
            clientPointsText.text = "N/A";
        }

        clientObject.GetComponent<Button>().onClick.AddListener(() => ShowClientDetails(index));
    }

    private void ShowClientDetails(int index)
    {
        Animations.Instance.PopupPanel(clientDetailPanel);

        clientLabelText.text = clientDataRoot.clients[index].label;

        string clientDataKey = clientDataRoot.clients[index].id;

        if (clientDataRoot.data.ContainsKey(clientDataKey))
        {
            nameText.text = "Name: " + clientDataRoot.data[clientDataKey].name;
            pointsText.text = "Points: " + clientDataRoot.data[clientDataKey].points;
            addressText.text = "Address: " + clientDataRoot.data[clientDataKey].address;
        }
        else
        {
            nameText.text = "Name: N/A";
            pointsText.text = "Points: N/A";
            addressText.text = "Address: N/A";
        }
    }
}