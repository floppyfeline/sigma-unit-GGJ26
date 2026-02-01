using System.Collections.Generic;
using UnityEngine;

public enum InterpolationType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut
    }
public enum MoveLoopType
{
    PingPong,
    Loop
}

public class MoveAlongPath : InspectorAttributes
{
    [SerializeField] private float moveSpeed = 1f; // units per second
    [SerializeField] private float waitTime = 0f;

    [Header("Interpolation")]
    [SerializeField] private InterpolationType interpolationType = InterpolationType.EaseInOut;
    [SerializeField] private float interpolationIntensity = 1f;

    [Header("Looping")]
    [SerializeField] private MoveLoopType loopType = MoveLoopType.PingPong;

    [Header("Points")]
    [SerializeField] private Transform pointObject;

    public List<Vector3> movePoints = new();

    private int currentPointIndex;
    private float segmentProgress;
    private float segmentLength;

    private bool waiting;
    private float waitTimer;

    private bool movementPaused;

    // Used only for PingPong
    private int direction = 1;

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

        if (pointObject == null)
            return;

        for (int i = 0; i < pointObject.childCount; i++)
            movePoints.Add(pointObject.GetChild(i).position);
    }

    public void PauseMovement()
    {
        movementPaused = true;
    }
    public void ResumeMovement()
    {
        movementPaused = false;
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
        direction = 1;

        transform.position = movePoints[0];
        SetupNextSegment();
    }

    private void Update()
    {
        if(movementPaused || !GameManager.Instance.GetGameActive()) return;

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

        int nextIndex = currentPointIndex;

        switch (loopType)
        {
            case MoveLoopType.PingPong:
            {
                nextIndex += direction;

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
                break;
            }

            case MoveLoopType.Loop:
            {
                nextIndex = (currentPointIndex + 1) % movePoints.Count;
                break;
            }
        }

        currentPointIndex = nextIndex;
        segmentEnd = movePoints[currentPointIndex];

        segmentLength = Vector3.Distance(segmentStart, segmentEnd);

        // Skip zero-length segments safely
        if (segmentLength <= Mathf.Epsilon)
            SetupNextSegment();
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