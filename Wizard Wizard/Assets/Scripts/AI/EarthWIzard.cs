using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject earthProjectilePrefab;


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
                timeTilAttack = attackTimer;
                curState = WizardState.IDLE;
                if (target == null)
                {
                    curState = WizardState.IDLE;
                    return;
                }

                EarthProjectile projectile =
                    Instantiate(earthProjectilePrefab, transform.position, Quaternion.identity)
                    .GetComponent<EarthProjectile>();

                projectile.isBuffed = CheckForStatusEffect(StatusEffect.EffectType.ATTACKBUFF);
                projectile.isAllyProjectile = isAlly;
                projectile.targetWizard(target);


            }
        }
        else curState = WizardState.IDLE;



    }

    public sealed override void BehaveFleeing()
    {
        throw new System.NotImplementedException();
    }
}
