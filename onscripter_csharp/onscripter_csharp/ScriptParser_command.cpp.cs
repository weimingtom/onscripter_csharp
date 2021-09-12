/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 14:51
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
		 *  ScriptParser_command.cpp - Define command executer of ONScripter-EN
		 *
		 *  Copyright (c) 2001-2010 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2008-2011 "Uncle" Mion Sonozaki
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
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#endif
//		
//		#include "ScriptParser.h"
//		#include <math.h>
//		#include <sys/stat.h>
//		#include <sys/types.h>
//		
//		#ifdef WIN32
//		#include <direct.h>
//		#include <windows.h>
//		#endif
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
//		
//		#ifndef M_PI
//		#define M_PI 3.14159265358979323846
//		#endif
		
		public partial class ScriptParser {
			public int zenkakkoCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "zenkakko: not in the define section" );
//			    zenkakko_flag = true;
//			    
//			    return RET_CONTINUE;
			}
			
			public int windowchipCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "windowchip: not in the define section" );
//			    windowchip_sprite_no = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int windowbackCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "windowback: not in the define section" );
//			    windowback_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int versionstrCommand()
			{
				return 0;
//			    delete[] version_str;
//			
//			    script_h.readStr();
//			    const char *save_buf = script_h.saveStringBuffer();
//			
//			    const char *buf = script_h.readStr();
//			    version_str = new char[ strlen( save_buf ) + strlen( buf ) + strlen("\n") * 2 + 1 ];
//			    sprintf( version_str, "%s\n%s\n", save_buf, buf );
//			
//			    return RET_CONTINUE;
			}
			
			public int usewheelCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "usewheel: not in the define section" );
//			
//			    usewheel_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int useescspcCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "useescspc: not in the define section" );
//			
//			    if ( !force_button_shortcut_flag )
//			        useescspc_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int underlineCommand()
			{
				return 0;
//			    underline_value = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int transmodeCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "transmode: not in the define section" );
//			
//			    if      ( script_h.compareString("leftup") )   trans_mode = AnimationInfo::TRANS_TOPLEFT;
//			    else if ( script_h.compareString("copy") )     trans_mode = AnimationInfo::TRANS_COPY;
//			    else if ( script_h.compareString("alpha") )    trans_mode = AnimationInfo::TRANS_ALPHA;
//			    else if ( script_h.compareString("righttup") ) trans_mode = AnimationInfo::TRANS_TOPRIGHT;
//			    script_h.readName();
//			
//			    return RET_CONTINUE;
			}
			
			public int timeCommand()
			{
				return 0;
//			    time_t t = time(NULL);
//			    struct tm *tm = localtime( &t );
//			
//			    script_h.readVariable();
//			    script_h.setInt( &script_h.current_variable, tm->tm_hour );
//			    
//			    script_h.readVariable();
//			    script_h.setInt( &script_h.current_variable, tm->tm_min );
//			    
//			    script_h.readVariable();
//			    script_h.setInt( &script_h.current_variable, tm->tm_sec );
//			
//			    return RET_CONTINUE;
			}
			
			public int textgosubCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "textgosub: not in the define section" );
//			
//			    setStr( &textgosub_label, script_h.readLabel()+1 );
//			    script_h.enableTextgosub(true);
//			    
//			    return RET_CONTINUE;
			}
			
			public int tanCommand()
			{
				return 0;
//			    script_h.readInt();
//			    script_h.pushVariable();
//			
//			    int val = script_h.readInt();
//			    script_h.setInt(&script_h.pushed_variable, (int)(tan(M_PI*val/180.0)*1000.0));
//			
//			    return RET_CONTINUE;
			}
			
			public int subCommand()
			{
				return 0;
//			    int val1 = script_h.readInt();
//			    script_h.pushVariable();
//			
//			    int val2 = script_h.readInt();
//			    script_h.setInt( &script_h.pushed_variable, val1 - val2 );
//			
//			    return RET_CONTINUE;
			}
			
			public int straliasCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "stralias: not in the define section" );
//			    
//			    script_h.readName();
//			    const char *save_buf = script_h.saveStringBuffer();
//			    const char *buf = script_h.readStr();
//			    
//			    script_h.addStrAlias( save_buf, buf );
//			    
//			    return RET_CONTINUE;
			}
			
			public int soundpressplginCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "soundpressplgin: not in the define section" );
//			
//			    const char *buf = script_h.readStr();
//			    int buf_len = (int) strlen(buf);
//			    char buf2[1024];
//			    if (buf_len + 1 > 1024) return RET_NOMATCH;
//			    strcpy(buf2, buf);
//			
//			    // only nbzplgin.dll and jpgplgin.dll are "supported"
//			    for (int i=0 ; i<12 ; i++)
//			        if (buf2[i] >= 'A' && buf2[i] <= 'Z') buf2[i] += 'a' - 'A';
//			    if (strncmp(buf2, "nbzplgin.dll", 12) && strncmp(buf2, "jpgplgin.dll", 12)){
//			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
//			                 "soundpressplgin: plugin %s is not available.", buf);
//			        errorAndCont(script_h.errbuf);
//			        return RET_CONTINUE;
//			    }
//			
//			    while(*buf && *buf != '|') buf++;
//			    if (*buf == 0) return RET_CONTINUE;
//			    
//			    buf++;
//			    //script_h.cBR->registerCompressionType( buf, BaseReader::NBZ_COMPRESSION );
//			
//			    return RET_CONTINUE;
			}
			
			public int skipCommand()
			{
				return 0;
//			    int line = current_label_info.start_line + current_line + script_h.readInt();
//			
//			    char *buf = script_h.getAddressByLine( line );
//			    current_label_info = script_h.getLabelByAddress( buf );
//			    current_line = script_h.getLineByAddress( buf );
//			    
//			    script_h.setCurrent( buf );
//			
//			    return RET_CONTINUE;
			}
			
			public int sinCommand()
			{
				return 0;
//			    script_h.readInt();
//			    script_h.pushVariable();
//			
//			    int val = script_h.readInt();
//			    script_h.setInt(&script_h.pushed_variable, (int)(sin(M_PI*val/180.0)*1000.0));
//			
//			    return RET_CONTINUE;
			}
			
			public int shadedistanceCommand()
			{
				return 0;
//			    shade_distance[0] = script_h.readInt();
//			    shade_distance[1] = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			//Mion
			public int setlayerCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "setlayer: not in the define section" );
