using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    Rigidbody rb;
    public Camera cam;
    public LayerMask groundMask;
    GameObject gameController;

    [Header("Move Control")]
    Vector3 moveDirection;
    public float moveSpeed;
    public float rotationSpeed;

    [Header("States Control")]
    public bool isRunning;
    public bool horizontalMove, verticalMove;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindWithTag("GameController");
    }
    void Update()
    {
        if (!gameController.GetComponent<GUIController>().panelsIsActive)
        {
            InputMove();
            AnimationControl();
        }
    }
    private void FixedUpdate()
    {
        if (!gameController.GetComponent<GUIController>().panelsIsActive)
        {
            if(GetComponent<PlayerShooting>().isShooting)
            {
                RotateCharacter();
                //RotateCharacterB();
                isRunning = false;
            }
            else if(! GetComponent<PlayerShooting>().isShooting) MoveModel();
        }

    }

    #region LookAtkAngle
    void RotateCharacter()
    {
        Vector2 positionOnScreen = cam.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
    }
    float AngleBetweenTwoPoints(Vector3 b, Vector3 a)
    {
        return Mathf.Atan2(a.x - b.y, a.y - b.x) * Mathf.Rad2Deg;
        //creditos: answers.unity.com/questions/855976/make-a-player-model-rotate-towards-mouse-location.html
    }
    void InputMove()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //moveDirection.Normalize();
    }
    #endregion

    void MoveModel()
    {
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        //alternative: transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime, Space.World);

        if(moveDirection != Vector3.zero)
        {
            isRunning = true;

            Quaternion rot = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.fixedDeltaTime);

            //transform.rotation = Quaternion.Euler(new Vector3(0f, rot.y, 0f));
        }
        else
        {
            isRunning = false;
            RotateCharacter();
        }
    }
    void AnimationControl()
    {
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isShooting", GetComponent<PlayerShooting>().isShooting);
    }


    //Iteração com pilar magico
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MagicStone"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.gameObject.GetComponent<MagicStone>().ActiveMagicStone();
            }
        }
    }


    /*
         void RotateCharacterB()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.yellow);

        RaycastHit impact;

        if (Physics.Raycast(ray, out impact, 100, groundMask))
        {
            Vector3 playerLookPosition = impact.point - transform.position;
            playerLookPosition.y = transform.position.y;

            Quaternion newRot = Quaternion.LookRotation(playerLookPosition);

            rb.MoveRotation(newRot);
        }
    }

        void MoveModelB()

    {
        //rb.MovePosition(rb.position + (moveDirection * moveSpeed * Time.deltaTime));
        Vector3 mDir = transform.forward * moveDirection.z + transform.right * moveDirection.x;
        rb.AddForce(mDir.normalized * moveSpeed * 3f, ForceMode.Force);

        SpeedControl();
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
     */
}
