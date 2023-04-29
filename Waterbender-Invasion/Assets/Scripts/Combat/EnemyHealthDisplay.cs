using System;
using TMPro;
using UnityEngine;
using WaterbenderInvasion.Attributes;

namespace WaterbenderInvasion.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().text = "N/A";
                return;
            }
            else
            {
                Health health = _fighter.GetTarget();
                GetComponent<TextMeshProUGUI>().text = String.Format("{0}%", health.GetPercentage());
            } 
        }
    }
}
