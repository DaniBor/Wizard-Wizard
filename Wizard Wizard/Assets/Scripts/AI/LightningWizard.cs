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


    public sealed override void Behave()
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


    protected sealed override void Update()
    {
        Behave();
    }

    protected sealed override void FixedUpdate()
    {
        if (curState == WizardState.RUNNING)
        {
            BehaveRunning();
        }
    }

    public sealed override void BehaveIdle()
    {
        if (target == null)
        {
            getClosestWizard();
        }
        else curState = WizardState.RUNNING;
    }

    public sealed override void BehaveRunning()
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
            curState = WizardState.IDLE;
            return;
        }
    }

    public sealed override void BehaveAttacking()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) < 2.5)
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

                projectile.isBuffed = CheckForStatusEffect(StatusEffect.EffectType.ATTACKBUFF);
                projectile.isAllyProjectile = isAlly;

                timeTilAttack = attackTimer;
            }
        }
        else curState = WizardState.IDLE;


    }

    public sealed override void BehaveFleeing()
    {
        throw new NotImplementedException();
    }
}
