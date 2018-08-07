using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Camera cam;
    public GameObject phoneObject;
    PhoneBehaviour phoneScript;

    

	// Use this for initialization
	void Start () {
        cam = GameObject.FindObjectOfType<Camera>();
        phoneScript = phoneObject.GetComponent<PhoneBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {

    }
}