//			
//			    int no = script_h.readInt();
//			    int interval = script_h.readInt();
//			    const char *dll = script_h.readStr();
//			
//			#ifdef NO_LAYER_EFFECTS
//			    snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
//			             "setlayer: layer effect support not available (%d,%d,'%s')",
//			             no, interval, dll);
//			    errorAndCont(script_h.errbuf);
//			#else
//			    Layer *handler = NULL;
//			    const char *bslash = strrchr(dll, '\\');
//			#ifndef BPP16 //not supporting oldmovie for 16bpp yet
//			    if ((bslash && !strncmp(bslash+1, "oldmovie.dll", 12)) ||
//			        !strncmp(dll, "oldmovie.dll", 12)) {
//			        handler = new OldMovieLayer( screen_width, screen_height );
//			    } else
//			#endif //BPP16
//			    if ((bslash && !strncmp(bslash+1, "snow.dll", 8)) ||
//			               !strncmp(dll, "snow.dll", 8)) {
//			        handler = new FuruLayer( screen_width, screen_height, false, script_h.cBR );
//			    } else if ((bslash && !strncmp(bslash+1, "hana.dll", 8)) ||
//			               !strncmp(dll, "hana.dll", 8)) {
//			        handler = new FuruLayer( screen_width, screen_height, true, script_h.cBR );
//			    } else {
//			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
//			                 "setlayer: layer effect '%s' is not implemented.", dll);
//			        errorAndCont(script_h.errbuf);
//			        return RET_CONTINUE;
//			    }
//			
//			    printf("Setup layer effect for '%s'.\n", dll);
//			    LayerInfo *layer = new LayerInfo();
//			    layer->num = no;
//			    layer->interval = interval;
//			    layer->handler = handler;
//			    layer->next = layer_info;
//			    layer_info = layer;
//			#endif // ndef NO_LAYER_EFFECTS
//			
//			    return RET_CONTINUE;
			}
			
			//Mion: for kinsoku
			public int setkinsokuCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "setkinsoku: not in the define section" );
//			
//			    script_h.readStr();
//			    char *start = script_h.saveStringBuffer();
//			    const char *end = script_h.readStr();
//			    setKinsoku(start, end, false);
//			    if (debug_level > 0)
//			        printf("setkinsoku: \"%s\",\"%s\"\n", start, end);
//			    
//			    return RET_CONTINUE;
			}
			
			public int selectvoiceCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "selectvoice: not in the define section" );
//			
//			    for ( int i=0 ; i<SELECTVOICE_NUM ; i++ )
//			        setStr( &selectvoice_file_name[i], script_h.readStr() );
//			
//			    return RET_CONTINUE;
			}
			
			public int selectcolorCommand()
			{
				return 0;
//			    const char *buf = readColorStr();
//			    readColor( &sentence_font.on_color, buf );
//			
//			    buf = readColorStr();
//			    readColor( &sentence_font.off_color, buf );
//			    
//			    return RET_CONTINUE;
			}
			
			public int savenumberCommand()
			{
				return 0;
//			    num_save_file = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int savenameCommand()
			{
				return 0;
//			    const char *buf = script_h.readStr();
//			    setStr( &save_menu_name, buf );
//			
//			    buf = script_h.readStr();
//			    setStr( &load_menu_name, buf );
//			
//			    buf = script_h.readStr();
//			    setStr( &save_item_name, buf );
//			
//			    return RET_CONTINUE;
			}
			
			public int savedirCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "savedir: not in the define section" );
//			
//			    const char *buf = script_h.readStr();
//			
//			    // Only allow setting the savedir once, no empty path
//			    if ((*buf != '\0') && (!savedir)) {
//			        // Note that savedir is relative to save_path
//			        setStr(&savedir, buf);
//			        script_h.setSavedir(buf);
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int rubyonCommand()
			{
				return 0;
//			    rubyon_flag = true;
//			    if (script_h.isName("rubyon2"))
//			        rubyon2_flag = true;
//			
//			    char *buf = script_h.getNext();
//			    if (!rubyon2_flag && ( buf[0] == 0x0a || buf[0] == ':' || buf[0] == ';' )){
//			        ruby_struct.font_size_xy[0] = -1;
//			        ruby_struct.font_size_xy[1] = -1;
//			        setStr( &ruby_struct.font_name, NULL );
//			    }
//			    else{
//			        ruby_struct.font_size_xy[0] = script_h.readInt();
//			        ruby_struct.font_size_xy[1] = script_h.readInt();
//			
//			        if (!rubyon2_flag && (script_h.getEndStatus() & ScriptHandler::END_COMMA)){
//			            setStr( &ruby_struct.font_name, script_h.readStr() );
//			        }
//			        else{
//			            setStr( &ruby_struct.font_name, NULL );
//			        }
//			    }
//			    sentence_font.setRubyOnFlag(true);
//			
//			    return RET_CONTINUE;
			}
			
			public int rubyoffCommand()
			{
				return 0;
//			    rubyon_flag = rubyon2_flag = false;
//			    sentence_font.setRubyOnFlag(false);
//			
//			    return RET_CONTINUE;
			}
			
			public int roffCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "roff: not in the define section" );
//			    rmode_flag = false;
//			
//			    return RET_CONTINUE;
			}
			
			public int rmenuCommand()
			{
				return 0;
//			    deleteRMenuLink();
//			
//			    RMenuLink *link = &root_rmenu_link;
//			    bool comma_flag = true;
//			    while ( comma_flag ){
//			        link->next = new RMenuLink();
//			
//			        const char *buf = script_h.readStr();
//			        setStr( &link->next->label, buf );
//			        if ( rmenu_link_width < strlen( buf )/2 + 1 )
//			            rmenu_link_width = strlen( buf )/2 + 1;
//			
//			        link->next->system_call_no = getSystemCallNo( script_h.readName() );
//			
//			        link = link->next;
//			        rmenu_link_num++;
//			
//			        comma_flag = script_h.getEndStatus() & ScriptHandler::END_COMMA;
//			    }
//			    
//			    return RET_CONTINUE;
			}
			
			public int rgosubCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "rgosub: not in the define section" );
