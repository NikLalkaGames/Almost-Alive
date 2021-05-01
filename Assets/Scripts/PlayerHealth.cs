using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public float healthReduceValue;
    private HealthBarController healthBar;

    // iframe vairalbes
    public float timeInvincible = 1f;
    public float invincibleTimer;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar = HealthBarController.healthBarStaticController;
    }

    // Update is called once per frame

    public void Damage(int damageAmount)
    {
        if (invincibleTimer < 0)
        {
            health += damageAmount;
            if (health > 0)
            {
                healthBar.SetValue(health / maxHealth);
                invincibleTimer = timeInvincible;
                Debug.Log("Health has been reduced: " + health / maxHealth);
            }
            else
            {
                SceneLoader.instance.LoadScene("EntryMenu");
            }

        }
    }

    public void Heal(int healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, maxHealth);
        healthBar.SetValue(health / maxHealth);
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

    private void Update()
    {
        if (invincibleTimer >= 0) invincibleTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            UpdateHealth(-10);
            invincibleTimer -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateHealth(+30);
        }
    }

    private void FixedUpdate() 
    {
        GradualHealthDecrease();
    }

    private void GradualHealthDecrease()
    {
        if (health > 0)
        {
            health -= healthReduceValue;
            healthBar.SetValue(health / maxHealth);
        }
        else
        {
            SceneLoader.instance.LoadScene("EntryMenu");
        }
    }
}
