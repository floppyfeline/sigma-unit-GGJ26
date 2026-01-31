using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
public class PlayerColourManager : Colourable
{
    private bool _isColoured = false;
    private bool _isHidden = false;
    [SerializeField] private float _colourTime = 1.0f;
    [SerializeField]private TileColour _standingOnColour = TileColour.None;
    [SerializeField] private Color _baseColour;

    public UnityEvent OnHide;
    public UnityEvent OnShow;
    Color _currentColour;
    Timer _transitionTimer;
    float _transitionTime = 0f;
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
        _currentColour = _baseColour;
    }
    public override void SetColour(TileColour colour, LevelPaletteStruct palette)
    {
        Colour = colour;
        Color target = GetColourFromPalette(palette);
        StartColourTransition(target);
        _isColoured = true;
        CheckForHidden();
    }
    public void StartColourTransition(Color targetColour)
    {
        if (_transitionTimer != null)
        {
            Timers.Remove(_transitionTimer);
        }
        _transitionTimer = Timers.UntilThen(_colourTime, () =>
        {
            _transitionTime += Time.deltaTime;
            base.SetColour(Color.Lerp(_currentColour, targetColour, _transitionTime / _colourTime));
        }, () =>
        {
            _currentColour = targetColour;
            _transitionTime = 0f;
            _transitionTimer = null;
        });
    }
    public void ResetColour()
    {
        StartColourTransition(_baseColour);
        _isColoured = false;
        OnShow?.Invoke();
        CheckForHidden();
    }
    public void CheckForHidden()
    {
        if(!_isColoured)
        {
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
