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
	/// Description of SDL_Event.
	/// </summary>
	public class SDL_Event
	{
		//FIXME: union, use SDL_MouseMotionEvent : SDL_Event extends
		public byte					type;
//		public SDL_KeyboardEvent		key;
//		public SDL_MouseMotionEvent	motion;
//		public SDL_MouseButtonEvent	button;
//		public SDL_UserEvent user; //FIXME:not implemented
//		public SDL_JoyButtonEvent jbutton; //FIXME:not implemented
//		public SDL_JoyAxisEvent jaxis;  //FIXME:not implemented
//		public SDL_ActiveEvent active; //FIXME:not implemented
		
		public SDL_Event()
		{
		}
	}
}
