using System;
using TMPro;
using UnityEngine;

namespace WaterbenderInvasion.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;
        
        public void DestroyText()
        {
            Destroy(gameObject);
        }
        
        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}", amount);
        }
    }
}
