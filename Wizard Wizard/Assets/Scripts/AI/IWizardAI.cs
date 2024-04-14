using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWizardAI
{
    void Behave();

    void ApplyStatusEffect(StatusEffect effect);
}
