/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 14:03
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
		 *  ScriptHandler.cpp - Script manipulation class of ONScripter-EN
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
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#pragma warning(disable:4996)
//		#endif
//		
//		#include "ScriptHandler.h"
//		#include "ONScripterLabel.h" //so this can call doErrorBox
//		#include <sys/stat.h>
//		#include <sys/types.h>
//		#ifdef WIN32
//		#include <direct.h>
//		#include <windows.h>
//		#endif
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
		
		public const int TMP_SCRIPT_BUF_LEN = 4096;
		public const int STRING_BUFFER_LENGTH = 2048;
		
		public static void SKIP_SPACE(CharPtr p) { while ( p[0] == ' ' || p[0] == '\t' ) p.inc(); }
		
		public partial class ScriptHandler {
			public ScriptHandler()
			{
			    num_of_labels = 0;
			    script_buffer = null;
			    kidoku_buffer = null;
			    log_info[LABEL_LOG].filename = "NScrllog.dat";
			    log_info[FILE_LOG].filename  = "NScrflog.dat";
			    clickstr_list = null;
			
			    string_buffer       = new char[STRING_BUFFER_LENGTH];
			    str_string_buffer   = new char[STRING_BUFFER_LENGTH];
			    saved_string_buffer = new char[STRING_BUFFER_LENGTH];
			    gosub_string_buffer = new char[STRING_BUFFER_LENGTH];
			
			    variable_data = new VariableData[VARIABLE_RANGE];
			    extended_variable_data = null;
			    num_extended_variable_data = 0;
			    max_extended_variable_data = 1;
			    root_array_variable = null;
			
			    screen_size = SCREEN_SIZE_640x480;
			    global_variable_border = 200;
			    
			    ons = null;
			    
			    archive_path = null;
			    save_path = null;
			    savedir = null;
			    script_path = null;
			    game_identifier = null;
			    game_hash = 0;
			    current_cmd[0] = '\0';
			    current_cmd_type = CMD_NONE;
			    strict_warnings = false;
			
			    default_script = LanguageScript.NO_SCRIPT_PREF;
			    preferred_script = LanguageScript.NO_SCRIPT_PREF;
			    system_menu_script = LanguageScript.NO_SCRIPT_PREF;
			
			    rgosub_wait_pos = null;
			    rgosub_wait_1byte = null;
			    total_rgosub_wait_size = num_rgosub_waits = cur_rgosub_wait = 0;
			    is_rgosub_click = rgosub_click_newpage = rgosub_1byte_mode = false;
			    rgosub_flag = false; // roto 20100205
			
			    ignore_textgosub_newline = false;
			}
			
			~ScriptHandler()
			{
			    reset();
			
			    if ( null!=script_buffer ) script_buffer = null;//delete[] script_buffer;
			    if ( null!=kidoku_buffer ) kidoku_buffer = null;//delete[] kidoku_buffer;
			
			    string_buffer=null;//delete[] string_buffer;
			    str_string_buffer = null;// delete[] str_string_buffer;
			    saved_string_buffer = null;//delete[] saved_string_buffer;
			    gosub_string_buffer = null;//delete[] gosub_string_buffer;
			    variable_data = null;//delete[] variable_data;
			    
			    if (null!=script_path) script_path=null;//delete[] script_path;
			    if (null!=save_path) save_path=null;//delete[] save_path;
			    if (null!=savedir) savedir=null;//delete[] savedir;
			
			    if (null!=game_identifier) game_identifier=null;//delete[] game_identifier;
			}
			
			public void reset()
			{
			    for (int i=0 ; i<VARIABLE_RANGE ; i++)
			        variable_data[i].reset(true);
			
			    if (null!=extended_variable_data) extended_variable_data = null;//delete[] extended_variable_data;
			    extended_variable_data = null;
			    num_extended_variable_data = 0;
			    max_extended_variable_data = 1;
			
			    ArrayVariable av = root_array_variable;
			    while(null!=av){
			        ArrayVariable tmp = av;
			        av = av.next;
			        tmp = null;//delete tmp;
			    }
			    root_array_variable = current_array_variable = null;
			
			    // reset log info
			    resetLog( log_info[LABEL_LOG] );
			    resetLog( log_info[FILE_LOG] );
			
			    // reset number alias
			    Alias alias;
			    alias = root_num_alias.next;
			    while (null!=alias){
			        Alias tmp_ = alias;
			        alias = alias.next;
			        tmp_ = null;//delete tmp;
			    };
			    last_num_alias = root_num_alias;
			    last_num_alias.next = null;
			
			    // reset string alias
			    alias = root_str_alias.next;
			    while (null!=alias){
			        Alias tmp = alias;
			        alias = alias.next;
			        tmp = null;//delete tmp;
			    };
			    last_str_alias = root_str_alias;
			    last_str_alias.next = null;
			
			    // reset misc. variables
			    end_status = END_NONE;
			    kidokuskip_flag = false;
			    text_flag = true;
			    linepage_flag = false;
			    english_mode = false;
			    textgosub_flag = false;
			    skip_enabled = false;
			    if (null!=clickstr_list){
			        //delete[] clickstr_list;
			        clickstr_list = null;
			    }
			    internal_current_script = null;
			    preferred_script = default_script;
			
			    if (null!=rgosub_wait_pos) rgosub_wait_pos = null;//delete[] rgosub_wait_pos;
			    rgosub_wait_pos = null;
			    if (null!=rgosub_wait_1byte) rgosub_wait_1byte = null;//delete[] rgosub_wait_1byte;
			    rgosub_wait_1byte = null;
			    total_rgosub_wait_size = num_rgosub_waits = cur_rgosub_wait = 0;
			    is_rgosub_click = rgosub_click_newpage = rgosub_1byte_mode = false;
			}
			
			public FILEPtr fopen( CharPtr path, CharPtr mode, bool save=false, bool usesavedir=false )
			{
				CharPtr root;
			    CharPtr file_name;
			    FILEPtr fp = null;
			
			    if (usesavedir && null!=savedir) {
			        root = savedir;
			        file_name = new char[strlen(root)+strlen(path)+1];
			        sprintf( file_name, "%s%s", root, path );
			        //printf("handler:fopen(\"%s\")\n", file_name);
			
			        fp = ONScripter.fopen( file_name, mode );
			    } else if (save) {
			        root = save_path;
			        file_name = new char[strlen(root)+strlen(path)+1];
			        sprintf( file_name, "%s%s", root, path );
			        //printf("handler:fopen(\"%s\")\n", file_name);
			
			        fp = ONScripter.fopen( file_name, mode );
			    } else {
			        // search within archive_path(s)
			        file_name = new char[archive_path.max_path_len()+strlen(path)+1];
			        for (int n=0; n<archive_path.get_num_paths(); n++) {
			            root = archive_path.get_path(n);
			            //printf("root: %s\n", root);
			            sprintf( file_name, "%s%s", root, path );
			            //printf("handler:fopen(\"%s\")\n", file_name);
			            fp = ONScripter.fopen( file_name, mode );
			            if (fp != null) break;
			        }
			    }
			    file_name = null;//delete[] file_name;
			    return fp;
			}
			
			public FILEPtr fopen( CharPtr root, CharPtr path, CharPtr mode )
			{
				CharPtr file_name;
			    FILEPtr fp = null;
			
			    file_name = new char[strlen(root)+strlen(path)+1];
			    sprintf( file_name, "%s%s", root, path );
			    //printf("handler:fopen(\"%s\")\n", file_name);
			
			    fp = ONScripter.fopen( file_name, mode );
			
			    file_name = null;//delete[] file_name;
			    return fp;
			}
			
			public void setKeyTable( UnsignedCharPtr key_table )
			{
			    int i;
			    if (null!=key_table){
			        key_table_flag = true;
			        for (i=0 ; i<256 ; i++) this.key_table[i] = key_table[i];
			    }
			    else{
			        key_table_flag = false;
			        for (i=0 ; i<256 ; i++) this.key_table[i] = (byte)i;
			    }
			}
			
			public void setSavedir( CharPtr dir )
			{
			    savedir = new char[ strlen(dir) + strlen(save_path) + 2];
			    sprintf( savedir, "%s%s%c", save_path, dir, DELIMITER );
			    mkdir(savedir
			#if false && !WIN32
			          , 0755
			#endif
			         );
			}
			
			// basic parser function
			public CharPtr readToken()
			{
				current_script = new CharPtr(next_script);
				CharPtr buf = new CharPtr(current_script);
			    end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			    num_rgosub_waits = cur_rgosub_wait = 0;
			
			    text_flag = false;
			
			    if (rgosub_flag && is_rgosub_click){
			        string_counter = 0;
			        char ch_ = rgosub_click_newpage ? '\\' : '@';
			        addStringBuffer( ch_ );
			        if (rgosub_1byte_mode)
			            addStringBuffer( '`' );
			        rgosub_wait_1byte[num_rgosub_waits] = rgosub_1byte_mode;
			        rgosub_wait_pos[num_rgosub_waits++] = buf;
			        text_flag = true;
			        if (!rgosub_1byte_mode){
			            SKIP_SPACE( buf );
			        }
			    } else {
			        SKIP_SPACE( buf );
			    }
			
			    markAsKidoku( buf );
			
			  readTokenTop:
			    if (!is_rgosub_click)
			        string_counter = 0;
			    char ch = buf[0];
			    if ((ch == ';') && !is_rgosub_click){ // comment
			        while ( ch != 0x0a && ch != '\0' ){
			            addStringBuffer( ch );
			            buf.inc(); ch = buf[0];
			        }
			    }
			    else if (is_rgosub_click || 0!=(ch & 0x80) ||
			             (ch >= '0' && ch <= '9') ||
			             ch == '@' || ch == '\\' || ch == '/' ||
			             ch == '%' || ch == '?' || ch == '$' ||
			             ch == '[' || ch == '(' || ch == '`' ||
			             ch == '!' || ch == '#' || ch == ',' ||
			             ch == '{' || ch == '<' || ch == '>' ||
			             ch == '"'){ // text
			
			        bool ignore_click_flag = false;
			        bool clickstr_flag = false;
			        bool in_pretext_tag = (ch == '[');
			        bool is_nscr_english = (english_mode && (ch == '>'));
			        if (is_nscr_english) {
			        	buf.inc(); ch = buf[0];
			        }
			        bool in_1byte_mode = is_nscr_english;
			        if (is_rgosub_click)
			            in_1byte_mode = rgosub_1byte_mode;
			        while (true){
			            if (rgosub_flag && (num_rgosub_waits == total_rgosub_wait_size)){
			                //double the rgosub wait buffer size
			                CharPtr[] tmp_ = rgosub_wait_pos;
			                bool[] tmp2 = rgosub_wait_1byte;
			                total_rgosub_wait_size *= 2;
			                rgosub_wait_pos = new CharPtr[total_rgosub_wait_size];
			                rgosub_wait_1byte = new bool[total_rgosub_wait_size];
			                for (int i=0; i<num_rgosub_waits; i++){
			                	rgosub_wait_pos[i] = new CharPtr(tmp_[i]);
			                    rgosub_wait_1byte[i] = tmp2[i];
			                }
			                tmp_ = null;//delete[] tmp;
			                tmp2 = null;//delete[] tmp2;
			            }
			        	CharPtr tmp = new CharPtr(buf);
			            SKIP_SPACE(tmp);
			            if (!(is_nscr_english || in_1byte_mode) ||
			                (tmp[0] == 0x0a) || (tmp[0] == 0x00)) {
			                // always skip trailing spaces
			                buf = new CharPtr(tmp);
			                ch = buf[0];
			            }
			            if ((ch == 0x0a) || (ch == 0x00)) break;
			            if ( IS_TWO_BYTE(ch) ){
			                if (!in_pretext_tag && !ignore_click_flag &&
			                    (checkClickstr(buf) > 0))
			                    clickstr_flag = true;
			                else
			                    clickstr_flag = false;
			                addStringBuffer( ch );
			                buf.inc(); ch = buf[0];
			                if (ch == 0x0a || ch == '\0') break; //invalid 2-byte char
			                addStringBuffer( ch );
			                buf.inc(); ch = buf[0];
			                //Mion: ons-en processes clickstr chars here in readToken,
			                // not in ONScripterLabel_text - adds a special
			                // sequence '\@' after the clickstr char
			                if (clickstr_flag) {
			                    // insert a clickwait-or-newpage
			                    addStringBuffer('\\');
			                    addStringBuffer('@');
			                    if (textgosub_flag) {
			                    	CharPtr tmp__ = new CharPtr(buf);
			                        SKIP_SPACE(tmp__);
			                        // if "ignore-textgosub-newline" cmd-line option,
			                        // ignore newline after clickwait if textgosub used
			                        // (fixes some older onscripter-en games)
			                        if (ignore_textgosub_newline && (tmp__[0] == 0x0a))
			                        	buf = new CharPtr(tmp__, +1);
			                        break;
			                    }
			                    if (rgosub_flag){
			                        rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
			                        rgosub_wait_pos[num_rgosub_waits++] = new CharPtr(buf,+1);
			                    }
			                }
			                ignore_click_flag = clickstr_flag = false;
			            }
			            else {
			                if (ch == '`'){
			                    addStringBuffer( ch );
			                    buf.inc(); ch = buf[0];
			                    if (!is_nscr_english)
			                        in_1byte_mode = !in_1byte_mode;
			                }
			                else if (in_pretext_tag && (ch == ']')){
			                    addStringBuffer( ch );
			                    buf.inc(); ch = buf[0];
			                    in_pretext_tag = false;
			                    in_1byte_mode = false;
			                }
			                else if (in_1byte_mode) {
			                    if (in_pretext_tag){
			                        addStringBuffer( ch );
			                        buf.inc(); ch = buf[0];
			                        continue;
			                    }
			                    if (ch == '$'){
			            			if (buf[1] == '$') buf.inc(); else{
			                            addStrVariable(ref buf);
			                            while (true) {buf.dec(); if (buf[0] == ' ') {;}else {break;}}
			                            buf.inc(); ch = buf[0];
			                            ignore_click_flag = false;
			                            continue;
			                        }
			                    }
			            		if ((ch == '_') && (checkClickstr(new CharPtr(buf,+1)) > 0)) {
			                        ignore_click_flag = true;
			                        buf.inc();ch = buf[0];
			                        continue;
			                    }
			                    if ((ch == '@') || (ch == '\\')) {
			                        if (!ignore_click_flag){
			                            addStringBuffer( ch );
			                            if (rgosub_flag){
			                                rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
			                                rgosub_wait_pos[num_rgosub_waits++] = new CharPtr(buf,+1);
			                            }
			                        }
			                        if (textgosub_flag){
			            				buf.inc();
			                            // if "ignore-textgosub-newline", ignore
			                            // newline after clickwait if textgosub
			                            // (fixes older onscripter-en games)
			                            CharPtr tmp_2 = new CharPtr(buf);
			                            SKIP_SPACE(tmp_2);
			                            if (ignore_textgosub_newline &&(tmp_2[0] == 0x0a))
			                            	buf = new CharPtr(tmp_2,+1);
			                            break;
			                        }
			            			buf.inc(); ch = buf[0];
			                        continue;
			                    }
			                    if (!ignore_click_flag && (checkClickstr(buf) > 0))
			                        clickstr_flag = true;
			                    else
			                        clickstr_flag = false;
			                    // no ruby in 1byte mode; escape parens
			                    if (ch == '(') {
			                    	addStringBuffer( (char)LEFT_PAREN );
			                    } else if (ch == ')') {
			                        addStringBuffer( (char)RIGHT_PAREN );
			                    } else if (ch == 0x0a || ch == '\0') break;
			                    else
			                        addStringBuffer( ch );
			                    buf.inc(); ch = buf[0];
			                    //Mion: ons-en processes clickstr chars here in readToken,
			                    // not in ONScripterLabel_text - adds a special
			                    // sequence '\@' after the clickstr char
			                    if (clickstr_flag) {
			                        // insert a clickwait-or-newpage
			                        addStringBuffer('\\');
			                        addStringBuffer('@');
			                        if (textgosub_flag) break;
			                        if (rgosub_flag){
			                            rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
			                            rgosub_wait_pos[num_rgosub_waits++] = buf;
			                        }
			                    }
			                    ignore_click_flag = clickstr_flag = false;
			                }
			                else{ //!in_1byte_mode
			                    if (in_pretext_tag){
			                        addStringBuffer( ch );
			                        buf.inc(); ch = buf[0];
			                        continue;
			                    }
			                    else if ((ch == '@') || (ch == '\\')) {
			                        if (!ignore_click_flag){
			                            addStringBuffer( ch );
			                            if (rgosub_flag){
			                                rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
			                                rgosub_wait_pos[num_rgosub_waits++] = new CharPtr(buf,+1);
			                            }
			                        }
			                        if (textgosub_flag){
			            				buf.inc();
			                            // if "ignore-textgosub-newline", ignore
			                            // newline after clickwait if textgosub
			                            // (fixes older onscripter-en games)
			                            CharPtr tmp_2 = new CharPtr(buf);
			                            SKIP_SPACE(tmp_2);
			                            if (ignore_textgosub_newline &&(tmp_2[0] == 0x0a))
			                            	buf = new CharPtr(tmp_2,+1);
			                            break;
			                        }
			            			buf.inc(); ch = buf[0];
			                        continue;
			                    }
			                    if (ch == '%' || ch == '?'){
			                        addIntVariable(ref buf);
			                    }
			                    else if (ch == '$'){
			                        addStrVariable(ref buf);
			                    }
			                    else if (ch == '<'){
			            			addStringBuffer((char)TXTBTN_START);
			            			buf.inc(); ch = buf[0];
			                    }
			                    else if (ch == '>'){
			                        addStringBuffer((char)TXTBTN_END);
			                        buf.inc(); ch = buf[0];
			                    }
			                    else if (ch == '{') {
			                        // comma list of var/val pairs
			                        buf.inc();
			                        pushCurrent(buf);
			                        next_script = new CharPtr(buf);
			                        TmpVariableDataLink tmp_3 = tmp_variable_data_link;
			                        while (tmp_3.next != null)
			                            tmp_3 = tmp_3.next;
			                        while( buf[0] != '}' ) {
			                        
			                            readVariable();
			                            pushVariable();
			                            tmp_3.next = new TmpVariableDataLink();
			                            tmp_3 = tmp_3.next;
			                            tmp_3.vi.var_no = pushed_variable.var_no;
			                            tmp_3.vi.type = pushed_variable.type;
			                            VariableData vd = getVariableData(tmp_3.vi.var_no);
			                            tmp_3.num = vd.num;
			                            //printf("variable: $%d\n", pushed_variable.var_no);
			                            buf = next_script;
			
			                            if ( 0!=(tmp_3.vi.type & VAR_INT) ) {
			                                tmp_3.num = parseIntExpression(ref buf);
			                                //printf("int: %d\n", x);
			                            } else if ( 0!=(tmp_3.vi.type & VAR_STR) ) {
			                                bool invar_1byte_mode = false;
			                                int tmp_count = 0;
			                                strcpy(saved_string_buffer, "");
			                                while (buf[0] != 0x0a && buf[0] != '\0' &&
			                                       (invar_1byte_mode || ((buf[0] != ',') && (buf[0] != '}')))) {
			                                	if (buf[0] == '`')
			                                        invar_1byte_mode = !invar_1byte_mode;
			                                    if ((tmp_count+1 >= STRING_BUFFER_LENGTH) ||
			                                        (IS_TWO_BYTE(buf[0]) && (tmp_count+2 >= STRING_BUFFER_LENGTH)))
			                                        errorAndExit("readToken: var string length exceeds 2048 bytes.");
			                                    else if (IS_TWO_BYTE(buf[0])) {
			                                    	saved_string_buffer[tmp_count++] = buf[0]; buf.inc();
			                                    	saved_string_buffer[tmp_count++] = buf[0]; buf.inc();
			                                    } else if ((buf[0] == '\\') || (buf[0] == BACKSLASH)) {
			                                        //Mion: I really shouldn't be modifying
			                                        //  the script buffer FIXME
			                                        saved_string_buffer[tmp_count++] = '\\';
			                                        buf[0] = (char)BACKSLASH; buf.inc();
			                                    } else {
			                                    	saved_string_buffer[tmp_count++] = buf[0]; buf.inc();
			                                    }
			                                    saved_string_buffer[tmp_count] = '\0';
			                                }
			                                setStr( ref tmp_3.str, saved_string_buffer );
			                                //printf("string: %s\n", saved_string_buffer);
			                            }
			                            next_script = checkComma(buf);
			                            buf = new CharPtr(next_script);
			                            if (0==(getEndStatus() & END_COMMA)) break;
			                        }
			                        end_status = END_NONE;
			                        current_variable.type = VAR_NONE;
			                        popCurrent();
			                        if (buf[0] == '}')
			                        	buf.inc();
			                    }
			                    else{
			                        if (ch == '_')
			                            ignore_click_flag = true;
			                        else
			                            ignore_click_flag = false;
			                        addStringBuffer( ch );
			                        buf.inc();
			                        if (buf[0] == ' ') {
			                        	addStringBuffer( buf[0] );
			                            buf.inc();
			                        }
			                    }
			            		ch = buf[0];
			                    if (ch == 0x0a || ch == '\0') break;
			                }
			            }
			        }
			        //now process any {} tmp variables
			        TmpVariableDataLink tmp_4 = tmp_variable_data_link.next;
			        while (tmp_4 != null) {
			        	if ( 0!=(tmp_4.vi.type & VAR_INT) )
			                setInt( tmp_4.vi, tmp_4.num);
			            else if ( 0!=(tmp_4.vi.type & VAR_STR) )
			                setStr( ref variable_data[ tmp_4.vi.var_no ].str, tmp_4.str );
			            TmpVariableDataLink last = tmp_4;
			            tmp_4 = tmp_4.next;
			            last = null;//delete last;
			        }
			        tmp_variable_data_link.next = null;
			        text_flag = true;
			    }
			    else if ((ch >= 'a' && ch <= 'z') ||
			             (ch >= 'A' && ch <= 'Z') ||
			             ch == '_'){ // command
			        do{
			    		if (ch >= 'A' && ch <= 'Z') ch = (char)(ch + ('a' - 'A'));
			            addStringBuffer( ch );
			            buf.inc(); ch = buf[0];
			        }
			        while((ch >= 'a' && ch <= 'z') ||
			              (ch >= 'A' && ch <= 'Z') ||
			              (ch >= '0' && ch <= '9') ||
			              ch == '_');
			    }
			    else if (ch == '*'){ // label
			        return readLabel();
			    }
			    else if ((ch == 0x0a) && !is_rgosub_click){
			        addStringBuffer( ch );
			        markAsKidoku( buf ); buf.inc();
			    }
			    else if (ch == '~' || ch == ':'){
			        addStringBuffer( ch );
			        markAsKidoku( buf ); buf.inc();
			    }
			    else if (ch != '\0'){
			        fprintf(stderr, "readToken: skip unknown heading character %c (%x)\n", ch, ch);
			        buf.inc();
			        goto readTokenTop;
			    }
			    is_rgosub_click = false;
			    next_script = checkComma(buf);
			
			    //printf("readToken [%s] len=%d [%c(%x)] %p\n", string_buffer, strlen(string_buffer), ch, ch, next_script);
			
			    return new CharPtr(string_buffer);
			}
			
			public CharPtr readName()
			{
				// bare word - not a string variable
			    end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			
			    current_script = new CharPtr(next_script);
			    SKIP_SPACE( current_script );
			    CharPtr buf = new CharPtr(current_script);
			
			    string_counter = 0;
			    char ch = buf[0];
			    if ( ((ch >= 'a') && (ch <= 'z')) ||
			         ((ch >= 'A') && (ch <= 'Z')) ||
			         (ch == '_') ){
			        if ( (ch >= 'A') && (ch <= 'Z') )
			        	ch = (char)(ch + ('a' - 'A'));
			        addStringBuffer( ch );
			        buf.inc(); ch = buf[0];
			        while( ((ch >= 'a') && (ch <= 'z')) ||
			               ((ch >= 'A') && (ch <= 'Z')) ||
			               ((ch >= '0') && (ch <= '9')) ||
			               (ch == '_') ){
			            if ( (ch >= 'A') && (ch <= 'Z') )
			            	ch = (char)(ch + ('a' - 'A'));
			            addStringBuffer( ch );
			            buf.inc(); ch = buf[0];
			        }
			    }
			    addStringBuffer( '\0' );
			
			    next_script = checkComma(buf);
			
			    return new CharPtr(string_buffer);
			}
			
			//FIXME:added is_not_null_is_color
			public CharPtr readColor(ref bool is_color, bool is_not_null_is_color)
			{
				// bare color type - not a string variable
			    end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			
			    current_script = new CharPtr(next_script);
			    SKIP_SPACE( current_script );
			    CharPtr buf = new CharPtr(current_script);
			
			    string_counter = 0;
			    addStringBuffer( '#' );
			    buf.inc(); char ch = buf[0];
			    int i;
			    for (i=0; i<7; i++) {
			        if ( ((ch >= '0') && (ch <= '9')) ||
			             ((ch >= 'a') && (ch <= 'f')) ||
			             ((ch >= 'A') && (ch <= 'F')) ) {
			            addStringBuffer( ch );
			            buf.inc(); ch = buf[0];
			        } else
			            break;
			    }
			    if (i!=6) {
			        if (is_not_null_is_color) {
			            is_color = false;
			            string_counter = 0;
			            addStringBuffer( '\0' );
			            return new CharPtr(string_buffer);
			        } else {
			            strncpy(string_buffer, current_script, 16);
			            string_buffer[16] = '\0';
			            errorAndExit( "readColor: not a valid color type." );
			        }
			    }
			    addStringBuffer( '\0' );
			    next_script = checkComma(buf);
			    if (is_not_null_is_color)
			        is_color = true;
			
			    return string_buffer;
			}
			
			public CharPtr readLabel()
			{
				// *NAME, "*NAME", or $VAR that equals "*NAME"
			    end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			
			    current_script = new CharPtr(next_script);
			    SKIP_SPACE( current_script );
			    CharPtr buf = new CharPtr(current_script);
			    CharPtr tmp = null;
			
			    string_counter = 0;
			    char ch = buf[0];
			    if ((ch == '$') || (ch == '"') || (ch == '`')){
			        parseStr(ref buf);
			        tmp = new CharPtr(buf);
			        string_counter = 0;
			        buf = new CharPtr(str_string_buffer);
			        SKIP_SPACE(buf);
			        ch = buf[0];
			    }
			    if (ch == '*') {
			        while (ch == '*'){
			            addStringBuffer( ch );
			            buf.inc(); ch = buf[0];
			        }
			        SKIP_SPACE(buf);
			
			        ch = buf[0];
			        while( ((ch >= 'a') && (ch <= 'z')) ||
			               ((ch >= 'A') && (ch <= 'Z')) ||
			               ((ch >= '0') && (ch <= '9')) ||
			               (ch == '_') ){
			            if ( (ch >= 'A') && (ch <= 'Z') )
			            	ch = (char)(ch + ('a' - 'A'));
			            addStringBuffer( ch );
			            buf.inc(); ch = buf[0];
			        }
			    }
			    addStringBuffer( '\0' );
			    if ( (string_buffer[0] == '\0') || (string_buffer[1] == '\0') ){
			        buf = current_script;
			        while (0!=buf[0] && (buf[0] != 0x0a))
			        	buf.inc();
			        ch = buf[0];
			        buf[0] = '\0';
			        if (tmp != null) {
			            snprintf(errbuf, MAX_ERRBUF_LEN, 
			                     "Invalid label specification '%s' ('%s')",
			                     current_script, str_string_buffer);
			        	buf[0] = ch;
			            errorAndExit(errbuf);
			        } else {
			            snprintf(errbuf, MAX_ERRBUF_LEN,
			                     "Invalid label specification '%s'", current_script);
			        	buf[0] = ch;
			            errorAndExit(errbuf);
			        }
			    }
			    if (tmp != null)
			    	buf = new CharPtr(tmp);
			
			    next_script = checkComma(buf);
			
			    return new CharPtr(string_buffer);
			}
			
			public CharPtr readStr()
			{
				end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			
			    current_script = new CharPtr(next_script);
			    SKIP_SPACE( current_script );
			    CharPtr buf = new CharPtr(current_script);
			
			    string_buffer[0] = '\0';
			    string_counter = 0;
			
			    while(true){
			        parseStr(ref buf);
			        buf = checkComma(buf);
			        string_counter = (int)(string_counter + strlen(str_string_buffer));
			        if (string_counter+1 >= STRING_BUFFER_LENGTH)
			            errorAndExit("readStr: string length exceeds 2048 bytes.");
			        strcat(string_buffer, str_string_buffer);
			        if (buf[0] != '+') break;
			        buf.inc();
			    }
			    next_script = new CharPtr(buf);
			
			    return new CharPtr(string_buffer);
			}
			
			public int readInt()
			{
			    string_counter = 0;
			    string_buffer[string_counter] = '\0';
			
			    end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			
			    current_script = new CharPtr(next_script);
			    SKIP_SPACE( current_script );
			    CharPtr buf = new CharPtr(current_script);
			
			    int ret = parseIntExpression(ref buf);
			
			    next_script = checkComma(buf);
			
			    return ret;
			}
			
			public void skipToken()
			{
			    SKIP_SPACE( current_script );
			    CharPtr buf = new CharPtr(current_script);
			
			    bool quat_flag = false;
			    bool text_flag = false;
			    while(true){
			    	if ( buf[0] == 0x0a ||
			    	    (!quat_flag && !text_flag && (buf[0] == ':' || buf[0] == ';') ) ) break;
			    	if ( buf[0] == '"' ) quat_flag = !quat_flag;
			    	if ( IS_TWO_BYTE(buf[0]) ){
			    		buf.inc(2);
			            if ( !quat_flag ) text_flag = true;
			        }
			        else
			        	buf.inc();
			    }
			    if (text_flag && buf[0] == 0x0a) buf.inc();
			    
			    next_script = new CharPtr(buf);
			}
			
			// string access function
			public CharPtr saveStringBuffer()
			{
				strcpy( saved_string_buffer, string_buffer );
				return new CharPtr(saved_string_buffer);
			}
			
			// script address direct manipulation function
			public void setCurrent(CharPtr pos)
			{
				current_script = new CharPtr(pos); next_script = new CharPtr(pos);
			}
			
			public void pushCurrent( CharPtr pos )
			{
				pushed_current_script = new CharPtr(current_script);
				pushed_next_script = new CharPtr(next_script);
			
				current_script = new CharPtr(pos);
				next_script = new CharPtr(pos);
			}
			
			public void popCurrent()
			{
				current_script = new CharPtr(pushed_current_script);
				next_script = new CharPtr(pushed_next_script);
			}
			
			public void enterExternalScript(CharPtr pos)
			{
				internal_current_script = new CharPtr(current_script);
				current_script = new CharPtr(pos);
				internal_next_script = new CharPtr(next_script);
				next_script = new CharPtr(pos);
			    internal_end_status = end_status;
			    internal_current_variable = current_variable;
			    internal_pushed_variable = pushed_variable;
			}
			
			public void leaveExternalScript()
			{
				current_script = new CharPtr(internal_current_script);
			    internal_current_script = null;
			    next_script = new CharPtr(internal_next_script);
			    end_status = internal_end_status;
			    current_variable = internal_current_variable;
			    pushed_variable = internal_pushed_variable;
			}
			
			public bool isExternalScript()
			{
				return (internal_current_script != null);
			}
			
			public int getOffset( CharPtr pos )
			{
				return CharPtr.minus(pos, script_buffer);
			}
			
			public CharPtr getAddress( int offset )
			{
				return new CharPtr(script_buffer, + offset);
			}
			
			public int getLineByAddress( CharPtr address )
			{
				LabelInfo label = getLabelByAddress( address );
			
			    CharPtr addr = label.label_header;
			    int line = 0;
			    while ( CharPtr.isLargerThen(address, addr) ){
			    	if ( addr[0] == 0x0a ) line++;
			    	addr.inc();
			    }
			    return line;
			}
			
			public CharPtr getAddressByLine( int line )
			{
				LabelInfo label = getLabelByLine( line );
			
			    int l = line - label.start_line;
			    CharPtr addr = new CharPtr(label.label_header);
			    while ( l > 0 ){
			    	while( addr[0] != 0x0a ) addr.inc();
			        addr.inc();
			        l--;
			    }
			    return addr;
			}
			
			public LabelInfo getLabelByAddress( CharPtr address )
			{
				int i;
			    for ( i=0 ; i<num_of_labels-1 ; i++ ){
					if ( CharPtr.isLargerThen(label_info[i+1].start_address, address) )
			            return label_info[i];
			    }
			    return label_info[i];
			}
			
			public LabelInfo getLabelByLine( int line )
			{
				int i;
			    for ( i=0 ; i<num_of_labels-1 ; i++ ){
			        if ( label_info[i+1].start_line > line )
			            return label_info[i];
			    }
			    return label_info[i];
			}
			
			public bool isName( CharPtr name )
			{
				return (strncmp( name, string_buffer, (int)strlen(name) )==0)?true:false;
			}
			
			public bool isText()
			{
				return text_flag;
			}
			
			public bool compareString(CharPtr buf)
			{
				SKIP_SPACE(next_script);
			    uint i, num = strlen(buf);
			    for (i=0 ; i<num ; i++){
			        char ch = next_script[i];
			        if ('A' <= ch && 'Z' >= ch) ch = (char)(ch + ('a' - 'A'));
			        if (ch != buf[i]) break;
			    }
			    return (i==num)?true:false;
			}
			
			public void skipLine( int no=1 )
			{
			    for ( int i=0 ; i<no ; i++ ){
					while ( current_script[0] != 0x0a ) current_script.inc();
					current_script.inc();
			    }
			    next_script = current_script;
			}
			
			public void setLinepage( bool val )
			{
			    linepage_flag = val;
			}
			
			// function for kidoku history
			public bool isKidoku()
			{
				return skip_enabled;
			}
			
			public void markAsKidoku( CharPtr address )
			{
			    if (!kidokuskip_flag || internal_current_script != null) return;
			
			    int offset = CharPtr.minus(current_script, script_buffer);
			    if ( null!=address ) offset = CharPtr.minus(address, script_buffer);
			    //printf("mark (%c)%x:%x = %d\n", *current_script, offset /8, offset%8, kidoku_buffer[ offset/8 ] & ((char)1 << (offset % 8)));
			    if ( 0!=(kidoku_buffer[ offset/8 ] & ((sbyte)1 << (offset % 8))) )
			        skip_enabled = true;
			    else
			        skip_enabled = false;
			    kidoku_buffer[ offset/8 ] = (char)(kidoku_buffer[ offset/8 ] | ((sbyte)1 << (offset % 8)));
			}
			
			public void setKidokuskip( bool kidokuskip_flag )
			{
			    this.kidokuskip_flag = kidokuskip_flag;
			}
			
			public void saveKidokuData()
			{
			    FILEPtr fp;
			
			    if ( ( fp = fopen( "kidoku.dat", "wb", true, true ) ) == null ){
			        fprintf( stderr, "can't write kidoku.dat\n" );
			        return;
			    }
			
			    if ( fwrite( kidoku_buffer, 1, (uint)(script_buffer_length/8), fp ) !=
			        (uint)(script_buffer_length/8) )
			        fprintf( stderr, "Warning: failed to write to kidoku.dat\n" );
			    fclose( fp );
			}
			
			public void loadKidokuData()
			{
			    FILEPtr fp;
			
			    setKidokuskip( true );
			    kidoku_buffer = new char[ script_buffer_length/8 + 1 ];
			    memset( kidoku_buffer, 0, (uint)(script_buffer_length/8 + 1) );
			
			    if ( ( fp = fopen( "kidoku.dat", "rb", true, true ) ) != null ){
			    	if (fread( kidoku_buffer, 1, (uint)(script_buffer_length/8), fp ) !=
			    	    (uint)(script_buffer_length/8)) {
			            if (0!=ferror(fp))
			                fputs("Warning: failed to read kidoku.dat\n", stderr);
			        }
			        fclose( fp );
			    }
			}
			
			public void addIntVariable(ref CharPtr buf, bool no_zenkaku=false)
			{
				char[] num_buf = new char[20];
			    int no = parseInt(ref buf);
			
			    int len = getStringFromInteger( num_buf, no, -1, false, !no_zenkaku );
			    for (int i=0 ; i<len ; i++)
			        addStringBuffer( num_buf[i] );
			}
			
			public void addStrVariable(ref CharPtr buf)
			{
				buf.inc();
			    int no = parseInt(ref buf);
			    VariableData vd = getVariableData(no);
			    if ( null!=vd.str ){
			        for (uint i=0 ; i<strlen( vd.str ) ; i++){
			            addStringBuffer( vd.str[i] );
			        }
			    }
			}
			
			public void enableTextgosub(bool val)
			{
			    textgosub_flag = val;
			}
			
			public void enableRgosub(bool val)
			{
			    rgosub_flag = val;
			
			    if (rgosub_flag && null==rgosub_wait_pos){
			        total_rgosub_wait_size = 4;
			        rgosub_wait_pos = new CharPtr[total_rgosub_wait_size];
			        rgosub_wait_1byte = new bool[total_rgosub_wait_size];
			    }
			}
			
			public void setClickstr(CharPtr list)
			{
			    if (null!=clickstr_list) clickstr_list=null;//delete[] clickstr_list;
			    clickstr_list = new char[strlen(list)+2];
			    memcpy( clickstr_list, list, strlen(list)+1 );
			    clickstr_list[strlen(list)+1] = '\0';
			}
			
			public int checkClickstr(CharPtr buf, bool recursive_flag=false)
			{
				if ((buf[0] == '\\') && (buf[1] == '@')) return -2;  //clickwait-or-page
			    if ((buf[0] == '@') || (buf[0] == '\\')) return -1;
			
			    if (clickstr_list == null) return 0;
			    bool only_double_byte_check = true;
			    CharPtr click_buf = new CharPtr(clickstr_list);
			    while(0!=click_buf[0]){
			        if (click_buf[0] == '`'){
			    		click_buf.inc();
			            only_double_byte_check = false;
			            continue;
			        }
			        if (! only_double_byte_check){
			            if (!IS_TWO_BYTE(click_buf[0]) && !IS_TWO_BYTE(buf[0]) 
			                && (click_buf[0] == buf[0])){
			    			if (!recursive_flag && checkClickstr(new CharPtr(buf,+1), true) != 0) return 0;
			                return 1;
			            }
			        }
			        if (IS_TWO_BYTE(click_buf[0]) && IS_TWO_BYTE(buf[0]) &&
			            (click_buf[0] == buf[0]) && (click_buf[1] == buf[1])){
			    		if (!recursive_flag && checkClickstr(new CharPtr(buf,+2), true) != 0) return 0;
			            return 2;
			        }
			    	if (IS_TWO_BYTE(click_buf[0])) click_buf.inc();
			        click_buf.inc();
			    }
			
			    return 0;
			}
			
			public int getIntVariable( VariableInfo var_info )
			{
				if ( var_info == null ) var_info = current_variable;
			
			    if ( var_info.type == VAR_INT )
			        return getVariableData(var_info.var_no).num;
			    else if ( var_info.type == VAR_ARRAY )
			    	return getArrayPtr( var_info.var_no, var_info.array, 0 )[0];
			    return 0;
			}
			
			public void readVariable( bool reread_flag = false)
			{
			    end_status = END_NONE;
			    current_variable.type = VAR_NONE;
			    if ( reread_flag ) next_script = new CharPtr(current_script);
			    current_script = next_script;
			    CharPtr buf = new CharPtr(current_script);
			
			    SKIP_SPACE(buf);
			
			    bool ptr_flag = false;
			    if ( buf[0] == 'i' || buf[0] == 's' ){
			        ptr_flag = true;
			        buf.inc();
			    }
			
			    if ( buf[0] == '%' ){
			    	buf.inc();
			        current_variable.var_no = parseInt(ref buf);
			        current_variable.type = VAR_INT;
			    }
			    else if ( buf[0] == '?' ){
			    	ArrayVariable av = new ArrayVariable();
			        current_variable.var_no = parseArray( ref buf, av );
			        current_variable.type = VAR_ARRAY;
			        current_variable.array = av;
			    }
			    else if ( buf[0] == '$' ){
			    	buf.inc();
			        current_variable.var_no = parseInt(ref buf);
			        current_variable.type = VAR_STR;
			    }
			
			    if (ptr_flag) current_variable.type |= VAR_PTR;
			
			    next_script = checkComma(buf);
			}
			
			public void setInt( VariableInfo var_info, int val, int offset = 0 )
			{
				if ( 0!=(var_info.type & VAR_INT) ){
			        setNumVariable( var_info.var_no + offset, val );
			    }
				else if ( 0!=(var_info.type & VAR_ARRAY) ){
					getArrayPtr( var_info.var_no, var_info.array, offset )[0] = val;
			    }
			    else{
			        errorAndExit( "setInt: no integer variable." );
			    }
			}
			
			public void setStr( ref CharPtr dst, CharPtr src, int num=-1 )
			{
			    //if ( *dst ) delete[] *dst;
			    dst = null;
			    
			    if ( null!=src ){
			        if (num >= 0){
			            dst = new char[ num + 1 ];
			            memcpy( dst, src, (uint)num );
			            dst[num] = '\0';
			        }
			        else{
			            dst = new char[ strlen( src ) + 1];
			            strcpy( dst, src );
			        }
			    }
			}
			
			public void pushVariable()
			{
			    pushed_variable = current_variable;
			}
			
			public void setNumVariable( int no, int val )
			{
			    VariableData vd = getVariableData(no);
			    if ( vd.num_limit_flag ){
			        if ( val < vd.num_limit_lower )
			            val = vd.num_limit_lower;
			        else if ( val > vd.num_limit_upper )
			            val = vd.num_limit_upper;
			    }
			    vd.num = val;
			}
			
			public int getStringFromInteger( CharPtr buffer, int no, int num_column,
			                                         bool is_zero_inserted,
			                                         bool use_zenkaku )
			{
				int i, num_space=0, num_minus = 0;
			    if (no < 0){
			        num_minus = 1;
			        no = -no;
			    }
			    int num_digit=1, no2 = no;
			    while(no2 >= 10){
			        no2 /= 10;
			        num_digit++;
			    }
			
			    if (num_column < 0) num_column = num_digit+num_minus;
			    if (num_digit+num_minus <= num_column)
			        num_space = num_column - (num_digit+num_minus);
			    else{
			        for (i=0 ; i<num_digit+num_minus-num_column ; i++)
			            no /= 10;
			        num_digit -= num_digit+num_minus-num_column;
			    }
			
			    if (!use_zenkaku) {
			        if (num_minus == 1) no = -no;
			        char[] format = new char[6];
			        if (is_zero_inserted)
			            sprintf(format, "%%0%dd", num_column);
			        else
			            sprintf(format, "%%%dd", num_column);
			        
			        sprintf(buffer, format, no);
			        return num_column;
			    }
			
			    int c = 0;
			    if (is_zero_inserted){
			        for (i=0 ; i<num_space ; i++){
			    		buffer[c++] = (CharPtr.fromDoubleByte("侽"))[0];
			    		buffer[c++] = (CharPtr.fromDoubleByte("侽"))[1];
			        }
			    }
			    else{
			        for (i=0 ; i<num_space ; i++){
			    		buffer[c++] = (CharPtr.fromDoubleByte("丂"))[0];
			    		buffer[c++] = (CharPtr.fromDoubleByte("丂"))[1];
			        }
			    }
			    if (num_minus == 1){
			        buffer[c++] = "亅"[0];
			        buffer[c++] = "亅"[1];
			    }
			    c = (num_column-1)*2;
			    CharPtr num_str = CharPtr.fromDoubleByte("侽侾俀俁係俆俇俈俉俋");
			    for (i=0 ; i<num_digit ; i++){
			        buffer[c]   = num_str[ no % 10 * 2];
			        buffer[c+1] = num_str[ no % 10 * 2 + 1];
			        no /= 10;
			        c -= 2;
			    }
			    buffer[num_column*2] = '\0';
			
			    return num_column*2;
			}
			
			public int readScriptSub( FILEPtr fp, ref CharPtr buf, int encrypt_mode )
			{
				byte[] magic = new byte[5] {0x79, 0x57, 0x0d, 0x80, 0x04 };
			    int  magic_counter = 0;
			    bool newline_flag = true;
			    bool cr_flag = false;
			
			    if (encrypt_mode == 3 && !key_table_flag)
			        simpleErrorAndExit("readScriptSub: the EXE file must be specified with --key-exe option.");
			
			    uint len=0, count=0;
			    while(true){
			        if (len == count){
			    		len = fread(new UnsignedCharPtr(tmp_script_buf), 1, TMP_SCRIPT_BUF_LEN, fp);
			            if (len == 0){
			            	if (cr_flag) { buf[0] = (char)0x0a; buf.inc(); }
			                break;
			            }
			            count = 0;
			        }
			    	byte ch = (byte)(tmp_script_buf[count++]);
			        if      ( encrypt_mode == 1 ) ch ^= 0x84;
			        else if ( encrypt_mode == 2 ){
			        	ch = (byte)((ch ^ magic[magic_counter++]) & 0xff);
			            if ( magic_counter == 5 ) magic_counter = 0;
			        }
			        else if ( encrypt_mode == 3){
			        	ch = (byte)(key_table[(byte)ch] ^ 0x84);
			        }
			
			        if ( cr_flag && ch != 0x0a ){
			        	buf[0] = (char)0x0a; buf.inc();
			            newline_flag = true;
			            cr_flag = false;
			        }
			
			        if ( ch == '*' && newline_flag ) num_of_labels++;
			        if ( ch == 0x0d ){
			            cr_flag = true;
			            continue;
			        }
			        if ( ch == 0x0a ){
			        	buf[0] = (char)0x0a; buf.inc();
			            newline_flag = true;
			            cr_flag = false;
			        }
			        else{
			        	buf[0] = (char)ch; buf.inc();
			            if ( ch != ' ' && ch != '\t' )
			                newline_flag = false;
			        }
			    }
			
			    buf[0] = (char)0x0a; buf.inc();
			    return 0;
			}
			
			public int readScript( DirPaths path )
			{
			    archive_path = path;
			
			    FILEPtr fp = null;
			    CharPtr filename = new char[10];
			    int i, n=0, encrypt_mode = 0;
			    while ((fp == null) && (n<archive_path.get_num_paths())) {
			        CharPtr curpath = archive_path.get_path(n);
			        CharPtr filename_ = "";
			        
			        if ((fp = fopen(curpath, "0.txt", "rb")) != null){
			            encrypt_mode = 0;
			            filename_ = "0.txt";
			        }
			        else if ((fp = fopen(curpath, "00.txt", "rb")) != null){
			            encrypt_mode = 0;
			            filename_ = "00.txt";
			        }
			        else if ((fp = fopen(curpath, "nscr_sec.dat", "rb")) != null){
			            encrypt_mode = 2;
			            filename_ = "nscr_sec.dat";
			        }
			        else if ((fp = fopen(curpath, "nscript.___", "rb")) != null){
			            encrypt_mode = 3;
			            filename_ = "nscript.___";
			        }
			        else if ((fp = fopen(curpath, "nscript.dat", "rb")) != null){
			            encrypt_mode = 1;
			            filename_ = "nscript.dat";
			        }
			
			        if (fp != null) {
			            fprintf(stderr, "Script found: %s%s\n", curpath, filename_);
			            setStr(ref script_path, curpath);
			        }
			        n++;
			    }
			    if (fp == null){
			#if MACOSX 
			        simpleErrorAndExit("No game data found.\nThis application must be run "
			                           "from a directory containing ONScripter game data.",
			                           "can't open any of 0.txt, 00.txt, or nscript.dat",
			                           "Missing game data");
			#else
			        simpleErrorAndExit("No game script found.",
			                           "can't open any of 0.txt, 00.txt, or nscript.dat",
			                           "Missing game data");
			#endif
			        return -1;
			    }
			
			    fseek( fp, 0, SEEK_END );
			    int estimated_buffer_length = (int)(ftell( fp ) + 1);
			
			    if (encrypt_mode == 0){
			        fclose(fp);
			        for (i=1 ; i<100 ; i++){
			            sprintf(filename, "%d.txt", i);
			            if ((fp = fopen(script_path, filename, "rb")) == null){
			                sprintf(filename, "%02d.txt", i);
			                fp = fopen(script_path, filename, "rb");
			            }
			            if (null!=fp){
			                fseek( fp, 0, SEEK_END );
			                estimated_buffer_length = (int)(estimated_buffer_length + (ftell(fp)+1));
			                fclose(fp);
			            }
			        }
			    }
			
			    if ( null!=script_buffer ) script_buffer = null;//delete[] script_buffer;
			    script_buffer = new char[ estimated_buffer_length ];
			
			    CharPtr p_script_buffer;
			    current_script = p_script_buffer = script_buffer;
			
			    tmp_script_buf = new char[TMP_SCRIPT_BUF_LEN];
			    if (encrypt_mode > 0){
			        fseek( fp, 0, SEEK_SET );
			        readScriptSub( fp, ref p_script_buffer, encrypt_mode );
			        fclose( fp );
			    }
			    else{
			        for (i=0 ; i<100 ; i++){
			            sprintf(filename, "%d.txt", i);
			            if ((fp = fopen(script_path, filename, "rb")) == null){
			                sprintf(filename, "%02d.txt", i);
			                fp = fopen(script_path, filename, "rb");
			            }
			            if (null!=fp){
			                readScriptSub( fp, ref p_script_buffer, 0 );
			                fclose(fp);
			            }
			        }
			    }
			    tmp_script_buf = null;//delete[] tmp_script_buf;
			
			    // Haeleth: Search for gameid file (this overrides any builtin
			    // ;gameid directive, or serves its purpose if none is available)
			    if (null==game_identifier) { //Mion: only if gameid not already set
			        fp = fopen(script_path, "game.id", "rb"); //Mion: search only the script path
			        if (null!=fp) {
			            uint line_size = 0;
			            sbyte c;
			            do {
			            	c = (sbyte)fgetc(fp);
			                ++line_size;
			            } while (c != '\r' && c != '\n' && c != EOF);
			            fseek(fp, 0, SEEK_SET);
			            game_identifier = new char[line_size];
			            if (fgets(game_identifier, (int)line_size, fp) == null)
			                fputs("Warning: couldn't read game ID from game.id\n", stderr);
			            fclose(fp);
			        }
			    }
			
			    script_buffer_length = CharPtr.minus(p_script_buffer, script_buffer);
			    game_hash = script_buffer_length;  // Reasonable "hash" value
			
			    /* ---------------------------------------- */
			    /* screen size and value check */
			    CharPtr buf = new CharPtr(script_buffer, +1);
			    while( script_buffer[0] == ';' ){
			        if ( 0==strncmp( buf, "mode", 4 ) ){
			    		buf.inc(4);
			            if      ( 0==strncmp( buf, "800", 3 ) )
			                screen_size = SCREEN_SIZE_800x600;
			            else if ( 0==strncmp( buf, "400", 3 ) )
			                screen_size = SCREEN_SIZE_400x300;
			            else if ( 0==strncmp( buf, "320", 3 ) )
			                screen_size = SCREEN_SIZE_320x240;
			            else
			                screen_size = SCREEN_SIZE_640x480;
			            buf.inc(3);
			        }
			        else if ( 0==strncmp( buf, "value", 5 ) ){
			    		buf.inc(5);
			            SKIP_SPACE(buf);
			            global_variable_border = 0;
			            while ( buf[0] >= '0' && buf[0] <= '9' ) {
			            	global_variable_border = global_variable_border * 10 + (int)(buf[0] - '0'); buf.inc();
			            }
			            //printf("set global_variable_border: %d\n", global_variable_border);
			        }
			        else{
			            break;
			        }
			        if ( buf[0] != ',' ){
			    		while ( true ) {char temp = buf[0]; buf.inc(); if (temp != '\n'){;}else{break;} }
			        	break;
			        }
			    	buf.inc();
			    }
			    char temp2 = buf[0]; buf.inc();
			    if ( temp2 == ';' && null==game_identifier ){
			    	while (buf[0] == ' ' || buf[0] == '\t') buf.inc();
			    	if ( 0==strncmp( buf, "gameid ", 7 ) ){
			    		buf.inc(7);
			    		int i_ = 0;
			    		while ( buf[i_++] != '\n' );
			    		game_identifier = new char[i_];
			    		strncpy( game_identifier, buf, (uint)(i_ - 1) );
			    		game_identifier[i_ - 1] = (char)0;
			    	}
			    }
			
			    return labelScript();
			}
			
			public int labelScript()
			{
				int label_counter = -1;
			    int current_line = 0;
			    CharPtr buf = new CharPtr(script_buffer);
			    label_info = new LabelInfo[ num_of_labels+1 ];
			
			    while ( CharPtr.isLessThen(buf, new CharPtr(script_buffer, + script_buffer_length)) ){
			        SKIP_SPACE( buf );
			        if ( buf[0] == '*' ){
			            setCurrent( buf );
			            readLabel();
			            label_info[ ++label_counter ].name = new char[ strlen(string_buffer) ];
			            strcpy( label_info[ label_counter ].name, new CharPtr(string_buffer, +1) );
			            label_info[ label_counter ].label_header = buf;
			            label_info[ label_counter ].num_of_lines = 1;
			            label_info[ label_counter ].start_line   = current_line;
			            buf = getNext();
			            if ( buf[0] == 0x0a ){
			            	buf.inc();
			                SKIP_SPACE(buf);
			                current_line++;
			            }
			            label_info[ label_counter ].start_address = buf;
			        }
			        else{
			            if ( label_counter >= 0 )
			                label_info[ label_counter ].num_of_lines++;
			            while( buf[0] != 0x0a ) buf.inc();
			            buf.inc();
			            current_line++;
			        }
			    }
			
			    label_info[num_of_labels].start_address = null;
			
			    return 0;
			}
			
			public LabelInfo lookupLabel( CharPtr label )
			{
				int i = findLabel( label );
			
			    findAndAddLog( log_info[LABEL_LOG], label_info[i].name, true );
			    return label_info[i];
			}
			
			public LabelInfo lookupLabelNext( CharPtr label )
			{
				int i = findLabel( label );
			    if ( i+1 < num_of_labels ){
			        findAndAddLog( log_info[LABEL_LOG], label_info[i+1].name, true );
			        return label_info[i+1];
			    }
			
			    return label_info[num_of_labels];
			}
			
			public LogLink findAndAddLog( LogInfo info, CharPtr name, bool add_flag )
			{
				char[] capital_name = new char[256];
			    for ( uint i=0 ; i<strlen(name)+1 ; i++ ){
			        capital_name[i] = name[i];
			        if ( 'a' <= capital_name[i] && capital_name[i] <= 'z' ) capital_name[i] = (char)(capital_name[i] + ('A' - 'a'));
			        else if ( capital_name[i] == '/' ) capital_name[i] = '\\';
			    }
			
			    LogLink cur = info.root_log.next;
			    while( null!=cur ){
			        if ( 0==strcmp( cur.name, capital_name ) ) break;
			        cur = cur.next;
			    }
			    if ( !add_flag || null!=cur ) return cur;
			
			    LogLink link = new LogLink();
			    link.name = new char[strlen(capital_name)+1];
			    strcpy( link.name, capital_name );
			    info.current_log.next = link;
			    info.current_log = info.current_log.next;
			    info.num_logs++;
			
			    return link;
			}
			
			public void resetLog( LogInfo info )
			{
			    LogLink link = info.root_log.next;
			    while( null!=link ){
			        LogLink tmp = link;
			        link = link.next;
			        tmp=null;//delete tmp;
			    }
			
			    info.root_log.next = null;
			    info.current_log = info.root_log;
			    info.num_logs = 0;
			}
			
			public ArrayVariable getRootArrayVariable(){
				return root_array_variable;
			}
			
			public void addNumAlias( CharPtr str, int no )
			{
			    Alias p_num_alias = new Alias( str, no );
			    last_num_alias.next = p_num_alias;
			    last_num_alias = last_num_alias.next;
			}
			
			public void addStrAlias( CharPtr str1, CharPtr str2 )
			{
			    Alias p_str_alias = new Alias( str1, str2 );
			    last_str_alias.next = p_str_alias;
			    last_str_alias = last_str_alias.next;
			}
			
			public bool findNumAlias( CharPtr str, ref int value )
			{
				Alias p_num_alias = root_num_alias.next;
			    while( null!=p_num_alias ){
					if ( 0==strcmp( p_num_alias.alias, str ) ){
					    value = p_num_alias.num;
					    return true;
					}
					p_num_alias = p_num_alias.next;
			    }
			    return false;
			}
			
			public bool findStrAlias( CharPtr str, CharPtr buffer )
			{
				Alias p_str_alias = root_str_alias.next;
			    while( null!=p_str_alias ){
					if ( 0==strcmp( p_str_alias.alias, str ) ){
					    strcpy( buffer, p_str_alias.str );
					    return true;
					}
					p_str_alias = p_str_alias.next;
			    }
			    return false;
			}
			
			public void processError( CharPtr str, CharPtr title=null, CharPtr detail=null, bool is_warning=false, bool is_simple=false )
			{
			    //if not yet running the script, no line no/cmd - keep it simple
			    if (script_buffer == null)
			        is_simple = true;
			
			    if (title == null)
			        title = "Error";
			    CharPtr type = is_warning ? "Warning" : "Fatal";
			
			    if (is_simple) {
			        fprintf(stderr, " ***[%s] %s: %s ***\n", type, title, str);
			        if (null!=detail)
			            fprintf(stderr, "\t%s\n", detail);
			
			        if (is_warning && !strict_warnings) return;
			
			        if (null==ons) exit(-1); //nothing left to do without doErrorBox
			
			        if (! ons.doErrorBox(title, str, true, is_warning))
			            return;
			
			        if (is_warning)
			            fprintf(stderr, " ***[Fatal] User terminated at warning ***\n");
			
			    } else {
			
			    	char[] errhist = new char[1024], errcmd = new char[128];
			
			        LabelInfo label = getLabelByAddress(getCurrent());
			        int lblinenum = getLineByAddress(getCurrent());
			        int linenum = label.start_line + lblinenum + 1;
			
			        errcmd[0] = '\0';
			        if (strlen(current_cmd) > 0) {
			            if (current_cmd_type == CMD_BUILTIN)
			                snprintf(errcmd, 128, ", cmd \"%s\"", current_cmd);
			            else if (current_cmd_type == CMD_USERDEF)
			                snprintf(errcmd, 128, ", user-defined cmd \"%s\"", current_cmd);
			        }
			        fprintf(stderr, " ***[%s] %s at line %d (*%s:%d)%s - %s ***\n",
			                type, title, linenum, label.name, lblinenum, errcmd, str);
			        if (null!=detail)
			            fprintf(stderr, "\t%s\n", detail);
			        if (null!=string_buffer && string_buffer[0]!=0)
			            fprintf(stderr, "\t(String buffer: [%s])\n", string_buffer);
			
			        if (is_warning && !strict_warnings) return;
			
			        if (null==ons) exit(-1); //nothing left to do without doErrorBox
			
			        if (is_warning) {
			            snprintf(errhist, 1024, "%s\nat line %d (*%s:%d)%s\n%s",
			                     str, linenum, label.name, lblinenum, errcmd,
			                     null!=detail ? detail : "");
			
			            if(!ons.doErrorBox(title, errhist, false, true))
			                return;
			
			            fprintf(stderr, " ***[Fatal] User terminated at warning ***\n");
			        }
			
			        //Mion: grabbing the current line in the script & up to 2 previous ones,
			        // in-place (replaces the newlines with '\0', and then puts the newlines
			        // back when finished)
			        int i;
			        CharPtr[] line = new CharPtr[3], tmp = new CharPtr[4];
			        for (i=0; i<4; i++) tmp[i] = null;
			        CharPtr end = getCurrent();
			        while (0!=end[0] && end[0] != 0x0a) end.inc();
			        if (0!=end[0]) tmp[3] = end;
			        end[0] = '\0';
			        CharPtr buf = getCurrent();
			        for (i=2; i>=0; i--) {
			            if (linenum + i - 3 > 0) {
			        		while (buf[0] != 0x0a) buf.dec();
			                tmp[i] = buf;
			                buf[0] = '\0';
			                line[i] = new CharPtr(buf, +1);
			            } else if (linenum + i - 3 == 0) {
			                line[i] = script_buffer;
			            } else
			                line[i] = end;
			        }
			
			        snprintf(errhist, 1024, "%s\nat line %d (*%s:%d)%s\n\n| %s\n| %s\n> %s",
			                 str, linenum, label.name, lblinenum, errcmd,
			                 line[0], line[1], line[2]);
			
			        for (i=0; i<4; i++) {
			        	if (null!=tmp[i]) tmp[i][0] = (char)0x0a;
			        }
			
			        if (! ons.doErrorBox(title, errhist, false, false))
			            return;
			
			    }
			
			    exit(-1);
			}
			
			public void errorAndExit( CharPtr str, CharPtr detail = null, CharPtr title = null, bool is_warning = false )
			{
			    if (title == null)
			        title = "Script Error";
			
			    processError(str, detail, title, is_warning);
			}
			
			public void simpleErrorAndExit( CharPtr str, CharPtr title=null, CharPtr detail=null, bool is_warning=false )
			{
			    if (title == null)
			        title = "Script Error";
			
			    processError(str, detail, title, is_warning, true);
			}
			
			public void addStringBuffer( char ch )
			{
			    if (string_counter+1 == STRING_BUFFER_LENGTH)
			        errorAndExit("addStringBuffer: string length exceeds 2048.");
			    string_buffer[string_counter++] = ch;
			    string_buffer[string_counter] = '\0';
			}
			
			public void trimStringBuffer( uint n )
			{
				string_counter = (int)(string_counter - n);
			    if (string_counter < 0)
			        string_counter = 0;
			    string_buffer[string_counter] = '\0';
			}
			
			public void pushStringBuffer(int offset)
			{
			    strcpy(gosub_string_buffer, string_buffer);
			    gosub_string_offset = offset;
			}
			
			public int popStringBuffer()
			{
				strcpy(string_buffer, gosub_string_buffer);
			    text_flag = true;
			    return gosub_string_offset;
			}
			
			public VariableData getVariableData(int no)
			{
				if (no >= 0 && no < VARIABLE_RANGE)
			        return variable_data[no];
			
			    for (int i=0 ; i<num_extended_variable_data ; i++)
			        if (extended_variable_data[i].no == no) 
			            return extended_variable_data[i].vd;
			        
			    num_extended_variable_data++;
			    if (num_extended_variable_data == max_extended_variable_data){
			    	//FIXME:???
			    	//ExtendedVariableData tmp = extended_variable_data[0];
			        extended_variable_data = new ExtendedVariableData[max_extended_variable_data*2];
			        if (null != extended_variable_data[0]/*tmp*/){
			        	memcpy(extended_variable_data, extended_variable_data[0]/*tmp*/, /*sizeof(ExtendedVariableData)**/(uint)max_extended_variable_data);
			            extended_variable_data[0] = null;//delete[] tmp;
			        }
			        max_extended_variable_data *= 2;
			    }
			
			    extended_variable_data[num_extended_variable_data-1].no = no;
			
			    return extended_variable_data[num_extended_variable_data-1].vd;
			}
			
			// ----------------------------------------
			// Private methods
			
			public int findLabel( CharPtr label )
			{
				int i;
				char[] capital_label = new char[256];
			
			    for ( i=0 ; i<(int)strlen( label )+1 ; i++ ){
			        capital_label[i] = label[i];
			        if ( 'A' <= capital_label[i] && capital_label[i] <= 'Z' ) capital_label[i] = (char)(capital_label[i] + ('a' - 'A'));
			    }
			    for ( i=0 ; i<num_of_labels ; i++ ){
			        if ( 0==strcmp( label_info[i].name, capital_label ) )
			            return i;
			    }
			
			    snprintf(errbuf, MAX_ERRBUF_LEN, "Label \"*%s\" not found.", label);
			    errorAndExit( errbuf, null, "Label Error" );
			
			    return -1; // dummy
			}
			
			public CharPtr checkComma( CharPtr buf )
			{
				SKIP_SPACE( buf );
				if (buf[0] == ','){
			        end_status |= END_COMMA;
			        buf.inc();
			        SKIP_SPACE( buf );
			    }
			
				return new CharPtr(buf);
			}
			
			public void parseStr( ref CharPtr buf )
			{
			    SKIP_SPACE( buf );
			
			    if ( buf[0] == '(' ){
			        // (foo) bar baz : apparently returns bar if foo has been
			        // viewed, baz otherwise.
			        // (Rather like a trigram implicitly using "fchk")
			
			        buf.inc();
			        parseStr(ref buf);
			        SKIP_SPACE( buf );
			        if ( buf[0] != ')' ) errorAndExit("parseStr: missing ')'.");
			        buf.inc();
			
			        if ( null!=findAndAddLog( log_info[FILE_LOG], str_string_buffer, false ) ){
			            parseStr(ref buf);
			            CharPtr tmp_buf = new char[ strlen( str_string_buffer ) + 1 ];
			            strcpy( tmp_buf, str_string_buffer );
			            parseStr(ref buf);
			            strcpy( str_string_buffer, tmp_buf );
			            tmp_buf = null;//delete[] tmp_buf;
			        }
			        else{
			            parseStr(ref buf);
			            parseStr(ref buf);
			        }
			        current_variable.type |= VAR_CONST;
			    }
			    else if ( buf[0] == '$' ){
			    	buf.inc();
			        int no = parseInt(ref buf);
			        VariableData vd = getVariableData(no);
			
			        if ( null!=vd.str )
			            strcpy( str_string_buffer, vd.str );
			        else
			            str_string_buffer[0] = '\0';
			        current_variable.type = VAR_STR;
			        current_variable.var_no = no;
			    }
			    else if ( buf[0] == '"' ){
			        int c=0;
			        buf.inc();
			        while ( buf[0] != '"' && buf[0] != 0x0a ) {
			        	str_string_buffer[c++] = buf[0]; buf.inc(); }
			        str_string_buffer[c] = '\0';
			        if ( buf[0] == '"' ) buf.inc();
			        current_variable.type |= VAR_CONST;
			    }
			    else if ( buf[0] == '`' ){
			        int c=0;
			        str_string_buffer[c++] = buf[0]; buf.inc();
			        while ( buf[0] != '`' && buf[0] != 0x0a ) {
			        	str_string_buffer[c++] = buf[0]; buf.inc(); }
			        str_string_buffer[c] = '\0';
			        if ( buf[0] == '`' ) buf.inc();
			        current_variable.type |= VAR_CONST;
			        end_status |= END_1BYTE_CHAR;
			    }
			    else if ( buf[0] == '#' ){ // for color
			    	for ( int i=0 ; i<7 ; i++ ) {
			    		str_string_buffer[i] = buf[0]; buf.inc(); }
			        str_string_buffer[7] = '\0';
			        current_variable.type = VAR_NONE;
			    }
			    else if ( buf[0] == '*' ){ // label
			        int c=0;
			        str_string_buffer[c++] = buf[0]; buf.inc();
			        SKIP_SPACE(buf);
			        char ch = buf[0];
			        while((ch >= 'a' && ch <= 'z') || 
			              (ch >= 'A' && ch <= 'Z') ||
			              (ch >= '0' && ch <= '9') ||
			              ch == '_'){
			        	if (ch >= 'A' && ch <= 'Z') ch = (char)(ch + ('a' - 'A'));
			            str_string_buffer[c++] = ch;
			            buf.inc(); ch = buf[0];
			        }
			        str_string_buffer[c] = '\0';
			        current_variable.type |= VAR_CONST;
			    }
			    else{ // str alias
			    	char ch; char[] alias_buf = new char[512];
			        int alias_buf_len = 0;
			        bool first_flag = true;
			
			        while(true){
			            if ( alias_buf_len == 511 ) break;
			            ch = buf[0];
			
			            if ( (ch >= 'a' && ch <= 'z') ||
			                 (ch >= 'A' && ch <= 'Z') ||
			                 ch == '_' ){
			            	if (ch >= 'A' && ch <= 'Z') ch = (char)(ch + ('a' - 'A'));
			                first_flag = false;
			                alias_buf[ alias_buf_len++ ] = ch;
			            }
			            else if ( ch >= '0' && ch <= '9' ){
			                if ( first_flag )
			                    errorAndExit("parseStr: string alias cannot start with a digit.");
			                first_flag = false;
			                alias_buf[ alias_buf_len++ ] = ch;
			            }
			            else break;
			            buf.inc();
			        }
			        alias_buf[alias_buf_len] = '\0';
			
			        if ( alias_buf_len == 0 ){
			            str_string_buffer[0] = '\0';
			            current_variable.type = VAR_NONE;
			            return;
			        }
			
			        if (!findStrAlias( new CharPtr(alias_buf), str_string_buffer )) {
			            snprintf(errbuf, MAX_ERRBUF_LEN, "Undefined string alias '%s'", alias_buf);
			            errorAndExit(errbuf);
			        }
			        current_variable.type |= VAR_CONST;
			    }
			}
			
			public int parseInt( ref CharPtr buf )
			{
				int ret = 0;
			
			    SKIP_SPACE( buf );
			
			    if ( buf[0] == '%' ){
			    	buf.inc();
			        current_variable.var_no = parseInt(ref buf);
			        current_variable.type = VAR_INT;
			        return getVariableData(current_variable.var_no).num;
			    }
			    else if ( buf[0] == '?' ){
			    	ArrayVariable av = new ArrayVariable();
			        current_variable.var_no = parseArray( ref buf, av );
			        current_variable.type = VAR_ARRAY;
			        current_variable.array = av;
			        return getArrayPtr( current_variable.var_no, current_variable.array, 0 )[0];
			    }
			    else{
			    	char ch; char[] alias_buf = new char[256];
			        int alias_buf_len = 0, alias_no = 0;
			        bool direct_num_flag = false;
			        bool num_alias_flag = false;
			
			        CharPtr buf_start = new CharPtr(buf);
			        while( true ){
			        	ch = buf[0];
			
			            if ( (ch >= 'a' && ch <= 'z') ||
			                 (ch >= 'A' && ch <= 'Z') ||
			                 ch == '_' ){
			        		if (ch >= 'A' && ch <= 'Z') ch += (char)('a' - 'A');
			                if ( direct_num_flag ) break;
			                num_alias_flag = true;
			                alias_buf[ alias_buf_len++ ] = ch;
			            }
			            else if ( ch >= '0' && ch <= '9' ){
			                if ( !num_alias_flag ) direct_num_flag = true;
			                if ( direct_num_flag )
			                    alias_no = alias_no * 10 + ch - '0';
			                else
			                    alias_buf[ alias_buf_len++ ] = ch;
			            }
			            else break;
			            buf.inc();
			        }
			
			        if ( CharPtr.minus(buf, buf_start)  == 0 ){
			            current_variable.type = VAR_NONE;
			            return 0;
			        }
			
			        /* ---------------------------------------- */
			        /* Solve num aliases */
			        if ( num_alias_flag ){
			            alias_buf[ alias_buf_len ] = '\0';
			
			            if ( !findNumAlias( new CharPtr( alias_buf), ref alias_no ) ) {
			                //printf("can't find num alias for %s... assume 0.\n", alias_buf );
			                current_variable.type = VAR_NONE;
			                buf = new CharPtr(buf_start);
			                return 0;
				    	}
					}
			        current_variable.type = VAR_INT | VAR_CONST;
			        ret = alias_no;
			    }
			
			    SKIP_SPACE( buf );
			
			    return ret;
			}
			
			public int parseIntExpression( ref CharPtr buf )
			{
				int[] num = new int[3], op = new int[2]; // internal buffer
			
			    SKIP_SPACE( buf );
			
			    int temp = 0;
			    readNextOp( ref buf, ref temp, false, ref num[0] );
			
			    readNextOp( ref buf, ref op[0], true, ref num[1] );
			    if ( op[0] == OP_INVALID )
			        return num[0];
			
			    while(true){
			        readNextOp( ref buf, ref op[1], true, ref num[2] );
			        if ( op[1] == OP_INVALID ) break;
			
			        if ( 0==(op[0] & 0x04) && 0!=(op[1] & 0x04) ){ // if priority of op[1] is higher than op[0]
			            num[1] = calcArithmetic( num[1], op[1], num[2] );
			        }
			        else{
			            num[0] = calcArithmetic( num[0], op[0], num[1] );
			            op[0] = op[1];
			            num[1] = num[2];
			        }
			    }
			    return calcArithmetic( num[0], op[0], num[1] );
			}
			
			/*
			 * Internal buffer looks like this.
			 *   num[0] op[0] num[1] op[1] num[2]
			 * If priority of op[0] is higher than op[1], (num[0] op[0] num[1]) is computed,
			 * otherwise (num[1] op[1] num[2]) is computed.
			 * Then, the next op and num is read from the script.
			 * Num is an immediate value, a variable or a bracketed expression.
			 */
			//FIXME: added is_not_null_op
			public void readNextOp( ref CharPtr buf, ref int op, bool is_not_null_op, ref int num )
			{
			    bool minus_flag = false;
			    SKIP_SPACE(buf);
			    CharPtr buf_start = new CharPtr(buf);
			
			    if ( is_not_null_op ){
			    	if      ( buf[0] == '+' ) op = OP_PLUS;
			    	else if ( buf[0] == '-' ) op = OP_MINUS;
			    	else if ( buf[0] == '*' ) op = OP_MULT;
			    	else if ( buf[0] == '/' ) op = OP_DIV;
			        else if ( buf[0] == 'm' &&
			                  buf[1] == 'o' &&
			                  buf[2] == 'd' &&
			                  ( buf[3] == ' '  ||
			                    buf[3] == '\t' ||
			                    buf[3] == '$' ||
			                    buf[3] == '%' ||
			                    buf[3] == '?' ||
			                    ( buf[3] >= '0' && buf[3] <= '9') ))
			    		op = OP_MOD;
			        else{
			    		op = OP_INVALID;
			            return;
			        }
			    	if ( op == OP_MOD ) buf.inc(3);
			    	else                 buf.inc();
			        SKIP_SPACE(buf);
			    }
			    else{
			        if ( buf[0] == '-' ){
			            minus_flag = true;
			            buf.inc();
			            SKIP_SPACE(buf);
			        }
			    }
			
			    if ( buf[0] == '(' ){
			    	buf.inc();
			    	num = parseIntExpression( ref buf );
			        if (minus_flag) num = -num;
			        SKIP_SPACE(buf);
			        if ( buf[0] != ')' ) errorAndExit("Missing ')' in expression");
			        buf.inc();
			    }
			    else{
			        num = parseInt( ref buf );
			        if (minus_flag) num = -num;
			        if ( current_variable.type == VAR_NONE ){
			            if (is_not_null_op) op = OP_INVALID;
			            buf = new CharPtr(buf_start);
			        }
			    }
			}
			
			public int calcArithmetic( int num1, int op, int num2 )
			{
				int ret=0;
			
			    if      ( op == OP_PLUS )  ret = num1+num2;
			    else if ( op == OP_MINUS ) ret = num1-num2;
			    else if ( op == OP_MULT )  ret = num1*num2;
			    else if ( op == OP_DIV )   ret = num1/num2;
			    else if ( op == OP_MOD )   ret = num1%num2;
			
			    current_variable.type = VAR_INT | VAR_CONST;
			
			    return ret;
			}
			
			public int parseArray( ref CharPtr buf, ArrayVariable array )
			{
				SKIP_SPACE( buf );
			
				buf.inc(); // skip '?'
			    int no = parseInt( ref buf );
			
			    SKIP_SPACE( buf );
			    array.num_dim = 0;
			    while ( buf[0] == '[' ){
			    	buf.inc();
			        array.dim[array.num_dim] = parseIntExpression(ref buf);
			        array.num_dim++;
			        SKIP_SPACE( buf );
			        if ( buf[0] != ']' ) errorAndExit( "parseArray: missing ']'." );
			        buf.inc();
			    }
			    for ( int i=array.num_dim ; i<20 ; i++ ) array.dim[i] = 0;
			
			    return no;
			}
			
			public IntPtr getArrayPtr( int no, ArrayVariable array, int offset )
			{
				ArrayVariable av = root_array_variable;
			    while(null!=av){
			        if (av.no == no) break;
			        av = av.next;
			    }
			    if (av == null) {
			        snprintf(errbuf, MAX_ERRBUF_LEN, "Undeclared array number %d", no);
			        errorAndExit( errbuf, null, "Array access error" );
			    }
			
			    int dim = 0, i;
			    for ( i=0 ; i<av.num_dim ; i++ ){
			        if ( av.dim[i] <= array.dim[i] )
			            errorAndExit( "Array access out of bounds", "dim[i] <= array.dim[i]", "Array access error" );
			        dim = dim * av.dim[i] + array.dim[i];
			    }
			    if ( av.dim[i-1] <= array.dim[i-1] + offset )
			        errorAndExit( "Array access out of bounds", "dim[i-1] <= array.dim[i-1] + offset", "Array access error" );
			
			    return new IntPtr(av.data, dim+offset);
			}
			
			public void declareDim()
			{
			    current_script = next_script;
			    CharPtr buf = new CharPtr(current_script);
			
			    if (null!=current_array_variable){
			        current_array_variable.next = new ArrayVariable();
			        current_array_variable = current_array_variable.next;
			    }
			    else{
			        root_array_variable = new ArrayVariable();
			        current_array_variable = root_array_variable;
			    }
			
			    ArrayVariable array = new ArrayVariable();
			    current_array_variable.no = parseArray( ref buf, array );
			
			    int dim = 1;
			    current_array_variable.num_dim = array.num_dim;
			    for ( int i=0 ; i<array.num_dim ; i++ ){
			        current_array_variable.dim[i] = array.dim[i]+1;
			        dim *= (array.dim[i]+1);
			    }
			    current_array_variable.data = new IntPtr(new int[dim]);
			    memset( current_array_variable.data, 0, (uint)(sizeof(int) * dim) );
			
			    next_script = new CharPtr(buf);
			}
		}
	}
}