//			
//			    setStr( &rgosub_label, script_h.readLabel()+1 );
//			    script_h.enableRgosub(true);
//			
//			    return RET_CONTINUE;
			}
			
			public int returnCommand()
			{
				return 0;
//			    if ( !last_nest_info->previous || last_nest_info->nest_mode != NestInfo::LABEL )
//			        errorAndExit( "return: not in gosub" );
//			    
//			    current_label_info = script_h.getLabelByAddress( last_nest_info->next_script );
//			    current_line = script_h.getLineByAddress( last_nest_info->next_script );
//			    
//			    const char *label = script_h.readStr();
//			    if (label[0] != '*')
//			        script_h.setCurrent( last_nest_info->next_script );
//			    else
//			        setCurrentLabel( label+1 );
//			
//			    bool textgosub_flag = last_nest_info->textgosub_flag;
//			
//			    int rgosub_state = last_nest_info->rgosub_click_state;
//			    if (rgosub_label && (rgosub_state != CLICK_NONE)){
//			        script_h.is_rgosub_click = true;
//			        script_h.rgosub_click_newpage = (rgosub_state == CLICK_NEWPAGE);
//			        script_h.rgosub_1byte_mode = last_nest_info->rgosub_1byte_mode;
//			    }
//			
//			    last_nest_info = last_nest_info->previous;
//			    delete last_nest_info->next;
//			    last_nest_info->next = NULL;
//			
//			    if (textgosub_flag){
//			        string_buffer_offset = script_h.popStringBuffer();
//			        if (script_h.getStringBuffer()[string_buffer_offset] != 0)
//			            return RET_NO_READ;
//			        else
//			            return RET_CONTINUE | RET_EOT;
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int pretextgosubCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "pretextgosub: not in the define section" );
//			
//			    setStr( &pretextgosub_label, script_h.readStr()+1 );
//			    
//			    return RET_CONTINUE;
			}
			
			public int pagetagCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "pagetag: not in the define section" );
//			
//			    pagetag_flag = true;
//			    
//			    return RET_CONTINUE;
			}
			
			public int numaliasCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "numalias: numalias: not in the define section" );
//			
//			    script_h.readName();
//			    const char *save_buf = script_h.saveStringBuffer();
//			
//			    int no = script_h.readInt();
//			    script_h.addNumAlias( save_buf, no );
//			    
//			    return RET_CONTINUE;
			}
			
			public int nsadirCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "nsadir: not in the define section" );
//			
//			    const char *buf = script_h.readStr();
//			    
//			    nsa_path = DirPaths(buf);
//			
//			    delete script_h.cBR;
//			    script_h.cBR = new NsaReader( archive_path, nsa_offset, key_table );
//			    if ( script_h.cBR->open( nsa_path.get_all_paths() ) ){
//			        errorAndCont( "nsadir: couldn't open any NSA archives" );
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int nsaCommand()
			{
				return 0;
//			    //Mion: WARNING - commands "ns2" and "ns3" have nothing to do with
//			    // archive files named "*.ns2", they are for "*.nsa" files.
//			    // I suggest using command-line options "--nsa-offset 1" and
//			    // "--nsa-offset 2" instead of these commands
//			    if ( script_h.isName("ns2") ){
//			        nsa_offset = 1;
//			    }
//			    else if ( script_h.isName("ns3") ){
//			        nsa_offset = 2;
//			    }
//			    
//			    delete script_h.cBR;
//			    script_h.cBR = new NsaReader( archive_path, nsa_offset, key_table );
//			    if ( script_h.cBR->open( nsa_path.get_all_paths() ) ){
//			        errorAndCont( "nsa: couldn't open any NSA archives" );
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int nextCommand()
			{
				return 0;
//			//Mion: apparently NScr allows 'break' outside of a for loop, it just skips ahead to 'next'
//			    if (!last_nest_info->previous || last_nest_info->nest_mode != NestInfo::FOR) {
//			        errorAndCont("next: not in for loop\n");
//			        break_flag = false;
//			        return RET_CONTINUE;
//			    }
//			    
//			    int val;
//			    if ( !break_flag ){
//			        val = script_h.getVariableData(last_nest_info->var_no).num;
//			        script_h.setNumVariable( last_nest_info->var_no, val + last_nest_info->step );
//			    }
//			
//			    val = script_h.getVariableData(last_nest_info->var_no).num;
//			    
//			    if ( break_flag ||
//			         ((last_nest_info->step > 0) && (val > last_nest_info->to)) ||
//			         ((last_nest_info->step < 0) && (val < last_nest_info->to)) ){
//			        break_flag = false;
//			        last_nest_info = last_nest_info->previous;
//			
//			        delete last_nest_info->next;
//			        last_nest_info->next = NULL;
//			    }
//			    else{
//			        script_h.setCurrent( last_nest_info->next_script );
//			        current_label_info =
//			            script_h.getLabelByAddress( last_nest_info->next_script );
//			        current_line =
//			            script_h.getLineByAddress( last_nest_info->next_script );
//			    }
//			    
//			    return RET_CONTINUE;
			}
			
			public int mulCommand()
			{
				return 0;
//			    int val1 = script_h.readInt();
//			    script_h.pushVariable();
//			    
//			    int val2 = script_h.readInt();
//			    script_h.setInt( &script_h.pushed_variable, val1*val2 );
//			
//			    return RET_CONTINUE;
			}
			
			public int movCommand()
			{
				return 0;
//			    int count = 1;
//			    
//			    if ( script_h.isName( "mov10" ) ){
//			        count = 10;
//			    }
//			    else if ( script_h.isName( "movl" ) ){
//			        count = -1; // infinite
//			    }
//			    else if ( script_h.getStringBuffer()[3] >= '3' && script_h.getStringBuffer()[3] <= '9' ){
//			        count = script_h.getStringBuffer()[3] - '0';
//			    }
//			
//			    script_h.readVariable();
//			
//			    if ( script_h.current_variable.type == ScriptHandler::VAR_INT ||
//			         script_h.current_variable.type == ScriptHandler::VAR_ARRAY ){
//			        script_h.pushVariable();
//			        bool loop_flag = (script_h.getEndStatus() & ScriptHandler::END_COMMA);
//			        int i=0;
//			        while ( (count==-1 || i<count) && loop_flag ){
//			            int no = script_h.readInt();
//			            loop_flag = (script_h.getEndStatus() & ScriptHandler::END_COMMA);
//			            script_h.setInt( &script_h.pushed_variable, no, i++ );
//			        }
//			    }
//			    else if ( script_h.current_variable.type == ScriptHandler::VAR_STR ){
//			        script_h.pushVariable();
//			        const char *buf = script_h.readStr();
//			        setStr( &script_h.getVariableData(script_h.pushed_variable.var_no).str, buf );
//			    }
//			    else errorAndExit( "mov: no variable" );
//			    
//			    return RET_CONTINUE;
			}
			
			public int mode_wave_demoCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "mode_wave_demo: not in the define section" );
