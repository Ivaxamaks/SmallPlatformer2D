using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HealthBar
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField, NotNull] private Slider _slider;
        [SerializeField, NotNull] private Gradient _gradient;
        [SerializeField, NotNull] private Image _fill;

        private IHealth _healthController;
    
        public void Initialize(IHealth health)
        {
            _slider.interactable = false;
            _healthController = health;
            _healthController.HealthChanged += SetHealthValue;

            UpdatePosition();
            UpdateScale();
            if(!_healthController.IsInitialized) return;
            SetHealthValue(health.Health, health.MaxHealth);
        }

        private void Update()
        {
            var isInitialized = _healthController != null;
            if(!isInitialized || _healthController.IsDead) return;
            UpdatePosition();
        }

        private void SetHealthValue(float health, float maxHealth)
        {
            _slider.value = health / maxHealth;
            var color = _gradient.Evaluate(_slider.value);
            _fill.color = color;
        }

        private void UpdatePosition()
        {
            transform.position = _healthController.HealthBarAnchor.position;
        }

        private void UpdateScale()
        {
            transform.localScale = _healthController.HealthBarAnchor.localScale;
        }
    }
}