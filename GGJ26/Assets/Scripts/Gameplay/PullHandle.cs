using UnityEngine;

public class PullHandle : MonoBehaviour, ITongueable
{
    private Vector3 pullTo;
    private Transform playerTransform;

    private float pullTimer;
    private Vector3 initPos;
    private Vector3 hitPoint;
    private TongueData tongueData;

    public void OnTongued(Transform tongueOrigin, Transform pTransform, TongueData tongueData, Vector3 hitPoint)
    {
        playerTransform = pTransform;

        this.hitPoint = hitPoint;
        this.tongueData = tongueData;

        // Get true center of handle object
        Vector3 centerOfMe = new
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

        pullTimer = Constants.TONGUE_Speed / 2;
        initPos = playerTransform.position;

        Timers.UntilThen(Constants.TONGUE_Speed / 2, () => { PullToHandle(); }, () => { tongueData.ResetTongue(); });
    }
    private void PullToHandle()
    {
        tongueData.StayAttached(hitPoint);

        pullTimer -= Time.deltaTime;

        if (pullTimer <= 0f)
        {
            playerTransform.position = pullTo;
            return;
        }

        float t = 1f - (pullTimer / (Constants.TONGUE_Speed / 2)); 
        t = 1f - (1f - t) * (1f - t); // Ease Out code

        playerTransform.position = Vector3.Lerp(initPos, pullTo, t);
    }
}
