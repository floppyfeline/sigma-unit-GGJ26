using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour, ITongueable
{
    [SerializeField] private UnityEvent OnPushed;
    [SerializeField] private GameObject pressed;
    [SerializeField] private GameObject unpressed;
    public void OnTongued(Transform tongueOrigin, Transform playerTransform, TongueData tongueData, Vector3 hitPoint)
    {
        OnPushed?.Invoke();


        TogglePressed(true);
        Timers.UntilThen(Constants.TONGUE_Speed / 2, () => 
        { 
            tongueData.StayAttached(hitPoint); 
        }, () => 
        { 
            tongueData.ResetTongue(); 
            TogglePressed(false);
        }); 
    }
    void Start()
    {
        TogglePressed(false);
    }
    void TogglePressed(bool state)
    {
        pressed.SetActive(state);
        unpressed.SetActive(!state);
    }
}