//			    mode_wave_demo_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int mode_sayaCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "mode_saya: not in the define section" );
//			    mode_saya_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int mode_extCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "mode_ext: not in the define section" );
//			    mode_ext_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int modCommand()
			{
				return 0;
//			    int val1 = script_h.readInt();
//			    script_h.pushVariable();
//			    
//			    int val2 = script_h.readInt();
//			    script_h.setInt( &script_h.pushed_variable, val1%val2 );
//			
//			    return RET_CONTINUE;
			}
			
			public int midCommand()
			{
				return 0;
//			    script_h.readStr();
//			    if ( script_h.current_variable.type != ScriptHandler::VAR_STR )
//			        errorAndExit( "mid: no string variable" );
//			    int no = script_h.current_variable.var_no;
//			    
//			    script_h.readStr();
//			    const char *save_buf = script_h.saveStringBuffer();
//			    unsigned int start = script_h.readInt();
//			    unsigned int len   = script_h.readInt();
//			
//			    ScriptHandler::VariableData &vd = script_h.getVariableData(no);
//			    if ( vd.str ) delete[] vd.str;
//			    if ( start >= strlen(save_buf) ){
//			        vd.str = NULL;
//			    }
//			    else{
//			        if ( start+len > strlen(save_buf ) )
//			            len = strlen(save_buf) - start;
//			        vd.str = new char[len+1];
//			        memcpy( vd.str, save_buf+start, len );
//			        vd.str[len] = '\0';
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int menusetwindowCommand()
			{
				return 0;
//			    menu_font.ttf_font        = NULL;
//			    menu_font.font_size_xy[0] = script_h.readInt();
//			    menu_font.font_size_xy[1] = script_h.readInt();
//			    menu_font.pitch_xy[0]     = script_h.readInt() + menu_font.font_size_xy[0];
//			    menu_font.pitch_xy[1]     = script_h.readInt() + menu_font.font_size_xy[1];
//			    menu_font.is_bold         = script_h.readInt()?true:false;
//			    menu_font.is_shadow       = script_h.readInt()?true:false;
//			
//			    if (script_h.getEndStatus() & ScriptHandler::END_COMMA) {
//			        const char *buf = readColorStr();
//			        readColor( &menu_font.window_color, buf );
//			    }
//			    else{
//			        menu_font.window_color[0] = menu_font.window_color[1] = menu_font.window_color[2] = 0x99;
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int menuselectvoiceCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "menuselectvoice: not in the define section" );
//			
//			    for ( int i=0 ; i<MENUSELECTVOICE_NUM ; i++ )
//			        setStr( &menuselectvoice_file_name[i], script_h.readStr() );
//			
//			    return RET_CONTINUE;
			}
			
			public int menuselectcolorCommand()
			{
				return 0;
//			    const char *buf = readColorStr();
//			    readColor( &menu_font.on_color, buf );
//			
//			    buf = readColorStr();
//			    readColor( &menu_font.off_color, buf );
//			    
//			    buf = readColorStr();
//			    readColor( &menu_font.nofile_color, buf );
//			    
//			    return RET_CONTINUE;
			}
			
			public int maxkaisoupageCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "maxkaisoupage: not in the define section" );
//			    max_page_list = script_h.readInt()+1;
//			
//			    return RET_CONTINUE;
			}
			
			public int luasubCommand()
			{
				return 0;
//			    const char *cmd = script_h.readName();
//			
//			    if (cmd[0] >= 'a' && cmd[0] <= 'z'){
//			        UserFuncHash &ufh = user_func_hash[cmd[0]-'a'];
//			        ufh.last->next = new UserFuncLUT();
//			        ufh.last = ufh.last->next;
//			        ufh.last->lua_flag = true;
//			        setStr( &ufh.last->command, cmd );
//			    }
//			    
//			    return RET_CONTINUE;
			}
			
			public int luacallCommand()
			{
				return 0;
//			    const char *label = NULL;
//			    label = script_h.readLabel();
//			
//			#ifdef USE_LUA
//			    lua_handler.addCallback(label);
//			#endif
//			    
//			    return RET_CONTINUE;
			}
			
			public int lookbackspCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "lookbacksp: not in the define section" );
//			
//			    for ( int i=0 ; i<2 ; i++ )
//			        lookback_sp[i] = script_h.readInt();
//			
//			    if ( filelog_flag ){
//			        script_h.findAndAddLog( script_h.log_info[ScriptHandler::FILE_LOG], DEFAULT_LOOKBACK_NAME0, true );
//			        script_h.findAndAddLog( script_h.log_info[ScriptHandler::FILE_LOG], DEFAULT_LOOKBACK_NAME1, true );
//			        script_h.findAndAddLog( script_h.log_info[ScriptHandler::FILE_LOG], DEFAULT_LOOKBACK_NAME2, true );
//			        script_h.findAndAddLog( script_h.log_info[ScriptHandler::FILE_LOG], DEFAULT_LOOKBACK_NAME3, true );
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			public int lookbackcolorCommand()
			{
				return 0;
//			    const char *buf = readColorStr();
//			    readColor( &lookback_color, buf );
//			
//			    return RET_CONTINUE;
			}
			
			public int loadgosubCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "loadgosub: not in the define section" );
