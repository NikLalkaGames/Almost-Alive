using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public static HealthBar Instance { get; private set; }

        private Image _healthBarImage;

        public float Value => _healthBarImage.fillAmount;

        private void Awake() 
        {
            if (Instance == null) Instance = this;
        }

        private void Start() => _healthBarImage = transform.GetChild(1).GetComponent<Image>();

        public void SetValue(float value) => _healthBarImage.fillAmount = value;

    }
}
