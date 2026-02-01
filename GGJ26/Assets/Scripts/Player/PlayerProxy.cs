using UnityEngine;

public class PlayerProxy : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float interpolationSpeed = 3f;

    void Start()
    {
        PlayerController playerController = FindAnyObjectByType<PlayerController>();

        if (playerController != null)
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
        if(!GameManager.Instance.GetGameActive()) return;

        // Interpolate toward the player's position
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerTransform.position.x - verticalOffset, 0, playerTransform.position.z - verticalOffset), Time.deltaTime * interpolationSpeed);


        //transform.position = new Vector3(playerTransform.position.x - verticalOffset, 0, playerTransform.position.z - verticalOffset);
    }
}
