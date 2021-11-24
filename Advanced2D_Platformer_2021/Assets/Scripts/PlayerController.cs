using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{







    #region PUBLIC PROPERTIES

    public static PlayerController instance;

    public HealthBar healthBar;






    [Header("PLAYER HEALTH")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("PLAYER PROPERTIES")]
    public float gravity = 20f;
    public float walkSpeed = 10f;
    public float jumpSpeed = 15f;
    public float doubleJumpSpeed = 10f;

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


    [Header("PLAYER ABILITIES")]
    public bool canWallJump;
    public bool canWallSlide;



    [Header("PLAYER STATES")]
    public bool isDead;
    public bool isGrounded;
    public bool isFacingRight;
    public bool isJumping;
    public bool isDoubleJumping;
    public bool isDashing;
    public bool isWallJumping;
    public bool isKnockedBack;
    public bool isBlocking;
    public bool isBackflipping;

    public bool isAttacking = false;
    public bool isInvincible;
    public bool isWallGrabbing;
    public bool isWallSliding;

    public bool isKnockbackFromRight;
    public bool isKnockbackFromLeft;

    public bool testBool;


    public LayerMask enemyLayers;


    #endregion


    #region PRIVATE PROPERTIES
    //INPUT FLAGS

    private bool _startJump;
    private bool _releaseJump;

    private bool _startAttack;


    public float _dashTimer;


    public Vector2 _input;
    private Vector2 _moveDirection;

    //private CharacterController2D _characterController;
    //private CharacterController characterController;
    // private EnemyController _enemycontroller;

    public Animator anim;
    #endregion


    /*
    private void Awake()
    {
        instance = this;

    }




    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);


        // _characterController = gameObject.GetComponent<CharacterController2D>();
        //characterController = gameObject.GetComponent<CharacterController>();
        //_enemycontroller = gameObject.GetComponent<EnemyController>();

        anim = GetComponent<Animator>();

    }





    void Update()
    {

        _dashTimer -= Time.deltaTime;

        if (_dashTimer > 0)
        {
            _dashTimer -= Time.deltaTime;
        }




        //Sets moveY for animation
        anim.SetFloat("MovementY", _moveDirection.y);




        //INVULNERABILITY
        if (isInvincible)
        {
            Physics2D.IgnoreLayerCollision(3, 7, true);
        }
        else if (!isInvincible)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
        }




        //Flip
        if (!isBlocking)
        {
            if (_moveDirection.x < 0)
            {
                isFacingRight = false;
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            }
            else if (_moveDirection.x > 0)
            {
                isFacingRight = true;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }

        }





        //MOVEMENT
        if (!isWallJumping)
        {


            if (!isBackflipping)
            {
                _moveDirection.x = _input.x;
                _moveDirection.x *= walkSpeed;

            }

            // _moveDirection.x *= walkSpeed;





            //DASH
            if (isDashing)
            {

                anim.SetBool("isDashing", true);

                if (isFacingRight)
                {

                    _moveDirection.x = dashSpeed;
                }
                else
                {
                    _moveDirection.x = -dashSpeed;
                }

                _moveDirection.y = 0;

            }
            else if (!isDashing)
            {
                anim.SetBool("isDashing", false);
            }


            //KNOCKBACK
            if (knockbackCount > 0 && !isDashing)
            {


                if (isKnockbackFromRight)
                {
                    _moveDirection.x = -horizontalKnockbackForce;
                    _moveDirection.y = verticalKnockbackForce;
                    //transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                }
                if (!isKnockbackFromRight)
                {
                    _moveDirection.x = horizontalKnockbackForce;
                    _moveDirection.y = verticalKnockbackForce;
                    //transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                }



                StartCoroutine("ImpactPause");

                StartCoroutine("Invincible");


                knockbackCount -= Time.deltaTime;


            }


            //DEFENSE
            if (isBlocking && isGrounded && !isDashing)
            {


                anim.SetBool("isIdleDefense", true);
                anim.SetBool("isWalkingDefense", false);




                if (isFacingRight)
                {
                    if (_input.x > 0)
                    {
                        if (!isBackflipping)
                        {
                            _moveDirection.x = walkSpeed * 0.6f;
                            anim.SetFloat("defenseWalkSpeed", 1);
                            anim.SetBool("isWalkingDefense", true);
                            anim.SetBool("isIdleDefense", false);
                        }



                    }
                    if (_input.x < 0)
                    {
                        if (!isBackflipping)
                        {
                            _moveDirection.x = -walkSpeed * 0.6f;
                            anim.SetFloat("defenseWalkSpeed", -1);
                            anim.SetBool("isWalkingDefense", true);
                            anim.SetBool("isIdleDefense", false);
                        }

                        //Backflip
                        if (_startJump)
                        {
                            isBackflipping = true;
                            anim.SetBool("isBackflip", true);
                            // StartCoroutine("Flash");
                            _startJump = false;
                            _moveDirection.x = -xBackflipSpeed;
                            _moveDirection.y = yBackflipSpeed;
                            isJumping = true;

                            charactercontroller.DisableGroundCheck();

                        }
                    }









                }






                else if (!isFacingRight)
                {
                    if (_input.x > 0)
                    {
                        if (!isBackflipping)
                        {
                            _moveDirection.x = walkSpeed * 0.6f;
                            anim.SetFloat("defenseWalkSpeed", -1);
                            anim.SetBool("isWalkingDefense", true);
                            anim.SetBool("isIdleDefense", false);
                        }

                        //Backflip
                        if (_startJump)
                        {
                            isBackflipping = true;
                            anim.SetBool("isBackflip", true);
                            // StartCoroutine("Flash");
                            _startJump = false;
                            _moveDirection.x = xBackflipSpeed;
                            _moveDirection.y = yBackflipSpeed;
                            isJumping = true;

                            charactercontroller.DisableGroundCheck();

                        }


                    }
                    if (_input.x < 0)
                    {
                        if (!isBackflipping)
                        {
                            _moveDirection.x = -walkSpeed * 0.6f;
                            anim.SetFloat("defenseWalkSpeed", 1);
                            anim.SetBool("isWalkingDefense", true);
                            anim.SetBool("isIdleDefense", false);
                        }




                    }
                }







            }
            else
            {
                anim.SetFloat("defenseWalkSpeed", 1);
                anim.SetBool("isIdleDefense", false);
                anim.SetBool("isWalkingDefense", false);
            }







        }










        //GROUNDED
        if (charactercontroller.below && knockbackCount <= 0)
        {
            isGrounded = true;
            _moveDirection.y = 0f; //reset 0

            //Clear flags for in the air
            isJumping = false;
            isDoubleJumping = false;
            isWallJumping = false;
            isWallSliding = false;
            isBackflipping = false;


            anim.SetBool("isGrounded", true);
            //anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJumping", false);
            anim.SetBool("isWallSliding", false);
            anim.SetBool("isBackflip", false);





            if (_moveDirection.x != 0)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }



            //NORMAL GROUND JUMP
            if (_startJump && !isDashing && !isBackflipping)
            {



                // StartCoroutine("Flash");
                _startJump = false;
               rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                isJumping = true;

                charactercontroller.DisableGroundCheck();


            }



        }
        // PLAYER IS IN THE AIR
        else
        {
            isGrounded = false;
            anim.SetBool("isGrounded", false);
            anim.SetBool("isRunning", false);


            //short & long jump
            if (_releaseJump && !isWallJumping)
            {
                _releaseJump = false;

                if (_moveDirection.y > 0)
                {
                    _moveDirection.y *= 0.5f;
                }


            }


            //pressed jump button in air
            if (_startJump && !isDashing)
            {

                //Double Jump
                if (!charactercontroller.left && !charactercontroller.right)
                {
                    if (!isDoubleJumping)
                    {
                        _moveDirection.y = doubleJumpSpeed;
                        isDoubleJumping = true;
                        //anim.SetBool("isDoubleJumping", true);

                    }
                }



                //WALL JUMP
                if (canWallJump && charactercontroller.left || charactercontroller.right)
                {
                    anim.SetBool("isBackflip", false);

                    if (charactercontroller.left)
                    {
                        _moveDirection.x = xWallJumpSpeed;
                        _moveDirection.y = yWallJumpSpeed;
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                    }
                    else if (charactercontroller.right)
                    {
                        _moveDirection.x = -xWallJumpSpeed;
                        _moveDirection.y = yWallJumpSpeed;
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);

                    }

                    isWallJumping = true;

                    StartCoroutine("WallJumpWaiter");

                    isDoubleJumping = false;

                }

                _startJump = false;
            }


            //WALL SLIDING
            if (charactercontroller.left || charactercontroller.right)
            {


                // anim.SetTrigger("wallHitTrigger");
                anim.SetBool("isWallSliding", true);

                if (charactercontroller.right && isWallSliding)
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else if (charactercontroller.left && isWallSliding)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }




            }
            else
            {
                anim.SetBool("isWallSliding", false);

            }




            GravityCalculations();


        }


        charactercontroller.Move(_moveDirection * Time.deltaTime);


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
                anim.SetTrigger("wallHitTrigger");
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }



    #region INPUT METHODS

    //look at coding out cancel movements such as dash and attacks using input methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
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


    #endregion



    #region COURTINES

    IEnumerator WallJumpWaiter()
    {
        isWallJumping = true;
        anim.SetBool("isWallJumping", true);

        yield return new WaitForSeconds(wallJumpWaiter);

        isWallJumping = false;
        anim.SetBool("isWallJumping", false);

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

        isKnockedBack = true;



        yield return new WaitForSecondsRealtime(invincibleTime);


        isInvincible = false;

        isKnockedBack = false;
    }




    #endregion

    */
}
