using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : MonoBehaviour, IProjectile
{
    private int maxTargets;
    private int maxRange;
    public bool isAllyProjectile;

    private List<Wizard> lightningChain = new List<Wizard>();

    private LineRenderer lr;

    public void Behave()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxRange = 3;
        maxTargets = 4;

        lr = GetComponent<LineRenderer>();
        lightningChain = FindTargets();
        RenderLine();

    }

    // Update is called once per frame
    void Update()
    {
        
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

    List<Wizard> FindTargets()
    {
        List<Wizard> targets = Overseer.Instance.getEnemyWizards();
        targets = SortTargets(transform, targets);


        if (Vector3.Distance(transform.position, targets[0].transform.position) < maxRange)
        {
            lightningChain.Add(targets[0]);
            targets.RemoveAt(0);
        }
        else return null;


        for (int i = 0; i < maxTargets; i++)
        {
            targets = SortTargets(lightningChain[lightningChain.Count - 1].transform, targets);

            Debug.Log(Vector3.Distance(lightningChain[i].transform.position, targets[0].transform.position));
            if (Vector3.Distance(lightningChain[i].transform.position, targets[0].transform.position) < maxRange)
            {
                lightningChain.Add(targets[0]);
                targets.RemoveAt(0);
            }
            else break;

            
        }
        Debug.Log(lightningChain.Count);
        return lightningChain;
    }

    private void RenderLine()
    {
        
        if (lightningChain != null)
        {
            foreach (var target in lightningChain)
            {
                Debug.Log(target.name);
            }

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
                wizard.DamageMe(new Damage(3, Damage.DamageType.LIGHTNING));
            }
        }
    }
}

