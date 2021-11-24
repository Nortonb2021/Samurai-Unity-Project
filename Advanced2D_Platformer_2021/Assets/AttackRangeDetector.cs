using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    [SerializeField]
    internal EnemyController enemyController;

    [SerializeField]
    Transform attackPoint;

    public LayerMask Player;

    public float attackRange;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var collider = Physics2D.OverlapCircle(attackPoint.position, attackRange, Player);

        enemyController.isInRange = collider != null;
       
        if(collider != null)
        {
            enemyController.isInRange = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
