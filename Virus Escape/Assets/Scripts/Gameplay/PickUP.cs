using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] AudioClip collectedSFX;
    [SerializeField] AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(collectedSFX);
            Destroy(gameObject);
            Debug.Log("collected");

        }
    }
}
