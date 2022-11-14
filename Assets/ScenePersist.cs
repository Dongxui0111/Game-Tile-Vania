using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    static ScenePersist scenePersistInstance = null;
    int startingSceneIndex;

    void Start()
    {
        if (!scenePersistInstance)
        {
            scenePersistInstance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
            DontDestroyOnLoad(gameObject);
        }
        else if (scenePersistInstance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (startingSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            scenePersistInstance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (startingSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            scenePersistInstance = null;
            Destroy(gameObject);
        }
    }
}
