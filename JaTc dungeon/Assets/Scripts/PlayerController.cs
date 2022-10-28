using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private CharacterController characterController;
    [Range (0, 2f)][SerializeField] private float moveSpeed = 1.5f;
    private float moveHorizontal;
    private float moveVertical;
    private Vector3 playerInput;
    private Vector3 playerMove;
    [SerializeField] private float playerGravity = 20f;
    private float fallVelocity;
    [Range(1, 4f)][SerializeField] private float jumpForce = 3f;
    [SerializeField] private Animator playerAnimation;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        playerInput = new Vector3(moveHorizontal, 0f, moveVertical);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        playerAnimation.SetFloat("playerVelocity", playerInput.magnitude * moveSpeed);

        CameraDirection();

        playerMove = playerInput.x * cameraRight + playerInput.z * cameraForward;
        playerMove *= moveSpeed;

        characterController.transform.LookAt(characterController.transform.position + playerMove);

        SetGravityPlayer();

        PlayerSkills();

        //Moviment
        characterController.Move(moveSpeed * Time.deltaTime * playerMove);

        Debug.Log(characterController.velocity.magnitude);
    }

    //Function to know where the player is looking
    public void CameraDirection()
    {
        cameraForward = mainCamera.transform.forward;
        cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }

    //Function to get the falling speed
    public void SetGravityPlayer()
    {
        if (characterController.isGrounded)
        {
            fallVelocity = -playerGravity * Time.deltaTime;
            playerMove.y = fallVelocity;
        }
        else
        {
            fallVelocity -= playerGravity * Time.deltaTime;
            playerMove.y = fallVelocity;
            playerAnimation.SetFloat("playerVelocityVertical", characterController.velocity.y);
        }
        playerAnimation.SetBool("isGrounded", characterController.isGrounded);
    }

    //Function for player skills
    public void PlayerSkills()
    {
        //Jump
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            playerMove.y = fallVelocity;
            playerAnimation.SetTrigger("playerJump");
        }
    }
}