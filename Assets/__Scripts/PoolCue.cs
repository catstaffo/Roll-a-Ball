using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCue : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;

    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive( false );
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter() {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive( true );
    }

    void OnMouseExit() {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive( false );
    }

    void OnMouseDown() {
        //player has pressed down
        aimingMode = true;
        projectile = Instantiate( prefabProjectile ) as GameObject;
        //start at launchpoint
        projectile.transform.position = launchPos;
        //set to isKinematic
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    void Update() {
        //if slingshot isn't in aimingMode, don't run this code
        if (!aimingMode) return;

        //get the current mouse pos in 2d screen coord
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint ( mousePos2D );

        //find delta from launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D-launchPos;
        //limit mouseDelta to the radius of the Cue Capsule Collider
        float maxMagnitude = this.GetComponent<CapsuleCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;

        //move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta ;
        projectile.transform.position = projPos;

        if ( Input.GetMouseButtonUp(0) ) {
            //the mouse has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            //FollowCam.POI = projectile;
            projectile = null;
            //MissionDemolition.ShotFired();
        }
        }
    }
}
