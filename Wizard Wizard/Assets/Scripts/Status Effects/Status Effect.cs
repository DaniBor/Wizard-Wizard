using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect
{
    private float duration;

    private float tickTime;
    private float secondsTillTick;

    public Wizard target;

    public IStatusEffect effect;
    

    public enum EffectType
    {
        BUFF,
        DEBUFF,
        STUN,
        STUNIMMUNITY
    }
    public EffectType type;

    public StatusEffect(float duration, float tickTime, Wizard target, IStatusEffect effect, EffectType type)
    {
        this.duration = duration;
        this.tickTime = tickTime;;
        this.target = target;
        this.effect = effect;
        this.type = type;
    }

    public void Update()
    {
        if(secondsTillTick <= 0)
        {
            Debug.Log("Ticking...");
            effect.OnTick(target);
            secondsTillTick = tickTime;
        }
    }

    public bool countDown()
    {
        duration -= Time.deltaTime;
        secondsTillTick -= Time.deltaTime;
        
        return duration <= 0;
    }

    public void EndEffect()
    {
        target = null;
        effect.OnEnd();
    }
}
