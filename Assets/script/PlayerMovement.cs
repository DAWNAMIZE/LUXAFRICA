using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public Transform[] views;

    public float transitionSpeed;
    public float mouseSensitivity = 100f;
    public Transform PlayerBody;
    private float xRotation = 0f;
    private Transform currentView;
    public GameObject target;
    private float smoothTime = 0.3f;
    private Vector3 _target;
    [SerializeField] private float _speed = 5f;
    private Camera mainCamera;
    public float Speed = 5;

    private bool isMoving = false;

    public float speed = 13f;
    public float gravity = -9f;

    Vector3 velocity;


    // Update is called once per frame
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            transform.eulerAngles += Speed * new Vector3(+Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }

      
    }


}
