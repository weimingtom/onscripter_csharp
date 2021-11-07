/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 11:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_Color.
	/// </summary>
	public class SDL_Color
	{
		public byte r;
		public byte g;
		public byte b;
		public byte unused;		
		
		public SDL_Color()
		{
		}
		public SDL_Color(byte r, byte g, byte b, byte unused)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.unused = unused;
		}
	}
}
