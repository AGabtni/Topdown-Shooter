
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
public class Health : MonoBehaviour
{
    [SerializeField] GameObject healthCanvas;
    public Image healthFill;
    private float _currentHealth;

    public float maxHealth = 100.0f;

    [HideInInspector]
    public float currentHealth
    {
        get { return _currentHealth; }
    }

    UnityEvent OnPlayerHit = new UnityEvent();
    private void Start()
    {

        _currentHealth = maxHealth;
        if (healthCanvas != null)
            healthCanvas.SetActive(false);
            
        if (GetComponent<CharacterController2D>())
            OnPlayerHit.AddListener(FindObjectOfType<HUDManager>().OnPlayerHit);
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
        if (healthAmount < 0 && OnPlayerHit != null)
            OnPlayerHit.Invoke();
            
        _currentHealth += healthAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        UpdateHealthBarUI();

    }

    public void RestoreHealth()
    {

        _currentHealth = maxHealth;
        UpdateHealthBarUI();



    }


    public bool IsAlive()
    {
        return _currentHealth > 0;
    }


}
