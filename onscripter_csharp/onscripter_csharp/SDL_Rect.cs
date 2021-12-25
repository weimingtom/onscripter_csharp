/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-3
 * Time: 9:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of SDL_Rect.
	/// </summary>
	public class SDL_Rect
	{
		public int x;
		public int y;
		public int w;
		public int h;
	
		public SDL_Rect()
		{
			
		}
		public SDL_Rect(int x, int y, int w, int h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
		}
		
		//FIXME:added
		public SDL_Rect(SDL_Rect rect)
		{
			this.x = rect.x;
			this.y = rect.y;
			this.w = rect.w;
			this.h = rect.h;			
		}
		
		//FIXME:added
		public void copy(SDL_Rect rect)
		{
			this.x = rect.x;
			this.y = rect.y;
			this.w = rect.w;
			this.h = rect.h;			
		}
	}
}
