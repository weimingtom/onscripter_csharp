/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-12-5
 * Time: 3:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_keysym.
	/// </summary>
	public class SDL_keysym
	{
	//	BYTE	scancode;
		public SDLKey	sym;
	//	SDLMod	mod;
		public UInt16	unicode; //FIXME: not implemented
		
		public SDL_keysym()
		{
		}
	}
}
