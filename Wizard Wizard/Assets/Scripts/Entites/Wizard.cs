using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Entity
{
    [SerializeField] protected bool isAlly;

    protected Wizard target;

    protected float attackTimer;
    protected float timeTilAttack;
    protected float attackRate;

    protected bool stunned;

    [SerializeField] public List<StatusEffect> effects;


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
        health = 0;

        effects = new List<StatusEffect>();
    }

    private void Update()
    {
        
    }

    public void DamageMe(Damage dmg)
    {
        health -= dmg.damageAmount - defense;
        if (health < 0)
        {
            DeleteWizard();
        }
    }

    public bool checkAlly()
    {
        return isAlly;
    }
    protected void getClosestWizard()
    {
        List<Wizard> enemies;
        if (isAlly)
            enemies = Overseer.Instance.getEnemyWizards();
        else
            enemies = Overseer.Instance.getPlayerWizards();

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
    protected void DeleteWizard()
    {
        if (isAlly)
            Overseer.Instance.OnAllyWizardKill(this);
        else
            Overseer.Instance.OnEnemyWizardKill(this);
        Destroy(gameObject);
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        effects.Add(effect);
    }

    protected void UpdateEffects()
    {
        foreach (var effect in effects)
        {
            if (effect != null)
            {
                Debug.Log("Updating Effcts...");
                effect.Update();
                if (effect.countDown()){
                    effect.EndEffect();
                    effects.Remove(effect);
                }
            }
        }
    }
}
