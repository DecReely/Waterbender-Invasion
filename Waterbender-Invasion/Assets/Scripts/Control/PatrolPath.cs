using UnityEngine;

namespace WaterbenderInvasion.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WaypointGizmosRadius = 0.3f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), WaypointGizmosRadius);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(GetNextIndex(i)).position);
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }
    }
}
