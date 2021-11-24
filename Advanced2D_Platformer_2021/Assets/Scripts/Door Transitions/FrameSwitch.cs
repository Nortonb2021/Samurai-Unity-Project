using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSwitch : MonoBehaviour
{

    //first frame to transition from i.e, could be frame "3"
    public GameObject frame1;
    //second frame your transitioning to, i.e would be frame "4"
    public GameObject frame2;


    //added by me to move position on switch





    public Transform playerpos;
    public float frame1PosX;
    public float frame1PosY;

    public float frame2PosX;
    public float frame2PosY;


    //when you enter the trigger collider 
    private void OnTriggerEnter2D(Collider2D collision)
    {

       // Moving from 1 > 2 
        if(frame1.active == true)
        {
            //deactivate frame
            frame1.SetActive(false);
            frame2.SetActive(true);

           
            playerpos.position = new Vector2(frame1PosX, frame1PosY);
            


        }
        //Moving from 2 > 1
        else if(frame1.active == false)
        {

            frame1.SetActive(true);
            frame2.SetActive(false);

            playerpos.position = new Vector2(frame2PosX, frame2PosY);

        }
    }


}
