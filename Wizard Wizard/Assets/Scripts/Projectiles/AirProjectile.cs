using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AirProjectile : MonoBehaviour, IProjectile
{
    CircleCollider2D circleCollider;

    public float speed;
    public Vector2 dir;
    public bool isAllyProjectile;
    public bool isBuffed;

    private class TornadoVictim
    {
        public TornadoVictim(Wizard wiz)
        {
            this.wiz = wiz;
        }

        public Wizard wiz;
        public float timeCaught;
    }

    private List<TornadoVictim> victimList = new List<TornadoVictim>();
    private List<Wizard> caughtWizards = new List<Wizard>();

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        Behave();
    }

    public void Behave()
    {
        transform.position -= (Vector3)(speed * Time.deltaTime * dir);

        //Projectile is out of bounds
        if (Vector2.Distance(Vector2.zero, transform.position) >= 100)
            Destroy(gameObject);

        victimList.RemoveAll(item => item.timeCaught >= 3);
        victimList.RemoveAll(item => item.wiz.IsDestroyed() == true);

        foreach (TornadoVictim victim in victimList)
        {

            Rigidbody2D vrb = victim.wiz.getRigidBody();
            Vector3 newPos = new Vector3(
                transform.position.x + Mathf.Sin(victim.timeCaught * Mathf.PI),
                transform.position.y + Mathf.Sin(victim.timeCaught + 1 * Mathf.PI),
                0f);
            vrb.MovePosition(newPos);
            victim.timeCaught += Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Wizard>(out var wiz))
        {
            if (isAllyProjectile != wiz.checkAlly() && !caughtWizards.Contains(wiz))
            {
                EffectAir ea = new EffectAir();
                StatusEffect air = new StatusEffect(2, 1, wiz, ea, StatusEffect.EffectType.STUN);

                if (wiz.ApplyStatusEffect(air))
                {
                    wiz.DamageMe(new Damage(1, Damage.DamageType.WIND, isBuffed));
                    victimList.Add(new TornadoVictim(wiz));
                    caughtWizards.Add(wiz);
                }

                
            }
        }
    }
}
