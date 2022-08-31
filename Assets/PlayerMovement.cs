using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    Rigidbody rb;
    public Camera cam;

    [Header("Move Control")]
    Vector3 moveDirection;
    public float moveSpeed;

    [Header("States Control")]
    public bool isRunning;
    public bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        InputMove();


        //Anims
        AnimationControl();
    }

    private void FixedUpdate()
    {
        Move();
        RotateCharacter();

    }

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
        if(moveDirection != Vector3.zero) isRunning = true;
        else isRunning = false;
    }
    void Move()

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

    void AnimationControl()
    {
        anim.SetBool("isRunning", isRunning);
    }



}
