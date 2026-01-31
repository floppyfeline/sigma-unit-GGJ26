using UnityEngine;

public class SpecialColourManager : Colourable
{
    private void Start()
    {
       
        base.Start();
        Colour = TileColour.Special;
        SetColour(Colour, PaletteManager.Instance.CurrentLevelPalette.palette);
    }
}
