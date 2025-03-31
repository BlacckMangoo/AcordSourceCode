using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveBetweenNPoints : Action
{

    public SharedTransformList movePoints;
    public SharedFloat moveSpeed = 5f;
    public SharedFloat minWaitTime = 0.5f;
    public SharedFloat maxWaitTime = 2f;
    public SharedFloat minMoveTime = 3f;
    public SharedFloat maxMoveTime = 8f;

    private int currentPointIndex = -1;
    private float waitCounter;
    private float moveTimeCounter;
    private bool isMoving = false;
    private HashSet<int> visitedPoints = new HashSet<int>();

    public override void OnStart()
    {
        if (movePoints.Value.Count == 0)
        {
            return;
        }

        visitedPoints.Clear();
        currentPointIndex = Random.Range(0, movePoints.Value.Count);
        isMoving = true;
        waitCounter = 0f;
        moveTimeCounter = GetRandomMoveTime();
    }

    public override TaskStatus OnUpdate()
    {
        if (movePoints.Value.Count == 0)
        {
            return TaskStatus.Failure;
        }

        if (isMoving)
        {
            Transform targetPoint = movePoints.Value[currentPointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed.Value * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
            {
                visitedPoints.Add(currentPointIndex);
                isMoving = false;
                waitCounter = GetRandomWaitTime();

                if (visitedPoints.Count == movePoints.Value.Count)
                {
                    return TaskStatus.Success;
                }
            }
            else
            {
                moveTimeCounter -= Time.deltaTime;
                if (moveTimeCounter <= 0f)
                {
                    ChooseNewPoint();
                    moveTimeCounter = GetRandomMoveTime();
                }
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                ChooseNewPoint();
                isMoving = true;
                moveTimeCounter = GetRandomMoveTime();
            }
        }

        return TaskStatus.Running;
    }

    private void ChooseNewPoint()
    {
        if (visitedPoints.Count == movePoints.Value.Count - 1)
        {
            currentPointIndex = Enumerable.Range(0, movePoints.Value.Count)
                                          .Except(visitedPoints)
                                          .First();
        }
        else
        {
            int newPointIndex;
            do
            {
                newPointIndex = Random.Range(0, movePoints.Value.Count);
            } while (newPointIndex == currentPointIndex || visitedPoints.Contains(newPointIndex));

            currentPointIndex = newPointIndex;
        }
    }

    private float GetRandomWaitTime()
    {
        return Random.Range(minWaitTime.Value, maxWaitTime.Value);
    }

    private float GetRandomMoveTime()
    {
        return Random.Range(minMoveTime.Value, maxMoveTime.Value);
    }

    public override void OnReset()
    {
        movePoints = null;
        moveSpeed = 5f;
        minWaitTime = 0.5f;
        maxWaitTime = 2f;
        minMoveTime = 3f;
        maxMoveTime = 8f;
    }
}
