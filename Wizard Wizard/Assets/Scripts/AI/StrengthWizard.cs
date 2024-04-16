using System;
using UnityEngine;

public class StrengthWizard : Wizard, IWizardAI
{
    protected sealed override void Awake()
    {
        base.Awake();
        isStatusWizard = true;
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
            getClosestUnbuffedAlly(StatusEffect.EffectType.DEFENSEBUFF);
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
        if (timeTilAttack <= 0)
        {
            EffectAttackBuff buff = new EffectAttackBuff();
            StatusEffect status = new StatusEffect(10, 1, target, buff, StatusEffect.EffectType.ATTACKBUFF);

            target.ApplyStatusEffect(status);
            target = null;
            curState = WizardState.IDLE;
            timeTilAttack = attackTimer;
        }
    }

    public override void BehaveFleeing()
    {
        throw new NotImplementedException();
    }
}
