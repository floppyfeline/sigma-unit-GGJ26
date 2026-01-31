using System;
using System.Collections;
using UnityEngine;

public class PaletteManager : InspectorAttributes
{
    public static PaletteManager Instance;
    [SerializeField] private LevelPalette _currentLevelPalette;
    public LevelPalette CurrentLevelPalette => _currentLevelPalette;
    public Action<LevelPaletteStruct> OnPaletteChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
    }
}
