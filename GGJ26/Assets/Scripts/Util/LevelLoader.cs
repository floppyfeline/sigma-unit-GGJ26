using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public UnityEvent loadStarted = new();
	private bool _isLoading = false;
    public void LoadNextLevel()
	{
        if (_isLoading) return;
        loadStarted.Invoke();
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}
	public void LoadMainMenu()
	{
        if (_isLoading) return;
        StartCoroutine(LoadMainMenu(0));
	}

    public void ReloadCurrentScene()
    {
        if (_isLoading) return;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
	}
	public void LoadSceneByName(string name)
	{
		if(_isLoading) return;
        StartCoroutine(LoadLevel(name));
	}
	IEnumerator LoadMainMenu(int index)
	{
        loadStarted.Invoke();
		AsyncOperation nextScene = SceneManager.LoadSceneAsync(index);
		while (!nextScene.isDone)
		{
			yield return null;
		}
	}
	IEnumerator LoadLevel(int levelIndex)
	{
		loadStarted.Invoke();
		AsyncOperation nextScene = SceneManager.LoadSceneAsync(levelIndex);
		nextScene.allowSceneActivation = false;
		AsyncOperation playerScene = SceneManager.LoadSceneAsync("PLAYER", LoadSceneMode.Additive);
		playerScene.allowSceneActivation = false;
		while (!nextScene.isDone)
		{
			if (nextScene.progress >= 0.9f && playerScene.progress >= 0.9f)
			{
				playerScene.allowSceneActivation = true;
				nextScene.allowSceneActivation = true;
			}
			yield return null;
		}
	}
    IEnumerator LoadLevel(string name)
    {
        _isLoading = true;
        loadStarted.Invoke();

        var nextScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        var playerScene = SceneManager.LoadSceneAsync("GameManager", LoadSceneMode.Additive);

        nextScene.allowSceneActivation = false;
        playerScene.allowSceneActivation = false;

        while (nextScene.progress < 0.9f || playerScene.progress < 0.9f)
        {
            Debug.Log($"Loading... {nextScene.progress} / {playerScene.progress}");
            yield return null;
        }

        nextScene.allowSceneActivation = true;
        playerScene.allowSceneActivation = true;

        while (!nextScene.isDone || !playerScene.isDone)
            yield return null;
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}


