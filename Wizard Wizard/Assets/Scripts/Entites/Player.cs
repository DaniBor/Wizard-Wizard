using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite[] playerSprites;

    private enum PlayerState
    {
        IDLE,
        RUNNING,
        TYPING
    }
    private PlayerState playerState;


    


    private void Awake()
    {
        speed = 5.0f;
        playerState = PlayerState.RUNNING;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 newPos = transform.position;

        if(playerState == PlayerState.RUNNING)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                newPos.y += speed * Time.deltaTime;
                sr.sprite = playerSprites[1];
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                newPos.y -= speed * Time.deltaTime;
                sr.sprite = playerSprites[0];
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                newPos.x += speed * Time.deltaTime;
                sr.sprite = playerSprites[2];
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                newPos.x -= speed * Time.deltaTime;
                sr.sprite = playerSprites[2];
                transform.localScale = new Vector3(-1, 1, 1);
            }

            

            if (Input.GetButtonDown("Chat"))
            {
                Overseer.Instance.startInput();
                playerState = PlayerState.TYPING;
            }
        }
        else if(playerState == PlayerState.TYPING)
        {
            if (Input.GetButtonDown("Submit"))
            {
                Overseer.Instance.handleInput();
                playerState = PlayerState.RUNNING;
            }
        }
        newPos.z = 0;
        transform.position = newPos;
    }

}
