using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWizard : Wizard
{
    [SerializeField] private GameObject fireProjectilePrefab;
    

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

    public override void BehaveIdle()
    {
        if (target == null)
        {
            getClosestWizard();
        }
        else curState = WizardState.RUNNING;
    }

    public override void BehaveRunning()
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

    public override void BehaveAttacking()
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
                FireballProjectile projectile =
                    Instantiate(fireProjectilePrefab, transform.position, Quaternion.identity)
                    .GetComponent<FireballProjectile>();

                projectile.isBuffed = CheckForStatusEffect(StatusEffect.EffectType.ATTACKBUFF);
                projectile.dir = dir;
                projectile.isAllyProjectile = isAlly;
                projectile.speed = 5.0f;

                timeTilAttack = attackTimer;
            }
        }
        else curState = WizardState.IDLE;


    }

    public override void BehaveFleeing()
    {
        throw new NotImplementedException();
    }
}
