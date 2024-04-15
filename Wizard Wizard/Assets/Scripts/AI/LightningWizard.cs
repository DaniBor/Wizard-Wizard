using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject lightningProjectilePrefab;



    protected sealed override void Awake()
    {
        base.Awake();
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


    private void Update()
    {
        Behave();
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
            LightningProjectile projectile =
                Instantiate(lightningProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<LightningProjectile>();

            projectile.isAllyProjectile = isAlly;

            timeTilAttack = attackTimer;
        }
    }

    public void BehaveFleeing()
    {
        throw new NotImplementedException();
    }
}
