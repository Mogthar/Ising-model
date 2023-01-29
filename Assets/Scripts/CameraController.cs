using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _speed;
    
    private Vector2 _movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Translate();
    }

    public void OnMove(InputValue value)
    {
        _movementDirection = value.Get<Vector2>();
    }

    private void Translate()
    {
        Vector3 translation = new Vector3(_movementDirection.x, _movementDirection.y, 0f);
        transform.position += _speed * Time.deltaTime * translation;
    }
}
