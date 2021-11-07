/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-25
 * Time: 9:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of TTF_Font.
	/// </summary>
	public class TTF_Font
	{
		public TTF_Font()
		{
		}
		
		public static TTF_Font fromUnsignedCharPtr(UnsignedCharPtr ptr) {
			return new TTF_Font();
		}
	}
}
