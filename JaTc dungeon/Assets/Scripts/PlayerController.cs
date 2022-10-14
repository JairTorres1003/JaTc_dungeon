using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float gravityScale = 10f;

    private Vector3 moveDirection;

    [SerializeField] private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float yStore = moveDirection.y;

        // Moviment
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = yStore;

        // Jump
        if (Input.GetButtonDown("Jump") && moveDirection.y < 1.1f)
        {
            moveDirection.y = jumpForce;
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}