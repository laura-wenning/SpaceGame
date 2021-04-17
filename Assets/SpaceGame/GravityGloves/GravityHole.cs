using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Applied to the gravity hole prefab, either in the prefab or on instantiation 
 */
public class GravityHole : MonoBehaviour
{
  private GameObject collidingObject;

  // Start is called before the first frame update
  void Start()
  {
      
  }

  public void Fire() {
    if (collidingObject) {
      Debug.DrawRay(
        transform.position,
        (transform.position - collidingObject.transform.position),
        Color.yellow
      );
      collidingObject.GetComponent<Rigidbody>().AddForce(
        (transform.position - collidingObject.transform.position) * 20,
        ForceMode.Force
      );
      collidingObject.GetComponent<Rigidbody>().drag = 5;
    }
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    
  }

  public void OnTriggerEnter(Collider other) {
    SetCollidingObject(other);
  }

  public void OnTriggerStay(Collider other) {
    SetCollidingObject(other);
  }

  public void OnTriggerExit(Collider other) {
    Deactivate();
  }

  public void Deactivate() {
    if (!collidingObject) { return; }
    collidingObject.GetComponent<Rigidbody>().drag = 0;
    collidingObject = null;
  }

  private void SetCollidingObject(Collider col) {
    if (collidingObject || !col.GetComponent<Rigidbody>() || !col.gameObject.GetComponent<Tetherable>()) {
      return;
    }

    collidingObject = col.gameObject;
  }
}
