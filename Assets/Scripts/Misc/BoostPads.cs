using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPads : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Game.Instance.Player.jumpForce += 3;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Game.Instance.Player.jumpForce -= 3;
        }
    }
}
