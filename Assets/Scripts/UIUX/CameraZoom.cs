using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public GameObject player;  // Reference to the player
    public float zoomSpeed = 5f;  // Speed of the camera zoom
    public float slowMoFactor = 0.1f;  // Slow motion factor
    public float zoomAmount = 10f;  // How much to zoom in
    public float duration = 3f;  // Duration of the effect

    private bool isDead = false;
    private float originalFOV;
    private Vector3 originalPosition;
    private float timer = 0f;

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
            if (timer < duration)
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originalFOV - zoomAmount, Time.deltaTime * zoomSpeed);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, player.transform.position, Time.deltaTime * zoomSpeed);
            }
            else
            {
                Time.timeScale = 1f;  // Reset time scale to normal
                isDead = false;  // Reset the dead state
                Camera.main.fieldOfView = originalFOV;  // Reset the field of view
                Camera.main.transform.position = originalPosition;  // Reset the camera position
            }
        }
    }

    public void PlayerDied()
    {
        isDead = true;
        timer = 0f;
        Time.timeScale = slowMoFactor;  // Activate slow motion
    }
}
