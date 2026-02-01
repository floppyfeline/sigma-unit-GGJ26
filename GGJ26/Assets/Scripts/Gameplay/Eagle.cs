using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Eagle : MonoBehaviour
{
    [SerializeField] private List<Transform> raycastSocketOffsets;
    [SerializeField] private DecalProjector decalProjector;

    // ===== Detection =====
    private float lastSeenTime = -999f;
    private float detectionTimer = 0f;

    void Start()
    {
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

    // =========================================================
    // Detection Logic (time-based, physics safe)
    // =========================================================
    private void HandleDetection()
    {
        // Seen within last frame's worth of time?
        bool seenRecently = Time.time - lastSeenTime <= Time.deltaTime;

        if (seenRecently)
        {
            detectionTimer += Time.deltaTime;

            float caughtProgress = detectionTimer / Constants.EAGLE_DetectionTime;
            Debug.Log($"Eagle caught progress: {caughtProgress:P0}");

            if (detectionTimer >= Constants.EAGLE_DetectionTime)
            {
                GameManager.Instance.PlayerCaught();
            }
        }
        else
        {
            detectionTimer = 0f;
        }
    }

    // Called by ANY trigger that sees the player
    private void SeeingPlayer()
    {
        lastSeenTime = Time.time;
    }

    // =========================================================
    // Decal Projector Resize
    // =========================================================
    private void ResizeDecal()
    {
        float highestRayHitDistance = 0f;

        for (int i = 0; i < raycastSocketOffsets.Count; i++)
        {
            if (Physics.Raycast(raycastSocketOffsets[i].position, Vector3.down, out var hit, 10f))
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