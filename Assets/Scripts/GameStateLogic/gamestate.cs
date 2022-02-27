
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
public class gamestate : MonoBehaviour
{


	// Declare properties
	[SerializeField] static gamestate instance;
	[SerializeField] static keyTracker keyTrackerInstance;
	[SerializeField] int currentHP;
	[SerializeField] int maxHidingUses;
	[SerializeField] int candleDuration;
	[SerializeField] string candleStatus;
	[SerializeField] int lanternsRemaining;
	[SerializeField] bool isPlayerHidden;
	[SerializeField] bool isPlayerInvincible;
	[SerializeField] int zombieDamange;
	[SerializeField] int skeletonDamage;
	[SerializeField] int ghostDamage;
	[SerializeField] Text infoText;
	[SerializeField] string state; 

	// Store lower bound at index 0, and higher bound at index 1
	private float[] lightningFrequency;



	// ---------------------------------------------------------------------------------------------------
	// gamestate()
	// --------------------------------------------------------------------------------------------------- 
	// Creates an instance of gamestate as a gameobject if an instance does not exist
	// ---------------------------------------------------------------------------------------------------
	public static gamestate Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("gamestate").AddComponent<gamestate>();
				instance.state = "menu";
				print("created in the instance!");
			}

			return instance;
		}
	}

	public void assignInfoText(Text text)
	{
		infoText = text;
	}

	public int getZombieDamage()
    {
		return this.zombieDamange;
    }

	public int getSkeletonDamage()
	{
		return this.skeletonDamage;
	}

	public int getGhostDamage()
	{
		return this.ghostDamage;
	}

	public void displayMessage(string text, int duration)
    {
		StartCoroutine(displayMessageCoroutine(text, duration));
    }

	IEnumerator displayMessageCoroutine(string text, int duration)
    {
		print(text);
		GameObject.FindGameObjectWithTag("PopupText").GetComponent<Text>().text = text;
		yield return new WaitForSeconds(duration);
		GameObject.FindGameObjectWithTag("PopupText").GetComponent<Text>().text = "";
	}
	public void OnApplicationQuit()
	{
		state = "menu";
	}

	public void setRunning()
    {
		state = "running";
    }

	public bool isRunning()
    {
		return (state == "running");
    }

	public void setPaused()
	{
		state = "paused";
	}

	public bool isPaused()
    {
		return (state == "paused");
    }
	public int getCandleDuration()
    {
		return candleDuration;
    }

	public string getCandleStatus()
    {
		return candleStatus;
    }

	public void setCandleStatus(string status)
    {
		candleStatus = status; 
    }

	public float getRandomLightningDelay()
    {
		return Random.Range(lightningFrequency[0], lightningFrequency[1]);
    }

	public int getMaxHideAttempts()
    {
		return maxHidingUses;
    }

	public int getCurrentHP()
    {
		return currentHP;
    }

	public void inflictDamage(int damageValue, bool considerInvincibility)
    {
		if (considerInvincibility && !isPlayerInvincible)
		{
			StartCoroutine(PlayerInvincibility(damageValue));
		}
		else if (!considerInvincibility)
		{
			currentHP -= damageValue;
		}
    }

	public void inflictDamage(int damageValue, float invincibilityTimer, bool considerInvincibility)
	{
		if (considerInvincibility && !isPlayerInvincible)
		{
			StartCoroutine(PlayerInvincibility(damageValue, invincibilityTimer));
		}
		else if (!considerInvincibility)
		{
			currentHP -= damageValue;
		}
	}

	public bool getPlayerInvincible()
    {
		return isPlayerInvincible;
    }

	public int getLanternsRemaining()
    {
		return lanternsRemaining;
    }

	public void incrementLanternsRemaining()
    {
		lanternsRemaining++;
    }

	public void decrementLanternsRemaining()
    {
		lanternsRemaining--;
    }

	public bool getIsPlayerHidden()
    {
		return isPlayerHidden;
    }

	public void setIsPlayerHidden(bool input)
    {
		isPlayerHidden = input;
    }

	public keyTracker GetKeyTracker()
    {
		return keyTrackerInstance;
    }
	// Logic for win and lose states
	public bool isCharacterAlive()
    {
		return currentHP > 0;
    }

	public bool candleTimerRanOut()
    {
		return CandleTimerScript.timerStop;
    }


	public bool allCandlesLit()
    {
		return lanternsRemaining <= 0;
    }

	public void returnToMenu()
    {
		state = "menu";
		SceneManager.LoadScene("MenuScene");
	}


    public void startEasyState()
	{
		print("Creating a new game state");
		currentHP = 100;
		candleDuration = 300;
		candleStatus = "lit";
		isPlayerInvincible = false;
		lanternsRemaining = 40;
		maxHidingUses = 3;
		isPlayerHidden = false;
		ghostDamage = 10;
		skeletonDamage = 0;
		zombieDamange = 0;
		lightningFrequency = new float[] { 7, 10 };
		keyTrackerInstance = keyTracker.Instance;
		keyTrackerInstance.startEasyState();
		StartCoroutine(LoadScene("Easy"));

	}

	public void startNormalState()
	{
		print("Creating a new game state");

		currentHP = 100;
		candleDuration = 300;
		candleStatus = "lit";
		isPlayerInvincible = false;
		lanternsRemaining = 45;
		maxHidingUses = 3;
		isPlayerHidden = false;
		ghostDamage = 10;
		skeletonDamage = 10;
		zombieDamange = 0;
		lightningFrequency = new float[] { 7, 10 };
		keyTrackerInstance = keyTracker.Instance;
		keyTrackerInstance.startEasyState();
		StartCoroutine(LoadScene("Normal"));

	}

	public void startHardState()
	{
		print("Creating a new hard game state");

		currentHP = 100;
		candleDuration = 300;
		candleStatus = "lit";
		isPlayerInvincible = false;
		lanternsRemaining = 52;
		maxHidingUses = 3;
		isPlayerHidden = false;
		ghostDamage = 15;
		skeletonDamage = 15;
		zombieDamange = 12;
		lightningFrequency = new float[] { 7, 10 };
		keyTrackerInstance = keyTracker.Instance;
		keyTrackerInstance.startEasyState();
		StartCoroutine(LoadScene("Hard"));

	}



	IEnumerator LoadScene(string difficulty)
	{
		yield return null;

		//Begin to load the Scene you specify
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(difficulty);
		//Don't let the Scene activate until you allow it to
		asyncOperation.allowSceneActivation = false;
		while (!asyncOperation.isDone)
		{
			// Check if the load has finished
			if (asyncOperation.progress >= 0.9f)
			{
					asyncOperation.allowSceneActivation = true;
			}

			yield return null;
		}
		print("Loaded, assigning doors");
        AssignDoors(difficulty);

    }

	private void AssignDoors(string difficulty)
    {
		GameObject[] doors = GameObject.FindGameObjectsWithTag("DoorwayRandom");
		GameObject[] orderedDoors = new GameObject[doors.Length];
		print("number of doors is " + doors.Length);
		foreach (GameObject door in doors)
        {
			doorwayRandom script = door.GetComponent<doorwayRandom>();
			orderedDoors[script.doorNumber - 1] = door;
        }

		foreach (GameObject door in orderedDoors)
		{
			doorwayRandom script = door.GetComponent<doorwayRandom>();
			print(script.doorNumber);
		}

		//Pick a random map, and read the JSON
		int num = Random.Range(1, 4);
		print("Loading map " + difficulty + num);
        StreamReader r = new StreamReader(Application.streamingAssetsPath + "/" + difficulty + num + ".json");
		string json = r.ReadToEnd();
		JSONEntry[] fileDoors = JsonHelper.FromJson<JSONEntry>(json);
		// Set the doors
		bool[] activeKeys = new bool[doors.Length];
		int i = 0;
        foreach (GameObject door in orderedDoors)
		{
			print(i);
			doorwayRandom script = door.GetComponent<doorwayRandom>();
			script.keyNumber = fileDoors[i].key;
			if (script.keyNumber != -1)
            {
				activeKeys[script.keyNumber - 1] = true;
				string color = gamestate.Instance.GetKeyTracker().getKey(fileDoors[i].key).getColor();

				// Get the Sprite Renderer of the door and adjust the size
				SpriteRenderer sr = door.GetComponent<SpriteRenderer>();
				sr.sprite = Resources.Load<Sprite>("Custom Sprites/Doors/door_" + color);
				sr.size = new Vector2(2, 2);
			}
			int destination = fileDoors[i].door;
			if (destination != -1)
			{
				script.destination = orderedDoors[destination - 1];
			}
			i++;
		}

		GameObject[] keys = GameObject.FindGameObjectsWithTag("KeyRandom");
		GameObject[] orderedKeys = new GameObject[keys.Length];
		print("number of keys is " + keys.Length);

		// Set the keys
		foreach (GameObject key in keys)
        {
			keyRandom script = key.GetComponent<keyRandom>();
			orderedKeys[script.keyNumber - 1] = key;
        }

		GameObject[] boxes = GameObject.FindGameObjectsWithTag("InventoryBox");
		int j = 0;
		i = 0;
		foreach (GameObject key in orderedKeys)
        {
			if (activeKeys[i])
			{
				key.SetActive(true);
				Instance.GetKeyTracker().getKey(i + 1).setActive(true);
				boxes[j].GetComponent<InventoryScript>().keyNumber = i + 1;
				j++;
			}
            else
            {
				key.SetActive(false);
            }
			i++;
        }




	}

	IEnumerator PlayerInvincibility(int damageValue)
	{
		this.isPlayerInvincible = true;
		currentHP -= damageValue;
		yield return new WaitForSeconds(3);
		this.isPlayerInvincible = false;
	}

	IEnumerator PlayerInvincibility(int damageValue, float timeInSeconds)
	{
		this.isPlayerInvincible = true;
		currentHP -= damageValue;
		yield return new WaitForSeconds(timeInSeconds);
		this.isPlayerInvincible = false;
	}

	private void Update()
    {
		switch(state)
        {
			case ("menu"):
				Time.timeScale = 0f;
				break;

			case ("paused"):
				Time.timeScale = 0f;
				break;

			case ("running"):
				Time.timeScale = 1f;
				break;

        }
	}

}
public static class JsonHelper
{
	public static T[] FromJson<T>(string json)
	{
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
		return wrapper.Items;
	}

	public static string ToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.Items = array;
		return JsonUtility.ToJson(wrapper);
	}

	public static string ToJson<T>(T[] array, bool prettyPrint)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.Items = array;
		return JsonUtility.ToJson(wrapper, prettyPrint);
	}

	[System.Serializable]
	private class Wrapper<T>
	{
		public T[] Items;
	}
}
[System.Serializable]
public class Player
{
	public string playerId;
	public string playerLoc;
	public string playerNick;
}

[System.Serializable]
public class JSONEntry
{
	public int door;
	public int key;
}