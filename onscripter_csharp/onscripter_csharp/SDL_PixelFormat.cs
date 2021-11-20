/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-11-21
 * Time: 1:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_PixelFormat.
	/// </summary>
	public class SDL_PixelFormat
	{
		public byte BitsPerPixel; //depth
		public byte Rloss; 
		public byte Gloss; 
		public byte Bloss; 
		public byte Aloss; 
		public byte Rshift;
		public byte Gshift;
		public byte Bshift;
		public byte Ashift;
		public UInt32 Rmask;
		public UInt32 Gmask;
		public UInt32 Bmask;
		public UInt32 Amask;
		public UInt32 colorkey;		
		
		public SDL_PixelFormat()
		{
		}
	}
}
