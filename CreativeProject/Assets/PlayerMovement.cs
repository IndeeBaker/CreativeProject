//this script used AI for assistance 

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace playerMovement
{
    public class PlayerMovement : MonoBehaviour
    {
        public float movementSpeed = 0.5f; // Controls how fast the player moves
        public float cameraSmoothFollowSpeed;
        public GameObject menu, map;

        private InputSystem_Actions inputActions;
        private Rigidbody2D rb2D;
        private Vector2 movementInput; // Stores movement input from player

        private void Awake()
        {
            inputActions = new InputSystem_Actions(); // Initialize input actions
            inputActions.UI.Menu.performed += ctx => ToggleMenu();
            inputActions.UI.Map.performed += ctx => ToggleMap();
        }

        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>(); // Cache the Rigidbody2D

            // DEBUG: Freeze everything to see if physics is the issue
            //rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Update()
        {
            // Capture input from old input system
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            movementInput = new Vector2(moveX, moveY).normalized; // Normalize to prevent faster diagonal movement
        }

        private void FixedUpdate()
        {
            // Move player using Rigidbody2D physics
            rb2D.MovePosition(rb2D.position + movementInput * movementSpeed * Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            // Smooth camera follow using Lerp
            Vector3 camPos = Camera.main.transform.position;
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, camPos.z);
            Camera.main.transform.position = Vector3.Lerp(camPos, targetPos, Time.deltaTime * cameraSmoothFollowSpeed);
        }

        private void OnEnable()
        {
            inputActions.Enable(); // Enable the input actions
        }

        private void OnDisable()
        {
            inputActions.Disable(); // Disable the input actions
        }

        private void ToggleMenu()
        {
            menu.SetActive(!menu.activeSelf); // Toggles the menu UI
        }

        private void ToggleMap()
        {
            map.SetActive(!map.activeSelf); // Toggles the map UI
        }
    }
}
