using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject magicProjectilePrefab;

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
                MagicProjectile projectile =
                    Instantiate(magicProjectilePrefab, transform.position, Quaternion.identity)
                    .GetComponent<MagicProjectile>();

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
