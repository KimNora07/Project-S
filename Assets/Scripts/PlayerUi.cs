using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public Image healthBar;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        // �̺�Ʈ ����
        playerStats.OnHealthChanged += UpdateHealthUI;

        // �ʱ� UI ����
        UpdateHealthUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            playerStats.TakeDamage(10);
        }
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = playerStats.currentHealth / playerStats.maxHealth;
    }
}
