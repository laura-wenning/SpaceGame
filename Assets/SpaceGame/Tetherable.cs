using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetherable : MonoBehaviour
{
  public Material glowPrefab;
  public Shader shaderPrefab;
  private Material glow;
  private Material none; 

  private MeshRenderer render;


  // Start is called before the first frame update
  void Start()
  {
    glow = Instantiate(glowPrefab);
    render = GetComponent<MeshRenderer>();
    Debug.Log(render);
  }

  // Update is called once per frame
  void Update()
  {
      
  }

  public void TargetObject() {
    // if (render.materials.Length >= 2) { return; }
    // render.materials[1] = glowPrefab; 
  }

  public void UntargetObject() {
    // glow.SetActive(false); 
    // render.materials[1] = glowPrefab;
  }
}
