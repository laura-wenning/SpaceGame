using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabObject : MonoBehaviour
{
  // Start is called before the first frame update
  public SteamVR_Behaviour_Pose controllerPose;

  private GameObject collidingObject;
  private GameObject objectInHand;

  private void SetCollidingObject(Collider col) {
    if (collidingObject || !col.GetComponent<Rigidbody>()) {
      return;
    }

    collidingObject = col.gameObject;
  }

  public void OnTriggerEnter(Collider other) {
    SetCollidingObject(other);
  }

  public void OnTriggerStay(Collider other) {
    SetCollidingObject(other);
  }

  public void OnTriggerExit(Collider other) {
    if (!collidingObject) { return; }
    collidingObject = null;
  }

  private void GrabObject() {
    objectInHand = collidingObject;
    collidingObject = null;

    var joint = AddFixedJoint();
    joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
  }

  private FixedJoint AddFixedJoint() {
    FixedJoint fx = gameObject.AddComponent<FixedJoint>();
    fx.breakForce = 20000;
    fx.breakTorque = 20000;
    return fx;
  }

  private void ReleaseObject() {
    if (GetComponent<FixedJoint>()) {
      GetComponent<FixedJoint>().connectedBody = null;
      Destroy(GetComponent<FixedJoint>());

      objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
      objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
    }

    objectInHand = null;
  }

  // Update is called once per frame
  void Update()
  {
    
  }

  public void AttemptGrab() {
    if (collidingObject) { GrabObject(); }
  }

  public void AttemptRelease() {
    if (objectInHand) { ReleaseObject(); }
  }
}
