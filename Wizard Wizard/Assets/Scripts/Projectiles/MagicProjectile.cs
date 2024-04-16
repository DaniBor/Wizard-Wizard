using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour, IProjectile
{
    CircleCollider2D circleCollider;

    public float speed;
    public Vector2 dir;
    public bool isAllyProjectile;
    public bool isBuffed;


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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Wizard>(out var wiz))
        {
            if (isAllyProjectile != wiz.checkAlly())
            {
                wiz.DamageMe(new Damage(5, Damage.DamageType.MAGIC, isBuffed));

                Destroy(gameObject);
            }
        }
    }
}
