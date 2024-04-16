using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public enum DamageType
    {
        MAGIC,
        FIRE,
        LIGHTNING,
        WATER,
        EARTH,
        WIND,
    }
    public DamageType type;
    public int damageAmount;
    bool isbuffed;

    public int getDamage()
    {
        if (isbuffed)
            return damageAmount * 2;
        return damageAmount;
    }

    public Damage(int amount,  DamageType type, bool isBuffed)
    {
        this.type = type;
        this.damageAmount = amount;
    }
}
