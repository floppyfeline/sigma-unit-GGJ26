using UnityEngine;

public class BirdPickup : MonoBehaviour
{
    [SerializeField] private Vector3 InitOffsetToPlayer;
    [SerializeField] private float moveSpeed = 5f;
    public void CatchPlayer()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        
    }
}
