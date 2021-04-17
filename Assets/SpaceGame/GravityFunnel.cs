using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GravityFunnel : MonoBehaviour
{
  public SteamVR_Input_Sources handType;
  public SteamVR_Behaviour_Pose controllerPose;
  public SteamVR_Action_Boolean primeAction;
  public SteamVR_Action_Boolean fireAction;

  public GameObject funnelPrefab;
  public GameObject holePrefab;

  private GameObject funnel;
  private Transform funnelTransform;
  private GameObject hole;

  public float x, y, z;


  private bool funnelPrimed;
  private bool funnelLocked;
  private GameObject funnelTarget;

  // Start is called before the first frame update
  void Start()
  {
      funnel = Instantiate(funnelPrefab);
      hole = Instantiate(holePrefab);
      hole.transform.parent = transform;
      funnelTransform = funnel.transform;
      funnelPrimed = false;
      funnelLocked = false;
  }

  // Update is called once per frame
  void Update() {
    if (primeAction.GetState(handType)) {
      PrimeFunnel();
      if (fireAction.GetState(handType)) {
        FireFunnel();
      } else {
        ReleaseFunnel();
      }
    } else {
      DeprimeFunnel();
    }
  }

  private void FireFunnel() {
    if (!funnelTarget) { return; }
    var tether = funnelTarget.GetComponent<Tetherable>();
    if (!tether) { return; }

    if (funnelTarget)
    funnelTarget.GetComponent<Rigidbody>().AddForce(
      funnelTransform.TransformDirection(Vector3.down) / 5,
      ForceMode.Force
    );
  }

  private void ReleaseFunnel() {
    return;
  }

  private void DeprimeFunnel() {
    funnel.SetActive(false);
    hole.GetComponent<GravityHole>().Deactivate();
    hole.SetActive(false);
    funnelPrimed = false;
    funnelLocked = false;
    funnelTarget = null;

  }

  private void PrimeFunnel() {
    // funnel.SetActive(true);
    hole.SetActive(true);
    funnelTransform.position = controllerPose.transform.position; 
    hole.transform.localPosition = new Vector3(0, 0, 0.2f);
    hole.transform.rotation = controllerPose.transform.rotation;

    if (funnelLocked) {
      // Angle towards target
      // If > 5 degrees, forcibly break
    } else {
      funnelTransform.rotation = controllerPose.transform.rotation; 
      funnelTransform.Rotate(135, 0, 0); // Should be 135, need to match the target vector
      funnelTransform.Translate(0f, .125f, 0.05f);
    }
    
    // If we're looking 
    // If hit
    RaycastHit hit;

    // TODO - currently aligned on the top of the funnel. Should be in center
    Vector3 targetPos = funnelTransform.position; //controllerPose.transform.position;
    Vector3 targetVector = funnelTransform.TransformDirection(Vector3.up);

    if (Physics.Raycast(targetPos, targetVector, out hit, 100)) {
      if (!hit.rigidbody) { return; }
      funnelTarget = hit.rigidbody.gameObject;
      var tether = funnelTarget.GetComponent<Tetherable>();
      if (tether) { tether.TargetObject(); }
      
    } else {
      if (funnelTarget != null) {
        var tether = funnelTarget.GetComponent<Tetherable>();
        if (tether) { tether.UntargetObject(); }
        funnelTarget = null;
      }
    }
  }
}
