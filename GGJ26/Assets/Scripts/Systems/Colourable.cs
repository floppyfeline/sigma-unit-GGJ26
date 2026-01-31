using UnityEngine;
using System.Collections.Generic;
using System;

public class Colourable : InspectorAttributes
{
    [SerializeField] public TileColour Colour;
    [SerializeField] protected List<MeshRenderer> _colourables;
    public Action<TileColour> OnColourChanged;
    protected virtual void Start()
    {
        PaletteManager.Instance.OnPaletteChanged += (palette) => SetColour(Colour, palette);
        InputSystem inputs = GetComponent<InputSystem>();
        if (inputs != null)
        {
            inputs.Color1.performed += ctx => SetColour(TileColour.First, PaletteManager.Instance.CurrentLevelPalette.palette);
            inputs.Color2.performed += ctx => SetColour(TileColour.Second, PaletteManager.Instance.CurrentLevelPalette.palette);
            inputs.Color3.performed += ctx => SetColour(TileColour.Third, PaletteManager.Instance.CurrentLevelPalette.palette);
            inputs.Color4.performed += ctx => SetColour(TileColour.Fourth, PaletteManager.Instance.CurrentLevelPalette.palette);
        }
        SetColour(Colour, PaletteManager.Instance.CurrentLevelPalette.palette);
    }
    private Color GetColourFromPalette(LevelPaletteStruct palette)
    {
        switch (Colour)
        {
            case TileColour.First:
                return palette.Color1;
            case TileColour.Second:
                return palette.Color2;
            case TileColour.Third:
                return palette.Color3;
            case TileColour.Fourth:
                return palette.Color4;
            default:
                return Color.clear;
        }
    }
    public virtual void SetColour(TileColour colour, LevelPaletteStruct palette)
    {
        Colour = colour;
        var block = new MaterialPropertyBlock();
        foreach (MeshRenderer renderer in _colourables)
        {
            renderer.GetPropertyBlock(block);
            block.SetColor("_TileColour", GetColourFromPalette(palette));
            renderer.SetPropertyBlock(block, 0);
        }
        OnColourChanged?.Invoke(Colour);
    }
    public virtual void OnFloorChange(TileColour floorColour)
    {
    }
    public enum TileColour
    {
        First, Second, Third, Fourth, None
    }
}
