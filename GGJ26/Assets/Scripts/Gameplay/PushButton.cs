using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour, ITongueable
{
    [SerializeField] private UnityEvent OnPushed;
    private TongueData tongueData;
    private Vector3 hitPoint;
    public void OnTongued(Transform tongueOrigin, Transform playerTransform, TongueData tongueData, Vector3 hitPoint)
    {
        this.tongueData = tongueData;
        this.hitPoint = hitPoint;

        OnPushed?.Invoke();

        Timers.UntilThen(Constants.TONGUE_Speed / 2, () => { tongueData.StayAttached(hitPoint); }, () => { tongueData.ResetTongue(); });
    }
}
