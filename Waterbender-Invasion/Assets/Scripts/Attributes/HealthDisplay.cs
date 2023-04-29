using System;
using TMPro;
using UnityEngine;

namespace WaterbenderInvasion.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0}%", _health.GetPercentage());
        }
    }
}
