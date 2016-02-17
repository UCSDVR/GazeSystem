/* Author: Connor Smith
 * Version: 3.0
 * Date: 02/15/16
 */

using UnityEngine;
using System.Collections;

/* This interface must be implemented by any script
 * on an object that should be activated when looked at
 */
public interface Gazeable {

  /*
   * CheckGazeConditions should return a boolean value
   * if all conditions to activate the object when looked at
   * been cleared. This method will be called from inside the
   * bundled GazeSystem, so there is no need to manually call it.
   * Just ensure that it returns false if the object should not
   * yet activate when looked at, and true if it should.
   */
  bool CheckGazeConditions();

  /*
   * Activate is the method that will actually activate the
   * object when it's looked at and when the gaze timer has
   * completed. Whatever is written inside this method will
   * be the gaze action. This method is called automatically
   * from the bundled GazeSystem, so there is no need to
   * manually call it
   */
  void Activate();

  /*
   * Deactivate will be called whenever a Gazeable object
   * has alerady been activated, and the very first time 
   * the user looks away from that activated object. Whatever
   * is written inside this method will therefore run when
   * the object is looked away from after activation
   */
  void Deactivate();

}
