using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    //**BUGS**\\\
    // jump hit doesnt switch to knockback.

    #region CHILD SCRIPTS
    [Header("CHILD SCRIPTS")]
    
    [SerializeField] CharacterController2D charactercontroller;
    [SerializeField] AudioManager audioManager;
    
    public HealthBar healthBar;
    public static Player instance;
    public Collider2D playerBody;
    public Collider2D enemyBody;
    #endregion


    #region PUBLIC PROPERTIES

    [Header("PLAYER HEALTH")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("PLAYER PROPERTIES")]
    public float gravity = 20f;
    public float walkSpeed = 10f;
    public float defensiveWalkSpeed = 5f;
    public float jumpSpeed = 15f;
 

    public float xWallJumpSpeed = 15f;
    public float yWallJumpSpeed = 15f;
    public float xBackflipSpeed = 15f;
    public float yBackflipSpeed = 15f;

    public float wallJumpWaiter;
    public float wallSlideAmount = 0.1f;

    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldownTime = 1f;
    public float invincibleTime = 1f;

    public float verticalKnockbackForce;
    public float horizontalKnockbackForce;
    public float knockbackLength;
    public float knockbackCount;



    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.9f;
    public float attackDelay;








    [Header("PLAYER ABILITIES")]
    public bool canWallJump;
    public bool canWallSlide;



    [Header("PLAYER STATES")]
    public bool isGrounded;
    public bool isJumping;
    public bool isWallJumping;
    public bool isDashing;
    public bool isBlocking;
    public bool isHit;
    public bool isUsing;
    public bool isDefensive;
    public bool isDead;

    public bool isFacingRight;
    public bool isHitFromRight;
    

    public bool isAttackin = false;
    public bool isInvincible;
    public bool isWallGrabbing;
    public bool isWallSliding;


    public bool attackPressed;
    public bool attack1;
    public bool attack2;
    public bool airAttack;

    public bool testBool;
    public LayerMask enemyLayers;
    #endregion


    #region PRIVATE PROPERTIES
    //INPUT FLAGS
    private bool jumpPressed;
    private bool _releaseJump;
    private bool _startAttack;
    public float _dashTimer;

    public Vector2 _input;
    private Vector2 _moveDirection;

    public Animator anim;

 
    #endregion



    private enum State { idle, run, jump, fall, doublejump, walljump, wallslide, defensiveidle, defensivewalk, 
        backflip, dash, attack1, attack2, knockback, block , airattack } 

    private State state = State.idle;


    private void Awake()
    {
        instance = this;
    }




    void Start()
    {
        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();

        

    }





    void Update()
    {

        //Combo Timer
        if(Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }


        //Dash Timer
        _dashTimer -= Time.deltaTime;
        if (_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
        }





        /*
        //INVULNERABILITY
        if (isInvincible)
        {
            Physics2D.IgnoreCollision(Collider2D playerBody, Collider2D enemyBody, true);
        }
        else if (!isInvincible)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }
        */


        //Flip
        if (!isBlocking && !isWallJumping)
        {
            if (_input.x < 0 && !isDefensive)//
            {
                isFacingRight = false;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }
            else if (_input.x > 0 && !isDefensive)//
            {
                isFacingRight = true;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }

        }



        //GROUND AND AIR MOVEMENT "BUT NOT WALL JUMPING".
        if (!isWallJumping)//
        {
            //Move left and right, grounded & in the air.
            _moveDirection.x = _input.x;
            _moveDirection.x *= walkSpeed;
        }




        //////////-----[STATE MACHINE BEHAVIOR]-----\\\\\\\\\\


        ////-Idle State = 0-\\\\
        if (charactercontroller.below && _moveDirection.x == 0 && !isDefensive)
        {
            state = State.idle;
        }



        ////-Run State [1]-\\\\
        if (charactercontroller.below && _moveDirection.x != 0 && knockbackCount <= 0 && !isWallJumping)
        {
            state = State.run;
            _moveDirection.y = 0f; //reset 0
        }



        ////Jump State [2]-\\\\
        if (jumpPressed)
        {
            state = State.jump;

            //ground jump
            if (charactercontroller.below)
            {

                jumpPressed = false;
                _moveDirection.y = jumpSpeed;
                isJumping = true;
                charactercontroller.DisableGroundCheck();

                noOfClicks = 0;

                audioManager.Play("Jump");
            }

        }
        else if (charactercontroller.groundType == GlobalTypes.GroundType.OneWayPlatform)
        {
            //tartCoroutine(DisableOneWayPlatform(true))


        }
          
        else
        {
            
            //short & long jump
            if (_releaseJump && !isWallJumping)//
            {
                _releaseJump = false;

                if (_moveDirection.y > 0)
                {
                    _moveDirection.y *= 0.5f;
                }
            }
            
        }

        
        ////Fall State [3]-\\\\
        if (!charactercontroller.below && (!charactercontroller.left || !charactercontroller.right) && _moveDirection.y < 0.1f)//
        {
            state = State.fall;
        }

   

        ////Wall Jump State [5]-\\\\ ""and not is dashing to be added""
        if (jumpPressed && !charactercontroller.below && (charactercontroller.left || charactercontroller.right))
        {
            
            state = State.walljump;

            isWallJumping = true;

            StartCoroutine("WallJumpWaiter");

            if (charactercontroller.left && jumpPressed)
            {
                _moveDirection.x = xWallJumpSpeed;
                _moveDirection.y = yWallJumpSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (charactercontroller.right && jumpPressed)
            {
                _moveDirection.x = -xWallJumpSpeed;
                _moveDirection.y = yWallJumpSpeed;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            jumpPressed = false;
            
            



        }
        

        //Wall Slide State [6]-\\\\ 
        if (!charactercontroller.below && (charactercontroller.left || charactercontroller.right) && _moveDirection.y < 0)
        {
            

            anim.SetTrigger("wallImpact");

            state = State.wallslide;

            if (charactercontroller.right)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (charactercontroller.left)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

        }
        else
        {
            anim.ResetTrigger("wallImpact");
        }



        ////Defensive Idle State [7]-\\\\
        if (charactercontroller.below && isDefensive && _moveDirection.x == 0)
        {
            state = State.defensiveidle;
        }


        ////Defensive Walk State [8]-\\\\
        if (charactercontroller.below && isDefensive && _moveDirection.x != 0)
        {
            state = State.defensivewalk;


            if (isFacingRight)
            {


                if (_input.x > 0)
                {
                    _moveDirection.x = defensiveWalkSpeed;
                    anim.SetFloat("defensewalkspeed", 1);
                }
                if (_input.x < 0)
                {
                    _moveDirection.x = -defensiveWalkSpeed;
                    anim.SetFloat("defensewalkspeed", -1);
                }



            }
            else if (!isFacingRight)
            {
                if (_input.x > 0)
                {
                    _moveDirection.x = defensiveWalkSpeed;
                    anim.SetFloat("defensewalkspeed", -1);
                    
                }
                if (_input.x < 0)
                {
                    _moveDirection.x = -defensiveWalkSpeed;
                    anim.SetFloat("defensewalkspeed", 1);
                }

            }
            
        }

   


        /*
         

     
        
        ////-Dash State [10]-\\\\
        if (isDashing)
        {

            state = State.dash;

            StartCoroutine("Dash");
           
            if (isFacingRight)
            {

                if(_input.x < 0)
                {
                    _moveDirection.x = -dashSpeed;
                }
                else if(_input.x >= 0)
                {
                    _moveDirection.x = dashSpeed;
                }

            }

            if (!isFacingRight)
            {

                if (_input.x > 0)
                {
                    _moveDirection.x = dashSpeed;
                }
                else if (_input.x <= 0)
                {
                    _moveDirection.x = -dashSpeed;
                }

            }




            _moveDirection.y = 0;

        }


       */




        ////-Attack State [11]-\\\\
        if (attack1 && charactercontroller.below)
        {
            state = State.attack1;
            

            if (isGrounded)
            {
                _moveDirection.x = 0;
            }

           
        }




        ////-Attack State [12]-\\\\
        if (attack2 && !attack1 && charactercontroller.below)
        {
            state = State.attack2;

            if (isGrounded)
            {
                _moveDirection.x = 0;
            }

            
        }





        
        ////-Knockback State [13]-\\\\      ***find better method of detecting hit - variables of hit length and damage to deal*** 
        if (knockbackCount > 0 && !isDashing)
        {




            if (!isBlocking)
            {
                state = State.knockback;

                isHit = true;

                //hit from right
                if (isHitFromRight) 
                {
                    _moveDirection.x = -horizontalKnockbackForce;
                    _moveDirection.y = verticalKnockbackForce;
 
                }
                //hit from left
                else if (!isHitFromRight)
                {
                    _moveDirection.x = horizontalKnockbackForce;
                    _moveDirection.y = verticalKnockbackForce;
                }

                StartCoroutine("ImpactPause");
                StartCoroutine("Invincible");

                audioManager.Play("PlayerHit");

                
            }




            else if (isBlocking)
            {
                state = State.block;

                //hit from right
                if (isHitFromRight)
                {
                    _moveDirection.x = -horizontalKnockbackForce;
                }
                //hit from left
                else if (!isHitFromRight)
                {
                    _moveDirection.x = horizontalKnockbackForce;
                }

                audioManager.Play("SwordClash");

            }

            knockbackCount -= Time.deltaTime;

        }
  
      


        ////-Block [14]-\\\\
        if (isBlocking)
        {
            state = State.block;

            _moveDirection.x = 0;


        }





        ////-Air Attack [15]-\\\\
        if (airAttack && !charactercontroller.below)
        {
            state = State.airattack;

        }




        ////-GROUNDED LOGIC-\\\\
        if (charactercontroller.below && knockbackCount <= 0)
        {
            _moveDirection.y = 0f; //reset 0

            isGrounded = true;

            anim.SetBool("isGrounded", true);

            // audioManager.Play("Land");



            //Clear Flags
            airAttack = false;
            isJumping = false;
            isWallSliding = false;
            //isHit = false;
            anim.ResetTrigger("wallImpact");



        }
        ////-IN AIR LOGIC-\\\\
        else
        {
            isGrounded = false;

            anim.SetBool("isGrounded", false);

            GravityCalculations();

           

        }







        StateSwitch();

        charactercontroller.Move(_moveDirection * Time.deltaTime);

        anim.SetInteger("state", (int)state);


    }





    public void TakeHit()
    {
        //takeHit = true;
    }



    public void Footsteps()
    {
        audioManager.Play("FootStep");
    }

    public void LandSFX()
    {
        audioManager.Play("Land");
       
    }

     
    private void StateSwitch()
    {

      
    }


    //GRAVITY
    void GravityCalculations()
    {



        //Disable Gravity
        if (_moveDirection.y > 0f && charactercontroller.above)
        {
            _moveDirection.y = 0f;
        }



        //Wall Slide
        if (canWallSlide && (charactercontroller.left || charactercontroller.right))
        {

            

            if (charactercontroller.hitWallThisFrame)
            {
                _moveDirection.y = 0;
                isWallSliding = true;
               
            }

            if (_moveDirection.y <= 0)
            {
                _moveDirection.y -= (gravity * wallSlideAmount) * Time.deltaTime;
            }
            else
            {
                _moveDirection.y -= gravity * Time.deltaTime;
            }

        }
        else
        {

            if (!isDashing)
            {
                _moveDirection.y -= gravity * Time.deltaTime;
            }

            isWallSliding = false;
        }





    }



    #region INPUT METHODS

    //look at coding out cancel movements such as dash and attacks using input methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //pressed // find a better method to stop the second jump when button pressed in air.
        if (context.started)
        {
            jumpPressed = true;
                _releaseJump = false; 
        }
        //released
        else if (context.canceled)
        {
            _releaseJump = true;
            jumpPressed = false;
        }

    }

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
        if (context.started && !isDashing && !isBlocking)
        {
            attackPressed = true;

            lastClickedTime = Time.time;

            if (charactercontroller.below)
            {
                noOfClicks++;

            }
            else if (!charactercontroller.below)
            {
                airAttack = true;
            }


            if(noOfClicks == 1)
            {
                attack1 = true;
            }
            else if (noOfClicks == 2)
            {
                attack2 = true;
                StartCoroutine("AttackDelay");
            }
          

            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);





        }
        else
        {
            attackPressed = false;
        }
      
    }

    public void OnDefensive(InputAction.CallbackContext context)
    {
        //pressed
        if (context.started)
        {

            isDefensive = true;

        }
        //released
        else if (context.canceled)
        {
            isDefensive = false;

        }


    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        //pressed
        if (context.started && charactercontroller.below)
        {

            isBlocking = true;

        }
        //released
        else if (context.canceled)
        {
            isBlocking = false;

        }


    }

    public void OnFlip(InputAction.CallbackContext context)
    {
        //pressed
        if (context.performed && isDefensive)
        {

            if (isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                isFacingRight = false;
            }
            else if (!isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                isFacingRight = true;
            }

            

        }


    }

    public void OnUse(InputAction.CallbackContext context)
    {
        //pressed // find a better method to stop the second jump when button pressed in air.
        if (context.started)
        {
            isUsing = true;
        }
        //released
        else if (context.canceled)
        {
            isUsing = false;
        }
    }






    #endregion



    #region COURTINES

    IEnumerator WallJumpWaiter()
    {
        
        yield return new WaitForSeconds(wallJumpWaiter);

        isWallJumping = false;
       
    }


    IEnumerator Dash()
    {

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        isInvincible = false;

        _dashTimer = dashCooldownTime;

    }


    IEnumerator ImpactPause()
    {
        var original = Time.timeScale;

        Time.timeScale = 0;


        yield return new WaitForSecondsRealtime(0.2f);


        Time.timeScale = 1;


    }


    IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSecondsRealtime(invincibleTime);

        isInvincible = false;
    }


   
    IEnumerator AttackDelay()
    {

        yield return new WaitForSecondsRealtime(attackDelay);

        noOfClicks = 0;

    }


    /*
    IEnumerator DisableOneWayPlatform()
    {
        bool 
    }
    */


    #endregion

}
