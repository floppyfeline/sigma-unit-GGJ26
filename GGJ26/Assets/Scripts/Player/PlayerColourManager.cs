using UnityEngine;
using UnityEngine.Events;
public class PlayerColourManager : Colourable
{
    private bool _isColoured = false;
    private bool _isHidden = false;
    [SerializeField] private float _colourTime = 1.0f;
    [SerializeField]private TileColour _standingOnColour = TileColour.None;
    [SerializeField] private Color _baseColour;

    public UnityEvent OnHide;
    public UnityEvent OnShow;
    protected override void Start()
    {
        base.Start();
        _baseColour = PaletteManager.Instance.CurrentLevelPalette.palette.ChamRestColor;
        InputSystem inputs = GetComponent<InputSystem>();
        if (inputs != null)
        {
            inputs.Color1.performed += ctx => SetColour(TileColour.First, PaletteManager.Instance.CurrentLevelPalette.palette);
            inputs.Color2.performed += ctx => SetColour(TileColour.Second, PaletteManager.Instance.CurrentLevelPalette.palette);
            inputs.Color3.performed += ctx => SetColour(TileColour.Third, PaletteManager.Instance.CurrentLevelPalette.palette);
            inputs.Color4.performed += ctx => SetColour(TileColour.Fourth, PaletteManager.Instance.CurrentLevelPalette.palette);
        }
        inputs.Jump.performed += ctx => ResetColour();
        inputs.LaunchTongue.performed += ctx => ResetColour();

        SetColour(_baseColour);
    }
    public override void SetColour(TileColour colour, LevelPaletteStruct palette)
    {
        base.SetColour(colour, palette);
        _isColoured = true;
        CheckForHidden();
    }
    public void ResetColour()
    {
        base.SetColour(_baseColour);
        _isColoured = false;
        OnShow?.Invoke();
        CheckForHidden();
    }
    public void CheckForHidden()
    {
        if(!_isColoured)
        {
            Debug.Log("Player is not coloured");
            _isHidden = false;
            OnShow?.Invoke();
            return;
        }
        if (Colour == _standingOnColour)
        {
            Debug.Log("Player is hidden");
            _isHidden = true;
            OnHide?.Invoke();
            return;
        }
        else
        {
            Debug.Log("Player is visible");
            _isHidden = false;
            OnShow?.Invoke();
        }
    }
    public override void OnFloorChange(TileColour floorColour)
    {
        _standingOnColour = floorColour;
        Debug.Log($"Player stepped on color: {floorColour}");
        CheckForHidden();
    }
}
