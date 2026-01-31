using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public bool IsWalled { get; private set; }
    private int wallContacts = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) return;
        wallContacts++;
        UpdateWalled();
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.isTrigger) return;
        wallContacts--;
        UpdateWalled();
    }

    private void UpdateWalled()
    {
        IsWalled = wallContacts > 0;
    }
}
