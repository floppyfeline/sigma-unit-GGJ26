using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public UnityEvent loadStarted = new();
	private bool _isLoading = false;
    private void Start()
    {
		LoadSceneByName("lvl1");
    }
    public void LoadNextLevel()
	{
		if (_isLoading) return;
		loadStarted.Invoke();
		StartCoroutine(LoadLevel(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name));
	}
	public void ReloadCurrentScenes()
	{
		if (_isLoading) return;
		Debug.Log("Reloading current scene: " + SceneManager.GetActiveScene().name);
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
	}
	public void LoadSceneByName(string name)
	{
		if (_isLoading) return;
		StartCoroutine(LoadLevel(name));
	}
	IEnumerator LoadLevel(string name)
	{
		loadStarted.Invoke();
		_isLoading = true;
		AsyncOperation gameManagerScene = SceneManager.LoadSceneAsync("GameManager", LoadSceneMode.Additive);
		AsyncOperation uiScene = SceneManager.LoadSceneAsync("GameplayUI", LoadSceneMode.Additive);
		AsyncOperation playerScene = SceneManager.LoadSceneAsync("Player", LoadSceneMode.Additive);
        AsyncOperation nextScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
		nextScene.allowSceneActivation = false;
		playerScene.allowSceneActivation = false;
		uiScene.allowSceneActivation = false;
		gameManagerScene.allowSceneActivation = false;
        while (nextScene.progress < 0.9f
            | playerScene.progress < 0.9f
            || uiScene.progress < 0.9f
            || gameManagerScene.progress < 0.9f)
        {
            Debug.Log($"Loading... {nextScene.progress} / {playerScene.progress} / {uiScene.progress} / {gameManagerScene.progress}");
            yield return null;
		}
			playerScene.allowSceneActivation = true;
			nextScene.allowSceneActivation = true;
			uiScene.allowSceneActivation = true;
			gameManagerScene.allowSceneActivation = true;
		_isLoading = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}


