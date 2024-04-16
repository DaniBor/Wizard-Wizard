using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;

    protected int defense;

    [SerializeField] protected float speed;

}
