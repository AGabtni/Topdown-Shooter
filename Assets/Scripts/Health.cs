using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{


    public float maxHealth = 100.0f;
    [SerializeField] Image healthFill;
    [SerializeField] private float _currentHealth;

    [HideInInspector]
    public float currentHealth
    {
        get { return _currentHealth; }
    }

    [HideInInspector]
    public UnityEvent OnHealtedChange;
    private void Start()
    {
        if (OnHealtedChange == null)
            OnHealtedChange = new UnityEvent();
        OnHealtedChange.AddListener(UpdateHealthBarUI);
        _currentHealth = maxHealth;
    }

    public void UpdateHealthBarUI()
    {
        if (healthFill == null)
            return;

        float healthAmount = currentHealth / maxHealth;
        healthFill.fillAmount = healthAmount;

    }
    public void ChangeHealth(int healthAmount)
    {

        _currentHealth += healthAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
    }

    public void RestoreHealth()
    {

        _currentHealth = maxHealth;



    }


    public bool IsAlive()
    {
        return _currentHealth > 0;
    }


}
