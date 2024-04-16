using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Wizard
{
    private SpriteRenderer sr;

    [SerializeField] private float maxMana;
    [SerializeField] private float mana;
    [SerializeField] private float manaRegen;

    [SerializeField] private Sprite[] playerSprites;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;


    private enum PlayerState
    {
        IDLE,
        RUNNING,
        TYPING
    }
    private PlayerState playerState;


    public void refillHealth()
    {
        health = maxHealth;
    }

    protected sealed override void Awake()
    {
        base.Awake();

        speed = 5.0f;
        playerState = PlayerState.RUNNING;
        sr = GetComponent<SpriteRenderer>();
    }

    protected sealed override void Update()
    {
        Vector3 newPos = transform.position;
        healthText.text = "Player Health: " + health;
        manaText.text = "Player Mana: " + (int) mana;

        

        if (playerState == PlayerState.RUNNING)
        {
            if (!isStunned())
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
            }

            if (Input.GetButtonDown("Chat"))
            {
                Overseer.Instance.startInput();
                playerState = PlayerState.TYPING;
            }
        }


        if (playerState == PlayerState.TYPING)
        {
            if (Input.GetButtonDown("Submit"))
            {
                Overseer.Instance.handleInput();
                playerState = PlayerState.RUNNING;
            }
        }


        UpdateEffects();
        mana = Mathf.Min(mana + manaRegen * Time.deltaTime, maxMana);
        Debug.Log(mana);
        newPos.z = 0;
        transform.position = newPos;
    }


    public bool hasMana(int cost)
    {
        return mana >= cost;
    }

    public void spendMana(int amount)
    {
        mana -= amount;
    }
}
