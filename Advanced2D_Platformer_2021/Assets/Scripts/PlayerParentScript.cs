using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParentScript : MonoBehaviour
{



    [Header("COLLISION PARAMETERS")]
    public LayerMask LevelLayer;
    

    public bool below;
    public bool left;
    public bool right;
    public bool above;

    public float collisonRadius;

    public Vector2 belowOffset;
    public Vector2 rightOffset;
    public Vector2 leftOffset;
    public Vector2 aboveOffset;

    public Color gizmoColor = Color.red;

    public int side;

  






    public Vector2 input;


    private bool _startJump;
    private bool _releaseJump;


    internal Rigidbody2D rb;
















    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


      

      


        
   

     


    }









    #region INPUT METHODS

    //look at coding out cancel movements such as dash and attacks using input methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //pressed
        if (context.started)
        {
            _startJump = true;
            _releaseJump = false;
        }
        //released
        else if (context.canceled)
        {
            _releaseJump = true;
            _startJump = false;
        }
    }
/*
    public void OnDash(InputAction.CallbackContext context)
    {


        //pressed
        if (context.started && _dashTimer <= 0)
        {
            isDashing = true;
            isInvincible = true;
            StartCoroutine("Dash");

        }
        //released
        else if (context.canceled && _dashTimer <= 0)
        {

            if (isDashing)
            {
                _dashTimer = dashCooldownTime;
            }

            StopCoroutine("Dash");
            isDashing = false;
            isInvincible = false;
        }

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing)
        {
            isAttacking = true;
            isDashing = false;
            anim.SetBool("isAttacking", true);
        }
        else
        {
            isAttacking = false;

        }

    }

    public void OnDefend(InputAction.CallbackContext context)
    {
        //pressed
        if (context.started)
        {

            isBlocking = true;

        }
        //released
        else if (context.canceled)
        {
            isBlocking = false;

        }

    }


    */


    #endregion


}
