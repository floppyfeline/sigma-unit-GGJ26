using UnityEngine;
using UnityEngine.Events;   
public class Collectible : MonoBehaviour, ITongueable
{
    private float _disappearDelay = .2f;
    private float _disappearDuration = 1.2f;
    private float _timeSinceTrigger = 0f;
    private Vector3 _startSize;
    public UnityEvent OnPickup;

    private void Pickup()
    {
        if (IsCollected) return;
        IsCollected = true;
        PaletteManager.Instance.SetRandomPalette();
        GameManager.Instance.OnCollectiblePickedUp();
        Timers.After(_disappearDelay, () => {
            Timers.UntilThen(_disappearDuration, () =>
            {
                _timeSinceTrigger += Time.deltaTime;
                transform.localScale = Vector3.Lerp(_startSize, Vector3.zero, _timeSinceTrigger / _disappearDuration);
            },
           () =>
           {
               gameObject.SetActive(false);
           });
        });
        OnPickup?.Invoke();
    }
    public virtual void OnTongued(Transform tongueOrigin, Transform playerTransform, TongueData tongueData, Vector3 hitPoint)
    {
        Pickup();
        tongueData.ResetTongue();
    }
    public bool IsCollected { get; private set; } = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }
}
