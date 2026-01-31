using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour, ITongueable
{
    [SerializeField] private UnityEvent OnPushed;
    public void OnTongued(Transform tongueOrigin, Transform playerTransform)
    {
        OnPushed?.Invoke();
    }
}
