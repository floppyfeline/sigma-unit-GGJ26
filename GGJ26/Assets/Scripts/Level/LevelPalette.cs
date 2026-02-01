using UnityEngine;

[CreateAssetMenu(fileName = "LevelPalette", menuName = "Scriptable Objects/LevelPalette")]
public class LevelPalette : ScriptableObject
{
    public LevelPaletteStruct palette;
    public LevelPaletteStruct GetSwappedPalette()
    {
        LevelPaletteStruct swapped = new LevelPaletteStruct();

        Color[] colors = new Color[]
        {
        palette.Color1,
        palette.Color2,
        palette.Color3,
        palette.Color4  
        };

        for (int i = colors.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (colors[i], colors[randomIndex]) = (colors[randomIndex], colors[i]);
        }

        swapped.Color1 = colors[0];
        swapped.Color2 = colors[1];
        swapped.Color3 = colors[2];
        swapped.Color4 = colors[3];
        swapped.ChamRestColor = palette.ChamRestColor;
        swapped.None = palette.None;
        swapped.ShadowColor = palette.ShadowColor;
        swapped.Special = palette.Special;

        return swapped;
    }
    public Color GetColor(int index)
    {
        return index switch
        {
            0 => palette.Color1,
            1 => palette.Color2,
            2 => palette.Color3,
            3 => palette.Color4,
            _ => Color.white,
        };
    }
}
[System.Serializable]
public struct LevelPaletteStruct
{
    public Color ChamRestColor;
    public Color Color1;
    public Color Color2;
    public Color Color3;
    public Color Color4;
    public Color None;
    public Color ShadowColor;
        public Color Special;
}