using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private EnemyAttributes enemyAttributes;
    [SerializeField] private Animator anim;

    [Space(5)]
    public GameObject[] vfx;

    [Space(5)]
    public Transform hitPosition;

    [Space(5)]
    public float moveSpeed;
    public float rotateSpeed;
    public float speedToSpawn;

    [SerializeField] private float rangeToDetect;
    [SerializeField] private float rangeToAttacking; //Define range do atk
    [SerializeField] private float distance;

    [SerializeField] private bool attackPlayer;
    [SerializeField] private bool isRunning;

    public float timeToAtk = 0;
    public float delayToRestartAtk = 1;

    public bool readyToFight;

    bool hitPlayer;
    public int power = 30;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        enemyAttributes = GetComponent<EnemyAttributes>();
        anim = GetComponent<Animator>();

        speedToSpawn = Random.Range(1f, 3f);
    }

    private void FixedUpdate()
    {
        if(!readyToFight)
        {
            rb.MovePosition(rb.position + Vector3.up * speedToSpawn * Time.fixedDeltaTime);
            GetComponent<BoxCollider>().enabled = false;

            if (transform.position.y >= -0.689f)
            {
                readyToFight = true;
                GetComponent<BoxCollider>().enabled = true;
            }
        }
        if (readyToFight)
        {
            distance = (player.transform.position - transform.position).sqrMagnitude;

            if (!enemyAttributes.isDead)
            {
                if(distance < rangeToDetect && distance > rangeToAttacking)
                {
                    Move();
                    isRunning = true;
                }
                else if(distance < rangeToDetect && distance < rangeToAttacking)
                {
                    isRunning = false;

                    if(timeToAtk < delayToRestartAtk && !attackPlayer)
                    {
                        timeToAtk += Time.deltaTime;

                    }
                    else if(timeToAtk > delayToRestartAtk && !attackPlayer)
                    {
                        //Executa uma vez o ataque antes dele reiniciar
                    
                        attackPlayer = true;
                        timeToAtk = 0;
                    }
                }
                else
                {
                    isRunning = false;
                    attackPlayer = false;
                }

                anim.SetBool("isRunning", isRunning);
                anim.SetBool("isAttacking", attackPlayer);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hitPlayer)
        {
            Instantiate(vfx[0], hitPosition.transform.position, hitPosition.transform.rotation);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().TakeDmg(power);
            hitPlayer = false;
        }
    }

    //Events nas animações
    public void CheckDmgOnPlayer(int num)
    {
        hitPlayer = true;
    }

    public void EndAttack()
    {
        attackPlayer = false;
    }

    #region Move Control
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
    #endregion



}

