using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Collectible : MonoBehaviour, ITongueable
{
    private float _disappearDelay = .2f;
    private float _disappearDuration = 1.2f;
    private float _timeSinceTrigger = 0f;
    private Vector3 _startSize;
    public UnityEvent OnPickup;

    private Vector3 pullTo;
    private float pullTimer;
    private Vector3 initPos;
    private TongueData tongueData;
    private Vector3 hitPointOffset;
    private float minScale = 0.5f;

    private void Pickup()
    {
        if (IsCollected) return;
        IsCollected = true;

        DecalProjector[] decals = GetComponentsInChildren<DecalProjector>();
        for(int i = 0; i < decals.Length; i++)
        {
            decals[i].enabled = false;
        }

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

        hitPointOffset = hitPoint - transform.position;
        this.tongueData = tongueData;

        pullTo = tongueData.TongueExtent.position - hitPointOffset + tongueOrigin.forward * 0.25f + Vector3.down * 0.25f;

        pullTimer = Constants.TONGUE_Speed / 2;
        initPos = transform.position - hitPointOffset;

        Timers.UntilThen(Constants.TONGUE_Speed / 2, () => { PullToPlayer(); }, () => { tongueData.ResetTongue(); });
    }
    public bool IsCollected { get; private set; } = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void Update()
    {
        if(tongueData != null) transform.position = tongueData.TongueTip.position;
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

        transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(minScale, minScale, minScale), t);

        tongueData.StayAttached(transform.position + hitPointOffset);
    }
}
