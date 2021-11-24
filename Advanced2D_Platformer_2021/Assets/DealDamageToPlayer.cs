using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToPlayer : MonoBehaviour
{
 
    //Player Hit Detection
    private void OnTriggerEnter2D(Collider2D hitPlayer)
    {
        if (hitPlayer.gameObject.tag == "Player")
        {

            var player = hitPlayer.gameObject.GetComponent<Player>();


            if (player.isBlocking)
            {
                player.knockbackCount = 0.05f;
            }
            else
            {
                player.knockbackCount = player.knockbackLength;
            }
                    
            



            if (hitPlayer.transform.position.x < transform.position.x)

                player.isHitFromRight = true;

            else
                player.isHitFromRight = false;

           

            //enemy.TakeDamage(damageToDeal);

        }
      
        
    }
    
}
