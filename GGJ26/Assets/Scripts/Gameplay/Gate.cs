using System.Collections.Generic;
using UnityEngine;

public class Gate : InspectorAttributes
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private InterpolationType interpolationType = InterpolationType.EaseInOut;
    [SerializeField] private float interpolationIntensity = 1f;
    [SerializeField] private Transform pointObject;

    [SerializeField] private bool looping = false;
    [SerializeField] private bool moveInitially = false;
    public List<Vector3> movePoints = new();

    private int currentPointIndex;
    private int direction;

    private float segmentProgress;
    private float segmentLength;

    private bool isMoving;
    private bool isOpen;

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
            movePoints.Add(pointObject.GetChild(i).position);
    }

    private void Start()
    {
        if (movePoints.Count < 2)
        {
            enabled = false;
            Debug.LogWarning("Gate requires at least 2 move points.");
            return;
        }

        currentPointIndex = 0;
        transform.position = movePoints[0];
        isOpen = false;

        if (moveInitially) isMoving = true;
    }
    public void TriggerMove()
    {
        if (isMoving) return;

        if (looping)
        {
            // start ping-pong forever
            direction = 1;
            isMoving = true;
        }
        else
        {
            // toggle open/close
            direction = isOpen ? -1 : 1;
            isOpen = !isOpen;
            isMoving = true;
        }

        SetupNextSegment();
    }

    private void Update()
    {
        if (!isMoving || !GameManager.Instance.GetGameActive()) return;

        segmentProgress += moveSpeed / segmentLength * Time.deltaTime;

        float t = Mathf.Clamp01(segmentProgress);
        float easedT = ApplyEasing(t);

        transform.position = Vector3.Lerp(segmentStart, segmentEnd, easedT);

        if (t >= 1f)
        {
            HandleSegmentFinished();
        }
    }

    private void HandleSegmentFinished()
    {
        if (looping)
        {
            // reverse at ends and keep going forever
            if (currentPointIndex == movePoints.Count - 1)
                direction = -1;
            else if (currentPointIndex == 0)
                direction = 1;

            SetupNextSegment();
            return;
        }

        // non-looping behavior (toggle mode)
        if ((direction == 1 && currentPointIndex >= movePoints.Count - 1) ||
            (direction == -1 && currentPointIndex <= 0))
        {
            isMoving = false;
            return;
        }

        SetupNextSegment();
    }

    private void SetupNextSegment()
    {
        segmentProgress = 0f;

        segmentStart = movePoints[currentPointIndex];
        currentPointIndex += direction;
        segmentEnd = movePoints[currentPointIndex];

        segmentLength = Vector3.Distance(segmentStart, segmentEnd);
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