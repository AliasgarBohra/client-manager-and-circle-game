using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject disableTouchInstruction;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private LineDrawer lineDrawer;

    private void Start()
    {
        lineDrawer.onTouchUp += OnGameOver;
        lineDrawer.onTouchDown += OnTouchDown;
    }

    private void OnTouchDown()
    {
        Animations.Instance.ItemClose(disableTouchInstruction);
    }
    private void OnGameOver()
    {
        Debug.Log("Game Over");

        Animations.Instance.PopupPanel(gameOverPanel);
    }
    public void RestartGame()
    {
        Animations.Instance.ClosePopupPanel(gameOverPanel, delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
    }
}