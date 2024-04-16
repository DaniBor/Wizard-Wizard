using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Wizard : Entity, IWizardAI
{
    [SerializeField] protected int manaCost;
    public bool isAlly;
    protected bool isStatusWizard = false;

    [SerializeField] protected Wizard target;

    [SerializeField] protected float attackTimer;
    [SerializeField] protected float timeTilAttack;
    [SerializeField] protected float attackRate;

    public List<StatusEffect> effects;
    protected Rigidbody2D rb;

    protected Dictionary<Damage.DamageType, int> resistances;


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
        effects = new List<StatusEffect>();

        resistances = new Dictionary<Damage.DamageType, int>
        {
            { Damage.DamageType.MAGIC, 0 },
            { Damage.DamageType.FIRE, 0 },
            { Damage.DamageType.LIGHTNING, 0 },
            { Damage.DamageType.WIND, 0 },
            { Damage.DamageType.EARTH, 0 },
            { Damage.DamageType.WATER, 0 }
        };
    }

    protected virtual void Update()
    {
        if (!isStunned())
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
        health -= Mathf.Max(1, dmg.damageAmount - getDefense() - resistances[dmg.type]);
        if (health < 0)
        {
            effects.Clear();
            Overseer.Instance.OnWizardKill(this);
            return;
        }
    }

    public int getDefense()
    {
        if (CheckForStatusEffect(StatusEffect.EffectType.DEFENSEBUFF))
            return defense * 2;

        return defense;
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
    
    protected void getClosestUnbuffedAlly(StatusEffect.EffectType type)
    {
        List<Wizard> allies = Overseer.Instance.getWizards(!isAlly);
        allies.RemoveAll(item => item.isStatusWizard == true);
        if (allies == null || allies.Count == 0)
            return;

        foreach (Wizard w in allies)
        {
            if (w != null && !w.CheckForStatusEffect(type))
            {
                if (w == this)
                    continue;

                if (target == null && !w.isStatusWizard)
                {
                    target = w;
                    continue;
                }
                else
                {
                    float oldDist = Vector2.Distance(transform.position, target.transform.position);
                    float dist = Vector2.Distance(transform.position, w.transform.position);

                    if (oldDist > dist && !target.isStatusWizard)
                    {
                        target = w;
                    }
                }
            }
        }
    }

    public bool ApplyStatusEffect(StatusEffect effect)
    {
        if (effect.type == StatusEffect.EffectType.STUN)
        {
            foreach (var eff in effects)
            {
                if (eff.type == StatusEffect.EffectType.STUN || eff.type == StatusEffect.EffectType.STUNIMMUNITY)
                    return false;
            }
            effects.Add(new StatusEffect(effect.duration + 15, 1, this, new EffectStunimmunity(), StatusEffect.EffectType.STUNIMMUNITY));
        }

        effects.Add(effect);
        return true;
    }

    public bool CheckForStatusEffect(StatusEffect.EffectType effect)
    {
        foreach (var eff in effects)
        {
            if(eff.type == effect)
                return true;
        }
        return false;
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

    public int getCost()
    {
        return manaCost;
    }

    protected bool isStunned()
    {
        bool result = false;
        foreach (var effect in effects)
        {
            if(effect.type == StatusEffect.EffectType.STUN)
                result = true;
        }
        return result;
    }

    public Rigidbody2D getRigidBody()
    {
        return rb;
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
