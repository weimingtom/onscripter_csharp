/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 10:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_MouseButtonEvent.
	/// </summary>
	public class SDL_MouseButtonEvent : SDL_Event
	{
//		BYTE	type;
	//	BYTE	which;
		public byte	button;
		public byte	state;
		public Int16	x;
		public Int16	y;
		
		public SDL_MouseButtonEvent()
		{
		}
	}
}
