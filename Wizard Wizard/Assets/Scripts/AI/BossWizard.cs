using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWizard : Wizard
{
    public Vector3 targetPosition;



    protected sealed override void Awake()
    {
        base.Awake();
    }

    protected sealed override void Update()
    {
        base.Update();
    }

    protected sealed override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void BehaveIdle()
    {
        curState = WizardState.RUNNING;
    }

    public override void BehaveRunning()
    {
        Vector3 dir = transform.position - targetPosition;
        dir.Normalize();
        rb.MovePosition(transform.position - speed * Time.deltaTime * dir);

        if(Vector3.Distance(rb.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
            SpawnRandomWizard();
        }

    }

    public override void BehaveAttacking()
    {
        
    }

    public override void BehaveFleeing()
    {
        //Will not flee (not like any fleeing behavior exists...)
    }

    private void SpawnRandomWizard()
    {
        Overseer.Instance.SpawnRandomEnemyWizard(transform);
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = Random.insideUnitCircle * 10f;
    }
}
