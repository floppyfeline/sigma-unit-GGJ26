using UnityEngine;
using UnityEngine.Rendering;

public class ColourTile : InspectorAttributes
{
    [SerializeField] public TileColour Colour;
    MeshRenderer _renderer;
    public enum TileColour
    {
        First, Second, Third, Fourth, None
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
    public void SetTileColour(TileColour colour, LevelPaletteStruct palette)
    {
        Colour = colour;
        var block = new MaterialPropertyBlock();
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            if (_renderer == null)
            {
                Debug.LogWarning("Renderer is null on GameObject: " + gameObject.name);
            }
        }
        _renderer.GetPropertyBlock(block);
        block.SetColor("_TileColour", GetColourFromPalette(palette));
        _renderer.SetPropertyBlock(block, 0);
    }
    public void SetTileColour(LevelPaletteStruct palette)
    {
        var block = new MaterialPropertyBlock();
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            if (_renderer != null)
            {
                Debug.LogWarning("Renderer is null on GameObject: " + gameObject.name);
            }
        }
        _renderer.GetPropertyBlock(block);
        block.SetColor("_TileColour", GetColourFromPalette(palette));
        _renderer.SetPropertyBlock(block, 0);
    }
    private void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }
}
