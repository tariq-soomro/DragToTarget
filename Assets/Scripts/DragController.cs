using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] private float maxDrag = 3f; // Maximum drag distance

    [SerializeField] private LineRenderer lr;

    private Vector3 dragBeganPos; // Position of the mouse or touch when the drag began
    private Touch touch;

    [HideInInspector] public Vector3 dragDirection; // Pure drag direction without max drag distance
    [HideInInspector] public Vector3 dragDirectionClamped; // Clamped drag direction with max drag distance

    private enum ControlsType
    {
        Touch,
        Mouse
    }

    [SerializeField] private ControlsType controlsType = ControlsType.Mouse; // Default controls type are mouse

    void Update()
    {
        if (controlsType == ControlsType.Mouse)
        {
            DragWithMouse();
        }
        else if (controlsType == ControlsType.Touch)
        {
            DragWithTouch();
        }
    }

    void DragWithMouse()
    {
        // Mouse button inputs
        if (Input.GetMouseButtonDown(0)) DragBegan();
        if (Input.GetMouseButton(0)) DragMoved();
        if (Input.GetMouseButtonUp(0)) DragEnded();
    }

    void DragWithTouch()
    {
        // Touch inputs
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) DragBegan();
            if (touch.phase == TouchPhase.Moved) DragMoved();
            if (touch.phase == TouchPhase.Ended) DragEnded();
        }
    }

    // Drag begins defining start position of Line Rederer
    void DragBegan()
    {
        dragBeganPos = Camera.main.ScreenToWorldPoint(
            controlsType == ControlsType.Mouse ? Input.mousePosition : (Vector3)touch.position
        );
        dragBeganPos.z = 0f;
        lr.positionCount = 1;
        lr.SetPosition(0, dragBeganPos);
    }

    // Drag moved calculates the end point of the Line Rederer until mouse or touch is released
    void DragMoved()
    {
        Vector3 dragMovedPos = Camera.main.ScreenToWorldPoint(
            controlsType == ControlsType.Mouse ? Input.mousePosition : (Vector3)touch.position
        );
        dragMovedPos.z = 0f;
        lr.positionCount = 2;
        lr.SetPosition(1, dragMovedPos);
    }

    // Drag ended defines the end point of the Line Rederer and calculates the drag direction and clamped drag direction
    void DragEnded()
    {
        lr.positionCount = 0;

        Vector3 dragEndedPos = Camera.main.ScreenToWorldPoint(
            controlsType == ControlsType.Mouse ? Input.mousePosition : (Vector3)touch.position
        );
        dragEndedPos.z = 0f;

        dragDirection = dragEndedPos - dragBeganPos; // It will apply force without any max speed defined  (no clamp)
        dragDirectionClamped = Vector3.ClampMagnitude(dragDirection, maxDrag); // It will apply force with max speed defined

        // Here you can use dragDirection or dragDirectionClamped however you want
        Debug.Log(dragDirection);
        Debug.Log(dragDirectionClamped);
    }
}