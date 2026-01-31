using UnityEngine;

public class GroundedCheck : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    private int groundContacts = 0;

    private void OnTriggerEnter(Collider other)
    {
        groundContacts++;
        UpdateGrounded();
    }
    private void OnTriggerExit(Collider other)
    {
        groundContacts--;
        UpdateGrounded();
    }

    private void UpdateGrounded()
    {
        IsGrounded = groundContacts > 0;
    }
}
