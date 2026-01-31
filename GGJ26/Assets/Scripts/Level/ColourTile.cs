using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
public class ColourTile : Colourable
{
   protected override void Start()
    {
        base.Start();
        CollectColourables();
    }
    public void CollectColourables()
    {
        MeshRenderer[] rend = GetComponentsInChildren<MeshRenderer>();
        _colourables = new List<MeshRenderer>(rend);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name, other);
        if (other.CompareTag("Player"))
        { 
            PlayerColourManager playerColourManager = other.GetComponentInParent<PlayerColourManager>();
            Debug.Log("Player entered tile of colour: " + Colour.ToString());
            if (playerColourManager != null)
            {
                playerColourManager.OnFloorChange(Colour);
            }
        }
    }
}
