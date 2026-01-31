using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RaycastCollider : MonoBehaviour
{
    private BoxCollider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }


    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Constants.LAYER_Default))
        {
            triggerCollider.center = hit.point - new Vector3(0, triggerCollider.size.y / 2f, 0);
        }
        else
        {
            triggerCollider.center = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_Player) && other.TryGetComponent(out PlayerColourManager player))
        {
            bool isHidden = false;
        }
    }
}
