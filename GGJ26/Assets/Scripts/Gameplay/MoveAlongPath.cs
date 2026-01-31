using System.Collections.Generic;
using UnityEngine;

public enum InterpolationType
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut
        }
public class MoveAlongPath : InspectorAttributes
{
    [SerializeField] private float moveSpeed = 1f; // units per second
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private InterpolationType interpolationType = InterpolationType.EaseInOut;
    [SerializeField] private float interpolationIntensity = 1f;
    [SerializeField] private Transform pointObject;

    public List<Vector3> movePoints;

    private int currentPointIndex;
    private float segmentProgress;
    private float segmentLength;
    private bool waiting;
    private float waitTimer;
    private int direction = 1; // 1 = forward, -1 = backward

    private Vector3 segmentStart;
    private Vector3 segmentEnd;

    private void OnValidate()
    {
        ResetMovePoints();
    }

    [MethodButton("Reset Move Points")]
    public void ResetMovePoints()
    {
        movePoints.Clear();

        if (pointObject == null) return;

        for (int i = 0; i < pointObject.childCount; i++)
        {
            movePoints.Add(pointObject.GetChild(i).position);
        }
    }

    private void Start()
    {
        if (movePoints.Count < 2)
        {
            enabled = false;
            Debug.LogWarning($"{nameof(MoveAlongPath)} requires at least 2 move points.");
            return;
        }

        currentPointIndex = 0;
        transform.position = movePoints[0];
        SetupNextSegment();
    }

    private void Update()
    {
        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                SetupNextSegment();
            }
            return;
        }

        segmentProgress += moveSpeed / segmentLength * Time.deltaTime;
        float t = Mathf.Clamp01(segmentProgress);

        float easedT = ApplyEasing(t);
        transform.position = Vector3.Lerp(segmentStart, segmentEnd, easedT);

        if (t >= 1f)
        {
            waiting = true;
            waitTimer = waitTime;
        }
    }

    private void SetupNextSegment()
    {
        segmentProgress = 0f;

        segmentStart = movePoints[currentPointIndex];

        int nextIndex = currentPointIndex + direction;

        // Reverse direction at the ends
        if (nextIndex >= movePoints.Count)
        {
            direction = -1;
            nextIndex = currentPointIndex + direction;
        }
        else if (nextIndex < 0)
        {
            direction = 1;
            nextIndex = currentPointIndex + direction;
        }

        currentPointIndex = nextIndex;
        segmentEnd = movePoints[currentPointIndex];

        segmentLength = Vector3.Distance(segmentStart, segmentEnd);

        if (segmentLength <= Mathf.Epsilon)
        {
            SetupNextSegment();
        }
    }

    private float ApplyEasing(float t)
    {
        float p = Mathf.Max(0.01f, interpolationIntensity);

        switch (interpolationType)
        {
            case InterpolationType.EaseIn:
                return Mathf.Pow(t, p * 2f);

            case InterpolationType.EaseOut:
                return 1f - Mathf.Pow(1f - t, p * 2f);

            case InterpolationType.EaseInOut:
                return Mathf.SmoothStep(0f, 1f, Mathf.Pow(t, p));

            default:
                return t;
        }
    }
}
