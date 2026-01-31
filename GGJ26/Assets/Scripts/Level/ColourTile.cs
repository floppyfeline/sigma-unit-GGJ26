using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
public class ColourTile : Colourable
{
   protected override void Start()
    {
        base.Start();
        CollectColourables();
        SetColour(Colour, PaletteManager.Instance.CurrentLevelPalette.palette);
    }
    public void CollectColourables()
    {
        Renderer[] rend = GetComponentsInChildren<Renderer>();
        _colourables = new List<Renderer>(rend);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            PlayerColourManager playerColourManager = other.GetComponentInParent<PlayerColourManager>();
            if (playerColourManager != null)
            {
                playerColourManager.OnFloorChange(Colour);
            }
        }
    }
}
