using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LocalPlayerSetup : MonoBehaviour
{
    [Header("Jugadores")]
    [SerializeField] private GameObject jugador1;
    [SerializeField] private GameObject jugador2;
    [SerializeField] private GameObject camJugador1;

    [Header("Cámara Jugador 2 (Render Texture)")]
    [SerializeField] private Camera camJugador2;
    [SerializeField] private RenderTexture renderTextureJugador2;
    [SerializeField] private RawImage uiRawImageJugador2;

    [Header("Input Asset")]
    [SerializeField] private InputActionAsset inputActions;

    private int numJugadores = 1;

    void Start()
    {
        numJugadores = PlayerPrefs.GetInt("NPlayer", 1);

        jugador1.SetActive(true);
        SetupPlayerInputs(jugador1, "Keyboard");

        if (numJugadores >= 2)
        {
            jugador2.SetActive(true);
            SetupPlayerInputs(jugador2, "Gamepad");
            SetupCameraJugador2();
            UIManager.instance.CambiarA2();
        }
        else
        {
            jugador2.SetActive(false);

            if (uiRawImageJugador2 != null)
                uiRawImageJugador2.gameObject.SetActive(false);

            if (camJugador2 != null)
                camJugador2.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        camJugador1.transform.position = new Vector3(jugador1.transform.position.x,jugador1.transform.position.y,camJugador1.transform.position.z);
    }

    private void SetupCameraJugador2()
    {
        if (camJugador2 == null) return;

        camJugador2.gameObject.SetActive(true);

        if (renderTextureJugador2 != null)
        {
            camJugador2.targetTexture = renderTextureJugador2;
        }

        if (uiRawImageJugador2 != null)
        {
            uiRawImageJugador2.gameObject.SetActive(true);
            if (renderTextureJugador2 != null)
            {
                uiRawImageJugador2.texture = renderTextureJugador2;
            }
        }
    }

    private void SetupPlayerInputs(GameObject player, string actionMapName)
    {
        if (inputActions == null) return;

        InputActionAsset instance = Instantiate(inputActions);
        InputActionMap map = instance.FindActionMap(actionMapName, true);
        map.Enable();

        InputAction move = map.FindAction("Move", true);
        InputAction interact = map.FindAction("Interact", true);
        InputAction throwAct = map.FindAction("Throw", true);
        InputAction aim = map.FindAction("Aim", true);
        InputAction nextObj = map.FindAction("NextObject", true);
        InputAction prevObj = map.FindAction("PreviousObject", true);

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.SetMoveAction(move);
        }

        PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
        if (interaction != null)
        {
            bool isGamepad = actionMapName == "Gamepad";
            interaction.SetGamepad(isGamepad);
            interaction.SetInputActions(interact, throwAct, aim, nextObj, prevObj);
        }
    }
}
