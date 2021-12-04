/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-12-5
 * Time: 3:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_UserEvent.
	/// </summary>
	public class SDL_UserEvent : SDL_Event
	{
//		Uint8 type;	/* SDL_USEREVENT through SDL_NUMEVENTS-1 */
		public int code;	/* User defined event code */
		public object data1;	/* User defined data pointer */
		public object data2;	/* User defined data pointer */
		
		public SDL_UserEvent()
		{
		}
	}
}
