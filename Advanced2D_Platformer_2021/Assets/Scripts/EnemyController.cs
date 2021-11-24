using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{


    [Header("CHILD SCRIPTS")]

    [SerializeField]
    internal CharacterController2D charactercontroller;

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform castPoint;

    
    public static EnemyController instance;

    #region PUBLIC PROPERTIES

    [Header("ENEMY HEALTH")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("ENEMY PROPERTIES")]
    public float gravity = 20f;
    public float walkSpeed = 10f;
    public float chaseSpeed = 10f;

    public float coolDownTimer = 1.5f;

    public float agroRange;
    public float attackRange;

    public float verticalKnockbackForce;
    public float horizontalKnockbackForce;
    public float knockbackLength;
    public float knockbackCount;


    [Header("ENEMY STATES")]
    public bool isWalking;
    public bool isChasing;
    public bool isInRange;
    public bool attackMode;
    public bool isCooling;
    public bool isDead;
    public bool isFacingRight = false;
    public bool playerDetected;
    public bool isAttacking;


    public bool isKnockbackFromRight;
    public bool isKnockbackFromLeft;



    public LayerMask Player;
    #endregion




    #region PRIVATE PROPERTIES
    //INPUT FLAGS





    private Vector2 _moveDirection;
    public Animator anim;

  


    #endregion


    private enum State { idle, walk, chase, attack, cooldown }
    private State state = State.walk;
    private bool isAgro = false;



    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();

    }





    void Update()
    {

        /*
        //Distance to Player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        print("distToPlayer;" + distToPlayer);
        */





        //Flip
        if (!isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }






        //////////-----[STATE MACHINE BEHAVIOR]-----\\\\\\\\\\


        //Idle State = 0
        if(isCooling)
        {
            state = State.idle;

        }

      


        //Walk State = 1
        if (!isChasing && !attackMode && !isCooling && !isAttacking)
        {

            state = State.walk;

            isWalking = true;


            if (isFacingRight)
            {
                _moveDirection.x = walkSpeed;
            }
            else if (!isFacingRight)
            {
                _moveDirection.x = -walkSpeed;
            }
           


            
            if (charactercontroller.right)
            {
                isFacingRight = false;
            }
            else if (charactercontroller.left)
            {
                isFacingRight = true;

            }
            


            _moveDirection.y = 0f; //reset 0


        }
       
      


   




        //Chase State = 2
        if (isInRange && !attackMode && !isCooling && !isAttacking)
        {
            state = State.chase;
            isAgro = true;

          //move right
            if (transform.position.x <= player.position.x)
            {
                _moveDirection.x = chaseSpeed;
                isFacingRight = true;
            }
            //move left
            else if (transform.position.x > player.position.x)
            {
                _moveDirection.x = -chaseSpeed;
                isFacingRight = false;
            }



            _moveDirection.y = 0f; //reset 0
        }




        //Attack = 3
        if (attackMode && !isCooling)
        {
            state = State.attack;

            _moveDirection.x = 0;
            
        }





        //Hurt State = 6





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
            

           

            GravityCalculations();

        }


       

        StateSwitch();

        charactercontroller.Move(_moveDirection * Time.deltaTime);

        anim.SetInteger("state", (int)state);

    }



    /*
    //Raycast for player detection
    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;


        if (!isFacingRight)
        {
            castDist = -distance;
        }


        Vector2 endpos = castPoint.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endpos, 1 << LayerMask.NameToLayer("PlayerLayer"));


        if(hit.collider != null)
        {
            isChasing = true;
            isAgro = true;

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }

            Debug.DrawLine(castPoint.position, hit.point, Color.green);

        }
        else
        {
            
            isChasing = false;

            if (isAgro)
            {
                //Invoke()
            }

            Debug.DrawLine(castPoint.position, endpos, Color.red);
        }

        return val;

    }
    */



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


        _moveDirection.y -= gravity * Time.deltaTime;


    }



    public void TakeDamage (int damage)
    {
        currentHealth -= damage;

        // play hurt animation


        if(currentHealth <= 0)
        {
            //die
        }

    }

   


    IEnumerator CoolDown()
    {
        isCooling = true;

        _moveDirection.x = 0;

        yield return new WaitForSeconds(coolDownTimer);

        isCooling = false;

    }
    
  

}
