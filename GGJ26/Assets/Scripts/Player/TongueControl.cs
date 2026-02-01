using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TongueData
{
    // Will only be rotated and scaled on the Z
    public Transform TongueExtent;
    // Seperate object, so needs to be turned off indepentently
    public Transform TongueTip;
    public Action OnReset;
  
    public void ResetTongue()
    {
        TongueExtent.localScale = new Vector3(Constants.TONGUE_Thickness, Constants.TONGUE_Thickness, Constants.TONGUE_Thickness);
        TongueExtent.rotation = Quaternion.identity;
        TongueExtent.gameObject.SetActive(false);
        TongueTip.gameObject.SetActive(false);
        OnReset?.Invoke();  
    }
    public void StayAttached(Vector3 target)
    {
        TongueExtent.rotation = Quaternion.LookRotation(target - TongueExtent.position).normalized;
        TongueExtent.localScale = new Vector3(Constants.TONGUE_Thickness, Constants.TONGUE_Thickness, (TongueExtent.position - target).magnitude);
    }
}
public class TongueControl : MonoBehaviour
{
    [SerializeField] private Transform tongueOrigin;
    [SerializeField] private Transform tongueVisualOrigin;
    [SerializeField] private float tongueRange = 5f;
    [SerializeField] private float tongueCooldown = 1f;

    private PlayerInputs moveInput;
    private Transform playerTransform;
    [SerializeField] private TongueData tongueData;
    private bool rotationEnabled = false;
    private bool tongueLaunched = false;
    private bool tongueOnCooldown = false;
    private float tongueTimer = 0f;
    private ITongueable currentTarget;
    private Vector3 hitPoint;

    public UnityEvent OnTongueLaunched;
    public UnityEvent OnTongueRetracted;
    private void Start()
    {
        tongueData.OnReset += () =>
        {
           OnTongueRetracted?.Invoke();
        };
    }
    void OnEnable()
    {
        tongueData.ResetTongue();
    }

    public void LaunchTongue()
    {
        if (tongueOnCooldown || !GameManager.Instance.GetGameActive()) return;

        tongueOnCooldown = true;
        Timers.After(tongueCooldown, () => { tongueOnCooldown = false; });

        ToggleRotation(false);

        tongueData.TongueExtent.gameObject.SetActive(true);
        tongueData.TongueTip.gameObject.SetActive(true);
        
        if (Physics.SphereCast(transform.position, 0.25f, transform.forward, out RaycastHit hit, tongueRange, Constants.LAYER_Tongueable))
        {
            if (hit.transform.TryGetComponent(out ITongueable tongueable))
            {
                currentTarget = tongueable;
                tongueLaunched = true;
                tongueTimer = Constants.TONGUE_Speed / 2;

                hitPoint = hit.point;
            }
        }
        else
        {
            currentTarget = null;
            hitPoint = transform.position + (transform.forward * tongueRange);
            tongueLaunched = true;

            tongueTimer = Constants.TONGUE_Speed / 2;
        }
        OnTongueLaunched?.Invoke();
    }

    public void ToggleRotation(bool toggle)
    {
        rotationEnabled = toggle;
    }

    private void HandleRotation()
    {
        // Clamp rotation to 90° Steps
        if (moveInput.Move != Vector2.zero)
        {
            Vector3 camForward = moveInput.CameraRotation * Vector3.forward;
            Vector3 camRight   = moveInput.CameraRotation * Vector3.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection =
                camRight * moveInput.Move.x +
                camForward * moveInput.Move.y;

            Vector3 moveVector = moveDirection;

            Vector3 moveDir =
                camRight * moveInput.Move.x +
                camForward * moveInput.Move.y;

            moveDir.y = 0f;
            moveDir.Normalize();

            // 0° reference = world +Z
            Vector2 move2D = new Vector2(moveDir.x, moveDir.z);

            float angle = Mathf.Atan2(move2D.x, move2D.y) * Mathf.Rad2Deg;

            float snapped = Mathf.Round(angle / 90f) * 90f;

            playerTransform.rotation = Quaternion.Euler(0f, snapped, 0f);
        }
    }

    private void Update()
    {
        if (rotationEnabled) HandleRotation();

        if (tongueLaunched)
        {
            TravelTongue();
        }
    }
    public void SetInputs(PlayerInputs inputs, Transform pTransform)
    {
        moveInput = inputs;
        playerTransform = pTransform;
    }

    private void TravelTongue()
    {
        // Look toward the hit point
        tongueData.TongueExtent.rotation = Quaternion.LookRotation(hitPoint - tongueData.TongueExtent.position);
        
        // Scale the tongue's z axis with the exact distance between the tongue's origin and the hit point to connect the two over time
        tongueData.TongueExtent.localScale = new Vector3
        (
            tongueData.TongueExtent.localScale.x,
            tongueData.TongueExtent.localScale.y,
            Mathf.Lerp(0f, Vector3.Distance(tongueData.TongueExtent.position, hitPoint), 1f - (tongueTimer / (Constants.TONGUE_Speed / 2)))
        );
        
        // As soon as the tongue has reached the target, trigger the OnTongued Method on the Target
        if (tongueTimer <= 0f)
        {
            if(currentTarget != null)
            {
                currentTarget.OnTongued(tongueVisualOrigin, playerTransform, tongueData, hitPoint);
                tongueLaunched = false;
            }
            else
            {
                float fullLength = Vector3.Distance(tongueData.TongueExtent.position, hitPoint);

                // 0 → 1 over the retract duration
                float t = -tongueTimer / (Constants.TONGUE_Speed / 2);

                tongueData.TongueExtent.localScale = new Vector3
                (
                    Constants.TONGUE_Thickness,
                    Constants.TONGUE_Thickness,
                    Mathf.Lerp(fullLength, 0f, t)
                );

                if (tongueTimer <= -Constants.TONGUE_Speed / 2)
                {
                    tongueData.ResetTongue();
                    OnTongueRetracted?.Invoke();
                    tongueLaunched = false;
                }
            }
        }
        tongueTimer -= Time.deltaTime;
    }
}
