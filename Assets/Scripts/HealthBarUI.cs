using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Image _healthBarBackgroundImage;
    [SerializeField] private Unit _unit;

    private void Start() {
        _healthBarImage.fillAmount = 1f;
        Hide();
    }

    private void Unit_HealthChanged(float healthPercentage)
    {
        _healthBarImage.fillAmount = healthPercentage;
        Debug.Log($"Health percentage: {healthPercentage}");
        if (healthPercentage <= 0f || healthPercentage == 1f)
        {
            Debug.Log($"0 health or 100% health");
            Hide();
        }
        else
        {
            Debug.Log($"Health: {healthPercentage}");
            Show();
        }
    }

    public void Hide()
    {
        _healthBarBackgroundImage.enabled = false;
        _healthBarImage.enabled = false;
    }

    public void Show()
    {
        _healthBarBackgroundImage.enabled = true;
        _healthBarImage.enabled = true;
    }

    private void OnEnable() 
    {
        _unit.HealthChanged += Unit_HealthChanged;
    }

    private void OnDisable() 
    {
        _unit.HealthChanged -= Unit_HealthChanged;
    }
}
