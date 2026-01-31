using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Colourable : InspectorAttributes
{
    [SerializeField] public TileColour Colour;
    [SerializeField] protected List<Renderer> _colourables;
    public Action<TileColour> OnColourChanged;
    protected virtual void Start()
    {
        PaletteManager.Instance.OnPaletteChanged += (palette) => SetColour(Colour, palette);
    }
    protected Color GetColourFromPalette(LevelPaletteStruct palette)
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
                case TileColour.None:
                    return palette.None;
            default:
                return Color.black;
        }
    }
    public virtual void SetColour(TileColour colour, LevelPaletteStruct palette)
    {
        Colour = colour;
        Color color = GetColourFromPalette(palette);
        var block = new MaterialPropertyBlock();
        foreach (Renderer renderer in _colourables)
        {
            int materials = renderer.sharedMaterials.Length;
            for (int i = 0; i < materials; i++)
            {
                renderer.GetPropertyBlock(block);
                block.SetColor("_TileColour", color);
                block.SetColor("_ShadowColour", palette.ShadowColor);
                renderer.SetPropertyBlock(block, i);
            }
        }
        OnColourChanged?.Invoke(Colour);
    }
    public virtual void SetColour(Color colour)
    {
        var block = new MaterialPropertyBlock();
        foreach (Renderer renderer in _colourables)
        {
            int materials = renderer.sharedMaterials.Length;
            for (int i = 0; i < materials; i++)
            {
                renderer.GetPropertyBlock(block);
                block.SetColor("_TileColour", colour);
                block.SetColor("_ShadowColour", PaletteManager.Instance.CurrentLevelPalette.palette.ShadowColor);
                renderer.SetPropertyBlock(block, i);
            }
        }
    }
    public virtual void OnFloorChange(TileColour floorColour)
    {
    }
    public enum TileColour
    {
        First, Second, Third, Fourth, None
    }
}
