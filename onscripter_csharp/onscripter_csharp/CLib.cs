/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-9-12
 * Time: 10:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	public partial class ONScripter {
		public const int EOF = -1; //FIXME:????
		public const int SEEK_SET = 1; //FIXME:
		public const int SEEK_END = 2; //FIXME:
		public const FILEPtr stdout = null;//FIXME:
		public const FILEPtr stderr = null;//FIXME:
		
		
		
		public class time_t {
			
		}
		
		public class tm {
			public int tm_year, tm_mon, tm_mday;
			public int tm_hour, tm_min, tm_sec;
		}
		
		public static time_t time(time_t t) {
			return new time_t();
		}
		
		public static tm localtime(time_t t) {
			return new tm();
		}
		
		public static CharPtr strcpy(CharPtr dst, CharPtr src)
		{
			return null;
		}
		public static CharPtr strncpy(CharPtr dst, CharPtr src, uint n)
		{
			return null;
		}
		
		public static uint strlen(CharPtr str)
		{
			return 0;
		}
		
				
		public static CharPtr strcat(CharPtr dst, CharPtr src)
		{
			return null;	
		}
		
		public static void printf(string str, params Object[] args) 
		{
			
		}
		
		public static void snprintf(CharPtr dst, int n, string str, params Object[] args)
		{
			
		}
		
		public static int strcmp(CharPtr s1, CharPtr s2)
		{
			return 0;
		}
		
		public static int atoi(CharPtr str)
		{
			return 0;
		}
		
		public static double cos(double x)
		{
			return 0;
		}
		
		public static void sprintf(CharPtr dst, CharPtr str, params Object[] args)
		{
			
		}
		
		public static void memcpy(CharPtr s1, CharPtr s2, uint length)
		{
			
		}
		public static void memcpy(IntPtr s1, IntPtr s2, uint length)
		{
			
		}
		public static void memcpy(UnsignedCharPtr s1, UnsignedCharPtr s2, uint length)
		{
			
		}
		public static void memcpy(UnsignedCharPtr s1, WAVE_HEADER s2, uint length)
		{
			
		}
		public static void memcpy(CharPtr s1, UnsignedCharPtr s2, uint length)
		{
			
		}
		public static void memcpy(UnsignedCharPtr s1, CharPtr s2, uint length)
		{
			
		}
		public static void memcpy(ScriptHandler.ExtendedVariableData[] s1, ScriptHandler.ExtendedVariableData s2, uint length)
		{
			//FIXME:???
		}
		
		
		public static CharPtr strrchr(CharPtr str, char ch)
		{
			return null;
		}
		
		public static int strncmp(CharPtr str1, CharPtr str2, int n)
		{
			return 0;
		}
		
		public static double sin(double d)
		{
			return 0;
		}
		
		public static double tan(double d)
		{
			return 0;
		}
		
		public static FILEPtr fopen( CharPtr path, CharPtr mode)
		{
			return null;
		}
		
		public static int fgetc(FILEPtr fp)
		{
			return 0;
		}
		
		public static void fclose(FILEPtr fp)
		{
			
		}
		
		public static void fseek(FILEPtr fp, int pos, int mode)
		{
			
		}

		public static long ftell(FILEPtr fp)
		{
			return 0;
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
		
		public static int remove(CharPtr str)
		{
			return 0;
		}
		public static int rename(CharPtr str,CharPtr str2)
		{
			return 0;
		}
		public static int fputs(CharPtr buffer, FILEPtr fp)
		{
			return 0;
		}
		public static int fputc(char buffer, FILEPtr fp)
		{
			return 0;
		}
		
		public static void fflush(FILEPtr fp)
		{
		}
		
		public static void OutputDebugString(CharPtr str)
		{
			
		}
		
		public static void srand(time_t t)
		{
			
		}
		
		public static double rand()
		{
			return 0;
		}
		
		public static void memset(IntPtr s, int ch, uint n)
		{
			
		}
		public static void memset(CharPtr s, int ch, uint n)
		{
			
		}
		public static void memset(UnsignedLongPtr s, int ch, uint n)
		{
			
		}
		
		public static void exit(int a)
		{
			
		}
		
		public static void fprintf(FILEPtr fp, string str, params Object[] args) 
		{
			
		}
		
		public static CharPtr fgets(CharPtr str, int n, FILEPtr stream)
		{
			return null;
		}
		
		public static int ferror(FILEPtr stream)
		{
			return 0;
		}
		
		public static void mkdir(CharPtr str)
		{
			
		}
		
		public static void SDL_XXX()
		{
			
		}
		
		public static int SDL_BlitSurface(SDL_Surface src, SDL_Rect srcrect, SDL_Surface dst, SDL_Rect dstrect)
		{
			return 0;
		}
		
		public static int MultiByteToWideChar(uint     CodePage,
			    long    dwFlags,
			    CharPtr   lpMultiByteStr,
			    int      cchMultiByte,
			    UnsignedShortPtr   lpWideCharStr,
			    int      cchWideChar)
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
		public static void SDL_FreeSurface(SDL_Surface surface)
		{
			
		}
		public static int sizeof_WAVE_HEADER() {
			return 0;
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
		public static SDL_Surface IMG_Load_RW(SDL_RWops src, int freesrc)
		{
			return null;
		}
		public static CharPtr IMG_GetError()
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
		
	}
}
