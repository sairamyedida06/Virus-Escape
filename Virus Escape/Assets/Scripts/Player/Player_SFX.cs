using UnityEngine;

public class Player_SFX : MonoBehaviour
{
    [Header("Audiosource")]
    [SerializeField] private AudioSource audioSource;

    [Header("AudioClips")]
    [SerializeField] AudioClip jumpSFX;
    



    public void OnJumped()
    {
        audioSource.PlayOneShot(jumpSFX);
    }

    

}
