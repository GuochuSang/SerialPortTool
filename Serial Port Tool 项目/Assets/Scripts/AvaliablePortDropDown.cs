using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvaliablePortDropDown : MonoBehaviour
{
	public Dropdown dropDown;

	public void RefreshDropdown(List<string> avaliablePort)
	{
		dropDown.ClearOptions();
		foreach (var s in avaliablePort)
		{
			dropDown.options.Add(new Dropdown.OptionData(s));
		}

		dropDown.RefreshShownValue();
		UIMainManager.instance.PortConfirm();
	}
	// Use this for initialization
	void Start ()
	{
		SerialPortManager.instance.OnPortRefresh += RefreshDropdown;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
