/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-11-2
 * Time: 3:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of UnsignedLongPtr.
	/// </summary>
	public class UnsignedLongPtr
	{
		public bool checkChange = false;		
		
		public ulong[] chars;
		
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
		
		public UnsignedLongPtr()
		{
		}

		public UnsignedLongPtr(UnsignedLongPtr ptr, int offset)
		{
		}
				
		public UnsignedLongPtr(ulong[] bytes)
		{
			
		}
		
		public UnsignedLongPtr(CharPtr bytes)
		{
			
		}
		
		public ulong this[int offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		public ulong this[uint offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
			
		//FIXME:
		public void inc() {
			
		}
		public void inc(int size) {
			
		}
	}
}
