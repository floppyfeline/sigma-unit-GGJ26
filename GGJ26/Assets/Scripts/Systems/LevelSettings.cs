using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [SerializeField] private int _levelTimeLimit = 60;
    private void Start()
    {
        GameManager.Instance.StartLevelTimer(_levelTimeLimit);
    }
}
