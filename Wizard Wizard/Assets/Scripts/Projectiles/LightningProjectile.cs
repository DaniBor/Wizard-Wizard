using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningProjectile : MonoBehaviour, IProjectile
{
    private int maxTargets;
    private float maxRange;
    public bool isAllyProjectile;
    public bool isBuffed;

    private List<Wizard> lightningChain = new List<Wizard>();

    private LineRenderer lr;

    [SerializeField] private GameObject lightingParticlePrefab;

    private float timer = 0.2f;

    public void Behave()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxRange = 3.5f;
        maxTargets = 4;

        lr = GetComponent<LineRenderer>();
        FindTargets();
        if(lightningChain.Count > 0)
        {
            RenderLine();
            ApplyLightning();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    List<Wizard> SortTargets(Transform t, List<Wizard> wizards)
    {
        wizards.Sort(delegate (Wizard a, Wizard b)
        {
            return Vector2.Distance(t.position, a.transform.position)
         .CompareTo(
           Vector2.Distance(t.position, b.transform.position));
        });

        return wizards;
    }

    void FindTargets()
    {
        List<Wizard> targetsInRange = new List<Wizard>();
        Transform anchor = transform;
        //targets = SortTargets(transform, targets);

        for(int i = 0; i < maxTargets; i++)
        {
            targetsInRange = Overseer.Instance.getWizardsInRange(anchor, maxRange, isAllyProjectile);
            if (targetsInRange.Count > 0)
                targetsInRange = SortTargets(anchor, targetsInRange);
            else break;

            targetsInRange.RemoveAll(item => lightningChain.Contains(item) == true);
            if (targetsInRange.Count > 0)
            {
                lightningChain.Add(targetsInRange[0]);
                anchor = targetsInRange[0].transform;
            }
            else break;
        }
    }

    private void RenderLine()
    {
        if (lightningChain != null)
        {
            Vector3[] pos = new Vector3[lightningChain.Count + 1];
            pos[0] = transform.position;
            for (int i = 1; i < lightningChain.Count + 1; i++)
            {
                pos[i] = lightningChain[i - 1].transform.position;
                pos[i].z = -0.1f;
            }
            lr.positionCount = pos.Length;
            lr.SetPositions(pos);
        }
    }

    private void ApplyLightning()
    {
        if (lightningChain != null)
        {
            foreach(var wizard in lightningChain)
            {

                EffectAir el = new EffectAir();
                StatusEffect lightning = new StatusEffect(4, 1, wizard, el, StatusEffect.EffectType.STUN);

                if (wizard.ApplyStatusEffect(lightning))
                {
                    wizard.DamageMe(new Damage(2, Damage.DamageType.LIGHTNING, isBuffed));
                    Instantiate(lightingParticlePrefab, wizard.transform.position, Quaternion.identity);
                }
            }
        }
    }
}

