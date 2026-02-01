using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class RaycastCollider : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<int> SeeingPlayer = new();
    private bool isSeen;
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
    private void SeePlayer()
    {
        if(isSeen) return;
        
        SeeingPlayer?.Invoke(1);
        isSeen = true;
    }
    private void LosePlayer()
    {
        if(!isSeen) return;
        SeeingPlayer?.Invoke(-1);
        isSeen = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_Player) && other.TryGetComponent(out PlayerColourManager player))
        {
            player.OnHide.AddListener(LosePlayer);
            player.OnShow.AddListener(SeePlayer); 



            if(!player.IsHidden) SeePlayer();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.TAG_Player) && other.TryGetComponent(out PlayerColourManager player))
        {
            player.OnHide.RemoveListener(LosePlayer);
            player.OnShow.RemoveListener(SeePlayer); 


            LosePlayer();
        }
    }
}
