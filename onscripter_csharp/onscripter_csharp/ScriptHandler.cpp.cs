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
//		
//		#define TMP_SCRIPT_BUF_LEN 4096
//		#define STRING_BUFFER_LENGTH 2048
		
		public static void SKIP_SPACE(CharPtr p) { while ( p[0] == ' ' || p[0] == '\t' ) p.inc(); }
		
		public partial class ScriptHandler {
			public ScriptHandler()
			{
//			    num_of_labels = 0;
//			    script_buffer = NULL;
//			    kidoku_buffer = NULL;
//			    log_info[LABEL_LOG].filename = "NScrllog.dat";
//			    log_info[FILE_LOG].filename  = "NScrflog.dat";
//			    clickstr_list = NULL;
//			
//			    string_buffer       = new char[STRING_BUFFER_LENGTH];
//			    str_string_buffer   = new char[STRING_BUFFER_LENGTH];
//			    saved_string_buffer = new char[STRING_BUFFER_LENGTH];
//			    gosub_string_buffer = new char[STRING_BUFFER_LENGTH];
//			
//			    variable_data = new VariableData[VARIABLE_RANGE];
//			    extended_variable_data = NULL;
//			    num_extended_variable_data = 0;
//			    max_extended_variable_data = 1;
//			    root_array_variable = NULL;
//			
//			    screen_size = SCREEN_SIZE_640x480;
//			    global_variable_border = 200;
//			    
//			    ons = NULL;
//			    
//			    archive_path = NULL;
//			    save_path = NULL;
//			    savedir = NULL;
//			    script_path = NULL;
//			    game_identifier = NULL;
//			    game_hash = 0;
//			    current_cmd[0] = '\0';
//			    current_cmd_type = CMD_NONE;
//			    strict_warnings = false;
//			
//			    default_script = NO_SCRIPT_PREF;
//			    preferred_script = NO_SCRIPT_PREF;
//			    system_menu_script = NO_SCRIPT_PREF;
//			
//			    rgosub_wait_pos = NULL;
//			    rgosub_wait_1byte = NULL;
//			    total_rgosub_wait_size = num_rgosub_waits = cur_rgosub_wait = 0;
//			    is_rgosub_click = rgosub_click_newpage = rgosub_1byte_mode = false;
//			    rgosub_flag = false; // roto 20100205
//			
//			    ignore_textgosub_newline = false;
			}
			
//			ScriptHandler::~ScriptHandler()
//			{
//			    reset();
//			
//			    if ( script_buffer ) delete[] script_buffer;
//			    if ( kidoku_buffer ) delete[] kidoku_buffer;
//			
//			    delete[] string_buffer;
//			    delete[] str_string_buffer;
//			    delete[] saved_string_buffer;
//			    delete[] gosub_string_buffer;
//			    delete[] variable_data;
//			    
//			    if (script_path) delete[] script_path;
//			    if (save_path) delete[] save_path;
//			    if (savedir) delete[] savedir;
//			
//			    if (game_identifier) delete[] game_identifier;
//			}
			
			public void reset()
			{
//			    for (int i=0 ; i<VARIABLE_RANGE ; i++)
//			        variable_data[i].reset(true);
//			
//			    if (extended_variable_data) delete[] extended_variable_data;
//			    extended_variable_data = NULL;
//			    num_extended_variable_data = 0;
//			    max_extended_variable_data = 1;
//			
//			    ArrayVariable *av = root_array_variable;
//			    while(av){
//			        ArrayVariable *tmp = av;
//			        av = av->next;
//			        delete tmp;
//			    }
//			    root_array_variable = current_array_variable = NULL;
//			
//			    // reset log info
//			    resetLog( log_info[LABEL_LOG] );
//			    resetLog( log_info[FILE_LOG] );
//			
//			    // reset number alias
//			    Alias *alias;
//			    alias = root_num_alias.next;
//			    while (alias){
//			        Alias *tmp = alias;
//			        alias = alias->next;
//			        delete tmp;
//			    };
//			    last_num_alias = &root_num_alias;
//			    last_num_alias->next = NULL;
//			
//			    // reset string alias
//			    alias = root_str_alias.next;
//			    while (alias){
//			        Alias *tmp = alias;
//			        alias = alias->next;
//			        delete tmp;
//			    };
//			    last_str_alias = &root_str_alias;
//			    last_str_alias->next = NULL;
//			
//			    // reset misc. variables
//			    end_status = END_NONE;
//			    kidokuskip_flag = false;
//			    text_flag = true;
//			    linepage_flag = false;
//			    english_mode = false;
//			    textgosub_flag = false;
//			    skip_enabled = false;
//			    if (clickstr_list){
//			        delete[] clickstr_list;
//			        clickstr_list = NULL;
//			    }
//			    internal_current_script = NULL;
//			    preferred_script = default_script;
//			
//			    if (rgosub_wait_pos) delete[] rgosub_wait_pos;
//			    rgosub_wait_pos = NULL;
//			    if (rgosub_wait_1byte) delete[] rgosub_wait_1byte;
//			    rgosub_wait_1byte = NULL;
//			    total_rgosub_wait_size = num_rgosub_waits = cur_rgosub_wait = 0;
//			    is_rgosub_click = rgosub_click_newpage = rgosub_1byte_mode = false;
			}
			
			public FILEPtr fopen( CharPtr path, CharPtr mode, bool save, bool usesavedir )
			{
				return null;
//			    const char* root;
//			    char *file_name;
//			    FILE *fp = NULL;
//			
//			    if (usesavedir && savedir) {
//			        root = savedir;
//			        file_name = new char[strlen(root)+strlen(path)+1];
//			        sprintf( file_name, "%s%s", root, path );
//			        //printf("handler:fopen(\"%s\")\n", file_name);
//			
//			        fp = ::fopen( file_name, mode );
//			    } else if (save) {
//			        root = save_path;
//			        file_name = new char[strlen(root)+strlen(path)+1];
//			        sprintf( file_name, "%s%s", root, path );
//			        //printf("handler:fopen(\"%s\")\n", file_name);
//			
//			        fp = ::fopen( file_name, mode );
//			    } else {
//			        // search within archive_path(s)
//			        file_name = new char[archive_path->max_path_len()+strlen(path)+1];
//			        for (int n=0; n<archive_path->get_num_paths(); n++) {
//			            root = archive_path->get_path(n);
//			            //printf("root: %s\n", root);
//			            sprintf( file_name, "%s%s", root, path );
//			            //printf("handler:fopen(\"%s\")\n", file_name);
//			            fp = ::fopen( file_name, mode );
//			            if (fp != NULL) break;
//			        }
//			    }
//			    delete[] file_name;
//			    return fp;
			}
			
			public FILEPtr fopen( CharPtr root, CharPtr path, CharPtr mode )
			{
				return null;
//			    char *file_name;
//			    FILE *fp = NULL;
//			
//			    file_name = new char[strlen(root)+strlen(path)+1];
//			    sprintf( file_name, "%s%s", root, path );
//			    //printf("handler:fopen(\"%s\")\n", file_name);
//			
//			    fp = ::fopen( file_name, mode );
//			
//			    delete[] file_name;
//			    return fp;
			}
			
			public void setKeyTable( UnsignedCharPtr key_table )
			{
//			    int i;
//			    if (key_table){
//			        key_table_flag = true;
//			        for (i=0 ; i<256 ; i++) this->key_table[i] = key_table[i];
//			    }
//			    else{
//			        key_table_flag = false;
//			        for (i=0 ; i<256 ; i++) this->key_table[i] = i;
//			    }
			}
			
			public void setSavedir( CharPtr dir )
			{
//			    savedir = new char[ strlen(dir) + strlen(save_path) + 2];
//			    sprintf( savedir, "%s%s%c", save_path, dir, DELIMITER );
//			    mkdir(savedir
//			#ifndef WIN32
//			          , 0755
//			#endif
//			         );
			}
			
			// basic parser function
			public CharPtr readToken()
			{
				return null;
//			    current_script = next_script;
//			    char *buf = current_script;
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			    num_rgosub_waits = cur_rgosub_wait = 0;
//			
//			    text_flag = false;
//			
//			    if (rgosub_flag && is_rgosub_click){
//			        string_counter = 0;
//			        char ch = rgosub_click_newpage ? '\\' : '@';
//			        addStringBuffer( ch );
//			        if (rgosub_1byte_mode)
//			            addStringBuffer( '`' );
//			        rgosub_wait_1byte[num_rgosub_waits] = rgosub_1byte_mode;
//			        rgosub_wait_pos[num_rgosub_waits++] = buf;
//			        text_flag = true;
//			        if (!rgosub_1byte_mode){
//			            SKIP_SPACE( buf );
//			        }
//			    } else {
//			        SKIP_SPACE( buf );
//			    }
//			
//			    markAsKidoku( buf );
//			
//			  readTokenTop:
//			    if (!is_rgosub_click)
//			        string_counter = 0;
//			    char ch = *buf;
//			    if ((ch == ';') && !is_rgosub_click){ // comment
//			        while ( ch != 0x0a && ch != '\0' ){
//			            addStringBuffer( ch );
//			            ch = *++buf;
//			        }
//			    }
//			    else if (is_rgosub_click || ch & 0x80 ||
//			             (ch >= '0' && ch <= '9') ||
//			             ch == '@' || ch == '\\' || ch == '/' ||
//			             ch == '%' || ch == '?' || ch == '$' ||
//			             ch == '[' || ch == '(' || ch == '`' ||
//			             ch == '!' || ch == '#' || ch == ',' ||
//			             ch == '{' || ch == '<' || ch == '>' ||
//			             ch == '"'){ // text
//			
//			        bool ignore_click_flag = false;
//			        bool clickstr_flag = false;
//			        bool in_pretext_tag = (ch == '[');
//			        bool is_nscr_english = (english_mode && (ch == '>'));
//			        if (is_nscr_english)
//			            ch = *++buf;
//			        bool in_1byte_mode = is_nscr_english;
//			        if (is_rgosub_click)
//			            in_1byte_mode = rgosub_1byte_mode;
//			        while (1){
//			            if (rgosub_flag && (num_rgosub_waits == total_rgosub_wait_size)){
//			                //double the rgosub wait buffer size
//			                char **tmp = rgosub_wait_pos;
//			                bool *tmp2 = rgosub_wait_1byte;
//			                total_rgosub_wait_size *= 2;
//			                rgosub_wait_pos = new char*[total_rgosub_wait_size];
//			                rgosub_wait_1byte = new bool[total_rgosub_wait_size];
//			                for (int i=0; i<num_rgosub_waits; i++){
//			                    rgosub_wait_pos[i] = tmp[i];
//			                    rgosub_wait_1byte[i] = tmp2[i];
//			                }
//			                delete[] tmp;
//			                delete[] tmp2;
//			            }
//			            char *tmp = buf;
//			            SKIP_SPACE(tmp);
//			            if (!(is_nscr_english || in_1byte_mode) ||
//			                (*tmp == 0x0a) || (*tmp == 0x00)) {
//			                // always skip trailing spaces
//			                buf = tmp;
//			                ch = *buf;
//			            }
//			            if ((ch == 0x0a) || (ch == 0x00)) break;
//			            if ( IS_TWO_BYTE(ch) ){
//			                if (!in_pretext_tag && !ignore_click_flag &&
//			                    (checkClickstr(buf) > 0))
//			                    clickstr_flag = true;
//			                else
//			                    clickstr_flag = false;
//			                addStringBuffer( ch );
//			                ch = *++buf;
//			                if (ch == 0x0a || ch == '\0') break; //invalid 2-byte char
//			                addStringBuffer( ch );
//			                ch = *++buf;
//			                //Mion: ons-en processes clickstr chars here in readToken,
//			                // not in ONScripterLabel_text - adds a special
//			                // sequence '\@' after the clickstr char
//			                if (clickstr_flag) {
//			                    // insert a clickwait-or-newpage
//			                    addStringBuffer('\\');
//			                    addStringBuffer('@');
//			                    if (textgosub_flag) {
//			                        char *tmp = buf;
//			                        SKIP_SPACE(tmp);
//			                        // if "ignore-textgosub-newline" cmd-line option,
//			                        // ignore newline after clickwait if textgosub used
//			                        // (fixes some older onscripter-en games)
//			                        if (ignore_textgosub_newline && (*tmp == 0x0a))
//			                            buf = tmp+1;
//			                        break;
//			                    }
//			                    if (rgosub_flag){
//			                        rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
//			                        rgosub_wait_pos[num_rgosub_waits++] = buf+1;
//			                    }
//			                }
//			                ignore_click_flag = clickstr_flag = false;
//			            }
//			            else {
//			                if (ch == '`'){
//			                    addStringBuffer( ch );
//			                    ch = *++buf;
//			                    if (!is_nscr_english)
//			                        in_1byte_mode = !in_1byte_mode;
//			                }
//			                else if (in_pretext_tag && (ch == ']')){
//			                    addStringBuffer( ch );
//			                    ch = *++buf;
//			                    in_pretext_tag = false;
//			                    in_1byte_mode = false;
//			                }
//			                else if (in_1byte_mode) {
//			                    if (in_pretext_tag){
//			                        addStringBuffer( ch );
//			                        ch = *++buf;
//			                        continue;
//			                    }
//			                    if (ch == '$'){
//			                        if (buf[1] == '$') ++buf; else{
//			                            addStrVariable(&buf);
//			                            while (*--buf == ' ');
//			                            ch = *++buf;
//			                            ignore_click_flag = false;
//			                            continue;
//			                        }
//			                    }
//			                    if ((ch == '_') && (checkClickstr(buf+1) > 0)) {
//			                        ignore_click_flag = true;
//			                        ch = *++buf;
//			                        continue;
//			                    }
//			                    if ((ch == '@') || (ch == '\\')) {
//			                        if (!ignore_click_flag){
//			                            addStringBuffer( ch );
//			                            if (rgosub_flag){
//			                                rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
//			                                rgosub_wait_pos[num_rgosub_waits++] = buf+1;
//			                            }
//			                        }
//			                        if (textgosub_flag){
//			                            ++buf;
//			                            // if "ignore-textgosub-newline", ignore
//			                            // newline after clickwait if textgosub
//			                            // (fixes older onscripter-en games)
//			                            char *tmp = buf;
//			                            SKIP_SPACE(tmp);
//			                            if (ignore_textgosub_newline &&(*tmp == 0x0a))
//			                                buf = tmp+1;
//			                            break;
//			                        }
//			                        ch = *++buf;
//			                        continue;
//			                    }
//			                    if (!ignore_click_flag && (checkClickstr(buf) > 0))
//			                        clickstr_flag = true;
//			                    else
//			                        clickstr_flag = false;
//			                    // no ruby in 1byte mode; escape parens
//			                    if (ch == '(') {
//			                        addStringBuffer( LEFT_PAREN );
//			                    } else if (ch == ')') {
//			                        addStringBuffer( RIGHT_PAREN );
//			                    } else if (ch == 0x0a || ch == '\0') break;
//			                    else
//			                        addStringBuffer( ch );
//			                    ch = *++buf;
//			                    //Mion: ons-en processes clickstr chars here in readToken,
//			                    // not in ONScripterLabel_text - adds a special
//			                    // sequence '\@' after the clickstr char
//			                    if (clickstr_flag) {
//			                        // insert a clickwait-or-newpage
//			                        addStringBuffer('\\');
//			                        addStringBuffer('@');
//			                        if (textgosub_flag) break;
//			                        if (rgosub_flag){
//			                            rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
//			                            rgosub_wait_pos[num_rgosub_waits++] = buf;
//			                        }
//			                    }
//			                    ignore_click_flag = clickstr_flag = false;
//			                }
//			                else{ //!in_1byte_mode
//			                    if (in_pretext_tag){
//			                        addStringBuffer( ch );
//			                        ch = *++buf;
//			                        continue;
//			                    }
//			                    else if ((ch == '@') || (ch == '\\')) {
//			                        if (!ignore_click_flag){
//			                            addStringBuffer( ch );
//			                            if (rgosub_flag){
//			                                rgosub_wait_1byte[num_rgosub_waits] = in_1byte_mode;
//			                                rgosub_wait_pos[num_rgosub_waits++] = buf+1;
//			                            }
//			                        }
//			                        if (textgosub_flag){
//			                            ++buf;
//			                            // if "ignore-textgosub-newline", ignore
//			                            // newline after clickwait if textgosub
//			                            // (fixes older onscripter-en games)
//			                            char *tmp = buf;
//			                            SKIP_SPACE(tmp);
//			                            if (ignore_textgosub_newline &&(*tmp == 0x0a))
//			                                buf = tmp+1;
//			                            break;
//			                        }
//			                        ch = *++buf;
//			                        continue;
//			                    }
//			                    if (ch == '%' || ch == '?'){
//			                        addIntVariable(&buf);
//			                    }
//			                    else if (ch == '$'){
//			                        addStrVariable(&buf);
//			                    }
//			                    else if (ch == '<'){
//			                        addStringBuffer(TXTBTN_START);
//			                        ch = *++buf;
//			                    }
//			                    else if (ch == '>'){
//			                        addStringBuffer(TXTBTN_END);
//			                        ch = *++buf;
//			                    }
//			                    else if (ch == '{') {
//			                        // comma list of var/val pairs
//			                        buf++;
//			                        pushCurrent(buf);
//			                        next_script = buf;
//			                        TmpVariableDataLink *tmp = &tmp_variable_data_link;
//			                        while (tmp->next != NULL)
//			                            tmp = tmp->next;
//			                        while( *buf != '}' ) {
//			                        
//			                            readVariable();
//			                            pushVariable();
//			                            tmp->next = new TmpVariableDataLink;
//			                            tmp = tmp->next;
//			                            tmp->vi.var_no = pushed_variable.var_no;
//			                            tmp->vi.type = pushed_variable.type;
//			                            VariableData &vd = getVariableData(tmp->vi.var_no);
//			                            tmp->num = vd.num;
//			                            //printf("variable: $%d\n", pushed_variable.var_no);
//			                            buf = next_script;
//			
//			                            if ( tmp->vi.type & VAR_INT ) {
//			                                tmp->num = parseIntExpression(&buf);
//			                                //printf("int: %d\n", x);
//			                            } else if ( tmp->vi.type & VAR_STR ) {
//			                                bool invar_1byte_mode = false;
//			                                int tmp_count = 0;
//			                                strcpy(saved_string_buffer, "");
//			                                while (*buf != 0x0a && *buf != '\0' &&
//			                                       (invar_1byte_mode || ((*buf != ',') && (*buf != '}')))) {
//			                                    if (*buf == '`')
//			                                        invar_1byte_mode = !invar_1byte_mode;
//			                                    if ((tmp_count+1 >= STRING_BUFFER_LENGTH) ||
//			                                        (IS_TWO_BYTE(*buf) && (tmp_count+2 >= STRING_BUFFER_LENGTH)))
//			                                        errorAndExit("readToken: var string length exceeds 2048 bytes.");
//			                                    else if (IS_TWO_BYTE(*buf)) {
//			                                        saved_string_buffer[tmp_count++] = *buf++;
//			                                        saved_string_buffer[tmp_count++] = *buf++;
//			                                    } else if ((*buf == '\\') || (*buf == BACKSLASH)) {
//			                                        //Mion: I really shouldn't be modifying
//			                                        //  the script buffer FIXME
//			                                        saved_string_buffer[tmp_count++] = '\\';
//			                                        *buf++ = BACKSLASH;
//			                                    } else
//			                                        saved_string_buffer[tmp_count++] = *buf++;
//			                                    saved_string_buffer[tmp_count] = '\0';
//			                                }
//			                                setStr( &tmp->str, saved_string_buffer );
//			                                //printf("string: %s\n", saved_string_buffer);
//			                            }
//			                            next_script = checkComma(buf);
//			                            buf = next_script;
//			                            if (!(getEndStatus() & END_COMMA)) break;
//			                        }
//			                        end_status = END_NONE;
//			                        current_variable.type = VAR_NONE;
//			                        popCurrent();
//			                        if (*buf == '}')
//			                            buf++;
//			                    }
//			                    else{
//			                        if (ch == '_')
//			                            ignore_click_flag = true;
//			                        else
//			                            ignore_click_flag = false;
//			                        addStringBuffer( ch );
//			                        buf++;
//			                        if (*buf == ' ') {
//			                            addStringBuffer( *buf );
//			                            buf++;
//			                        }
//			                    }
//			                    ch = *buf;
//			                    if (ch == 0x0a || ch == '\0') break;
//			                }
//			            }
//			        }
//			        //now process any {} tmp variables
//			        TmpVariableDataLink *tmp = tmp_variable_data_link.next;
//			        while (tmp != NULL) {
//			            if ( tmp->vi.type & VAR_INT )
//			                setInt( &tmp->vi, tmp->num);
//			            else if ( tmp->vi.type & VAR_STR )
//			                setStr( &variable_data[ tmp->vi.var_no ].str, tmp->str );
//			            TmpVariableDataLink *last = tmp;
//			            tmp = tmp->next;
//			            delete last;
//			        }
//			        tmp_variable_data_link.next = NULL;
//			        text_flag = true;
//			    }
//			    else if ((ch >= 'a' && ch <= 'z') ||
//			             (ch >= 'A' && ch <= 'Z') ||
//			             ch == '_'){ // command
//			        do{
//			            if (ch >= 'A' && ch <= 'Z') ch += 'a' - 'A';
//			            addStringBuffer( ch );
//			            ch = *++buf;
//			        }
//			        while((ch >= 'a' && ch <= 'z') ||
//			              (ch >= 'A' && ch <= 'Z') ||
//			              (ch >= '0' && ch <= '9') ||
//			              ch == '_');
//			    }
//			    else if (ch == '*'){ // label
//			        return readLabel();
//			    }
//			    else if ((ch == 0x0a) && !is_rgosub_click){
//			        addStringBuffer( ch );
//			        markAsKidoku( buf++ );
//			    }
//			    else if (ch == '~' || ch == ':'){
//			        addStringBuffer( ch );
//			        markAsKidoku( buf++ );
//			    }
//			    else if (ch != '\0'){
//			        fprintf(stderr, "readToken: skip unknown heading character %c (%x)\n", ch, ch);
//			        buf++;
//			        goto readTokenTop;
//			    }
//			    is_rgosub_click = false;
//			    next_script = checkComma(buf);
//			
//			    //printf("readToken [%s] len=%d [%c(%x)] %p\n", string_buffer, strlen(string_buffer), ch, ch, next_script);
//			
//			    return string_buffer;
			}
			
			public CharPtr readName()
			{
				return null;
//			    // bare word - not a string variable
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			
//			    current_script = next_script;
//			    SKIP_SPACE( current_script );
//			    char *buf = current_script;
//			
//			    string_counter = 0;
//			    char ch = *buf;
//			    if ( ((ch >= 'a') && (ch <= 'z')) ||
//			         ((ch >= 'A') && (ch <= 'Z')) ||
//			         (ch == '_') ){
//			        if ( (ch >= 'A') && (ch <= 'Z') )
//			            ch += 'a' - 'A';
//			        addStringBuffer( ch );
//			        ch = *(++buf);
//			        while( ((ch >= 'a') && (ch <= 'z')) ||
//			               ((ch >= 'A') && (ch <= 'Z')) ||
//			               ((ch >= '0') && (ch <= '9')) ||
//			               (ch == '_') ){
//			            if ( (ch >= 'A') && (ch <= 'Z') )
//			                ch += 'a' - 'A';
//			            addStringBuffer( ch );
//			            ch = *(++buf);
//			        }
//			    }
//			    addStringBuffer( '\0' );
//			
//			    next_script = checkComma(buf);
//			
//			    return string_buffer;
			}
			
			//FIXME:added is_not_null_is_color
			public CharPtr readColor(ref bool is_color, bool is_not_null_is_color)
			{
				return null;
//			    // bare color type - not a string variable
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			
//			    current_script = next_script;
//			    SKIP_SPACE( current_script );
//			    char *buf = current_script;
//			
//			    string_counter = 0;
//			    addStringBuffer( '#' );
//			    char ch = *(++buf);
//			    int i;
//			    for (i=0; i<7; i++) {
//			        if ( ((ch >= '0') && (ch <= '9')) ||
//			             ((ch >= 'a') && (ch <= 'f')) ||
//			             ((ch >= 'A') && (ch <= 'F')) ) {
//			            addStringBuffer( ch );
//			            ch = *(++buf);
//			        } else
//			            break;
//			    }
//			    if (i!=6) {
//			        if (is_color != NULL) {
//			            *is_color = false;
//			            string_counter = 0;
//			            addStringBuffer( '\0' );
//			            return string_buffer;
//			        } else {
//			            strncpy(string_buffer, current_script, 16);
//			            string_buffer[16] = '\0';
//			            errorAndExit( "readColor: not a valid color type." );
//			        }
//			    }
//			    addStringBuffer( '\0' );
//			    next_script = checkComma(buf);
//			    if (is_color != NULL)
//			        *is_color = true;
//			
//			    return string_buffer;
			}
			
			public CharPtr readLabel()
			{
				return null;
//			    // *NAME, "*NAME", or $VAR that equals "*NAME"
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			
//			    current_script = next_script;
//			    SKIP_SPACE( current_script );
//			    char *buf = current_script;
//			    char *tmp = NULL;
//			
//			    string_counter = 0;
//			    char ch = *buf;
//			    if ((ch == '$') || (ch == '"') || (ch == '`')){
//			        parseStr(&buf);
//			        tmp = buf;
//			        string_counter = 0;
//			        buf = str_string_buffer;
//			        SKIP_SPACE(buf);
//			        ch = *buf;
//			    }
//			    if (ch == '*') {
//			        while (ch == '*'){
//			            addStringBuffer( ch );
//			            ch = *(++buf);
//			        }
//			        SKIP_SPACE(buf);
//			
//			        ch = *buf;
//			        while( ((ch >= 'a') && (ch <= 'z')) ||
//			               ((ch >= 'A') && (ch <= 'Z')) ||
//			               ((ch >= '0') && (ch <= '9')) ||
//			               (ch == '_') ){
//			            if ( (ch >= 'A') && (ch <= 'Z') )
//			                ch += 'a' - 'A';
//			            addStringBuffer( ch );
//			            ch = *(++buf);
//			        }
//			    }
//			    addStringBuffer( '\0' );
//			    if ( (string_buffer[0] == '\0') || (string_buffer[1] == '\0') ){
//			        buf = current_script;
//			        while (*buf && (*buf != 0x0a))
//			            ++buf;
//			        ch = *buf;
//			        *buf = '\0';
//			        if (tmp != NULL) {
//			            snprintf(errbuf, MAX_ERRBUF_LEN, 
//			                     "Invalid label specification '%s' ('%s')",
//			                     current_script, str_string_buffer);
//			            *buf = ch;
//			            errorAndExit(errbuf);
//			        } else {
//			            snprintf(errbuf, MAX_ERRBUF_LEN,
//			                     "Invalid label specification '%s'", current_script);
//			            *buf = ch;
//			            errorAndExit(errbuf);
//			        }
//			    }
//			    if (tmp != NULL)
//			        buf = tmp;
//			
//			    next_script = checkComma(buf);
//			
//			    return string_buffer;
			}
			
			public CharPtr readStr()
			{
				return null;
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			
//			    current_script = next_script;
//			    SKIP_SPACE( current_script );
//			    char *buf = current_script;
//			
//			    string_buffer[0] = '\0';
//			    string_counter = 0;
//			
//			    while(1){
//			        parseStr(&buf);
//			        buf = checkComma(buf);
//			        string_counter += strlen(str_string_buffer);
//			        if (string_counter+1 >= STRING_BUFFER_LENGTH)
//			            errorAndExit("readStr: string length exceeds 2048 bytes.");
//			        strcat(string_buffer, str_string_buffer);
//			        if (buf[0] != '+') break;
//			        buf++;
//			    }
//			    next_script = buf;
//			
//			    return string_buffer;
			}
			
			public int readInt()
			{
				return 0;
//			    string_counter = 0;
//			    string_buffer[string_counter] = '\0';
//			
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			
//			    current_script = next_script;
//			    SKIP_SPACE( current_script );
//			    char *buf = current_script;
//			
//			    int ret = parseIntExpression(&buf);
//			
//			    next_script = checkComma(buf);
//			
//			    return ret;
			}
			
			public void skipToken()
			{
//			    SKIP_SPACE( current_script );
//			    char *buf = current_script;
//			
//			    bool quat_flag = false;
//			    bool text_flag = false;
//			    while(1){
//			        if ( *buf == 0x0a ||
//			             (!quat_flag && !text_flag && (*buf == ':' || *buf == ';') ) ) break;
//			        if ( *buf == '"' ) quat_flag = !quat_flag;
//			        if ( IS_TWO_BYTE(*buf) ){
//			            buf += 2;
//			            if ( !quat_flag ) text_flag = true;
//			        }
//			        else
//			            buf++;
//			    }
//			    if (text_flag && *buf == 0x0a) buf++;
//			    
//			    next_script = buf;
			}
			
			// string access function
			public CharPtr saveStringBuffer()
			{
				return null;
//			    strcpy( saved_string_buffer, string_buffer );
//			    return saved_string_buffer;
			}
			
			// script address direct manipulation function
			public void setCurrent(CharPtr pos)
			{
//			    current_script = next_script = pos;
			}
			
			public void pushCurrent( CharPtr pos )
			{
//			    pushed_current_script = current_script;
//			    pushed_next_script = next_script;
//			
//			    current_script = pos;
//			    next_script = pos;
			}
			
			public void popCurrent()
			{
//			    current_script = pushed_current_script;
//			    next_script = pushed_next_script;
			}
			
			public void enterExternalScript(CharPtr pos)
			{
//			    internal_current_script = current_script;
//			    current_script = pos;
//			    internal_next_script = next_script;
//			    next_script = pos;
//			    internal_end_status = end_status;
//			    internal_current_variable = current_variable;
//			    internal_pushed_variable = pushed_variable;
			}
			
			public void leaveExternalScript()
			{
//			    current_script = internal_current_script;
//			    internal_current_script = NULL;
//			    next_script = internal_next_script;
//			    end_status = internal_end_status;
//			    current_variable = internal_current_variable;
//			    pushed_variable = internal_pushed_variable;
			}
			
			public bool isExternalScript()
			{
				return false;
//			    return (internal_current_script != NULL);
			}
			
			public int getOffset( CharPtr pos )
			{
				return 0;
//			    return pos - script_buffer;
			}
			
			public CharPtr getAddress( int offset )
			{
				return null;
//			    return script_buffer + offset;
			}
			
			public int getLineByAddress( CharPtr address )
			{
				return 0;
//			    LabelInfo label = getLabelByAddress( address );
//			
//			    char *addr = label.label_header;
//			    int line = 0;
//			    while ( address > addr ){
//			        if ( *addr == 0x0a ) line++;
//			        addr++;
//			    }
//			    return line;
			}
			
			public CharPtr getAddressByLine( int line )
			{
				return null;
//			    LabelInfo label = getLabelByLine( line );
//			
//			    int l = line - label.start_line;
//			    char *addr = label.label_header;
//			    while ( l > 0 ){
//			        while( *addr != 0x0a ) addr++;
//			        addr++;
//			        l--;
//			    }
//			    return addr;
			}
			
			public LabelInfo getLabelByAddress( CharPtr address )
			{
				return null;
//			    int i;
//			    for ( i=0 ; i<num_of_labels-1 ; i++ ){
//			        if ( label_info[i+1].start_address > address )
//			            return label_info[i];
//			    }
//			    return label_info[i];
			}
			
			public LabelInfo getLabelByLine( int line )
			{
				return null;
//			    int i;
//			    for ( i=0 ; i<num_of_labels-1 ; i++ ){
//			        if ( label_info[i+1].start_line > line )
//			            return label_info[i];
//			    }
//			    return label_info[i];
			}
			
			public bool isName( CharPtr name )
			{
				return false;
//			    return (strncmp( name, string_buffer, strlen(name) )==0)?true:false;
			}
			
			public bool isText()
			{
				return false;
//			    return text_flag;
			}
			
			public bool compareString(CharPtr buf)
			{
				return false;
//			    SKIP_SPACE(next_script);
//			    unsigned int i, num = strlen(buf);
//			    for (i=0 ; i<num ; i++){
//			        char ch = next_script[i];
//			        if ('A' <= ch && 'Z' >= ch) ch += 'a' - 'A';
//			        if (ch != buf[i]) break;
//			    }
//			    return (i==num)?true:false;
			}
			
			public void skipLine( int no )
			{
//			    for ( int i=0 ; i<no ; i++ ){
//			        while ( *current_script != 0x0a ) current_script++;
//			        current_script++;
//			    }
//			    next_script = current_script;
			}
			
			public void setLinepage( bool val )
			{
//			    linepage_flag = val;
			}
			
			// function for kidoku history
			public bool isKidoku()
			{
				return false;
//			    return skip_enabled;
			}
			
			public void markAsKidoku( CharPtr address )
			{
//			    if (!kidokuskip_flag || internal_current_script != NULL) return;
//			
//			    int offset = current_script - script_buffer;
//			    if ( address ) offset = address - script_buffer;
//			    //printf("mark (%c)%x:%x = %d\n", *current_script, offset /8, offset%8, kidoku_buffer[ offset/8 ] & ((char)1 << (offset % 8)));
//			    if ( kidoku_buffer[ offset/8 ] & ((char)1 << (offset % 8)) )
//			        skip_enabled = true;
//			    else
//			        skip_enabled = false;
//			    kidoku_buffer[ offset/8 ] |= ((char)1 << (offset % 8));
			}
			
			public void setKidokuskip( bool kidokuskip_flag )
			{
//			    this->kidokuskip_flag = kidokuskip_flag;
			}
			
			public void saveKidokuData()
			{
//			    FILE *fp;
//			
//			    if ( ( fp = fopen( "kidoku.dat", "wb", true, true ) ) == NULL ){
//			        fprintf( stderr, "can't write kidoku.dat\n" );
//			        return;
//			    }
//			
//			    if ( fwrite( kidoku_buffer, 1, script_buffer_length/8, fp ) !=
//			         size_t(script_buffer_length/8) )
//			        fprintf( stderr, "Warning: failed to write to kidoku.dat\n" );
//			    fclose( fp );
			}
			
			public void loadKidokuData()
			{
//			    FILE *fp;
//			
//			    setKidokuskip( true );
//			    kidoku_buffer = new char[ script_buffer_length/8 + 1 ];
//			    memset( kidoku_buffer, 0, script_buffer_length/8 + 1 );
//			
//			    if ( ( fp = fopen( "kidoku.dat", "rb", true, true ) ) != NULL ){
//			        if (fread( kidoku_buffer, 1, script_buffer_length/8, fp ) !=
//			            size_t(script_buffer_length/8)) {
//			            if (ferror(fp))
//			                fputs("Warning: failed to read kidoku.dat\n", stderr);
//			        }
//			        fclose( fp );
//			    }
			}
			
			public void addIntVariable(CharPtr[] buf, bool no_zenkaku)
			{
//			    char num_buf[20];
//			    int no = parseInt(buf);
//			
//			    int len = getStringFromInteger( num_buf, no, -1, false, !no_zenkaku );
//			    for (int i=0 ; i<len ; i++)
//			        addStringBuffer( num_buf[i] );
			}
			
			public void addStrVariable(CharPtr[] buf)
			{
//			    (*buf)++;
//			    int no = parseInt(buf);
//			    VariableData &vd = getVariableData(no);
//			    if ( vd.str ){
//			        for (unsigned int i=0 ; i<strlen( vd.str ) ; i++){
//			            addStringBuffer( vd.str[i] );
//			        }
//			    }
			}
			
			public void enableTextgosub(bool val)
			{
//			    textgosub_flag = val;
			}
			
			public void enableRgosub(bool val)
			{
//			    rgosub_flag = val;
//			
//			    if (rgosub_flag && !rgosub_wait_pos){
//			        total_rgosub_wait_size = 4;
//			        rgosub_wait_pos = new char*[total_rgosub_wait_size];
//			        rgosub_wait_1byte = new bool[total_rgosub_wait_size];
//			    }
			}
			
			public void setClickstr(CharPtr list)
			{
//			    if (clickstr_list) delete[] clickstr_list;
//			    clickstr_list = new char[strlen(list)+2];
//			    memcpy( clickstr_list, list, strlen(list)+1 );
//			    clickstr_list[strlen(list)+1] = '\0';
			}
			
			public int checkClickstr(CharPtr buf, bool recursive_flag)
			{
				return 0;
//			    if ((buf[0] == '\\') && (buf[1] == '@')) return -2;  //clickwait-or-page
//			    if ((buf[0] == '@') || (buf[0] == '\\')) return -1;
//			
//			    if (clickstr_list == NULL) return 0;
//			    bool only_double_byte_check = true;
//			    char *click_buf = clickstr_list;
//			    while(click_buf[0]){
//			        if (click_buf[0] == '`'){
//			            click_buf++;
//			            only_double_byte_check = false;
//			            continue;
//			        }
//			        if (! only_double_byte_check){
//			            if (!IS_TWO_BYTE(click_buf[0]) && !IS_TWO_BYTE(buf[0]) 
//			                && (click_buf[0] == buf[0])){
//			                if (!recursive_flag && checkClickstr(buf+1, true) != 0) return 0;
//			                return 1;
//			            }
//			        }
//			        if (IS_TWO_BYTE(click_buf[0]) && IS_TWO_BYTE(buf[0]) &&
//			            (click_buf[0] == buf[0]) && (click_buf[1] == buf[1])){
//			            if (!recursive_flag && checkClickstr(buf+2, true) != 0) return 0;
//			            return 2;
//			        }
//			        if (IS_TWO_BYTE(click_buf[0])) click_buf++;
//			        click_buf++;
//			    }
//			
//			    return 0;
			}
			
			public int getIntVariable( VariableInfo var_info )
			{
				return 0;
//			    if ( var_info == NULL ) var_info = &current_variable;
//			
//			    if ( var_info->type == VAR_INT )
//			        return getVariableData(var_info->var_no).num;
//			    else if ( var_info->type == VAR_ARRAY )
//			        return *getArrayPtr( var_info->var_no, var_info->array, 0 );
//			    return 0;
			}
			
			public void readVariable( bool reread_flag = false)
			{
//			    end_status = END_NONE;
//			    current_variable.type = VAR_NONE;
//			    if ( reread_flag ) next_script = current_script;
//			    current_script = next_script;
//			    char *buf = current_script;
//			
//			    SKIP_SPACE(buf);
//			
//			    bool ptr_flag = false;
//			    if ( *buf == 'i' || *buf == 's' ){
//			        ptr_flag = true;
//			        buf++;
//			    }
//			
//			    if ( *buf == '%' ){
//			        buf++;
//			        current_variable.var_no = parseInt(&buf);
//			        current_variable.type = VAR_INT;
//			    }
//			    else if ( *buf == '?' ){
//			        ArrayVariable av;
//			        current_variable.var_no = parseArray( &buf, av );
//			        current_variable.type = VAR_ARRAY;
//			        current_variable.array = av;
//			    }
//			    else if ( *buf == '$' ){
//			        buf++;
//			        current_variable.var_no = parseInt(&buf);
//			        current_variable.type = VAR_STR;
//			    }
//			
//			    if (ptr_flag) current_variable.type |= VAR_PTR;
//			
//			    next_script = checkComma(buf);
			}
			
			public void setInt( VariableInfo var_info, int val, int offset = 0 )
			{
//			    if ( var_info->type & VAR_INT ){
//			        setNumVariable( var_info->var_no + offset, val );
//			    }
//			    else if ( var_info->type & VAR_ARRAY ){
//			        *getArrayPtr( var_info->var_no, var_info->array, offset ) = val;
//			    }
//			    else{
//			        errorAndExit( "setInt: no integer variable." );
//			    }
			}
			
			public void setStr( CharPtr[] dst, CharPtr src, int num )
			{
//			    if ( *dst ) delete[] *dst;
//			    *dst = NULL;
//			    
//			    if ( src ){
//			        if (num >= 0){
//			            *dst = new char[ num + 1 ];
//			            memcpy( *dst, src, num );
//			            (*dst)[num] = '\0';
//			        }
//			        else{
//			            *dst = new char[ strlen( src ) + 1];
//			            strcpy( *dst, src );
//			        }
//			    }
			}
			
			public void pushVariable()
			{
//			    pushed_variable = current_variable;
			}
			
			public void setNumVariable( int no, int val )
			{
//			    VariableData &vd = getVariableData(no);
//			    if ( vd.num_limit_flag ){
//			        if ( val < vd.num_limit_lower )
//			            val = vd.num_limit_lower;
//			        else if ( val > vd.num_limit_upper )
//			            val = vd.num_limit_upper;
//			    }
//			    vd.num = val;
			}
			
			public int getStringFromInteger( CharPtr buffer, int no, int num_column,
			                                         bool is_zero_inserted,
			                                         bool use_zenkaku )
			{
				return 0;
//			    int i, num_space=0, num_minus = 0;
//			    if (no < 0){
//			        num_minus = 1;
//			        no = -no;
//			    }
//			    int num_digit=1, no2 = no;
//			    while(no2 >= 10){
//			        no2 /= 10;
//			        num_digit++;
//			    }
//			
//			    if (num_column < 0) num_column = num_digit+num_minus;
//			    if (num_digit+num_minus <= num_column)
//			        num_space = num_column - (num_digit+num_minus);
//			    else{
//			        for (i=0 ; i<num_digit+num_minus-num_column ; i++)
//			            no /= 10;
//			        num_digit -= num_digit+num_minus-num_column;
//			    }
//			
//			    if (!use_zenkaku) {
//			        if (num_minus == 1) no = -no;
//			        char format[6];
//			        if (is_zero_inserted)
//			            sprintf(format, "%%0%dd", num_column);
//			        else
//			            sprintf(format, "%%%dd", num_column);
//			        
//			        sprintf(buffer, format, no);
//			        return num_column;
//			    }
//			
//			    int c = 0;
//			    if (is_zero_inserted){
//			        for (i=0 ; i<num_space ; i++){
//			            buffer[c++] = ((char*)"侽")[0];
//			            buffer[c++] = ((char*)"侽")[1];
//			        }
//			    }
//			    else{
//			        for (i=0 ; i<num_space ; i++){
//			            buffer[c++] = ((char*)"丂")[0];
//			            buffer[c++] = ((char*)"丂")[1];
//			        }
//			    }
//			    if (num_minus == 1){
//			        buffer[c++] = "亅"[0];
//			        buffer[c++] = "亅"[1];
//			    }
//			    c = (num_column-1)*2;
//			    char num_str[] = "侽侾俀俁係俆俇俈俉俋";
//			    for (i=0 ; i<num_digit ; i++){
//			        buffer[c]   = num_str[ no % 10 * 2];
//			        buffer[c+1] = num_str[ no % 10 * 2 + 1];
//			        no /= 10;
//			        c -= 2;
//			    }
//			    buffer[num_column*2] = '\0';
//			
//			    return num_column*2;
			}
			
			public int readScriptSub( FILEPtr fp, CharPtr[] buf, int encrypt_mode )
			{
				return 0;
//			    unsigned char magic[5] = {0x79, 0x57, 0x0d, 0x80, 0x04 };
//			    int  magic_counter = 0;
//			    bool newline_flag = true;
//			    bool cr_flag = false;
//			
//			    if (encrypt_mode == 3 && !key_table_flag)
//			        simpleErrorAndExit("readScriptSub: the EXE file must be specified with --key-exe option.");
//			
//			    size_t len=0, count=0;
//			    while(1){
//			        if (len == count){
//			            len = fread(tmp_script_buf, 1, TMP_SCRIPT_BUF_LEN, fp);
//			            if (len == 0){
//			                if (cr_flag) *(*buf)++ = 0x0a;
//			                break;
//			            }
//			            count = 0;
//			        }
//			        unsigned char ch = tmp_script_buf[count++];
//			        if      ( encrypt_mode == 1 ) ch ^= 0x84;
//			        else if ( encrypt_mode == 2 ){
//			            ch = (ch ^ magic[magic_counter++]) & 0xff;
//			            if ( magic_counter == 5 ) magic_counter = 0;
//			        }
//			        else if ( encrypt_mode == 3){
//			            ch = key_table[(unsigned char)ch] ^ 0x84;
//			        }
//			
//			        if ( cr_flag && ch != 0x0a ){
//			            *(*buf)++ = 0x0a;
//			            newline_flag = true;
//			            cr_flag = false;
//			        }
//			
//			        if ( ch == '*' && newline_flag ) num_of_labels++;
//			        if ( ch == 0x0d ){
//			            cr_flag = true;
//			            continue;
//			        }
//			        if ( ch == 0x0a ){
//			            *(*buf)++ = 0x0a;
//			            newline_flag = true;
//			            cr_flag = false;
//			        }
//			        else{
//			            *(*buf)++ = ch;
//			            if ( ch != ' ' && ch != '\t' )
//			                newline_flag = false;
//			        }
//			    }
//			
//			    *(*buf)++ = 0x0a;
//			    return 0;
			}
			
			public int readScript( DirPaths path )
			{
				return 0;
//			    archive_path = &path;
//			
//			    FILE *fp = NULL;
//			    char filename[10];
//			    int i, n=0, encrypt_mode = 0;
//			    while ((fp == NULL) && (n<archive_path->get_num_paths())) {
//			        const char *curpath = archive_path->get_path(n);
//			        const char *filename = "";
//			        
//			        if ((fp = fopen(curpath, "0.txt", "rb")) != NULL){
//			            encrypt_mode = 0;
//			            filename = "0.txt";
//			        }
//			        else if ((fp = fopen(curpath, "00.txt", "rb")) != NULL){
//			            encrypt_mode = 0;
//			            filename = "00.txt";
//			        }
//			        else if ((fp = fopen(curpath, "nscr_sec.dat", "rb")) != NULL){
//			            encrypt_mode = 2;
//			            filename = "nscr_sec.dat";
//			        }
//			        else if ((fp = fopen(curpath, "nscript.___", "rb")) != NULL){
//			            encrypt_mode = 3;
//			            filename = "nscript.___";
//			        }
//			        else if ((fp = fopen(curpath, "nscript.dat", "rb")) != NULL){
//			            encrypt_mode = 1;
//			            filename = "nscript.dat";
//			        }
//			
//			        if (fp != NULL) {
//			            fprintf(stderr, "Script found: %s%s\n", curpath, filename);
//			            setStr(&script_path, curpath);
//			        }
//			        n++;
//			    }
//			    if (fp == NULL){
//			#if defined(MACOSX) 
//			        simpleErrorAndExit("No game data found.\nThis application must be run "
//			                           "from a directory containing ONScripter game data.",
//			                           "can't open any of 0.txt, 00.txt, or nscript.dat",
//			                           "Missing game data");
//			#else
//			        simpleErrorAndExit("No game script found.",
//			                           "can't open any of 0.txt, 00.txt, or nscript.dat",
//			                           "Missing game data");
//			#endif
//			        return -1;
//			    }
//			
//			    fseek( fp, 0, SEEK_END );
//			    int estimated_buffer_length = ftell( fp ) + 1;
//			
//			    if (encrypt_mode == 0){
//			        fclose(fp);
//			        for (i=1 ; i<100 ; i++){
//			            sprintf(filename, "%d.txt", i);
//			            if ((fp = fopen(script_path, filename, "rb")) == NULL){
//			                sprintf(filename, "%02d.txt", i);
//			                fp = fopen(script_path, filename, "rb");
//			            }
//			            if (fp){
//			                fseek( fp, 0, SEEK_END );
//			                estimated_buffer_length += ftell(fp)+1;
//			                fclose(fp);
//			            }
//			        }
//			    }
//			
//			    if ( script_buffer ) delete[] script_buffer;
//			    script_buffer = new char[ estimated_buffer_length ];
//			
//			    char *p_script_buffer;
//			    current_script = p_script_buffer = script_buffer;
//			
//			    tmp_script_buf = new char[TMP_SCRIPT_BUF_LEN];
//			    if (encrypt_mode > 0){
//			        fseek( fp, 0, SEEK_SET );
//			        readScriptSub( fp, &p_script_buffer, encrypt_mode );
//			        fclose( fp );
//			    }
//			    else{
//			        for (i=0 ; i<100 ; i++){
//			            sprintf(filename, "%d.txt", i);
//			            if ((fp = fopen(script_path, filename, "rb")) == NULL){
//			                sprintf(filename, "%02d.txt", i);
//			                fp = fopen(script_path, filename, "rb");
//			            }
//			            if (fp){
//			                readScriptSub( fp, &p_script_buffer, 0 );
//			                fclose(fp);
//			            }
//			        }
//			    }
//			    delete[] tmp_script_buf;
//			
//			    // Haeleth: Search for gameid file (this overrides any builtin
//			    // ;gameid directive, or serves its purpose if none is available)
//			    if (!game_identifier) { //Mion: only if gameid not already set
//			        fp = fopen(script_path, "game.id", "rb"); //Mion: search only the script path
//			        if (fp) {
//			            size_t line_size = 0;
//			            char c;
//			            do {
//			                c = fgetc(fp);
//			                ++line_size;
//			            } while (c != '\r' && c != '\n' && c != EOF);
//			            fseek(fp, 0, SEEK_SET);
//			            game_identifier = new char[line_size];
//			            if (fgets(game_identifier, line_size, fp) == NULL)
//			                fputs("Warning: couldn't read game ID from game.id\n", stderr);
//			            fclose(fp);
//			        }
//			    }
//			
//			    script_buffer_length = p_script_buffer - script_buffer;
//			    game_hash = script_buffer_length;  // Reasonable "hash" value
//			
//			    /* ---------------------------------------- */
//			    /* screen size and value check */
//			    char *buf = script_buffer+1;
//			    while( script_buffer[0] == ';' ){
//			        if ( !strncmp( buf, "mode", 4 ) ){
//			            buf += 4;
//			            if      ( !strncmp( buf, "800", 3 ) )
//			                screen_size = SCREEN_SIZE_800x600;
//			            else if ( !strncmp( buf, "400", 3 ) )
//			                screen_size = SCREEN_SIZE_400x300;
//			            else if ( !strncmp( buf, "320", 3 ) )
//			                screen_size = SCREEN_SIZE_320x240;
//			            else
//			                screen_size = SCREEN_SIZE_640x480;
//			            buf += 3;
//			        }
//			        else if ( !strncmp( buf, "value", 5 ) ){
//			            buf += 5;
//			            SKIP_SPACE(buf);
//			            global_variable_border = 0;
//			            while ( *buf >= '0' && *buf <= '9' )
//			                global_variable_border = global_variable_border * 10 + *buf++ - '0';
//			            //printf("set global_variable_border: %d\n", global_variable_border);
//			        }
//			        else{
//			            break;
//			        }
//			        if ( *buf != ',' ){
//			        	while ( *buf++ != '\n' );
//			        	break;
//			        }
//			        buf++;
//			    }
//			    if ( *buf++ == ';' && !game_identifier ){
//			    	while (*buf == ' ' || *buf == '\t') ++buf;
//			    	if ( !strncmp( buf, "gameid ", 7 ) ){
//			    		buf += 7;
//			    		int i = 0;
//			    		while ( buf[i++] != '\n' );
//			    		game_identifier = new char[i];
//			    		strncpy( game_identifier, buf, i - 1 );
//			    		game_identifier[i - 1] = 0;
//			    	}
//			    }
//			
//			    return labelScript();
			}
			
			public int labelScript()
			{
				return 0;
//			    int label_counter = -1;
//			    int current_line = 0;
//			    char *buf = script_buffer;
//			    label_info = new LabelInfo[ num_of_labels+1 ];
//			
//			    while ( buf < script_buffer + script_buffer_length ){
//			        SKIP_SPACE( buf );
//			        if ( *buf == '*' ){
//			            setCurrent( buf );
//			            readLabel();
//			            label_info[ ++label_counter ].name = new char[ strlen(string_buffer) ];
//			            strcpy( label_info[ label_counter ].name, string_buffer+1 );
//			            label_info[ label_counter ].label_header = buf;
//			            label_info[ label_counter ].num_of_lines = 1;
//			            label_info[ label_counter ].start_line   = current_line;
//			            buf = getNext();
//			            if ( *buf == 0x0a ){
//			                buf++;
//			                SKIP_SPACE(buf);
//			                current_line++;
//			            }
//			            label_info[ label_counter ].start_address = buf;
//			        }
//			        else{
//			            if ( label_counter >= 0 )
//			                label_info[ label_counter ].num_of_lines++;
//			            while( *buf != 0x0a ) buf++;
//			            buf++;
//			            current_line++;
//			        }
//			    }
//			
//			    label_info[num_of_labels].start_address = NULL;
//			
//			    return 0;
			}
			
			public LabelInfo lookupLabel( CharPtr label )
			{
				return null;
//			    int i = findLabel( label );
//			
//			    findAndAddLog( log_info[LABEL_LOG], label_info[i].name, true );
//			    return label_info[i];
			}
			
			public LabelInfo lookupLabelNext( CharPtr label )
			{
				return null;
//			    int i = findLabel( label );
//			    if ( i+1 < num_of_labels ){
//			        findAndAddLog( log_info[LABEL_LOG], label_info[i+1].name, true );
//			        return label_info[i+1];
//			    }
//			
//			    return label_info[num_of_labels];
			}
			
			public LogLink findAndAddLog( LogInfo info, CharPtr name, bool add_flag )
			{
				return null;
//			    char capital_name[256];
//			    for ( unsigned int i=0 ; i<strlen(name)+1 ; i++ ){
//			        capital_name[i] = name[i];
//			        if ( 'a' <= capital_name[i] && capital_name[i] <= 'z' ) capital_name[i] += 'A' - 'a';
//			        else if ( capital_name[i] == '/' ) capital_name[i] = '\\';
//			    }
//			
//			    LogLink *cur = info.root_log.next;
//			    while( cur ){
//			        if ( !strcmp( cur->name, capital_name ) ) break;
//			        cur = cur->next;
//			    }
//			    if ( !add_flag || cur ) return cur;
//			
//			    LogLink *link = new LogLink();
//			    link->name = new char[strlen(capital_name)+1];
//			    strcpy( link->name, capital_name );
//			    info.current_log->next = link;
//			    info.current_log = info.current_log->next;
//			    info.num_logs++;
//			
//			    return link;
			}
			
			public void resetLog( LogInfo info )
			{
//			    LogLink *link = info.root_log.next;
//			    while( link ){
//			        LogLink *tmp = link;
//			        link = link->next;
//			        delete tmp;
//			    }
//			
//			    info.root_log.next = NULL;
//			    info.current_log = &info.root_log;
//			    info.num_logs = 0;
			}
			
			public ArrayVariable getRootArrayVariable(){
				return null;
//			    return root_array_variable;
			}
			
			public void addNumAlias( CharPtr str, int no )
			{
//			    Alias *p_num_alias = new Alias( str, no );
//			    last_num_alias->next = p_num_alias;
//			    last_num_alias = last_num_alias->next;
			}
			
			public void addStrAlias( CharPtr str1, CharPtr str2 )
			{
//			    Alias *p_str_alias = new Alias( str1, str2 );
//			    last_str_alias->next = p_str_alias;
//			    last_str_alias = last_str_alias->next;
			}
			
			public bool findNumAlias( CharPtr str, ref int value )
			{
				return false;
//			    Alias *p_num_alias = root_num_alias.next;
//			    while( p_num_alias ){
//				if ( !strcmp( p_num_alias->alias, str ) ){
//				    *value = p_num_alias->num;
//				    return true;
//				}
//				p_num_alias = p_num_alias->next;
//			    }
//			    return false;
			}
			
			public bool findStrAlias( CharPtr str, CharPtr buffer )
			{
				return false;
//			    Alias *p_str_alias = root_str_alias.next;
//			    while( p_str_alias ){
//				if ( !strcmp( p_str_alias->alias, str ) ){
//				    strcpy( buffer, p_str_alias->str );
//				    return true;
//				}
//				p_str_alias = p_str_alias->next;
//			    }
//			    return false;
			}
			
			public void processError( CharPtr str, CharPtr title, CharPtr detail, bool is_warning, bool is_simple )
			{
//			    //if not yet running the script, no line no/cmd - keep it simple
//			    if (script_buffer == NULL)
//			        is_simple = true;
//			
//			    if (title == NULL)
//			        title = "Error";
//			    const char *type = is_warning ? "Warning" : "Fatal";
//			
//			    if (is_simple) {
//			        fprintf(stderr, " ***[%s] %s: %s ***\n", type, title, str);
//			        if (detail)
//			            fprintf(stderr, "\t%s\n", detail);
//			
//			        if (is_warning && !strict_warnings) return;
//			
//			        if (!ons) exit(-1); //nothing left to do without doErrorBox
//			
//			        if (! ons->doErrorBox(title, str, true, is_warning))
//			            return;
//			
//			        if (is_warning)
//			            fprintf(stderr, " ***[Fatal] User terminated at warning ***\n");
//			
//			    } else {
//			
//			        char errhist[1024], errcmd[128];
//			
//			        LabelInfo label = getLabelByAddress(getCurrent());
//			        int lblinenum = getLineByAddress(getCurrent());
//			        int linenum = label.start_line + lblinenum + 1;
//			
//			        errcmd[0] = '\0';
//			        if (strlen(current_cmd) > 0) {
//			            if (current_cmd_type == CMD_BUILTIN)
//			                snprintf(errcmd, 128, ", cmd \"%s\"", current_cmd);
//			            else if (current_cmd_type == CMD_USERDEF)
//			                snprintf(errcmd, 128, ", user-defined cmd \"%s\"", current_cmd);
//			        }
//			        fprintf(stderr, " ***[%s] %s at line %d (*%s:%d)%s - %s ***\n",
//			                type, title, linenum, label.name, lblinenum, errcmd, str);
//			        if (detail)
//			            fprintf(stderr, "\t%s\n", detail);
//			        if (string_buffer && *string_buffer)
//			            fprintf(stderr, "\t(String buffer: [%s])\n", string_buffer);
//			
//			        if (is_warning && !strict_warnings) return;
//			
//			        if (!ons) exit(-1); //nothing left to do without doErrorBox
//			
//			        if (is_warning) {
//			            snprintf(errhist, 1024, "%s\nat line %d (*%s:%d)%s\n%s",
//			                     str, linenum, label.name, lblinenum, errcmd,
//			                     detail ? detail : "");
//			
//			            if(!ons->doErrorBox(title, errhist, false, true))
//			                return;
//			
//			            fprintf(stderr, " ***[Fatal] User terminated at warning ***\n");
//			        }
//			
//			        //Mion: grabbing the current line in the script & up to 2 previous ones,
//			        // in-place (replaces the newlines with '\0', and then puts the newlines
//			        // back when finished)
//			        int i;
//			        char *line[3], *tmp[4];
//			        for (i=0; i<4; i++) tmp[i] = NULL;
//			        char *end = getCurrent();
//			        while (*end && *end != 0x0a) end++;
//			        if (*end) tmp[3] = end;
//			        *end = '\0';
//			        char *buf = getCurrent();
//			        for (i=2; i>=0; i--) {
//			            if (linenum + i - 3 > 0) {
//			                while (*buf != 0x0a) buf--;
//			                tmp[i] = buf;
//			                *buf = '\0';
//			                line[i] = buf+1;
//			            } else if (linenum + i - 3 == 0) {
//			                line[i] = script_buffer;
//			            } else
//			                line[i] = end;
//			        }
//			
//			        snprintf(errhist, 1024, "%s\nat line %d (*%s:%d)%s\n\n| %s\n| %s\n> %s",
//			                 str, linenum, label.name, lblinenum, errcmd,
//			                 line[0], line[1], line[2]);
//			
//			        for (i=0; i<4; i++) {
//			            if (tmp[i]) *(tmp[i]) = 0x0a;
//			        }
//			
//			        if (! ons->doErrorBox(title, errhist, false, false))
//			            return;
//			
//			    }
//			
//			    exit(-1);
			}
			
			public void errorAndExit( CharPtr str, CharPtr detail = null, CharPtr title = null, bool is_warning = false )
			{
//			    if (title == NULL)
//			        title = "Script Error";
//			
//			    processError(str, detail, title, is_warning);
			}
			
			public void simpleErrorAndExit( CharPtr str, CharPtr title, CharPtr detail, bool is_warning )
			{
//			    if (title == NULL)
//			        title = "Script Error";
//			
//			    processError(str, detail, title, is_warning, true);
			}
			
			public void addStringBuffer( char ch )
			{
//			    if (string_counter+1 == STRING_BUFFER_LENGTH)
//			        errorAndExit("addStringBuffer: string length exceeds 2048.");
//			    string_buffer[string_counter++] = ch;
//			    string_buffer[string_counter] = '\0';
			}
			
			public void trimStringBuffer( uint n )
			{
//			    string_counter -= n;
//			    if (string_counter < 0)
//			        string_counter = 0;
//			    string_buffer[string_counter] = '\0';
			}
			
			public void pushStringBuffer(int offset)
			{
//			    strcpy(gosub_string_buffer, string_buffer);
//			    gosub_string_offset = offset;
			}
			
			public int popStringBuffer()
			{
				return 0;
//			    strcpy(string_buffer, gosub_string_buffer);
//			    text_flag = true;
//			    return gosub_string_offset;
			}
			
			public VariableData getVariableData(int no)
			{
				return null;
//			    if (no >= 0 && no < VARIABLE_RANGE)
//			        return variable_data[no];
//			
//			    for (int i=0 ; i<num_extended_variable_data ; i++)
//			        if (extended_variable_data[i].no == no) 
//			            return extended_variable_data[i].vd;
//			        
//			    num_extended_variable_data++;
//			    if (num_extended_variable_data == max_extended_variable_data){
//			        ExtendedVariableData *tmp = extended_variable_data;
//			        extended_variable_data = new ExtendedVariableData[max_extended_variable_data*2];
//			        if (tmp){
//			            memcpy(extended_variable_data, tmp, sizeof(ExtendedVariableData)*max_extended_variable_data);
//			            delete[] tmp;
//			        }
//			        max_extended_variable_data *= 2;
//			    }
//			
//			    extended_variable_data[num_extended_variable_data-1].no = no;
//			
//			    return extended_variable_data[num_extended_variable_data-1].vd;
			}
			
			// ----------------------------------------
			// Private methods
			
			public int findLabel( CharPtr label )
			{
				return 0;
//			    int i;
//			    char capital_label[256];
//			
//			    for ( i=0 ; i<(int)strlen( label )+1 ; i++ ){
//			        capital_label[i] = label[i];
//			        if ( 'A' <= capital_label[i] && capital_label[i] <= 'Z' ) capital_label[i] += 'a' - 'A';
//			    }
//			    for ( i=0 ; i<num_of_labels ; i++ ){
//			        if ( !strcmp( label_info[i].name, capital_label ) )
//			            return i;
//			    }
//			
//			    snprintf(errbuf, MAX_ERRBUF_LEN, "Label \"*%s\" not found.", label);
//			    errorAndExit( errbuf, NULL, "Label Error" );
//			
//			    return -1; // dummy
			}
			
			public CharPtr checkComma( CharPtr buf )
			{
				return null;
//			    SKIP_SPACE( buf );
//			    if (*buf == ','){
//			        end_status |= END_COMMA;
//			        buf++;
//			        SKIP_SPACE( buf );
//			    }
//			
//			    return buf;
			}
			
			public void parseStr( CharPtr[] buf )
			{
//			    SKIP_SPACE( *buf );
//			
//			    if ( **buf == '(' ){
//			        // (foo) bar baz : apparently returns bar if foo has been
//			        // viewed, baz otherwise.
//			        // (Rather like a trigram implicitly using "fchk")
//			
//			        (*buf)++;
//			        parseStr(buf);
//			        SKIP_SPACE( *buf );
//			        if ( (*buf)[0] != ')' ) errorAndExit("parseStr: missing ')'.");
//			        (*buf)++;
//			
//			        if ( findAndAddLog( log_info[FILE_LOG], str_string_buffer, false ) ){
//			            parseStr(buf);
//			            char *tmp_buf = new char[ strlen( str_string_buffer ) + 1 ];
//			            strcpy( tmp_buf, str_string_buffer );
//			            parseStr(buf);
//			            strcpy( str_string_buffer, tmp_buf );
//			            delete[] tmp_buf;
//			        }
//			        else{
//			            parseStr(buf);
//			            parseStr(buf);
//			        }
//			        current_variable.type |= VAR_CONST;
//			    }
//			    else if ( **buf == '$' ){
//			        (*buf)++;
//			        int no = parseInt(buf);
//			        VariableData &vd = getVariableData(no);
//			
//			        if ( vd.str )
//			            strcpy( str_string_buffer, vd.str );
//			        else
//			            str_string_buffer[0] = '\0';
//			        current_variable.type = VAR_STR;
//			        current_variable.var_no = no;
//			    }
//			    else if ( **buf == '"' ){
//			        int c=0;
//			        (*buf)++;
//			        while ( **buf != '"' && **buf != 0x0a )
//			            str_string_buffer[c++] = *(*buf)++;
//			        str_string_buffer[c] = '\0';
//			        if ( **buf == '"' ) (*buf)++;
//			        current_variable.type |= VAR_CONST;
//			    }
//			    else if ( **buf == '`' ){
//			        int c=0;
//			        str_string_buffer[c++] = *(*buf)++;
//			        while ( **buf != '`' && **buf != 0x0a )
//			            str_string_buffer[c++] = *(*buf)++;
//			        str_string_buffer[c] = '\0';
//			        if ( **buf == '`' ) (*buf)++;
//			        current_variable.type |= VAR_CONST;
//			        end_status |= END_1BYTE_CHAR;
//			    }
//			    else if ( **buf == '#' ){ // for color
//			        for ( int i=0 ; i<7 ; i++ )
//			            str_string_buffer[i] = *(*buf)++;
//			        str_string_buffer[7] = '\0';
//			        current_variable.type = VAR_NONE;
//			    }
//			    else if ( **buf == '*' ){ // label
//			        int c=0;
//			        str_string_buffer[c++] = *(*buf)++;
//			        SKIP_SPACE(*buf);
//			        char ch = **buf;
//			        while((ch >= 'a' && ch <= 'z') || 
//			              (ch >= 'A' && ch <= 'Z') ||
//			              (ch >= '0' && ch <= '9') ||
//			              ch == '_'){
//			            if (ch >= 'A' && ch <= 'Z') ch += 'a' - 'A';
//			            str_string_buffer[c++] = ch;
//			            ch = *++(*buf);
//			        }
//			        str_string_buffer[c] = '\0';
//			        current_variable.type |= VAR_CONST;
//			    }
//			    else{ // str alias
//			        char ch, alias_buf[512];
//			        int alias_buf_len = 0;
//			        bool first_flag = true;
//			
//			        while(1){
//			            if ( alias_buf_len == 511 ) break;
//			            ch = **buf;
//			
//			            if ( (ch >= 'a' && ch <= 'z') ||
//			                 (ch >= 'A' && ch <= 'Z') ||
//			                 ch == '_' ){
//			                if (ch >= 'A' && ch <= 'Z') ch += 'a' - 'A';
//			                first_flag = false;
//			                alias_buf[ alias_buf_len++ ] = ch;
//			            }
//			            else if ( ch >= '0' && ch <= '9' ){
//			                if ( first_flag )
//			                    errorAndExit("parseStr: string alias cannot start with a digit.");
//			                first_flag = false;
//			                alias_buf[ alias_buf_len++ ] = ch;
//			            }
//			            else break;
//			            (*buf)++;
//			        }
//			        alias_buf[alias_buf_len] = '\0';
//			
//			        if ( alias_buf_len == 0 ){
//			            str_string_buffer[0] = '\0';
//			            current_variable.type = VAR_NONE;
//			            return;
//			        }
//			
//			        if (!findStrAlias( (const char*)alias_buf, str_string_buffer )) {
//			            snprintf(errbuf, MAX_ERRBUF_LEN, "Undefined string alias '%s'", alias_buf);
//			            errorAndExit(errbuf);
//			        }
//			        current_variable.type |= VAR_CONST;
//			    }
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
