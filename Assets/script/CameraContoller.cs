using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class CameraContoller : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;

    public float touchSensitivity = 40f;
    public float mouseSensitivity = 100f;
   
    public Transform PlayerBody;
    private float xRotation = 0f;
    private Transform currentView;
    public GameObject target;
    private float smoothTime = 0.3f;
    private Vector3 _target;
    [SerializeField]private float _speed = 5f;
    private Camera mainCamera;
    public float CameraDragSpeed = 5;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private LayerMask Interactable;

    public float ObjectSelectionDistance = 1000;

    private bool isMoving = false;


    private float sensitivity;

    [DllImport("__Internal")]
    private static extern void CartTime(string id);
    private Rigidbody rb;



    [SerializeField]public LayerMask markerLayer;


    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        rb = GetComponentInParent<Rigidbody>();
    }

    private float GetSensitivity()
    {
        if (StoreManager.Main == null)
        {
            sensitivity = mouseSensitivity;
            return sensitivity;
        }

        sensitivity = StoreManager.Main.IsRunningOnMobile ? touchSensitivity : mouseSensitivity;
        return sensitivity;
    }

    private void Update()
    {
        PlayerMovement();

        ObjectSelection();
        ObjectHover();

        UseCameraGyro();
    }

    private void UseCameraGyro()
    {
        if (StoreManager.Main == null)
            return;
        if (StoreManager.Main.IsRunningOnMobile)
        {
            if (StoreManager.Main.DeviceSupportGyro)
            {
                mainCamera.transform.rotation = StoreManager.Main.GyroQuaternion;
                mainCamera.transform.Rotate(90f, 0f, 0f, Space.World);
            }
        }
    }

    private void CameraDrag()
    {
        if (UIManager.Main.IsUIOpen == true)
        {
            return;
        }

        if (VirtualJoystick.Main != null)
        {
            if (VirtualJoystick.Main.isDragging)
                return;
        }

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * GetSensitivity() * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * GetSensitivity() * Time.deltaTime;


//            Debug.Log(mouseX + " " + mouseY);
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            //transform.eulerAngles += CameraDragSpeed * new Vector3(+Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            PlayerBody.Rotate(Vector3.up * mouseX);

        }
    }

    private void LateUpdate()
    {
            CameraDrag();

        


        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving == false)
            {

                MoveCameraToPosition();
            }
        }

      
    }

    private Vector3 MovementDir()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 touchDir = Vector3.zero;
        if (VirtualJoystick.Main != null)
            touchDir = new Vector3(VirtualJoystick.Main.direction.x, 0, VirtualJoystick.Main.direction.y);
        if (dir != Vector3.zero)
            return dir;
        else
            return touchDir;
    }

    private void PlayerMovement()
    {
        if (UIManager.Main.IsUIOpen == true)
        {
            return;
        }
        Vector3 dir = MovementDir();
        
        Vector3 moveVector = PlayerBody.transform.TransformDirection(dir) * walkSpeed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
        
    }


    private void MoveCameraToPosition()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, markerLayer) )
        {
            if (hit.collider.CompareTag("FloorMarker"))
            {
                Vector3 newMovePosition = new Vector3(hit.collider.transform.position.x, PlayerBody.transform.position.y, hit.collider.transform.position.z);

               
                // Move the camera into position
                // PlayerBody.transform.position = Vector3.Lerp(PlayerBody.transform.position, newMovePosition, Time.deltaTime * _speed);
                isMoving = true;
                PlayerBody.GetComponent<ObjectMotion>().MoveTo(newMovePosition, _speed, () =>
                {
                    isMoving = false;
                });
            }
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    //Mistake happened here vvvv
        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;
        //}

        //if (Cursor.visible && Input.GetMouseButtonDown(1))
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}

    }

    private Collider _lastHoverObject;
    private void ObjectSelection()
    {

        if (UIManager.Main.IsUIOpen)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, ObjectSelectionDistance, Interactable))
            {
                if (hit.collider.gameObject.layer != 10)
                    return;

                   if (_lastHoverObject != hit.collider)
                {
                    _lastHoverObject = hit.collider;
                    Outline objectOutline = _lastHoverObject.GetComponent<Outline>();
                    if (objectOutline != null)
                    {
                        objectOutline.OutlineMode = Outline.Mode.OutlineVisible;
                    }
                }


                if (hit.collider.CompareTag("ShopItems"))
                {
                    Debug.Log(hit.collider.name);

                    UIManager.Main.DisplayItemDiscription(hit.collider);

                    //#if UNITY_WEBGL == true && UNITY_EDITOR == false
                    //                    CartTime(hit.collider.name);
                    //#endif
                    //UIManager.Main.AddToCart(hit.collider.name);
                }
            }
            else
            {
                if(_lastHoverObject != null)
                {
                   Outline objectOutline = _lastHoverObject.GetComponent<Outline>();
                    if (objectOutline != null)
                    {
                        objectOutline.OutlineMode = Outline.Mode.OutlineHidden;
                    }
                    _lastHoverObject = null;
                }
            }
        }

    }

    private void ObjectHover()
    {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, ObjectSelectionDistance, Interactable))
            {
            if (hit.collider.gameObject.layer != 10)
                return;

            if (_lastHoverObject != hit.collider)
                {
                    _lastHoverObject = hit.collider;
                    Outline objectOutline = _lastHoverObject.GetComponent<Outline>();
                    if (objectOutline != null)
                    {
                        objectOutline.OutlineMode = Outline.Mode.OutlineVisible;
                    }
                }
            }
            else
            {
                if (_lastHoverObject != null)
                {
                    Outline objectOutline = _lastHoverObject.GetComponent<Outline>();
                    if (objectOutline != null)
                    {
                        objectOutline.OutlineMode = Outline.Mode.OutlineHidden;
                    }
                    _lastHoverObject = null;
                }
            }
        }
    

}