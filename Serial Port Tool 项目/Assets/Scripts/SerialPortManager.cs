using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using UnityEngine;
public class SerialPortManager : MonoBehaviour
{
	public static SerialPortManager instance;
	public void Awake()
	{
		instance = this;
	}
	public bool test;
     public string port = "COM3";//串口名
     public int baudRate = 9600;//波特率
     public Parity parity = Parity.None;//效验位
     public int dataBits = 8;//数据位
     public StopBits stopBits = StopBits.One;//停止位
     public SerialPort sp = null;
     Thread dataReceiveThread;
	public event Action<List<string>> OnPortRefresh;
	public List<string> avaliablePort = new List<string>();

	public List<byte> listReceive = new List<byte>();

	public void RefreshPort()
	{

		avaliablePort.Clear();
		foreach (string s in SerialPort.GetPortNames())
		{
			avaliablePort.Add(s);
		}

		if (avaliablePort == null)
			return;
		else if (OnPortRefresh != null) OnPortRefresh(avaliablePort);
		if (!avaliablePort.Contains(port))
			port = avaliablePort[0];
	}
	// Use this for initialization
		/// <summary>
		/// 初始化
		/// </summary>
	void Start ()
		{
			RefreshPort();
				// OpenPort();
		         dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
		         dataReceiveThread.Start();
	}
	/// <summary>
	/// 打开端口
	/// </summary>
	public void OpenPort()
	{
		ClosePort();
		sp = new SerialPort(port, baudRate, parity, dataBits, stopBits);
		sp.ReadTimeout = 400;
		         try
		         {
			             sp.Open();
			      }
		         catch (Exception ex)
		         {
			             Debug.Log(ex.Message);
			      }
		}
	// Update is called once per frame
	void Update()
	{
		if (test)
		{
			test = false;
			Test();
		}

		RefreshPort();
	}
	/// <summary>
	/// 退出时关闭端口
	/// </summary>
	void OnApplicationQuit()
		     {
		         ClosePort();
		     }
	/// <summary>
	/// 关闭端口
	/// </summary>
     public void ClosePort()
     {
         try
         {
             sp.Close();
             dataReceiveThread.Abort();
         }
         catch (Exception ex)
         {
             Debug.Log(ex.Message);
         }
	}
	/// <summary>
	/// 接收数据
	/// </summary>
	void DataReceiveFunction()
	{
		byte[] buffer = new byte[1];
		int bytes = 0;
		while (true)
		{
			if (sp != null && sp.IsOpen)
			{
				try
				{
					bytes = sp.Read(buffer, 0, buffer.Length);//接收字节
					if (bytes == 0)
					{
						continue;
					}
					else
					{
						for (int i = 0; i < buffer.Length; i++)
						{
							listReceive.Add(buffer[i]);
						}
					}
				}
				catch (Exception ex)
				{
					if (ex.GetType() != typeof(ThreadAbortException))
					{
					}
				}
			}
			Thread.Sleep(10);
		}
	}
	/// <summary>
	/// 串行发送一个字节数组
	/// </summary>
	/// <param name="b"></param>
	     public void WriteByte(byte[] b)
		     {
		         if (sp.IsOpen)
		         {
			         sp.Write(b, 0, b.Length);
		         }
		     }
	/// <summary>
	/// 测试用代码
	/// </summary>
	public void Test()
	{
		WriteByte(listReceive.ToArray());
	}

	public void SetBaudRate(int b)
	{
		baudRate = b;
	}
	public void SetBaudRate(string b)
	{
		baudRate = int.Parse(b);
	}
}
