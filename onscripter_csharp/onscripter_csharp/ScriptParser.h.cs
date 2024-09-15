/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	public partial class ONScripter
	{
		/* -*- C++ -*-
		 * 
		 *  ScriptParser.h - Define block parser of ONScripter-EN
		 *
		 *  Copyright (c) 2001-2009 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2007-2010 "Uncle" Mion Sonozaki
		 *
		 *  UncleMion@gmail.com
		 *
		 *  This program is free software; you can redistribute it and/or modify
		 *  it under the terms of the GNU General Public License as published by
		 *  the Free Software Foundation; either version 2 of the License, or
		 *  (at your option) any later version.
		 *
		 *  This program is distributed in the hope that it will be useful,
		 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
		 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
		 *  GNU General Public License for more details.
		 *
		 *  You should have received a copy of the GNU General Public License
		 *  along with this program; if not, see <http://www.gnu.org/licenses/>
		 *  or write to the Free Software Foundation, Inc.,
		 *  59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
		 */
		
		// Modified by Haeleth, Autumn 2006, to better support OS X/Linux packaging;
		// and since then numerous other times (see SVN changelog for full details)
		
		// Modified by Mion of Sonozaki Futago-tachi, March 2008, to update from
		// Ogapee's 20080121 release source code.
		
//		#ifndef __SCRIPT_PARSER_H__
//		#define __SCRIPT_PARSER_H__
//		
//		#include <stdio.h>
//		#include <stdlib.h>
//		#include <string.h>
//		#include <math.h>
//		#include <time.h>
//		
//		#include "DirPaths.h"
//		#include "ScriptHandler.h"
//		#include "NsaReader.h"
//		#include "DirectReader.h"
//		#include "AnimationInfo.h"
//		#include "FontInfo.h"
//		#include "Layer.h"
//		#ifdef USE_LUA
//		#include "LUAHandler.h"
//		#endif
//		
//		#ifndef M_PI
//		#define M_PI 3.14159265358979323846
//		#endif
		
		public const int DEFAULT_FONT_SIZE = 26;
		
		public const string DEFAULT_LOOKBACK_NAME0 = "uoncur.bmp";
		public const string DEFAULT_LOOKBACK_NAME1 = "uoffcur.bmp";
		public const string DEFAULT_LOOKBACK_NAME2 = "doncur.bmp";
		public const string DEFAULT_LOOKBACK_NAME3 = "doffcur.bmp";
		
		// Mion: kinsoku
		public const string DEFAULT_START_KINSOKU = "乿亁乯乶乸丄丅丆丏丒丠両丷丼乀乁乆乕";
		public const string DEFAULT_END_KINSOKU   = "乽亀乮乵乷";
		
//		typedef unsigned char uchar3[3];
		
		public class OVInfo{
		    //SDL_AudioCVT cvt;
		    public int cvt_len;
		    public int mult1;
		    public int mult2;
		    public UnsignedCharPtr buf;
		    public long decoded_length;
		}
		
		public partial class ScriptParser
		{
//		public:
			public class MusicStruct{
		        public int volume;
		        public bool is_mute;
		        public MusicStruct()
		        { volume = (0); is_mute = (false); }
		    }
		
//		    ScriptParser();
//		    virtual ~ScriptParser();
//		
//		    void reset();
//		    void resetDefineFlags(); // for resetting (non-pointer) variables
//		    int open();
//		    int parseLine();
//		    void setCurrentLabel( const char *label );
//		    void gosubReal( const char *label, char *next_script, bool textgosub_flag=false, int rgosub_state=CLICK_NONE, bool rgosub_1byte=false );
//		
//		    FILE *fopen(const char *path, const char *mode, const bool save = false, const bool usesavedir = false);
//		    void saveGlovalData();
//		    void setArchivePath(const char *path);
//		    void setSavePath(const char *path);
//		    void setNsaOffset(const char *off);
//		
//		#ifdef MACOSX
//		    void checkBundled();
//		    bool isBundled() {return is_bundled; }
//		    char *bundleResPath() { return bundle_res_path; }
//		    char *bundleAppPath() { return bundle_app_path; }
//		    char *bundleAppName() { return bundle_app_name; }
//		#endif
//		
//		    /* Command */
//		    int zenkakkoCommand();
//		    int windowchipCommand();
//		    int windowbackCommand();
//		    int versionstrCommand();
//		    int usewheelCommand();
//		    int useescspcCommand();
//		    int underlineCommand();
//		    int transmodeCommand();
//		    int timeCommand();
//		    int textgosubCommand();
//		    int tanCommand();
//		    int subCommand();
//		    int straliasCommand();
//		    int soundpressplginCommand();
//		    int skipCommand();
//		    int sinCommand();
//		    int shadedistanceCommand();
//		    int setlayerCommand();
//		    int setkinsokuCommand();
//		    int selectvoiceCommand();
//		    int selectcolorCommand();
//		    int savenumberCommand();
//		    int savenameCommand();
//		    int savedirCommand();
//		    int rubyonCommand();
//		    int rubyoffCommand();
//		    int roffCommand();
//		    int rmenuCommand();
//		    int rgosubCommand();
//		    int returnCommand();
//		    int pretextgosubCommand();
//		    int pagetagCommand();
//		    int numaliasCommand();
//		    int nsadirCommand();
//		    int nsaCommand();
//		    int nextCommand();
//		    int mulCommand();
//		    int movCommand();
//		    int mode_wave_demoCommand();
//		    int mode_sayaCommand();
//		    int mode_extCommand();
//		    int modCommand();
//		    int midCommand();
//		    int menusetwindowCommand();
//		    int menuselectvoiceCommand();
//		    int menuselectcolorCommand();
//		    int maxkaisoupageCommand();
//		    int luasubCommand();
//		    int luacallCommand();
//		    int lookbackspCommand();
//		    int lookbackcolorCommand();
//		    //int lookbackbuttonCommand();
//		    int loadgosubCommand();
//		    int linepageCommand();
//		    int lenCommand();
//		    int labellogCommand();
//		    int kidokuskipCommand();
//		    int kidokumodeCommand();
//		    int itoaCommand();
//		    int intlimitCommand();
//		    int incCommand();
//		    int ifCommand();
//		    int humanzCommand();
//		    int humanposCommand();
//		    int gotoCommand();
//		    int gosubCommand();
//		    int globalonCommand();
//		    int getparamCommand();
//		    //int gameCommand();
//		    int forCommand();
//		    int filelogCommand();
//		    int errorsaveCommand();
//		    int englishCommand();
//		    int effectcutCommand();
//		    int effectblankCommand();
//		    int effectCommand();
//		    int dsoundCommand();
//		    int divCommand();
//		    int dimCommand();
//		    int defvoicevolCommand();
//		    int defsubCommand();
//		    int defsevolCommand();
//		    int defmp3volCommand();
//		    int defaultspeedCommand();
//		    int defaultfontCommand();
//		    int decCommand();
//		    int dateCommand();
//		    int cosCommand();
//		    int cmpCommand();
//		    int clickvoiceCommand();
//		    int clickstrCommand();
//		    int clickskippageCommand();
//		    int btnnowindoweraseCommand();
//		    int breakCommand();
//		    int atoiCommand();
//		    int arcCommand();
//		    int addnsadirCommand();
//		    int addkinsokuCommand();
//		    int addCommand();
//		    
//		    void add_debug_level();
//		    void errorAndExit( const char *str, const char *reason=NULL, const char *title=NULL, bool is_simple=false );
//		    void errorAndCont( const char *str, const char *reason=NULL, const char *title=NULL, bool is_simple=false );
		
		    //Mion: syntax flags
		    public bool allow_color_type_only;    // only allow color type (#RRGGBB) for args of color type,
		                                   // i.e. not string variables
		    public bool set_tag_page_origin_to_1; // 'gettaglog' will consider the current page as 1, not 0
		    public bool answer_dialog_with_yes_ok;// give 'yesnobox' and 'okcancelbox' 'yes/ok' results
		    public CharPtr readColorStr() {
		    	if (allow_color_type_only) {
		            bool temp = false; return script_h.readColor(ref temp, false);
		    	} else
		            return script_h.readStr();
		    }
//		protected:
		    public class UserFuncLUT{
		        public UserFuncLUT next;
		        public CharPtr command;
		        public bool lua_flag;
		        public UserFuncLUT() { next = (null); command = (null); lua_flag = (false); }
		        ~UserFuncLUT(){
		            if (command != null) command = null;
		        }
		    }
		
		    public class UserFuncHash {
				public UserFuncLUT root = new UserFuncLUT();
		        public UserFuncLUT last = null;
		    } 
		    public UserFuncHash[] user_func_hash = user_func_hash_init(); 
		    private static UserFuncHash[] user_func_hash_init() 
		    {
		    	UserFuncHash[] result = new UserFuncHash['z'-'a'+1];
		    	for (int i = 0; i < result.Length; ++i)
		    	{
		    		result[i] = new UserFuncHash();
		    	}
		    	return result;
		    }
		
		    public class NestInfo{
		        public const int LABEL = 0;
		        public const int FOR   = 1;
		        public NestInfo previous = null, next = null;
		        public int  nest_mode;
		        public CharPtr next_script; // points into script_buffer; used in gosub and for
		        public int  var_no, to, step; // used in for
		        public bool textgosub_flag; // used in textgosub and pretextgosub
		        public int  rgosub_click_state; // used for rgosub
		        public bool rgosub_1byte_mode; // used for rgosub
		
//		        NestInfo()
//		        : previous(NULL), next(NULL), nest_mode(LABEL),
//		          next_script(NULL), var_no(0), to(0), step(0),
//		          textgosub_flag(false),
//		          rgosub_click_state(CLICK_NONE), rgosub_1byte_mode(false) {}
//		        //pointers previous, next, & next_script do not need to be freed
		    }
			public NestInfo last_tilde = new NestInfo();
		
		    public const int SYSTEM_NULL        = 0;
		    public const int SYSTEM_SKIP        = 1;
		    public const int SYSTEM_RESET       = 2;
		    public const int SYSTEM_SAVE        = 3;
		    public const int SYSTEM_LOAD        = 4;
		    public const int SYSTEM_LOOKBACK    = 5;
		    public const int SYSTEM_WINDOWERASE = 6;
		    public const int SYSTEM_MENU        = 7;
		    public const int SYSTEM_YESNO       = 8;
		    public const int SYSTEM_AUTOMODE    = 9;
		    public const int SYSTEM_END         = 10;
		    
		    public const int RET_NOMATCH   = 0,
		           RET_SKIP_LINE = 1,
		           RET_CONTINUE  = 2,
		           RET_NO_READ   = 4,
		           RET_EOL       = 8, // end of line (0x0a is found)
		           RET_EOT       = 16 // end of text (the end of string_buffer is reached)
		    ;
//		#if MSC_VER <= 1200
//			public:
//		#endif
		    public const int CLICK_NONE    = 0;
		    public const int CLICK_WAIT    = 1;
		    public const int CLICK_NEWPAGE = 2;
		    public const int CLICK_WAITEOL = 4;
		    
//		#if MSC_VER <= 1200
//			protected:
//		#endif
		    public const int NORMAL_MODE = 0, DEFINE_MODE = 1;
		    public int current_mode;
		    public int debug_level;
		
		#if MACOSX
		    bool is_bundled;
		    char *bundle_res_path;
		    char *bundle_app_path;
		    char *bundle_app_name;
		#endif
		    public CharPtr cmdline_game_id;
		    public DirPaths archive_path = new DirPaths();
		    public DirPaths nsa_path = new DirPaths();
		    public int nsa_offset = 0;
		    public bool globalon_flag;
		    public bool labellog_flag;
		    public bool filelog_flag;
		    public bool kidokuskip_flag;
		    public bool kidokumode_flag;
		
		    public bool clickskippage_flag;
		
		    public int z_order;
		    public bool rmode_flag;
		    public bool windowback_flag;
		    public bool btnnowindowerase_flag;
		    public bool usewheel_flag;
		    public bool useescspc_flag;
		    public bool mode_wave_demo_flag;
		    public bool mode_saya_flag;
		    public bool mode_ext_flag;
		    public bool force_button_shortcut_flag;
		    public bool rubyon_flag;
		    public bool rubyon2_flag;
		    public bool zenkakko_flag;
		    public bool pagetag_flag;
		    public int  windowchip_sprite_no;
		    
		    public int string_buffer_offset;
		
		    public NestInfo root_nest_info = new NestInfo(), last_nest_info = null;
		    public ScriptHandler.LabelInfo current_label_info = new ScriptHandler.LabelInfo();
		    public int current_line;
		
//		#ifdef USE_LUA
//		    LUAHandler lua_handler;
//		#endif
//		
		    /* ---------------------------------------- */
		    /* Global definitions */
		#if true//RCA_SCALE
		    public float scr_stretch_x, scr_stretch_y;
		#endif
		    public int preferred_width;
		    public int script_width, script_height;
		    public int screen_ratio1, screen_ratio2;
		    public int screen_width, screen_height;
		    public int screen_texture_width, screen_texture_height;
		    public int screen_bpp;
		    public CharPtr version_str;
		    public int underline_value;
		    public int[] humanpos = new int[3]; // l,c,r
		    public CharPtr savedir;
		
//		    void deleteNestInfo();
//		    void setStr( char **dst, const char *src, int num=-1 );
//		    
//		    void readToken();
//		
		    /* ---------------------------------------- */
		    /* Effect related variables */
		    public class EffectLink{
		        public EffectLink next = null;
		        public int no;
		        public int effect;
		        public int duration;
		        public AnimationInfo anim = new AnimationInfo();
		
		        public EffectLink(){
		            next = null;
		            effect = 10;
		            duration = 0;
		        }
		    }
		    
		    public EffectLink root_effect_link = new EffectLink(), last_effect_link = null, window_effect = new EffectLink(), tmp_effect = new EffectLink();
		    
		    public int effect_blank;
		    public bool effect_cut_flag;
		
//		    int readEffect( EffectLink *effect );
//		    EffectLink *parseEffect(bool init_flag);
//		
//		    /* ---------------------------------------- */
		#if !NO_LAYER_EFFECTS
		    /* Layer related variables */ //Mion
		    public class LayerInfo{
		        public LayerInfo next = null;
		        public Layer handler = null;
		        public int num;
		        public UInt32 interval;
		        public UInt32 last_update;
		        public LayerInfo(){
		            num = -1;
		            interval = last_update = 0;
		            handler = null;
		            next = null;
		        }
		        ~LayerInfo(){
		            if (handler!=null) {
		                //delete handler;
		                handler = null;
		            }
		        }
		    }
		    public LayerInfo layer_info = null;
//		    void deleteLayerInfo();
		#endif
		    /* ---------------------------------------- */
		    /* Lookback related variables */
		    //char *lookback_image_name[4];
		    public int[] lookback_sp = new int[2];
		    public byte[] lookback_color = new Byte[3];
		    
		    /* ---------------------------------------- */
		    /* For loop related variables */
		    public bool break_flag;
		    
		    /* ---------------------------------------- */
		    /* Transmode related variables */
		    public int trans_mode;
		    
		    /* ---------------------------------------- */
		    /* Save/Load related variables */
		    public class SaveFileInfo{
		        public bool valid;
		        public int  month, day, hour, minute;
		        public char[] sjis_no = new char[5];
		        public char[] sjis_month = new char[5];
		        public char[] sjis_day = new char[5];
		        public char[] sjis_hour = new char[5];
		        public char[] sjis_minute = new char[5];
		    }
		    public uint num_save_file;
		    public CharPtr save_menu_name;
		    public CharPtr load_menu_name;
		    public CharPtr save_item_name;
//		    void setDefaultMenuLabels();
		
		    public UnsignedCharPtr save_data_buf;
		    public UnsignedCharPtr file_io_buf;
		    public uint file_io_buf_ptr;
		    public uint file_io_buf_len;
		    public uint save_data_len;
		    
		    public bool errorsave;
		    
		    /* ---------------------------------------- */
		    /* Text related variables */
		    public CharPtr default_env_font;
		    public int[] default_text_speed = new int[3];
		    public class Page{
		        public Page next, previous;
		
		        public CharPtr text;
		        public int max_text;
		        public int text_count;
		        public CharPtr tag;
		
		        public Page() { next = (null); previous = (null);
		                text = (null); max_text = (0); text_count = (0); tag = (null); }
		        ~Page(){
		            if (null!=text) /*delete[] text;*/ text = null;
		            if (null!=tag)  /*delete[] tag;*/  tag = null;
		            next = previous = null;
		        }
		        public int add(byte ch){
		            if (text_count >= max_text) return -1;
		            text[text_count++] = (char)ch;
		            return 0;
		        }
		    }
		    public Page[] page_list = null; public Page start_page = null, current_page = null; // ring buffer
		    public int  max_page_list;
		    public int  clickstr_line;
		    public int  clickstr_state;
		    public int  linepage_mode;
		    public int  num_chars_in_sentence;
		    public bool english_mode;
		
		    public class Kinsoku {
		    	public char[] chr = new char[2];
//		    	private char[] chr_ = new char[2];
//		    	
//		    	public char[] chr {
//		    		get { return chr_; }
//		    		set { 
//		    			chr_ = value; 
//		    		}
//		    	}
		    } 
		    public Kinsoku[] start_kinsoku = null; public Kinsoku[] end_kinsoku = null; //Mion: for kinsoku chars
		    public int num_start_kinsoku, num_end_kinsoku;
//		    void setKinsoku(const char *start_chrs, const char *end_chrs, bool add); //Mion
//		    bool isStartKinsoku(const char *str);
//		    bool isEndKinsoku(const char *str);
//		    
//		    /* ---------------------------------------- */
//		    /* Sound related variables */
//		    MusicStruct music_struct;
		    public int music_volume;
		    public int voice_volume;
		    public int se_volume;
		    public bool use_default_volume;
		
		    public const int CLICKVOICE_NORMAL  = 0;
		    public const int CLICKVOICE_NEWPAGE = 1;
		    public const int CLICKVOICE_NUM     = 2;
		    
		    public CharPtr[] clickvoice_file_name = new CharPtr[CLICKVOICE_NUM];
		
		    public const int SELECTVOICE_OPEN   = 0;
		    public const int SELECTVOICE_OVER   = 1;
		    public const int SELECTVOICE_SELECT = 2;
		    public const int SELECTVOICE_NUM    = 3;
		    
		    public CharPtr[] selectvoice_file_name = new CharPtr[SELECTVOICE_NUM];
		
		    public const int MENUSELECTVOICE_OPEN   = 0;
		    public const int MENUSELECTVOICE_CANCEL = 1;
		    public const int MENUSELECTVOICE_OVER   = 2;
		    public const int MENUSELECTVOICE_CLICK  = 3;
		    public const int MENUSELECTVOICE_WARN   = 4;
		    public const int MENUSELECTVOICE_YES    = 5;
		    public const int MENUSELECTVOICE_NO     = 6;
		    public const int MENUSELECTVOICE_NUM    = 7;
		    
		    public CharPtr[] menuselectvoice_file_name = new CharPtr[MENUSELECTVOICE_NUM];
		     
		    /* ---------------------------------------- */
		    /* Font related variables */
		    public Fontinfo current_font = null, sentence_font = new Fontinfo(), menu_font = new Fontinfo(), ruby_font = new Fontinfo();
		    public class RubyStruct{
		        public const int NONE = 0;
		        public const int BODY = 1;
		        public const int RUBY = 2;
		        public int stage;
		        public int body_count;
		        public CharPtr ruby_start;
		        public CharPtr ruby_end;
		        public int ruby_count;
		        public int margin;
		
		        public int[] font_size_xy = new int[2];
		        public CharPtr font_name;
		
		        public RubyStruct(){
		            stage = NONE;
		            font_size_xy[0] = 0;
		            font_size_xy[1] = 0;
		            font_name = null;
		        }
		        ~RubyStruct(){
		            if ( null!=font_name ) font_name = null;
		        }
		    }
		    public RubyStruct ruby_struct = new RubyStruct();
		    public int[] shade_distance = new int[2];
		
		    /* ---------------------------------------- */
		    /* RMenu related variables */
		    public class RMenuLink{
		        public RMenuLink next;
		        public CharPtr label;
		        public int system_call_no;
		
		        public RMenuLink(){
		            next  = null;
		            label = null;
		        }
		        ~RMenuLink(){
		            if (label!=null) label=null;
		        }
		    } 
		    public RMenuLink root_rmenu_link = new RMenuLink();
		    public uint rmenu_link_num, rmenu_link_width;
		
//		    void deleteRMenuLink();
//		    int getSystemCallNo( const char *buffer );
//		    unsigned char convHexToDec( char ch );
//		
//		    void setColor( uchar3 &dstcolor, uchar3 srccolor );
//		    void readColor( uchar3 *color, const char *buf );
//		    
//		    void allocFileIOBuf();
//		    int saveFileIOBuf( const char *filename, int offset=0, const char *savestr=NULL );
//		    int loadFileIOBuf( const char *filename );
//		
//		    void writeChar( char c, bool output_flag );
//		    char readChar();
//		    void writeInt( int i, bool output_flag );
//		    int readInt();
//		    void writeStr( char *s, bool output_flag );
//		    void readStr( char **s );
//		    void writeVariables( int from, int to, bool output_flag );
//		    void readVariables( int from, int to );
//		    void writeArrayVariable( bool output_flag );
//		    void readArrayVariable();
//		    void writeLog( ScriptHandler::LogInfo &info );
//		    void readLog( ScriptHandler::LogInfo &info );
//		
		    /* ---------------------------------------- */
		    /* System customize related variables */
		    public CharPtr textgosub_label;
		    public CharPtr pretextgosub_label;
		    public CharPtr loadgosub_label;
		    public CharPtr rgosub_label;
		
		    public ScriptHandler script_h = new ScriptHandler();
		    
		    public UnsignedCharPtr key_table;
		
//		    void createKeyTable( const char *key_exe );
		};
		
//		#endif // __SCRIPT_PARSER_H__
	}
}
