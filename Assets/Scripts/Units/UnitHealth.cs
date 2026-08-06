using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public float healthBarHeight = 2f;

    private HealthBarFollow healthBar;
    public Team team;
    public int maxHealth = 100;
    public int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        GameObject bar = Instantiate(healthBarPrefab, FindFirstObjectByType<Canvas>().transform);

        healthBar = bar.GetComponent<HealthBarFollow>();
        healthBar.target = this;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        TownHall townHall = GetComponent<TownHall>();

        if (townHall != null)
        {
            GameManager.Instance.OnTownHallDestroyed(townHall);
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }
}