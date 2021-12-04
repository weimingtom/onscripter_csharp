/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-12-5
 * Time: 3:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_JoyAxisEvent.
	/// </summary>
	public class SDL_JoyAxisEvent : SDL_Event
	{
//		Uint8 type;	/* SDL_JOYAXISMOTION */
		public byte which;	/* The joystick device index */
		public byte axis;	/* The joystick axis index */
		public Int16 value_;	/* The axis value (range: -32768 to 32767) */
		
		public SDL_JoyAxisEvent()
		{
		}
	}
}
