using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth = 100;
    public int health = 0;
    public int HealthPoints
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            if (health <= 0)
            {
                Die();
            }
        }
    }
    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        HealthPoints -= damage;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
