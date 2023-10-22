using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField, Range(0.1f, 2)] private float width = 0.1f;

    private Camera cam;
    private LineRenderer lineRend;
    private Vector3 previousPosition;

    private bool isDrawing = false;
    private bool isDrawnOneTime = false;
    private bool isGameOver = false;
    private bool hasDrawn = false;

    public UnityAction onTouchUp, onTouchDown;

    private List<GameObject> deadCircles = new List<GameObject>();

    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 1;
        lineRend.startWidth = lineRend.endWidth = width;

        previousPosition = transform.position;
        cam = Camera.main;
    }
    private void Update()
    {
        if (isGameOver)
            return;

        if (Input.GetMouseButton(0))
        {
            isDrawing = true;
        }
        else
        {
            if (isDrawing && hasDrawn)
            {
                isDrawnOneTime = true;
                isGameOver = true;

                onTouchUp?.Invoke();
            }
            isDrawing = false;

            return;
        }
        if (!isDrawnOneTime)
        {
            Vector2 currentPos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Vector3.Distance(currentPos, previousPosition) > minDistance)
            {
                if (previousPosition == transform.position)
                {
                    lineRend.SetPosition(0, currentPos);
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Linecast(previousPosition, currentPos);

                    if (hit.collider != null)
                    {
                        GameObject circle = hit.collider.gameObject;

                        if (!deadCircles.Contains(circle))
                        {
                            deadCircles.Add(circle);

                            Animations.Instance.ItemClose(hit.collider.gameObject, delegate { Destroy(circle); deadCircles.Remove(circle); });
                        }
                    }

                    lineRend.positionCount++;
                    lineRend.SetPosition(lineRend.positionCount - 1, currentPos);

                    if (!hasDrawn)
                    {
                        onTouchDown?.Invoke();
                    }
                    hasDrawn = true;
                }
                previousPosition = currentPos;
            }
        }
    }
}