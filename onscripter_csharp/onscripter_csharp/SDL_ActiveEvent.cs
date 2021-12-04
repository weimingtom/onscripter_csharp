/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-12-5
 * Time: 3:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_ActiveEvent.
	/// </summary>
	public class SDL_ActiveEvent : SDL_Event
	{
//		Uint8 type;	/* SDL_ACTIVEEVENT */
		public byte gain;	/* Whether given states were gained or lost (1/0) */
		public byte state;	/* A mask of the focus states */
		
		public SDL_ActiveEvent()
		{
		}
	}
}
