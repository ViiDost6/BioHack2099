using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;
    [HideInInspector] public bool isDead = false;
    public float invincibilityTime = 1f;

    [HideInInspector] public float enemyHealth;

    [Header("UI References")]
    public Canvas playerUI;
    public Image healthBar;

    public Canvas deathScreen;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth; // Initialize health bar
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            DeathScreen(); // Call death screen method
        }

        if (!isDead)
        {
            healthBar.fillAmount = currentHealth / maxHealth; // Update health bar

            currentHealth += Time.deltaTime * 2; // Heal over time
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth; // Clamp to max health
            }
        }
        
    }

    private void DeathScreen()
    {
        isDead = true; // Set player as dead
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("DeathScreen", LoadSceneMode.Additive);
    }
}
