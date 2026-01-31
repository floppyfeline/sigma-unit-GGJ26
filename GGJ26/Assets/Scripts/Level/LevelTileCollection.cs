using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
public class LevelTileCollection : InspectorAttributes
{
    private const string TILE_PREFAB_PATH = "Prefabs/PF_ColourTile";
    public bool IsGenerated = false;
    [SerializeField] private List<ColourTile> _levelTiles = new List<ColourTile>();
    [SerializeField] private LevelTileCollection _generatedTiles;

    [MethodButton("Collect Tiles In Children")]
    public void CollectTiles()
    {
        _levelTiles.Clear();
        ColourTile[] tilesInScene = GetComponentsInChildren<ColourTile>();
        _levelTiles.AddRange(tilesInScene);
    }
    private LevelPaletteStruct GetTestPalette()
    {
        return new LevelPaletteStruct
        {
            Color1 = Color.red,
            Color2 = Color.green,
            Color3 = Color.blue,
            Color4 = Color.yellow,
            None = Color.black
        };
    }
    [MethodButton("Set Test Palette")]
    public void DebugSetTestPalette()
    {
        Timers.UntilThen(1f, () =>
        { // do this while
        }, () =>{ // do this after
        });
        SetTileColours(GetTestPalette());
    }

    public void SetTileColours(LevelPaletteStruct palette)
    {
        foreach (ColourTile tile in _levelTiles)
        {
            tile.CollectColourables();
            tile.SetColour(tile.Colour, palette);
        }
    }
    [MethodButton("Generate Collection")]
    public void GenerateCollection()
    {

        if (IsGenerated)
        {
            Debug.LogWarning("Cannot generate collection from an already generated collection.");
            return;
        }
        if(_generatedTiles != null)
        {
            DestroyImmediate(_generatedTiles.gameObject);
        }
        GameObject newCollection = new GameObject("Generated Tiles");
        _generatedTiles = newCollection.AddComponent<LevelTileCollection>();
        _generatedTiles.IsGenerated = true;
        foreach (ColourTile tile in _levelTiles)
        {
            tile.CollectColourables();
            //add individual cubes for each tile

            BoxCollider[] tileColliders = tile.GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider col in tileColliders)
            {
                Debug.Log(col);
                Bounds bounds = col.bounds;

                int width = Mathf.RoundToInt(bounds.size.x);
                int length = Mathf.RoundToInt(bounds.size.z);
                int height = Mathf.RoundToInt(bounds.size.y);
                Debug.Log($"Generating tiles for {tile.name} with width {width}, length {length}, height {height}");
                for (int z = 0; z < length; z++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            Vector3 spawnPosition = new Vector3(
                                bounds.min.x + x + 1,
                                bounds.min.y + y + 1,
                                bounds.min.z + z + 1
                            );
                            GameObject tileCube = Instantiate(
                                Resources.Load<GameObject>(TILE_PREFAB_PATH),
                                spawnPosition,
                                Quaternion.identity,
                                newCollection.transform
                            );
                            tileCube.name = $"{tile.name}_Cube_{x}_{y}_{z}";
                            tileCube.transform.SetParent(newCollection.transform);

                            ColourTile cubeColourTile = tileCube.GetComponent<ColourTile>();
                            cubeColourTile.SetColour(tile.Colour, GetTestPalette());
                            _generatedTiles._levelTiles.Add(cubeColourTile);
                        }
                    }
                }
            }
        }
        gameObject.SetActive(false);
    }
}
