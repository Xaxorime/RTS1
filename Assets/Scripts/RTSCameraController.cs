using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RTSCameraController : MonoBehaviour
{
    private RTSControls controls;
    public static RTSCameraController instance;

    // If we want to select an item to follow, inside the item script add:
    // public void OnMouseDown(){
    //   CameraController.instance.followTransform = transform;
    // }

    [Header("General")]
    [SerializeField] Transform cameraTransform;
    public Transform followTransform;
    Vector3 newPosition;
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;
    

    [Header("Optional Functionality")]
    [SerializeField] bool moveWithKeyboad;
    [SerializeField] bool moveWithEdgeScrolling;
    [SerializeField] bool moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] float fastSpeed = 0.05f;
    [SerializeField] float normalSpeed = 0.01f;
    [SerializeField] float movementSensitivity = 1f; // Hardcoded Sensitivity
    float movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] float edgeSize = 50f;
    bool isCursorSet = false;
    public Texture2D cursorArrowUp;
    public Texture2D cursorArrowDown;
    public Texture2D cursorArrowLeft;
    public Texture2D cursorArrowRight;

    CursorArrow currentCursor = CursorArrow.DEFAULT;

    private void Awake()
    {
        controls = new RTSControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    enum CursorArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT
    }

    private void Start()
    {
        instance = this;

        newPosition = transform.position;

        movementSpeed = normalSpeed;
    }

    private void Update()
    {
        // Allow Camera to follow Target
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        // Let us control Camera
        else
        {
            HandleCameraMovement();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            followTransform = null;
        }
    }

    void HandleCameraMovement()
    {
        // Mouse Drag
        if (moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        // Keyboard Control
        if (moveWithKeyboad)
        {
            movementSpeed = controls.Camera.FastMove.IsPressed()
                ? fastSpeed
                : normalSpeed;

            Vector2 move = controls.Camera.Move.ReadValue<Vector2>();

            Vector3 direction =
                transform.forward * move.y +
                transform.right * move.x;

            direction.y = 0f;

            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            newPosition += direction * movementSpeed;
        }

        // Edge Scrolling
        if (moveWithEdgeScrolling)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            // Move Right
            if (mousePos.x > Screen.width - edgeSize)
            {
                newPosition += (transform.right * movementSpeed);
                ChangeCursor(CursorArrow.RIGHT);
                isCursorSet = true;
            }

            // Move Left
            else if (mousePos.x < edgeSize)
            {
                newPosition += (transform.right * -movementSpeed);
                ChangeCursor(CursorArrow.LEFT);
                isCursorSet = true;
            }

            // Move Up
            else if (mousePos.y > Screen.height - edgeSize)
            {
                newPosition += (transform.forward * movementSpeed);
                ChangeCursor(CursorArrow.UP);
                isCursorSet = true;
            }

            // Move Down
            else if (mousePos.y < edgeSize)
            {
                newPosition += (transform.forward * -movementSpeed);
                ChangeCursor(CursorArrow.DOWN);
                isCursorSet = true;
            }
            else
            {
                if (isCursorSet)
                {
                    ChangeCursor(CursorArrow.DEFAULT);
                    isCursorSet = false;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSensitivity);

        Cursor.lockState = CursorLockMode.Confined; // If we have an extra monitor we don't want to exit screen bounds
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        // Only change cursor if its not the same cursor
        if (currentCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowRight.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }

            currentCursor = newCursor;
        }
    }



    private void HandleMouseDragInput()
    {
        if (controls.Camera.DragCamera.WasPressedThisFrame() && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (controls.Camera.DragCamera.IsPressed() && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }
}