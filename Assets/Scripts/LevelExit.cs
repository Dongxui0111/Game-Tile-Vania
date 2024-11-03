using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float levelExitTimeFactor = 0.25f;

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        StartCoroutine(LoadNextLevel());
    }


    IEnumerator LoadNextLevel()
    {
        Time.timeScale = levelExitTimeFactor;
        Time.fixedDeltaTime *= Time.timeScale;

        yield return new WaitForSeconds(levelLoadDelay * levelExitTimeFactor);

        Time.fixedDeltaTime /= Time.timeScale;
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
       
    }
}


