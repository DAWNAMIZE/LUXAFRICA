using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _target;
    private float _speed;

    private void start()
    {
        _speed = 6f;

    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _target = (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if ((Vector3) transform.position != _target)
        {
            MovePlayer();
        }

    }
    private void MovePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }
}
