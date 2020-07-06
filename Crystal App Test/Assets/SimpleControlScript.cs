using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleControlScript : MonoBehaviour
{
    public Text timeUI;
    public Text dead;
    public Text deathUI;
    public Camera camera;
    public Transform RespawnPoint;
    public float moveSpeed = 0.1f;
    public float JumpPow = 100f;
    float SmoothAnglStep=0.1f;
    float SmoothAnglVelocity;
    Rigidbody myRigidbody;
    GameObject myGameObject;
    float horizontal, vertical;
    public bool isAlive = true;
    float time = 0;
    int death;
    bool isGround;
    // Start is called before the first frame update
    void Start()
    {
        myGameObject = GetComponent<GameObject>();
        myRigidbody = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        DisplayValues();
        if (isAlive)
        {
            CheckIfAlive();
            CountTime();
            Movement();   
        }
        else {
            TemporaryReset();  
        }
    }

    private void TemporaryReset()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            isAlive = true;
            dead.enabled = false;
            time = 0;
            transform.position = RespawnPoint.position;
        } 
    }

    private void CheckIfAlive()
    {

        if (myRigidbody.velocity.y < -10 && isGround) {
            isAlive = false;
            dead.enabled = true;
            death++;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.down) > 0.3) isGround = false;
        }
        if (collision.gameObject.tag == "Trap") {
            isAlive = false;
            dead.enabled = true;
            death++;
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
        float tempAngl = Mathf.SmoothDampAngle(transform.eulerAngles.y, camera.transform.eulerAngles.y, ref SmoothAnglVelocity, SmoothAnglStep);
        transform.rotation = Quaternion.Euler(0, tempAngl,0);
        if (dir.magnitude >= 0.1f) {
            Vector3 MoveDir = Quaternion.Euler(0, camera.transform.eulerAngles.y,0)*dir;
            transform.Translate(MoveDir*moveSpeed*Time.deltaTime,Space.World);
        }
        if (Input.GetKey(KeyCode.Space)&&isGround)
        {
            isGround = false;
            myRigidbody.velocity+=Vector3.up*JumpPow*Time.deltaTime;
        }
    }
    void CountTime() {
        time += Time.deltaTime;
    }
    void DisplayValues() {
        timeUI.text = "TIME: " + time;
        deathUI.text = "DEATHS: " + death;

    }
}
