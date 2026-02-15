using UnityEngine;

public class Player_VFX : MonoBehaviour
{
    [SerializeField] ParticleSystem landVFX;

    public void OnLandedVFX()
    {
        Instantiate(landVFX, transform.position,Quaternion.identity);

       
    }
}
