using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overseer : MonoBehaviour
{
    public static Overseer instance { get; private set; }

    private HashSet<Wizard> playerWizards;
    private HashSet<Wizard> enemyWizards;



    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
}
