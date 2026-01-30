using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private LevelLoader SceneLoader;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private int money;
    private int yarnBalls;

    void Start()
    {
        SceneLoader = gameObject.AddComponent<LevelLoader>();
    }
    private void OnDestroy()
    {
        Timers.Clear();
    }
    private void Update()
    {
        Timers.RunTimers();
    }
    #region Backend
    public void ReturnToMainMenu()
    {
        SceneLoader.LoadMainMenu();
    }
    #endregion
    [System.Serializable]
    private class SaveGameData
    {
    }

#region SaveGame
    public string SaveGame()
    {
        // Fill up the SaveGameData object here
        SaveGameData data = new SaveGameData();

        // Convert to json
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/savefile.json";

        File.WriteAllText(path, json);

        Debug.Log("Game saved to " + path);

        return path;
    }
    public bool LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveGameData data = JsonUtility.FromJson<SaveGameData>(json);


            Debug.Log("Game loaded from " + path);

            return true;
        }

        Debug.LogWarning("No save file found at " + path);
        return false;
    }
#endregion
}

