/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-3
 * Time: 9:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	public class CharPtr
	{
		public CharPtr()
		{
		}
		
		public CharPtr(string str) {
			
		}
		
		public CharPtr(char[] str) {
			
		}
		
		public CharPtr(CharPtr str) {
			
		}
		
		public static implicit operator CharPtr(string str) { return new CharPtr(str); }
		public static implicit operator CharPtr(char[] chars) { return new CharPtr(chars); }
	}
}
