using System;
using TMPro;
using UnityEngine;

namespace WaterbenderInvasion.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0}", _experience.GetPoints());
        }
    }
}
