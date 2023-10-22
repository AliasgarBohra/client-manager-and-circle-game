using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }
    public void LoadTask(int taskIndex)
    {
        SceneManager.LoadScene("Task " + taskIndex);
    }
}