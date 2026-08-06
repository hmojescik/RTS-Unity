using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameObject tutorialPanel;

    private bool isOpen;

    private void Start()
    {
        tutorialPanel.SetActive(false);
    }

    public void ToggleTutorial()
    {
        isOpen = !isOpen;
        tutorialPanel.SetActive(isOpen);
    }
}