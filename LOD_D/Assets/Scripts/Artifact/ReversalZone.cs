using UnityEngine;

public class ReversalZone : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = new Color(0, 1, 1, 0.2f);
    
    private void OnDrawGizmos()
    {
        // แสดงพื้นที่ที่สามารถใช้ artifact ได้ใน editor
        Gizmos.color = gizmoColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}