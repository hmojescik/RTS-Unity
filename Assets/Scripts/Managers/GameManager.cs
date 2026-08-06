using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TownHall playerTownHall;
    public TownHall enemyTownHall;

    public bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (gameEnded)
            return;
    }

    private void Victory()
    {
        gameEnded = true;
        Debug.Log("Wygrana");
        GameOverUI.Instance.ShowVictory();
    }

    private void Defeat()
    {
        gameEnded = true;
        GameOverUI.Instance.ShowDefeat();
    }

    public void OnTownHallDestroyed(TownHall townHall)
    {
        Debug.Log("OnTownHallDestroyed");
        Debug.Log("Przekazany: " + townHall.name);
        Debug.Log("Player? " + (townHall == playerTownHall));
        Debug.Log("Enemy? " + (townHall == enemyTownHall));

        if (townHall == playerTownHall)
        {
            Debug.Log("DEFEAT");
            Defeat();
        }
        else if (townHall == enemyTownHall)
        {
            Debug.Log("VICTORY");
            Victory();
        }
    }
}