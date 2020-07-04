using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControlScript : MonoBehaviour
{
    public Camera camera;
    public float moveSpeed = 0.1f;
    public float JumpPow = 100f;
    float SmoothAnglStep=0.1f;
    float SmoothAnglVelocity;
    Rigidbody myRigidbody;
    GameObject myGameObject;
    float horizontal, vertical;
    //Animator animator;
    public bool isAlive = true;
    bool isGround;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        myGameObject = GetComponent<GameObject>();
        myRigidbody = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //CameraRotation();
        if (isAlive)
        {
            Movement();
            CheckIfAlive();
        }
        else {
            TemporaryReset();   
        }
        //Debug.Log(vertical);
        //Debug.Log(myRigidbody.velocity.y);
        Debug.Log(isGround);
        Debug.Log(isAlive);
    }

    private void TemporaryReset()
    {
        if (Input.GetKeyDown(KeyCode.R)) isAlive = true;
    }

    private void CheckIfAlive()
    {
        if (myRigidbody.velocity.y < -10 && isGround) {
            isAlive = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.down) > 0.5) isGround = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
    }
    void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Debug.Log(dir.x + "|" + dir.y + "|" + dir.z);
        if (dir.magnitude >= 0.1f) {
            float angl = Mathf.Atan2(dir.x,dir.z)*Mathf.Rad2Deg+camera.transform.eulerAngles.y;
            float tempAngl = Mathf.SmoothDampAngle(transform.eulerAngles.y, angl, ref SmoothAnglVelocity, SmoothAnglStep);
            transform.rotation = Quaternion.Euler(0,tempAngl,0);
            Vector3 MoveDir = Quaternion.Euler(0f, angl, 0f)*Vector3.forward;
            transform.Translate(MoveDir*moveSpeed*Time.deltaTime,Space.World);
        }
        if (Input.GetKey(KeyCode.Space)&&isGround)
        {
            isGround = false;
            myRigidbody.AddForce(Vector3.up*JumpPow,ForceMode.Impulse);
        }
    }
}
