using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

// File Manipulate [3/7/2014 leo]

namespace SIP.Common
{
    public enum FileOverWriteFlag
    {
        AskBeforeOverWrite,
        OverWrite,
        OverWriteAll,
        Discart,
        DiscardAll,
    }

    public enum FileStatusCompare
    {

    }

    public class FileManipulate
    {

        /************************************************************************/
        // Date: 14/3/2014 
        // Author: Leo 
        // Description: Remove file.
        /************************************************************************/
        public static void RemoveFile(string path, string name)
        {
            string filePath = path + "//" + name;

            RuntimePlatform platform = Application.platform;

            if (IsFileExist(filePath))
            {
                if(platform == RuntimePlatform.IPhonePlayer)
                    File.Delete("/private" + filePath);
                else
                    File.Delete(filePath);
            }
        }

        public static string GetTempPath()
        {
            string path = "";

            return path;
        }

        /************************************************************************/
        // Date: 14/3/2014 
        // Author: Leo 
        // Description: Create empty file by giving file length.
        /************************************************************************/
        public static void CreateFile(string path, string name, int length)
        {
            string filePath = path + "//" + name;

            if (IsFileExist(filePath))
            {

            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                byte[] byteArray = new byte[length];
                fs.Write(byteArray, 0, byteArray.Length);
                fs.Close();
            }
        }

        /************************************************************************/
        // Date: 14/3/2014 
        // Author: Leo 
        // Description: Write or create a file by input data.
        /************************************************************************/
        public static void WriteFile(string path, string name, byte[] data)
        {
            if (IsFileExist(path + "//" + name))
            {
                File.Delete(path + "//" + name);
                File.WriteAllBytes(path + "//" + name, data);
            }
            else
            {
                File.WriteAllBytes(path + "//" + name, data);
            }
        }

        /************************************************************************/
        // Date: 22/4/2014 
        // Author: Leo 
        // Description: Check if file exist, because in ios, the File.Exists
        //              can not be used.
        /************************************************************************/
        public static bool IsFileExist(string path)
        {
            FileInfo info = new FileInfo(path);
            if (info == null || info.Exists == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /************************************************************************/
        // Date: 14/3/2014 
        // Author: Leo 
        // Description: Write data to file from start point.
        /************************************************************************/
        public static void WriteFile(string path, string name, int start, byte[] data)
        {
            string filePath = path + "//" + name;

            if (IsFileExist(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                fs.Seek(start, SeekOrigin.Current);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
            else
            {

            }
        }

        /************************************************************************/
        // Date: 14/3/2014 
        // Author: Leo 
        // Description: Load file by giving length.
        /************************************************************************/
        public static byte[] LoadFile(string path, string name)
        {
            try
            {
                byte[] data = File.ReadAllBytes(path + "//" + name);
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        /************************************************************************/
        // Date: 14/3/2014 
        // Author: Leo 
        // Description: 
        /************************************************************************/
        public static string GetRWFilePath()
        {
            string path = "";
            RuntimePlatform platform = Application.platform;

            switch (platform)
            {
                case RuntimePlatform.Android:
                    {
                        path = Application.persistentDataPath + "//";
                        break;
                    }
                case RuntimePlatform.IPhonePlayer:
                    {
                        path = Application.persistentDataPath + "//";
                        break;
                    }
                case RuntimePlatform.OSXEditor:
                    {
                        path = Application.dataPath + "//StreamingAssets//";
                        break;
                    }
                case RuntimePlatform.OSXPlayer:
                    {
                        path = Application.dataPath + "//StreamingAssets//";
                        break;
                    }
                case RuntimePlatform.WindowsEditor:
                    {
                        path = Application.dataPath + "//StreamingAssets//";
                        break;
                    }
                case RuntimePlatform.WindowsPlayer:
                    {
                        path = Application.dataPath + "//StreamingAssets//";
                        break;
                    }
            }

            return path;
        }

		/************************************************************************/
		// Date: 10/10/2014 
		// Author: Leo 
		// Description: 
		/************************************************************************/
		public static string GetStreamAssetFilePath()
		{
			string path = "";
			RuntimePlatform platform = Application.platform;
			
			switch (platform)
			{
			case RuntimePlatform.Android:
			{
				path = "jar:file://" + Application.dataPath + "!/assets//";
				break;
			}
			case RuntimePlatform.IPhonePlayer:
			{
				path = Application.dataPath +"//Raw//";
				break;
			}
			case RuntimePlatform.OSXEditor:
			{
				path = Application.dataPath + "//StreamingAssets//";
				break;
			}
			case RuntimePlatform.OSXPlayer:
			{
				path = Application.dataPath + "//StreamingAssets//";
				break;
			}
			case RuntimePlatform.WindowsEditor:
			{
				path = Application.dataPath + "//StreamingAssets//";
				break;
			}
			case RuntimePlatform.WindowsPlayer:
			{
				path = Application.dataPath + "//StreamingAssets//";
				break;
			}
			}
			
			return path;
		}
    }
}
