using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    [Header("Player")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;


    [SerializeField] int gravityScale;
    float verticalVelocity;
    public bool Grounded => characterController.isGrounded;

    [SerializeField] private float jumpHeight;
    private bool jumpInput;

    bool wasGrounded;

    public UnityEvent Jumped;
    public UnityEvent Landed;



    [SerializeField]  CharacterController characterController;
    Vector2 moveInput;

    [SerializeField] Animator animator;

    

    #region Unity Callbacks

    private void Update()
    {
        UpdateMovement();
        UpdateAnimation();
    }
    #endregion


    #region Input Handling
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // jump

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
    }
    #endregion


    #region UpdateMovement
    void UpdateMovement()
    {
        Vector3 moveInput3D = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 motion = moveInput3D * moveSpeed * Time.deltaTime;

        UpdatePlayerRotation(moveInput3D);

        
        if (Grounded)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }

        if(jumpInput && Grounded)
        {
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y * gravityScale));
            jumpInput = false;

            Jumped.Invoke();
        }

        if(!wasGrounded && Grounded )
        {
            Landed.Invoke();
            Debug.Log("landed");
        }
        wasGrounded = Grounded;
 

        motion.y = verticalVelocity * Time.deltaTime;

        characterController.Move(motion);
       



    }
    #endregion

    #region Rotation
    void UpdatePlayerRotation(Vector3 moveInput)
    {
        if (moveInput.sqrMagnitude <= 0.01f)
        {
            return;
        }

        Vector3 playerRoatation = transform.rotation.eulerAngles;

        playerRoatation.y = GetAngleFromVector(moveInput);

        Quaternion targetRoatation = Quaternion.Euler(playerRoatation);

        float maxDegrees = turnSpeed * Time.deltaTime; 
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRoatation, maxDegrees);
    }

    float GetAngleFromVector(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


        return rotation.eulerAngles.y;
    }
    #endregion

    #region Animation
    void UpdateAnimation()
    {
        bool jump = false;
        bool fall = false;

        if (Grounded)
        {
            jump = false;
            fall = false;
        }
        else
        {
            if(verticalVelocity >= 0)
            {
                jump = true;
            }
            else
            {
                fall = true;
            }

        }

        Vector3 velocity = characterController.velocity;
        velocity.y = 0;
        float speed = velocity.magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("Jump", jump);
        animator.SetBool("Fall", fall);

    }
    #endregion

}
