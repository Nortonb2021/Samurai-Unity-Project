using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRangeBox : MonoBehaviour
{
    [SerializeField]
    internal EnemyController enemycontroller;



    private void OnTriggerEnter2D(Collider2D attackRange)
    {
        if (attackRange.tag == "Player")
        {
            enemycontroller.isInRange = true;
        }

    }



    private void OnTriggerExit2D(Collider2D attackRange)
    {
        if (attackRange.tag == "Player")
        {
            enemycontroller.isInRange = false;
        }
    }
}
