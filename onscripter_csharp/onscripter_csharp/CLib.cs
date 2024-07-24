/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-9-12
 * Time: 10:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace onscripter_csharp
{
	public partial class ONScripter {


		//done
		//public static FILEPtr stdin = new FILEPtr(Console.OpenStandardInput());
		public static FILEPtr stdout = new FILEPtr(Console.OpenStandardOutput());
		public static FILEPtr stderr = new FILEPtr(Console.OpenStandardError());
		
		
		
		public class time_t {
			//done
		}
		
		//done
		public class tm {
			public int tm_year, tm_mon, tm_mday;
			public int tm_hour, tm_min, tm_sec;
			
			public tm() {
				DateTime now = DateTime.Now;
				this.tm_year = now.Year;
				this.tm_mon = now.Month;
				this.tm_mday = now.Day;
				this.tm_hour = now.Hour;
				this.tm_min = now.Minute;
				this.tm_sec = now.Second;
			}
		}
		
		//done
		public static time_t time(time_t t) {
			return new time_t();
		}
		
		//done
		public static tm localtime(time_t t) {
			return new tm();
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		//done
		public static int printf(string str, params Object[] args) 
		{
			Tools.printf(str.ToString(), args);
			return 1; //Returns the number of characters printed
		}
		
		//done
		public static void snprintf(CharPtr dst, int n, string str, params Object[] args)
		{
			string temp = Tools.sprintf(str.ToString(), args);
			strncpy(dst, new CharPtr(temp), (uint)n);
		}
		//done	
		public static void sprintf(CharPtr dst, CharPtr str, params Object[] args)
		{
			string temp = Tools.sprintf(str.ToString(), args);
			strcpy(dst, new CharPtr(temp));
		}
		//done
		public static int fprintf(FILEPtr fp, string str, params Object[] args) 
		{
			string result = Tools.sprintf(str.ToString(), args);
			Debug.WriteLine("[fprintf]" + result + "[/fprintf]");
			char[] chars = result.ToCharArray();
			byte[] bytes = new byte[chars.Length];
			for (int i=0; i<chars.Length; i++)
				bytes[i] = (byte)chars[i];
			fp.stream.Write(bytes, 0, bytes.Length);
			return 1; //Returns the number of characters printed
		}
		
		public static CharPtr sscanf(CharPtr dst, string str, params Object[] args)
		{
			return null;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		//done
		public static CharPtr strcpy(CharPtr dst, CharPtr src)
		{
			int i;
			for (i = 0; src[i] != '\0'; i++)
				dst[i] = src[i];
			dst[i] = '\0';
			return dst;
		}
		
		//done
		public static CharPtr strncpy(CharPtr dst, CharPtr src, uint n)
		{
			int i;
			for (i = 0; src[i] != '\0' && i < n ; i++) {
				dst[i] = src[i];
			}
			dst[i] = '\0';
			return dst;
		}
		
		//done
		public static uint strlen(CharPtr str)
		{
			if (str == null) 
			{
				//FIXME:
				Debug.WriteLine("<<< strlen str null");
				return 0;
			}
			uint index = 0;
			while (str[index] != '\0')
				index++;
			return index;
		}
	
		//done
		public static CharPtr strcat(CharPtr dst, CharPtr src)
		{
			int dst_index = 0;
			while (dst[dst_index] != '\0')
				dst_index++;
			int src_index = 0;
			while (src[src_index] != '\0')
				dst[dst_index++] = src[src_index++];
			dst[dst_index++] = '\0';
			return dst;	
		}
		
		//done
		public static int strcmp(CharPtr s1, CharPtr s2)
		{
			if (s1 == s2)
				return 0;
			if (s1 == null)
				return -1;
			if (s2 == null)
				return 1;

			for (int i = 0; ; i++)
			{
				if (s1[i] != s2[i])
				{
					if (s1[i] < s2[i])
						return -1;
					else
						return 1;
				}
				if (s1[i] == '\0')
					return 0;
			}
		}
		
		//done
		public static int strncmp(CharPtr str1, CharPtr str2, int n)
		{
			if (str1 == str2)
				return 0;
			if (str1 == null)
				return -1;
			if (str2 == null)
				return 1;

			for (int i = 0; i < n; i++)
			{
				if (str1[i] != str2[i])
				{
					if (str1[i] < str2[i])
						return -1;
					else
						return 1;
				}
				if (str1[i] == '\0')
					return 0;
			}
			return 0;
		}
		
		//done
		public static CharPtr strchr(CharPtr str, int c)
		{
			if (c != '\0')
			{
				for (int index = str.index; str.chars[index] != 0; index++)
					if (str.chars[index] == c)
						return new CharPtr(str.chars, index);
			}
			else
			{
				for (int index = str.index; index < str.chars.Length; index++)
					if (str.chars[index] == c)
						return new CharPtr(str.chars, index);
			}
			return null;
		}
		
		//done
		public static CharPtr strrchr(CharPtr str, char ch)
		{
			int result = -1;
			if (ch != '\0')
			{
				for (int index = str.index; str.chars[index] != 0; index++) 
					if (str.chars[index] == ch)
						result = index;
			}
			else
			{
				for (int index = str.index; index < str.chars.Length; index++)
					if (str.chars[index] == ch)
						result = index;
			}
			if (result >= 0) 
			{
				return new CharPtr(str.chars, result);
			}
			return null;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		public static void memcpy(CharPtr ptr1, CharPtr ptr2, uint size)
		{
			for (int i = 0; i < size; i++)
				ptr1[i] = ptr2[i];
		}
		public static void memcpy(IntPtr ptr1, IntPtr ptr2, uint length, int size)
		{
			for (int i = 0; i < size; i++)
				ptr1[i] = ptr2[i];
		}
		public static void memcpy(UnsignedCharPtr ptr1, UnsignedCharPtr ptr2, uint size)
		{
			for (int i = 0; i < size; i++)
				ptr1[i] = ptr2[i];			
		}
		//----------------//
		public static int sizeof_ONSBuf()
		{
			return 0;
		}
		public static void memcpy(Uint32Ptr s1, Uint32Ptr s2, uint length)
		{
			
		}
		//----------------//
		public static int sizeof_WAVE_HEADER() 
		{
			return 0;
		}
		public static void memcpy(UnsignedCharPtr s1, WAVE_HEADER s2, uint length)
		{
			
		}
		//----------------//
		public static void memcpy(CharPtr ptr1, UnsignedCharPtr ptr2, uint size)
		{
			for (int i = 0; i < size; i++)
				ptr1[i] = (char)ptr2[i];
		}
		public static void memcpy(UnsignedCharPtr ptr1, CharPtr ptr2, uint size)
		{
			for (int i = 0; i < size; i++)
				ptr1[i] = (byte)ptr2[i];			
		}
		public static void memcpy(ScriptHandler.ExtendedVariableData[] s1, ScriptHandler.ExtendedVariableData s2, uint length)
		{
			//FIXME:???
		}
		//----------------//
		public static int sizeof_DirPaths() 
		{
			return 0;
		}
		public static void memcpy(DirPaths s1, DirPaths s2, uint length)
		{
			//FIXME:???
		}
		//----------------//
		public static int sizeof_AnimationInfo()
		{
			return 0;
		}
		public static void memcpy(AnimationInfo s1, AnimationInfo s2, uint length)
		{
			//FIXME:???
		}
		//----------------//
		public static int sizeof_int()
		{
			return 0;
		}
		public static void memcpy(int[] s1, int[] s2, uint length)
		{
			//FIXME:???
		}
		//----------------//
		public static int sizeof_uchar3()
		{
			return 0;
		}
		public static void memcpy(byte[][] s1, byte[][] s2, uint length)
		{
			//FIXME:???
		}
		//----------------//
		public static void memset(IntPtr s, int ch, uint n)
		{
			
		}
		public static void memset(CharPtr s, int ch, uint n)
		{
			
		}
		public static void memset(UnsignedCharPtr s, int ch, uint n)
		{
			
		}
		public static void memset(UnsignedLongPtr s, int ch, uint n)
		{
			
		}
		//----------------//
		public static int sizeof_DirectReader_FileInfo()
		{
			return 0;
		}
		public static void memset(DirectReader.FileInfo s, int ch, uint n)
		{
			
		}
		//----------------//
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		//done
		public static double sin(double d)
		{
			return Math.Sin(d);
		}
		//done
		public static double cos(double x)
		{
			return Math.Cos(x);
		}
		//done
		public static double tan(double d)
		{
			return Math.Tan(d);
		}
		//done
		public static int abs(int x)
		{
			return Math.Abs(x);
		}
		//done
		public static double sqrt(double x)
		{
			return Math.Sqrt(x);
		}
		//done
		public static double floor(double x)
		{
			return Math.Floor(x);
		}
		//done
		private static Random _random = new Random();
		public static void srand(time_t t)
		{
			_random = new Random((int)(DateTime.Now.Ticks & 0x7fffffff));
		}
		//done
		public const int RAND_MAX = 0x7fff;
		public static int rand()
		{
			return _random.Next() & RAND_MAX;
		}
		//done
		public static void __unused(bool x)
		{
			//do nothing
		}
		//done
		public static void exit(int exitCode)
		{
			Environment.Exit(exitCode);
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		//done
		public const int EOF = -1;
		//done
		public static FILEPtr fopen(CharPtr filename, CharPtr mode)
		{
			FileStream stream = null;
			string str = filename.ToString();			
			FileMode filemode = FileMode.Open;
			FileAccess fileaccess = (FileAccess)0;			
			for (int i=0; mode[i] != '\0'; i++)
				switch (mode[i])
				{
					case 'r': 
						fileaccess = fileaccess | FileAccess.Read;
						if (!File.Exists(str))
							return null;
						break;

					case 'w':
						filemode = FileMode.Create;
						fileaccess = fileaccess | FileAccess.Write;
						break;
				}
			try
			{
				stream = new FileStream(str, filemode, fileaccess);
			}
			catch
			{
				stream = null;
			}			
			
			FILEPtr ret = new FILEPtr();
			ret.stream = stream;
			return ret;
		}
		//done
		public static FILEPtr _wfopen(UnsignedShortPtr path, UnsignedShortPtr mode)
		{
			CharPtr path_utf8 = new CharPtr(new char[path.chars.Length * 2]);
			int path_utf8_size = WideCharToMultiByte(CP_UTF8, 0, path, path.chars.Length, path_utf8, 0, null, null);
			WideCharToMultiByte(CP_UTF8, 0, path, path.chars.Length, path_utf8, path_utf8_size, null, null);

			CharPtr mode_utf8 = new CharPtr(new char[mode.chars.Length * 2]);
			int mode_utf8_size = WideCharToMultiByte(CP_UTF8, 0, mode, mode.chars.Length, mode_utf8, 0, null, null);
			WideCharToMultiByte(CP_UTF8, 0, mode, mode.chars.Length, mode_utf8, mode_utf8_size, null, null);
			
			return fopen(path_utf8, mode_utf8);
		}
		//done
		public static void fclose(FILEPtr fp)
		{
			try
			{
				fp.stream.Flush();
				fp.stream.Close();
			}
			catch { }			
		}
				
		//done
		public static int feof(FILEPtr fp)
		{
			return fp.stream.Position >= fp.stream.Length ? 1 : 0;
		}

		//done		
		public static int fgetc(FILEPtr fp)
		{
			int result = fp.stream.ReadByte();
			if (result == (int)'\r') //FIXME: only tested under Windows
			{
				result = fp.stream.ReadByte();
			}
			return result;

		}
		
		//done
		public const int SEEK_SET = 0;
		public const int SEEK_CUR = 1;
		public const int SEEK_END = 2;
		public static void fseek(FILEPtr fp, int pos, int mode)
		{
			if (mode == SEEK_SET) {
				fp.stream.Seek(pos, SeekOrigin.Begin);
			} else if (mode == SEEK_CUR) {
				fp.stream.Seek(pos, SeekOrigin.Current);
			} else if (mode == SEEK_END) {
				fp.stream.Seek(pos, SeekOrigin.End);
			} else {
				throw new Exception("fseek mode not supported");
			}
		}

		//done
		public static long ftell(FILEPtr fp)
		{
			return fp.stream.Position;
		}
		
		public static uint fread(UnsignedCharPtr buffer, uint size, uint count, FILEPtr fp)
		{
			return 0;
		}
		public static uint fread(CharPtr buffer, uint size, uint count, FILEPtr fp)
		{
			return 0;
		}
		public static uint fwrite(UnsignedCharPtr buffer, uint size, uint count, FILEPtr fp)
		{
			return 0;
		}
		public static uint fwrite(CharPtr buffer, uint size, uint count, FILEPtr fp)
		{
			return 0;
		}
		
		//done
		public static int fputs(CharPtr buffer, FILEPtr fp)
		{
			UnsignedCharPtr ptr = new UnsignedCharPtr(buffer);
			fp.stream.Write(ptr.chars, ptr.index, (int)strlen(buffer));
			if (feof(fp)!=0)
			{
				return EOF;
			}
			else
			{
				//This function returns a non-negative value, or else on error it returns EOF.
				return 0;
			}
		}
		
		//done
		public static int fputc(char buffer, FILEPtr fp)
		{
			fp.stream.WriteByte((byte)buffer);
			if (feof(fp)!=0)
			{
				return EOF;
			}
			else
			{
				return (int)buffer;
			}
		}
		
		//done
		public static void fflush(FILEPtr fp)
		{
			try
			{
				fp.stream.Flush();
			}
			catch { }	
		}
		
		//done
		public static CharPtr fgets(CharPtr s, int n, FILEPtr fp)
		{
			bool isEnd = false;
			StringBuilder sb = new StringBuilder();
			while (true)
			{
				if (sb.Length >= n - 1)
				{
					break;
				}
				int result = fp.stream.ReadByte();
				if (result == (int)'\r') //FIXME: only tested under Windows
				{
					result = fp.stream.ReadByte();
					if (result == -1)
					{
						isEnd = true;
						break;
					}
					break;
				} 
				else if (result == -1)
				{
					isEnd = true;
					break;
				}
				sb.Append((char)(byte)result);
			}
			if (isEnd && sb.Length == 0)
			{
				//FIXME:if get eof not \n after the string, make eof like \n
				return null;
			}
			CharPtr src = sb.ToString() + "\n"; //FIXME:\n
			strcpy(s, src);
			return s;
		}
		
		public static int ferror(FILEPtr stream)
		{
			return 0;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		//done
		public static int remove(CharPtr filename)
		{
			try 
			{
				if (File.Exists(filename.ToString()))
	            {  
					File.Delete(filename.ToString());
					return 0;
				}
				else
				{
					return -1;
				}
			} 
			catch
			{
				return -1;
			}
		}
		//done
		public static int rename(CharPtr oldname, CharPtr newname)
		{
			try {
				new FileInfo(oldname.ToString()).MoveTo(newname.ToString());
				return 0;
			} catch (Exception e) {
				Debug.WriteLine(e);
				return -1;
			}
		}
		//done
		public static void mkdir(CharPtr path) //FIXME: second param?
		{
			if (!Directory.Exists(path.ToString()))
			{
				Directory.CreateDirectory(path.ToString());
			}
		}
		
				
		

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		

		
		
		
		
		
		
		
		
		
		

		

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		


		
		
		
		
		
		
		
		
		
		//done
		public static int atoi(CharPtr str)
		{
			int result = 0;
			try {
				if (int.TryParse(str.ToString(), out result))
			    {
			    	return result;
			    }
			} catch (Exception) {
				
			}
			return 0;
		}
		//done
		public static CharPtr getenv(CharPtr name)
		{
			string result = Environment.GetEnvironmentVariable(name.ToString());
			return result != null ? new CharPtr(result) : null;
		}
		//done
		public static int toupper(int c)
		{
			return (int)Char.ToUpper((char)c);
		}
				
				
		
		
		
		
		
		
		
		
		
		
		
		//done
		public static void OutputDebugString(CharPtr str)
		{
			Debug.WriteLine(str.ToString());
		}
		
		public const uint CP_ACP = 0;
		public const uint CP_UTF8 = 65001;
		public static int MultiByteToWideChar(uint     CodePage,
			    long    dwFlags,
			    CharPtr   lpMultiByteStr,
			    int      cchMultiByte,
			    UnsignedShortPtr   lpWideCharStr,
			    int      cchWideChar)
		{
			return 0;
		}
		public static int WideCharToMultiByte(uint     CodePage,
			    long    dwFlags,
			    UnsignedShortPtr  lpWideCharStr,
			    int      cchWideChar,
			    CharPtr    lpMultiByteStr,
			    int      cchMultiByte,
			    CharPtr   lpDefaultChar,
			    bool[]   lpUsedDefaultChar)
		{
			return 0;
		}
		public const UInt32 GENERIC_READ = (UInt32)(0x80000000L);
		public const int OPEN_EXISTING = 3;
		public const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
		public const uint INVALID_HANDLE_VALUE = (uint)(uint.MaxValue); //-1
		public static uint CreateFile(
			CharPtr lpFileName,
		    UInt32 dwDesiredAccess,
		    UInt32 dwShareMode,
		    Object lpSecurityAttributes,
		    UInt32 dwCreationDisposition,
		    UInt32 dwFlagsAndAttributes,
		    uint hTemplateFile
		    )
		{
			return 0;
		}
		public static void CloseHandle(uint handle)
		{
			
		}
		
		public class FILETIME
		{
			
		}
		public class SYSTEMTIME
		{
			public Int16 wYear;
		    public Int16 wMonth;
		    public Int16 wDayOfWeek;
		    public Int16 wDay;
		    public Int16 wHour;
		    public Int16 wMinute;
		    public Int16 wSecond;
		    public Int16 wMilliseconds;
		}
		public static void GetFileTime(
		    uint hFile,
		    FILETIME lpCreationTime,
		    FILETIME lpLastAccessTime,
		    FILETIME lpLastWriteTime
		    )
		{
			
		}
		public static bool FileTimeToLocalFileTime(
		    FILETIME lpFileTime,
		    FILETIME lpLocalFileTime
		    )
		{
			return false;
		}
		public static bool FileTimeToSystemTime(
		    FILETIME lpFileTime,
		    SYSTEMTIME lpSystemTime
		    )
		{
			return false;
		}
		
		
		public const uint WM_SETTEXT = 0x000C;
		public static long SendMessageA(
		    long/*HWND*/ hWnd,
		    uint Msg,
		    uint wParam,
		    CharPtr/*long*/ lParam) //FIXME:change to CharPtr
		{
			return 0;
		}
		
		
		public const int SW_SHOW = 5;
		public const int SW_SHOWNORMAL = 1;
		public static long ShellExecuteA(
			  object   hwnd,
			  CharPtr lpOperation,
			  CharPtr lpFile,
			  CharPtr lpParameters,
			  CharPtr lpDirectory,
			  int    nShowCmd
			) {
			
//			ProcessStartInfo processStartInfo = new ProcessStartInfo();
//            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//            processStartInfo.CreateNoWindow = true;
//            processStartInfo.UseShellExecute = false;
//            processStartInfo.RedirectStandardOutput = true;
//            processStartInfo.RedirectStandardError = true;
//            processStartInfo.FileName = "cmd";
//            processStartInfo.Arguments = "/c ";
//            processStartInfo.Arguments += str.ToString();
//
//            Process process = Process.Start(processStartInfo);
//            process.WaitForExit();
//            return process.ExitCode;

            
			return 0;
		}
		
		
		public const int MAX_PATH = 260;
		
		public const UInt32 CSIDL_COMMON_APPDATA = 0x0023; // for [Profiles]/All Users/Application Data
		public const UInt32 CSIDL_APPDATA = 0x001A; // for [Profiles]/[User]/Application Data
		public const UInt32 S_OK = (uint)((long)0x00000000L);
		public const UInt32 S_FALSE = (uint)((long)0x00000001L);
		public const UInt32 E_FAIL = (uint)((long)(0x80004005L));
		public const UInt32 E_INVALIDARG = (uint)((long)(0x80000003L));
		
		//FIXME:???
		public static long SHGetFolderPathA(int param1, UInt32 param2, int param3, int param4, CharPtr hpath)
		{
			return 0;
		}
		
				
		public static int CreateDirectory(
		    CharPtr lpPathName,
		    int lpSecurityAttributes
		    )
		{
			try {
				if (!Directory.Exists(lpPathName.ToString()))
				{
					Directory.CreateDirectory(lpPathName.ToString());
					return 1;
				}
			} catch {
			
			}
			return 0;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		public static void SDL_XXX()
		{
			
		}
		public static int SDL_BlitSurface(SDL_Surface src, SDL_Rect srcrect, SDL_Surface dst, SDL_Rect dstrect)
		{
			return 0;
		}
		public static int SDL_Surface_get_w(SDL_Surface surface)
		{
			return 0;
		}
		public static int SDL_Surface_get_h(SDL_Surface surface)
		{
			return 0;
		}
		public static void SDL_savebmp(MSD_Surface surf, CharPtr name)
		{
			
		}
		public static void SDL_FreeSurface(SDL_Surface surface)
		{
			
		}
		public static void SDL_LockSurface(SDL_Surface x)
		{
			
		}
		public static void SDL_UnlockSurface(SDL_Surface x)
		{
			
		}
		public static UnsignedCharPtr SDL_Surface_get_pixels(SDL_Surface x)
		{
			return null;
		}
		
		public static UInt32 SDL_MapRGB(SDL_PixelFormat format, 
		                                       byte r, 
		                                       byte g, 
		                                       byte b)
		{
			return 0;
		}
		public static SDL_PixelFormat SDL_Surface_get_format(SDL_Surface surface)
		{
			return null;
		}
		public static int SDL_Surface_get_pitch(SDL_Surface x)
		{
			return 0;
		}
		public static void SDL_RWclose(SDL_RWops ctx)
		{
			
		}
		public static SDL_RWops SDL_RWFromMem(UnsignedCharPtr mem, int size)
		{
			return null;
		}
		
		public const UInt32 SDL_SWSURFACE = 0x00000000;
		public static SDL_Surface SDL_CreateRGBSurface(
			UInt32 flags, int width, int height, int depth, 
			UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask)
		{
			return null;
		}
		public static UInt32 SDL_MapRGBA(SDL_PixelFormat format, 
		    byte r, byte g, byte b, byte a)
		{
			return 0;
		}
		public static int SDL_FillRect(SDL_Surface dst, SDL_Rect dstrect, UInt32 color)
		{
			return 0;
		}
		public static SDL_Surface SDL_ConvertSurface(SDL_Surface src, SDL_PixelFormat fmt, UInt32 flags)
		{
			return null;
		}
		public const byte SDL_ALPHA_OPAQUE = 255;
		public static int SDL_SetAlpha(SDL_Surface surface, UInt32 flag, byte alpha)
		{
			return 0;
		}
		public static int SDL_WaitEvent(SDL_Event event_)
		{
			return 0;
		}
		public const int SDL_RELEASED = 0;
		public const int SDL_PRESSED = 1;
		public const int SDL_NOEVENT = 0;
		public const int SDL_ACTIVEEVENT = 1; //FIXME:not implemented
		public const int SDL_KEYDOWN = 2;
		public const int SDL_KEYUP = 3;
		public const int SDL_MOUSEMOTION = 4;
		public const int SDL_MOUSEBUTTONDOWN = 5;
		public const int SDL_MOUSEBUTTONUP = 6;
		public const int SDL_JOYAXISMOTION = 7; //FIXME:not implemented
		public const int SDL_JOYBALLMOTION = 8; //FIXME:not implemented
		public const int SDL_JOYHATMOTION = 9; //FIXME:not implemented
		public const int SDL_JOYBUTTONDOWN = 10; //FIXME:not implemented
		public const int SDL_JOYBUTTONUP = 11; //FIXME:not implemented
		public const int SDL_QUIT = 12;
	//	public const int SDL_SYSWMEVENT = 13;
	//	public const int SDL_EVENT_RESERVEDA = 14;
	//	public const int SDL_EVENT_RESERVEDB = 15;
	//	public const int SDL_VIDEORESIZE = 16;
		public const int SDL_VIDEOEXPOSE = 17; //FIXME:not implemented
	//	public const int SDL_EVENT_RESERVED2 = 18;
	//	public const int SDL_EVENT_RESERVED3 = 19;
	//	public const int SDL_EVENT_RESERVED4 = 20;
	//	public const int SDL_EVENT_RESERVED5 = 21;
	//	public const int SDL_EVENT_RESERVED6 = 22;
	//	public const int SDL_EVENT_RESERVED7 = 23;
		public const int SDL_USEREVENT = 24;
		public const int SDL_NUMEVENTS = 32;
		private static UInt32 SDL_EVENTMASK(int X) { return (UInt32)(1<<(X)); }
		public static UInt32 SDL_QUITMASK = SDL_EVENTMASK(SDL_QUIT);
		public const UInt32 SDL_ALLEVENTS = 0xFFFFFFFF;
		public enum SDL_eventaction {
			SDL_ADDEVENT,
			SDL_PEEKEVENT,
			SDL_GETEVENT
		}
		public static int SDL_PeepEvents(SDL_Event events, int numevents, SDL_eventaction action, UInt32 mask)
		{
			return 0;
		}
		
		public delegate UInt32 SDL_NewTimerCallback(UInt32 interval, object param);
		public class SDL_TimerID {
			public ulong idTimer;
			public int idx;
			public SDL_NewTimerCallback callback;
			public int interval;
			public object param;
		}
		public enum SDL_bool {
			SDL_FALSE = 0,
			SDL_TRUE  = 1
		}
		public static SDL_TimerID SDL_AddTimer(UInt32 interval, SDL_NewTimerCallback callback, object param)
		{
			return null;
		}
		public static SDL_bool SDL_RemoveTimer(SDL_TimerID t)
		{
			return SDL_bool.SDL_FALSE;
		}
		public static int SDL_PushEvent(SDL_Event event_)
		{
			return 0;
		}
		public static int SDL_PollEvent(SDL_Event event_)
		{
			return 0;	
		}
		
		
		public static void SDL_UpdateRect(SDL_Surface screen, Int32 x, Int32 y, UInt32 w, UInt32 h)
		{
			return;
		}
		
		public static void SDL_WM_SetCaption(CharPtr title, CharPtr icon)
		{
			
		}
		
		public static void SDL_Delay(UInt32 ms)
		{
			
		}
		public const int SDL_BUTTON_LEFT = 1;
		public const int SDL_BUTTON_MIDDLE = 2;
		public const int SDL_BUTTON_RIGHT = 3;
		public const int SDL_BUTTON_WHEELUP = 4;
		public const int SDL_BUTTON_WHEELDOWN = 5;
		
		public static UInt32 SDL_GetTicks()
		{
			return 0;
		}
		
		public static int SDL_GetWMInfo(SDL_SysWMinfo info)
		{
			return 0;
		}
		public static int SDL_SaveBMP(SDL_Surface surface, CharPtr file)
		{
			return 0;
		}
		
		
		public static byte SDL_GetMouseState(ref int x, ref int y)
		{
			return 0;
		}
		
		public static void SDL_PumpEvents()
		{
			
		}
		
		public static void SDL_UpdateRects(SDL_Surface screen, int numrects, SDL_Rect[] rects)
		{
			
		}
		public static void SDL_Quit() 
		{
			
		}
		public const UInt32	SDL_INIT_TIMER = 0x00000001;
		public const UInt32	SDL_INIT_AUDIO = 0x00000010;
		public const UInt32	SDL_INIT_VIDEO = 0x00000020;
		public const UInt32	SDL_INIT_CDROM = 0x00000100;
		public const UInt32	SDL_INIT_JOYSTICK = 0x00000200;
		public const UInt32	SDL_INIT_NOPARACHUTE = 0x00100000;	/* Don't catch fatal signals */
		public const UInt32	SDL_INIT_EVENTTHREAD = 0x01000000;	/* Not supported on all OS's */
		public const UInt32	SDL_INIT_EVERYTHING	= 0x0000FFFF;
		public static int SDL_Init(UInt32 flags)
		{
			return 0;
		}
		public static CharPtr SDL_GetError()
		{
			return null;
		}
		public delegate void atexit_func();
		public static int atexit(atexit_func func)
		{
			return 0;
		}
		
		public static int SDL_InitSubSystem(UInt32 flags)
		{
			return 0;
		}
		public static int SDL_EnableUNICODE(int enable)
		{
			return 0;
		}
		public static SDL_VideoInfo SDL_GetVideoInfo()
		{
			return null;
		}
		
		public const UInt32 DEFAULT_VIDEO_SURFACE_FLAG = (SDL_SWSURFACE);
		public static SDL_Surface SDL_SetVideoMode(int width, int height, int bpp, UInt32 flags)
		{
			return null;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
				
		public static int TTF_Init()
		{
			return 0;
		}
		
		public static int TTF_GlyphMetrics(TTF_Font font, ushort ch, 
		                                   ref int minx, ref int maxx, 
		                                   ref int miny, ref int maxy, 
		                                   ref int advance)
		{
			return 0;	
		}
		public static int TTF_FontAscent(TTF_Font font)
		{
			return 0;
		}
		public static SDL_Surface TTF_RenderGlyph_Shaded(TTF_Font font, UInt16 ch, SDL_Color fg, SDL_Color bg)
		{
			return null;
		}
		
		public static TTF_Font TTF_OpenFont(CharPtr file, int ptsize)
		{
			return null;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		public static SDL_Surface IMG_Load_RW(SDL_RWops src, int freesrc)
		{
			return null;
		}
		public static CharPtr IMG_GetError()
		{
			return null;
		}
		
		
	}
}
