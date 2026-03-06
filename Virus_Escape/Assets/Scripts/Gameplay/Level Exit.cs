using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] Collider gateCollider;

    [SerializeField] Transform gateMesh;

    [SerializeField] string targetScene;

    [SerializeField] bool open = false;

    private void Start()
    {
        gateCollider.enabled = false;
        Debug.Log("gateClosed");
    }
    public void OpenGate()
    {
        open = true;

        gateCollider.enabled = true;

        Debug.Log("opend");

    }

    public void exitLevel()
    {
        SceneManager.LoadScene(targetScene);
    }
    private void Update()
    {
        if (open)
        {
            Vector3 targetPosition = new Vector3(0f, -2.1f, 0f);

           
            gateMesh.localPosition = Vector3.Lerp(gateMesh.localPosition, targetPosition, 1f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exitLevel();
        }
    }
}
