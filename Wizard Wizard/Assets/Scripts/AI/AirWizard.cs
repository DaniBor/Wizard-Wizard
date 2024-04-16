using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWizard : Wizard, IWizardAI
{
    


    [SerializeField] private GameObject airProjectilePrefab;


    protected sealed override void Awake()
    {
        base.Awake();
    }

    protected sealed override void Update()
    {
        base.Update();
    }

    protected sealed override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public sealed override void Behave()
    {
        //Nothing ?
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
            if (Vector2.Distance(transform.position, target.transform.position) > 2.5f)
            {
                Vector3 delta = transform.position - target.transform.position;
                delta.Normalize();

                rb.MovePosition(transform.position - speed * Time.deltaTime * delta);

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

    public sealed override void BehaveAttacking()
    {

        if (target != null && Vector3.Distance(transform.position, target.transform.position) < 2.5)
        {
            if (timeTilAttack <= 0)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 3.0)
                {
                    if (target == null)
                    {
                        curState = WizardState.IDLE;
                        return;
                    }

                    AirProjectile projectile =
                        Instantiate(airProjectilePrefab, transform.position, Quaternion.identity)
                        .GetComponent<AirProjectile>();
                    Vector3 dir = transform.position - target.transform.position;
                    dir.Normalize();

                    projectile.isBuffed = CheckForStatusEffect(StatusEffect.EffectType.ATTACKBUFF);
                    projectile.isAllyProjectile = isAlly;
                    projectile.dir = dir;


                    timeTilAttack = attackTimer;
                    curState = WizardState.IDLE;
                }
                else
                {
                    curState = WizardState.RUNNING;
                }

            }
        }
        else curState = WizardState.IDLE;


    }

    public sealed override void BehaveFleeing()
    {
        throw new System.NotImplementedException();
    }
}
