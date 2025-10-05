using UnityEngine;

public class RobotInteraction : MonoBehaviour
{
    public GameObject interactUI;
    public float interactionDistance = 3f;
    public Transform player;
    public KeyCode interactKey = KeyCode.E;

    [Header("Interfaz del robot")]
    public GameObject robotUIPanel;
    public Transform cameraTarget;
    public float cameraMoveSpeed = 5f;

    private bool isPlayerNear = false;
    private bool isInteracting = false;

    private Camera playerCamera;
    private Vector3 originalCamPos;
    private Quaternion originalCamRot;
    private MonoBehaviour playerMovementScript;
    private MonoBehaviour playerLookScript;

    void Start()
    {
        playerCamera = Camera.main;

        // Busca automáticamente los scripts de movimiento y mirada del jugador
        playerMovementScript = player.GetComponent<MonoBehaviour>();
        playerLookScript = playerCamera.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!isInteracting && distance <= interactionDistance)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
                interactUI.SetActive(true);
            }

            if (Input.GetKeyDown(interactKey))
            {
                StartInteraction();
            }
        }
        else if (!isInteracting && isPlayerNear)
        {
            isPlayerNear = false;
            interactUI.SetActive(false);
        }
        else if (isInteracting && Input.GetKeyDown(interactKey))
        {
            ExitInteraction();
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        interactUI.SetActive(false);

        // Guardamos la posición original de la cámara
        originalCamPos = playerCamera.transform.position;
        originalCamRot = playerCamera.transform.rotation;

        // Bloquear controles del jugador
        TogglePlayerControls(false);

        // ✅ Mostrar el cursor para la interfaz
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(MoveCameraToRobot());
    }

    System.Collections.IEnumerator MoveCameraToRobot()
    {
        float t = 0;
        Vector3 startPos = playerCamera.transform.position;
        Quaternion startRot = playerCamera.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            playerCamera.transform.position = Vector3.Lerp(startPos, cameraTarget.position, t);
            playerCamera.transform.rotation = Quaternion.Lerp(startRot, cameraTarget.rotation, t);
            yield return null;
        }

        if (robotUIPanel != null)
            robotUIPanel.SetActive(true);
    }

    public void ExitInteraction()
    {
        StartCoroutine(ReturnCamera());
    }

    System.Collections.IEnumerator ReturnCamera()
    {
        if (robotUIPanel != null)
            robotUIPanel.SetActive(false);

        float t = 0;
        Vector3 startPos = playerCamera.transform.position;
        Quaternion startRot = playerCamera.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            playerCamera.transform.position = Vector3.Lerp(startPos, originalCamPos, t);
            playerCamera.transform.rotation = Quaternion.Lerp(startRot, originalCamRot, t);
            yield return null;
        }

        // ✅ Restaurar controles del jugador
        TogglePlayerControls(true);

        // ✅ Volver a ocultar y bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isInteracting = false;
        isPlayerNear = false;
    }

    void TogglePlayerControls(bool enabled)
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = enabled;

        if (playerLookScript != null)
            playerLookScript.enabled = enabled;
    }
}
