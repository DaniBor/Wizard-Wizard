using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBurning : IStatusEffect
{
    public void OnStart()
    {
        //Do Nothing
    }

    public void OnTick(Wizard wiz)
    {
        if(wiz != null)
            wiz.DamageMe(new Damage(1, Damage.DamageType.FIRE, false));
    }

    public void OnRefresh()
    {
        //Do Nothing
    }

    public void OnEnd()
    {
        //Do Nothing
    }

}
