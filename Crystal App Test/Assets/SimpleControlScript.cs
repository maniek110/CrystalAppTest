using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControlScript : MonoBehaviour
{
    public Camera playerCamera;
    private float moveSpeed = 0.1f;
    public float scrollSpeed = 100f;
    Rigidbody myRigidbody;
    float horizontal, vertical;
    Animator animator;
    bool isGround;
    public int buttonVal = 0;
    public int height = 0;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("vertical", vertical);
        animator.SetBool("isGround", isGround);
        animator.SetFloat("horizontal", horizontal);
        SpaceMovement();
        CameraRotation();
        //Debug.Log(vertical);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGround = true;
    }
    void SpaceMovement()
    {
        if (horizontal != 0 || vertical != 0)
        {
            transform.position += moveSpeed * (Input.GetAxisRaw("Horizontal") * transform.right + transform.forward * Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetKey(KeyCode.Space)&&isGround)
        {
            animator.Play("Jump");
            isGround = false;
            myRigidbody.AddForce(Vector3.up*scrollSpeed,ForceMode.Impulse);
        }
    }

    void CameraRotation()
    {
        Vector2 posOnScreen = Camera.main.WorldToScreenPoint(playerCamera.transform.position);
        Vector2 mousePosOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(posOnScreen.x = mousePosOnScreen.x, posOnScreen.y = mousePosOnScreen.y) * Mathf.Rad2Deg;
        Vector3 eulers = playerCamera.transform.eulerAngles;
        eulers.z = 0;
        playerCamera.transform.eulerAngles = eulers;
        playerCamera.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -10, 0, 0));
        // Debug.Log(Input.GetAxis("Mouse Y") * -10);
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 10, 0));
    }
}