//			
//			    setStr( &loadgosub_label, script_h.readStr()+1 );
//			
//			    return RET_CONTINUE;
			}
			
			public int linepageCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "linepage: not in the define section" );
//			
//			    if ( script_h.isName( "linepage2" ) ){
//			        linepage_mode = 2;
//			        clickstr_line = script_h.readInt();
//			    }
//			    else
//			        linepage_mode = 1;
//			
//			    script_h.setLinepage(true);
//			
//			    return RET_CONTINUE;
			}
			
			public int lenCommand()
			{
				return 0;
//			    script_h.readInt();
//			    script_h.pushVariable();
//			    
//			    const char *buf = script_h.readStr();
//			
//			    script_h.setInt( &script_h.pushed_variable, strlen( buf ) );
//			
//			    return RET_CONTINUE;
			}
			
			public int labellogCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "labellog: not in the define section" );
//			
//			    labellog_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int kidokuskipCommand()
			{
				return 0;
//			    kidokuskip_flag = true;
//			    kidokumode_flag = true;
//			    script_h.loadKidokuData();
//			    
//			    return RET_CONTINUE;
			}
			
			public int kidokumodeCommand()
			{
				return 0;
//			    if ( script_h.readInt() == 1 )
//			        kidokumode_flag = true;
//			    else
//			        kidokumode_flag = false;
//			
//			    return RET_CONTINUE;
			}
			
			public int itoaCommand()
			{
				return 0;
//			    bool itoa2_flag = false;
//			
//			    if ( script_h.isName( "itoa2" ) )
//			        itoa2_flag = true;
//			    
//			    script_h.readVariable();
//			    if ( script_h.current_variable.type != ScriptHandler::VAR_STR )
//			        errorAndExit( "itoa: no string variable." );
//			    int no = script_h.current_variable.var_no;
//			
//			    int val = script_h.readInt();
//			
//			    char val_str[20];
//			    if (itoa2_flag)
//			        script_h.getStringFromInteger(val_str, val, -1, false, true);
//			    else
//			        sprintf( val_str, "%d", val );
//			    setStr( &script_h.getVariableData(no).str, val_str );
//			    
//			    return RET_CONTINUE;
			}
			
			public int intlimitCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "intlimit: not in the define section" );
