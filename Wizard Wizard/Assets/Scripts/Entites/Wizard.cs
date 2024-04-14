using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Entity
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


    private void Awake()
    {
        speed = 1.5f;

        effects = new List<StatusEffect>();
    }

    private void Update()
    {
        
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

        if (dmg.type == Damage.DamageType.LIGHTNING)
        {
            Debug.Log("Checking for stun...");
            if (effects.Count > 0)
            {
                foreach(var effect in effects)
                {
                    if (effect.type == StatusEffect.EffectType.STUN){
                        Debug.Log("Stun detected...");
                        return;
                    }
                }
            }
            Debug.Log("Applying Status effect...");
            ApplyStatusEffect(new StatusEffect(1, 1, this, new EffectLightning(), StatusEffect.EffectType.STUN));
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
}
