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

  //the gaze timer is a visual circular timer on the cursor that
  //shows a user that they're activating an object, and how long
  //they have until it's activated.
  //defaults to enabled
  public bool gazeTimer = true;

  //the timer duration is the length. in seconds, of the gaze timer
  public float timerDuration = 2;

  //rapid activation repeatedly calls the Activate() method on gaze
  //defaults to disabled
  public bool rapidActivation = false;


  //the actual cursor animation, for use in finding animation length
  private AnimationClip cursorAnimation;

  //the animator on the cursor, for use in resetting and starting cursor
  private Animator cursorAnimator; //cursor animation

  private Gazeable[] gazeScripts;

  //checks if animation was activated 
  private bool cursorActivated = false;

  //for use with rapid activation, allows object to keep activating
  private bool objectActivated = false;

  //how fast the cursor animation should play
  //value is calculated dynamically
  private float timerSpeed;

  //Use this for initialization
  void Start () {

    //sets cursor animator and animation variables
    cursorAnimation = GazeStatics.cursorAnimation;
    cursorAnimator = GazeStatics.cursorAnimator;

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
  	  gazeScripts = (Gazeable[])interactableObject.collider.GetComponents<Gazeable>();

  	  //if there were any gaze scripts found
  	  if (gazeScripts.Length > 0) {
  	  
	      //if cursor isn't active and gaze conditions are met
	      if (!cursorActivated && SearchGazeConditions(gazeScripts)) {

	        //if the gaze timer is enabled, activate it
	        if (gazeTimer) {

	          //activate the cursor, starts the animation
	          ActivateCursor();

	        }

	        //if the gaze timer is disabled, just activate object
	        else {

            //if the object hasn't already been activated OR
            //if rapid activation is enabled
            if (!objectActivated || rapidActivation) {

              //activates all gaze scripts on the object
	            ActivateGazeObject(gazeScripts);
            } 
	        }
	      }

	      //if cursor isn't active and gaze conditions are not met
	      //Usually occurs when cursor just finished and conditions change
	      else if (!cursorActivated && !SearchGazeConditions(gazeScripts)) {

	        //stop the cursor, cursor hasn't completed
	        ResetCursor(false);

	      }

	      //if cursor is active and gaze conditions are not met
	      //Usually occurs when gaze conditions suddenly change during cursor
	      else if (cursorActivated && !SearchGazeConditions(gazeScripts)) {

          //stop the cursor, cursor hasn't completed
	        ResetCursor(false);

	      }
       
	      //if the cursor has reached 95% completion (Allows for human error)
        // OR
        //if the object has already been activated and rapid activation is enabled
        if (cursorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95 ||
           (objectActivated && rapidActivation)) {

	        //Activates the gaze object
	        ActivateGazeObject(gazeScripts);

	      }
	    }
  	  
  	  //deactivate the cursor animation if the user looks at something else
  	  else {

        //checks if the object needs to be deactivated
        CheckDeactivate(gazeScripts);
       
  	    //stop the cursor cursor hasn't completed
  	    ResetCursor(false);
     
  	  }
  	}
  	
	  //deactivate the cursor animation if the user looks at nothing
    else {

      //checks if the object needs to be deactivated
      CheckDeactivate(gazeScripts);

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
      cursorActivated = false;

      //user is no longer still looking at the object
      objectActivated = false;

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

    //object has now been activated
    objectActivated = true;

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
   * Deactivates an already activated object, by
   * individually deactivating each gaze script
   * on the object
   */
  void DeactivateGazeObject(Gazeable[] gazeScripts) {

    //checks every gaze script on the object
    foreach (Gazeable gazeObject in gazeScripts) {

      //calls the deactivate method on the object
      gazeObject.Deactivate();

    }
  }

  /*
   * Begins the cursor timer animation
   */
  void ActivateCursor() {
     
	  //starts the animation
	  cursorAnimator.speed = timerSpeed;
    
  	//cursor has been activated, won't loop again
	  cursorActivated = true;

  }

  /*
   * Checks if the user was looking at an Activated object
   * before looking away, and if so, deactivate the object.
   */
  void CheckDeactivate(Gazeable[] gazeScripts) {

    //if the object has already been activated
    if (objectActivated) {

      //deactivate the object
      DeactivateGazeObject(gazeScripts);

    }
  }
}
