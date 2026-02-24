using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    public Vector2 moveInput;

    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private float turnSpeed;

    [SerializeField] private float verticalVelocity = 0;

    [SerializeField] private int gravityScale;

    public bool Grounded => characterController.isGrounded;




    private void Update()
    {
        UpdateMovement();

        
    }

    #region
    //movement
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();  
    }

   

    #endregion


    void UpdateMovement()
    {
        Vector3 moveInput3D = new Vector3(moveInput.x, 0f , moveInput.y);

        Vector3 motion = moveInput3D * moveSpeed * Time.deltaTime;

        Gravity();
       
        motion.y = verticalVelocity * Time.deltaTime;

        characterController.Move(motion);

        UpdatePlayerRotation(moveInput3D);
    }

    public void Gravity()
    {
        if (Grounded)
        {
            verticalVelocity = -3f;

        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
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


