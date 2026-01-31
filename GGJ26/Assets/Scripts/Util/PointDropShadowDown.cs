using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PointDropShadowDown : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 2f;
    [SerializeField] private Rigidbody _targetRigidbody;
    private SphereCollider _targetCollider;
    [SerializeField] private float _projectionDepthBuffer = 0.01f;
    [SerializeField] private LayerMask _detectedGroundLayers;
    private Vector3 _offset;
    [SerializeField] private bool _useManualOffset = false;
    [SerializeField] private Transform _manualOffset;
    private Vector3 _originalSize;
    private DecalProjector _projector;
    private void Start()
    {
        _projector = GetComponent<DecalProjector>();
        _originalSize = _projector.size;
        _offset = transform.position - _targetRigidbody.position;
        _targetCollider = _targetRigidbody.GetComponent<SphereCollider>();
    }
    void Update()
    {
        if (_targetRigidbody == null || _projector.enabled == false)
            return;

        // Copy only Y rotation, ignore X/Z to keep it flat
        Vector3 euler = _targetRigidbody.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(90f, euler.y, 0f); // 90° pitch to lie flat, Y to match

        // Raycast to manage projection depth
        float projectionDepth = Physics.SphereCast(
            _targetRigidbody.position + Vector3.up * _targetCollider.radius,
            _targetCollider.radius,
            Vector3.down,
            out RaycastHit hit,
            Mathf.Infinity,
            _detectedGroundLayers, QueryTriggerInteraction.Ignore
        ) ? hit.distance + _projectionDepthBuffer - _targetCollider.radius : 0f;

        Vector3 basePosition = _useManualOffset ? _manualOffset.position : _targetRigidbody.position;
        Vector3 offset = _useManualOffset
            ? Vector3.zero
            : _offset;

        Vector3 rotatedOffset = _targetRigidbody.rotation * offset;

        transform.position = basePosition + rotatedOffset + Vector3.down * projectionDepth;

        if (hit.collider == null || hit.distance == 0)
            return;
        float safeDistance = Mathf.Max(hit.distance, 0.1f);
        float t = Mathf.InverseLerp(0.1f, _maxDistance, safeDistance);
        float scale = 1f - t; // 1 at close, 0 at maxDistance+
        _projector.size = new Vector3(_originalSize.x * scale, _originalSize.y * scale, _originalSize.z);
    }


}