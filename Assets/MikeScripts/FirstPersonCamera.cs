using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 5f; 
    public float mouseSensitivity = 2.0f;
    private float cameraVerticalRotation = 0.0f;  

    private bool isFocused = false; // To track application focus state

    // Start is called before the first frame update
    void Start()
    {
        //LockCursor(); 
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space)) {
            isFocused = !isFocused;

            if (isFocused) {
                LockCursor(); 
            } else {
                UnlockCursor(); 
            }
        }

        // get mouse input
        if (!isFocused) return; 
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity; 
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity; 

        // rotate vertically 
        cameraVerticalRotation -= inputY; 
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f); 
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation; 

        Vector3 moveDirection = Vector3.zero; 

        // rotate horizontally
        player.Rotate(Vector3.up * inputX); 

        if (Input.GetKey(KeyCode.W)) {
            moveDirection += player.forward; // Move forward
        }

        if (Input.GetKey(KeyCode.A)) {
            moveDirection -= player.right; // Move left
        }

        if (Input.GetKey(KeyCode.S)) {
            moveDirection -= player.forward; // Move backward
        }

        if (Input.GetKey(KeyCode.D)) {
            moveDirection += player.right; // Move right
        }

        if (Input.GetKey(KeyCode.E)) {
            moveDirection += player.up; // Move right
        }

        if (Input.GetKey(KeyCode.Q)) {
            moveDirection -= player.up; // Move right
        }

        if (moveDirection.magnitude > 0)
        {
            moveDirection.Normalize();
            player.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
