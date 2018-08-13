using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject weapon;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public CharacterController charC;
    
    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");

        if (charC.isGrounded)
        {
            moveDirection = new Vector3(inputH, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        charC.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            weapon.SetActive(true);
        }
    }
}
