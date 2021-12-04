/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-12-5
 * Time: 3:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_JoyButtonEvent.
	/// </summary>
	public class SDL_JoyButtonEvent : SDL_Event
	{
//		public Uint8 type;	/* SDL_JOYBUTTONDOWN or SDL_JOYBUTTONUP */
		public byte which;	/* The joystick device index */
		public byte button;	/* The joystick button index */
		public byte state;	/* SDL_PRESSED or SDL_RELEASED */
		
		public SDL_JoyButtonEvent()
		{
		}
	}
}
