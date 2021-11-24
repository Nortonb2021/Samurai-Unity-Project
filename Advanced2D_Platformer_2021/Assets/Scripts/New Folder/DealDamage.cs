using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
        public int damageToDeal;

    [SerializeField]
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter2D(Collider2D enemycollision)
    {
        if (enemycollision.tag == "Enemy")
        {

            var enemy = enemycollision.GetComponent<EnemyController>();
            enemy.knockbackCount = enemy.knockbackLength;

            if (enemycollision.transform.position.x < transform.position.x)

                enemy.isKnockbackFromRight= true;

            else

                enemy.isKnockbackFromRight= false;

            audioManager.Play("HitSoftBody");

           enemy.TakeDamage(damageToDeal);

        }
    }
    

}
