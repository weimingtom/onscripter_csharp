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
using System.Text;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of UnsignedCharPtr.
	/// </summary>
	public class UnsignedCharPtr
	{
		public bool checkChange = false;		
		
		public byte[] chars;
		public UInt32[] charsUInt32;
		
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
			this.chars = null;
			this.index = 0;
		}

		public UnsignedCharPtr(UnsignedCharPtr ptr)
		{
			this.chars = ptr.chars;
			this.index = ptr.index;
		}
		public UnsignedCharPtr(Uint8Ptr ptr)
		{
			this.chars = ptr.chars;
			this.index = ptr.index;			
		}
		public UnsignedCharPtr(UnsignedCharPtr ptr, int offset)
		{
			this.chars = ptr.chars;
			this.index = ptr.index + offset;			
		}
		public UnsignedCharPtr(Uint32Ptr ptr, int offset)
		{
			this.charsUInt32 = ptr.chars;
			this.index = ptr.index + offset;
			Debug.Assert(false, "charsUInt32");
		}
		public UnsignedCharPtr(Uint32Ptr ptr)
		{
			this.charsUInt32 = ptr.chars;
			this.index = ptr.index;
			Debug.Assert(false, "charsUInt32");			
		}
				
		public UnsignedCharPtr(byte[] bytes)
		{
			this.chars = bytes;
			this.index = 0;	
		}
		
		public UnsignedCharPtr(CharPtr str)
		{			
			this.chars = new byte[str.chars.Length];
			for (int i = 0; i < str.chars.Length; ++i)
			{
				this.chars[i] = (byte)str.chars[i];
			}
			this.index = str.index;	
		}
		
		public byte this[int offset]
		{
//			get { return chars[index + offset]; }
//			set { chars[index + offset] = value; }
			get { 
				//FIXME:???
				if (index + offset >= chars.Length || index + offset < 0) {
					Debug.WriteLine("<< UnsignedCharPtr.this.getter, index overflow return 0 : " + (index + offset) + ", len == " + chars.Length);
					return (byte)0;
				}
				return chars[index + offset];
			}
			set { 
				//FIXME:???
				if (index + offset >= chars.Length || index + offset < 0) {
					Debug.WriteLine("<< UnsignedCharPtr.this.setter, index overflow return : " + (index + offset) + ", len == " + chars.Length);
					return;
				}
				chars[index + offset] = value;
			}
		}
		public byte this[uint offset]
		{
			get { return chars[index + offset]; }
			set { chars[index + offset] = value; }
		}
			
		//FIXME:
		public UnsignedCharPtr inc() 
		{
			//FIXME:not correct
			this._index++;
			return this;	
		}
		public UnsignedCharPtr inc(int offset) 
		{
			//FIXME:not correct
			this._index+=offset;
			return this;	
		}
		
		//--
		public UnsignedCharPtr dec() 
		{
			//FIXME:not correct
			this._index--;
			return this;
		}
		public UnsignedCharPtr dec(int offset) 
		{
			//FIXME:not correct
			this._index-=offset;
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
		
			public static int operator -(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index - ptr2.index; }
			public static bool operator <(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index < ptr2.index; }
			public static bool operator <=(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index <= ptr2.index; }
			public static bool operator >(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index > ptr2.index; }
			public static bool operator >=(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {
				Debug.Assert(ptr1.chars == ptr2.chars); return ptr1.index >= ptr2.index; }
			public static bool operator ==(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {
				object o1 = ptr1 as UnsignedCharPtr;
				object o2 = ptr2 as UnsignedCharPtr;
				if ((o1 == null) && (o2 == null)) return true;
				if (o1 == null) return false;
				if (o2 == null) return false;
				return (ptr1.chars == ptr2.chars) && (ptr1.index == ptr2.index); }
			public static bool operator !=(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2) {return !(ptr1 == ptr2); }

			
			//FIXME: (new CharPtr(dptr, -1))[0] != DELIMITER
		
			public static bool operator ==(UnsignedCharPtr ptr, byte ch) { return ptr[0] == ch; }
			public static bool operator ==(byte ch, UnsignedCharPtr ptr) { return ptr[0] == ch; }
			public static bool operator !=(UnsignedCharPtr ptr, byte ch) { return ptr[0] != ch; }
			public static bool operator !=(byte ch, UnsignedCharPtr ptr) { return ptr[0] != ch; }
			
			
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
