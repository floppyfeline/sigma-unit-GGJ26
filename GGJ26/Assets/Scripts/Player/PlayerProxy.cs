using UnityEngine;

public class PlayerProxy : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float verticalOffset;

    void Start()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();

        if(playerController != null)
        {
            playerTransform = playerController.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x - verticalOffset, 0, playerTransform.position.z - verticalOffset);
    }
}
