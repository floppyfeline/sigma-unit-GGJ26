using UnityEngine;

public class BirdPickup : MonoBehaviour
{
    [SerializeField] private Vector3 initOffsetToPlayer;
    [SerializeField] private Vector3 carryToOffset;
    [SerializeField] private float moveSpeed = 5f;

    private Transform player;

    private bool playerApproach = false;
    private bool playerCarry = false;

    public void CatchPlayer(Transform player)
    {
        gameObject.SetActive(true);

        this.player = player;
        transform.position = player.position + initOffsetToPlayer;
        transform.rotation = Quaternion.LookRotation(new Vector3(0, player.position.y - transform.position.y, 0));

        playerApproach = true;
        playerCarry = false;
    }

    private void Update()
    {
        if (player == null) return;

        if (playerApproach && !playerCarry)
        {
            Vector3 target = player.position + initOffsetToPlayer;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.SqrMagnitude(transform.position - target) < 0.001f)
            {
                player.SetParent(transform);
                playerApproach = false;
                playerCarry = true;
            }
        }

        if (playerCarry && !playerApproach)
        {
            Vector3 target = player.position + carryToOffset;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
    }
}