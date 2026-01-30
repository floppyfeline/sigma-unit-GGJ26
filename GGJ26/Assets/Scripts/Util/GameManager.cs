using UnityEngine;
using System.IO;
using Yarn.Unity;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    #region Dialogue

    public Action OnDialogueEnter;
    public Action OnDialogueExit;

    public Action<int> OnMoneyUpdated;
    public Action<int> OnYarnUpdated;

    #endregion

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
    public int Money
    {
        get { return money; }
        set
        {
            money = value;

            OnMoneyUpdated?.Invoke(money);
        }
    }

    public int YarnBalls
    {
        get { return yarnBalls; }
        set
        {
            yarnBalls = value;

            OnYarnUpdated?.Invoke(yarnBalls);
        }
    }

    #region Dialogue
    public void StartDialogue(string nodeName)
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.StartDialogue(nodeName);
        }
        else
        {
            Debug.LogWarning("DialogueRunner is not assigned in GameManager.");
        }
    }
    public void RequestNextLine(InputAction.CallbackContext ctx)
    {
        Debug.Log("Requesting next line from input");
        dialogueRunner.RequestNextLine();
    }

    #endregion

    [System.Serializable]
    private class SaveGameData
    {
        public int money;
        public int yarnBalls;
    }

#region SaveGame
    public string SaveGame()
    {
        // Fill up the SaveGameData object here
        SaveGameData data = new SaveGameData();
        data.money = money;
        data.yarnBalls = yarnBalls;

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

            money = data.money;
            yarnBalls = data.yarnBalls;

            Debug.Log("Game loaded from " + path);

            return true;
        }

        Debug.LogWarning("No save file found at " + path);
        return false;
    }
#endregion
}

