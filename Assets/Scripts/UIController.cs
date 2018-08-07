using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameObject phone;
    PhoneBehaviour phoneScript;

    Text title;
    Text infoBar;
    Text subtitle;
    Font defaultFont;
    public Font shatteredFont;


    public int defaultFontSize = 60;

    // Use this for initialization
	void Start () {

        phoneScript = phone.GetComponent<PhoneBehaviour>();
        title = GameObject.Find("Title").GetComponent<Text>();
        infoBar = GameObject.Find("InfoBar").GetComponent<Text>();
        subtitle = GameObject.Find("Subtitle").GetComponent<Text>();

        defaultFont = title.font;
	}
	
	// Update is called once per frame
	void Update () {

        subtitle.enabled = false;

        title.fontSize = defaultFontSize;
        infoBar.fontSize = defaultFontSize;
        subtitle.fontSize = defaultFontSize;

        if (!phoneScript.isFlying && !phoneScript.onGround && !phoneScript.inspecting)
        {
            title.font = defaultFont;
            title.text = "Swipe up to throw";
        }
        if (phoneScript.isFlying)
        {
            title.text = "";
            float height = Mathf.Floor(phone.transform.position.y * 100) / 100;
            infoBar.text = height.ToString() + " m";
        }
        if (phoneScript.onGround)
        {
            title.fontSize = defaultFontSize + 10;
            title.text = "Tap the phone to see your score";
            infoBar.text = "";

        }


        if (phoneScript.inspecting)
        {
            subtitle.enabled = true;

            if (phoneScript.isCracked)
            {
                title.fontSize = defaultFontSize * 2 - 10;
                title.font = shatteredFont;
                title.text = "Shattered!";
            } else
            {
                title.fontSize = defaultFontSize * 2;
                title.text = "Safe!";
            }

            if (phoneScript.hiScore > 0)
            {
                float hiScore = Mathf.Floor(phoneScript.hiScore * 100f) / 100f;
                infoBar.text = "Hi Score " + hiScore.ToString() + " m";
            }
            

            

        }
    }
}
