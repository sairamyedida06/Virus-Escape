using UnityEngine;

public class Syringe_Animatin : MonoBehaviour
{
    [SerializeField] Transform SyringeMesh;
    [SerializeField] float rotateSpeed;

    private void Update()
    {
        SyringeMesh.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
