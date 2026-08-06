using UnityEngine;

public class WorkerPanelUI : MonoBehaviour
{
    public static WorkerPanelUI Instance;

    public GameObject panel;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("Pokazuję WorkerPanel");
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}