using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWizardAI
{
    void BehaveIdle();
    void BehaveRunning();
    void BehaveAttacking();
    void BehaveFleeing();

    void ApplyStatusEffect(StatusEffect effect);
}
