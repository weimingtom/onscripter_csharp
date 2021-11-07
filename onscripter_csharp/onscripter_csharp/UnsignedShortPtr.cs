/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-11-7
 * Time: 10:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of UnsignedShortPtr.
	/// </summary>
	public class UnsignedShortPtr
	{
		public bool checkChange = false;		
		
		public ushort[] chars; //like wchar_t
		
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
		
		public UnsignedShortPtr()
		{
		}

		public UnsignedShortPtr(UnsignedShortPtr ptr, int offset)
		{
		}
				
		public UnsignedShortPtr(ushort[] bytes)
		{
			
		}
		
		public UnsignedShortPtr(CharPtr bytes)
		{
			
		}
		
		public ushort this[int offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		public ushort this[uint offset]
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
