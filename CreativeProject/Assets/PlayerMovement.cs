using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace playerMovement 
{
    public class PlayerMovement : MonoBehaviour
        {
        public float movementSpeed = 0.5f; //controls how fast the player moves
        public float cameraSmoothFollowSpeed;
        public GameObject menu, map;
        private InputSystem_Actions inputActions;
        // Update is called once per frame

        private Rigidbody2D rb2D;//attempting to use rigid body not transform object
        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();//rigidbody attempt
        }

        private void Awake()
        {
            inputActions = new InputSystem_Actions(); // Initialize the input actions
            inputActions.UI.Menu.performed += ctx => ToggleMenu();
            inputActions.UI.Map.performed += ctx => ToggleMap();
        }

        void Update()
        {
            float moveX = Input.GetAxis("Horizontal"); //A/D arrows
            float moveY = Input.GetAxis("Vertical"); //W/S arrows

            Vector2 movement = new Vector2(moveX, moveY); //vector2 combines both inputs into one single vector 

            rb2D.MovePosition(rb2D.position + movement * movementSpeed * Time.fixedDeltaTime);
           // transform.Translate(movement * movementSpeed * Time.deltaTime); //move character based on both inputs and the speed
            Vector3 camPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z); //follows player with a delay (lerp)
            Vector3 playerPos = new Vector3(transform.position.x, transform.position.y, -10f);
            Camera.main.transform.position = Vector3.Lerp(camPos, playerPos, Time.deltaTime * cameraSmoothFollowSpeed);
        }

        private void OnEnable()
        {
            inputActions.Enable(); // Enable the input actions when the key is enabled
        }

        private void OnDisable()
        {
            inputActions.Disable(); // Disable the input actions when the key is disabled
        }
        private void ToggleMenu()
        {
            menu.SetActive(!menu.activeSelf); //toggles menu game object
        }

        private void ToggleMap()
        {
            map.SetActive(!map.activeSelf); //toggles map
        }
    }
}