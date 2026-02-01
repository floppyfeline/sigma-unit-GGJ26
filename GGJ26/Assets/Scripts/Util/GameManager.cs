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
        SceneLoader = FindAnyObjectByType<LevelLoader>();

    }
    private void OnDestroy()
    {
        Timers.Clear();
    }
    private void Update()
    {
        Timers.RunTimers();
    }

    private Timer _timerTimer;
    public Action<int> OnTimerUpdate;
    private int _secondsLeft;
        public int SecondsLeft
    {
        get
            { return _secondsLeft; }
        set
        {
            _secondsLeft = value; 
            OnTimerUpdate?.Invoke(_secondsLeft);
        }
    }
    public void PlayerCaught()
    {
        PauseGameActivity();
        Timers.After(2f, () => 
            {
                player = FindAnyObjectByType<PlayerController>();
                player.GetCaught(); 
                Timers.After(3.5f, () => {ReloadCurrentLevel(); ResumeGameActivity(); });
            }
        );
    }
    int _collectiblesPickedUp = 0;
    public Action OnPickup;
    public void OnCollectiblePickedUp()
    {
        _collectiblesPickedUp++;
        if (_collectiblesPickedUp >= 3)
        {
            Debug.Log("All collectibles picked up! You win!");
            PauseGameActivity();
            Timers.After(3f, () => 
            {
                Debug.Log("Loading next level...");
                SceneLoader.LoadNextLevel();
                ResumeGameActivity();
            });
        }
    }

    public void StartLevelTimer(int timeLimit)
    {
        SecondsLeft = timeLimit;
        _timerTimer = Timers.After(1f, () => 
        {
            SecondsLeft--;
            if (_secondsLeft > 0)
            {
                StartLevelTimer(SecondsLeft);
            }
            else
            {
                Debug.Log("Time's up!");
                PlayerCaught();
            }
        });
    }

    private void PauseGameActivity()
    {
        gameActive = false;
        _timerTimer?.Pause(true);
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

    public void ReloadCurrentLevel()
    {
        Debug.Log("Ya lost");
        SceneLoader.ReloadCurrentScenes();
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

