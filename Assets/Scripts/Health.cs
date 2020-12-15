
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
public class Health : MonoBehaviour
{
    [SerializeField] GameObject healthCanvas;
    [SerializeField] Image healthFill;
    private float _currentHealth;

    public float maxHealth = 100.0f;

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
        if (healthCanvas != null)
            healthCanvas.SetActive(false);
    }

    public void UpdateHealthBarUI()
    {
        if (healthFill == null)
            return;
        if (healthCanvas != null)
        {
            StopAllCoroutines();
            healthCanvas.SetActive(true);
        }

        float healthAmount = currentHealth / maxHealth;
        healthFill.fillAmount = healthAmount;
        if (healthCanvas != null)
            StartCoroutine(HideHealthBar());
    }
    IEnumerator HideHealthBar()
    {

        yield return new WaitForSeconds(1f);
        healthCanvas.SetActive(false);

    }
    public void ChangeHealth(int healthAmount)
    {

        _currentHealth += healthAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
    }

    public void RestoreHealth()
    {

        _currentHealth = maxHealth;
        OnHealtedChange.Invoke();



    }


    public bool IsAlive()
    {
        return _currentHealth > 0;
    }


}
