/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-11
 * Time: 18:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of IntPtr.
	/// </summary>
	public class IntPtr
	{
		public bool checkChange = false;
		
		public int[] chars;
		
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
		
		
		public IntPtr()
		{
		}
		
		public IntPtr(int[] ptr)
		{
			
		}
		
		public int this[int offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		public int this[uint offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
	}
}
