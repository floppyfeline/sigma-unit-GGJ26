using System;
using System.Collections;
using UnityEngine;

public class PaletteManager : InspectorAttributes
{
    [SerializeField] private LevelPalette _currentLevelPalette;
    public LevelPalette CurrentLevelPalette => _currentLevelPalette;
    public Action<LevelPaletteStruct> OnPaletteChanged;


    private void Start()
    {
        OnPaletteChanged?.Invoke(_currentLevelPalette.palette);
    }
}
