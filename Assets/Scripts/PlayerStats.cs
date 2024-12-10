using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Variables
    public float maxHealth;
    public float currentHealth;

    public event Action OnHealthChanged;
    #endregion

    private void Awake()
    {
        // √ ±‚»≠
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnHealthChanged?.Invoke();
    }
}
