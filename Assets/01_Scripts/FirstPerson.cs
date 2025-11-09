using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float speed = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

      
        // NO borramos los PlayerPrefs aquí
        // PlayerPrefs.DeleteKey("PlayerPosX");
        // PlayerPrefs.DeleteKey("PlayerPosY");
        // PlayerPrefs.DeleteKey("PlayerPosZ");
    }

    void Update()
    {
        // Movimiento con WASD y Horizontal/Vertical
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W)) moveZ += 1f;
        if (Input.GetKey(KeyCode.S)) moveZ -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime * transform.localScale.x);

        // Aplicar gravedad
        if (!controller.isGrounded)
        {
            playerVelocity.y += gravity * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else if (playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar cualquier minijuego
        if (other.CompareTag("Minijuego"))
        {
            

            // Cambiar de escena al minijuego
            MinigameTrigger trigger = other.GetComponent<MinigameTrigger>();
            if (trigger != null && !string.IsNullOrEmpty(trigger.nombreEscenaMinijuego))
            {
                SceneManager.LoadScene(trigger.nombreEscenaMinijuego);
            }
        }
    }
}
