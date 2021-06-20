/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 8:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	public partial class ONScripter
	{
		public static string xstr(string s) { return str(s); }
		public static string str(string s) { return s; }
		
		public const string VER_NUMBER = "20110314-en";
		public static string ONS_VERSION = xstr(VER_NUMBER);
		public const int NSC_VERSION = 294;
	}
}
