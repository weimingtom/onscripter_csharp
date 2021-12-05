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
	/// Description of Uint32Ptr.
	/// </summary>
	//FIXME: same as UnisgnedLongPtr
	public class Uint32Ptr
	{
		public bool checkChange = false;		
		
		public UInt32[] chars;
		
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
		
		public Uint32Ptr()
		{
		}
		
		public Uint32Ptr(UnsignedCharPtr ptr)
		{
			
		}
		public Uint32Ptr(UnsignedCharPtr ptr, int offset)
		{
			
		}
		public Uint32Ptr(Uint32Ptr ptr, int offset)
		{
			
		}
		
		public UInt32 this[int offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		public UInt32 this[uint offset]
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
	}
}
