using UnityEngine;
using System.Collections;

public class GazeDebug : MonoBehaviour {

  private TextMesh debugText; 

  private GameObject gazeObject;
  private bool isGazeable;
  private bool gazeConditions;

	// Use this for initialization
	void Start () {

    debugText = this.GetComponent<TextMesh>();
   
    gazeObject = null;
    isGazeable = false;
    gazeConditions = false;
	
	}
	
	// Update is called once per frame
	void Update () {
  
	
	}

  public void SetGazeObject(GameObject gazeObject) {

    this.gazeObject = gazeObject;
    UpdateDebugText();

  }

  public void SetIsGazeable(bool isGazeable) {

    this.isGazeable = isGazeable;
    UpdateDebugText();

  }

  public void SetGazeConditionStatus(bool gazeConditions) {

    this.gazeConditions = gazeConditions;
    UpdateDebugText();

  }

  public void UpdateDebugText() {

    string objectString = "Nothing";
    string gazeableString = "N/A";
    string gazeConditionsString = "N/A";

    if (gazeObject) {

      objectString = gazeObject.name;

      if (isGazeable) {

        gazeableString = "Yes";
        gazeConditionsString = gazeConditions.ToString();

      }

      else {

        gazeableString = "No";

      }
    }


    debugText.text = "Looking At: " + objectString + "\n" +
                "Is Gazeable: " + gazeableString + "\n" +
                "Gaze Conditions: " + gazeConditionsString;

  }
}
