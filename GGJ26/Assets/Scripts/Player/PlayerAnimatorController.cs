using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private const string WALK_BOOL = "isWalking";
    private const string TONGUE_BOOL = "isTongueing";

    private PlayerController _controller;
    private Animator _animator;

    private void Start()
    {
        _controller = GetComponentInParent<PlayerController>();
        _animator = GetComponent<Animator>();
        _controller.OnMove += SetMoveBool;
    }
    private void SetMoveBool(bool enabled)
    {
        _animator.SetBool(WALK_BOOL, enabled);
    }
    public void SetTongueBool(bool enabled)
    {
        Debug.Log("Setting tongue bool to " + enabled);
        _animator.SetBool(TONGUE_BOOL, enabled);
    }
    }
