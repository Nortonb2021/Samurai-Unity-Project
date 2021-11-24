using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D playercollision)
    {
        if (playercollision.tag == "Player")
        {

            var player = playercollision.GetComponent<Player>();
            player.knockbackCount = player.knockbackLength;

            if (playercollision.transform.position.x < transform.position.x)

                player.isHitFromRight = true;
            else
                player.isHitFromRight = false;

            //enemy.TakeDamage(damageToDeal);

        }
    }


}
