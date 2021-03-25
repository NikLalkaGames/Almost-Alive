using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public static HealthBarController healthBarStaticController;

    private Image HealthBarImage;

    private void Awake() 
    {
        healthBarStaticController = this;
    }

    private void Start()
    {
        HealthBarImage = transform.GetChild(1).GetComponent<Image>();
    }

    public void SetValue(float value)
    {
        HealthBarImage.fillAmount = value;
    }

    public float GetValue()
    {
        return HealthBarImage.fillAmount;
    }
}
