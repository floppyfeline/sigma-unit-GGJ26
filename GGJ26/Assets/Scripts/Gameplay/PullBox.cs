using UnityEngine;

public class PullBox : MonoBehaviour, ITongueable
{
    private Vector3 pullTo;

    private float pullTimer;
    private Vector3 initPos;
    private TongueData tongueData;
    private Vector3 hitPointOffset;

    public void OnTongued(Transform tongueOrigin, Transform playerTransform, TongueData tongueData, Vector3 hitPoint)
    {
        hitPointOffset = hitPoint - transform.position;
        this.tongueData = tongueData;

        Vector3 clampedTonguePos = tongueOrigin.position + tongueOrigin.forward * 0.75f;;
        clampedTonguePos += new Vector3(0.5f, 0.5f, 0.5f);

        clampedTonguePos = new Vector3
        (
            Mathf.RoundToInt(clampedTonguePos.x),
            Mathf.RoundToInt(clampedTonguePos.y),
            Mathf.RoundToInt(clampedTonguePos.z)
        );

        pullTo = clampedTonguePos;

        pullTimer = Constants.TONGUE_Speed / 2;
        initPos = transform.position;

        Timers.UntilThen(Constants.TONGUE_Speed / 2, () => { PullToPlayer(); }, () => { tongueData.ResetTongue(); });
    }
    
    private void PullToPlayer()
    {
        pullTimer -= Time.deltaTime;

        if (pullTimer <= 0f)
        {
            transform.position = pullTo;
            return;
        }

        float t = 1f - (pullTimer / (Constants.TONGUE_Speed / 2)); 
        t = 1f - (1f - t) * (1f - t); // Ease Out code

        transform.position = Vector3.Lerp(initPos, pullTo, t);

        tongueData.StayAttached(transform.position + hitPointOffset);
    }
}
