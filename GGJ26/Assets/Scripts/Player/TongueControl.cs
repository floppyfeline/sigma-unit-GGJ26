using UnityEngine;

public class TongueControl : MonoBehaviour
{
    [SerializeField] private Transform tongueTo;
    [SerializeField] private float tongueRange = 5f;

    private PlayerInputs moveInput;
    private Transform playerTransform;
    private bool rotationEnabled = false;
    public void LaunchTongue()
    {
        ToggleRotation(false);

        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, tongueRange, Constants.LAYER_Tongueable))
        {
            if (hit.transform.TryGetComponent(out ITongueable tongueable))
            {
                tongueable.OnTongued(tongueTo, playerTransform);
            }
        }
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
    }
    public void SetInputs(PlayerInputs inputs, Transform pTransform)
    {
        moveInput = inputs;
        playerTransform = pTransform;
    }
}
