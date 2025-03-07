using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;
    [HideInInspector] public bool isDead = false;
    public LayerMask enemyLayer;
    public float invincibilityTime = 1f;

    [Header("UI References")]
    public Canvas playerUI;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isDead)
        {
            //shows player ui
            playerUI.enabled = true;
            //shows on text current health
            playerUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = 
            "Player Health: " + currentHealth.ToString();
        }
        
    }
}
