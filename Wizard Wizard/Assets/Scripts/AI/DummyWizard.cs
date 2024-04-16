using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyWizard : Wizard, IWizardAI
{
    public sealed override void Behave()
    {
        //Do nothing
    }

    public sealed override void BehaveAttacking()
    {
        //Do nothing
    }

    public sealed override void BehaveFleeing()
    {
        //Do nothing
    }

    public sealed override void BehaveIdle()
    {
        //Do nothing
    }

    public sealed override void BehaveRunning()
    {
        //Do nothing
    }

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
}
