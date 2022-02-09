using UnityEngine;
public class AttackPoints : MonoBehaviour
{
    public Transform A, B;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(A.position, new Vector3(B.position.x, A.position.y, 0));
        Gizmos.DrawLine(new Vector3(B.position.x, A.position.y, 0), B.position);

        Gizmos.DrawLine(B.position, new Vector3(A.position.x, B.position.y, 0));
        Gizmos.DrawLine(new Vector3(A.position.x, B.position.y, 0), A.position);
    }
}