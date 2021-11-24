using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{


    [SerializeField]
    PlayerParentScript playerParentScript;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        playerParentScript.below = Physics2D.OverlapCircle((Vector2)transform.position + playerParentScript.belowOffset, playerParentScript.collisonRadius, playerParentScript.LevelLayer);
        playerParentScript.left = Physics2D.OverlapCircle((Vector2)transform.position + playerParentScript.leftOffset, playerParentScript.collisonRadius, playerParentScript.LevelLayer);
        playerParentScript.right = Physics2D.OverlapCircle((Vector2)transform.position + playerParentScript.rightOffset, playerParentScript.collisonRadius, playerParentScript.LevelLayer);
        playerParentScript.above = Physics2D.OverlapCircle((Vector2)transform.position + playerParentScript.aboveOffset, playerParentScript.collisonRadius, playerParentScript.LevelLayer);




    }






    
    private void OnDrawGizmos()
    {
        Gizmos.color = playerParentScript.gizmoColor;
        Gizmos.DrawSphere((Vector2)transform.position + playerParentScript.belowOffset, playerParentScript.collisonRadius);
        Gizmos.DrawSphere((Vector2)transform.position + playerParentScript.rightOffset, playerParentScript.collisonRadius);
        Gizmos.DrawSphere((Vector2)transform.position + playerParentScript.leftOffset, playerParentScript.collisonRadius);
        Gizmos.DrawSphere((Vector2)transform.position + playerParentScript.aboveOffset, playerParentScript.collisonRadius);
    }



    


}
