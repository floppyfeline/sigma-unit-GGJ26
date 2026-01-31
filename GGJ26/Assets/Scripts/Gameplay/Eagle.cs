using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Eagle : MonoBehaviour
{
    [SerializeField] private List<Transform> raycastSocketOffsets;
    [SerializeField] private DecalProjector decalProjector;
    void Start()
    {

    }

    void Update()
    {
        float highestRayHitDistance = 0f;
        for(int i = 0; i < raycastSocketOffsets.Count; i++)
        {
            RaycastHit hit;
            Vector3 rayStart = raycastSocketOffsets[i].position;
            Vector3 rayDirection = Vector3.down;
            float rayLength = 10f;

            if (Physics.Raycast(rayStart, rayDirection, out hit, rayLength))
            {
                if (hit.distance > highestRayHitDistance)
                {
                    highestRayHitDistance = hit.distance;
                }
            }
        }

        decalProjector.size = new Vector3(decalProjector.size.x, decalProjector.size.y, highestRayHitDistance + 2f);
    }
}
