using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Space(5)]
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    float h;
    float v;
    Vector3 moveDireciton;
    Rigidbody rb;

    [Space(5)]
    [Header("GroundCheck")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask groundMask;
    [SerializeField] bool isOnGround;

    [Space(5)]
    [Header("Jump Control")]
    public float jumpStrength = 2;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    #region PlayerMove
    void MyInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }
    void MovePlayer()
    {
        moveDireciton = orientation.forward * v + orientation.right * h;
        rb.AddForce(moveDireciton.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    #endregion

    #region Jump
    void GroundCheck()
    {
        //0.5f metade do player + 0.2f abaixo do player
        isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, groundMask);

        //Player não deslizar
        if(isOnGround) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    void Jump()
        {
            if (Input.GetButtonDown("Jump") && isOnGround)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpStrength, 0);
            }
        }
    #endregion
}
