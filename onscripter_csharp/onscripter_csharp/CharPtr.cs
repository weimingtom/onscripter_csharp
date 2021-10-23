/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-3
 * Time: 9:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;

namespace onscripter_csharp
{
	public class CharPtr
	{
		public bool checkChange = false;
		
		public char[] chars;
		
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
		
		public CharPtr()
		{
		}
		
		public CharPtr(string str) {
			
		}
		
		public CharPtr(char[] str) {
			
		}
		
		public CharPtr(CharPtr str) {
			
		}
		
		public CharPtr(CharPtr str, int offset) {
			
		}
		
		public static implicit operator CharPtr(string str) { return new CharPtr(str); }
		public static implicit operator CharPtr(char[] chars) { return new CharPtr(chars); }
		
		public char this[int offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		public char this[uint offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
		
		//++
		public CharPtr inc() 
		{
			//FIXME:not correct
			this._index++;
			return this;
		}
		//+=3
		public CharPtr inc(int offset) 
		{
			//FIXME:not correct
			this._index+=offset;
			return this;
		}
		//--
		public CharPtr dec() 
		{
			//FIXME:not correct
			this._index--;
			return this;
		}
		
		public static int minus(CharPtr s1, CharPtr s2)
		{
			return 0;
		}
		
		//<
		public bool lessThen(CharPtr ptr)
		{
			//FIXME:
			return false;
		}
		
		//FIXME:todo
		public static CharPtr fromDoubleByte(string str) {
			return null;
		}
	}
}
