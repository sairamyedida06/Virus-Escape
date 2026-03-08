using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("Player Sounds")]
    [SerializeField] AudioClip jumpSound;
    //[SerializeField] AudioClip landSound;

    public void PlayJumpSound()
    {
        AudioManager.Instance.PlaySFX(jumpSound);
    }

    //public void PlayLandSound()
    //{
    //    AudioManager.Instance.PlaySFX(landSound);
    //}
}