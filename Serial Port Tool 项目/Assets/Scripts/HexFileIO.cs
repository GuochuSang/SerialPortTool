using System;
using System.IO;
using UnityEngine;

public class HexFileIO:Singleton<HexFileIO>
	{
		public static HexFile ReadHexFile(string path,out string s)
		{
			HexFile hexFile = new HexFile();
			 s = "";
			Debug.Log("Hello");
			if (!System.IO.File.Exists(path))
			{
				s = "不存在相应的目录";
				throw new Exception("目录文件不存在");
			}
			else
			{
				using (var stream = File.OpenRead(path))
				{
					hexFile.hexData = new byte[stream.Length];
					stream.Read(hexFile.hexData, 0, hexFile.hexData.Length);
				}

				Debug.Log(hexFile.hexData.Length);
				return hexFile;
			}
		}
	}