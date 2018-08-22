using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMainManager : MonoBehaviour
{
	public InputField baudRate;
	public Dropdown port;
	public Dropdown parity;
	public Dropdown stopBits;
	public InputField dataBits;
	public Animator ConnectButtonAnimator;
	public static  UIMainManager instance;
	public EventSystem es;
	public List<string> HexStrings = new List<string>();
	public int page;
	public Image right, left;
	public Text pageText;
	public Button Send;
	public void NextPage()
	{
		if (page < HexStrings.Count - 1)
		{
			page++;
			pageText.text = (page + 1).ToString() + "/" + (HexStrings.Count).ToString();
			HexArea.text = HexStrings[page];
			RefreshPageButton();

		}
	}
	public void LastPage()
	{
		if (page >=0)
		{
			page--;
			pageText.text = (page + 1).ToString() + "/" + (HexStrings.Count).ToString();
			HexArea.text = HexStrings[page];
			RefreshPageButton();
		}
	}

	public void RefreshPageButton()
	{
		if (page >= HexStrings.Count - 1)
		{
			right.color = Color.gray;
			right.GetComponent<Button>().interactable = false;
		}
		else
		{
			right.color = Color.white;
			right.GetComponent<Button>().interactable = true;


		}
		if (page <=0)
		{
			left.color = Color.gray;
			left.GetComponent<Button>().interactable = false;

		}
		else
		{
			left.GetComponent<Button>().interactable = true;
			left.color = Color.white;
		}
	}

	public void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	public void BaudRateConfirm()
	{
		SerialPortManager.instance.baudRate = int.Parse(baudRate.text);
	}

	public void PortConfirm()
	{
		SerialPortManager.instance.port = port.options[port.value].text;
	}

	public void ParityConfirm()
	{
		SerialPortManager.instance.parity =(Parity)Enum.ToObject(typeof(Parity), parity.value);
	}

	public void StopBitsConfirm()
	{
		SerialPortManager.instance.stopBits= (StopBits)Enum.ToObject(typeof(StopBits), stopBits.value);
	}

	public void DataBitsConfirm()
	{
		SerialPortManager.instance.dataBits = int.Parse(dataBits.text);
	}

	public void OnButtonClick()
	{
		if (SerialPortManager.instance.sp != null && SerialPortManager.instance.sp.IsOpen)
			SerialPortManager.instance.ClosePort();
		else
			SerialPortManager.instance.OpenPort();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (SerialPortManager.instance.sp!=null&&SerialPortManager.instance.sp.IsOpen)
		{
			ConnectButtonAnimator.SetBool("connect", true);
		}
		else
		{
			ConnectButtonAnimator.SetBool("connect", false);

		}

		if (es.currentSelectedGameObject == ConnectButtonAnimator.gameObject||es.currentSelectedGameObject==Send.gameObject)
		{
			es.SetSelectedGameObject(port.gameObject);
		}
	}
	public void OpenProject()
	{
		OpenFileDlg pth = new OpenFileDlg();
		pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
		pth.filter = "十六进制文件(*.hex)\0*.hex\0\0";
		pth.file = new string(new char[256]);
		pth.maxFile = pth.file.Length;
		pth.fileTitle = new string(new char[64]);
		pth.maxFileTitle = pth.fileTitle.Length;
		pth.initialDir = Application.dataPath + @"/GameData/Rooms";
		pth.title = "打开hex项目";
		pth.defExt = "hex";
		pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
		if (OpenFileDialog.GetOpenFileName(pth))
		{
			string filepath = pth.file;//选择的文件路径;  
			Debug.Log(filepath);
			string s = "";
			MainManager.instance.hexFile = HexFileIO.ReadHexFile(filepath, out s);
			ShowHex();
		}
	}
	public Text HexArea;
	public void ClearHexFile()
	{
		MainManager.instance.hexFile = null;
	}
	public void ClearHexArea()
	{
		//MainManager.instance.hexFile = null;
		HexStrings.Clear();
		HexArea.text="";
	}

	public void ShowHex()
	{
		ClearHexArea();
		List<string> Texts = new List<string>();
		int i = 1;
		int page = 0;
		int count = 0;
		string s = "";
		foreach (var b in MainManager.instance.hexFile.hexData)
		{
			s += b.ToString("X2")+" ";
			if (i % 8 == 0)
				s += "\n";
			i++;
			count++;
			if (count % 400 == 0)
			{
				page++;
				string a = s;
				Texts.Add(a);
				s = "";
			}
		}
		HexStrings = Texts;
		HexArea.text = HexStrings[0];
		page = 0;
		pageText.text = (page + 1).ToString() + "/" + (HexStrings.Count).ToString();
		RefreshPageButton();
	}
}

