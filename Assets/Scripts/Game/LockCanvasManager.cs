﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockCanvasManager : MonoBehaviour 
{
	public Text timeText;
	public Text countdownText;
	public Text endText;

	[SerializeField]
	private List<GameObject> buttonObjs;

	[SerializeField]
	private Text skillText;
	[SerializeField]
	private Text pinsText;
	[SerializeField]
	private Text lockLevelText;

    [SerializeField]
    private GameObject howToPlayContainer;


	public void SetTimeText(float currTime)
	{
		timeText.text = Mathf.Floor(currTime).ToString();
	}

	public void SetButtonsActive(bool isActive)
	{
		for(int i = 0; i < this.buttonObjs.Count; i++)
		{
			this.buttonObjs[i].SetActive(isActive);
		}
	}

	public void SetSkillText(int skill)
	{
		//Debug.Log("SKILL" + skill.ToString());
		this.skillText.text = skill.ToString();
	}

	public void SetPinsText(int pins)
	{
		this.pinsText.text = pins.ToString();
	}

	public void SetLockLevelText(string level)
	{
		this.lockLevelText.text = level;
	}


    public void OnHowToPlayClick(bool _isActive)
    {
        this.howToPlayContainer.SetActive(_isActive);
    }

    public void OnVolumeClick()
    {
        AudioManager.Instance.ToggleAudioMute();

    }
}
