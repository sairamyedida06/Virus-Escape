using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public CharacterController characterController;
    [SerializeField] PlayerStamina playerStamina;

    public Vector2 moveInput;

    [SerializeField] float speed;

    private bool sprintInput;

    [SerializeField] float sprintSpeed;
  

    [SerializeField] private float turnSpeed;

    [SerializeField] public float verticalVelocity = 0;

    [SerializeField] private float gravityScale;

    [SerializeField] private bool jumpInput;

    [SerializeField] private float jumpHeight;

    public bool InputHandling = true;

    bool wasGrounded;

    public bool Grounded => characterController.isGrounded;

    private float MoveSpeed => sprintInput ? sprintSpeed : speed;

    [Space(10)]
    [SerializeField] private float SprintStaminaCost;
 


    public UnityEvent jumped;
    public UnityEvent Landed;


    private void Update()
    {
        SprintStamina();
        UpdateMovement();
        
    }

    public void SprintStamina()
    {
        
        
            if (sprintInput == true)
            {
                float staminaConsumed = SprintStaminaCost * Time.deltaTime;

                if (playerStamina.HasEnoughStamina(staminaConsumed))
                {
                    playerStamina.ConsumeStamina(staminaConsumed);
                }
                else
                {
                    sprintInput = false;
                }
            }
        
    }

    #region
    //movement
    public void OnMove(InputAction.CallbackContext context)
    {
        if (InputHandling == false)
        {
            moveInput = Vector2.zero;
            return;
        }
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (InputHandling == false)
        {
            jumpInput = false;
            return;
        }

        if (context.performed)
        {
            jumpInput = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            sprintInput = true;

        }
        else if(context.canceled)
        {
            sprintInput= false;
        }
    }


    #endregion


    void UpdateMovement()
    {

        Vector3 moveInput3D = new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 motion = moveInput3D * MoveSpeed * Time.deltaTime;

        Gravity();
 
        UpdateJump();    

        motion.y = verticalVelocity * Time.deltaTime;

        characterController.Move(motion);

        UpdatePlayerRotation(moveInput3D);

        OnLand();

    }

    public void UpdateJump()
    {
        if (jumpInput && Grounded)
        {
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(Physics.gravity.y * gravityScale));

            jumpInput = false;

            jumped.Invoke();
        }
    }
    public void Gravity()
    {
        if (Grounded && verticalVelocity < 0)
        {
            verticalVelocity = -3f;

        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }


    public void OnLand()
    {
        if(!wasGrounded && Grounded)
        {
            Landed.Invoke();

            VFX_Manager.Instance.PlayLandVFX(transform.position);
            
        }
        wasGrounded = Grounded;
    }

    //Rotation
    void UpdatePlayerRotation(Vector3 moveInput)
    {
        if (moveInput.sqrMagnitude <= 0.01f)
        {
            return;
        }


        Vector3 playerRotation = transform.rotation.eulerAngles;

        playerRotation.y = GetAngleFromVector(moveInput);

        Quaternion targetRotation = Quaternion.Euler(playerRotation);

        float maxDegrees = turnSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegrees);

    }
    float GetAngleFromVector(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        return rotation.eulerAngles.y;

    }


}


