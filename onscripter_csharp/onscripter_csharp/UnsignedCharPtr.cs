/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-3
 * Time: 9:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of UnsignedCharPtr.
	/// </summary>
	public class UnsignedCharPtr
	{
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
		
		public UnsignedCharPtr()
		{
		}

		public UnsignedCharPtr(UnsignedCharPtr ptr)
		{
		}
		public UnsignedCharPtr(UnsignedCharPtr ptr, int offset)
		{
		}
				
		public UnsignedCharPtr(byte[] bytes)
		{
			
		}
		
		public UnsignedCharPtr(CharPtr bytes)
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
			
		//FIXME:
		public void inc() {
			
		}
		public void inc(int size) {
			
		}
		public void minus(int size)
		{
			
		}
	}
}
