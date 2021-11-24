using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private bool playerDetected;
    public Transform doorPos;
    public float width;
    public float height;

    public LayerMask playerLayer;

    [SerializeField]
    private string sceneName;


    Player player;
    SceneSwitch sceneSwitch;


    public Transform playerpos;

    public float posX;
    public float posY;




    private void Start()
    {
        player = FindObjectOfType<Player>();
        sceneSwitch = FindObjectOfType<SceneSwitch>();

    }


    private void Update()
    {


        
        playerDetected = Physics2D.OverlapBox(doorPos.position, new Vector2(width, height), 0, playerLayer);
        

        if(playerDetected == true)
        {

            /*
            if (player.isUsing)
            {
                sceneSwitch.SwitchScene(sceneName);
            }
            */

            if (player.isUsing)
            {
                playerpos.position = new Vector2(posX, posY);
            }



        }










    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(doorPos.position, new Vector3(width, height, 1));
    }

}
