/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 10:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_MouseMotionEvent.
	/// </summary>
	public class SDL_MouseMotionEvent : SDL_Event
	{
//		BYTE	type;
	//	BYTE	which;
		public byte	state;
		public UInt16	x;
		public UInt16	y;
		public short	xrel;
		public short	yrel;
	
		public SDL_MouseMotionEvent()
		{
		}
	}
}
