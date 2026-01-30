using UnityEngine;
using UnityEngine.Events;

public class OnTriggerExitCall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent OnTriggerExitCallback;
    public GameObject GameObjectToExit;

    [SerializeField] private bool _callOnce;
    private bool _called = false;
    private void OnTriggerExit(Collider other)
    {
        if (_callOnce && _called) return;
        if (other.gameObject == GameObjectToExit)
        {
            OnTriggerExitCallback.Invoke();
            if (_callOnce) _called = true;
        }
    }
}