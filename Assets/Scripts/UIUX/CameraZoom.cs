using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public GameObject[] players;  
    public float zoomSpeed = 5f;  
    public float slowMoFactor = 0.1f;  
    public float zoomAmount = 10f;  
    public float duration = .5f;  

    private bool isDead = false;
    private float originalFOV;
    private Vector3 originalPosition;
    private float timer = 0f;
    private Vector3 deadPlayer;

    void Start()
    {
        originalFOV = Camera.main.fieldOfView;
        originalPosition = Camera.main.transform.position;
    }

    void Update()
    {
        if (isDead)
        {
            timer += Time.deltaTime;
            if (timer <= duration)
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originalFOV - zoomAmount, Time.deltaTime * zoomSpeed);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, deadPlayer, Time.deltaTime * zoomSpeed);
            }
            else
            {
                Time.timeScale = 1f;  
                isDead = false;  
                Camera.main.fieldOfView = originalFOV;
                Camera.main.transform.position = originalPosition;
                timer = 0f;
            }
        }
    }

    public void PlayerDied(Vector3 position)
    {
        deadPlayer = position + new Vector3(0, 0, -10);
        isDead = true;
        timer = 0f;
        Time.timeScale = slowMoFactor; 
    }
}
