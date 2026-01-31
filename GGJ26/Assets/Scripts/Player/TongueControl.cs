using UnityEngine;

public class TongueControl : MonoBehaviour
{
    [SerializeField] private Transform tongueTo;
    [SerializeField] private float tongueRange = 5f;
    public void LaunchTongue()
    {
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, tongueRange, Constants.LAYER_Tongueable))
        {
            if (hit.transform.TryGetComponent(out ITongueable tongueable))
            {
                tongueable.OnTongued(tongueTo);
            }
        }
    }
}
