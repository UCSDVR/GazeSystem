/*
 * Author: Connor Smith
 * Version: 3.0
 * Date: 02/16/16
 */

using UnityEngine;
using System.Collections;

public class GazeExample : MonoBehaviour, Gazeable {

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }

  /*
   * Required method for use with the Gaze system, based on
   * implementation of Gazeable. Will return true if all
   * conditions for gaze activation have been met, and
   * false if the Activate() method should not yet be called
   * on gaze.
   */
	public bool CheckGazeConditions() {

    //always returns true, gaze conditions always met
    return true;

	}

  /*
   * Required method for use with the Gaze System, based on
   * implementation of Gazeable. This method will run when
   * the object is looked at and all gaze conditions are
   * met.
   *
   * In this example, the method will change the color of the
   * object the script is attached to
   */
	public void Activate() {

    //randomizes object color
	  RandomizeColor();

	}

  /*
   * Required method for use with the Gaze System, based on
   * implementation of Gazeable. This method will run when
   * the object has already been activated, and the user then
   * looks away. 
   */
  public void Deactivate() { 


  }
 
  /*
   * Sets a random color on this object
   */
	void RandomizeColor() {

    //creates a random color
    Color randomColor = new Color(Random.Range(0,1f),
	                                Random.Range(0,1f),
	                                Random.Range(0,1f));


    //creates a new material based on current object's material
	  Material newMaterial = new Material(this.GetComponent<MeshRenderer>().material);

    //sets new material to a random color
	  newMaterial.SetColor("_Color", randomColor);

    //updates current material
    this.GetComponent<MeshRenderer>().material = newMaterial;
    
	}
}
