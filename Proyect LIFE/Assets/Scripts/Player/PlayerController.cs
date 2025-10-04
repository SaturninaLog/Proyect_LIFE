using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public CharacterController controller;
    public Camera playerCamera;
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private float xRotation = 0f;

    [Header("Herramientas")]
    public ToolManager toolManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
        HandleToolInputs();
    }

    // --- Movimiento del jugador ---
    void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // --- Rotación cámara/jugador ---
    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // --- Control de herramientas ---
    void HandleToolInputs()
    {
        if (toolManager == null) return;

        // 1 = suelo
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolManager.SetTool(ToolType.Build);
            toolManager.SetBuildPrefab(toolManager.soilPrefab);
        }

        // 2 = casa
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toolManager.SetTool(ToolType.Build);
            toolManager.SetBuildPrefab(toolManager.housePrefab);
        }

        // 3 = invernadero
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toolManager.SetTool(ToolType.Build);
            toolManager.SetBuildPrefab(toolManager.greenhousePrefab);
        }

        // 4 = modo plantar
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            toolManager.SetTool(ToolType.Plant);
        }

        // 0 = desactivar todo
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            toolManager.SetTool(ToolType.None);
        }
    }
}
