﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-29
 * Time: 9:34
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
		 *  ONScripterLabel_command.cpp - Command executer of ONScripter-EN
		 *
		 *  Copyright (c) 2001-2011 Ogapee. All rights reserved.
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
		
		// Modified by Haeleth, autumn 2006, to remove unnecessary diagnostics and support ADOS properly.
		
		// Modified by Mion, March 2008, to update from
		// Ogapee's 20080121 release source code.
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4996)
//		#pragma warning(disable:4244)
//		#endif
//		
//		#include "ONScripterLabel.h"
//		#include "version.h"
//		
//		#include <cstdio>
//		#include <sys/stat.h>
//		#include <sys/types.h>
//		#include <string.h>
//		#include <errno.h>
//		
//		#ifdef WIN32
//		#include <direct.h>
//		#include <windows.h>
//		#include "SDL_syswm.h"
//		#endif
//		
//		#ifdef MACOSX
//		#include "cocoa_url.h"
//		#include "cocoa_encoding.h"
//		#include "cocoa_alertbox.h"
//		#endif
//		
//		#ifdef LINUX
//		#include <sys/wait.h>
//		#include <unistd.h>
//		#include <errno.h>
//		#endif
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
		
		private const string DEFAULT_CURSOR_WAIT    = ":l/3,160,2;cursor0.bmp";
		private const string DEFAULT_CURSOR_NEWPAGE = ":l/3,160,2;cursor1.bmp";
		
