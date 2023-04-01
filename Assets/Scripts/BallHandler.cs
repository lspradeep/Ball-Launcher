using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D anchorRB;
    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSprintJoint;
    private bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SpawnBall), 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            if (isDragging)
            {
                isDragging = false;

                currentBallRigidbody.isKinematic = false;//make ball dynamic
                currentBallRigidbody = null;//clearing the reference so that use can't hold a ball more than one time

                Invoke(nameof(LaunchBall), 0.2f);
            }
            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;
        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);
        currentBallRigidbody.position = worldPos;
    }


    private void LaunchBall()
    {
        currentBallSprintJoint.enabled = false;//cut the spring attachment
        currentBallSprintJoint = null;

        Invoke(nameof(SpawnBall), 2f);
    }

    private void SpawnBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, anchorRB.position, Quaternion.identity);

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSprintJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSprintJoint.connectedBody = anchorRB;
    }
}
