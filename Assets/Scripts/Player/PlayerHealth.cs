using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;
    [HideInInspector] public bool isDead = false;
    public float invincibilityTime = 1f;
    public GameObject enemy;
    [HideInInspector] public float enemyHealth;

    [Header("UI References")]
    public Canvas playerUI;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyHealth = enemy.GetComponent<Enemy>().health;
    }

    private void Update()
    {
        if (!isDead)
        {
            //shows player ui
            playerUI.enabled = true;
            //shows on text current health
            playerUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = 
            "Player Health: " + currentHealth.ToString() + "/" + enemyHealth.ToString();
        }
        
    }
}
