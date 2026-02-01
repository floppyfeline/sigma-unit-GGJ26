using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Eagle : MonoBehaviour
{
    [SerializeField] private List<Transform> raycastSocketOffsets;
    [SerializeField] private DecalProjector decalProjector;

    // ===== Detection =====
    private float detectionTimer = 10f;
    private int collidersSeeingPlayer = 0;

    void Start()
    {
        detectionTimer = Constants.EAGLE_DetectionTime;

        for (int i = 0; i < raycastSocketOffsets.Count; i++)
        {
            raycastSocketOffsets[i]
                .GetComponent<RaycastCollider>()
                .SeeingPlayer
                .AddListener(SeeingPlayer);
        }
    }

    void Update()
    {
        HandleDetection();
        ResizeDecal();
    }

    // Detection Logic
    private void HandleDetection()
    {
        if(!GameManager.Instance.GetGameActive()) return;

        if(collidersSeeingPlayer <= 0)
            detectionTimer = Constants.EAGLE_DetectionTime;

        detectionTimer -= Time.deltaTime;

        if(detectionTimer <= 0) 
        {
            GameManager.Instance.PlayerCaught();
            return;
        }
    }

    // Called by ANY trigger that sees the player
    private void SeeingPlayer(int i)
    {
        collidersSeeingPlayer += i;

        if(collidersSeeingPlayer <= 0) 
        {
            collidersSeeingPlayer = 0;
            return;
        }
        
    }

    // Decal Projector Resize

    private void ResizeDecal()
    {
        float highestRayHitDistance = 0f;

        for (int i = 0; i < raycastSocketOffsets.Count; i++)
        {
            if (Physics.SphereCast(raycastSocketOffsets[i].position, 0.33f, Vector3.down, out var hit, 15f))
            {
                highestRayHitDistance = Mathf.Max(highestRayHitDistance, hit.distance);
            }
        }

        decalProjector.size = new Vector3(
            decalProjector.size.x,
            decalProjector.size.y,
            highestRayHitDistance + 2f
        );
    }
}