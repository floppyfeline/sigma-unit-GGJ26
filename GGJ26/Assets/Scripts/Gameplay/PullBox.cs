using UnityEngine;

public class PullBox : MonoBehaviour, ITongueable
{
    [Tooltip("Time it takes for the box to reach the player")]
    [SerializeField] private float pullSpeed = 0.2f;
    private bool pulling = false;
    private Vector3 pullTo;

    private float pullTimer;
    private Vector3 initPos;

    public void OnTongued(Transform tongueOrigin, Transform playerTransform)
    {
        pulling = true;
        Vector3 clampedTonguePos = tongueOrigin.position;
        clampedTonguePos += new Vector3(0.5f, 0.5f, 0.5f);

        clampedTonguePos = new Vector3
        (
            Mathf.RoundToInt(clampedTonguePos.x),
            Mathf.RoundToInt(clampedTonguePos.y),
            Mathf.RoundToInt(clampedTonguePos.z)
        );

        pullTo = clampedTonguePos;

        pullTimer = pullSpeed;
        initPos = transform.position;

        Timers.UntilThen(pullSpeed, () => { PullToPlayer(); }, () => { pulling = false;} );
    }
    
    private void PullToPlayer()
    {
        pullTimer -= Time.deltaTime;

        if (pullTimer <= 0f)
        {
            pulling = false;
            transform.position = pullTo;
            return;
        }

        float t = 1f - (pullTimer / pullSpeed); 
        t = 1f - (1f - t) * (1f - t); // Ease Out code

        transform.position = Vector3.Lerp(initPos, pullTo, t);
    }
}
