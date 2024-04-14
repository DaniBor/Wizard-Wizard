using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class FireWizard : Wizard, IWizardAI
{
    [SerializeField] private GameObject fireProjectilePrefab;
    private Rigidbody2D rb;

    private void Awake()
    {
        attackTimer = 2;
        timeTilAttack = attackTimer;
        attackRate = 1;
        curState = WizardState.RUNNING;

        health = 50;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        getClosestWizard();
        speed = 1.5f;
    }

    private void Update()
    {

        UpdateEffects();
        Behave();
    }

    public void Behave()
    {
        if(target == null)
            getClosestWizard();

        switch (curState)
        {
            case WizardState.RUNNING:
                break;
            case WizardState.ATTACKING:
                UpdateAttack();
                break;
        }

        timeTilAttack -= attackRate * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    

    void UpdateMovement()
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
            target = null;
            return;
        }
    }

    void UpdateAttack()
    {
        if (timeTilAttack <= 0)
        {
            Vector2 dir;
            try
            {
                dir = transform.position - target.transform.position;
            }
            catch (Exception)
            {
                target = null;
                curState = WizardState.RUNNING;
                return;
            }

            dir.Normalize();
            FireballProjectile projectile =
                Instantiate(fireProjectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<FireballProjectile>();

            projectile.dir = dir;
            projectile.isAllyProjectile = isAlly;
            projectile.speed = 5.0f;

            timeTilAttack = attackTimer;
        }
    }
}
