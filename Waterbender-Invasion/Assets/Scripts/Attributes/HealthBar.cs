using UnityEngine;

namespace WaterbenderInvasion.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;
        
        void Update()
        {
            if ( Mathf.Approximately(health.GetFraction(), 0) || 
                 Mathf.Approximately(health.GetFraction(), 1)  )
            {
                rootCanvas.enabled = false;
                return;
            }
            
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(health.GetFraction(), 1, 1);
        }
    }
}
