using UnityEngine;

public interface ITongueable
{
    public abstract void OnTongued(Transform tongueOrigin, Transform playerTransform, TongueData tongueData, Vector3 hitPoint);
}
