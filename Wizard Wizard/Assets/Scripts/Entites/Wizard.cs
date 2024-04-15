using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wizard : Entity, IWizardAI
{
    public bool isAlly;
    [SerializeField] protected Wizard target;

    protected float attackTimer;
    protected float timeTilAttack;
    protected float attackRate;

    protected bool stunned;

    public List<StatusEffect> effects;
    protected Rigidbody2D rb;


    protected enum WizardState
    {
        IDLE,
        RUNNING,
        ATTACKING,
        FLEEING
    }
    [SerializeField] protected WizardState curState;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attackTimer = 2;
        timeTilAttack = attackTimer;
        attackRate = 1;

        speed = 2.0f;

        effects = new List<StatusEffect>();
    }

    protected virtual void Update()
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

    protected virtual void FixedUpdate()
    {
        if (curState == WizardState.RUNNING)
        {
            BehaveRunning();
        }
    }

    public void DamageMe(Damage dmg)
    {
        Debug.Log("Killed by lightning for some reason");
        health -= dmg.damageAmount - defense;
        if (health < 0)
        {
            effects.Clear();
            Overseer.Instance.OnWizardKill(this);
            return;
        }
    }

    public bool checkAlly()
    {
        return isAlly;
    }
    protected void getClosestWizard()
    {
        List<Wizard> enemies = Overseer.Instance.getWizards(isAlly);

        foreach (Wizard w in enemies)
        {
            if (w != null)
            {
                if (target == null)
                {
                    target = w;
                    continue;
                }
                else
                {
                    float oldDist = Vector2.Distance(transform.position, target.transform.position);
                    float dist = Vector2.Distance(transform.position, w.transform.position);

                    if (oldDist > dist)
                    {
                        target = w;
                    }
                }
            }
        }
    }
    

    public void ApplyStatusEffect(StatusEffect effect)
    {
        effects.Add(effect);
    }

    protected void UpdateEffects()
    {
        if(effects.Count > 0)
        {
            try
            {
                foreach (var effect in effects)
                {
                    if (effect != null)
                    {
                        effect.Update();
                        if (effect.countDown())
                        {
                            effect.EndEffect();
                            effects.Remove(effect);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        
    }

    protected bool IsWizardCloser(Wizard a, Wizard b)
    {
        return Vector3.Distance(transform.position, a.transform.position) >
            Vector3.Distance(transform.position, b.transform.position);
    }

    public virtual void Behave()
    {
        //Do Nothing
    }

    public virtual void BehaveIdle()
    {
        //Do Nothing
    }

    public virtual void BehaveRunning()
    {
        //Do Nothing
    }

    public virtual void BehaveAttacking()
    {
        //Do Nothing
    }

    public virtual void BehaveFleeing()
    {
        //Do Nothing
    }
}
