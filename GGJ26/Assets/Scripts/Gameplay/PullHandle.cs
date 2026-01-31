using UnityEngine;

public class PullHandle : MonoBehaviour, ITongueable
{
    [Tooltip("Time it takes for the player to reach the handle")]
    [SerializeField] private float pullSpeed = 0.2f;
    private Vector3 pullTo;
    private Transform playerTransform;

    private float pullTimer;
    private Vector3 initPos;
    public void OnTongued(Transform tongueOrigin, Transform pTransform)
    {
        playerTransform = pTransform;

        // Get true center of handle object
        Vector3 centerOfMe = new Vector3
        (
            transform.position.x - 0.5f,
            transform.position.y - 0.5f,
            transform.position.z - 0.5f
        );

        Vector3 toPlayer = playerTransform.position - centerOfMe;

        float dx = Mathf.Abs(toPlayer.x);
        float dz = Mathf.Abs(toPlayer.z);

        if (dx > dz)
        {
            // closer to X axis
            pullTo = centerOfMe + new Vector3(Mathf.Sign(toPlayer.x), 0f, 0f);
        }
        else
        {
            // closer to Z axis
            pullTo = centerOfMe + new Vector3(0f, 0f, Mathf.Sign(toPlayer.z));
        }

        pullTimer = pullSpeed;
        initPos = playerTransform.position;

        Timers.UntilThen(pullSpeed, () => { PullToHandle(); }, () => { } );
    }
    private void PullToHandle()
    {
        pullTimer -= Time.deltaTime;

        if (pullTimer <= 0f)
        {
            playerTransform.position = pullTo;
            return;
        }

        float t = 1f - (pullTimer / pullSpeed); 
        t = 1f - (1f - t) * (1f - t); // Ease Out code

        playerTransform.position = Vector3.Lerp(initPos, pullTo, t);
    }
}
