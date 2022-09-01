using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    public Transform player;
    Rigidbody rb;
    public float moveSpeed;
    public float rotateSpeed; 

    public float rangeToDetect;
    public bool playerDetected; //Detectou player, começar a andar em direção ao player
    
    public bool attackRange; //Define range do atk
    public bool isAttacking; //Inimigo ataca
    bool dmgOnPlayerCheck; //Quando realizar o atk o player estiver na Range o mesmo leva dano

    public float atkDelay;

    //bool switchState;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {


    }

    private void FixedUpdate()
    {
        if (!GetComponent<EnemyAttributes>().die)
        {
           
            DetectPlayer();

            if (playerDetected && !attackRange)
            {
                Move();
                GetComponent<EnemyAttributes>().AnimSetBool("isRunning", true);
            }
            else if(playerDetected && attackRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine(EndAtkAnimation());
                    StartCoroutine(DmgCheck());
                }
                LookToPlayer();
                GetComponent<EnemyAttributes>().AnimSetBool("isRunning", false);
            }

            //if (! switchState) StartCoroutine(ChangeMoveAction());
        }
    }
    


    void DetectPlayer()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < rangeToDetect)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
            GetComponent<EnemyAttributes>().AnimSetBool("isRunning", false);
            //switchState = false;
        }
    }
    void LookToPlayer()
    {
        //transform.LookAt(player, Vector3.up * Time.deltaTime) ;

        //Rotação suavizada
        Quaternion rot = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
    }
    void Move()
    {
        LookToPlayer();
        SpeedControl();

        Vector3 direction = player.transform.position - transform.position;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
    void SpeedControl()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (vel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = vel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attackRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attackRange = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (dmgOnPlayerCheck && isAttacking && other.gameObject.CompareTag("Player")){
            dmgOnPlayerCheck = false;
            Debug.Log("Dano no player");
        }
    }
    IEnumerator EndAtkAnimation()
    {
        isAttacking = true;
        GetComponent<EnemyAttributes>().AnimSetBool("isAttacking", isAttacking);
        yield return new WaitForSeconds(atkDelay);
        isAttacking = false;
        GetComponent<EnemyAttributes>().AnimSetBool("isAttacking", isAttacking);
    }

    IEnumerator DmgCheck()
    {
        yield return new WaitForSeconds(atkDelay /2);
        dmgOnPlayerCheck = true;
    }

    /*
    IEnumerator ChangeMoveAction()
    {
        switchState = true;
        yield return new WaitForSeconds(Random.Range(3f, 10f));

        isMoving = (Random.value > 0.5f);
        switchState = false;

    }
    */

}

