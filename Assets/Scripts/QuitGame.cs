using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] float quitGameDelay = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ExitGame());
    }

    private IEnumerator ExitGame()
    {
        yield return new WaitForSecondsRealtime(quitGameDelay);
        Application.Quit();
    }
}
