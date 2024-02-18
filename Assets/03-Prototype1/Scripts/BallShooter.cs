using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShooter: MonoBehaviour
{
    static private BallShooter S; // a
    // fields set in the Unity Inspector pane
    [Header("Set in Inspector")] // a
    public GameObject prefabBall;
    public float velocityMult = 8f; // a

    // fields set dynamically
    [Header("Set Dynamically")] // a

    public GameObject launchPoint;
    public Vector3 launchPos; // b
    public GameObject Ball; // b
    public bool aimingMode; // b
    private Rigidbody BallRigidbody; // a
    static public Vector3 LAUNCH_POS
    { // b
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this; // c
        Transform launchPointTrans = transform.Find("LaunchPoint"); // a
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // b
        launchPos = launchPointTrans.position; // c
    }

    void OnMouseEnter()
    {
        //print("BallShooter:OnMouseEnter()");
        launchPoint.SetActive(true); // b
    }

    void OnMouseExit()
    {
        //print("BallShooter:OnMouseExit()");
        launchPoint.SetActive(false); // b
    }

    void OnMouseDown()
    { // d
      // The player has pressed the mouse button while over BallShooter
        aimingMode = true;
        // Instantiate a Ball
        Ball = Instantiate(prefabBall) as GameObject;
        // Start it at the launchPoint
        Ball.transform.position = launchPos;
        // Set it to isKinematic for now
        Ball.GetComponent<Rigidbody>().isKinematic = true;
        // Set it to isKinematic for now
        BallRigidbody = Ball.GetComponent<Rigidbody>(); // a
        BallRigidbody.isKinematic = true; // a
    }
    void Update()
    {
        // If BallShooter is not in aimingMode, don't run this code
        if (!aimingMode) return; // b
                                 // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition; // c
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Limit mouseDelta to the radius of the BallShooter SphereCollider // d
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Move the Ball to this new position
        Vector3 ballPos = launchPos + mouseDelta;
        Ball.transform.position = ballPos;
        if (Input.GetMouseButtonUp(0))
        { // e
          // The mouse has been released
            aimingMode = false;
            BallRigidbody.isKinematic = false;
            BallRigidbody.velocity = -mouseDelta * velocityMult;
            FollowBall.POI = Ball;
            Ball = null;
            Plinko.ShotFired(); // a
        }
    }
}