using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour 
{
	// Game difficulty
	public enum Difficulty
	{
		easy = 0, 
		medium,
		hard
	}
	public Difficulty gameDifficulty = Difficulty.easy;

	// Refs to player stuff
	private PlayerData playerData;
	private PlayerInput playerInput;

	// While the game is running
	private bool isRunning = false;

	// Game time
	[SerializeField]
	private List<float> baseTimeLimits;
	private float gameTimeLimit;
	private float currentTime;

	// Lock stuff
	[SerializeField]
	private Lock gameLock;

	// UI
	[SerializeField]
	private LockCanvasManager canvasManager;

	// gettin lazy
	private int numberOfPins = 3;
	[SerializeField]
	private int skillIncrease = 10;
	[SerializeField]
	private float bonusPinTime = 10.0f;

	//some txt crap
	[SerializeField]
	private List<string> winnerTextList;
	[SerializeField]
	private List<string> timeupTextList;
	[SerializeField]
	private List<string> gameoverTextList;


	#region Main

	private IEnumerator Start()
	{
		// Game Lock
		this.gameLock.gameManager = this;

		// Player data & input
		this.playerData = this.GetComponent<PlayerData>();
		this.playerInput = this.GetComponent<PlayerInput>();
		this.playerInput.isInputEnabled = false;

		// Turn off lock
		gameLock.gameObject.SetActive(false);

		// Update canvas
		this.UpdateCanvasGameText(playerData.GetExperience(), this.numberOfPins, "");

		yield return null;
	}

	private IEnumerator SetupGame()
	{
        //turn off buttons and enable text
        this.canvasManager.SetButtonsActive(false);
        this.canvasManager.timeText.enabled = true;

        // Turn on lock
        gameLock.gameObject.SetActive(true);

		// Set up lock - set lock sweet spot, reset pin rotation
		gameLock.SetupLock(this.gameDifficulty);

		// set game parameters
		this.gameTimeLimit = this.baseTimeLimits[(int)this.gameDifficulty] + ((int)playerData.GetPlayerSkillLevel() * 5.0f); // increase time by player skill level (0-2) multiplied by 5

		// update ui
		this.UpdateCanvasGameText(playerData.GetExperience(), this.numberOfPins, this.DifficultyToString(this.gameDifficulty));

		this.StartCoroutine(this.RunGame());

		yield return null;
	}

	// For button click
	public void StartGame(int difficulty)
	{
		this.SetGameDifficulty((Difficulty)difficulty);
		this.StartCoroutine(this.SetupGame());
	}

	// Run the simulation
	private IEnumerator RunGame()
	{
		// keep track of time
		currentTime = this.gameTimeLimit;

		// switch flags
		isRunning = true;

		// enable input
		this.playerInput.isInputEnabled = true;
		this.playerInput.isForcing = false;

		yield return null;
	}

	// End the simulation (Enable UI, increase stats on success)
	public IEnumerator EndGame(bool isSuccesful, bool isOutOfPins)
	{
		if(isRunning)
		{
			isRunning = false;
			playerInput.isInputEnabled = false;

			// Check for succesful win
			if(isOutOfPins)
			{
				// reset player experience
				this.playerData.ResetExperience();

				// reset pins
				this.numberOfPins = 3;

				// show failure
				this.canvasManager.endText.text = this.gameoverTextList[Random.Range(0, this.gameoverTextList.Count)];
			}
			else if(isSuccesful)
			{
				// check for bobby pin increase (completed within x seconds)
				if(this.gameTimeLimit - this.currentTime < bonusPinTime)
				{
					//Debug.Log("Time: " + (this.gameTimeLimit - this.currentTime).ToString());
					numberOfPins++;
				}

				// Increase player xp
				playerData.IncreaseExperience(skillIncrease);

				// show success
				this.canvasManager.endText.text = this.winnerTextList[Random.Range(0, this.winnerTextList.Count)];
			}
			else
			{
				// show failure
				this.canvasManager.endText.text = this.timeupTextList[Random.Range(0, this.timeupTextList.Count)];
			}

			this.UpdateCanvasGameText(playerData.GetExperience(), this.numberOfPins, "");
			this.canvasManager.endText.enabled = true;

			float count = 3.0f;
			int dumbInt = 0;
			while(count > 0.0f)
			{
				dumbInt++;
				if(dumbInt > 3)
				{
					dumbInt = 0;
					this.canvasManager.endText.enabled = !this.canvasManager.endText.enabled;
				}

				count -= Time.deltaTime;
				yield return null;
			}

			this.canvasManager.endText.enabled = false;

            // turn lockn off
            this.gameLock.gamePin.ResetPin();
            this.gameLock.gameObject.SetActive(false);

			// turn difficulty select back on and time off
			this.canvasManager.timeText.enabled = false;
			this.canvasManager.SetButtonsActive(true);
		}

		yield return null;
	}

	private void Update()
	{
		if(isRunning)
		{
			// decrease time
			currentTime -= Time.deltaTime;

			// update UI
			canvasManager.SetTimeText(currentTime);

			if(currentTime <= 0.0f)
			{
				this.StartCoroutine(this.EndGame(false, false));
			}
		}
	}

	#endregion

	#region PlayerAction

	public void MovePin(bool isClockwise)
	{
		this.gameLock.gamePin.RotatePin(isClockwise);
	}

	public void MoveLock()
	{
		this.gameLock.RotateLock();
	}

	public void ReleasePressure()
	{
		this.gameLock.ReleasePressure();
	}

	// disable input, break the pin, update ui, enable input
	public IEnumerator BreakPin()
	{
		playerInput.isInputEnabled = false;

        // TODO: launch pin
        this.gameLock.gamePin.LaunchPin();

		// dec count
		this.numberOfPins--;

		// update ui
		this.UpdateCanvasGameText(playerData.GetExperience(), this.numberOfPins, this.DifficultyToString(this.gameDifficulty));

		// check if it was the last pin
		if(this.numberOfPins <= 0)
		{
			this.StartCoroutine(this.EndGame(false, true));
		}
		else
		{
			// UI Countdown
			float count = 4.0f;
			this.canvasManager.countdownText.text = count.ToString();
			this.canvasManager.countdownText.enabled = true;

			while(count > 1.0f)
			{
				count -= Time.deltaTime;
				this.canvasManager.countdownText.text = Mathf.Floor(count).ToString();
				yield return null;
			}

            this.gameLock.gamePin.ResetPin();

			this.canvasManager.countdownText.enabled = false;
			playerInput.isInputEnabled = true;
			this.playerInput.isForcing = false;
		}

		yield return null;
	}

	#endregion

	#region Get/Set

	public void SetGameDifficulty(Difficulty d)
	{
		this.gameDifficulty = d;
	}

	#endregion

	#region Canvas

	private void UpdateCanvasGameText(int skill, int pins, string level)
	{
		// canvas
		this.canvasManager.SetSkillText(skill);
		this.canvasManager.SetPinsText(pins);
		this.canvasManager.SetLockLevelText(level);
	}

	#endregion

	private string DifficultyToString(Difficulty d)
	{
		string str;
		switch(d)
		{
		case Difficulty.easy:
			str = "EASY";
			break;
		case Difficulty.medium:
			str = "MEDIUM";
			break;
		case Difficulty.hard:
			str = "HARD";
			break;
		default:
			str = "WTF M8";
			break;
		}

		return str;
	}
}
