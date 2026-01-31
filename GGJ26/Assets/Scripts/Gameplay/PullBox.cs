using UnityEngine;

public class PullBox : MonoBehaviour, ITongueable
{
    [Tooltip("Time it takes for the box to reach the player")]
    [SerializeField] private float pullSpeed = 0.2f;
    private bool pulling = false;
    private Vector3 pullTo;

    private float pullTimer;
    private Vector3 initPos;

    public void OnTongued(Transform tongueOrigin)
    {
        pulling = true;
        pullTo = tongueOrigin.position;

        pullTimer = pullSpeed;
        initPos = transform.position;
    }
    
    private void PullToPlayer()
    {
        pullTimer -= Time.deltaTime;

        if (pullTimer <= 0f)
        {
            pulling = false;
            transform.position = pullTo;
            return;
        }

        float t = 1f - (pullTimer / pullSpeed); 
        t = 1f - (1f - t) * (1f - t); // Ease Out code

        transform.position = Vector3.Lerp(initPos, pullTo, t);
    }


    // Update is called once per frame
    void Update()
    {
        if(pulling)
        {
            PullToPlayer();
        }
    }
}
