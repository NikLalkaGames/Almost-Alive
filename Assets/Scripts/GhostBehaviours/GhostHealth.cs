using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    # region Fields
    
    // health fields
    [SerializeField] private float _maxHealth;
    private float _health;
    [SerializeField] private float _healthReductionValue;
    private HealthBar _healthBar;

    // iframe vairalbes
    public float _timeInvincible = 1f;
    public float _invincibleTimer;

    # endregion

    # region Methods

    void Start()
    {
        _health = _maxHealth;
        _healthBar = HealthBar.instance;
    }

    public void Damage(int damageAmount)
    {
        if (_invincibleTimer < 0)
        {
            _health += damageAmount;
            if (_health > 0)
            {
                _healthBar.SetValue(_health / _maxHealth);
                _invincibleTimer = _timeInvincible;
                Debug.Log("Health has been reduced: " + _health / _maxHealth);
            }
            else
            {
                SceneLoader.instance.LoadScene("EntryMenu");            // TODO: later change to restart scene
            }
        }
    }

    public void Heal(int healAmount)
    {
        _health = Mathf.Clamp(_health + healAmount, 0, _maxHealth);
        _healthBar.SetValue(_health / _maxHealth);
        Debug.Log("Health has been restored: " + _health / _maxHealth);
    }

    public void UpdateHealth(int amount)
    {
        if (amount < 0)
        {
            Damage(amount);
        }
        else
        {
            Heal(amount);
        }
    }


    // TODO: Delete on release
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UpdateHealth(-10);
            _invincibleTimer -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateHealth(+30);
        }
    }

    private void FixedUpdate() 
    {
        if (_invincibleTimer >= 0) _invincibleTimer -= Time.fixedDeltaTime;
        
        if (_health > 0)
        {
            _health -= _healthReductionValue;
            _healthBar.SetValue(_health / _maxHealth);
        }
        else
        {
            SceneLoader.instance.LoadScene("EntryMenu");
        }
    }

    public void IncreaseHealthReduction(float value = 0.001f) => _healthReductionValue += value;

    # endregion
}
