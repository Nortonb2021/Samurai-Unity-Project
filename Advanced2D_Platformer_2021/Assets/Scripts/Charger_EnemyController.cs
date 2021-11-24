using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Charger_EnemyController : MonoBehaviour
{


    [Header("CHILD SCRIPTS")]

    [SerializeField]
    internal CharacterController2D charactercontroller;


    #region PUBLIC PROPERTIES

    [Header("ENEMY HEALTH")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("ENEMY PROPERTIES")]
    public float gravity = 20f;
    public float walkSpeed = 10f;
    public float runSpeed = 10f;
    public float agroRange;

    public float verticalKnockbackForce;
    public float horizontalKnockbackForce;
    public float knockbackLength;
    public float knockbackCount;


    [Header("ENEMY STATES")]
    public bool isIdle;
    public bool isChasing;
    public bool isDead;
    public bool isGrounded;
    public bool isFacingRight = false;


    public bool isKnockbackFromRight;
    public bool isKnockbackFromLeft;

    
    public LayerMask Player;

    #endregion




    #region PRIVATE PROPERTIES
    //INPUT FLAGS

    private Vector2 _moveDirection;
    public Animator anim;


    #endregion


    private enum State { idle, walk, run, attack, knockback, hurt, dead }
    private State state = State.idle;



    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }





    void Update()
    {

        //Flip
        if (!isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }






        //////////-----[STATE MACHINE BEHAVIOR]-----\\\\\\\\\\


        //Idle State = 0
        if (isIdle)
        {
            state = State.idle;
        }



        //Walk State = 1
        if (charactercontroller.below && !isChasing)
        {
            state = State.run;



            //move left
            if (isFacingRight)
            {
                _moveDirection.x = walkSpeed;
            }
            //move Right
            else if (!isFacingRight)
            {
                _moveDirection.x = -walkSpeed;
            }

            //flip on collision
            if (charactercontroller.left || charactercontroller.right)
            {
                if (charactercontroller.left)
                {
                    isFacingRight = true;
                    _moveDirection.x = walkSpeed;
                }
                else if (charactercontroller.right)
                {
                    isFacingRight = false;
                    _moveDirection.x = -walkSpeed;
                }
            }





            _moveDirection.y = 0f; //reset 0
        }



        //Run State = 2 
        if (charactercontroller.below)
        {

        }




        //Knockback State = 3
        if (knockbackCount > 0)
        {
            state = State.knockback;

            if (isKnockbackFromRight)
            {
                _moveDirection.x = -horizontalKnockbackForce;
                _moveDirection.y = verticalKnockbackForce;
                //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (!isKnockbackFromRight)
            {
                _moveDirection.x = horizontalKnockbackForce;
                _moveDirection.y = verticalKnockbackForce;
                //transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }


            knockbackCount -= Time.deltaTime;

        }


        //Dead State = 4 
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }




        //////////-----[STATE MACHINE BEHAVIOR]-----\\\\\\\\\\








        //GROUNDED LOGIC
        if (charactercontroller.below)
        {
            _moveDirection.y = 0f; //reset 0


            //Clear Flags




        }
        //IN AIR LOGIC
        else
        {
            isGrounded = false;

            anim.SetBool("isGrounded", false);

            GravityCalculations();

        }




        StateSwitch();

        charactercontroller.Move(_moveDirection * Time.deltaTime);

        //anim.SetInteger("state", (int)state);

    }




    private void StateSwitch()
    {

        if (state == State.run)
        {

        }

    }



    //GRAVITY
    void GravityCalculations()
    {

        //Disable Gravity
        if (_moveDirection.y > 0f && charactercontroller.above)
        {
            _moveDirection.y = 0f;
        }


        _moveDirection.y -= gravity * Time.deltaTime;


    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // play hurt animation


        if (currentHealth <= 0)
        {
            //die
        }

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
