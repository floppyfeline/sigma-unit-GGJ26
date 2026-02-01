using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class RaycastCollider : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent SeeingPlayer = new();
    private BoxCollider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, Constants.LAYER_Default, QueryTriggerInteraction.Ignore))
        {
            triggerCollider.center = transform.InverseTransformPoint(hit.point + triggerCollider.size.y / 2 * Vector3.up);
        }
        else
        {
            triggerCollider.center = Vector3.zero;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.TAG_Player) && other.TryGetComponent(out PlayerColourManager player))
        {
            if(player.IsHidden)
                return;

            SeeingPlayer?.Invoke();
        }
    }
}
