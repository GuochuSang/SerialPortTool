using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
	public static MainManager instance;
	public HexFile hexFile = new HexFile();
	public void Awake()
	{
		instance= this;	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