//			    
//			    int no = script_h.readInt();
//			
//			    script_h.getVariableData(no).num_limit_flag  = true;
//			    script_h.getVariableData(no).num_limit_lower = script_h.readInt();
//			    script_h.getVariableData(no).num_limit_upper = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int incCommand()
			{
				return 0;
//			    int val = script_h.readInt();
//			    script_h.setInt( &script_h.current_variable, val+1 );
//			
//			    return RET_CONTINUE;
			}
			
			public int ifCommand()
			{
				return 0;
//			    //printf("ifCommand\n");
//			    int condition_status = 0; // 0 ... none, 1 ... and, 2 ... or
//			    bool f = false, condition_flag = false;
//			    char *op_buf;
//			    const char *buf;
//			
//			    bool if_flag = true;
//			    if ( script_h.isName( "notif" ) ) if_flag = false;
//			
//			    while(1){
//			        if (script_h.compareString("fchk")){
//			            script_h.readName();
//			            buf = script_h.readStr();
//			            if (*buf == '\0')
//			                f = false;
//			            else {
//			                f = (script_h.findAndAddLog( script_h.log_info[ScriptHandler::FILE_LOG], buf, false ) != NULL);
//			                //printf("fchk %s(%d) ", tmp_string_buffer, (findAndAddFileLog( tmp_string_buffer, fasle )) );
//			            }
//			        }
//			        else if (script_h.compareString("lchk")){
//			            script_h.readName();
//			            buf = script_h.readLabel();
//			            if (*buf == '\0')
//			                f = false;
//			            else {
//			                f = (script_h.findAndAddLog( script_h.log_info[ScriptHandler::LABEL_LOG], buf+1, false ) != NULL);
//			                //printf("lchk %s (%d)\n", buf, f );
//			            }
//			        }
//			        else{
//			            int no = script_h.readInt();
//			            if (script_h.current_variable.type & ScriptHandler::VAR_INT ||
//			                script_h.current_variable.type & ScriptHandler::VAR_ARRAY){
//			                int left_value = no;
//			                //printf("left (%d) ", left_value );
//			
//			                op_buf = script_h.getNext();
//			                if ( (op_buf[0] == '>' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '<' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '=' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '!' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '<' && op_buf[1] == '>') )
//			                    script_h.setCurrent(op_buf+2);
//			                else if ( op_buf[0] == '<' ||
//			                          op_buf[0] == '>' ||
//			                          op_buf[0] == '=' )
//			                    script_h.setCurrent(op_buf+1);
//			                //printf("current %c%c ", op_buf[0], op_buf[1] );
//			
//			                int right_value = script_h.readInt();
//			                //printf("right (%d) ", right_value );
//			
//			                if      (op_buf[0] == '>' && op_buf[1] == '=') f = (left_value >= right_value);
//			                else if (op_buf[0] == '<' && op_buf[1] == '=') f = (left_value <= right_value);
//			                else if (op_buf[0] == '=' && op_buf[1] == '=') f = (left_value == right_value);
//			                else if (op_buf[0] == '!' && op_buf[1] == '=') f = (left_value != right_value);
//			                else if (op_buf[0] == '<' && op_buf[1] == '>') f = (left_value != right_value);
//			                else if (op_buf[0] == '<')                     f = (left_value <  right_value);
//			                else if (op_buf[0] == '>')                     f = (left_value >  right_value);
//			                else if (op_buf[0] == '=')                     f = (left_value == right_value);
//			            }
//			            else{
//			                script_h.setCurrent(script_h.getCurrent());
//			                buf = script_h.readStr();
//			                const char *save_buf = script_h.saveStringBuffer();
//			
//			                op_buf = script_h.getNext();
//			
//			                if ( (op_buf[0] == '>' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '<' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '=' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '!' && op_buf[1] == '=') ||
//			                     (op_buf[0] == '<' && op_buf[1] == '>') )
//			                    script_h.setCurrent(op_buf+2);
//			                else if ( op_buf[0] == '<' ||
//			                          op_buf[0] == '>' ||
//			                          op_buf[0] == '=' )
//			                    script_h.setCurrent(op_buf+1);
//			            
//			                buf = script_h.readStr();
//			
//			                int val = strcmp( save_buf, buf );
//			                if      (op_buf[0] == '>' && op_buf[1] == '=') f = (val >= 0);
//			                else if (op_buf[0] == '<' && op_buf[1] == '=') f = (val <= 0);
//			                else if (op_buf[0] == '=' && op_buf[1] == '=') f = (val == 0);
//			                else if (op_buf[0] == '!' && op_buf[1] == '=') f = (val != 0);
//			                else if (op_buf[0] == '<' && op_buf[1] == '>') f = (val != 0);
//			                else if (op_buf[0] == '<')                     f = (val <  0);
//			                else if (op_buf[0] == '>')                     f = (val >  0);
//			                else if (op_buf[0] == '=')                     f = (val == 0);
//			            }
//			        }
//			
//			        f = if_flag ? f : !f;
//			        condition_flag |= f;
//			        op_buf = script_h.getNext();
//			        if ( op_buf[0] == '|' ){
//			            if (condition_status == 1)
//			                errorAndExit( "if: using & and | at the same time is not supported.");
//			            while(*op_buf == '|') op_buf++;
//			            script_h.setCurrent(op_buf);
//			            condition_status = 2;
//			            continue;
//			        }
//			
//			        if ( (condition_status == 2 && !condition_flag) || 
//			             (condition_status != 2 && !f) )
//			            return RET_SKIP_LINE;
//			
//			        if ( op_buf[0] == '&' ){
//			            if (condition_status == 2)
//			                errorAndExit( "if: using & and | at the same time is not supported.");
//			            while(*op_buf == '&') op_buf++;
//			            script_h.setCurrent(op_buf);
//			            condition_status = 1;
//			            continue;
//			        }
//			        break;
//			    };
//			
//			    /* Execute command */
//			    return RET_CONTINUE;
			}
			
			public int humanzCommand()
			{
				return 0;
//			    z_order = script_h.readInt();
//			    
//			    return RET_CONTINUE;
			}
			
			public int humanposCommand()
			{
				return 0;
//			    for (int i=0; i<3; i++)
//			        humanpos[i] = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int gotoCommand()
			{
				return 0;
//			    setCurrentLabel( script_h.readLabel()+1 );
//			    
//			    return RET_CONTINUE;
			}
			
			public void gosubReal( CharPtr label, CharPtr next_script,
			                              bool textgosub_flag, int rgosub_state,
			                              bool rgosub_1byte )
			{
//			    last_nest_info->next = new NestInfo();
//			    last_nest_info->next->previous = last_nest_info;
//			
//			    last_nest_info = last_nest_info->next;
//			    last_nest_info->next_script = next_script;
//			
//			    if (textgosub_flag){
//			        script_h.pushStringBuffer(string_buffer_offset);
//			        last_nest_info->textgosub_flag = true;
//			    }
//			    last_nest_info->rgosub_click_state = rgosub_state;
//			    last_nest_info->rgosub_1byte_mode = rgosub_1byte;
//			
//			    setCurrentLabel( label );
			}
			
			public int gosubCommand()
			{
				return 0;
//			    const char *buf = script_h.readLabel();
//			    gosubReal( buf+1, script_h.getNext() );
//			
//			    return RET_CONTINUE;
			}
			
			public int globalonCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "globalon: not in the define section" );
//			    globalon_flag = true;
//			    
//			    return RET_CONTINUE;
			}
			
			public int getparamCommand()
			{
				return 0;
//			    if ( !last_nest_info->previous || last_nest_info->nest_mode != NestInfo::LABEL )
//			        errorAndExit( "getparam: not in a subroutine" );
//			
//			    int end_status;
//			    do{
//			        script_h.readVariable();
//			        script_h.pushVariable();
//			        
//			        script_h.pushCurrent(last_nest_info->next_script);
//			
//			        if ( script_h.pushed_variable.type & ScriptHandler::VAR_PTR ){
//			            script_h.readVariable();
//			            script_h.setInt( &script_h.pushed_variable, script_h.current_variable.var_no );
//			        }
//			        else if ( script_h.pushed_variable.type & ScriptHandler::VAR_INT ||
//			                  script_h.pushed_variable.type & ScriptHandler::VAR_ARRAY ){
//			            script_h.setInt( &script_h.pushed_variable, script_h.readInt() );
//			        }
//			        else if ( script_h.pushed_variable.type & ScriptHandler::VAR_STR ){
//			            const char *buf = script_h.readStr();
//			            setStr( &script_h.getVariableData( script_h.pushed_variable.var_no ).str, buf );
//			        }
//			        
//			        end_status = script_h.getEndStatus();
//			        
//			        last_nest_info->next_script = script_h.getNext();
//			        script_h.popCurrent();
//			    }
//			    while(end_status & ScriptHandler::END_COMMA);
//			
//			    return RET_CONTINUE;
			}
			
			public int forCommand()
			{
				return 0;
//			    last_nest_info->next = new NestInfo();
//			    last_nest_info->next->previous = last_nest_info;
//			
//			    last_nest_info = last_nest_info->next;
//			    last_nest_info->nest_mode = NestInfo::FOR;
//			
//			    script_h.readVariable();
//			    if ( script_h.current_variable.type != ScriptHandler::VAR_INT )
//			        errorAndExit( "for: no integer variable." );
//			    
//			    last_nest_info->var_no = script_h.current_variable.var_no;
//			
//			    script_h.pushVariable();
//			
//			    if ( !script_h.compareString("=") ) 
//			        errorAndExit( "for: missing '='" );
//			
//			    script_h.setCurrent(script_h.getNext() + 1);
//			    int from = script_h.readInt();
//			    script_h.setInt( &script_h.pushed_variable, from );
//			    
//			    if ( !script_h.compareString("to") )
//			        errorAndExit( "for: missing 'to'" );
//			
//			    script_h.readName();
//			    
//			    last_nest_info->to = script_h.readInt();
//			
//			    if ( script_h.compareString("step") ){
//			        script_h.readName();
//			        last_nest_info->step = script_h.readInt();
//			    }
//			    else{
//			        last_nest_info->step = 1;
//			    }
//			
//			    break_flag = ((last_nest_info->step > 0) && (from > last_nest_info->to)) ||
//			                 ((last_nest_info->step < 0) && (from < last_nest_info->to));
//			    
//			    /* ---------------------------------------- */
//			    /* Step forward callee's label info */
//			    last_nest_info->next_script = script_h.getNext();
//			
//			    return RET_CONTINUE;
			}
			
			public int filelogCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "filelog: not in the define section" );
