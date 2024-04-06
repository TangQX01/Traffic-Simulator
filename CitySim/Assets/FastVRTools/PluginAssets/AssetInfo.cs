using System;
using System.Collections.Generic;
using SIP.Common;
using UnityEngine;

namespace SIP.FastVRTools.Assets
{
	public enum FILE_TYPE
	{
		PreviewImage,
		AssetBundles,
		Description,
		Image,
		Scene,
		Movie,
		Audio,
		None,
	};

	public enum ASSET_STATUS
	{
		None,
		Downloading,
		Downloaded,
		Loading,
		Loaded,
	}
	
	[Serializable]
	public class AssetInfo{
		
		#region Asset Properties from server
		//Asset Property from server
		public uint m_id = 0;								//Asset id in server.
		public string m_name;								//Asset name in server.
		public string m_md5;								//Asset md5 value in server.
		public string m_description;						//Asset description.
		public string m_extension;							//Asset extension in server.
		public ulong m_size = 0;							//Asset Length.
		#endregion
		
		#region Asset properties in system 
		public ASSET_STATUS m_status = ASSET_STATUS.None;	//Asset Current Status.
		public uint m_productId = 0;						//Asset productId.
		public string m_url;								//Asset server url address.
		public AssetBundle m_assetBundle = null;			//Asset bundle in system.
		public float m_downloadPercentage = 0.0f;				//Asset Download percentage.
		public float m_loadingPercentage = 0.0f;			//Asset Loading percentage.
		public FILE_TYPE m_type = FILE_TYPE.None;			//Asset Type.
		public GameObject m_obj;							//Asset binded GameObject.

		/// <summary>
		/// The path + name to save the file
		/// </summary>
		private string m_fullPath;

		public string FullPath
		{
			get
			{
				return m_fullPath;
			}

			set
			{
				m_fullPath = value;

				//Check if the file is exit
				if(FileManipulate.IsFileExist(m_fullPath))
				{
					m_status = ASSET_STATUS.Downloaded;
				}
			}

		}
		#endregion
		
		public AssetInfo(string fullPath)
		{
			FullPath = fullPath; 
		}
		
		public AssetInfo(uint id, string name, string md5, string extension, string fullPath)
		{
			m_id = id;
			m_name = name;
			m_md5 = md5;
			m_extension = extension;
			FullPath = fullPath;
		}
	}
}

