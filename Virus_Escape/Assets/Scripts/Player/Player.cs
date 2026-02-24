using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    Vector2 moveInput;

    [SerializeField] float moveSpeed = 1f;




    private void Update()
    {
        UpdateMovement();

        
    }


    //movement
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();  
    }

    void UpdateMovement()
    {
        Vector3 moveInput3D = new Vector3(moveInput.x, 0f , moveInput.y);

        Vector3 motion = moveInput3D * moveSpeed * Time.deltaTime;

        characterController.Move(motion);
    }

    //Rotation

    //Gravity

    //jump 

    //land detection


}
