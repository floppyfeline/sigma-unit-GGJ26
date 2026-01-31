using UnityEngine;

public class PlayerColourManager : Colourable
{
    private bool _isHidden = false;
    [SerializeField]private TileColour _standingOnColour = TileColour.None;
    public override void SetColour(TileColour colour, LevelPaletteStruct palette)
    {
        base.SetColour(colour, palette);
        CheckForHidden();
    }
    public void CheckForHidden()
    {
        if(Colour == _standingOnColour)
        {
            Debug.Log("Player is hidden");
            _isHidden = true;
        }
        else
        {
            Debug.Log("Player is visible");
            _isHidden = false;
        }
    }
    public override void OnFloorChange(TileColour floorColour)
    {
        _standingOnColour = floorColour;
        Debug.Log($"Player stepped on color: {floorColour}");
        CheckForHidden();
    }
}
