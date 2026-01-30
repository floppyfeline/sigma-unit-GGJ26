using UnityEngine;

public class GroundWallCheck : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Vector3 groundBoxCenter = new Vector3(0f, -1f, 0f);
    [SerializeField] private Vector3 groundBoxSize = new Vector3(0.8f, 0.2f, 0.8f);

    [Header("Wall Check")]
    [SerializeField] private Vector3 wallBoxCenter = new Vector3(0f, 0f, 0.6f);
    [SerializeField] private Vector3 wallBoxSize = new Vector3(0.8f, 1.6f, 0.2f);

    [Header("Collision Layers")]
    [SerializeField] private LayerMask collisionMask;

    public bool IsGrounded { get; private set; }
    public bool IsTouchingWall { get; private set; }

    void FixedUpdate()
    {
        IsGrounded = CheckOverlap(groundBoxCenter, groundBoxSize);
        IsTouchingWall = CheckOverlap(wallBoxCenter, wallBoxSize);
    }

    bool CheckOverlap(Vector3 localCenter, Vector3 size)
    {
        Vector3 worldCenter = transform.TransformPoint(localCenter);

        Collider[] hits = Physics.OverlapBox(
            worldCenter,
            size * 0.5f,
            transform.rotation,
            collisionMask,
            QueryTriggerInteraction.Ignore
        );

        return hits.Length > 0;
    }

    void OnDrawGizmosSelected()
    {
        DrawBox(groundBoxCenter, groundBoxSize, Color.green);
        DrawBox(wallBoxCenter, wallBoxSize, Color.red);
    }

    void DrawBox(Vector3 localCenter, Vector3 size, Color color)
    {
        Gizmos.color = color;
        Matrix4x4 matrix = Matrix4x4.TRS(
            transform.TransformPoint(localCenter),
            transform.rotation,
            Vector3.one
        );

        Gizmos.matrix = matrix;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
