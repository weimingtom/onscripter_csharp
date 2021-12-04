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
	/// Description of SDL_KeyboardEvent.
	/// </summary>
	public class SDL_KeyboardEvent : SDL_Event
	{
//		public byte		type;
	//	BYTE		which;
	//	BYTE		state;
		public SDL_keysym	keysym = new SDL_keysym();
		
		public SDL_KeyboardEvent()
		{
		}
	}
}
