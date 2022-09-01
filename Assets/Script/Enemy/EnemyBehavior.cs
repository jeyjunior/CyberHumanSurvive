using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    [SerializeField] Transform player;
    [SerializeField] Rigidbody rb;
    [SerializeField] EnemyAttributes enemyAttributes;
    [SerializeField] Animator anim;

    public float moveSpeed;
    public float rotateSpeed; 

    public float rangeToDetect;
    public bool playerDetected; //Detectou player, começar a andar em direção ao player
    
    public bool rangeToAttacking; //Define range do atk
    public bool isAttacking; //Inimigo ataca
    bool dmgOnPlayerCheck; //Quando realizar o atk o player estiver na Range o mesmo leva dano

    bool isRunning;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        enemyAttributes = GetComponent<EnemyAttributes>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!enemyAttributes.isDead)
        {
           
            DetectPlayer();

            if (playerDetected && !rangeToAttacking)
            {
                Move();
            }
            else if(playerDetected && rangeToAttacking)
            {
                LookToPlayer();
                isRunning = false;
            }

            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isRunning", isRunning);
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
            isRunning = false;
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
        isRunning = true;

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


    //Detectar se o player esta dentro ou fora da area de ataque
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rangeToAttacking = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rangeToAttacking = false;
        }
    }
    //Verificar se o player esta na area de dano quando o atk atingiu o player
    private void OnTriggerStay(Collider other)
    {

        if(!isAttacking && other.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }


        if (dmgOnPlayerCheck && other.gameObject.CompareTag("Player")){
            Debug.Log("Colisão detectada!");
            dmgOnPlayerCheck = false;
        }
    }
    //Events nas animações
    public void CheckDmgOnPlayer(int num)
    {
        dmgOnPlayerCheck = true;
        Debug.Log($"Dano no player: {num}");
    }
    public void EndIsAttacking()
    {
        isAttacking = false;
    }


}

