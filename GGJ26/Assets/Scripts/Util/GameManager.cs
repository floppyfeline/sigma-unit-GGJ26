using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private LevelLoader SceneLoader;

    private bool gameActive = true;
    private PlayerController player;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SceneLoader = gameObject.AddComponent<LevelLoader>();

        player = FindAnyObjectByType<PlayerController>();
    }
    private void OnDestroy()
    {
        Timers.Clear();
    }
    private void Update()
    {
        Timers.RunTimers();
    }

    public void PlayerCaught()
    {
        Timers.After(3f, () => 
            {
                player.GetCaught(); 
                Timers.After(5f, () => ReloadCurrentLevel());
            }
        );
    }

    private void PauseGameActivity()
    {
        gameActive = false;
    }
    private void ResumeGameActivity()
    {
        gameActive = false;
    }
    public bool GetGameActive()
    {
        return gameActive;
    }

    #region UI
    public void ReturnToMainMenu()
    {
        SceneLoader.LoadMainMenu();
    }

    public void ReloadCurrentLevel()
    {
        Debug.Log("Ya lost");
        SceneLoader.ReloadCurrentScene();
    }
    #endregion
    [System.Serializable]
    private class SaveGameData
    {
        // Levels, high scores, etc.
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

