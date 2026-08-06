using UnityEngine;

public class BarracksPanelUI : MonoBehaviour
{
    public static BarracksPanelUI Instance;

    public GameObject panel;

    private Barracks selectedBarracks;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(Barracks barracks)
    {
        selectedBarracks = barracks;
        panel.SetActive(true);
    }

    public void Hide()
    {
        selectedBarracks = null;
        panel.SetActive(false);
    }

    public void TrainSoldier()
    {
        if (selectedBarracks != null)
        {
            selectedBarracks.TrainSoldier();
        }
    }
}