using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerCoins = 0;
    [SerializeField] int coinsPerLife = 100;

    [SerializeField] float playerDeathDelay = 3f;
    float slowMoFactor = 0.2f;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        coinsText.text = playerCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ProcessPlayerDeath()
    {
        Time.timeScale = slowMoFactor;
        Time.fixedDeltaTime *= Time.timeScale;
        GameObject.Find("Foreground").GetComponent<CompositeCollider2D>().sharedMaterial.friction = .1f; // TODO

        yield return new WaitForSecondsRealtime(playerDeathDelay);

        GameObject.Find("Foreground").GetComponent<CompositeCollider2D>().sharedMaterial.friction = 0f;
        Time.fixedDeltaTime /= Time.timeScale;
        Time.timeScale = 1f;

        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        playerCoins += coinsToAdd;
        
        if (playerCoins >= coinsPerLife)
        {
            playerLives++;
            playerCoins -= coinsPerLife;
           
            UpdateLives();
        }

        UpdateCoins();
    }

    public void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        UpdateLives();
    }

    public void UpdateLives()
    {
        livesText.text = playerLives.ToString();
    }

    public void UpdateCoins()
    {
        coinsText.text = playerCoins.ToString();
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
