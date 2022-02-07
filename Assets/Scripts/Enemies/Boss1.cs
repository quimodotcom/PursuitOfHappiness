using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public GameObject player;
    string phase;
    bool pickOnce; //When idle, it picks a random position to go to once
    Vector3 pickedPosition;
    Vector3 move;
    int pick; //Pick a number and the number represents an action

    float originalScale; //Bosses original Size
    float bigBossScale; //Bosses Attack Scale

    void pickAgain() //Picks an Action
    {
        pick = Random.Range(1, 2);

        if (pick == 1)
        {
            pickOnce = false;
        }
        if (pick == 2);
        {
            phase = "Attack1";
        }

    }
    private void Start()
    {
        originalScale = 2f;
        bigBossScale = 4f;
        pick = 0;
        pickOnce = false;
        phase = "Idle";
        pickedPosition = new Vector3();
    }

    private void Update()
    {
        if(phase == "Idle") //Goes to a random location around the player
        {
            if (!pickOnce)
            {
                pickOnce = true;
                pickedPosition = new Vector3(Random.Range(player.transform.position.x - 6, player.transform.position.x + 6), transform.position.y, transform.position.z);
                Invoke("pickAgain", 4f);

            }
            move = Vector3.MoveTowards(transform.position, pickedPosition, 6f * Time.deltaTime);
            transform.position = move;

        }

        if(phase == "Attack1") 
        {
            if(transform.localScale.x < bigBossScale - 0.1f)
            {
                transform.localScale += new Vector3(1f * Time.deltaTime, 1f * Time.deltaTime, 0f);
                transform.position += new Vector3(0f, 0.5f * Time.deltaTime, 0f);
            }
            
        }

    }
}
