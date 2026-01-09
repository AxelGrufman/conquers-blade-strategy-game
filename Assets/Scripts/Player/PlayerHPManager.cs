using UnityEngine;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{

    private int maxHealth;
    private int currentHealth;
    public int WidthHealthBar;
    public int HeightHealthBar;
    public Health PlayerHealth;
    public RectTransform HealthBar;

    private void Start()
    {
       maxHealth = PlayerHealth.GetMaxHealth();
         currentHealth = PlayerHealth.GetHealth();
    }

    private void Update()
    {
        currentHealth = PlayerHealth.GetHealth();
        Debug.Log("Current Health: " + currentHealth);
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        currentHealth = PlayerHealth.GetHealth();
        float healthRatio = ((float)currentHealth / maxHealth) * WidthHealthBar;

        HealthBar.sizeDelta = new Vector2(healthRatio, HeightHealthBar);
    }
}
