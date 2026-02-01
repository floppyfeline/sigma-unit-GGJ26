using UnityEngine;
using UnityEngine.InputSystem;
public class DebugRandomPalette : MonoBehaviour
{
    [SerializeField] private InputAction _swapPalette;
    public void Start()
    {
        _swapPalette.Enable();
        _swapPalette.performed += ctx => PaletteManager.Instance.RandomizePalette();
    }
}
