using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private GameObject earthParticleEffect;

    private int maxRange;
    public bool isAllyProjectile;
    public bool isBuffed;

    private Wizard target;
    List<Wizard> targetsInRange;

    private void Awake()
    {
        maxRange = 2;
        targetsInRange = new List<Wizard>();
    }

    public void targetWizard(Wizard wizard)
    {
        target = wizard;
        targetsInRange = Overseer.Instance.getWizardsInRange(wizard.transform, maxRange, isAllyProjectile);

        target.DamageMe(new Damage(3, Damage.DamageType.MAGIC, isBuffed));
        Instantiate(earthParticleEffect, target.transform.position, Quaternion.identity);
        foreach (Wizard wiz in targetsInRange)
        {
            wiz.DamageMe(new Damage(3, Damage.DamageType.MAGIC, isBuffed));
            Instantiate(earthParticleEffect, wiz.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public void Behave()
    {
        throw new System.NotImplementedException();
    }
}
