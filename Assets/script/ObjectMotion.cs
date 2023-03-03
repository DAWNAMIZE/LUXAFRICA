using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMotion : MonoBehaviour
{

    private float _totalLerpDuration;
    private Vector3 _startPosition;
    private Vector3? _destination;
    private float _elapsedLerpDuration;
    private Action _onCompleteCallBack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        if (_destination.HasValue == false)
            return;

        if (_elapsedLerpDuration >= _totalLerpDuration && _totalLerpDuration > 0)
            return;

        _elapsedLerpDuration += Time.deltaTime; 

        float percent = _elapsedLerpDuration / _totalLerpDuration;

       transform.position = Vector3.Lerp(_startPosition, _destination.Value, percent);

        if (_elapsedLerpDuration >= _totalLerpDuration)
            _onCompleteCallBack?.Invoke();
    }

    public void MoveTo(Vector3 destination, float speedMetersPerSecond, Action onComplete = null)
    {
        float distanceToNextWaypoint = Vector3.Distance(transform.position, destination);
        _totalLerpDuration = distanceToNextWaypoint / speedMetersPerSecond;
        _startPosition = transform.position;
        _destination = destination;
        _elapsedLerpDuration = 0f;
        _onCompleteCallBack = onComplete;
    }

}
