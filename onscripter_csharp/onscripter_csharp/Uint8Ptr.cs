/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-3
 * Time: 9:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of Uint8Ptr.
	/// </summary>
	public class Uint8Ptr
	{
		//like UnsignedCharPtr
		
		public bool checkChange = false;		
		
		public byte[] chars;
		
		private int _index;
		public int index
		{
			get
			{
				return _index;
			}
			set
			{
				if (checkChange)
				{
					Debug.Assert(false, "index changed");
				}
				_index = value;
			}
		}
		
		public Uint8Ptr()
		{
		}
		
		
		
		
		
		
		
		public Uint8Ptr(CharPtr ptr)
		{
			
		}
		public Uint8Ptr(UnsignedCharPtr ptr)
		{
			
		}
		public Uint8Ptr(UnsignedCharPtr ptr, int offset)
		{
			
		}
		public Uint8Ptr(Uint32Ptr ptr, int offset)
		{
			
		}
		public Uint8Ptr(Uint32Ptr ptr)
		{
			
		}
		
		public byte this[int offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		public byte this[uint offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		
		public void inc()
		{
			
		}
		public void inc(int offset)
		{
			
		}
		public void dec(int offset)
		{
			
		}
		
		public static bool isLargerEqual(Uint8Ptr a, Uint8Ptr b)
		{
			return false;
		}
	}
}
