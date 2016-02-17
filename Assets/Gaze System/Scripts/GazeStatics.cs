/*
 * Author: Connor Smith
 * Version: 3.0
 * Date: 02/16/16
 */

using UnityEngine;
using System.Collections;

/*
 * Provides static variables for use with the gaze
 * system. Yay singletons.
 */
public class GazeStatics : MonoBehaviour {

  //the cursor animator object, to be dragged in
  public Animator cursorAnimatorObj;

  //the cursor animation object, to be dragged in
  public AnimationClip cursorAnimationObj;

  //static cursor Animator for use with Gaze System
  public static Animator cursorAnimator;

  //static cursor Animation for use with Gaze System
  public static AnimationClip cursorAnimation;

	// Use this for initialization
	void Start () {

    //sets static variables based on dragged values
    cursorAnimator = cursorAnimatorObj;
    cursorAnimation = cursorAnimationObj;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
