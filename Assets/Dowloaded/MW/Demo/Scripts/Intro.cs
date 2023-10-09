using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	
    [SerializeField]
    private Image customImage;

    

	

	void OnTriggerEnter()
	{


        customImage.enabled = true;
	}

	void OnTriggerExit()
	{


        customImage.enabled = false;
    }

	

}