//		#define CONTINUOUS_PLAY
//		
//		extern SDL_TimerID timer_bgmfade_id;
//		extern "C" Uint32 bgmfadeCallback( Uint32 interval, void *param );
		
		public partial class ONScripterLabel {
			public int yesnoboxCommand()
			{
			//Mion: Currently we only support dialog boxes on Windows and Mac OS X.
			//      Any ideas on making dialog boxes for Linux etc.?
			#if true//!MACOSX && !(WIN32 && USE_MESSAGEBOX)
			    if (!answer_dialog_with_yes_ok)
			        return RET_NOMATCH;
			#endif
			
			    bool is_yesnobox = false;
			    if (script_h.isName( "yesnobox" )){
			        fprintf(stderr,"yesnobox: ");
			        is_yesnobox = true;
			    }
			    else if (script_h.isName( "okcancelbox" ))
			        fprintf(stderr,"okcancelbox: ");
			    
			    if (is_yesnobox){;} //FIXME: not used
			    
			    script_h.readVariable();
			    script_h.pushVariable();
			    CharPtr buf = new CharPtr(script_h.readStr());
			    CharPtr msg = new char[strlen(buf)+1];
			    sprintf(msg,"%s",buf);
			    CharPtr title = new CharPtr(script_h.readStr());
			    int res = 1;
			    if(!answer_dialog_with_yes_ok) {
			        //The OS X dialog box routines are crashing when in fullscreen mode,
			        //so let's switch to windowed mode just in case
			        menu_windowCommand();
			#if MACOSX
			        bool selectedYes;
			        if(is_yesnobox) {
			            selectedYes = ONSCocoa::choicebox(title, msg, "Yes", "No", ONSCocoa::ENC_SJIS);
			        } else {
			            selectedYes = ONSCocoa::choicebox(title, msg, "OK", "Cancel", ONSCocoa::ENC_SJIS);
			        }
			
			        res = selectedYes ? 1 : 0;
			#elif WIN32 && USE_MESSAGEBOX
			        UINT mb_type = MB_OKCANCEL;
			        if (is_yesnobox)
			            mb_type = MB_YESNO;
			        HWND pwin = NULL;
			        SDL_SysWMinfo info;
			        SDL_VERSION(&info.version);
			        if (SDL_GetWMInfo(&info) == 1)
			            pwin = info.window;
			        res = MessageBox(pwin, msg, title, mb_type);
			        res = ((res == IDYES) || (res == IDOK)) ? 1 : 0;
			#endif
			    }
			    script_h.setInt( script_h.pushed_variable, res );
			    fprintf(stderr,"Got dialog '%s': '%s', returned value of %d\n",
			            title, msg, res);
			    msg = null;//delete[] msg;
			
			    return RET_CONTINUE;
			
			}
			
			public int waveCommand()
			{
				wave_play_loop_flag = false;
			
			    if (script_h.isName( "waveloop" ))
			        wave_play_loop_flag = true;
			
			    wavestopCommand();
			
			    setStr(ref wave_file_name, script_h.readStr());
			    playSound(wave_file_name, SOUND_WAVE|SOUND_OGG, wave_play_loop_flag, MIX_WAVE_CHANNEL);
			
			    return RET_CONTINUE;
			}
			
			public int wavestopCommand()
			{
				setStr( ref wave_file_name, null );
			
			    return RET_CONTINUE;
			}
			
			public int waittimerCommand()
			{
				int count = (int)(script_h.readInt() + internal_timer - SDL_GetTicks());
			    if (count < 0) count = 0;
			
			    event_mode = WAIT_TIMER_MODE;
			    waitEvent( count );
			
			    return RET_CONTINUE;
			}
			
			public int waitCommand()
			{
			//using insani's skippable wait concept (modified)
			    int count = script_h.readInt();
			
			    if( 0!=(skip_mode & (SKIP_NORMAL | SKIP_TO_WAIT)) || 0!=ctrl_pressed_status ) {
			        //Mion: instead of skipping entirely, let's do a shortened wait (safer)
			        if (count > 100) {
			            count = count / 10;
			        } else if (count > 10) {
			            count = 10;
			        }
			    }
			    if (count < 0) count = 0;
			
			    event_mode = WAIT_TIMER_MODE | WAIT_SLEEP_MODE;
			    waitEvent( count );
			
			    return RET_CONTINUE;
			}
			
			public int vspCommand()
			{
				leaveTextDisplayMode();
			
			    bool vsp2_flag = false;
			    if (script_h.isName("vsp2")) vsp2_flag = true;
			
			    int no = script_h.readInt();
			    int v  = script_h.readInt();
			
			    if (vsp2_flag){
			        sprite2_info[ no ].visible = (v==1)?true:false;
			        dirty_rect.add( sprite2_info[no].bounding_rect );
			    }
			    else {
			        sprite_info[ no ].visible = (v==1)?true:false;
			        dirty_rect.add( sprite_info[no].pos );
			        if ((v==0) && sprite_info[ no ].is_animatable){
			            sprite_info[ no ].current_cell = 0;
			            sprite_info[ no ].direction = 1;
			        }
			    }
			
			    return RET_CONTINUE;
			}
			
			public int voicevolCommand()
			{
				voice_volume = script_h.readInt();
			
			    channelvolumes[0] = voice_volume;
			
			    return RET_CONTINUE;
			}
			
			public int vCommand()
			{
				CharPtr buf = new char[256];
			
				sprintf(buf, "voice%c%s.wav", DELIMITER, new CharPtr(script_h.getStringBuffer(),+1));
			    playSound(buf, SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			
			    return RET_CONTINUE;
			}
			
			public int trapCommand()
			{
				if      ( script_h.isName( "lr_trap" ) ){
			        trap_mode = TRAP_LEFT_CLICK | TRAP_RIGHT_CLICK;
			    }
			    else if ( script_h.isName( "r_trap" ) ){
			        trap_mode = TRAP_RIGHT_CLICK;
			    }
			    else if ( script_h.isName( "trap" ) ){
			        trap_mode = TRAP_LEFT_CLICK;
			    }
			    else{
			        printf("trapCommand: cmd [%s] not recognized\n", script_h.getStringBuffer() );
			        trap_mode = TRAP_NONE;
			        return RET_CONTINUE;
			    }
			
			    if ( script_h.compareString("off") ){
			        script_h.readName();
			        trap_mode = TRAP_NONE;
			        return RET_CONTINUE;
			    }
			
				CharPtr buf = new CharPtr(script_h.readLabel());
			    if ( buf[0] == '*' ){
					setStr(ref trap_dest, new CharPtr(buf,+1));
			    }
			    else{
			        printf("trapCommand: [%s] is not supported\n", buf );
			    }
			
			    return RET_CONTINUE;
			}
			
			public int transbtnCommand()
			{
				transbtn_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int textspeeddefaultCommand()
			{
				sentence_font.wait_time = -1;
			
			    return RET_CONTINUE;
			}
			
			public int textspeedCommand()
			{
				sentence_font.wait_time = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int textshowCommand()
			{
				dirty_rect.fill( screen_width, screen_height );
			    refresh_window_text_mode = REFRESH_NORMAL_MODE | REFRESH_WINDOW_MODE | REFRESH_TEXT_MODE;
			    if (!setEffect(window_effect, true, false))
			        while(doEffect(window_effect, false));
			
			    return RET_CONTINUE;
			}
			
			public int textonCommand()
			{
				if (windowchip_sprite_no >= 0)
			        sprite_info[windowchip_sprite_no].visible = true;
			
			    enterTextDisplayMode();
			
			    text_on_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int textoffCommand()
			{
				if (windowchip_sprite_no >= 0)
			        sprite_info[windowchip_sprite_no].visible = false;
			
			    leaveTextDisplayMode(true);
			    refreshSurface(backup_surface, null, REFRESH_NORMAL_MODE);
			
			    text_on_flag = false;
			
			    return RET_CONTINUE;
			}
			
			public int texthideCommand()
			{
				dirty_rect.fill( screen_width, screen_height );
			    refresh_window_text_mode = REFRESH_NORMAL_MODE | REFRESH_WINDOW_MODE;
			    if (!setEffect(window_effect, true, true))
			        while(doEffect(window_effect, false));
			
			    return RET_CONTINUE;
			}
			
			public int textexbtnCommand()
			{
				int txtbtn_no = script_h.readInt();
				CharPtr buf = new CharPtr(script_h.readStr());
			
			    TextButtonInfoLink info = text_button_info.next;
			    TextButtonInfoLink found = null;
			    while (null!=info) {
			        if (info.no == txtbtn_no)
			            found = info;
			        info = info.next;
			    }
			    
			    if (null!=found) {
			        ButtonLink button = found.button;
			        while (null!=button) {
			            button.exbtn_ctl   = new char[ strlen( buf ) + 1 ];
			            strcpy( button.exbtn_ctl, buf );
			            button = button.same;
			        }
			    }
			    is_exbtn_enabled = true;
			
			    return RET_CONTINUE;
			}
			
			public int textclearCommand()
			{
				newPage( false );
			    return RET_CONTINUE;
			}
			
			public int textbtnstartCommand()
			{
				txtbtn_start_num = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int textbtnoffCommand()
			{
				txtbtn_show = false;
			
			    return RET_CONTINUE;
			}
			
			public int texecCommand()
			{
				if ( textgosub_clickstr_state == CLICK_NEWPAGE ){
			        newPage( true );
			        clickstr_state = CLICK_NONE;
			    }
			    else if ( textgosub_clickstr_state == CLICK_WAITEOL ){
			        if ( !sentence_font.isLineEmpty() && !new_line_skip_flag ){
			            indent_offset = 0;
			            line_enter_status = 0;
			            current_page.add( 0x0a );
			            sentence_font.newLine();
			        }
			    }
			
			    return RET_CONTINUE;
			}
			
			public int tateyokoCommand()
			{
				int mode = script_h.readInt();
			
			    //don't perform this command while in a textgosub
			    bool in_textgosub = false;
			    if (null!=textgosub_label) {
			        NestInfo tmp_nest = last_nest_info;
			        while (null!=tmp_nest) {
			            if (tmp_nest.textgosub_flag) {
			                in_textgosub = true;
			                break;
			            }
			            tmp_nest = tmp_nest.previous;
			        }
			    }
			
			    if (!in_textgosub)
			        sentence_font.setTateyokoMode( mode );
			    else
			        fprintf(stderr, "tateyoko: in textgosub, ignoring\n");
			
			    return RET_CONTINUE;
			}
			
			public int talCommand()
			{
				leaveTextDisplayMode();
			
			    char loc = script_h.readName()[0];
			    int no = -1, trans = 0;
			    if      ( loc == 'l' ) no = 0;
			    else if ( loc == 'c' ) no = 1;
			    else if ( loc == 'r' ) no = 2;
			
			    if (no >= 0){
			        trans = script_h.readInt();
			        if      (trans > 256) trans = 256;
			        else if (trans < 0  ) trans = 0;
			
			        tachi_info[ no ].trans = trans;
			        dirty_rect.add( tachi_info[ no ].pos );
			    }
			
			    EffectLink el = parseEffect(true);
			    if (setEffect(el, true, true)) return RET_CONTINUE;
			    while (doEffect(el));
			
			    return RET_CONTINUE;
			}
			
			public int tablegotoCommand()
			{
				int count = 0;
			    int no = script_h.readInt();
			
			    while( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) ){
			    	CharPtr buf = new CharPtr(script_h.readLabel());
			        if ( count++ == no ){
			    		setCurrentLabel( new CharPtr(buf,+1) );
			            break;
			        }
			    }
			
			    return RET_CONTINUE;
			}
			
			public int systemcallCommand()
			{
				system_menu_mode = getSystemCallNo( script_h.readName() );
			
			    //if (!rgosub_label)
			        executeSystemCall();
			    system_menu_mode = SYSTEM_NULL;
			
			    return RET_CONTINUE;
			}
			
			public int strspCommand()
			{
				leaveTextDisplayMode();
			
			    bool v=true;
			
			    if ( script_h.isName( "strsph" ) )
			        v = false;
			
			    int sprite_no = script_h.readInt();
			    AnimationInfo ai = sprite_info[sprite_no];
			    if ( ai.visible )
			        dirty_rect.add( ai.pos );
			    ai.remove();
			    setStr(ref ai.file_name, script_h.readStr());
			
			    Fontinfo fi = new Fontinfo();
			    fi.is_newline_accepted = true;
			    ai.orig_pos.x = script_h.readInt();
			    ai.orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(ai);
			    fi.num_xy[0] = script_h.readInt();
			    fi.num_xy[1] = script_h.readInt();
			    fi.font_size_xy[0] = script_h.readInt();
			    fi.font_size_xy[1] = script_h.readInt();
			    fi.pitch_xy[0] = script_h.readInt() + fi.font_size_xy[0];
			    fi.pitch_xy[1] = script_h.readInt() + fi.font_size_xy[1];
			    fi.is_bold = 0!=script_h.readInt()?true:false;
			    fi.is_shadow = 0!=script_h.readInt()?true:false;
			
			    CharPtr buffer = script_h.getNext();
			    while(0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA)){
			        ai.num_of_cells++;
			        script_h.readStr();
			    }
			    if (ai.num_of_cells == 0){
			        ai.num_of_cells = 1;
			        ai.color_list = new byte[ai.num_of_cells][];
			        for (int i = 0; i < ai.color_list.Length; ++i)
			        {
			        	ai.color_list[i] = new byte[3];
			        }
			        ai.color_list[0][0] = ai.color_list[0][1] = ai.color_list[0][2] = 0xff;
			    }
			    else{
			    	ai.color_list = new byte[ai.num_of_cells][];
			    	for (int i = 0; i < ai.color_list.Length; ++i)
			    	{
			    		ai.color_list[i] = new byte[3];
			    	}
			        script_h.setCurrent(buffer);
			        for (int i=0 ; i<ai.num_of_cells ; i++)
			            readColor(ref ai.color_list[i], readColorStr());
			    }
			
			    ai.trans_mode = AnimationInfo.TRANS_STRING;
			    ai.trans = 256;
			    ai.visible = v;
			    ai.is_single_line = false;
			    ai.is_tight_region = false;
			    setupAnimationInfo(ai, fi);
			    if ( ai.visible )
			        dirty_rect.add( ai.pos );
			
			    return RET_CONTINUE;
			}
			
			public int stopCommand()
			{
				wavestopCommand();
			    //NScr doesn't stop loopbgm w/this cmd
			    return mp3stopCommand();
			}
			
			public int sp_rgb_gradationCommand()
			{
				leaveTextDisplayMode();
			
			    int no = script_h.readInt();
			    int upper_r = script_h.readInt();
			    int upper_g = script_h.readInt();
			    int upper_b = script_h.readInt();
			    int lower_r = script_h.readInt();
			    int lower_g = script_h.readInt();
			    int lower_b = script_h.readInt();
			    UInt32 key_r = (UInt32)script_h.readInt();
			    UInt32 key_g = (UInt32)script_h.readInt();
			    UInt32 key_b = (UInt32)script_h.readInt();
			    UInt32 alpha = (UInt32)script_h.readInt();
			
			    AnimationInfo si=null;
			    if (no == -1) si = sentence_font_info;
			    else          si = sprite_info[no];
			    SDL_Surface surface = si.image_surface;
			    if (surface == null) return RET_CONTINUE;
			
			    SDL_PixelFormat fmt = SDL_Surface_get_format(surface);
			
			    UInt32 key_mask = (key_r >> fmt.Rloss) << fmt.Rshift |
			        (key_g >> fmt.Gloss) << fmt.Gshift |
			        (key_b >> fmt.Bloss) << fmt.Bshift;
			    UInt32 rgb_mask = fmt.Rmask | fmt.Gmask | fmt.Bmask;
			
			    SDL_LockSurface(surface);
			    // check upper and lower bound
			    int i, j;
			    int upper_bound=0, lower_bound=0;
			    bool is_key_found = false;
			    for (i=0 ; i<SDL_Surface_get_h(surface) ; i++){
			    	Uint32Ptr buf = new Uint32Ptr(SDL_Surface_get_pixels(surface), + SDL_Surface_get_w(surface) * i);
			    	for (j=0 ; j < SDL_Surface_get_w(surface) ; j++, buf.inc()){
			        	if ((buf[0] & rgb_mask) == key_mask){
			                if (is_key_found == false){
			                    is_key_found = true;
			                    upper_bound = lower_bound = i;
			                }
			                else{
			                    lower_bound = i;
			                }
			                break;
			            }
			        }
			    }
			
			    // replace pixels of the key-color with the specified color in gradation
			    for (i=upper_bound ; i<=lower_bound ; i++){
			    	Uint32Ptr buf = new Uint32Ptr(SDL_Surface_get_pixels(surface), + SDL_Surface_get_w(surface) * i);
			#if BPP16
			        unsigned char *alphap = si->alpha_buf + SDL_Surface_get_w(surface) * i;
			#else
			#if true//SDL_BYTEORDER == SDL_LIL_ENDIAN
					UnsignedCharPtr alphap = new UnsignedCharPtr(buf, + 3);
			#else
			        unsigned char *alphap = (unsigned char *)buf;
			#endif
			#endif
			        UInt32 color = alpha << SDL_Surface_get_format(surface).Ashift;
			        if (upper_bound != lower_bound){
			        	color |= (uint)((((lower_r - upper_r) * (i-upper_bound) / (lower_bound - upper_bound) + upper_r) >> fmt.Rloss) << fmt.Rshift);
    	                color |= (uint)((((lower_g - upper_g) * (i-upper_bound) / (lower_bound - upper_bound) + upper_g) >> fmt.Gloss) << fmt.Gshift);
    	                color |= (uint)((((lower_b - upper_b) * (i-upper_bound) / (lower_bound - upper_bound) + upper_b) >> fmt.Bloss) << fmt.Bshift);
			        }
			        else{
			            color |= (uint)((upper_r >> fmt.Rloss) << fmt.Rshift);
			            color |= (uint)((upper_g >> fmt.Gloss) << fmt.Gshift);
			        	color |= (uint)((upper_b >> fmt.Bloss) << fmt.Bshift);
			        }
			
			        for (j=0 ; j < SDL_Surface_get_w(surface) ; j++, buf.inc()){
			        	if ((buf[0] & rgb_mask) == key_mask){
			                buf[0] = color;
			                alphap[0] = (byte)alpha;
			            }
			#if BPP16
			            alphap++;
			#else
						alphap.inc(4);
			#endif
			        }
			    }
			
			    SDL_UnlockSurface(surface);
			
			    if ( si.visible )
			        dirty_rect.add( si.pos );
			
			    return RET_CONTINUE;
			}
			
			public int spstrCommand()
			{
				decodeExbtnControl( script_h.readStr() );
			
			    return RET_CONTINUE;
			}
			
			public int spreloadCommand()
			{
				leaveTextDisplayMode();
			
			    int no = script_h.readInt();
			    AnimationInfo si;
			    if (no == -1) si = sentence_font_info;
			    else          si = sprite_info[no];
			
			    parseTaggedString( si );
			    setupAnimationInfo( si );
			
			    if ( si.visible )
			        dirty_rect.add( si.pos );
			
			    return RET_CONTINUE;
			}
			
			public int splitCommand()
			{
				script_h.readStr();
				CharPtr save_buf = new CharPtr(script_h.saveStringBuffer());
			
			    char delimiter = script_h.readStr()[0];
			
			    CharPtr token = new char[strlen(save_buf)+1];
			    while( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) ){
			
			        uint c=0;
			        while(save_buf[c] != delimiter && save_buf[c] != '\0'){
			            if (IS_TWO_BYTE(save_buf[c]))
			                c += 2;
			            else
			                c++;
			        }
			        memcpy( token, save_buf, c );
			        token[c] = '\0';
			
			        script_h.readVariable();
			        if ( 0!=(script_h.current_variable.type & ScriptHandler.VAR_INT) ||
			             0!=(script_h.current_variable.type & ScriptHandler.VAR_ARRAY) ){
			            script_h.setInt( script_h.current_variable, atoi(token) );
			        }
			        else if ( 0!=(script_h.current_variable.type & ScriptHandler.VAR_STR) ){
			            setStr( ref script_h.getVariableData(script_h.current_variable.var_no).str, token );
			        }
			
			        save_buf.inc((int)c);
			        if (save_buf[0] != '\0') save_buf.inc();
			    }
			    token = null;//delete[] token;
			
			    return RET_CONTINUE;
			}
			
			public int spclclkCommand()
			{
				if ( !force_button_shortcut_flag )
			        spclclk_flag = true;
			    return RET_CONTINUE;
			}
			
			public int spbtnCommand()
			{
				bool cellcheck_flag = false;
			
			    if ( script_h.isName( "cellcheckspbtn" ) )
			        cellcheck_flag = true;
			
			    int sprite_no = script_h.readInt();
			    int no        = script_h.readInt();
			
			    if ( cellcheck_flag ){
			        if ( sprite_info[ sprite_no ].num_of_cells < 2 ) return RET_CONTINUE;
			    }
			    else{
			        if ( sprite_info[ sprite_no ].num_of_cells == 0 ) return RET_CONTINUE;
			    }
			
			    ButtonLink button = new ButtonLink();
			    root_button_link.insert( button );
			
			    button.button_type = ButtonLink.BUTTON_TYPE.SPRITE_BUTTON;
			    button.sprite_no   = sprite_no;
			    button.no          = no;
			
			    if ( null!=sprite_info[ sprite_no ].image_surface ||
			         sprite_info[ sprite_no ].trans_mode == AnimationInfo.TRANS_STRING )
			    {
			        button.image_rect = button.select_rect = sprite_info[ sprite_no ].pos;
			    }
			
			    return RET_CONTINUE;
			}
			
			public int skipoffCommand()
			{
				skip_mode &= ~SKIP_NORMAL;
			
			    return RET_CONTINUE;
			}
			
			#if LINUX
			int tryToLaunch(const char* command, const char* target)
			{
			    if (!command || !target) return -1;
			    pid_t child = vfork();
			    if (child == -1) {
				// Parent, failed
				fprintf(stderr, "Could not open `%s': fork error: %s\n",
					target, strerror(errno));
				return 0;
			    }
			    else if (child) {
				// Parent, success
				int status;
				waitpid(child, &status, 0);
				if (WIFEXITED(status)) return WEXITSTATUS(status);
				return -1;
			    }
			    else {
				// Child
				execlp(command, command, target, (char*) NULL);
				_exit(255);
			    }
			}
			#endif
			
			public int shellCommand()
			{
			#if true //def WIN32
			    CharPtr url = new CharPtr(script_h.readStr());
//			    HMODULE shdll = LoadLibrary("shell32");
//			    if (shdll) {
//			        typedef HINSTANCE (WINAPI *SHELLEXECUTE)(HWND, LPCSTR, LPCSTR, LPCSTR,
//									 LPCSTR, int);
//			        SHELLEXECUTE shexec =
//				    SHELLEXECUTE(GetProcAddress(shdll, "ShellExecuteA"));
//			    	if (shexec) {
			            ShellExecuteA(null, "open", url, null, null, SW_SHOW);
//			        }
//			        FreeLibrary(shdll);
//			    }
			    
			#elif MACOSX
			    ONSCocoa::open_url(script_h.readStr(), ONSCocoa::ENC_SJIS);
			#elif LINUX
			    // Linux/BSD/other Unixes don't provide standard APIs for this
			    // kind of thing, but there are various things we can try.
			    
			    const char *url = script_h.readStr();
			    int status;
			
			    // First up is xdg-open, the freedesktop solution that's becoming
			    // standard in the Linux world, at least.
			    //
			    status = tryToLaunch("xdg-open", url);
			    switch (status) {
			    case 0: // Success
				return RET_CONTINUE;
			
			    case 2: // File not found
				fprintf(stderr, "Failed to open %s: xdg-error reports that it doesn't "
					"exist\n", url);
				return RET_CONTINUE;
			
			    case 255: // execlp() failed (e.g. xdg-open not present)
				// Don't say anything.
				break;
				
			    case 1:   // Syntax error
			    case 3:   // Required tool missing
			    case 4:   // Action failed
			    case -1:  // child didn't exit normally
			    default:  // unknown problem
				fprintf(stderr, "Open URL with xdg-open failed with status %d\n",
					status);
			    }
			
			    // Failing that, try $BROWSER, or give up.
			    const char* browser = getenv("BROWSER");
			    if (browser) {
				char* cmd = new char[strlen(browser) + strlen(url) + 8];
				sprintf(cmd, "\"%s\" '%s' &", browser, url);
				if (system(cmd) != 0)
			            fprintf(stderr, "Couldn't launch web browser `%s': check your "
			                    "BROWSER setting.\n", browser);
				delete[] cmd;
			    }
			    else {
				fputs("Could not determine which web browser to use. "
				      "Please set BROWSER appropriately.\n", stderr);
			    }
			    
			#else
			    fprintf(stderr, "[shell] command not supported for this OS\n");
			#endif
			
			    return RET_CONTINUE;
			}
			
			public int sevolCommand()
			{
				se_volume = script_h.readInt();
			
			    for ( int i=1 ; i<ONS_MIX_CHANNELS ; i++ ) {
			        channelvolumes[i] = se_volume;
			    }
			
			    return RET_CONTINUE;
			}
			
			public void setwindowCore()
			{
			    sentence_font.ttf_font  = null;
			    sentence_font.top_xy[0] = script_h.readInt();
			    sentence_font.top_xy[1] = script_h.readInt();
			    sentence_font.num_xy[0] = script_h.readInt();
			    sentence_font.num_xy[1] = script_h.readInt();
			    sentence_font.font_size_xy[0] = script_h.readInt();
			    sentence_font.font_size_xy[1] = script_h.readInt();
			    sentence_font.pitch_xy[0] = script_h.readInt() + sentence_font.font_size_xy[0];
			    sentence_font.pitch_xy[1] = script_h.readInt() + sentence_font.font_size_xy[1];
			    sentence_font.wait_time = script_h.readInt();
			    sentence_font.is_bold = 0!=script_h.readInt()?true:false;
			    sentence_font.is_shadow = 0!=script_h.readInt()?true:false;
			
			    bool is_color = false;
			    CharPtr buf;
			    if (allow_color_type_only) {
			    	buf = new CharPtr(script_h.readColor(ref is_color, true));
			        if (!is_color)
			        	buf = new CharPtr(script_h.readStr());
			    } else {
			    	buf = new CharPtr(script_h.readStr());
			        if ( buf[0] == '#' ) is_color = true;
			    }
			    dirty_rect.add( sentence_font_info.pos );
			    if ( is_color ){
			        sentence_font.is_transparent = true;
			        readColor( ref sentence_font.window_color, buf );
			
			        sentence_font_info.orig_pos.x = script_h.readInt();
			        sentence_font_info.orig_pos.y = script_h.readInt();
			        sentence_font_info.orig_pos.w = script_h.readInt() - sentence_font_info.orig_pos.x + 1;
			        sentence_font_info.orig_pos.h = script_h.readInt() - sentence_font_info.orig_pos.y + 1;
			        UpdateAnimPosStretchXY(sentence_font_info);
			        UpdateAnimPosStretchWH(sentence_font_info);
			    }
			    else{
			        sentence_font.is_transparent = false;
			        sentence_font_info.setImageName( buf );
			        parseTaggedString( sentence_font_info );
			#if RCA_SCALE
			        setupAnimationInfo( sentence_font_info, null, scr_stretch_x, scr_stretch_y );
			#else
			        setupAnimationInfo( sentence_font_info );
			#endif
			        sentence_font_info.orig_pos.x = script_h.readInt();
			        sentence_font_info.orig_pos.y = script_h.readInt();
			        UpdateAnimPosStretchXY(sentence_font_info);
			        sentence_font.window_color[0] = sentence_font.window_color[1] = sentence_font.window_color[2] = 0xff;
			    }
			#if RCA_SCALE
			    // Scale and reposition window size if screen is bigger than game
			    if (scr_stretch_x > 1.0) {
			        sentence_font.top_xy[0] = sentence_font.top_xy[0] * scr_stretch_x + 0.5;
			#   if true
			        // Either increase number of characters on screen
			        sentence_font.num_xy[0] = sentence_font.num_xy[0] * scr_stretch_x + 0.5;
			#   else
			        // Or increase font size
			        sentence_font.pitch_xy[0] = sentence_font.pitch_xy[0] * scr_stretch_x + 0.5;
			        sentence_font.font_size_xy[0] = sentence_font.font_size_xy[0] * scr_stretch_x + 0.5;
			#   endif
			    }
			    if (scr_stretch_y > 1.0) {
			        sentence_font.top_xy[1] = sentence_font.top_xy[1] * scr_stretch_y + 0.5;
			#   if true
			        // Either increase number of characters on screen
			        sentence_font.num_xy[1] = sentence_font.num_xy[1] * scr_stretch_y + 0.5;
			#   else
			        // Or increase font size
			        sentence_font.pitch_xy[1] = sentence_font.pitch_xy[1] * scr_stretch_y + 0.5;
			        sentence_font.font_size_xy[1] = sentence_font.font_size_xy[1] * scr_stretch_y + 0.5;
			#   endif
			    }
			#endif //RCA_SCALE
			}
			
			public int setwindow3Command()
			{
				setwindowCore();
			
			    display_mode = DISPLAY_MODE_NORMAL;
			    flush( refreshMode(), sentence_font_info.pos );
			
			    return RET_CONTINUE;
			}
			
			public int setwindow2Command()
			{
				bool is_color = false;
			    CharPtr buf;
			    if (allow_color_type_only) {
			    	buf = new CharPtr(script_h.readColor(ref is_color, true));
			        if (!is_color)
			        	buf = new CharPtr(script_h.readStr());
			    } else {
			    	buf = new CharPtr(script_h.readStr());
			        if ( buf[0] == '#' ) is_color = true;
			    }
			    if ( is_color ){
			        sentence_font.is_transparent = true;
			        readColor( ref sentence_font.window_color, buf );
			    }
			    else{
			        sentence_font.is_transparent = false;
			        sentence_font_info.setImageName( buf );
			        parseTaggedString( sentence_font_info );
			        setupAnimationInfo( sentence_font_info );
			    }
			    repaintCommand();
			
			    return RET_CONTINUE;
			}
			
			public int setwindowCommand()
			{
				setwindowCore();
			
			    lookbackflushCommand();
			    indent_offset = 0;
			    line_enter_status = 0;
			    page_enter_status = 0;
			    display_mode = DISPLAY_MODE_NORMAL;
			    flush( refreshMode(), sentence_font_info.pos );
			
			    return RET_CONTINUE;
			}
			
			public int seteffectspeedCommand()
			{
				int no = script_h.readInt();
			
			    effectspeed = EFFECTSPEED_NORMAL;
			    if (no == 1)
			        effectspeed = EFFECTSPEED_QUICKER;
			    else if (no == 2)
			        effectspeed = EFFECTSPEED_INSTANT;
			
			    return RET_CONTINUE;
			}
			
			public int setcursorCommand()
			{
				bool abs_flag;
			
			    if ( script_h.isName( "abssetcursor" ) ){
			        abs_flag = true;
			    }
			    else{
			        abs_flag = false;
			    }
			
			    int no = script_h.readInt();
			    script_h.readStr();
			    CharPtr buf = new CharPtr(script_h.saveStringBuffer());
			    int x = script_h.readInt();
			    int y = script_h.readInt();
			
			    loadCursor( no, buf, x, y, abs_flag );
			
			    return RET_CONTINUE;
			}
			
			public int selectCommand()
			{
				enterTextDisplayMode();
			
			    int select_mode = SELECT_GOTO_MODE;
			    SelectLink last_select_link = new SelectLink();
			
			    if ( script_h.isName( "selnum" ) )
			        select_mode = SELECT_NUM_MODE;
			    else if ( script_h.isName( "selgosub" ) )
			        select_mode = SELECT_GOSUB_MODE;
			    else if ( script_h.isName( "select" ) )
			        select_mode = SELECT_GOTO_MODE;
			    else if ( script_h.isName( "csel" ) )
			        select_mode = SELECT_CSEL_MODE;
			
			    if ( select_mode == SELECT_NUM_MODE ){
			        script_h.readVariable();
			        script_h.pushVariable();
			    }
			
			    bool comma_flag = true;
			    if ( select_mode == SELECT_CSEL_MODE ){
			        saveoffCommand();
			    }
			    shortcut_mouse_line = -1;
			
			    int[] xy = new int[2];
			    xy[0] = sentence_font.xy[0];
			    xy[1] = sentence_font.xy[1];
			
			    if ( null!=selectvoice_file_name[SELECTVOICE_OPEN] )
			        playSound(selectvoice_file_name[SELECTVOICE_OPEN],
			                  SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL );
			
			    last_select_link = root_select_link;
			
			    while(true){
			        if ( script_h.getNext()[0] != 0x0a && comma_flag == true ){
			
			    		CharPtr buf = new CharPtr(script_h.readStr());
			    		comma_flag = (0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA));
			            if ( select_mode != SELECT_NUM_MODE && !comma_flag )
			                errorAndExit( "select: missing comma." );
			
			            // Text part
			            SelectLink slink = new SelectLink();
			            setStr( ref slink.text, buf );
			            //printf("Select text %s\n", slink->text);
			
			            // Label part
			            if (select_mode != SELECT_NUM_MODE){
			                script_h.readLabel();
			                setStr( ref slink.label, new CharPtr(script_h.getStringBuffer(),+1) );
			                //printf("Select label %s\n", slink->label );
			            }
			            last_select_link.next = slink;
			            last_select_link = last_select_link.next;
			
			            comma_flag = (0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA));
			            //printf("2 comma %d %c %x\n", comma_flag, script_h.getCurrent()[0], script_h.getCurrent()[0]);
			        }
			        else if (script_h.getNext()[0] == 0x0a){
			            //printf("comma %d\n", comma_flag);
			            CharPtr buf = new CharPtr(script_h.getNext(), + 1); // consume eol
			            while ( buf[0] == ' ' || buf[0] == '\t' ) buf.inc();
			
			            if (comma_flag && buf[0] == ',')
			                errorAndExit( "select: double comma." );
			
			            bool comma2_flag = false;
			            if (buf[0] == ','){
			                comma2_flag = true;
			                buf.inc();
			                while ( buf[0] == ' ' || buf[0] == '\t' ) buf.inc();
			            }
			            script_h.setCurrent(buf);
			
			            if (buf[0] == 0x0a){
			                comma_flag |= comma2_flag;
			                continue;
			            }
			
			            if (!comma_flag && !comma2_flag){
			                select_label_info.next_script = buf;
			                //printf("select: stop at the end of line\n");
			                break;
			            }
			
			            //printf("continue\n");
			            comma_flag = true;
			        }
			        else{ // if select ends at the middle of the line
			            select_label_info.next_script = script_h.getNext();
			            //printf("select: stop at the middle of the line\n");
			            break;
			        }
			    }
			
			    if ( select_mode != SELECT_CSEL_MODE ){
			        last_select_link = root_select_link.next;
			        int counter = 1;
			        while( null!=last_select_link ){
			            if ( 0!=last_select_link.text[0] ){
			                ButtonLink button = getSelectableSentence( last_select_link.text, sentence_font );
			                root_button_link.insert( button );
			                button.no = counter;
			            }
			            counter++;
			            last_select_link = last_select_link.next;
			        }
			    }
			
			    if ( select_mode == SELECT_CSEL_MODE ){
			        setCurrentLabel( "customsel" );
			        return RET_CONTINUE;
			    }
			    skip_mode &= ~SKIP_NORMAL;
			    automode_flag = false;
			    sentence_font.xy[0] = xy[0];
			    sentence_font.xy[1] = xy[1];
			
			    flush( refreshMode() );
			
			    refreshMouseOverButton();
			
			    bool actual_rmode = rmode_flag;
			    rmode_flag = false;
			    event_mode = WAIT_TEXT_MODE | WAIT_BUTTON_MODE | WAIT_TIMER_MODE;
			    do {
			        waitEvent(-1);
			    } while ( !current_button_state.valid_flag ||
			            (current_button_state.button <= 0) );
			    rmode_flag = actual_rmode;
			
			    if ( null!=selectvoice_file_name[SELECTVOICE_SELECT] )
			        playSound(selectvoice_file_name[SELECTVOICE_SELECT],
			                  SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL );
			
			    deleteButtonLink();
			
			    int counter_ = 1;
			    last_select_link = root_select_link.next;
			    while ( null!=last_select_link ){
			        if ( current_button_state.button == counter_++ ) break;
			        last_select_link = last_select_link.next;
			    }
			
			    if ( select_mode  == SELECT_GOTO_MODE ){
			        setCurrentLabel( last_select_link.label );
			    }
			    else if ( select_mode == SELECT_GOSUB_MODE ){
			        gosubReal( last_select_link.label, select_label_info.next_script );
			    }
			    else{ // selnum
			        script_h.setInt( script_h.pushed_variable, current_button_state.button - 1 );
			        current_label_info = script_h.getLabelByAddress( select_label_info.next_script );
			        current_line = script_h.getLineByAddress( select_label_info.next_script );
			        script_h.setCurrent( select_label_info.next_script );
			    }
			    deleteSelectLink();
			
			    newPage( true );
			
			    return RET_CONTINUE;
			}
			
			public int savetimeCommand()
			{
				int no = script_h.readInt();
			
				SaveFileInfo info = new SaveFileInfo();
			    searchSaveFile( info, no );
			
			    script_h.readVariable();
			    if ( !info.valid ){
			        script_h.setInt( script_h.current_variable, 0 );
			        for ( int i=0 ; i<3 ; i++ )
			            script_h.readVariable();
			        return RET_CONTINUE;
			    }
			
			    script_h.setInt( script_h.current_variable, info.month );
			    script_h.readInt();
			    script_h.setInt( script_h.current_variable, info.day );
			    script_h.readInt();
			    script_h.setInt( script_h.current_variable, info.hour );
			    script_h.readInt();
			    script_h.setInt( script_h.current_variable, info.minute );
			
			    return RET_CONTINUE;
			}
			
			public int savescreenshotCommand()
			{
				if      ( script_h.isName( "savescreenshot" ) ){
			    }
			    else if ( script_h.isName( "savescreenshot2" ) ){
			    }
			
				CharPtr buf = new CharPtr(script_h.readStr());
			    CharPtr filename = new char[4096];
			
			    CharPtr ext = strrchr( buf, '.' );
			    if ( null!=ext && (0==strcmp( new CharPtr(ext,+1), "BMP" ) || 0==strcmp( new CharPtr(ext,+1), "bmp" ) ) ){
			        sprintf( filename, "%s%s", script_h.save_path, buf );
			        int last_delim = 0;
			        for ( uint i=0 ; i<strlen( filename ) ; i++ ) {
			            if ( filename[i] == '/' || filename[i] == '\\' ) {
			                filename[i] = DELIMITER;
			                last_delim = (int)i;
			            }
			        }
			        if (0!=last_delim) {
			        	filename[last_delim] = (char)0;
			            mkdir(filename
			#if WIN32
			                  , 0755
			#endif
			                 );
			            filename[last_delim] = DELIMITER;
			        }
			        if ( screenshot_surface == null ) {
			            printf("savescreenshot: no screenshot buffer, creating a blank 1x1 surface.\n");
			            screenshot_surface = SDL_CreateRGBSurface( SDL_SWSURFACE, 1, 1, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000 );
			        }
			
			        SDL_SaveBMP( screenshot_surface, filename );
			    }
			    else
			        printf("savescreenshot: file %s is not supported.\n", buf );
			
			    return RET_CONTINUE;
			}
			
			public int saveonCommand()
			{
				saveon_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int saveoffCommand()
			{
				saveon_flag = false;
			
			    return RET_CONTINUE;
			}
			
			public int savegameCommand()
			{
				bool savegame2_flag = false;
			    if ( script_h.isName( "savegame2" ) )
			        savegame2_flag = true;
			    
			    int no = script_h.readInt();
			
			    CharPtr savestr = null;
			    if (savegame2_flag)
			    	savestr = new CharPtr(script_h.readStr());
			
			    if ( no < 0 )
			        errorAndExit("savegame: save number is less than 0.");
			    else
			        saveSaveFile( no, savestr ); 
			
			    return RET_CONTINUE;
			}
			
			public int savefileexistCommand()
			{
				script_h.readInt();
			    script_h.pushVariable();
			    int no = script_h.readInt();
			
			    SaveFileInfo info = new SaveFileInfo();
			    searchSaveFile( info, no );
			
			    script_h.setInt( script_h.pushed_variable, (info.valid==true)?1:0 );
			
			    return RET_CONTINUE;
			}
			
			public int rndCommand()
			{
				int upper, lower;
			
			    if ( script_h.isName( "rnd2" ) ){
			        script_h.readInt();
			        script_h.pushVariable();
			
			        lower = script_h.readInt();
			        upper = script_h.readInt();
			    }
			    else{
			        script_h.readInt();
			        script_h.pushVariable();
			
			        lower = 0;
			        upper = script_h.readInt() - 1;
			    }
			
			    script_h.setInt( script_h.pushed_variable, lower + (int)( (double)(upper-lower+1)*rand()/(RAND_MAX+1.0)) );
			
			    return RET_CONTINUE;
			}
			
			public int rmodeCommand()
			{
				if ( script_h.readInt() == 1 ) rmode_flag = true;
			    else                           rmode_flag = false;
			
			    return RET_CONTINUE;
			}
			
			public int resettimerCommand()
			{
				internal_timer = SDL_GetTicks();
			
			    return RET_CONTINUE;
			}
			
			public int resetCommand()
			{
				//clear out the event queue
				SDL_Event event_ = new SDL_Event();
			    while( 0!=SDL_PollEvent( event_ ) )
			        if (event_.type == SDL_QUIT) endCommand();
			
			    int fadeout = (int)mp3fadeout_duration;
			    mp3fadeout_duration = 0; //don't use fadeout during a reset
			    int effect = window_effect.effect;
			    int duration = window_effect.duration;
			    window_effect.effect = 1; //don't use window effect during a reset
			    window_effect.duration = 0;
			    resetSub();
			    window_effect.effect = effect;
			    window_effect.duration = duration;
			    //reopen the audio mixer with default settings, if needed
			    mp3fadeout_duration = (uint)fadeout;
			    clearCurrentPage();
			    string_buffer_offset = 0;
			
			    setCurrentLabel( "start" );
			    saveSaveFile(-1);
			
			    return RET_CONTINUE;
			}
			
			public int repaintCommand()
			{
				dirty_rect.fill( screen_width, screen_height );
			    flush( refreshMode() );
			
			    return RET_CONTINUE;
			}
			
			public int quakeCommand()
			{
				int quake_type;
			
			    if      ( script_h.isName( "quakey" ) ){
			        quake_type = 0;
			    }
			    else if ( script_h.isName( "quakex" ) ){
			        quake_type = 1;
			    }
			    else{
			        quake_type = 2;
			    }
			
			    tmp_effect.no       = script_h.readInt();
			    tmp_effect.duration = script_h.readInt();
			
			    //insani's skippable quake
			    if ( 0!=ctrl_pressed_status )
			    {
			        dirty_rect.fill( screen_width, screen_height );
			        SDL_BlitSurface( accumulation_surface, null, effect_dst_surface, null );
			        event_mode = IDLE_EVENT_MODE;
			        return RET_CONTINUE;
			    }
			
			    if ( tmp_effect.duration < tmp_effect.no * 4 ) tmp_effect.duration = tmp_effect.no * 4;
			    tmp_effect.effect   = CUSTOM_EFFECT_NO + quake_type;
			
			    dirty_rect.fill( screen_width, screen_height );
			    SDL_BlitSurface( accumulation_surface, null, effect_dst_surface, null );
			
			    if (setEffect(tmp_effect, true, true)) return RET_CONTINUE;
			    while (doEffect(tmp_effect));
			
			    return RET_CONTINUE;
			}
			
			public int puttextCommand()
			{
				enterTextDisplayMode(false);
			
			    script_h.readStr();
			
			    string_buffer_offset = 0;
			    if (0!=(script_h.getEndStatus() & ScriptHandler.END_1BYTE_CHAR))
			        string_buffer_offset = 1; // skip the heading `
			
			    while(processText());
			    processEOT();
			
			    return RET_CONTINUE;
			}
			
			public int prnumclearCommand()
			{
				if (system_menu_mode == SYSTEM_NULL)
			        leaveTextDisplayMode();
			
			    for ( int i=0 ; i<MAX_PARAM_NUM ; i++ ) {
			        if ( null!=prnum_info[i] ) {
			            dirty_rect.add( prnum_info[i].pos );
			            //delete prnum_info[i];
			            prnum_info[i] = null;
			        }
			    }
			    return RET_CONTINUE;
			}
			
			public int prnumCommand()
			{
				leaveTextDisplayMode();
			
			    int no = script_h.readInt();
			    if ( null!=prnum_info[no] ){
			        dirty_rect.add( prnum_info[no].pos );
			        prnum_info[no] = null;//delete prnum_info[no];
			    }
			    prnum_info[no] = new AnimationInfo();
			    prnum_info[no].trans_mode = AnimationInfo.TRANS_STRING;
			    prnum_info[no].num_of_cells = 1;
			    prnum_info[no].setCell(0);
			    prnum_info[no].color_list = new byte[ prnum_info[no].num_of_cells ][];
			    for (int i = 0; i < prnum_info[no].color_list.Length; ++i)
			    {
			    	prnum_info[no].color_list[i] = new byte[3];
			    }
			
			    prnum_info[no].param = script_h.readInt();
			    prnum_info[no].orig_pos.x = script_h.readInt();
			    prnum_info[no].orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(prnum_info[no]);
			    prnum_info[no].font_size_xy[0] = script_h.readInt();
			    prnum_info[no].font_size_xy[1] = script_h.readInt();
			
			    CharPtr buf = new CharPtr(readColorStr());
			    readColor( ref prnum_info[no].color_list[0], buf );
			
			    CharPtr num_buf = new char[7];
			    // Use fullwidth digits
			    script_h.getStringFromInteger( num_buf, prnum_info[no].param, 3, false, true );
			    setStr( ref prnum_info[no].file_name, num_buf );
			
			    setupAnimationInfo( prnum_info[no] );
			    dirty_rect.add( prnum_info[no].pos );
			
			    return RET_CONTINUE;
			}
			
			public int printCommand()
			{
				EffectLink el = parseEffect(true);
			
			    if (setEffect(el, true, true)) return RET_CONTINUE;
			    while (doEffect(el));
			
			    return RET_CONTINUE;
			}
			
			public int playstopCommand()
			{
				stopBGM( false );
			    return RET_CONTINUE;
			}
			
			public int playCommand()
			{
				bool loop_flag = true;
			    if ( script_h.isName( "playonce" ) )
			        loop_flag = false;
			
			    CharPtr buf = new CharPtr(script_h.readStr());
			    if ( buf[0] == '*' ){
			        cd_play_loop_flag = loop_flag;
			        int new_cd_track = atoi( new CharPtr(buf, + 1) );
			#if CONTINUOUS_PLAY
			        if ( current_cd_track != new_cd_track ) {
			#endif
			            stopBGM( false );
			            current_cd_track = new_cd_track;
			            playCDAudio();
			#if CONTINUOUS_PLAY
			        }
			#endif
			    }
			    else{ // play MIDI
			        stopBGM( false );
			
			        setStr(ref seqmusic_file_name, buf);
			        seqmusic_play_loop_flag = loop_flag;
			        if (playSound(seqmusic_file_name, SOUND_SEQMUSIC, seqmusic_play_loop_flag) != SOUND_SEQMUSIC){
			            fprintf(stderr, "can't play sequenced music file %s\n", seqmusic_file_name);
			        }
			    }
			
			    return RET_CONTINUE;
			}
			
			public int ofscopyCommand()
			{
				SDL_BlitSurface( screen_surface, null, accumulation_surface, null );
			
			    return RET_CONTINUE;
			}
			
			public int negaCommand()
			{
				nega_mode = script_h.readInt();
			
			    dirty_rect.fill( screen_width, screen_height );
			
			    return RET_CONTINUE;
			}
			
			public int mvCommand()
			{
				CharPtr buf = new char[256];
			
				sprintf(buf, "voice%c%s.mp3", DELIMITER, new CharPtr(script_h.getStringBuffer(),+2));
			
			    setStr(ref music_file_name, buf);
			
			    //don't bother with playback or fadeins if there's no audio
			    if ( !audio_open_flag ) return RET_CONTINUE;
			
			    int tmp = music_volume;
			    if (mp3fadein_duration > 0) {
			        music_volume = 0;
			    }
			
			    playSound(buf, SOUND_MP3, false, MIX_BGM_CHANNEL);
			
			    if (mp3fadein_duration > 0) {
			        // do a bgm fadein
			        music_volume = tmp;
			        mp3fade_start = SDL_GetTicks();
			        timer_bgmfade_id = SDL_AddTimer(20, bgmfadeCallback,
			                                        /*(void*)&*/timer_bgmfade_id); //FIXME:???
			        event_mode = WAIT_TIMER_MODE;
			        waitEvent(-1);
			    }
			
			    return RET_CONTINUE;
			}
			
			public int mspCommand()
			{
				leaveTextDisplayMode();
			
			    bool msp2_flag = false;
			    if (script_h.isName("msp2")) msp2_flag = true;
			
			    int no = script_h.readInt();
			
			    AnimationInfo si=null;
			    if (msp2_flag) {
			        si = sprite2_info[no];
			        dirty_rect.add( si.bounding_rect );
			    }
			    else{
			        si = sprite_info[no];
			        dirty_rect.add( si.pos );
			    }
			
			    int dx = script_h.readInt();
			    int dy = script_h.readInt();
			    si.orig_pos.x += dx;
			    si.orig_pos.y += dy;
			    UpdateAnimPosStretchXY(si);
			
			    if (msp2_flag){
			        si.scale_x += script_h.readInt();
			        si.scale_y += script_h.readInt();
			        si.rot += script_h.readInt();
			        si.calcAffineMatrix();
			        dirty_rect.add( si.bounding_rect );
			    }
			    else{
			        dirty_rect.add( si.pos );
			    }
			    
			    if ( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) )
			        si.trans += script_h.readInt();
			    if ( si.trans > 256 ) si.trans = 256;
			    else if ( si.trans < 0 ) si.trans = 0;
			    if ( si.is_animatable ) advanceAnimPhase();
			
			    return RET_CONTINUE;
			}
			
			public int mp3volCommand()
			{
				music_volume = script_h.readInt();
			
			    setCurMusicVolume(music_volume);
			
			    return RET_CONTINUE;
			}
			
			public int mp3stopCommand()
			{
				if (0!=playingMusic() && (mp3fadeout_duration > 0) &&
			        (system_menu_mode == SYSTEM_NULL)) {
			        // do a bgm fadeout
			        mp3fade_start = SDL_GetTicks();
			        timer_bgmfade_id = SDL_AddTimer(20, bgmfadeCallback, 0);
			
			        event_mode |= WAIT_TIMER_MODE;
			        waitEvent(-1);
			    }
			
			    stopBGM( false );
			    return RET_CONTINUE;
			}
			
			//Mion: integrating mp3fadeout as it's supposed to work.
			public int mp3fadeoutCommand()
			{
				mp3fadeout_duration = (uint)(script_h.readInt());
			
			    return RET_CONTINUE;
			}
			
			public int mp3fadeinCommand()
			{
				mp3fadein_duration = (uint)(script_h.readInt());
			
			    return RET_CONTINUE;
			}
			
			public int mp3Command()
			{
				bool loop_flag = false;
			    if      ( script_h.isName( "mp3save" ) ){
			        mp3save_flag = true;
			    }
			    else if ( script_h.isName( "bgmonce" ) ){
			        mp3save_flag = false;
			    }
			    else if ( script_h.isName( "mp3loop" ) ||
			              script_h.isName( "bgm" ) ){
			        mp3save_flag = true;
			        loop_flag = true;
			    }
			    else{
			        mp3save_flag = false;
			    }
			
			    mp3stopCommand();
			
			    music_play_loop_flag = loop_flag;
			
			    CharPtr buf = new CharPtr(script_h.readStr());
			    if (buf[0] != '\0'){
			        int tmp = music_volume;
			        setStr(ref music_file_name, buf);
			
			        //don't bother with playback or fadeins if there's no audio
			        if ( !audio_open_flag ) return RET_CONTINUE;
			
			        if (mp3fadein_duration > 0) {
			            music_volume = 0;
			        }
			
			        playSound(music_file_name,
			                  SOUND_WAVE | SOUND_OGG_STREAMING | SOUND_MP3 | SOUND_SEQMUSIC,
			                  music_play_loop_flag, MIX_BGM_CHANNEL);
			
			        music_volume = tmp;
			
			        if (mp3fadein_duration > 0) {
			            // do a bgm fadein
			            mp3fade_start = SDL_GetTicks();
			
			            timer_bgmfade_id = SDL_AddTimer(20, bgmfadeCallback,
			                                            timer_bgmfade_id); //FIXME:???
			
			            event_mode = WAIT_TIMER_MODE;
			            waitEvent(-1);
			        }
			    }
			
			    return RET_CONTINUE;
			}
			
			public int movieCommand()
			{
				bool mpegplay_flag = false;
			
			    if ( script_h.isName( "mpegplay" ) ){
			        mpegplay_flag = true;
			    } else {
			        if ( script_h.compareString( "stop" ) ){
			            script_h.readName();
			            return RET_CONTINUE;
			        }
			    }
			
			    script_h.readStr();
			    CharPtr save_buf = new CharPtr(script_h.saveStringBuffer());
			
			    if (mpegplay_flag) {
			        movie_loop_flag = false;
			        movie_click_flag = (script_h.readInt()==1)?true:false;
			        if (0!=playMPEG( save_buf, false )) endCommand();
			        return RET_CONTINUE;
			    }
			
			    movie_click_flag = false;
			    movie_loop_flag = false;
			    bool async_flag = false;
			    bool pos_flag = false;
			    int x=0,y=0,w=0,h=0;
			
			    while( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) ){
			        CharPtr param = script_h.readName();
			        if ( strcmp(param, "click") == 0 )
			            movie_click_flag = true;
			        else if ( strcmp(param, "loop") == 0 )
			            movie_loop_flag = true;
			        else if ( strcmp(param, "async") == 0 )
			            async_flag = true;
			        else if ( strcmp(param, "pos") == 0 ) {
			            pos_flag = true;
			            x = ExpandPos(script_h.readInt());
			            y = ExpandPos(script_h.readInt());
			            w = ExpandPos(script_h.readInt());
			            h = ExpandPos(script_h.readInt());
			        }
			    }
			
			    if (0!=playMPEG( save_buf, async_flag, pos_flag, x, y, w, h )) endCommand();
			
			    return RET_CONTINUE;
			}
			
			public int movemousecursorCommand()
			{
				int x = StretchPosX(script_h.readInt());
			    int y = StretchPosY(script_h.readInt());
			
			    //SDL_WarpMouse( x, y );
			
			    return RET_CONTINUE;
			}
			
			public int mousemodeCommand()
			{
				int no = script_h.readInt();
			    if (no == 0) {
			        ; //SDL_ShowCursor(SDL_DISABLE);
			    } else {
			        ; //SDL_ShowCursor(SDL_ENABLE);
				}
			
			    return RET_CONTINUE;
			}
			
			public int monocroCommand()
			{
				leaveTextDisplayMode();
			
			    if ( script_h.compareString( "off" ) ){
			        script_h.readName();
			        monocro_flag = false;
			    }
			    else{
			        monocro_flag = true;
			        readColor( ref monocro_color, readColorStr() );
			
			        for (int i=0 ; i<256 ; i++){
			        	monocro_color_lut[i][0] = (byte)(((monocro_color[0] * i) >> 8) & 0xff);
			        	monocro_color_lut[i][1] = (byte)(((monocro_color[1] * i) >> 8) & 0xff);
			        	monocro_color_lut[i][2] = (byte)(((monocro_color[2] * i) >> 8) & 0xff);
			        }
			    }
			
			    dirty_rect.fill( screen_width, screen_height );
			
			    return RET_CONTINUE;
			}
			
			public int minimizewindowCommand()
			{
				return RET_CONTINUE;
			}
			
			public int mesboxCommand()
			{
				CharPtr buf = new CharPtr(script_h.readStr());
			    CharPtr msg = new char[strlen(buf)+1];
			    sprintf(msg,"%s",buf);
			    CharPtr title = new CharPtr(script_h.readStr());
			#if MACOSX
			    //The OS X dialog box routines are crashing when in fullscreen mode,
			    //so let's switch to windowed mode just in case
			    menu_windowCommand();
			    ONSCocoa::alertbox(title, msg);
			#elif WIN32 && USE_MESSAGEBOX
			    menu_windowCommand();
			    HWND pwin = NULL;
			    SDL_SysWMinfo info;
			    SDL_VERSION(&info.version);
			    if (SDL_GetWMInfo(&info) == 1)
			        pwin = info.window;
			    MessageBox(pwin, msg, title, MB_OK);
			#endif
			    fprintf(stderr,"Got message box '%s': '%s'\n", title, msg);
			    msg = null;//delete[] msg;
			
			    return RET_CONTINUE;
			}
			
			public int menu_windowCommand()
			{
				if ( fullscreen_mode ){
			        fullscreen_mode = false;
			    }
			
			    return RET_CONTINUE;
			}
			
			public int menu_waveonCommand()
			{
				volume_on_flag = true;
			    printf("menu_waveon: setting main volume to on\n");
			
			    return RET_CONTINUE;
			}
			
			public int menu_waveoffCommand()
			{
				volume_on_flag = false;
			    printf("menu_waveoff: setting main volume to off\n");
			
			    return RET_CONTINUE;
			}
			
			public int menu_fullCommand()
			{
				if ( !fullscreen_mode ){
			    }
			
			    return RET_CONTINUE;
			}
			
			public int menu_click_pageCommand()
			{
				skip_mode |= SKIP_TO_EOP;
			    printf("menu_click_page: enabling page-at-once mode\n");
			
			    return RET_CONTINUE;
			}
			
			public int menu_click_defCommand()
			{
				skip_mode &= ~SKIP_TO_EOP;
			    printf("menu_click_def: disabling page-at-once mode\n");
			
			    return RET_CONTINUE;
			}
			
			public int menu_automodeCommand()
			{
				automode_flag = true;
			    skip_mode &= ~SKIP_NORMAL;
			    printf("menu_automode: change to automode\n");
			
			    return RET_CONTINUE;
			}
			
			public int lsp2Command()
			{
				leaveTextDisplayMode();
			
			    bool v=true;
			
			    if ( script_h.isName( "lsph2" ) || script_h.isName( "lsph2add" ) ||
			         script_h.isName( "lsph2sub" ) )
			        v = false;
			
			    int blend_mode = AnimationInfo.BLEND_NORMAL;
			    if ( script_h.isName( "lsp2add" ) || script_h.isName( "lsph2add" ) )
			        blend_mode = AnimationInfo.BLEND_ADD;
			    else if ( script_h.isName( "lsp2sub" ) || script_h.isName( "lsph2sub" ) )
			        blend_mode = AnimationInfo.BLEND_SUB;
			
			    int no = script_h.readInt();
			    if ( sprite2_info[no].visible )
			        dirty_rect.add( sprite2_info[no].bounding_rect );
			    sprite2_info[ no ].visible = v;
			    sprite2_info[ no ].blending_mode = blend_mode;
			    
			    CharPtr buf = new CharPtr(script_h.readStr());
			    sprite2_info[ no ].setImageName( buf );
			
			    sprite2_info[ no ].orig_pos.x = script_h.readInt();
			    sprite2_info[ no ].orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(sprite2_info[ no ]);
			    sprite2_info[ no ].scale_x = script_h.readInt();
			    sprite2_info[ no ].scale_y = script_h.readInt();
			    sprite2_info[ no ].rot = script_h.readInt();
			
			    if ( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) )
			        sprite2_info[ no ].trans = script_h.readInt();
			    else
			        sprite2_info[ no ].trans = 256;
			
			    parseTaggedString( sprite2_info[ no ] );
			#if RCA_SCALE
			    setupAnimationInfo( sprite2_info[ no ], null, scr_stretch_x, scr_stretch_y );
			#else
			    setupAnimationInfo( sprite2_info[ no ] );
			#endif
			    sprite2_info[ no ].calcAffineMatrix();
			
			    if ( sprite2_info[no].visible )
			        dirty_rect.add( sprite2_info[no].bounding_rect );
			    sprite2_info[ no ].is_animatable = false; //extended sprites don't animate
			
			    return RET_CONTINUE;
			}
			
			public int lspCommand()
			{
				leaveTextDisplayMode();
			
			    bool v=true;
			
			    if ( script_h.isName( "lsph" ) )
			        v = false;
			
			    int no = script_h.readInt();
			    if ( sprite_info[no].visible )
			        dirty_rect.add( sprite_info[no].pos );
			
			    CharPtr buf = new CharPtr(script_h.readStr());
			    sprite_info[ no ].setImageName( buf );
			
			    parseTaggedString( sprite_info[ no ] );
			
			    if (sprite_info[ no ].stale_image) {
			        //see if the same sprite has been loaded recently
			        int x = last_loaded_sprite_ind;
			        for (int i=0; i<SPRITE_NUM_LAST_LOADS; i++) {
			            if (last_loaded_sprite[x] < 0) continue;
			            AnimationInfo anim = sprite_info[ last_loaded_sprite[x] ];
			            if (!anim.stale_image &&
			                sameImageTag(anim, sprite_info[ no ])) {
			                sprite_info[ no ].deepcopy(anim);
			                sprite_info[ no ].current_cell = 0;
			                sprite_info[ no ].direction = 1;
			                sprite_info[ no ].stale_image = false;
			                break;
			            }
			            x += SPRITE_NUM_LAST_LOADS - 1;
			            x %= SPRITE_NUM_LAST_LOADS;
			        }
			    }
			
			    sprite_info[ no ].visible = v;
			    sprite_info[ no ].orig_pos.x = script_h.readInt();
			    sprite_info[ no ].orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(sprite_info[ no ]);
			    if ( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) )
			        sprite_info[ no ].trans = script_h.readInt();
			    else
			        sprite_info[ no ].trans = 256;
			
			#if RCA_SCALE
			    setupAnimationInfo( sprite_info[ no ], null, scr_stretch_x, scr_stretch_y );
			#else
			    setupAnimationInfo( sprite_info[ no ] );
			#endif
			    ++last_loaded_sprite_ind; last_loaded_sprite_ind %= SPRITE_NUM_LAST_LOADS;
			    last_loaded_sprite[last_loaded_sprite_ind] = no;
			
			    if ( sprite_info[no].visible )
			        dirty_rect.add( sprite_info[no].pos );
			    if ( sprite_info[ no ].is_animatable ) advanceAnimPhase();
			
			    return RET_CONTINUE;
			}
			
			public int loopbgmstopCommand()
			{
				setStr(ref loop_bgm_name[0], null);
			
			    return RET_CONTINUE;
			}
			
			public int loopbgmCommand()
			{
				CharPtr buf = new CharPtr(script_h.readStr());
			    setStr( ref loop_bgm_name[0], buf );
			    buf = new CharPtr(script_h.readStr());
			    setStr( ref loop_bgm_name[1], buf );
			
			    playSound(loop_bgm_name[1],
			              SOUND_PRELOAD|SOUND_WAVE|SOUND_OGG, false, MIX_LOOPBGM_CHANNEL1);
			    playSound(loop_bgm_name[0],
			              SOUND_WAVE|SOUND_OGG, false, MIX_LOOPBGM_CHANNEL0);
			
			    return RET_CONTINUE;
			}
			
			public int lookbackflushCommand()
			{
				current_page = current_page.next;
			    for ( int i=0 ; i<max_page_list-1 ; i++ ){
			        current_page.text_count = 0;
			        current_page = current_page.next;
			    }
			    clearCurrentPage();
			    start_page = current_page;
			
			    return RET_CONTINUE;
			}
			
			public int lookbackbuttonCommand()
			{
				for ( int i=0 ; i<4 ; i++ ){
					CharPtr buf = new CharPtr(script_h.readStr());
			        setStr( ref lookback_info[i].image_name, buf );
			        parseTaggedString( lookback_info[i] );
			        setupAnimationInfo( lookback_info[i] );
			    }
			    return RET_CONTINUE;
			}
			
			public int logspCommand()
			{
				leaveTextDisplayMode();
			
			    bool logsp2_flag = false;
			
			    if ( script_h.isName( "logsp2" ) )
			        logsp2_flag = true;
			
			    int sprite_no = script_h.readInt();
			
			    AnimationInfo si = sprite_info[sprite_no];
			    if ( si.visible ) dirty_rect.add( si.pos );
			    si.remove();
			    setStr( ref si.file_name, script_h.readStr() );
			
			    si.orig_pos.x = script_h.readInt();
			    si.orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(si);
			
			    si.trans_mode = AnimationInfo.TRANS_STRING;
			    if (logsp2_flag){
			        si.font_size_xy[0] = script_h.readInt();
			        si.font_size_xy[1] = script_h.readInt();
			        si.font_pitch = script_h.readInt() + si.font_size_xy[0];
			        script_h.readInt(); // dummy read for y pitch
			    }
			    else{
			        si.font_size_xy[0] = sentence_font.font_size_xy[0];
			        si.font_size_xy[1] = sentence_font.font_size_xy[1];
			        si.font_pitch = sentence_font.pitch_xy[0];
			    }
			
			    CharPtr current = new CharPtr(script_h.getNext());
			    int num = 0;
			    while(0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA)){
			        script_h.readStr();
			        num++;
			    }
			
			    script_h.setCurrent(current);
			    if (num == 0){
			        si.num_of_cells = 1;
			        si.color_list = new byte[ si.num_of_cells ][];
			        for (int i =0; i < si.color_list.Length; ++i)
			        {
			        	si.color_list[i] = new byte[3];
			        }
			        readColor( ref si.color_list[0], "#ffffff" );
			    }
			    else{
			        si.num_of_cells = num;
			        si.color_list = new byte[ si.num_of_cells ][];
			        for (int i =0; i < si.color_list.Length; ++i)
			        {
			        	si.color_list[i] = new byte[3];
			        }
			        for (int i=0 ; i<num ; i++){
			            readColor( ref si.color_list[i], readColorStr() );
			        }
			    }
			
			    si.is_single_line = false;
			    si.is_tight_region = false;
			    si.is_ruby_drawable = true;
			    si.skip_whitespace = false;
			    sentence_font.is_newline_accepted = true;
			#if RCA_SCALE
			    setupAnimationInfo( si, null, scr_stretch_x, scr_stretch_y );
			#else
			    setupAnimationInfo( si );
			#endif
			    sentence_font.is_newline_accepted = false;
			    si.visible = true;
			    dirty_rect.add( si.pos );
			
			    return RET_CONTINUE;
			}
			
			public int locateCommand()
			{
				int x = script_h.readInt();
			    int y = script_h.readInt();
			
			    //Mion: correcting the ons-en old style of locate, which depended
			    // on finding the movements from the current position to
			    // a new screen position, which could vary depending on the current
			    // window settings - better to use absolutes
			    // Also should account for possible vertical text and
			    // -1 (i.e. unchanged) x or y values
			    int[] cur_xy = new int[2];
			    cur_xy[0] = sentence_font.xy[0];
			    cur_xy[1] = sentence_font.xy[1];
			    bool tateyoko = (sentence_font.getTateyokoMode() == Fontinfo.TATE_MODE);
			
			    sentence_font.clear();
			    if (y >= 0) {
			        current_page.add(ScriptHandler.TEXT_FF); //reset to "top of page"
			        if (tateyoko)
			            cur_xy[0] = sentence_font.xy[0] - 2 * y;
			        else
			            cur_xy[1] = sentence_font.xy[1] + 2 * y;
			    }
			    char downchar = (char)ScriptHandler.TEXT_VTAB;
			    if (x >= 0) {
			        current_page.add(ScriptHandler.TEXT_CR); //reset to "start of line"
			        downchar = (char)ScriptHandler.TEXT_LF;
			        if (tateyoko)
			            cur_xy[1] = sentence_font.xy[1] + 2 * x;
			        else
			            cur_xy[0] = sentence_font.xy[0] + 2 * x;
			    }
			    for (int i=0; i<y*2; i+=2){
			    	current_page.add((byte)downchar);
			    }
			#if _MSC_VER 
				{
			#endif
			    for (int i=0; i<x*2; i+=2){
			        //a TAB is like adding a single space, but strippable in "gettext"
			        current_page.add(ScriptHandler.TEXT_TAB);
			        current_page.add(ScriptHandler.TEXT_TAB);
			    }
			#if _MSC_VER 
				}
			#endif
			    sentence_font.xy[0] = cur_xy[0];
			    sentence_font.xy[1] = cur_xy[1];
			
			    return RET_CONTINUE;
			}
			
			public int loadgameCommand()
			{
				int no = script_h.readInt();
			
			    if ( no < 0 )
			        errorAndExit( "loadgame: save number is less than 0." );
			
			    int fadeout = (int)mp3fadeout_duration;
			    mp3fadeout_duration = 0; //don't use fadeout during a load
			    if ( 0==loadSaveFile( no ) ){
			        dirty_rect.fill( screen_width, screen_height );
			        flush( refreshMode() );
			
			        saveon_flag = true;
			        internal_saveon_flag = true;
			        skip_mode &= ~SKIP_NORMAL;
			        automode_flag = false;
			        deleteButtonLink();
			        deleteSelectLink();
			        key_pressed_flag = false;
			        text_on_flag = false;
			        indent_offset = 0;
			        line_enter_status = 0;
			        page_enter_status = 0;
			        string_buffer_offset = 0;
			        break_flag = false;
			
			        refreshMouseOverButton();
			
			        if (null!=loadgosub_label)
			            gosubReal( loadgosub_label, script_h.getCurrent() );
			    }
			    mp3fadeout_duration = (uint)fadeout;
			
			    return RET_CONTINUE;
			}
			
			public int linkcolorCommand()
			{
				CharPtr buf;
			    
			    buf = readColorStr();
			    readColor( ref linkcolor[0], buf );
			    buf = readColorStr();
			    readColor( ref linkcolor[1], buf );
			
			    return RET_CONTINUE;
			}
			
			public int ldCommand()
			{
				leaveTextDisplayMode();
			
			    char loc = script_h.readName()[0];
			    int no = -1;
			    if      (loc == 'l') no = 0;
			    else if (loc == 'c') no = 1;
			    else if (loc == 'r') no = 2;
			
			    CharPtr buf = null;
			
			    if (no >= 0){
			        buf = script_h.readStr();
			        if ( null!=tachi_info[ no ].image_surface )
			            dirty_rect.add( tachi_info[ no ].pos );
			        tachi_info[ no ].setImageName( buf );
			        parseTaggedString( tachi_info[ no ] );
			#if RCA_SCALE
			        if (scr_stretch_y > 1.0) {
			            // RCA: Stretch characters to screen size.
			            // Note stretches are with Y-scale, so they don't get distorted
			            setupAnimationInfo( &tachi_info[ no ], NULL, scr_stretch_y, scr_stretch_y );
			        } else
			#endif
			        setupAnimationInfo( tachi_info[ no ] );
			
			        if ( null!=tachi_info[ no ].image_surface ){
			            tachi_info[ no ].visible = true;
			            //start with "orig_pos" at the center-bottom, for easier scaling
			            tachi_info[ no ].orig_pos.x = humanpos[no];
			            tachi_info[ no ].orig_pos.y = underline_value + 1;
			            UpdateAnimPosStretchXY(tachi_info[ no ]);
			            tachi_info[ no ].pos.x -= tachi_info[ no ].pos.w / 2;
			            tachi_info[ no ].pos.y -= tachi_info[ no ].pos.h;
			            if (!disable_rescale_flag){
			                tachi_info[ no ].orig_pos.x -= tachi_info[ no ].orig_pos.w / 2;
			                tachi_info[ no ].orig_pos.y -= tachi_info[ no ].orig_pos.h;
			            } else {
			                tachi_info[ no ].orig_pos.x -= ContractPos(tachi_info[ no ].orig_pos.w) / 2;
			                tachi_info[ no ].orig_pos.y -= ContractPos(tachi_info[ no ].orig_pos.h);
			            }
			            dirty_rect.add( tachi_info[ no ].pos );
			        }
			    }
			
			    EffectLink el = parseEffect(true);
			    if (setEffect(el, true, true)) return RET_CONTINUE;
			    while (doEffect(el));
			
			    return RET_CONTINUE;
			}
			
			public int layermessageCommand()
			{
				int no = script_h.readInt();
				CharPtr message = new CharPtr(script_h.readStr());
			
			#if NO_LAYER_EFFECTS
			    printf("layermessage: layer effect support not available (%d,'%s')\n",
			           no, message);
			#else
			    LayerInfo tmp = layer_info;
			    while (null!=tmp) {
			        if ( tmp.num == no ) break;
			        tmp = tmp.next;
			    }
			    if (null!=tmp) {
			        getret_str = tmp.handler.message(message, ref getret_int);
			        //printf("layermessage returned: '%s', %d\n", getret_str, getret_int);
			    }
			#endif // ndef NO_LAYER_EFFECTS
			
			    return RET_CONTINUE;
			}
			
			public int languageCommand()
			{
				CharPtr  which = new CharPtr(script_h.readName());
			    if ( strcmp(which, "japanese") == 0 ){
			        script_h.preferred_script = ScriptHandler.LanguageScript.JAPANESE_SCRIPT;
			    }
			    else if ( strcmp(which, "english") == 0 ){
			        script_h.preferred_script = ScriptHandler.LanguageScript.LATIN_SCRIPT;
			    }
			    else {
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                 "language: unknown language '%s'", which);
			        errorAndExit(script_h.errbuf, "valid options are 'japanese' and 'english'");
			    }
			    return RET_CONTINUE;
			}
			
			public int jumpfCommand()
			{
				CharPtr buf = new CharPtr(script_h.getNext());
				while(buf[0] != '\0' && buf[0] != '~') buf.inc();
				if (buf[0] == '~') buf.inc();
			
			    script_h.setCurrent(buf);
			    current_label_info = script_h.getLabelByAddress(buf);
			    current_line = script_h.getLineByAddress(buf);
			
			    return RET_CONTINUE;
			}
			
			public int jumpbCommand()
			{
				script_h.setCurrent( last_tilde.next_script );
			    current_label_info = script_h.getLabelByAddress( last_tilde.next_script );
			    current_line = script_h.getLineByAddress( last_tilde.next_script );
			
			    return RET_CONTINUE;
			}
			
			public int ispageCommand()
			{
				script_h.readInt();
			
			    if ( textgosub_clickstr_state == CLICK_NEWPAGE )
			        script_h.setInt( script_h.current_variable, 1 );
			    else
			        script_h.setInt( script_h.current_variable, 0 );
			
			    return RET_CONTINUE;
			}
			
			public int isfullCommand()
			{
				script_h.readInt();
			    script_h.setInt( script_h.current_variable, fullscreen_mode?1:0 );
			
			    return RET_CONTINUE;
			}
			
			public int isskipCommand()
			{
				script_h.readInt();
			
			    if ( automode_flag )
			        script_h.setInt( script_h.current_variable, 2 );
			    else if ( 0!=(skip_mode & SKIP_NORMAL) )
			        script_h.setInt( script_h.current_variable, 1 );
			    else if ( 0!=ctrl_pressed_status )
			        script_h.setInt( script_h.current_variable, 3 );
			    else if ( 0!=(skip_mode & SKIP_TO_EOP) )
			        script_h.setInt( script_h.current_variable, 4 );
			    else
			        script_h.setInt( script_h.current_variable, 0 );
			
			    return RET_CONTINUE;
			}
			
			public int isdownCommand()
			{
				script_h.readInt();
			
			    if ( current_button_state.down_flag )
			        script_h.setInt( script_h.current_variable, 1 );
			    else
			        script_h.setInt( script_h.current_variable, 0 );
			
			    return RET_CONTINUE;
			}
			
			public int inputCommand()
			{
				script_h.readStr();
			
			    if ( script_h.current_variable.type != ScriptHandler.VAR_STR )
			        errorAndExit( "input: no string variable." );
			    int no = script_h.current_variable.var_no;
			
			    script_h.readStr(); // description
			    CharPtr buf = new CharPtr(script_h.readStr()); // default value
			    setStr( ref script_h.getVariableData(no).str, buf );
			
			    printf( "*** inputCommand(): $%d is set to the default value: %s\n",
			            no, buf );
			    script_h.readInt(); // maxlen
			    script_h.readInt(); // widechar flag
			    if ( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) ){
			        script_h.readInt(); // window width
			        script_h.readInt(); // window height
			        script_h.readInt(); // text box width
			        script_h.readInt(); // text box height
			    }
			
			    return RET_CONTINUE;
			}
			
			public int indentCommand()
			{
				indent_offset = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int humanorderCommand()
			{
				leaveTextDisplayMode();
			
			    CharPtr buf = script_h.readStr();
			    int i;
			    for (i=0 ; i<3 ; i++){
			        if      (buf[i] == 'l') human_order[i] = 0;
			        else if (buf[i] == 'c') human_order[i] = 1;
			        else if (buf[i] == 'r') human_order[i] = 2;
			        else                    human_order[i] = -1;
			    }
			
			    for ( i=0 ; i<3 ; i++ )
			        dirty_rect.add( tachi_info[i].pos );
			
			    EffectLink el = parseEffect(true);
			    if (setEffect(el, true, true)) return RET_CONTINUE;
			    while (doEffect(el));
			
			    return RET_CONTINUE;
			}
			
			public int getzxcCommand()
			{
				getzxc_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getvoicevolCommand()
			{
				script_h.readInt();
			    script_h.setInt( script_h.current_variable, voice_volume );
			    return RET_CONTINUE;
			}
			
			public int getversionCommand()
			{
				script_h.readInt();
			    script_h.setInt( script_h.current_variable, NSC_VERSION );
			
			    return RET_CONTINUE;
			}
			
			public int gettimerCommand()
			{
				bool gettimer_flag=false;
			
			    if      ( script_h.isName( "gettimer" ) ){
			        gettimer_flag = true;
			    }
			    else if ( script_h.isName( "getbtntimer" ) ){
			    }
			
			    script_h.readInt();
			
			    if ( gettimer_flag ){
			    	script_h.setInt( script_h.current_variable, (int)(SDL_GetTicks() - internal_timer) );
			    }
			    else{
			    	script_h.setInt( script_h.current_variable, (int)btnwait_time );
			    }
			
			    return RET_CONTINUE;
			}
			
			public int gettextbtnstrCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    int txtbtn_no = script_h.readInt();
			
			    TextButtonInfoLink info = text_button_info.next;
			    TextButtonInfoLink found = null;
			    while (null!=info) {
			        if (info.no == txtbtn_no)
			            found = info;
			        info = info.next;
			    }
			
			    if (null!=found)
			        setStr(ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, found.text);
			    else
			        setStr(ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, null);
			
			    return RET_CONTINUE;
			}
			
			public int gettextCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    int page_no = 0;
			    Page page = current_page;
			    if (script_h.isName( "getlogtext" )) {
			        page_no = script_h.readInt();
			
			        while(page != start_page && page_no > 0){
			            page_no--;
			            page = page.previous;
			        }
			    }
			
			    if (page_no > 0)
			        setStr( ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, null );
			    else {
			        //extract control characters from the page text
			        CharPtr buf = new char[ page.text_count + 1 ];
			        int i, j;
			        for ( i=0, j=0 ; i<page.text_count ; i++ ){
			            if (page.text[i] == ScriptHandler.LEFT_PAREN)
			                buf[j++] = '(';
			            else if (page.text[i] == ScriptHandler.RIGHT_PAREN)
			                buf[j++] = ')';
			            else if ((byte)(page.text[i] & 0xff) >= 0x20)
			                buf[j++] = page.text[i];
			            //don't put any control characters into the string
			        }
			        buf[j] = '\0';
			
			        setStr( ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, buf );
			        buf = null;//delete[] buf;
			    }
			
			    return RET_CONTINUE;
			}
			
			public int gettaglogCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    int page_no = script_h.readInt();
			    int page_origin = 0;
			    if (set_tag_page_origin_to_1) page_origin = 1;
			
			    Page page = current_page;
			    while(page != start_page && page_no > page_origin){
			        page_no--;
			        page = page.previous;
			    }
			
			    if (null!=page.tag)
			        setStr(ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, page.tag);
			    else
			        setStr(ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, null);
			
			    return RET_CONTINUE;
			}
			
			public int gettagCommand()
			{
				if ( null==last_nest_info.previous || last_nest_info.nest_mode != NestInfo.LABEL )
			        errorAndExit( "gettag: not in a subroutine, i.e. pretextgosub" );
			
				CharPtr buf = new CharPtr(current_page.tag);
			
			    int end_status;
			    do{
			        script_h.readVariable();
			        end_status = script_h.getEndStatus();
			        script_h.pushVariable();
			
			        if ( 0!=(script_h.pushed_variable.type & ScriptHandler.VAR_INT) ||
			             0!=(script_h.pushed_variable.type & ScriptHandler.VAR_ARRAY) ){
			            if (null!=buf)
			                script_h.setInt( script_h.pushed_variable, script_h.parseInt(ref buf));
			            else
			                script_h.setInt( script_h.pushed_variable, 0);
			        }
			        else if ( 0!=(script_h.pushed_variable.type & ScriptHandler.VAR_STR) ){
			            if (null!=buf){
			        		CharPtr buf_start = new CharPtr(buf);
			                bool in_1byte_mode = false;
			                while((in_1byte_mode || buf[0] != '/') && buf[0] != 0){
			                    if (buf[0] == '`') {
			                        in_1byte_mode = !in_1byte_mode;
			                        buf.inc();
			                	} else if (IS_TWO_BYTE(buf[0]))
			                		buf.inc(2);
			                    else
			                    	buf.inc();
			                }
			                setStr( ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, buf_start, CharPtr.minus(buf,buf_start) );
			            }
			            else{
			                setStr( ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, null);
			            }
			        }
			
			        if (null!=buf && buf[0] == '/')
			        	buf.inc();
			        else
			            buf = null;
			    }
			    while(0!=(end_status & ScriptHandler.END_COMMA));
			
			    return RET_CONTINUE;
			}
			
			public int gettabCommand()
			{
				gettab_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getspsizeCommand()
			{
				int no = script_h.readInt();
			
			    script_h.readVariable();
			    script_h.setInt( script_h.current_variable, sprite_info[no].orig_pos.w );
			    script_h.readVariable();
			    script_h.setInt( script_h.current_variable, sprite_info[no].orig_pos.h );
			    if ( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) ){
			        script_h.readVariable();
			        script_h.setInt( script_h.current_variable, sprite_info[no].num_of_cells );
			    }
			
			    return RET_CONTINUE;
			}
			
			public int getspmodeCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    int no = script_h.readInt();
			    script_h.setInt( script_h.pushed_variable, sprite_info[no].visible?1:0 );
			
			    return RET_CONTINUE;
			}
			
			public int getskipoffCommand()
			{
				getskipoff_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getsevolCommand()
			{
				script_h.readInt();
			    script_h.setInt( script_h.current_variable, se_volume );
			    return RET_CONTINUE;
			}
			
			public int getscreenshotCommand()
			{
				int w = script_h.readInt();
			    int h = script_h.readInt();
			    if (disable_rescale_flag) {
			        w = ExpandPos(w);
			        h = ExpandPos(h);
			    }
			    if ( w == 0 ) w = 1;
			    if ( h == 0 ) h = 1;
			
			    if ( null!=screenshot_surface &&
			        (SDL_Surface_get_w(screenshot_surface) != w ||
			         SDL_Surface_get_h(screenshot_surface) != h )){
			        SDL_FreeSurface( screenshot_surface );
			        screenshot_surface = null;
			    }
			
			    if ( screenshot_surface == null )
			        screenshot_surface = SDL_CreateRGBSurface( SDL_SWSURFACE, w, h, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000 );
			
			    SDL_Surface surface = SDL_ConvertSurface( screen_surface, SDL_Surface_get_format(image_surface), SDL_SWSURFACE );
			    AnimationInfo.resizeSurface( surface, screenshot_surface );
			    SDL_FreeSurface( surface );
			
			    return RET_CONTINUE;
			}
			
			public int getsavestrCommand()
			{
				script_h.readVariable();
			    if ( script_h.current_variable.type != ScriptHandler.VAR_STR )
			        errorAndExit( "getsavestr: no string variable" );
			
			    int var_no = script_h.current_variable.var_no;
			    int no = script_h.readInt();
			
			    CharPtr savestr = null;
			    CharPtr tmpstr = null;
			    int slen = 0;
			    if (loadSaveFile(no, false) == 0) {
			        //grab the savestr, if any
			        readStr( ref tmpstr );
			        if (null!=tmpstr) {
			        	savestr = new CharPtr(tmpstr);
			        	slen = (int)strlen(savestr);
			            if ((savestr[0] == '"') && (savestr[slen-2] == '"') &&
			                (savestr[slen-1] == '*')) {
			        		savestr[slen-2] = (char)0;
			        		savestr.inc();
			        		slen = (int)strlen(savestr);
			            }
			        }
			    } else
			        printf("getsavestr: couldn't read save slot %d\n", no);
			
			    if (null!=savestr)
			        setStr(ref script_h.getVariableData( var_no ).str, savestr);
			    else
			        setStr(ref script_h.getVariableData( var_no ).str, "");
			    //printf("getsavestr: got '%s'\n", script_h.getVariableData( var_no ).str);
			
			    if (null!=tmpstr) tmpstr = null;//delete[] tmpstr;
			
			    return RET_CONTINUE;
			}
			
			public int getpageupCommand()
			{
				getpageup_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getpageCommand()
			{
				getpageup_flag = true;
			    getpagedown_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getretCommand()
			{
				script_h.readVariable();
			
			    if ( script_h.current_variable.type == ScriptHandler.VAR_INT ||
			         script_h.current_variable.type == ScriptHandler.VAR_ARRAY ){
			        script_h.setInt( script_h.current_variable, getret_int );
			    }
			    else if ( script_h.current_variable.type == ScriptHandler.VAR_STR ){
			        int no = script_h.current_variable.var_no;
			        setStr( ref script_h.getVariableData(no).str, getret_str );
			    }
			    else errorAndExit( "getret: no variable." );
			
			    return RET_CONTINUE;
			}
			
			public int getregCommand()
			{
				script_h.readVariable();
			
			    if ( script_h.current_variable.type != ScriptHandler.VAR_STR )
			        errorAndExit( "getreg: no string variable." );
			    int no = script_h.current_variable.var_no;
			
			    CharPtr buf = script_h.readStr();
			    CharPtr path = new char[256], key = new char[256];
			    strcpy( path, buf );
			    buf = script_h.readStr();
			    strcpy( key, buf );
			
			    printf("  reading Registry file for [%s] %s\n", path, key );
			
			    FILEPtr fp;
			    if ( ( fp = fopen( registry_file, "r" ) ) == null ){
			        fprintf( stderr, "Cannot open file [%s]\n", registry_file );
			        return RET_CONTINUE;
			    }
			
			    CharPtr reg_buf = new char[256], reg_buf2 = new char[256];
			    bool found_flag = false;
			    while( null!=fgets( reg_buf, 256, fp) && !found_flag ){
			        if ( reg_buf[0] == '[' ){
			            uint c=0;
			            while ( reg_buf[c] != ']' && reg_buf[c] != '\0' ) c++;
			            if ( 0==strncmp( new CharPtr(reg_buf, + 1), path, (int)((c-1>strlen(path))?(c-1):strlen(path)) ) ){
			                while( null!=fgets( reg_buf2, 256, fp) ){
			
			                    script_h.pushCurrent( reg_buf2 );
			                    buf = script_h.readStr();
			                    if ( 0!=strncmp( buf,
			                                  key,
			                                  (int)((strlen(buf)>strlen(key))?strlen(buf):strlen(key) )) ){
			                        script_h.popCurrent();
			                        continue;
			                    }
			
			                    if ( !script_h.compareString("=") ){
			                        script_h.popCurrent();
			                        continue;
			                    }
			                    script_h.setCurrent(new CharPtr(script_h.getNext(),+1));
			
			                    buf = script_h.readStr();
			                    setStr( ref script_h.getVariableData(no).str, buf );
			                    script_h.popCurrent();
			                    printf("  $%d = %s\n", no, script_h.getVariableData(no).str );
			                    found_flag = true;
			                    break;
			                }
			            }
			        }
			    }
			
			    if ( !found_flag ) fprintf( stderr, "  The key is not found.\n" );
			    fclose(fp);
			
			    return RET_CONTINUE;
			}
			
			public int getmp3volCommand()
			{
				script_h.readInt();
			    script_h.setInt( script_h.current_variable, music_volume );
			    return RET_CONTINUE;
			}
			
			public int getmouseposCommand()
			{
				script_h.readInt();
			    script_h.setInt( script_h.current_variable, ContractPos(current_button_state.x) );
			
			    script_h.readInt();
			    script_h.setInt( script_h.current_variable, ContractPos(current_button_state.y) );
			
			    return RET_CONTINUE;
			}
			
			public int getmouseoverCommand()
			{
				getmouseover_flag = true;
			    getmouseover_min = script_h.readInt();
			    getmouseover_max = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int getmclickCommand()
			{
				getmclick_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getlogCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    int page_no = script_h.readInt();
			
			    Page page = current_page;
			    while(page != start_page && page_no > 0){
			        page_no--;
			        page = page.previous;
			    }
			
			    if (page_no > 0)
			        setStr( ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, null );
			    else
			        setStr( ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, page.text, page.text_count );
			
			    return RET_CONTINUE;
			}
			
			public int getinsertCommand()
			{
				getinsert_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getfunctionCommand()
			{
				getfunction_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getenterCommand()
			{
				if ( !force_button_shortcut_flag )
			        getenter_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getcursorposCommand()
			{
				if ( script_h.isName( "getcursorpos2" ) ){
			        script_h.readInt();
			        script_h.setInt( script_h.current_variable, last_textpos_xy[0] );
			
			        script_h.readInt();
			        script_h.setInt( script_h.current_variable, last_textpos_xy[1] );
			    } else {
					int[] xy_bak = new int[2], line_offset_xy_bak = new int[2];
			        xy_bak[0] = sentence_font.xy[0];
			        xy_bak[1] = sentence_font.xy[1];
			        line_offset_xy_bak[0] = sentence_font.line_offset_xy[0];
			        line_offset_xy_bak[1] = sentence_font.line_offset_xy[1];
			        if ( script_h.isName( "getnextline" ) ){
			            sentence_font.newLine();
			        }
			
			        script_h.readInt();
			        //script_h.setInt( &script_h.current_variable, sentence_font.x() );
			#if RCA_SCALE
			        if (scr_stretch_x > 1.0)
			            script_h.setInt( &script_h.current_variable, (sentence_font.x()-sentence_font.ruby_offset_xy[0]) / scr_stretch_x + 0.5);
			        else
			#endif
			        script_h.setInt( script_h.current_variable, sentence_font.x()-sentence_font.ruby_offset_xy[0] ); // workaround for possibly a bug in the original
			
			        script_h.readInt();
			        //script_h.setInt( &script_h.current_variable, sentence_font.y() );
			#if RCA_SCALE
			        if (scr_stretch_y > 1.0)
			            script_h.setInt( &script_h.current_variable, (sentence_font.y()-sentence_font.ruby_offset_xy[1]) / scr_stretch_y + 0.5);
			        else
			#endif
			        script_h.setInt( script_h.current_variable, sentence_font.y()-sentence_font.ruby_offset_xy[1] ); // workaround for possibly a bug in the original
			
			        sentence_font.xy[0] = xy_bak[0];
			        sentence_font.xy[1] = xy_bak[1];
			        sentence_font.line_offset_xy[0] = line_offset_xy_bak[0];
			        sentence_font.line_offset_xy[1] = line_offset_xy_bak[1];
			    }
			
			    return RET_CONTINUE;
			}
			
			public int getcursorCommand()
			{
				if ( !force_button_shortcut_flag )
			        getcursor_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int getcselstrCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    int csel_no = script_h.readInt();
			
			    int counter = 0;
			    SelectLink link = root_select_link.next;
			    while (null!=link){
			        if (csel_no == counter++) break;
			        link = link.next;
			    }
			    if (null==link) errorAndExit("getcselstr: no select link");
			    setStr(ref script_h.getVariableData( script_h.pushed_variable.var_no ).str, link.text);
			
			    return RET_CONTINUE;
			}
			
			public int getcselnumCommand()
			{
				int count = 0;
			
			    SelectLink link = root_select_link.next;
			    while ( null!=link ) {
			        count++;
			        link = link.next;
			    }
			    script_h.readInt();
			    script_h.setInt( script_h.current_variable, count );
			
			    return RET_CONTINUE;
			}
			
			public int gameCommand()
			{
				int i;
			    current_mode = NORMAL_MODE;
			    effectspeed = EFFECTSPEED_NORMAL;
			
			    /* ---------------------------------------- */
			    if ( null==lookback_info[0].image_surface ){
			        setStr( ref lookback_info[0].image_name, DEFAULT_LOOKBACK_NAME0 );
			        parseTaggedString( lookback_info[0] );
			        setupAnimationInfo( lookback_info[0] );
			    }
			    if ( null==lookback_info[1].image_surface ){
			        setStr( ref lookback_info[1].image_name, DEFAULT_LOOKBACK_NAME1 );
			        parseTaggedString( lookback_info[1] );
			        setupAnimationInfo( lookback_info[1] );
			    }
			    if ( null==lookback_info[2].image_surface ){
			        setStr( ref lookback_info[2].image_name, DEFAULT_LOOKBACK_NAME2 );
			        parseTaggedString( lookback_info[2] );
			        setupAnimationInfo( lookback_info[2] );
			    }
			    if ( null==lookback_info[3].image_surface ){
			        setStr( ref lookback_info[3].image_name, DEFAULT_LOOKBACK_NAME3 );
			        parseTaggedString( lookback_info[3] );
			        setupAnimationInfo( lookback_info[3] );
			    }
			
			    /* ---------------------------------------- */
			    /* Load default cursor */
			    loadCursor( CURSOR_WAIT_NO, DEFAULT_CURSOR_WAIT, 0, 0 );
			    loadCursor( CURSOR_NEWPAGE_NO, DEFAULT_CURSOR_NEWPAGE, 0, 0 );
			
			    /* ---------------------------------------- */
			    /* Initialize text buffer */
			    page_list = new Page[max_page_list];
			    for ( i=0 ; i<max_page_list-1 ; i++ ){
			        page_list[i].next = page_list[i+1];
			        page_list[i+1].previous = page_list[i];
			    }
			    page_list[0].previous = page_list[max_page_list-1];
			    page_list[max_page_list-1].next = page_list[0];
			    start_page = current_page = page_list[0];
			
			    clearCurrentPage();
			
			    /* ---------------------------------------- */
			    /* Initialize local variables */
			    for ( i=0 ; i<script_h.global_variable_border ; i++ )
			        script_h.getVariableData(i).reset(false);
			
			    setCurrentLabel( "start" );
			    saveSaveFile(-1);
			
			    return RET_CONTINUE;
			}
			
			public int flushoutCommand()
			{
				//Mion: flushout special effect
			    // not quite the same as NScr's, but looks good
			    // does a "flushout" in 30 stages while fading to white
			    tmp_effect.duration = script_h.readInt();
			    tmp_effect.effect   = CUSTOM_EFFECT_NO + 3;
			
			    dirty_rect.fill( screen_width, screen_height );
			
			    if (setEffect(tmp_effect, false, false)) return RET_CONTINUE;
			
			    setStr( ref bg_info.file_name, "white" );
			    createBackground();
			    SDL_BlitSurface( bg_info.image_surface, null, effect_dst_surface, null );
			    SDL_BlitSurface( accumulation_surface, null, effect_tmp_surface, null );
			    while (doEffect(tmp_effect));
			
			    return RET_CONTINUE;
			}
			
			public int fileexistCommand()
			{
				script_h.readInt();
			    script_h.pushVariable();
			    CharPtr buf = script_h.readStr();
			
			    int found = (script_h.cBR.getFileLength(buf)>0)?1:0;
			    if (0==found) {
			        CharPtr fn = new char[4096];
			        sprintf(fn, "%s%s", script_h.save_path, buf);
			        CharPtr si = new CharPtr(fn);
			        do {
			        	if (IS_TWO_BYTE(si[0])) si.inc();
			        	else if ( (si[0] == '\\') || (si[0] == '/') ) si[0] = DELIMITER;
			        	si.inc();
			        } while (0!=si[0]);
			#if true//MSC_VER < 1200
					FILEPtr fp = fopen(fn, "rb"); // FIXME: failing even when file exists?!
			#else
			        FILE* fp = std::fopen(fn, "rb"); // FIXME: failing even when file exists?!
			#endif
			//printf("Seek %s, fp = %s, ", fn, fp ? "yes" : "no");
			        if (null!=fp) {
			            found = 1;
			            fclose(fp);
			//puts("found\n");
			        }
			//else printf(" fail (%s)\n", strerror(errno));
			    }
			    script_h.setInt( script_h.pushed_variable, found );
			
			    return RET_CONTINUE;
			}
			
			public int exec_dllCommand()
			{
				CharPtr buf = script_h.readStr();
			    CharPtr dll_name = new char[256];
			    uint c=0;
			    while(0!=buf[c] && (buf[c] != '/')){
			        dll_name[c] = buf[c];
			        c++;
			    }
			    dll_name[c] = '\0';
			
			    printf("  reading %s for %s\n", dll_file, dll_name );
			
			    FILEPtr fp;
			    if ( ( fp = fopen( dll_file, "r" ) ) == null ){
			        fprintf( stderr, "Cannot open file [%s]\n", dll_file );
			        return RET_CONTINUE;
			    }
			
			    CharPtr dll_buf = new char[256], dll_buf2 = new char[256];
			    bool found_flag = false;
			    while( null!=fgets( dll_buf, 256, fp) && !found_flag ){
			        if ( dll_buf[0] == '[' ){
			            c=0;
			            while ( dll_buf[c] != ']' && dll_buf[c] != '\0' ) c++;
			            if ( 0==strncmp( new CharPtr(dll_buf, + 1), dll_name, (int)((c-1>strlen(dll_name))?(c-1):strlen(dll_name)) ) ){
			                found_flag = true;
			                while( null!=fgets( dll_buf2, 256, fp) ){
			                    c=0;
			                    while ( dll_buf2[c] == ' ' || dll_buf2[c] == '\t' ) c++;
			                    if ( 0==strncmp( new CharPtr(dll_buf2,(int)c), "str", 3 ) ){
			                        c+=3;
			                        while ( dll_buf2[c] == ' ' || dll_buf2[c] == '\t' ) c++;
			                        if ( dll_buf2[c] != '=' ) continue;
			                        c++;
			                        while ( dll_buf2[c] != '"' ) c++;
			                        uint c2 = ++c;
			                        while ( dll_buf2[c2] != '"' && dll_buf2[c2] != '\0' ) c2++;
			                        dll_buf2[c2] = '\0';
			                        setStr( ref getret_str, new CharPtr(dll_buf2,(int)c) );
			                        printf("  getret_str = %s\n", getret_str );
			                    }
			                    else if ( 0==strncmp( new CharPtr(dll_buf2,(int)c), "ret", 3 ) ){
			                        c+=3;
			                        while ( dll_buf2[c] == ' ' || dll_buf2[c] == '\t' ) c++;
			                        if ( dll_buf2[c] != '=' ) continue;
			                        c++;
			                        while ( dll_buf2[c] == ' ' || dll_buf2[c] == '\t' ) c++;
			                        getret_int = atoi( new CharPtr(dll_buf2,(int)c) );
			                        printf("  getret_int = %d\n", getret_int );
			                    }
			                    else if ( dll_buf2[c] == '[' )
			                        break;
			                }
			            }
			        }
			    }
			
			    if ( !found_flag ) fprintf( stderr, "  The DLL is not found in %s.\n", dll_file );
			    fclose( fp );
			
			    return RET_CONTINUE;
			}
			
			public int exbtnCommand()
			{
				int sprite_no=-1, no=0;
			    ButtonLink button;
			
			    if ( script_h.isName( "exbtn_d" ) ){
			        button = exbtn_d_button_link;
			        if ( null!=button.exbtn_ctl ) button.exbtn_ctl = null;//delete[] button->exbtn_ctl;
			    }
			    else{
			        bool cellcheck_flag = false;
			
			        if ( script_h.isName( "cellcheckexbtn" ) )
			            cellcheck_flag = true;
			
			        sprite_no = script_h.readInt();
			        no = script_h.readInt();
			
			        if ( (cellcheck_flag && (sprite_info[ sprite_no ].num_of_cells < 2)) ||
			             (!cellcheck_flag && (sprite_info[ sprite_no ].num_of_cells == 0)) ){
			            script_h.readStr();
			            return RET_CONTINUE;
			        }
			
			        button = new ButtonLink();
			        root_button_link.insert( button );
			    }
			    is_exbtn_enabled = true;
			
			    CharPtr buf = script_h.readStr();
			
			    button.button_type = ButtonLink.BUTTON_TYPE.EX_SPRITE_BUTTON;
			    button.sprite_no   = sprite_no;
			    button.no          = no;
			    button.exbtn_ctl   = new char[ strlen( buf ) + 1 ];
			    strcpy( button.exbtn_ctl, buf );
			
			    if ( sprite_no >= 0 &&
			         ( null!=sprite_info[ sprite_no ].image_surface ||
			           sprite_info[ sprite_no ].trans_mode == AnimationInfo.TRANS_STRING ) )
			    {
			        button.image_rect = button.select_rect = sprite_info[ sprite_no ].pos;
			    }
			
			    return RET_CONTINUE;
			}
			
			public int erasetextwindowCommand()
			{
				erase_text_window_mode = script_h.readInt();
			    did_leavetext = false;
			    dirty_rect.add( sentence_font_info.pos );
			
			    return RET_CONTINUE;
			}
			
			public int erasetextbtnCommand()
			{
				if (!txtbtn_visible) return RET_CONTINUE;
			
			    TextButtonInfoLink info = text_button_info.next;
			    while (null!=info) {
			        ButtonLink cur_button_link = info.button;
			        while (null!=cur_button_link) {
			            cur_button_link.show_flag = 1;
			            cur_button_link.anim[0].visible = true;
			            cur_button_link.anim[0].setCell(0);
			            dirty_rect.add( cur_button_link.image_rect );
			            cur_button_link = cur_button_link.same;
			        }
			        info = info.next;
			    }
			    flush( refreshMode() );
			
			    return RET_CONTINUE;
			}
			
			public int endCommand()
			{
				printf("Quitting...\n");
			    quit();
			    exit(0);
			    return RET_CONTINUE; // dummy
			}
			
			public int effectskipCommand()
			{
				if (script_h.readInt() == 0)
			        effectskip_flag = false;
			    else
			        effectskip_flag = true;
			
			    return RET_CONTINUE;
			}
			
			public int dwavestopCommand()
			{
				int ch = script_h.readInt();
			    //Mion - ogapee2008: avoid dwavestop outside array
			    if (ch < 0) ch = 0;
			    else if (ch >= ONS_MIX_CHANNELS) ch = ONS_MIX_CHANNELS-1;
			
			    if ((ch == 0) && bgmdownmode_flag)
			        setCurMusicVolume( music_volume );
			
			    return RET_CONTINUE;
			}
			
			public int dwaveCommand()
			{
				int play_mode = WAVE_PLAY;
			    bool loop_flag = false;
			
			    if ( script_h.isName( "dwaveloop" ) ){
			        loop_flag = true;
			    }
			    else if ( script_h.isName( "dwaveload" ) ){
			        play_mode = WAVE_PRELOAD;
			    }
			    else if ( script_h.isName( "dwaveplayloop" ) ){
			        play_mode = WAVE_PLAY_LOADED;
			        loop_flag = true;
			    }
			    else if ( script_h.isName( "dwaveplay" ) ){
			        play_mode = WAVE_PLAY_LOADED;
			        loop_flag = false;
			    }
			
			    int ch = script_h.readInt();
			    if      (ch < 0) ch = 0;
			    else if (ch >= ONS_MIX_CHANNELS) ch = ONS_MIX_CHANNELS-1;
			
			    if (play_mode == WAVE_PLAY_LOADED){
			
			    }
			    else{
			        CharPtr buf = script_h.readStr();
			        int fmt = SOUND_WAVE|SOUND_OGG;
			        if (play_mode == WAVE_PRELOAD) fmt |= SOUND_PRELOAD;
			        playSound(buf, fmt, loop_flag, ch);
			    }
			    if ((ch == 0) && bgmdownmode_flag)
			        setCurMusicVolume( music_volume );
			
			    return RET_CONTINUE;
			}
			
			public int dvCommand()
			{
				CharPtr buf = new char[256];
			
				sprintf(buf, "voice%c%s.wav", DELIMITER, new CharPtr(script_h.getStringBuffer(),+2));
			    playSound(buf, SOUND_WAVE|SOUND_OGG, false, 0);
			
			    return RET_CONTINUE;
			}
			
			public int drawtextCommand()
			{
				SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(accumulation_surface), SDL_Surface_get_h(accumulation_surface));
			    text_info.blendOnSurface( accumulation_surface, 0, 0, ref clip );
			
			    return RET_CONTINUE;
			}
			
			public int drawsp3Command()
			{
				int sprite_no = script_h.readInt();
			    int cell_no = script_h.readInt();
			    int alpha = script_h.readInt();
			
			    int x = StretchPosX(script_h.readInt());
			    int y = StretchPosY(script_h.readInt());
			
			    AnimationInfo si = sprite_info[sprite_no];
			    int old_cell_no = si.current_cell;
			    si.setCell(cell_no);
			
			    si.mat[0][0] = script_h.readInt();
			    si.mat[0][1] = script_h.readInt();
			    si.mat[1][0] = script_h.readInt();
			    si.mat[1][1] = script_h.readInt();
			
			    int denom = (si.mat[0][0]*si.mat[1][1]-si.mat[0][1]*si.mat[1][0])/1000;
			    if (denom != 0){
			        si.inv_mat[0][0] =  si.mat[1][1] * 1000 / denom;
			        si.inv_mat[0][1] = -si.mat[0][1] * 1000 / denom;
			        si.inv_mat[1][0] = -si.mat[1][0] * 1000 / denom;
			        si.inv_mat[1][1] =  si.mat[0][0] * 1000 / denom;
			    }
			
			    SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(screen_surface), SDL_Surface_get_h(screen_surface));
			    si.blendOnSurface2( accumulation_surface, x, y, ref clip, alpha );
			    si.setCell(old_cell_no);
			
			    return RET_CONTINUE;
			}
			
			public int drawsp2Command()
			{
				int sprite_no = script_h.readInt();
			    int cell_no = script_h.readInt();
			    int alpha = script_h.readInt();
			
			    AnimationInfo si = new AnimationInfo(sprite_info[sprite_no]);
			    si.orig_pos.x = script_h.readInt();
			    si.orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(si);
			    si.scale_x = script_h.readInt();
			    si.scale_y = script_h.readInt();
			    si.rot = script_h.readInt();
			    si.calcAffineMatrix();
			    si.setCell(cell_no);
			
			    SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(screen_surface), SDL_Surface_get_h(screen_surface));
			    si.blendOnSurface2( accumulation_surface, si.pos.x, si.pos.y, ref clip, alpha );
			
			    return RET_CONTINUE;
			}
			
			public int drawspCommand()
			{
				int sprite_no = script_h.readInt();
			    int cell_no = script_h.readInt();
			    int alpha = script_h.readInt();
			    int x = StretchPosX(script_h.readInt());;
			    int y = StretchPosY(script_h.readInt());;
			
			    AnimationInfo si = sprite_info[sprite_no];
			    int old_cell_no = si.current_cell;
			    si.setCell(cell_no);
			    SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(accumulation_surface), SDL_Surface_get_h(accumulation_surface));
			    si.blendOnSurface( accumulation_surface, x, y, ref clip, alpha );
			    si.setCell(old_cell_no);
			
			    return RET_CONTINUE;
			}
			
			public int drawfillCommand()
			{
				int r = script_h.readInt();
			    int g = script_h.readInt();
			    int b = script_h.readInt();
			
			    SDL_FillRect( accumulation_surface, null, SDL_MapRGBA( SDL_Surface_get_format(accumulation_surface), (byte)r, (byte)g, (byte)b, 0xff) );
			
			    return RET_CONTINUE;
			}
			
			public int drawclearCommand()
			{
				SDL_FillRect( accumulation_surface, null, SDL_MapRGBA( SDL_Surface_get_format(accumulation_surface), 0, 0, 0, 0xff) );
			
			    return RET_CONTINUE;
			}
			
			public int drawbgCommand()
			{
				SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(accumulation_surface), SDL_Surface_get_h(accumulation_surface));
			    bg_info.blendOnSurface( accumulation_surface, bg_info.pos.x, bg_info.pos.y, ref clip );
			
			    return RET_CONTINUE;
			}
			
			public int drawbg2Command()
			{
				AnimationInfo bi = new AnimationInfo(bg_info);
			    bi.orig_pos.x = script_h.readInt();
			    bi.orig_pos.y = script_h.readInt();
			    UpdateAnimPosXY(bi);
			    bi.scale_x = script_h.readInt();
			    bi.scale_y = script_h.readInt();
			    bi.rot = script_h.readInt();
			    bi.calcAffineMatrix();
			
			    SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(screen_surface), SDL_Surface_get_h(screen_surface));
			    bi.blendOnSurface2( accumulation_surface, bi.pos.x, bi.pos.y,
			                        ref clip, 256 );
			
			    return RET_CONTINUE;
			}
			
			public int drawCommand()
			{
				SDL_Rect rect = new SDL_Rect(0, 0, screen_width, screen_height);
			    flushDirect( rect, REFRESH_NONE_MODE );
			    dirty_rect.clear();
			
			    return RET_CONTINUE;
			}
			
			public int deletescreenshotCommand()
			{
				if ( null!=screenshot_surface ) {
			        SDL_FreeSurface( screenshot_surface );
			        screenshot_surface = null;
			    }
			    return RET_CONTINUE;
			}
			
			public int delayCommand()
			{
				int t = script_h.readInt();
			
			    //Mion: use a shorter delay during skip mode
			    if( 0!=(skip_mode & (SKIP_NORMAL | SKIP_TO_WAIT)) || 0!=ctrl_pressed_status ) {
			        t = 0;
			    }
			
			    event_mode = WAIT_TIMER_MODE | WAIT_INPUT_MODE;
			    waitEvent( t );
			
			    return RET_CONTINUE;
			}
			
			public int defineresetCommand()
			{
				//clear out the event queue
				SDL_Event event_ = new SDL_Event();
			    while( 0!=SDL_PollEvent( event_ ) )
			        if (event_.type == SDL_QUIT) endCommand();
			
			    script_h.reset();
			    /*ScriptParser*/base.reset();
			    reset();
			    //reopen the audio mixer with default settings, if needed
			
			    setCurrentLabel( "define" );
			
			    return RET_CONTINUE;
			}
			
			public int cspCommand()
			{
				leaveTextDisplayMode();
			
			    bool csp2_flag = false;
			    if (script_h.isName("csp2")) csp2_flag = true;
			
			    int no = script_h.readInt();
			    AnimationInfo[] si = null;
			    int num = 0;
			    if (csp2_flag) {
			        num = MAX_SPRITE2_NUM;
			        si = sprite2_info;
			    }
			    else{
			        num = MAX_SPRITE_NUM;
			        si = sprite_info;
			    }
			
			    if ( no == -1 )
			        for ( int i=0 ; i<num ; i++ ){
			            if ( si[i].visible ){
			                if (csp2_flag)
			                    dirty_rect.add( si[i].bounding_rect );
			                else
			                    dirty_rect.add( si[i].pos );
			            }
			            if ( null!=si[i].image_name ){
			                si[i].orig_pos.x = -1000;
			                si[i].orig_pos.y = -1000;
			#if !RCA_SCALE
			                si[i].pos.x = ExpandPos(-1000);
			                si[i].pos.y = ExpandPos(-1000);
			#else
			                si[i].pos.x = ExpandPos(-10000);
			                si[i].pos.y = ExpandPos(-10000);
			#endif
			            }
			            if (!csp2_flag) root_button_link.removeSprite(i);
			            si[i].remove();
			        }
			    else if (no >= 0 && no < MAX_SPRITE_NUM){
			        if ( si[no].visible ) {
			            if (csp2_flag)
			                dirty_rect.add( si[no].bounding_rect );
			            else
			                dirty_rect.add( si[no].pos );
			        }
			        if (!csp2_flag) root_button_link.removeSprite(no);
			        si[no].remove();
			    }
			
			    return RET_CONTINUE;
			}
			
			public int cselgotoCommand()
			{
				int csel_no = script_h.readInt();
			
			    int counter = 0;
			    SelectLink link = root_select_link.next;
			    while( null!=link ){
			        if ( csel_no == counter++ ) break;
			        link = link.next;
			    }
			    if ( null==link ) errorAndExit( "cselgoto: no select link" );
			
			    setCurrentLabel( link.label );
			
			    deleteSelectLink();
			    newPage( true );
			
			    return RET_CONTINUE;
			}
			
			public int cselbtnCommand()
			{
				int csel_no   = script_h.readInt();
			    int button_no = script_h.readInt();
			
			    Fontinfo csel_info = sentence_font;
			    csel_info.setRubyOnFlag(false);
			    csel_info.top_xy[0] = script_h.readInt();
			    csel_info.top_xy[1] = script_h.readInt();
			
			    int counter = 0;
			    SelectLink link = root_select_link.next;
			    while ( null!=link ){
			        if ( csel_no == counter++ ) break;
			        link = link.next;
			    }
			    if ( link == null || link.text == null || link.text[0] == '\0' )
			        return RET_CONTINUE;
			
			    csel_info.setLineArea( (int)strlen(link.text)/2+1 );
			    csel_info.clear();
			    ButtonLink button = getSelectableSentence( link.text, csel_info );
			    root_button_link.insert( button );
			    button.no          = button_no;
			    button.sprite_no   = csel_no;
			
			    sentence_font.ttf_font = csel_info.ttf_font;
			
			    return RET_CONTINUE;
			}
			
			public int clickCommand()
			{
				bool lrclick_flag = false;
			    if ( script_h.isName( "lrclick" ) ) lrclick_flag = true;
			
			    //Mion: NScr doesn't stop skip-to-choice mode for a "click" command
			    if (0!=(skip_mode & SKIP_NORMAL))
			        return RET_CONTINUE;
			
			    skip_mode &= ~SKIP_TO_WAIT;
			    key_pressed_flag = false;
			
			    clickstr_state = CLICK_WAIT;
			    event_mode = WAIT_TIMER_MODE | WAIT_INPUT_MODE;
			    if (lrclick_flag) event_mode |= WAIT_RCLICK_MODE;
			    waitEvent(-1);
			    clickstr_state = CLICK_NONE;
			
			    if (lrclick_flag)
			        getret_int = (current_button_state.button == -1) ? 0 : 1;
			
			    return RET_CONTINUE;
			}
			
			public int clCommand()
			{
				leaveTextDisplayMode();
			
			    char loc = script_h.readName()[0];
			
			    if ( loc == 'l' || loc == 'a' ){
			        dirty_rect.add( tachi_info[0].pos );
			        tachi_info[0].remove();
			    }
			    if ( loc == 'c' || loc == 'a' ){
			        dirty_rect.add( tachi_info[1].pos );
			        tachi_info[1].remove();
			    }
			    if ( loc == 'r' || loc == 'a' ){
			        dirty_rect.add( tachi_info[2].pos );
			        tachi_info[2].remove();
			    }
			
			    EffectLink el = parseEffect(true);
			    if (setEffect(el, true, true)) return RET_CONTINUE;
			    while (doEffect(el));
			
			    return RET_CONTINUE;
			}
			
			public int chvolCommand()
			{
				int ch  = script_h.readInt();
			    //Mion - ogapee2008: avoid access outside array bounds
			    if (ch < 0) ch = 0;
			    else if (ch >= ONS_MIX_CHANNELS) ch = ONS_MIX_CHANNELS-1;
			    int vol = script_h.readInt();
			
			    channelvolumes[ch] = vol;
			
			    return RET_CONTINUE;
			}
			
			public int checkpageCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    if ( script_h.pushed_variable.type != ScriptHandler.VAR_INT &&
			         script_h.pushed_variable.type != ScriptHandler.VAR_ARRAY )
			        errorAndExit( "checkpage: no integer variable." );
			
			    int page_no = script_h.readInt();
			
			    Page page = current_page;
			    while(page != start_page && page_no > 0){
			        page_no--;
			        page = page.previous;
			    }
			
			    if (page_no > 0)
			        script_h.setInt( script_h.pushed_variable, 0 );
			    else
			        script_h.setInt( script_h.pushed_variable, 1 );
			
			    return RET_CONTINUE;
			}
			
			public int checkkeyCommand()
			{
				script_h.readVariable();
			    script_h.pushVariable();
			
			    if ( script_h.pushed_variable.type != ScriptHandler.VAR_INT &&
			         script_h.pushed_variable.type != ScriptHandler.VAR_ARRAY )
			        errorAndExit( "checkpage: no integer variable." );
			
			    CharPtr buf = script_h.readStr();
			    if (null==buf || (buf[0] == '\0')) {
			        script_h.setInt( script_h.pushed_variable, 0 );
			        return RET_CONTINUE;
			    }
			    CharPtr keystr = new char[strlen(buf)+1];
			
			    for (uint i=0; i<strlen(keystr); i++)
			    	keystr[i] = (char)toupper(buf[i]);
			    keystr[strlen(buf)] = '\0';
			
			    int ret = 1;
			    if (strlen(keystr) == 1){
			        if ((last_keypress >= SDLKey.SDLK_0) && (last_keypress <= SDLKey.SDLK_9))
			            ret = (last_keypress - SDLKey.SDLK_0) - (keystr[0] - '0');
			        else if ((last_keypress >= SDLKey.SDLK_a) && (last_keypress <= SDLKey.SDLK_z))
			        	ret = (last_keypress - SDLKey.SDLK_a) - (keystr[0] - 'A');
			    }
			    if (ret != 0){
			        switch (last_keypress){
			          default:
			            ret = 1; break;
			          case SDLKey.SDLK_RCTRL:
			          case SDLKey.SDLK_LCTRL:
			            ret = strcmp(keystr, "CTRL"); break;
			          case SDLKey.SDLK_RSHIFT:
			          case SDLKey.SDLK_LSHIFT:
			            ret = strcmp(keystr, "SHIFT"); break;
			          case SDLKey.SDLK_RETURN:
			            ret = strcmp(keystr, "RETURN");
			            if (ret != 0)
			                ret = strcmp(keystr, "ENTER");
			            break;
			          case SDLKey.SDLK_SPACE:
			            ret = strcmp(keystr, " ");
			            if (ret != 0)
			                ret = strcmp(keystr, "SPACE");
			            break;
			          case SDLKey.SDLK_PAGEUP:
			            ret = strcmp(keystr, "PAGEUP"); break;
			          case SDLKey.SDLK_PAGEDOWN:
			            ret = strcmp(keystr, "PAGEDOWN"); break;
			          case SDLKey.SDLK_UP:
			            ret = strcmp(keystr, "UP"); break;
			          case SDLKey.SDLK_DOWN:
			            ret = strcmp(keystr, "DOWN"); break;
			          case SDLKey.SDLK_LEFT:
			            ret = strcmp(keystr, "LEFT"); break;
			          case SDLKey.SDLK_RIGHT:
			            ret = strcmp(keystr, "RIGHT"); break;
			          case SDLKey.SDLK_F1:  ret = strcmp(keystr, "F1");  break;
			          case SDLKey.SDLK_F2:  ret = strcmp(keystr, "F2");  break;
			          case SDLKey.SDLK_F3:  ret = strcmp(keystr, "F3");  break;
			          case SDLKey.SDLK_F4:  ret = strcmp(keystr, "F4");  break;
			          case SDLKey.SDLK_F5:  ret = strcmp(keystr, "F5");  break;
			          case SDLKey.SDLK_F6:  ret = strcmp(keystr, "F6");  break;
			          case SDLKey.SDLK_F7:  ret = strcmp(keystr, "F7");  break;
			          case SDLKey.SDLK_F8:  ret = strcmp(keystr, "F8");  break;
			          case SDLKey.SDLK_F9:  ret = strcmp(keystr, "F9");  break;
			          case SDLKey.SDLK_F10: ret = strcmp(keystr, "F10"); break;
			          case SDLKey.SDLK_F11: ret = strcmp(keystr, "F11"); break;
			          case SDLKey.SDLK_F12: ret = strcmp(keystr, "F12"); break;
			        }
			    }
			    if (ret == 0) printf("checkkey: got key %s\n", keystr);
			    script_h.setInt( script_h.pushed_variable, (ret == 0) ? 1 : 0 );
			    keystr = null;//delete[] keystr;
			
			    return RET_CONTINUE;
			}
			
			public int cellCommand()
			{
				int sprite_no = script_h.readInt();
			    int no        = script_h.readInt();
			
			    sprite_info[sprite_no].setCell(no);
			    dirty_rect.add( sprite_info[sprite_no].pos );
			
			    return RET_CONTINUE;
			}
			
			public int captionCommand()
			{
				CharPtr buf = script_h.readStr();
			    uint len = strlen(buf);
			
			    CharPtr buf2 = new char[len*2+3];
			    CharPtr buf1 = new char[len+1];
			    strcpy(buf1, buf);
			    DirectReader.convertFromSJISToUTF8(buf2, buf1);
			    buf1 = null;//delete[] buf1;
			
			    setStr( ref wm_title_string, buf2 );
			    setStr( ref wm_icon_string,  buf2 );
			    buf2 = null;//delete[] buf2;
			    //printf("caption (utf8): '%s'\n", wm_title_string);
			#if true
				//FIXME:???
				setStr( ref wm_title_string, buf );
			#endif
			    SDL_WM_SetCaption( wm_title_string, wm_icon_string );
			#if true //WIN32
			    //convert from UTF-8 to Wide (Unicode) and thence to system ANSI
			    len = (uint)MultiByteToWideChar(CP_UTF8, 0, wm_title_string, -1, null, 0);
			    UnsignedShortPtr u16_tmp = new UnsignedShortPtr(new ushort[len]);
			    MultiByteToWideChar(CP_UTF8, 0, wm_title_string, -1, u16_tmp, (int)len);
			    len = (uint)WideCharToMultiByte(CP_ACP, 0, u16_tmp, -1, null, 0, null, null);
			    CharPtr cvt = new char[len+1];
			    WideCharToMultiByte(CP_ACP, 0, u16_tmp, -1, cvt, (int)len, null, null);
			    u16_tmp = null;//delete[] u16_tmp;
			
			    //set the window caption directly
			    SDL_SysWMinfo info = new SDL_SysWMinfo();
			    SDL_GetWMInfo(info);
			    SendMessageA(info.window, WM_SETTEXT, 0, (/*long*/CharPtr)cvt);
			    cvt = null;//delete[] cvt;
			#endif //WIN32
			
			    return RET_CONTINUE;
			}
			
			public int btnwaitCommand()
			{
				bool del_flag=false, textbtn_flag=false;
			    bool remove_window_flag = !( (erase_text_window_mode == 0) ||
			                                 btnnowindowerase_flag );
			
			    if ( script_h.isName( "btnwait2" ) ){
			    }
			    else if ( script_h.isName( "btnwait" ) ){
			        del_flag = true;
			    }
			    else if ( script_h.isName( "textbtnwait" ) ){
			        textbtn_flag = true;
			        remove_window_flag = false;
			    }
			
			    if (remove_window_flag)
			        leaveTextDisplayMode(remove_window_flag);
			
			    script_h.readInt();
			
			    bool skip_flag = 0!=(skip_mode & SKIP_NORMAL) || 0!=ctrl_pressed_status || 
			                     (0!=(skip_mode & SKIP_TO_EOP) && 
			                      ( (clickstr_state == CLICK_WAIT) ||
			                        (null!=textgosub_label &&
			                         ( (textgosub_clickstr_state == CLICK_WAIT) ||
			                           (textgosub_clickstr_state == CLICK_WAITEOL) )) ));
			
			    current_button_state.reset();
			    last_keypress = KEYPRESS_NULL;
			
			  btnwaitTop:
			
			    long button_timer_start = SDL_GetTicks(); //set here so btnwait is correct
			
			    if ( !( skip_flag && textbtn_flag ) ) {
			        shortcut_mouse_line = 0;
			        skip_mode &= ~SKIP_NORMAL;
			
			        if (txtbtn_show) txtbtn_visible = true;
			
			        if (is_exbtn_enabled && null!=exbtn_d_button_link.exbtn_ctl){
			        	SDL_Rect check_src_rect = new SDL_Rect(0, 0, screen_width, screen_height);
			            decodeExbtnControl( exbtn_d_button_link.exbtn_ctl, check_src_rect );
			        }
			
			        ButtonLink p_button_link = root_button_link.next;
			        while( null!=p_button_link ){
			            ButtonLink cur_button_link = p_button_link;
			            while (null!=cur_button_link) {
			                cur_button_link.show_flag = 0;
			                if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.SPRITE_BUTTON ||
			                     cur_button_link.button_type == ButtonLink.BUTTON_TYPE.EX_SPRITE_BUTTON ){
			                    sprite_info[ cur_button_link.sprite_no ].visible = true;
			                    sprite_info[ cur_button_link.sprite_no ].setCell(0);
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.TMP_SPRITE_BUTTON ){
			                    cur_button_link.show_flag = 1;
			                    sprite_info[ cur_button_link.sprite_no ].setCell(0);
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.TEXT_BUTTON ){
			                    if (txtbtn_visible)
			                        cur_button_link.show_flag = 1;
			                        sprite_info[ cur_button_link.sprite_no ].setCell(0);
			                }
			                else if ( cur_button_link.anim[1] != null ){
			                    cur_button_link.show_flag = 2;
			                }
			                dirty_rect.add( cur_button_link.image_rect );
			                cur_button_link = cur_button_link.same;
			            }
			            p_button_link = p_button_link.next;
			        }
			
			        flush( refreshMode() );
			
			        event_mode = WAIT_BUTTON_MODE;
			        refreshMouseOverButton();
			
			        int t = -1;
			        if ( btntime_value > 0 ){
			            if ( btntime2_flag )
			                event_mode |= WAIT_VOICE_MODE;
			            t = (int)btntime_value;
			        }
			        button_timer_start = SDL_GetTicks();
			
			        if ( textbtn_flag ){
			            event_mode |= WAIT_TEXTBTN_MODE;
			            if ( btntime_value == 0 ){
			                if ( automode_flag ){
			                    event_mode |= WAIT_VOICE_MODE;
			                    if ( automode_time < 0 ){
			                        if (t == -1 || t > -automode_time * num_chars_in_sentence)
			                        	t = (int)(-automode_time * num_chars_in_sentence);
			                    }
			                    else{
			                        if (t == -1 || t > automode_time)
			                        	t = (int)automode_time;
			                    }
			                }
			                else if ( (autoclick_time > 0) &&
			                          (t == -1 || t > autoclick_time))
			            		t = (int)autoclick_time;
			            }
			        }
			        if (t <= 0) t = -1;
			
			        event_mode |= WAIT_TIMER_MODE;
			        if (waitEvent(t)) {
			            return RET_CONTINUE;
			        }
			    }
			
			    btnwait_time = SDL_GetTicks() - button_timer_start;
			    num_chars_in_sentence = 0;
			
			    if ( skip_flag && textbtn_flag ){
			        //do a wait to catch possible 'ctrl' release
			        waitEvent(0);
			        if (!current_button_state.valid_flag)
			            current_button_state.set(0);
			    }
			
			    if (!current_button_state.valid_flag) {
			        goto btnwaitTop;
			    }
			
			
			    script_h.setInt( script_h.current_variable, current_button_state.button );
			
			    if ( current_button_state.button >= 1 && del_flag ){
			        btndef_info.remove();
			        deleteButtonLink();
			    }
			
			    event_mode = IDLE_EVENT_MODE;
			    disableGetButtonFlag();
			
			    ButtonLink p_button_link_ = root_button_link.next;
			    while( null!=p_button_link_ ){
			        ButtonLink cur_button_link = p_button_link_;
			        while (null!=cur_button_link) {
			            cur_button_link.show_flag = 0;
			            cur_button_link = cur_button_link.same;
			        }
			        p_button_link_ = p_button_link_.next;
			    }
			    flush( refreshMode() );
			
			    return RET_CONTINUE;
			}
			
			public int btntimeCommand()
			{
				if ( script_h.isName( "btntime2" ) )
			        btntime2_flag = true;
			    else
			        btntime2_flag = false;
			    btntime_value = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int btndownCommand()
			{
				btndown_flag = (script_h.readInt()==1)?true:false;
			
			    return RET_CONTINUE;
			}
			
			public int btndefCommand()
			{
				if (script_h.compareString("clear")){
			        script_h.readName();
			    }
			    else{
			        CharPtr buf = script_h.readStr();
			
			        btndef_info.remove();
			
			        if ( buf[0] != '\0' ){
			            btndef_info.setImageName( buf );
			            parseTaggedString( btndef_info );
			            btndef_info.trans_mode = AnimationInfo.TRANS_COPY;
			#if RCA_SCALE
			            setupAnimationInfo( btndef_info, null, scr_stretch_x, scr_stretch_y );
			#else
			            setupAnimationInfo( btndef_info );
			#endif
			            SDL_SetAlpha( btndef_info.image_surface, DEFAULT_BLIT_FLAG, SDL_ALPHA_OPAQUE );
			        }
			    }
			
			    btntime_value = 0;
			    deleteButtonLink();
			    current_button_state.reset();
			    last_keypress = KEYPRESS_NULL;
			    processTextButtonInfo();
			
			    disableGetButtonFlag();
			
			    return RET_CONTINUE;
			}
			
			public int btnareaCommand()
			{
				btnarea_flag = true;
			    btnarea_pos = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int btnCommand()
			{
				SDL_Rect src_rect = new SDL_Rect();
			
			    ButtonLink button = new ButtonLink();
			
			    button.no           = script_h.readInt();
			
			    button.image_rect.x = StretchPosX(script_h.readInt());
			    button.image_rect.y = StretchPosY(script_h.readInt());
			    button.image_rect.w = StretchPosX(script_h.readInt());
			    button.image_rect.h = StretchPosY(script_h.readInt());
			
			    button.select_rect = button.image_rect;
			
			    src_rect.x = StretchPosX(script_h.readInt());
			    src_rect.y = StretchPosY(script_h.readInt());
			
			    if (null==btndef_info.image_surface){
			        button.image_rect.w = 0;
			        button.image_rect.h = 0;
			    }
			    if (null!=btndef_info.image_surface &&
			        src_rect.x + button.image_rect.w > SDL_Surface_get_w(btndef_info.image_surface)){
			        button.image_rect.w = SDL_Surface_get_w(btndef_info.image_surface) - src_rect.x;
			    }
			    if (null!=btndef_info.image_surface &&
			        src_rect.y + button.image_rect.h > SDL_Surface_get_h(btndef_info.image_surface)){
			        button.image_rect.h = SDL_Surface_get_h(btndef_info.image_surface) - src_rect.y;
			    }
			    src_rect.w = button.image_rect.w;
			    src_rect.h = button.image_rect.h;
			
			    button.anim[0] = new AnimationInfo();
			    button.anim[0].num_of_cells = 1;
			    button.anim[0].trans_mode = AnimationInfo.TRANS_COPY;
			    button.anim[0].pos.x = button.image_rect.x;
			    button.anim[0].pos.y = button.image_rect.y;
			    if (null!=btndef_info.image_surface) {
			        button.anim[0].allocImage( button.image_rect.w, button.image_rect.h );
			        button.anim[0].copySurface( btndef_info.image_surface, src_rect );
			    }
			
			    root_button_link.insert( button );
			
			    return RET_CONTINUE;
			}
			
			public int brCommand()
			{
				sentence_font.newLine();
			    current_page.add( 0x0a );
			
			    return RET_CONTINUE;
			}
			
			public int bltCommand()
			{
				int dx,dy,dw,dh;
			    int sx,sy,sw,sh;
			
			    dx = StretchPosX(script_h.readInt());
			    dy = StretchPosY(script_h.readInt());
			    dw = StretchPosX(script_h.readInt());
			    dh = StretchPosY(script_h.readInt());
			    sx = StretchPosX(script_h.readInt());
			    sy = StretchPosY(script_h.readInt());
			    sw = StretchPosX(script_h.readInt());
			    sh = StretchPosY(script_h.readInt());
			
			    if (btndef_info.image_surface == null) return RET_CONTINUE;
			    if (dw == 0 || dh == 0 || sw == 0 || sh == 0) return RET_CONTINUE;
			
			    if ( sw == dw && sw > 0 && sh == dh && sh > 0 ){
			
			        SDL_Rect src_rect = new SDL_Rect(sx,sy,sw,sh);
			    	SDL_Rect dst_rect = new SDL_Rect(dx,dy,dw,dh);
			
			        SDL_BlitSurface( btndef_info.image_surface, src_rect, screen_surface, dst_rect );
			        SDL_UpdateRect( screen_surface, dst_rect.x, dst_rect.y, (uint)dst_rect.w, (uint)dst_rect.h );
			        dirty_rect.clear();
			    }
			    else{
			        SDL_LockSurface(accumulation_surface);
			        SDL_LockSurface(btndef_info.image_surface);
			        Uint32Ptr dst_buf = (Uint32Ptr)new Uint32Ptr(SDL_Surface_get_pixels(accumulation_surface));
			        Uint32Ptr src_buf = (Uint32Ptr)new Uint32Ptr(SDL_Surface_get_pixels(btndef_info.image_surface));
			#if BPP16
			        int dst_width = SDL_Surface_get_pitch(accumulation_surface) / 2;
			        int src_width = SDL_Surface_get_pitch(btndef_info.image_surface) / 2;
			#else
			        int dst_width = SDL_Surface_get_pitch(accumulation_surface) / 4;
			        int src_width = SDL_Surface_get_pitch(btndef_info.image_surface) / 4;
			#endif
			
			        int start_y = dy, end_y = dy+dh;
			        if (dh < 0){
			            start_y = dy+dh;
			            end_y = dy;
			        }
			        if (start_y < 0) start_y = 0;
			        if (end_y > screen_height) end_y = screen_height;
			
			        int start_x = dx, end_x = dx+dw;
			        if (dw < 0){
			            start_x = dx+dw;
			            end_x = dx;
			        }
			        if (start_x < 0) start_x = 0;
			        if (end_x >= screen_width) end_x = screen_width;
			
			        dst_buf.inc(start_y*dst_width);
			        for (int i=start_y ; i<end_y ; i++){
			            int y = sy+sh*(i-dy)/dh;
			            for (int j=start_x ; j<end_x ; j++){
			
			                int x = sx+sw*(j-dx)/dw;
			                if (x<0 || x>= SDL_Surface_get_w(btndef_info.image_surface) ||
			                    y<0 || y>= SDL_Surface_get_h(btndef_info.image_surface))
			                	dst_buf[+j] = 0;
			                else
			                	dst_buf[+j] = src_buf[+y*src_width+x];
			            }
			            dst_buf.inc(dst_width);
			        }
			        SDL_UnlockSurface(btndef_info.image_surface);
			        SDL_UnlockSurface(accumulation_surface);
			
			        SDL_Rect dst_rect = new SDL_Rect(start_x, start_y, end_x-start_x, end_y-start_y);
			        flushDirect( (SDL_Rect)dst_rect, REFRESH_NONE_MODE );
			    }
			
			    return RET_CONTINUE;
			}
			
			public int bgmdownmodeCommand()
			{
				bgmdownmode_flag = (script_h.readInt() != 0);
			
			    return RET_CONTINUE;
			}
			
			public int bgcopyCommand()
			{
				SDL_BlitSurface( screen_surface, null, accumulation_surface, null );
			
			    bg_info.num_of_cells = 1;
			    bg_info.trans_mode = AnimationInfo.TRANS_COPY;
			    bg_info.pos.x = 0;
			    bg_info.pos.y = 0;
			    bg_info.copySurface( accumulation_surface, null );
			
			    return RET_CONTINUE;
			}
			
			public int bgCommand()
			{
				//Mion: prefer removing textwindow for bg change effects even during skip;
			    //but don't remove text window if erasetextwindow == 0
			    leaveTextDisplayMode((erase_text_window_mode != 0));
			
			    CharPtr buf = null;
			    if (script_h.compareString("white")){
			        buf = "white";
			        script_h.readName();
			    }
			    else if (script_h.compareString("black")){
			        buf = "black";
			        script_h.readName();
			    }
			    else{
			        if (allow_color_type_only) {
			            bool is_color = false;
			            buf = script_h.readColor(ref is_color, true);
			            if (!is_color)
			                buf = script_h.readStr();
			        } else {
			            buf = script_h.readStr();
			        }
			    }
			
			    for ( int i=0 ; i<3 ; i++ )
			        tachi_info[i].remove();
			
			    bg_info.remove();
			    setStr( ref bg_info.file_name, buf );
			
			    createBackground();
			    dirty_rect.fill( screen_width, screen_height );
			
			    EffectLink el = parseEffect(true);
			    if (setEffect(el, true, true)) return RET_CONTINUE;
			#if true //TODO:
			    while (doEffect(el));
			#endif
			
			    return RET_CONTINUE;
			}
			
			public int barclearCommand()
			{
				if (system_menu_mode == SYSTEM_NULL)
			        leaveTextDisplayMode();
			
			    for ( int i=0 ; i<MAX_PARAM_NUM ; i++ ) {
			        if ( null!=bar_info[i] ) {
			            dirty_rect.add( bar_info[i].pos );
			            //delete bar_info[i];
			            bar_info[i] = null;
			        }
			    }
			    return RET_CONTINUE;
			}
			
			public int barCommand()
			{
				int no = script_h.readInt();
			    if ( null!=bar_info[no] ){
			        dirty_rect.add( bar_info[no].pos );
			        bar_info[no].remove();
			    }
			    else{
			        bar_info[no] = new AnimationInfo();
			    }
			    bar_info[no].trans_mode = AnimationInfo.TRANS_COPY;
			    bar_info[no].num_of_cells = 1;
			
			    bar_info[no].param = script_h.readInt();
			    bar_info[no].orig_pos.x = script_h.readInt();
			    bar_info[no].orig_pos.y = script_h.readInt();
			    UpdateAnimPosXY(bar_info[no]);
			
			    bar_info[no].max_width = script_h.readInt();
			    bar_info[no].orig_pos.h = script_h.readInt();
			    bar_info[no].pos.h = ExpandPos(bar_info[no].orig_pos.h);
			    bar_info[no].max_param = script_h.readInt();
			
			    CharPtr buf = readColorStr();
			    readColor( ref bar_info[no].color, buf );
			
			    int w = ExpandPos(bar_info[no].max_width) * bar_info[no].param / bar_info[no].max_param;
			    if ( bar_info[no].max_width > 0 && w > 0 ){
			        bar_info[no].pos.w = w;
			        bar_info[no].allocImage( bar_info[no].pos.w, bar_info[no].pos.h );
			        bar_info[no].fill( bar_info[no].color[0], bar_info[no].color[1], bar_info[no].color[2], 0xff );
			        dirty_rect.add( bar_info[no].pos );
			    }
			
			    return RET_CONTINUE;
			}
			
			public int aviCommand()
			{
				script_h.readStr();
			    CharPtr save_buf = script_h.saveStringBuffer();
			
			    bool click_flag = (script_h.readInt()==1)?true:false;
			
			    stopBGM( false );
			    if (0!=playAVI( save_buf, click_flag )) endCommand();
			
			    return RET_CONTINUE;
			}
			
			public int automode_timeCommand()
			{
				automode_time = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int autoclickCommand()
			{
				autoclick_time = script_h.readInt();
			
			    return RET_CONTINUE;
			}
			
			public int amspCommand()
			{
				leaveTextDisplayMode();
			
			    bool amsp2_flag = false;
			    if (script_h.isName("amsp2")) amsp2_flag = true;
			
			    int no = script_h.readInt();
			    AnimationInfo si=null;
			    if (amsp2_flag){
			        si = sprite2_info[no];
			        dirty_rect.add( si.bounding_rect );
			    }
			    else{
			        si = sprite_info[no];
			        dirty_rect.add( si.pos );
			    }
			
			    si.orig_pos.x = script_h.readInt();
			    si.orig_pos.y = script_h.readInt();
			    UpdateAnimPosStretchXY(si);
			    if (amsp2_flag){
			        si.scale_x = script_h.readInt();
			        si.scale_y = script_h.readInt();
			        si.rot = script_h.readInt();
			        si.calcAffineMatrix();
			        dirty_rect.add( si.bounding_rect );
			    }
			    else{
			        dirty_rect.add( si.pos );
			    }
			
			    if ( 0!=(script_h.getEndStatus() & ScriptHandler.END_COMMA) )
			        si.trans = script_h.readInt();
			
			    if ( si.trans > 256 ) si.trans = 256;
			    else if ( si.trans < 0 ) si.trans = 0;
			    if ( si.is_animatable ) advanceAnimPhase();
			
			    return RET_CONTINUE;
			}
			
			public int allsp2resumeCommand()
			{
				all_sprite2_hide_flag = false;
			    for ( int i=0 ; i<MAX_SPRITE2_NUM ; i++ ){
			        if ( sprite2_info[i].visible )
			            dirty_rect.add( sprite2_info[i].bounding_rect );
			    }
			    return RET_CONTINUE;
			}
			
			public int allspresumeCommand()
			{
				all_sprite_hide_flag = false;
			    for ( int i=0 ; i<MAX_SPRITE_NUM ; i++ ){
			        if ( sprite_info[i].visible )
			            dirty_rect.add( sprite_info[i].pos );
			    }
			    return RET_CONTINUE;
			}
			
			public int allsp2hideCommand()
			{
				all_sprite2_hide_flag = true;
			    for ( int i=0 ; i<MAX_SPRITE2_NUM ; i++ ){
			        if ( sprite2_info[i].visible )
			            dirty_rect.add( sprite2_info[i].bounding_rect );
			    }
			    return RET_CONTINUE;
			}
			
			public int allsphideCommand()
			{
				all_sprite_hide_flag = true;
			    for ( int i=0 ; i<MAX_SPRITE_NUM ; i++ ){
			        if ( sprite_info[i].visible )
			            dirty_rect.add( sprite_info[i].pos );
			    }
			    return RET_CONTINUE;
			}
			
			// Haeleth: Stub out some commands to suppress unwanted debug messages
			
			public int insertmenuCommand()
			{
				script_h.skipToken();
			    return RET_CONTINUE;
			}
			public int resetmenuCommand()
			{
			    script_h.skipToken();
			    return RET_CONTINUE;
			}
		}
	}
}
