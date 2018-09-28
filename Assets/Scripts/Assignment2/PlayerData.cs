using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour 
{

	public enum SkillLevel
	{
		Amateur = 0,
		Proffessional,
		Master
	}

	public SkillLevel skillLevel;

	private List<int> experienceRanges = new List<int>() {50, 75, 100};
	private int totalExperience = 0;


	public SkillLevel GetPlayerSkillLevel()
	{
		// Check which range we're in
		if(totalExperience < experienceRanges[0])
		{
			skillLevel = SkillLevel.Amateur;
		}
		else if(totalExperience < experienceRanges[1])
		{
			skillLevel = SkillLevel.Proffessional;
		}
		else
		{
			skillLevel = SkillLevel.Master;
		}

		return skillLevel;
	}

	public void IncreaseExperience(int exp)
	{
		if(totalExperience + exp >= 100)
			totalExperience = 100;
		else
			totalExperience += exp;
	}

	public int GetExperience()
	{
		return totalExperience;
	}

	public void ResetExperience()
	{
		this.totalExperience = 0;
	}
}