//			
//			    filelog_flag = true;
//			    readLog( script_h.log_info[ScriptHandler::FILE_LOG] );
//			    
//			    return RET_CONTINUE;
			}
			
			public int errorsaveCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "errorsave: not in the define section." );
//			    errorsave = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int englishCommand()
			{
				return 0;
//			    english_mode = true;
//			    script_h.setEnglishMode(true);
//			
//			    return RET_CONTINUE;
			}
			
			public int effectcutCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "effectcut: not in the define section." );
//			
//			    effect_cut_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int effectblankCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "effectblank: not in the define section" );
//			    
//			    effect_blank = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int effectCommand()
			{
				return 0;
//			    EffectLink *elink;
//			
//			    if ( script_h.isName( "windoweffect") ){
//			        elink = &window_effect;
//			    }
//			    else{
//			        if ( current_mode != DEFINE_MODE )
//			            errorAndExit( "effect: not in the define section" );
//			
//			        elink = new EffectLink();
//			        elink->no = script_h.readInt();
//			        if (elink->no < 2 || elink->no > 255)
//			            errorAndExit( "effect: effect number out of range" );
//			
//			        last_effect_link->next = elink;
//			        last_effect_link = last_effect_link->next;
//			    }
//			    
//			    readEffect( elink );
//			
//			    return RET_CONTINUE;
			}
			
			public int divCommand()
			{
				return 0;
//			    int val1 = script_h.readInt();
//			    script_h.pushVariable();
//			
//			    int val2 = script_h.readInt();
//			    script_h.setInt( &script_h.pushed_variable, val1/val2 );
//			
//			    return RET_CONTINUE;
			}
			
			public int dimCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "dim: not in the define section" );
//			
//			    script_h.declareDim();
//			    
//			    return RET_CONTINUE;
			}
			
			public int defvoicevolCommand()
			{
				return 0;
//			    int vol = script_h.readInt();
//			    if (use_default_volume)
//			        voice_volume = vol;
//			
//			    return RET_CONTINUE;
			}
			
			public int defsubCommand()
			{
				return 0;
//			    const char *cmd = script_h.readName();
//			
//			    if (cmd[0] >= 'a' && cmd[0] <= 'z'){
//			        UserFuncHash &ufh = user_func_hash[cmd[0]-'a'];
//			        ufh.last->next = new UserFuncLUT();
//			        ufh.last = ufh.last->next;
//			        setStr( &ufh.last->command, cmd );
//			    }
//			    
//			    return RET_CONTINUE;
			}
			
			public int defsevolCommand()
			{
				return 0;
//			    int vol = script_h.readInt();
//			    if (use_default_volume)
//			        se_volume = vol;
//			
//			    return RET_CONTINUE;
			}
			
			public int defmp3volCommand()
			{
				return 0;
//			    int vol = script_h.readInt();
//			    if (use_default_volume)
//			        music_volume = vol;
//			
//			    return RET_CONTINUE;
			}
			
			public int defaultspeedCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "defaultspeed: not in the define section" );
//			
//			    for ( int i=0 ; i<3 ; i++ ) default_text_speed[i] = script_h.readInt();
//			
//			    return RET_CONTINUE;
			}
			
			public int defaultfontCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "defaultfont: not in the define section" );
//			
//			    setStr( &default_env_font, script_h.readStr() );
//			
//			    return RET_CONTINUE;
			}
			
			public int decCommand()
			{
				return 0;
//			    int val = script_h.readInt();
//			    script_h.setInt( &script_h.current_variable, val-1 );
//			
//			    return RET_CONTINUE;
			}
			
			public int dateCommand()
			{
				return 0;
//			    time_t t = time(NULL);
//			    struct tm *tm = localtime( &t );
//			
//			    script_h.readInt();
//			    script_h.setInt( &script_h.current_variable, tm->tm_year % 100 );
//			
//			    script_h.readInt();
//			    script_h.setInt( &script_h.current_variable, tm->tm_mon + 1 );
//			
//			    script_h.readInt();
//			    script_h.setInt( &script_h.current_variable, tm->tm_mday );
//			
//			    return RET_CONTINUE;
			}
			
			public int cosCommand()
			{
				return 0;
//			    script_h.readInt();
//			    script_h.pushVariable();
//			
//			    int val = script_h.readInt();
//			    script_h.setInt(&script_h.pushed_variable, (int)(cos(M_PI*val/180.0)*1000.0));
//			
//			    return RET_CONTINUE;
			}
			
			public int cmpCommand()
			{
				return 0;
//			    script_h.readInt();
//			    script_h.pushVariable();
//			    
//			    script_h.readStr();
//			    char *save_buf = script_h.saveStringBuffer();
//			
//			    const char *buf = script_h.readStr();
//			
//			    int val = strcmp( save_buf, buf );
//			    if      ( val > 0 ) val = 1;
//			    else if ( val < 0 ) val = -1;
//			    script_h.setInt( &script_h.pushed_variable, val );
//			
//			    return RET_CONTINUE;
			}
			
			public int clickvoiceCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "clickvoice: not in the define section" );
