using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
	//ALL PORTALS KNOW WHICH IS TARGET ONE
	static string targetportal;

	bool canPort;
	//default is false so you can't port before you enter the trigger zone
	[SerializeField]
	AudioClip doorSound;
	[SerializeField]
	string targetMapName;
	[SerializeField]
	string thisPortalName;
	[SerializeField]
	string targetPortalName;
	[SerializeField]
	bool playSound;
    [SerializeField]
    private Image customImage;

    

	void Start()
	{
		
		if (targetportal == thisPortalName)
		{
			GameObject Player = GameObject.FindGameObjectWithTag("Player");
			Player.transform.position = transform.position;
            
            if (playSound)
			{
				AudioSource.PlayClipAtPoint(doorSound, transform.position);
			}
		}
	}

	void OnTriggerEnter()
	{
		targetportal = targetPortalName;
		canPort = true;

        customImage.enabled = true;
	}

	void OnTriggerExit()
	{
		canPort = false;

        customImage.enabled = false;
    }

	void Update()
	{
		if (Input.GetKey(KeyCode.E) & canPort)
		{
			SceneManager.LoadScene(targetMapName);
		}
	}



}
