using UnityEngine;

public class PullHandle : MonoBehaviour, ITongueable
{
    [Tooltip("Time it takes for the player to reach the handle")]
    [SerializeField] private float pullSpeed = 0.2f;
    private bool pulling = false;
    private Vector3 pullTo;
    private Transform playerTransform;

    private float pullTimer;
    private Vector3 initPos;
    public void OnTongued(Transform tongueOrigin, Transform pTransform)
    {
        playerTransform = pTransform;
        pulling = true;

        Vector3 centerOfMe = new Vector3
        (
            transform.position.x - 0.5f,
            transform.position.y - 0.5f,
            transform.position.z - 0.5f
        );
        Vector3 towardPlayer = (playerTransform.position - centerOfMe).normalized;

        Vector3 towardPlayerPos = transform.position + towardPlayer;

        Vector3 clampedTowardPlayerPos = towardPlayerPos;
        clampedTowardPlayerPos += new Vector3(0.5f, 0.5f, 0.5f);

        clampedTowardPlayerPos = new Vector3
        (
            Mathf.RoundToInt(clampedTowardPlayerPos.x),
            Mathf.RoundToInt(clampedTowardPlayerPos.y),
            Mathf.RoundToInt(clampedTowardPlayerPos.z)
        );

        pullTo = clampedTowardPlayerPos;

        // Snap to grid
        pullTo = new Vector3
        (
            pullTo.x - 0.5f,
            pullTo.y - 0.5f,
            pullTo.z - 0.5f
        );

        pullTimer = pullSpeed;
        initPos = playerTransform.position;

        Timers.UntilThen(pullSpeed, () => { PullToHandle(); }, () => { pulling = false;} );
    }
    private void PullToHandle()
    {
        pullTimer -= Time.deltaTime;

        if (pullTimer <= 0f)
        {
            pulling = false;
            playerTransform.position = pullTo;
            return;
        }

        float t = 1f - (pullTimer / pullSpeed); 
        t = 1f - (1f - t) * (1f - t); // Ease Out code

        playerTransform.position = Vector3.Lerp(initPos, pullTo, t);
    }
}
