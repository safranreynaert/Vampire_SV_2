using UnityEngine;


/// <summary>
/// Represents a following camera
/// on the X and Y axis
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Tooltip("The target to follow")] [SerializeField]
    GameObject _target;
    
    [Tooltip("Time in seconds to catch the target")] [SerializeField]
    float _speed = 0.3f;

    [Tooltip("Offset Z axis")] [SerializeField]
    float _offset;

    [Tooltip("Offset Y axis")] [SerializeField]
    float _y;

    [Header("Limits")] [SerializeField] Transform _topRight;
    [SerializeField] Transform _bottomLeft;

    Transform _transform;
    Vector3 _velocity;
    Camera _camera;
    
    void Awake()
    {
        _transform = transform;
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float orthoSizeZ = _camera.orthographicSize;
        float orthoSizeX = (Screen.width * _camera.orthographicSize) / Screen.height;

        var targetPosition = _target.transform.position;
        Vector3 cameraDestination = new Vector3(targetPosition.x, transform.position.y, targetPosition.z + _offset);

        cameraDestination.z = Mathf.Clamp(cameraDestination.z, _bottomLeft.transform.position.z + orthoSizeZ,
            _topRight.transform.position.z - orthoSizeZ);
        cameraDestination.x = Mathf.Clamp(cameraDestination.x, _bottomLeft.transform.position.x + orthoSizeX,
            _topRight.transform.position.x - orthoSizeX);

        _transform.position = Vector3.SmoothDamp(new Vector3(_transform.position.x, _y, _transform.position.z), cameraDestination, ref _velocity, _speed);
    }
}