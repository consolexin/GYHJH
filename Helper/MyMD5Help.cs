﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helper
{
	public static class MyMD5Help
	{
		public static string GetMD5(this string sDataIn)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] bytValue, bytHash;
			bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
			bytHash = md5.ComputeHash(bytValue);
			md5.Clear();
			string sTemp = "";
			for (int i = 0; i < bytHash.Length; i++)
			{
				sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
			}
			return sTemp.ToLower();

		}
	}
}
