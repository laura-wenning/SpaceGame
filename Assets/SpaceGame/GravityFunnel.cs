using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GravityFunnel : MonoBehaviour
{
  public SteamVR_Input_Sources handType;
  public SteamVR_Behaviour_Pose controllerPose;
  public SteamVR_Action_Boolean activateAction;
  public SteamVR_Action_Boolean pullAction;

  public GameObject gravityFunnelPrefab;
  private GameObject gravityFunnel;
  private Transform gravityFunnelTransform;
  private Vector3 hitPoint;
  private Rigidbody activeTarget;



  // Start is called before the first frame update
  void Start()
  {
      gravityFunnel = Instantiate(gravityFunnelPrefab);
      gravityFunnelTransform = gravityFunnel.transform;
  }

  // Update is called once per frame
  void Update() {
    if (activateAction.GetState(handType)) {
      RaycastHit hit;
      ActivateFunnel();
      // ShowTarget(hit)

    } else {
      gravityFunnel.SetActive(false);
    }
  }

  private void ActivateFunnel() {
    gravityFunnel.SetActive(true);
    gravityFunnelTransform.position = controllerPose.transform.position; 
    gravityFunnelTransform.rotation = controllerPose.transform.rotation; 
    gravityFunnelTransform.Rotate(90, 0, 0); // Should be 135, need to match the target vector
    gravityFunnelTransform.Translate(0f, .25f, 0.05f);

    // If we're looking 
    // If hit
    RaycastHit hit;
    Vector3 targetPos = controllerPose.transform.position;
    Vector3 targetVector = transform.forward * 200;
    Debug.DrawRay(targetPos, targetVector, Color.yellow);
    if (Physics.Raycast(targetPos, targetVector, out hit, 100)) {
      // if (!activeTarget) { activeTarget = hit.rigidbody; }
      if (!hit.rigidbody) { return; }
      // wtf. Should bring towards hand. Goes whichever wobbly direction
      // We need to rotate this somehow
      hit.rigidbody.AddForce(
        Quaternion.AngleAxis(-135, Vector3.right) * controllerPose.transform.eulerAngles / -100,
        ForceMode.Force
      );
    }
  }
}
