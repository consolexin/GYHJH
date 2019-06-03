using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
	public class MyGuidHelp
	{
		public static string GuidToString(Guid guid)
		{
			string uguid = guid.ToString().ToUpper();
			string[] uguids = uguid.Split('-');
			string result = "";
			for(int i = 0; i < uguids.Length; i++)
			{
				result += uguids[i];
			}
			return result;
		}
		
		public static Guid StringToGuid(string str)
		{
			return new Guid(str);
		}
	}
}
