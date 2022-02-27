using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamestart : MonoBehaviour
{

	[SerializeField] CanvasGroup overlay;
	// Our Startscreen GUI

	public void startEasy()
	{
		print("Starting game");
		StartCoroutine(Transition(0));
    }

	public void startNormal()
	{
		print("Starting game");
		StartCoroutine(Transition(1));
	}

	public void startHard()
    {
		print("Starting hard game");
		StartCoroutine(Transition(2));
	}

	IEnumerator Transition(int difficulty)
    {
		while (overlay.alpha < 1)
		{
			print(overlay.alpha);
			overlay.alpha = overlay.alpha + .007f * 1.2f;
			yield return null;
		}
		switch (difficulty)
        {
			case 0:
                gamestate.Instance.startEasyState();
                break;
			case 1:
				gamestate.Instance.startNormalState();
				break;
			case 2:
				gamestate.Instance.startHardState();
				break;
		}


	}





}

