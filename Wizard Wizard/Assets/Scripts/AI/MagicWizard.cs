using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject magicProjectilePrefab;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 2;
        timeTilAttack = attackTimer;
        attackRate = 1;
        speed = 2.0f;
    }

    private void Update()
    {
        Behave();
    }

    public void Behave()
    {
        switch (curState)
        {
            case WizardState.IDLE:
                BehaveIdle();
                break;
            case WizardState.ATTACKING:
                BehaveAttacking();
                break;
            case WizardState.FLEEING:
                BehaveFleeing();
                break;
            case WizardState.RUNNING:
                break;
            default:
                break;
        }
        UpdateEffects();
        timeTilAttack -= attackRate * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(curState == WizardState.RUNNING)
        {
            BehaveRunning();
        }
    }


    public void BehaveIdle()
    {
        if (target == null)
        {
            getClosestWizard();
        }
        else curState = WizardState.RUNNING;
    }

    public void BehaveAttacking()
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
            MagicProjectile projectile =
                Instantiate(magicProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<MagicProjectile>();

            projectile.dir = dir;
            projectile.isAllyProjectile = isAlly;
            projectile.speed = 5.0f;

            timeTilAttack = attackTimer;
        }
    }

    public void BehaveFleeing()
    {
        throw new System.NotImplementedException();
    }

    public void BehaveRunning()
    {
        try
        {
            Debug.Log("Trying to run.");
            if (Vector2.Distance(transform.position, target.transform.position) > 2.5f)
            {
                Debug.Log("Calculating dir");
                Vector3 delta = transform.position - target.transform.position;
                delta.Normalize();
                Debug.Log("New pos = " + delta);

                rb.MovePosition(transform.position - delta * Time.deltaTime * speed);

            }
            else
                curState = WizardState.ATTACKING;
        }
        catch (Exception)
        {
            Debug.LogError("Exception when running towards enemy!");
            target = null;
            curState = WizardState.IDLE;
            return;
        }
    }
}
