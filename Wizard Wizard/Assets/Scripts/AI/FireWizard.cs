using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject fireProjectilePrefab;
    

    private void Awake()
    {
        attackTimer = 2;
        timeTilAttack = attackTimer;
        attackRate = 1;
        speed = 1.5f;

        rb = GetComponent<Rigidbody2D>();
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
        if (curState == WizardState.RUNNING)
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

    public void BehaveRunning()
    {
        try
        {
            if (Vector2.Distance(transform.position, target.transform.position) > 2.5f)
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
            Debug.LogError("Exception when running towards enemy!");
            target = null;
            curState = WizardState.IDLE;
            return;
        }
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
            FireballProjectile projectile =
                Instantiate(fireProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<FireballProjectile>();

            projectile.dir = dir;
            projectile.isAllyProjectile = isAlly;
            projectile.speed = 5.0f;

            timeTilAttack = attackTimer;
        }
    }

    public void BehaveFleeing()
    {
        throw new NotImplementedException();
    }
}
