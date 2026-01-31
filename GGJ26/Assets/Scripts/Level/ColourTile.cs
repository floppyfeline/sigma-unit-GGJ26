using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
public class ColourTile : Colourable
{
   protected override void Start()
    {
        base.Start();
        MeshRenderer[] rend = GetComponentsInChildren<MeshRenderer>();
        _colourables = new List<MeshRenderer>(rend);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        { 
            PlayerColourManager playerColourManager = other.GetComponent<PlayerColourManager>();
            if(playerColourManager != null)
            {
                playerColourManager.OnFloorChange(Colour);
            }
        }
    }
}
