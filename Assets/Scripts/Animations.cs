using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Animations : MonoBehaviour
{
    public static Animations Instance;

    [SerializeField] private Ease popupOpenEase, popupCloseEase;
    [SerializeField] private float popupAnimCompletionTime = 0.25f;

    [SerializeField] private Ease itemOpenEase, itemCloseEase;
    [SerializeField] private float itemAnimCompletionTime = 0.2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #region Child Popup Animations
    public void PopupPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.GetChild(0).localScale = Vector3.zero;
        panel.transform.GetChild(0).DOScale(1, popupAnimCompletionTime).SetEase(popupOpenEase);
    }
    public void ClosePopupPanel(GameObject panel)
    {
        ClosePopupPanel(panel, null);
    }
    public void ClosePopupPanel(GameObject panel, UnityAction onClose = null)
    {
        panel.transform.GetChild(0).DOScale(0, popupAnimCompletionTime).SetEase(popupCloseEase)
        .OnComplete(() =>
        {
            panel.SetActive(false);
            onClose?.Invoke();
        });
    }
    #endregion

    #region Item Animations
    public void ItemOpen(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.transform.DOScale(1, itemAnimCompletionTime).SetEase(itemOpenEase);
    }
    public void ItemClose(GameObject panel, UnityAction onClose = null)
    {
        panel.transform.DOScale(0, itemAnimCompletionTime).SetEase(itemCloseEase)
        .OnComplete(() =>
        {
            panel.SetActive(false);
            onClose?.Invoke();
        });
    }
    #endregion
}