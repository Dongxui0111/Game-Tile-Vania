using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] float mainMenuLoadDelay = 1f;
    [SerializeField] float timeScaleToSlow = .2f;

    // Load Main Menu
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        Time.timeScale = timeScaleToSlow;
        yield return new WaitForSecondsRealtime(mainMenuLoadDelay);
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}
