using UnityEngine;

public class Collectable_animation : MonoBehaviour
{
    [SerializeField] float AngularSpeed = 50f;

    [SerializeField] float CoinHeight = 0.7f;

    [SerializeField] float MovementAmplitude = 0.5f;

    [SerializeField] float MovementFrequency = 1f;

    [SerializeField] Transform CoinMesh;

    [SerializeField] Collider Mesh;


    private void Update() 
    {

        CoinMesh.Rotate(0f, AngularSpeed * Time.deltaTime, 0f);

        float DeltaY = MovementAmplitude * Mathf.Sin(MovementFrequency * Time.time);

        CoinMesh.localPosition = new Vector3(CoinMesh.localPosition.x, CoinHeight + DeltaY, CoinMesh.localPosition.z);

    }
}
