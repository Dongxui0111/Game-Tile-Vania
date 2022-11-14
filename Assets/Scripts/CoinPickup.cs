using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        FindObjectOfType<GameSession>().AddCoins(coinValue);
        coinValue = 0;  // prevent from double Triggering

        PlayCoinSFX();
        Destroy(gameObject);
    }

    private void PlayCoinSFX()
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, FindObjectOfType<Player>().transform.position);
    }
}
