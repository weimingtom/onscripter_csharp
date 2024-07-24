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
using System.Text;

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
			this.chars = null;
			this.index = 0;
		}
		
		public CharPtr(string str) 
		{
			if (false)
			{
				this.chars = (str + '\0').ToCharArray();
				this.index = 0;
			}
			else
			{
				byte[] gbk = Encoding.GetEncoding("GBK").GetBytes(str);
				char[] result = new char[gbk.Length + 1];
				for (int i = 0; i < gbk.Length; ++i)
				{
					result[i] = (char)gbk[i];
				}
				result[gbk.Length] = '\0';
				this.chars = result;
				this.index = 0;
			}
		}
		
		public CharPtr(char[] str) 
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; ++i)
				{
					if ((int)str[i] > 255)
					{
						Debug.WriteLine("<< CharPtr(char[]) overflow : " + i);
					}
				}
			}
			this.chars = str;
			this.index = 0;
		}
		
		public CharPtr(CharPtr str) 
		{
			this.chars = str.chars;
			this.index = str.index;
		}
		
		public CharPtr(CharPtr str, int offset) 
		{
			this.chars = str.chars;
			this.index = offset;			
		}
		public CharPtr(UnsignedCharPtr str, int offset) {
			this.chars = new char[str.chars.Length];
			for (int i = 0; i < str.chars.Length; ++i)
			{
				this.chars[i] = (char)str.chars[i];
			}
			this.index = offset;	
		}
		
		
		public static implicit operator CharPtr(string str) { return new CharPtr(str); }
		public static implicit operator CharPtr(char[] chars) { return new CharPtr(chars); }
		
		public char this[int offset]
		{
			get { 
				//FIXME:???
				if (index + offset >= chars.Length || index + offset < 0) {
					Debug.WriteLine("<< CharPtr.this.getter, index overflow return 0 : " + (index + offset) + ", len == " + chars.Length);
					return '\0';
				}
				return chars[index + offset];
			}
			set { 
				//FIXME:???
				if (index + offset >= chars.Length || index + offset < 0) {
					Debug.WriteLine("<< CharPtr.this.setter, index overflow return : " + (index + offset) + ", len == " + chars.Length);
					return;
				}
				chars[index + offset] = value;
			}
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
		
		//public static int operator -(CharPtr ptr1, CharPtr ptr2) {
		public static int minus(CharPtr s1, CharPtr s2)
		{
			Debug.Assert(s1.chars == s2.chars); 
			return s1.index - s2.index;
		}
		
		//<
		//public static bool operator <(CharPtr ptr1, CharPtr ptr2) {
		public static bool isLessThen(CharPtr ptr1, CharPtr ptr2)
		{
			Debug.Assert(ptr1.chars == ptr2.chars); 
			return ptr1.index < ptr2.index;
		}
		
		//>
		//public static bool operator >(CharPtr ptr1, CharPtr ptr2) {
		public static bool isLargerThen(CharPtr ptr1, CharPtr ptr2)
		{
			Debug.Assert(ptr1.chars == ptr2.chars); 
			return ptr1.index > ptr2.index;
		}
		
		//FIXME:todo
		public static CharPtr fromDoubleByte(string str) {
			//FIXME:
			//return null;
			return new CharPtr(str);
		}
		
		public static CharPtr fromUnsignedCharPtr(UnsignedCharPtr str) {
			//FIXME:
			//return null;
			return new CharPtr(str, 0);
		}
		
		//ToString, TODO!!!!!
		
		
		
		//FIXME: for compare, like:
		//		if (ptr1 != ptr2)
		
			public static int operator -(CharPtr ptr1, CharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index - ptr2.index; }
			public static bool operator <(CharPtr ptr1, CharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index < ptr2.index; }
			public static bool operator <=(CharPtr ptr1, CharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index <= ptr2.index; }
			public static bool operator >(CharPtr ptr1, CharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index > ptr2.index; }
			public static bool operator >=(CharPtr ptr1, CharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index >= ptr2.index; }
			public static bool operator ==(CharPtr ptr1, CharPtr ptr2) {
				object o1 = ptr1 as CharPtr;
				object o2 = ptr2 as CharPtr;
				if ((o1 == null) && (o2 == null)) return true;
				if (o1 == null) return false;
				if (o2 == null) return false;
				return (ptr1.chars == ptr2.chars) && (ptr1.index == ptr2.index); }
			public static bool operator !=(CharPtr ptr1, CharPtr ptr2) {return !(ptr1 == ptr2); }

			
			//FIXME: (new CharPtr(dptr, -1))[0] != DELIMITER
		
			public static bool operator ==(CharPtr ptr, char ch) { return ptr[0] == ch; }
			public static bool operator ==(char ch, CharPtr ptr) { return ptr[0] == ch; }
			public static bool operator !=(CharPtr ptr, char ch) { return ptr[0] != ch; }
			public static bool operator !=(char ch, CharPtr ptr) { return ptr[0] != ch; }
			
			
			//FIXME: fprintf(stderr, "Adding path: %s\n", new_paths);
			public override string ToString()
			{
				string result = "";
				for (int i = index; (i<chars.Length) && (chars[i] != '\0'); i++)
					result += chars[i];
				return result;
			}
			
	}
}
