using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class OnTriggerEnterCall : MonoBehaviour
{
    //THIS IS THE ONE THAT WORKS
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent OnTriggerEnterCallback = new UnityEvent();
    public List<GameObject> GameObjectsToEnter = new();

    [SerializeField] private bool _callOnce = false;
    [HideInInspector] public bool RemoveAfterCall = false;
    private bool _called = false;
    private void OnTriggerEnter(Collider other)
    {
        if (_callOnce && _called) return;
        if (GameObjectsToEnter.Contains(other.gameObject))
        {
            OnTriggerEnterCallback.Invoke();
            if (RemoveAfterCall)
            {
                GameObjectsToEnter.Remove(other.gameObject);
                if(GameObjectsToEnter.Count == 0) Destroy(this);
            }
            if (_callOnce) _called = true;
        }
    }
}
