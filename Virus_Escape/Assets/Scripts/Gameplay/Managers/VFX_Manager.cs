using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    public static VFX_Manager Instance;

    [Header("VFX Assets")]
    [SerializeField] private GameObject LandVfx;
    [SerializeField] private GameObject coinVFX;
    [SerializeField] private GameObject spikeTrapVFX;

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; DontDestroyOnLoad(gameObject); 
        }
        else
        { 
            Destroy(gameObject); 
        }
    }

    public void PlayLandVFX(Vector3 position)
    {
        if (LandVfx != null)
        {
            Instantiate(LandVfx, position, Quaternion.identity);
        }
            
    }

    public void PlayCoinVFX(Vector3 position)
    {
        if (coinVFX != null) 
        {
            Instantiate(coinVFX, position, Quaternion.identity);
        }

        
    }
    public void PlaySpikeVFX(Vector3 position)
    {
        if (spikeTrapVFX != null)
        {
            Instantiate(spikeTrapVFX, position, Quaternion.identity);
        }


    }
}