using UnityEngine;
using UnityEngine.Events;
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPressureDown;
    [SerializeField] private UnityEvent OnPressureStay;
    [SerializeField] private UnityEvent OnPressureUp;

    [SerializeField] private GameObject pressed;
    [SerializeField] private GameObject unpressed;
    private void OnTriggerEnter(Collider other)
    {
        OnPressureDown?.Invoke();
        
        pressed.SetActive(true);
        unpressed.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        OnPressureStay?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        OnPressureUp?.Invoke();

        pressed.SetActive(false);
        unpressed.SetActive(true);
    }
}
