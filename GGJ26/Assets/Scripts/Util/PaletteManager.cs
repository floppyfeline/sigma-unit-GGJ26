using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class PaletteManager : InspectorAttributes
{
    public static PaletteManager Instance;
    [SerializeField] private LevelPalette _currentLevelPalette;
    [SerializeField] private List<LevelPalette> _allPalettes;
    public LevelPalette CurrentLevelPalette => _currentLevelPalette;
    public Action<LevelPaletteStruct> OnPaletteChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        int index = UnityEngine.Random.Range(0, _allPalettes.Count);
        _currentLevelPalette = _allPalettes[index];
    }
    private void Start()
    {
    }
    public void SetRandomPalette()
    {
        _currentLevelPalette.palette = _currentLevelPalette.GetSwappedPalette();
        OnPaletteChanged?.Invoke(_currentLevelPalette.palette);
        return;
        if (_allPalettes.Count == 0)
            return;
        int randomIndex = UnityEngine.Random.Range(0, _allPalettes.Count);
        _currentLevelPalette = _allPalettes[randomIndex];
        OnPaletteChanged?.Invoke(_currentLevelPalette.palette);
    }
}
