using System.Collections;
using UnityEngine;

public class HandMove : MonoBehaviour
{
    [SerializeField] private float _speed = 200;
    [SerializeField] private float _distance = 100;

    private float _passedDistance = 0;

    private void Start()
    {
        StartCoroutine(UnscaledFixedUpdate());
    }

    private IEnumerator UnscaledFixedUpdate()
    {
        yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);

        Vector3 pos = transform.localPosition;
        Vector3 newPos = Vector3.MoveTowards
            (
                pos, 
                pos + transform.TransformDirection(_speed > 0 ? Vector3.right : Vector3.left) * Mathf.Abs(_speed),
                Mathf.Abs(_speed) * Time.fixedUnscaledDeltaTime
            );

        _passedDistance += (newPos - pos).magnitude;

        transform.localPosition = newPos;

        if (_passedDistance >= _distance)
        {
            _speed = -_speed;
            _passedDistance = _distance - _passedDistance;
        }

        StartCoroutine(UnscaledFixedUpdate());
    }
}
