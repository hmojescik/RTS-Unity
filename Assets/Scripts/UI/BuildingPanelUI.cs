using UnityEngine;

public class BuildingPanelUI : MonoBehaviour
{
    public static BuildingPanelUI Instance;

    public GameObject panel;

    private TownHall selectedTownHall;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(TownHall townHall)
    {
        selectedTownHall = townHall;
        panel.SetActive(true);
    }

    public void Hide()
    {
        selectedTownHall = null;
        panel.SetActive(false);
    }

    public void TrainWorker()
    {
        if (selectedTownHall != null)
        {
            selectedTownHall.TrainWorker();
        }
    }
    public void BuildBarracks()
    {
        BuildManager.Instance.SelectBuilding(BuildManager.Instance.barracksPrefab);
    }

    public void BuildFarm()
    {
        BuildManager.Instance.SelectBuilding(BuildManager.Instance.farmPrefab);
    }
    public static void HideAll()
    {
        if (Instance != null)
            Instance.Hide();

        if (WorkerPanelUI.Instance != null)
            WorkerPanelUI.Instance.Hide();

        if (BarracksPanelUI.Instance != null)
            BarracksPanelUI.Instance.Hide();
    }
}