using UnityEngine;
using UnityEngine.InputSystem;

public class ShoulderCamera : MonoBehaviour
{
    [SerializeField] private Transform character;
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private float minY = -40f;
    [SerializeField] private float maxY = 70f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;

    public void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = character.position;
        character.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
