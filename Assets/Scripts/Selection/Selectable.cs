using UnityEngine;

public class Selectable : MonoBehaviour
{
    public GameObject selectionMark;

    public virtual void Select()
    {
        // Najpierw ukryj wszystkie panele
        BuildingPanelUI.HideAll();

        if (selectionMark != null)
            selectionMark.SetActive(true);

        TownHall townHall = GetComponent<TownHall>();

        if (townHall != null)
        {
            BuildingPanelUI.Instance.Show(townHall);
            return;
        }

        WorkerGather worker = GetComponent<WorkerGather>();

        if (worker != null)
        {
            WorkerPanelUI.Instance.Show();
            return;
        }

        Barracks barracks = GetComponent<Barracks>();

        if (barracks != null)
        {
            BarracksPanelUI.Instance.Show(barracks);
            return;
        }
    }

    public virtual void Deselect()
    {
        if (selectionMark != null)
            selectionMark.SetActive(false);
    }
}