//			
//			    for ( int i=0 ; i<CLICKVOICE_NUM ; i++ )
//			        setStr( &clickvoice_file_name[i], script_h.readStr() );
//			
//			    return RET_CONTINUE;
			}
			
			public int clickstrCommand()
			{
				return 0;
//			    script_h.readStr();
//			    const char *buf = script_h.saveStringBuffer();
//			
//			    clickstr_line = script_h.readInt();
//			
//			    script_h.setClickstr( buf );
//			           
//			    return RET_CONTINUE;
			}
			
			public int clickskippageCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "clickskippage: not in the define section" );
//			
//			    clickskippage_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int btnnowindoweraseCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "btnnowindowerase: not in the define section" );
//			
//			    btnnowindowerase_flag = true;
//			
//			    return RET_CONTINUE;
			}
			
			public int breakCommand()
			{
				return 0;
//			//Mion: apparently NScr allows 'break' outside of a for loop, it just skips ahead to 'next'
//			    bool unnested = false;
//			    if (!last_nest_info->previous || last_nest_info->nest_mode != NestInfo::FOR) {
//			        unnested = true;
//			        errorAndCont("break: not in 'for' loop");
//			    }
//			
//			    char *buf = script_h.getNext();
//			    if ( buf[0] == '*' ){
//			        if (!unnested) {
//			            last_nest_info = last_nest_info->previous;
//			            delete last_nest_info->next;
//			            last_nest_info->next = NULL;
//			        }
//			        setCurrentLabel( script_h.readLabel()+1 );
//			    }
//			    else{
//			        break_flag = true;
//			    }
//			    
//			    return RET_CONTINUE;
			}
			
			public int atoiCommand()
			{
				return 0;
//			    script_h.readInt();
//			    script_h.pushVariable();
//			    
//			    const char *buf = script_h.readStr();
//			        
//			    script_h.setInt( &script_h.pushed_variable, atoi(buf) );
//			    
//			    return RET_CONTINUE;
			}
			
			public int arcCommand()
			{
				return 0;
//			    const char *buf = script_h.readStr();
//			    char *buf2 = new char[ strlen( buf ) + 1 ];
//			    strcpy( buf2, buf );
//			
//			    int i = 0;
//			    while ( buf2[i] != '|' && buf2[i] != '\0' ) i++;
//			    buf2[i] = '\0';
//			
//			    if ( strcmp( script_h.cBR->getArchiveName(), "direct" ) == 0 ){
//			        delete script_h.cBR;
//			        script_h.cBR = new SarReader( archive_path, key_table );
//			        if ( script_h.cBR->open( buf2 ) ){
//			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
//			                     "arc: couldn't open archive '%s'", buf2);
//			            errorAndCont( script_h.errbuf );
//			        }
//			    }
//			    else if ( strcmp( script_h.cBR->getArchiveName(), "sar" ) == 0 ){
//			        if ( script_h.cBR->open( buf2 ) ){
//			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
//			                     "arc: couldn't open archive '%s'", buf2);
//			            errorAndCont( script_h.errbuf );
//			        }
//			    }
//			    // skipping "arc" commands after "ns?" command
//			    
//			    delete[] buf2;
//			    
//			    return RET_CONTINUE;
			}
			
			public int addnsadirCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "addnsadir: not in the define section" );
//			
//			    const char *buf = script_h.readStr();
//			    
//			    nsa_path.add(buf);
//			
//			    delete script_h.cBR;
//			    script_h.cBR = new NsaReader( archive_path, nsa_offset, key_table );
//			    if ( script_h.cBR->open( nsa_path.get_all_paths() ) ){
//			        errorAndCont( "addnsadir: couldn't open any NSA archives" );
//			    }
//			
//			    return RET_CONTINUE;
			}
			
			//Mion: for kinsoku
			public int addkinsokuCommand()
			{
				return 0;
//			    if ( current_mode != DEFINE_MODE )
//			        errorAndExit( "addkinsoku: not in the define section" );
//			
//			    script_h.readStr();
//			    char *start = script_h.saveStringBuffer();
//			    const char *end = script_h.readStr();
//			    setKinsoku(start, end, true);
//			    if (debug_level > 0)
//			        printf("addkinsoku: \"%s\",\"%s\"\n", start, end);
//			    
//			    return RET_CONTINUE;
			}
			
			public int addCommand()
			{
			    script_h.readVariable();
			    
			    if ( script_h.current_variable.type == ScriptHandler.VAR_INT ||
			         script_h.current_variable.type == ScriptHandler.VAR_ARRAY ){
			        int val = script_h.getIntVariable( script_h.current_variable );
			        script_h.pushVariable();
			
			        script_h.setInt( script_h.pushed_variable, val+script_h.readInt() );
			    }
			    else if ( script_h.current_variable.type == ScriptHandler.VAR_STR ){
			        int no = script_h.current_variable.var_no;
			
			        CharPtr buf = script_h.readStr();
			        ScriptHandler.VariableData vd = script_h.getVariableData(no);
			        CharPtr tmp_buffer = new CharPtr(vd.str);
			
			        if ( tmp_buffer != null ){
			            vd.str = new char[ strlen( tmp_buffer ) + strlen( buf ) + 1 ];
			            strcpy( vd.str, tmp_buffer );
			            strcat( vd.str, buf );
			            tmp_buffer = null; //delete[] tmp_buffer;
			        }
			        else{
			            vd.str = new char[ strlen( buf ) + 1 ];
			            strcpy( vd.str, buf );
			        }
			    }
			    else errorAndExit( "add: no variable." );
			
			    return RET_CONTINUE;
			}
			
			public int dsoundCommand()
			{
			    //added to remove "unsupported command" warnings for 'dsound'
			    return RET_CONTINUE;
			}
		}
	}
}
