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
    }
    public DamageType type;
    public int damageAmount;

    public Damage(int amount,  DamageType type)
    {
        this.type = type;
        this.damageAmount = amount;
    }
}
