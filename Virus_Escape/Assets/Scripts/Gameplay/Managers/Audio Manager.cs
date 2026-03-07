using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            sfxSource = GetComponent<AudioSource>();
            
            
        }
        else
        {
            sfxSource = GetComponent<AudioSource>();
            Destroy(gameObject);
            
        }
    }

    public void JumpAudio(AudioClip audioClip)
    {
        if(audioClip != null)
        {
            sfxSource.PlayOneShot(audioClip);
        }
    }

    public void CoinSFX(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            sfxSource.PlayOneShot(audioClip);
        }
    }
}
