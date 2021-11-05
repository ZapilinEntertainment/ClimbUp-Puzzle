using UnityEngine;

public sealed class ItemRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float jumpHeight = 0.3f, jumpTime = 0.5f, _scaleMin = 1f, _scaleMax = 1.5f, _scaleTime = 1f;
    [Header("Стартовое случайное вращение")] [SerializeField] private bool _startRandomRotation = true;
    [SerializeField] private bool _jumpEnabled = true, _scalingEnabled = false;
    private bool gamePaused;
    private float savedHeight, _startVal;

    private void Start()
    {
        savedHeight = transform.position.y;
        _startVal = Random.value;
        if (_startRandomRotation) transform.Rotate(Vector3.up, Random.value * 360f);
        if (_jumpEnabled && jumpTime == 0f) _jumpEnabled = false;
        if (_scalingEnabled && _scaleTime == 0f) _scalingEnabled = false;
    }

    void Update()
    {
        if (!gamePaused)
        {
            transform.Rotate(rotation * Time.deltaTime, Space.Self);
            float t = Time.time;
            if ( _jumpEnabled)  transform.position = new Vector3(transform.position.x, savedHeight + (Mathf.PingPong(t / jumpTime + _startVal, 2f) - 1f)  * jumpHeight, transform.position.z);
            if (_scalingEnabled) transform.localScale = Vector3.one * (_scaleMin + Mathf.PingPong(_startVal + t / _scaleTime, 1f) * (_scaleMax - _scaleMin));
        }
    }
}
