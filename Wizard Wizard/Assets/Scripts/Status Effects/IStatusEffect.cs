using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect
{

    void OnStart();
    void OnTick(Wizard wiz);
    void OnRefresh();
    void OnEnd();
}
