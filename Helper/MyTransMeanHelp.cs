using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
	public static class MyTransMeanHelp
	{
		private static Dictionary<char, string> TransMeanConfigs = new Dictionary<char, string>()
		{
			{ '+',"%2B"},
			{ ' ',"%20"},
			{ '/',"%2F"},
			{ '?',"%3F"},
			{ '%',"%25"},
			{ '&',"%26"},
			{ '=',"%3D"},
		};

		private static Dictionary<string, char> TransMeanConfigs2 = new Dictionary<string, char>()
		{
			{ "%2B",'+'},
			{ "%20",' '},
			{ "%2F",'/'},
			{ "%3F",'?'},
			{ "%25",'%'},
			{ "%26",'&'},
			{ "%3D",'='},
		};
		private static char[] specialSymbols = new char[] {'+', ' ', '/', '?', '%', '&', '='};
		private static string[] specialSymbols2 = new string[] { "%2B", "%20","%2F", "%3F", "%25", "%26", "%3D" };


		public static string ToTrans(this string url)
		{
			string result = "";
			for(int i = 0; i < url.Length; i++)
			{
				if (specialSymbols.Contains(url[i]))
				{
					result += TransMeanConfigs[url[i]];
				}
				else
				{
					result += url[i];
				}
			}
			return result;
		}

		public static string ToNormal(this string url)
		{
			string result = "";
			for(int i = 0; i < url.Length; i++)
			{
				if(i < url.Length - 2)
				{
					if (specialSymbols2.Contains(url.Substring(i, 3)))
					{
						result += TransMeanConfigs2[url.Substring(i, 3)];
						i += 2;
					}
					else
					{
						result += url[i];
					}
				}
				else
				{
					result += url[i];
				}
			}
			return result;
		}
	}
}
