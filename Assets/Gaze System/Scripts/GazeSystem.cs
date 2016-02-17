/* 
 * Author: Connor Smith
 * Version: 3.0
 * Date: 02/15/16
 */

using UnityEngine;
using System.Collections;

/* This script is the gaze system that allows the user to
 * interact with objects simply by looking around.
 * To use, make sure that the interactable object has
 * a script tag called "InteractEye" on it, unless
 * otherwise specified
 */
public class GazeSystem : MonoBehaviour {

  //enables/disables the gaze cursor timer
  public bool gazeTimer = true;
  public bool rapidActivation = false;
  public float timerDuration = 2;

  [HideInInspector]
  public AnimationClip cursorAnimation; //actual cursor animation

  [HideInInspector]
  public Animator cursorAnimator; //cursor animation

  //checks if animation was activated 
  private bool alreadyActivated = false;

  //for use with rapid activation, allows object to keep activating
  private bool keepActivating = false;

  //how fast the cursor animation should play
  //value is calculated dynamically
  private float timerSpeed;

  //boolean tag on the animator
  private string animBool = "activateCursor";

  //Use this for initialization
  void Start () {

    //calculates speed of the timer based on duration in seconds
    timerSpeed = 1 / (timerDuration / cursorAnimation.length);

    //place the gaze system on the camera
    PlaceOnCamera();

    //stop cursor timer from auto-playing
    ResetCursor(false);
	
  }
	
  // Update is called once per frame
  void Update () {

    //performs the raycast
	  Gaze();
    
  }

  /*
   * Performs the actual gaze raycast, starts the cursor, and
   * activates the object when necessary
   */
  void Gaze() {

    //creates the ray to use for raycasting, goes forward
    Ray gazeRay = new Ray (this.transform.position, this.transform.forward);

    //variable to store the object the raycast hits
  	RaycastHit interactableObject;

  	//performs Raycast infinitely forward, stores hit objects into interactableObject
  	if (Physics.Raycast(gazeRay, out interactableObject, Mathf.Infinity)) {

  	  //stores all gaze scripts on the target object into an array
  	  Gazeable[] gazeScripts = (Gazeable[])interactableObject.collider.GetComponents<Gazeable>();

  	  //if there were any gaze scripts found
  	  if (gazeScripts.Length > 0) {
  	  
	      //if cursor isn't active and gaze conditions are met
	      if (!alreadyActivated && SearchGazeConditions(gazeScripts)) {

	        //if the gaze timer is enabled, activate it
	        if (gazeTimer) {

	          //activate the cursor, starts the animation
	          ActivateCursor();

	        }

	        //if the gaze timer is disabled, just activate object
	        else {

	          ActivateGazeObject(gazeScripts);

	          if (!rapidActivation) {

	            alreadyActivated = true;

	          }
	        }
	      }

	      //if cursor isn't active and gaze conditions are not met
	      //Usually occurs when cursor just finished and conditions change
	      else if (!alreadyActivated && !SearchGazeConditions(gazeScripts)) {

	        //stop the cursor, cursor hasn't completed
	        ResetCursor(false);

	      }

	      //if cursor is active and gaze conditions are not met
	      //Usually occurs when gaze conditions suddenly change during cursor
	      else if (alreadyActivated && !SearchGazeConditions(gazeScripts)) {

          //stop the cursor, cursor hasn't completed
	        ResetCursor(false);

	      }

	      //if the cursor has reached 95% completion (Allows for human error)
	      if (keepActivating || cursorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95) {

	        //Activates the gaze object
	        ActivateGazeObject(gazeScripts);

	      }
	    }
  	  
  	  //deactivate the cursor animation if the user looks at something else
  	  else {

  	    //stop the cursor cursor hasn't completed
  	    ResetCursor(false);

  	  }
  	}
  	
	  //deactivate the cursor animation if the user looks at nothing
    else {

      //stop the cursor, cursor hasn't completed
      ResetCursor(false);

	  }
  }
  
  /* 
   * Deactivates the cursor timer by setting animation speed to 0
   * and resetting the animation back to the beginning.
   */
  void ResetCursor(bool cursorCompleted) {

    //sets animation back to beginning
    cursorAnimator.Play (0);

    //if the cursor timer wasn't completed, allow cursor to activate again
    if (!cursorCompleted) {

      //cursor is no longer activated, can run again
      alreadyActivated = false;

    }

    //if rapid activation is enabled, stop any rapid activating
    if (rapidActivation && keepActivating) {

      //stops rapid activating
      keepActivating = false;

    }

    //prevent cursor animation from playing
    cursorAnimator.speed = 0;
 
  }

  /* 
   * Places the gaze system onto the main camera 
   */
  void PlaceOnCamera() {

    //sets the rotation of the system
    this.transform.rotation = Camera.main.transform.rotation;

    //sets the position of the system
    this.transform.localPosition = Camera.main.transform.position;

    //attaches to camera
    this.transform.parent = Camera.main.transform;

  }

  /* 
   * Checks if ANY gaze conditions on the object are met.
   * Used to determine if cursor timer should start
   */
  bool SearchGazeConditions(Gazeable[] gazeScripts) {

    //checks each gaze script on the object
    foreach (Gazeable gazeObject in gazeScripts) {

      //checks gaze conditions on the script
      if (gazeObject.CheckGazeConditions()) {

        //returns true if conditions are met
        return true;

      }
    }

    //returns false if conditions are not met
    return false;

  }

  /*
   * Activates the correct gaze scripts on the
   * target object.
   * Called when cursor timer has completed
   */
  void ActivateGazeObject(Gazeable[] gazeScripts) {

    //Resets the cursor if the object activates
    ResetCursor(true);

    //if rapid activation is enabled, allow object to keep activating
    if (rapidActivation && !keepActivating) {

      keepActivating = true;

    }

    //checks every gaze script on the object
    foreach (Gazeable gazeObject in gazeScripts) {

	  //checks if the object's gaze conditions have been met
	  if (gazeObject.CheckGazeConditions()) {

	    //activates the gazeable object
	    gazeObject.Activate();

      }
    }
  }

  /*
   * Begins the cursor timer animation
   */
  void ActivateCursor() {
     
	//starts the animation
	cursorAnimator.speed = timerSpeed;

	//cursor has been activated, won't loop again
	alreadyActivated = true;

  }
}
