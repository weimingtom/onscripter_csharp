/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:22
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
		 *  ScriptHandler.h - Script manipulation class of ONScripter-EN
		 *
		 *  Copyright (c) 2001-2009 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2007-2011 "Uncle" Mion Sonozaki
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
		
		// Modified by Haeleth, Autumn 2006, to better support OS X/Linux packaging.
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
		//#ifndef __SCRIPT_HANDLER_H__
		//#define __SCRIPT_HANDLER_H__
		
		//#include <stdio.h>
		//#include <stdlib.h>
		//#include <string.h>
		//#include "BaseReader.h"
		//#include "DirPaths.h"
		
		public const int VARIABLE_RANGE = 4096;
		
//		//#define IS_TWO_BYTE(x) \
//		//        ( ((x) & 0xe0) == 0xe0 || ((x) & 0xe0) == 0x80 )
		public static bool IS_TWO_BYTE(int x) {
		    return ( ((x) & 0xe0) == 0xe0 || ((x) & 0xe0) == 0x80 ||
			 (((x) & 0xff) >=0xA1 && ((x) & 0xff) <=0xFE) ); }
		
//		#define MAX_ERRBUF_LEN 512
//		
//		typedef unsigned char uchar3[3];
//		
//		class ONScripterLabel;
//		
		public partial class ScriptHandler
		{
//		public:
		    public const int END_NONE       = 0;
		    public const int END_COMMA      = 1;
		    public const int END_1BYTE_CHAR = 2;
		    public const int END_COMMA_READ = 4; // for LUA
		    
		    public class LabelInfo{
		        public CharPtr name;
		        public CharPtr label_header;
		        public CharPtr start_address;
		        public int  start_line;
		        public int  num_of_lines;
		    }
		
		    public class ArrayVariable{
		        public ArrayVariable next = null;
		        public int no;
		        public int num_dim;
		        public int[] dim = new int[20];
		        public IntPtr data = null;
		        public ArrayVariable(){
		            next = null;
		            data = null;
		        }
		        ~ArrayVariable(){
		            if (null!=data) data = null;//delete[] data;
		        }
		        //FIXME:???operator=
		        public ArrayVariable assign(ArrayVariable av){
		            no = av.no;
		            num_dim = av.num_dim;
		
		            int total_dim = 1;
		            for (int i=0 ; i<20 ; i++){
		                dim[i] = av.dim[i];
		                total_dim *= dim[i];
		            }
		
		            if (data!=null) data=null;//delete[] data;
		            data = null;
		            if (null!=av.data){
		            	data = new IntPtr(new int[total_dim]);
		            	memcpy(data, av.data, (uint)(sizeof(int)*total_dim));
		            }
		
		            return this;
		        }
		    }
		
		    public const int VAR_NONE  = 0,
		           VAR_INT   = 1,  // integer
		           VAR_ARRAY = 2,  // array
		           VAR_STR   = 4,  // string
		           VAR_CONST = 8,  // direct value or alias, not variable
		           VAR_PTR   = 16  // pointer to a variable, e.g. i%0, s%0
		    ;
		    public class VariableInfo{
		        public int type;
		        public int var_no;   // for integer(%), array(?), string($) variable
		        public ArrayVariable array = new ArrayVariable(); // for array(?)
		    };
		
//		    ScriptHandler();
//		    ~ScriptHandler();
//		
//		    void reset();
//		    FILE *fopen( const char *path, const char *mode, const bool save = false, const bool usesavedir = false );
//		    FILE *fopen( const char *root, const char *path, const char *mode);
//		    void setKeyTable( const unsigned char *key_table );
//		
//		    void setSavedir( const char *dir );
//		    inline void setOns( ONScripterLabel *newons){ ons = newons; }
//		
//		    // basic parser function
//		    const char *readToken();
//		    const char *readName();
//		    const char *readColor(bool *is_color = NULL);
//		    const char *readLabel();
//		    void readVariable( bool reread_flag=false );
//		    const char *readStr();
//		    int  readInt();
//		    int  parseInt( char **buf );
//		    void skipToken();
		
		    // function for string access
		    public CharPtr getStringBuffer(){ return string_buffer; }
		    public CharPtr getSavedStringBuffer(){ return saved_string_buffer; }
//		    char *saveStringBuffer();
//		    void addStringBuffer( char ch );
//		    void trimStringBuffer( unsigned int n );
//		    void pushStringBuffer(int offset); // used in textgosub and pretextgosub
//		    int  popStringBuffer(); // used in textgosub and pretextgosub
		    
		    // function for direct manipulation of script address 
		    public CharPtr getCurrent(){ return current_script; }
		    public CharPtr getNext(){ return next_script; }
//		    void setCurrent(char *pos);
//		    void pushCurrent( char *pos );
//		    void popCurrent();
//		
//		    void enterExternalScript(char *pos); // LUA
//		    void leaveExternalScript();
//		    bool isExternalScript();
//		
//		    int  getOffset( char *pos );
//		    char *getAddress( int offset );
//		    int  getLineByAddress( char *address );
//		    char *getAddressByLine( int line );
//		    LabelInfo getLabelByAddress( char *address );
//		    LabelInfo getLabelByLine( int line );
//		
//		    bool isName( const char *name );
//		    bool isText();
//		    bool compareString( const char *buf );
		    public void setEndStatus(int val){ end_status |= val; }
		    public int getEndStatus(){ return end_status; }
//		    inline void toggle1ByteEndStatus() {
//		        if (end_status && END_1BYTE_CHAR)
//		            end_status &= ~END_1BYTE_CHAR;
//		        else
//		            end_status |= END_1BYTE_CHAR;
//		    }
//		    void skipLine( int no=1 );
//		    void setLinepage( bool val );
		    public void setEnglishMode( bool val ){ english_mode = val; }
		
//		    // function for kidoku history
//		    bool isKidoku();
//		    void markAsKidoku( char *address=NULL );
//		    void setKidokuskip( bool kidokuskip_flag );
//		    void saveKidokuData();
//		    void loadKidokuData();
//		
//		    void addStrVariable(char **buf);
//		    void addIntVariable(char **buf, bool no_zenkaku=false);
//		    void declareDim();
//		
//		    void enableTextgosub(bool val);
//		    void enableRgosub(bool val);
//		    void setClickstr( const char *list );
//		    int  checkClickstr(const char *buf, bool recursive_flag=false);
//		
//		    void setInt( VariableInfo *var_info, int val, int offset=0 );
//		    void setNumVariable( int no, int val );
//		    void pushVariable();
//		    int  getIntVariable( VariableInfo *var_info=NULL );
//		
//		    void setStr( char **dst, const char *src, int num=-1 );
//		
//		    int  getStringFromInteger( char *buffer, int no, int num_column,
//		                               bool is_zero_inserted=false,
//		                               bool use_zenkaku=false );
//		
//		    int  readScriptSub( FILE *fp, char **buf, int encrypt_mode );
//		    int  readScript( DirPaths &path );
//		    int  labelScript();
//		
//		    LabelInfo lookupLabel( const char* label );
//		    LabelInfo lookupLabelNext( const char* label );
//		
//		    ArrayVariable *getRootArrayVariable();
//		    void loadArrayVariable( FILE *fp );
//		    
//		    void addNumAlias( const char *str, int no );
//		    void addStrAlias( const char *str1, const char *str2 );
//		
//		    bool findNumAlias( const char *str, int *value );
//		    bool findStrAlias( const char *str, char* buffer );
		
		    public const int LABEL_LOG = 0;
		    public const int FILE_LOG = 1;
		    
		    public class LogLink{
		        public LogLink next = null;
		        public CharPtr name = null;
		
		        public LogLink(){
		            next = null;
		            name = null;
		        }
		        ~LogLink(){
		            if ( name!=null ) name = null;//delete[] name;
		        }
		    }
		    public class LogInfo{
		    	public LogLink root_log = new LogLink();
		        public LogLink current_log = null;
		        public int num_logs;
		        public CharPtr filename;
		    }
		    public LogInfo[] log_info = new LogInfo[2];
//		    LogLink *findAndAddLog( LogInfo &info, const char *name, bool add_flag );
//		    void resetLog( LogInfo &info );
//		    
		    /* ---------------------------------------- */
		    /* Variable */
		    public class VariableData{
		        public int num;
		        public bool num_limit_flag;
		        public int num_limit_upper;
		        public int num_limit_lower;
		        public CharPtr str;
		
		        public VariableData(){
		            str = null;
		            reset(true);
		        }
		        public void reset(bool limit_reset_flag){
		            num = 0;
		            if (limit_reset_flag)
		                num_limit_flag = false;
		            if (str != null){
		                //delete[] str;
		                str = null;
		            }
		        }
		    };
//		    VariableData &getVariableData(int no);
		
		    public VariableInfo current_variable = new VariableInfo(), pushed_variable = new VariableInfo();
		    
		    public int screen_size;
		    public const int SCREEN_SIZE_640x480 = 0;
		    public const int SCREEN_SIZE_800x600 = 1;
		    public const int SCREEN_SIZE_400x300 = 2;
		    public const int SCREEN_SIZE_320x240 = 3;
		    
		    public int global_variable_border;
		    
		    public CharPtr game_identifier;
		    public CharPtr save_path;
		    //Mion: savedir is set by savedirCommand, stores save files
		    // and main stored gamedata files except envdata
		    public CharPtr savedir;
		    public int  game_hash;
		
		    //Mion: for more helpful error msgs
		    public bool strict_warnings;
		    public char[] current_cmd = new char[64];
		    public const int CMD_NONE    = 0;
		    public const int CMD_BUILTIN = 1;
		    public const int CMD_TEXT    = 2;
		    public const int CMD_USERDEF = 3;
		    public const int CMD_UNKNOWN = 4;
		    public int current_cmd_type;
		    public char[] errbuf = new char[MAX_ERRBUF_LEN]; //intended for use creating error messages
		                                 // before they are passed to errorAndExit,
		                                 // simpleErrorAndExit or processError
//		    void processError( const char *str, const char *title=NULL,
//		                       const char *detail=NULL, bool is_warning=false,
//		                       bool is_simple=false );
//		
		    public BaseReader cBR = null;
		
		    public enum LanguageScript {
		        NO_SCRIPT_PREF  = 0,
		        LATIN_SCRIPT    = 1,
		        JAPANESE_SCRIPT = 2
		    }
		    public LanguageScript preferred_script, default_script, system_menu_script;
		
//		    //Mion: these are used to keep track of clickwait points in the script
//		    //for rgosub, to use as return script points
		    public CharPtr[] rgosub_wait_pos;
		    public bool[] rgosub_wait_1byte;
		    public int total_rgosub_wait_size;
		    public int num_rgosub_waits;
		    public int cur_rgosub_wait;
		    
		    public bool is_rgosub_click;
		    public bool rgosub_click_newpage;
		    public bool rgosub_1byte_mode;
		
		    public bool ignore_textgosub_newline;
		
//		    //Mion: onscripter-en special text escape characters
//		    enum {
//		        TXTBTN_START = 0x01, // for '<' in unmarked text as a textbtn delimiter
//		        LEFT_PAREN   = 0x02, // for '(' in text and backlog, to avoid ruby
//		        RIGHT_PAREN  = 0x03, // for ')' in text and backlog, to avoid ruby
//		        TXTBTN_END   = 0x04, // for '>' in unmarked text as a textbtn delimiter
//		        BACKSLASH    = 0x1F  // for '\' in {}-varlist strings _within script_ (indicates str newline)
//		    };
//		
//		    //Mion: for lookback text relocating
//		    //  Watch onscripter play teletype :)
//		    enum {
//		        TEXT_TAB   = 0x09,  //horizontal tab, like indenting 1 fullwidth space
//		        TEXT_LF    = 0x0A,  //newline!
//		        TEXT_VTAB  = 0x0B,  //like newline, but doesn't change position in line
//		        TEXT_FF    = 0x0C,  //implicit "locate -1,0"; form feed
//		        TEXT_CR    = 0x0D,  //implicit "locate 0,-1"; carriage return
//		        TEXT_UP    = 0x11,  //move up 1 character on screen (deprecated)
//		        TEXT_RIGHT = 0x12,  //move right 1 fullwidth character on screen (deprecated)
//		        TEXT_DOWN  = 0x13,  //move down 1 character on screen (deprecated)
//		        TEXT_LEFT  = 0x14   //move left 1 fullwidth character on screen (deprecated)
//		    };
//		    
//		private:
		    public const int OP_INVALID = 0; // 000
		    public const int OP_PLUS    = 2; // 010
		    public const int OP_MINUS   = 3; // 011
		    public const int OP_MULT    = 4; // 100
		    public const int OP_DIV     = 5; // 101
		    public const int OP_MOD     = 6;  // 110
		    
		    
		    public class Alias{
		        public Alias next = null;
		        public CharPtr alias;
		        public int  num;
		        public CharPtr str;
		
		        public Alias(){
		            next = null;
		            alias = null;
		            str = null;
		        }
		        public Alias( CharPtr name, int num ){
		            next = null;
		            alias = new char[ strlen(name) + 1];
		            strcpy( alias, name );
		            str = null;
		            this.num = num;
		        }
		        public Alias( CharPtr name, CharPtr str ){
		            next = null;
		            alias = new char[ strlen(name) + 1];
		            strcpy( alias, name );
		            this.str = new char[ strlen(str) + 1];
		            strcpy( this.str, str );
		        }
		        ~Alias(){
		            if (null!=alias) alias = null;//delete[] alias;
		            if (null!=str)   str = null;//delete[] str;
		        }
		    }
		    
//		    int findLabel( const char* label );
//		
//		    char *checkComma( char *buf );
//		    void parseStr( char **buf );
//		    int  parseIntExpression( char **buf );
//		    void readNextOp( char **buf, int *op, int *num );
//		    int  calcArithmetic( int num1, int op, int num2 );
//		    int  parseArray( char **buf, ArrayVariable &array );
//		    int  *getArrayPtr( int no, ArrayVariable &array, int offset );
		
		    /* ---------------------------------------- */
		    /* Variable */
		    public VariableData[] variable_data = null;
		    public class ExtendedVariableData{
		        public int no;
		        public VariableData vd = new VariableData();
		    }
		    public ExtendedVariableData[] extended_variable_data = null;
		    public int num_extended_variable_data;
		    public int max_extended_variable_data;
//		    struct TmpVariableDataLink{
//		        VariableInfo vi;
//		        int num;
//		        char *str;
//		        TmpVariableDataLink *next;
//		        TmpVariableDataLink(){
//		            str = NULL;
//		            next = NULL;
//		        };
//		        ~TmpVariableDataLink(){
//		            if (str) delete[] str;
//		        };
//		    } tmp_variable_data_link;
		
		    public Alias root_num_alias = new Alias(), last_num_alias = null;
		    public Alias root_str_alias = new Alias(), last_str_alias = null;
		    
		    public ArrayVariable root_array_variable = null, current_array_variable = null;
		
		    public ONScripterLabel ons = null; //Mion: so script_h can call doErrorBox
//		    void errorAndExit( const char *str, const char *title=NULL, const char *detail=NULL, bool is_warning=false );
//		    void simpleErrorAndExit( const char *str, const char *title=NULL, const char *detail=NULL, bool is_warning=false );
		
		    public DirPaths archive_path; //points to ScriptParser's archive_path
		    public CharPtr script_path;
		    public int  script_buffer_length;
		    public CharPtr script_buffer;
		    public CharPtr tmp_script_buf;
		    
		    public CharPtr string_buffer; // update only be readToken
		    public int  string_counter;
		    public CharPtr saved_string_buffer; // updated only by saveStringBuffer
		    public CharPtr str_string_buffer; // updated only by readStr
		    public CharPtr gosub_string_buffer; // used in textgosub and pretextgosub
		    public int gosub_string_offset; // used in textgosub and pretextgosub
		
		    public LabelInfo[] label_info = null;
		    public int num_of_labels;
		
		    public bool skip_enabled;
		    public bool kidokuskip_flag;
		    public CharPtr kidoku_buffer = null;
		
		    public bool text_flag; // true if the current token is text
		    public int  end_status;
		    public bool linepage_flag;
		    public bool textgosub_flag;
		    public bool rgosub_flag;
		    public CharPtr clickstr_list;
		    public bool english_mode;
		
		    public CharPtr current_script;
		    public CharPtr next_script;
		
		    public CharPtr pushed_current_script;
		    public CharPtr pushed_next_script;
		
		    public CharPtr internal_current_script;
		    public CharPtr internal_next_script;
		    public int  internal_end_status;
		    public VariableInfo internal_current_variable = new VariableInfo(), internal_pushed_variable = new VariableInfo();
		
		    public byte[] key_table = new byte[256];
		    public bool key_table_flag;
		};
		
		
		//#endif // __SCRIPT_HANDLER_H__
	}
}
