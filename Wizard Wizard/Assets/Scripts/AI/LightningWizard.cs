using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject lightningProjectilePrefab;



    private void Awake()
    {
        attackTimer = 2;
        timeTilAttack = attackTimer;
        attackRate = 1;
        curState = WizardState.RUNNING;

        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        getClosestWizard();
        speed = 1.5f;
    }


    public void Behave()
    {
        if (target == null)
            getClosestWizard();

        Debug.Log(target);

        switch (curState)
        {
            case WizardState.RUNNING:
                break;
            case WizardState.ATTACKING:
                UpdateAttack();
                break;
        }
    }


    private void Update()
    {
        UpdateEffects();
        Behave();

        

        timeTilAttack -= attackRate * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }


    void UpdateMovement()
    {
        try
        {
            if (Vector2.Distance(transform.position, target.transform.position) > 2.0f)
            {
                Vector3 delta = transform.position - target.transform.position;
                delta.Normalize();

                rb.MovePosition(transform.position - delta * Time.deltaTime * speed);

            }
            else
                curState = WizardState.ATTACKING;
        }
        catch (Exception)
        {
            target = null;
            return;
        }
    }


    void UpdateAttack()
    {
        if (timeTilAttack <= 0)
        {
            Vector2 dir;
            try
            {
                dir = transform.position - target.transform.position;
            }
            catch (Exception)
            {
                target = null;
                curState = WizardState.RUNNING;
                return;
            }

            dir.Normalize();
            LightningProjectile projectile =
                Instantiate(lightningProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<LightningProjectile>();

            projectile.isAllyProjectile = isAlly;

            timeTilAttack = attackTimer;
        }
    }
}
