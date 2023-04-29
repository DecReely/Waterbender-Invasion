using System;
using TMPro;
using UnityEngine;

namespace WaterbenderInvasion.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _baseStats;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0}", _baseStats.GetLevel());
        }
    }
}
