using System.Linq;
using UnityEngine;

public class BirdPickup : MonoBehaviour
{
    [SerializeField] private Vector3 initOffsetToPlayer;
    [SerializeField] private Vector3 carryToOffset;
    [SerializeField] private Vector3 pickUpOffset;
    [SerializeField] private float catchTime = 1.5f;

    private Transform player;
    private Vector3 stashedPlayerPos;

    private bool playerApproach = false;
    private bool playerCarry = false;

    private float timer;

    public void CatchPlayer(Transform player)
    {
        gameObject.SetActive(true);

        AudioManager.Instance.PlayAudioClip("EagleScreech");

        this.player = player;
        transform.position = player.position + initOffsetToPlayer;
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);

        playerApproach = true;
        playerCarry = false;

        timer = catchTime;
    }

    private void Update()
    {
        if (player == null) return;

        timer -= Time.deltaTime;

        if (playerApproach && !playerCarry)
        {
            transform.position = Vector3.Lerp(player.position + initOffsetToPlayer, player.position + pickUpOffset, 1 - (timer / catchTime));

            if(timer < 0)
            {
                timer = catchTime;
                playerApproach = false;
                playerCarry = true;

                stashedPlayerPos = player.position;

                player.SetParent(transform);

                Collider[] playerColliders = player.GetComponents<Collider>();
                for(int i = 0; i < playerColliders.Length; i ++)
                {
                    playerColliders[i].enabled = false;
                }
            }
        }

        if (playerCarry && !playerApproach)
        {
            transform.position = Vector3.Lerp(stashedPlayerPos + pickUpOffset, stashedPlayerPos + carryToOffset, 1 - (timer / catchTime));

        Collider[] playerColliders = player.GetComponents<Collider>();
        for(int i = 0; i < playerColliders.Length; i ++)
        {
            playerColliders[i].enabled = true;
        }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(carryToOffset, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(initOffsetToPlayer, 0.2f);
    }
}