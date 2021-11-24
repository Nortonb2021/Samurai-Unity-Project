using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//public class PlayerManager : MonoBehaviour
//{

    /*
    [SerializeField]
    internal PlayerMovementScript movementScript;

    [SerializeField]
    internal PlayerCollisionScript collisionScript;








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





    public Vector2 dir;
    private Vector2 moveDirection;


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>





    //private variables
    private Rigidbody2D rb;
    private Collision collision;
    private CapsuleCollider2D cc;


    private Vector2 colliderSize;



    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

      








    }





    #region INPUT METHODS

    //look at coding out cancel movements such as dash and attacks using input methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
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












}
    */