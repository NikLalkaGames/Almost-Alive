using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    private Image _healthBarImage;

    public float Value => _healthBarImage.fillAmount;

    private void Awake() 
    {
        if (instance == null) instance = this;
    }

    private void Start() => _healthBarImage = transform.GetChild(1).GetComponent<Image>();

    public void SetValue(float value) => _healthBarImage.fillAmount = value;

}
