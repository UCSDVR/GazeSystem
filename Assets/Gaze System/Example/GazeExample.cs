/*
 * Author: Connor Smith
 * Version: 3.0
 * Date: 02/16/16
 */

using UnityEngine;
using System.Collections;

public class GazeExample : MonoBehaviour, Gazeable {

  //Text Mesh component for the countdown example
  public TextMesh textTimer;

  //duration of countdown timer
  public int timerDuration;

  //in what interval the timer should count down
  public int timerInterval;

  //current value of the timer
  private int currentTimer;

  //string for the timer
  private string timerString = "You May Activate In ";

  //recorded game time for countdown timer
  private float lastRecordedTime;

	// Use this for initialization
	void Start () {

    //Stores the initial game time
	  lastRecordedTime = Time.time;

    //initializes the current timer value
	  currentTimer = timerDuration;

    //Updates the timer text
    textTimer.text = timerString + timerDuration;
	
	}
	
	// Update is called once per frame
	void Update () {

    //checks if there's any time left on the timer
	  if (currentTimer > 0) {

      //checks if the time interval has passed
	    if (Time.time - lastRecordedTime > timerInterval) {

        //decrements the timer
	      currentTimer--;

        //updates timer text
	      textTimer.text = timerString + currentTimer.ToString();

        //resets recorded time to current time
	      lastRecordedTime = Time.time;

	    }
	  }
	}

  /*
   * Required method for use with the Gaze system, based on
   * implementation of Gazeable. Will return true if all
   * conditions for gaze activation have been met, and
   * false if the Activate() method should not yet be called
   * on gaze.
   */
	public bool CheckGazeConditions() {

    //if the timer is at zero, gaze should activate
	  if (currentTimer == 0) {

	    return true;

	  }

    //if the timer isn't at zero, gaze shouldn't activate
	  return false;

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

    //resets the timer to start again
	  ResetTimer();

	}

  /*
   * Starts timer at beginning again
   */
	void ResetTimer() {

    //sets timer back to beginning
	  currentTimer = timerDuration;

    //updates text on the timer
    textTimer.text = timerString + timerDuration.ToString();

    //updates last recorded time to current time
    lastRecordedTime = Time.time;

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
