/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 11:30
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
		 *  ONScripterLabel_rmenu.cpp - Right click menu handler of ONScripter-EN
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
		
		// Modified by Mion, March 2008, to update from
		// Ogapee's 20080121 release source code.
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#endif
//		
//		#include "ONScripterLabel.h"
		
		private static string[][] messages = new string[2][] {
			new string[]{ "`%s%s    Date %s/%s    Time %s:%s",
		      "`%s%s    ------------------------",
		      "`Save in %s%s?",
		      "`Load from %s%s?",
		      "`Return to Title Menu?",
		      "`Quit?",
		      "Yes",
		      "No" },
		    new string[]{ "%s%s丂%s寧%s擔%s帪%s暘",
		      "%s%s丂亅亅亅亅亅亅亅亅亅亅亅亅",
		      "%s%s偵僙乕僽偟傑偡丅傛傠偟偄偱偡偐丠",
		      "%s%s傪儘乕僪偟傑偡丅傛傠偟偄偱偡偐丠",
		      "儕僙僢僩偟傑偡丅傛傠偟偄偱偡偐丠",
		      "廔椆偟傑偡丅傛傠偟偄偱偡偐丠",
		      "偼偄",
		      "偄偄偊" }
		};
		
		public partial class ONScripterLabel {
			public CharPtr getMessageString( MessageId which )
			{
				if (script_h.system_menu_script != ScriptHandler.LanguageScript.JAPANESE_SCRIPT)
					return messages[0][(int)which];
			    else
			        return messages[1][(int)which];
			}
			
			public void enterSystemCall()
			{
			    shelter_button_link = root_button_link.next;
			    root_button_link.next = null;
			    shelter_select_link = root_select_link.next;
			    root_select_link.next = null;
			    exbtn_d_shelter_button_link = exbtn_d_button_link;
			    exbtn_d_button_link.exbtn_ctl = null;
			    shelter_event_mode = event_mode;
			    shelter_text_info.deepcopy(text_info);
			    shelter_mouse_state.x = last_mouse_state.x;
			    shelter_mouse_state.y = last_mouse_state.y;
			    event_mode = IDLE_EVENT_MODE;
			    shelter_display_mode = display_mode;
			    display_mode = DISPLAY_MODE_TEXT;
			    shelter_draw_cursor_flag = draw_cursor_flag;
			    draw_cursor_flag = false;
			}
			
			public void leaveSystemCall( bool restore_flag = true )
			{
			    bool tmp = txtbtn_show;
			    txtbtn_show = false;
			
			    current_font = sentence_font;
			    key_pressed_flag = false;
			    current_button_state.reset();
			
			    if ( restore_flag ){
			
			        current_page = cached_page;
			        text_info.deepcopy(shelter_text_info);
			        shelter_text_info.reset();
			        root_button_link.next = shelter_button_link;
			        root_select_link.next = shelter_select_link;
			        exbtn_d_button_link = exbtn_d_shelter_button_link;
			
			        //use windoweffect on a return from windowerase systemcall
			        if (system_menu_mode == SYSTEM_WINDOWERASE) {
			            display_mode = shelter_display_mode;
			            refreshSurface( effect_dst_surface, null, refreshMode() );
			            system_menu_mode = SYSTEM_NULL;
			            if (!setEffect(window_effect, false, true)) {
			                while(doEffect(window_effect, false));
			            }
			        }
			
			        erasetextbtnCommand();
			        event_mode = shelter_event_mode;
			        draw_cursor_flag = shelter_draw_cursor_flag;
			        if ( 0!=(event_mode & WAIT_BUTTON_MODE) ){
			            //SDL_WarpMouse( shelter_mouse_state.x, shelter_mouse_state.y );
			        }
			    }
			    display_mode = shelter_display_mode;
			    dirty_rect.fill( screen_width, screen_height );
			    flush( refreshMode() );
			
			    //printf("leaveSystemCall %d %d\n",event_mode, clickstr_state);
			
			    refreshMouseOverButton();
			    txtbtn_show = tmp;
			
			    system_menu_mode = SYSTEM_NULL;
			}
			
			public bool executeSystemCall()
			{
				enterSystemCall();
			
			    while(system_menu_mode != SYSTEM_NULL){
			        dirty_rect.fill( screen_width, screen_height );
			        switch( system_menu_mode ){
			              case SYSTEM_SKIP:
			            executeSystemSkip();
			            return true;
			          case SYSTEM_RESET:
			            if (executeSystemReset()) return true;
			            break;
			          case SYSTEM_SAVE:
			            executeSystemSave();
			            break;
			          case SYSTEM_LOAD:
			            if (executeSystemLoad()) return true;
			            break;
			          case SYSTEM_LOOKBACK:
			            executeSystemLookback();
			            break;
			          case SYSTEM_WINDOWERASE:
			            executeWindowErase();
			            break;
			          case SYSTEM_MENU:
			            executeSystemMenu();
			            break;
			          case SYSTEM_AUTOMODE:
			            executeSystemAutomode();
			            return true;
			          case SYSTEM_END:
			            executeSystemEnd();
			            break;
			          default:
			            leaveSystemCall();
			            break;
			        }
			    }
			
			    return false;
			}
			
			public void executeSystemMenu()
			{
			    current_font = menu_font;
			
			    if ( null!=menuselectvoice_file_name[MENUSELECTVOICE_OPEN] )
			        playSound(menuselectvoice_file_name[MENUSELECTVOICE_OPEN],
			                  SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			
			    text_info.fill( 0, 0, 0, 0 );
			    flush( refreshMode() );
			
			    menu_font.num_xy[0] = (int)rmenu_link_width;
			    menu_font.num_xy[1] = (int)rmenu_link_num;
			    menu_font.top_xy[0] = (ContractPos(screen_width) - menu_font.num_xy[0] * menu_font.pitch_xy[0]) / 2;
			    menu_font.top_xy[1] = (ContractPos(screen_height) - menu_font.num_xy[1] * menu_font.pitch_xy[1]) / 2;
			    menu_font.setXY( (int)((menu_font.num_xy[0] - rmenu_link_width) / 2),
			                     (int)((menu_font.num_xy[1] - rmenu_link_num) / 2) );
			
			    RMenuLink link = root_rmenu_link.next;
			    int counter = 1;
			    while( null!=link ){
			        ButtonLink button = getSelectableSentence( link.label, menu_font, false );
			        root_button_link.insert( button );
			        button.no = counter++;
			
			        link = link.next;
			        flush( refreshMode() );
			    }
			
			    flushEvent();
			    refreshMouseOverButton();
			
			    event_mode = WAIT_BUTTON_MODE;
			    do waitEventSub(-1);
			    while (current_button_state.button == 0);
			
			    deleteButtonLink();
			
			    if ( current_button_state.button == -1 ){
			        if ( null!=menuselectvoice_file_name[MENUSELECTVOICE_CANCEL] )
			            playSound(menuselectvoice_file_name[MENUSELECTVOICE_CANCEL],
			                      SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			        leaveSystemCall();
			        return;
			    }
			
			    if ( null!=menuselectvoice_file_name[MENUSELECTVOICE_CLICK] )
			        playSound(menuselectvoice_file_name[MENUSELECTVOICE_CLICK],
			                  SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			
			    link = root_rmenu_link.next;
			    counter = 1;
			    while ( null!=link ){
			        if ( current_button_state.button == counter++ ){
			            system_menu_mode = link.system_call_no;
			            break;
			        }
			        link = link.next;
			    }
			}
			
			public void executeSystemSkip()
			{
			    skip_mode |= SKIP_NORMAL;
			    leaveSystemCall();
			}
			
			public void executeSystemAutomode()
			{
			    automode_flag = true;
			    skip_mode &= ~SKIP_NORMAL;
			    printf("systemcall_automode: change to automode\n");
			    leaveSystemCall();
			}
			
			public bool executeSystemReset()
			{
				if ( executeSystemYesNo( SYSTEM_RESET ) ){
			        resetCommand();
			        leaveSystemCall( false );
			        
			        return true;
			    }
			
			    leaveSystemCall();
			
			    return false;
			}
			
			public void executeSystemEnd()
			{
			    if ( executeSystemYesNo( SYSTEM_END ) )
			        endCommand();
			    else
			        leaveSystemCall();
			}
			
			public void executeWindowErase()
			{
			    if (windowchip_sprite_no >= 0)
			        sprite_info[windowchip_sprite_no].visible = false;
			
			    //use the windoweffect when executing the erase
			    refreshSurface(effect_dst_surface, dirty_rect.bounding_box,
			                   mode_saya_flag ? REFRESH_SAYA_MODE : REFRESH_NORMAL_MODE);
			    int shelter_menu_mode = system_menu_mode;
			    system_menu_mode = SYSTEM_NULL;  //prevents infinite event looping
			    if (!setEffect(window_effect, false, false)) {
			        while(doEffect(window_effect, false));
			    }
			    system_menu_mode = shelter_menu_mode;
			
			    event_mode = WAIT_TIMER_MODE | WAIT_BUTTON_MODE;
			    waitEventSub(-1);
			
			    if (windowchip_sprite_no >= 0)
			        sprite_info[windowchip_sprite_no].visible = true;
			
			    leaveSystemCall();
			}
			
			public bool executeSystemLoad()
			{
				current_font = menu_font;
			
			    text_info.fill( 0, 0, 0, 0 );
			
			    menu_font.num_xy[0] = (int)((strlen(save_item_name)+1)/2+2+13);
			    menu_font.num_xy[1] = (int)(num_save_file+2);
			    menu_font.top_xy[0] = (ContractPos(screen_width) - menu_font.num_xy[0] * menu_font.pitch_xy[0]) / 2;
			    menu_font.top_xy[1] = (ContractPos(screen_height) - menu_font.num_xy[1] * menu_font.pitch_xy[1]) / 2;
			    menu_font.setXY( (int)((menu_font.num_xy[0] - (strlen( load_menu_name )+1) / 2) / 2), 0 );
			    //Mion: fixed the menu title bug noted in the past by Seung Park:
			    // the menu title must be drawn close to last during refresh,
			    // not in the textwindow, since there could be sprites above the
			    // window if windowback is used.
			    if (null!=system_menu_title){
			        dirty_rect.add( system_menu_title.pos );
			        //delete system_menu_title;
			        system_menu_title = null;
			    }
			    system_menu_title = getSentence( load_menu_name, menu_font, 1, false );
			    menu_font.newLine();
			
			    flush( refreshMode() );
			
			    bool nofile_flag;
			    int slen = (int)strlen(save_item_name);
			    CharPtr buffer = new char[ slen + (slen % 2) + 30 + 3 ];
			
			    SaveFileInfo save_file_info = new SaveFileInfo();
			    for ( uint i=1 ; i<=num_save_file ; i++ ){
			    	searchSaveFile( save_file_info, (int)i );
			    	menu_font.setXY( (int)((menu_font.num_xy[0] - ((strlen( save_item_name )+1) / 2 + 15) ) / 2) );
			
			        if ( save_file_info.valid ){
			            sprintf( buffer, getMessageString(MessageId.MESSAGE_SAVE_EXIST),
			                     save_item_name,
			                     save_file_info.sjis_no,
			                     save_file_info.sjis_month,
			                     save_file_info.sjis_day,
			                     save_file_info.sjis_hour,
			                     save_file_info.sjis_minute );
			            nofile_flag = false;
			        }
			        else{
			            sprintf( buffer, getMessageString(MessageId.MESSAGE_SAVE_EMPTY),
			                     save_item_name,
			                     save_file_info.sjis_no );
			            nofile_flag = true;
			        }
			        ButtonLink button = getSelectableSentence( buffer, menu_font, false, nofile_flag );
			        root_button_link.insert( button );
			        button.no = (int)i;
			        flush( refreshMode() );
			    }
			    buffer = null;//delete[] buffer;
			
			    refreshMouseOverButton();
			
			    event_mode = WAIT_BUTTON_MODE;
			    do {
			        waitEventSub(-1);
			        if (current_button_state.button > 0){
			            int file_no = current_button_state.button;
			            searchSaveFile( save_file_info, file_no );
			            if ( !save_file_info.valid ){
			                refreshMouseOverButton();
			                current_button_state.button = 0;
			            }
			        }
			    } while (current_button_state.button == 0);
			
			    deleteButtonLink();
			    if ( null!=system_menu_title ) {
			        dirty_rect.add( system_menu_title.pos );
			        //delete system_menu_title;
			        system_menu_title = null;
			    }
			
			    if ( current_button_state.button > 0 ){
			        int file_no = current_button_state.button;
			        searchSaveFile( save_file_info, file_no );
			        if ( !save_file_info.valid ){
			            refreshMouseOverButton();
			            return false;
			        }
			
			        if (executeSystemYesNo( SYSTEM_LOAD, file_no )){
			            current_font = sentence_font;
			            if ( 0!=loadSaveFile( file_no ) )
			                return false;
			
			            leaveSystemCall( false );
			            saveon_flag = true;
			            internal_saveon_flag = true;
			            text_on_flag = false;
			            indent_offset = 0;
			            line_enter_status = 0;
			            page_enter_status = 0;
			            string_buffer_offset = 0;
			            break_flag = false;
			
			            if (null!=loadgosub_label)
			                gosubReal( loadgosub_label, script_h.getCurrent() );
			            return true;
			        }
			
			        return false;
			    }
			
			    leaveSystemCall();
			    
			    return false;
			}
			
			public void executeSystemSave()
			{
			    current_font = menu_font;
			
			    text_info.fill( 0, 0, 0, 0 );
			
			    menu_font.num_xy[0] = (int)((strlen(save_item_name)+1)/2+2+13);
			    menu_font.num_xy[1] = (int)(num_save_file+2);
			    menu_font.top_xy[0] = (ContractPos(screen_width) - menu_font.num_xy[0] * menu_font.pitch_xy[0]) / 2;
			    menu_font.top_xy[1] = (ContractPos(screen_height) - menu_font.num_xy[1] * menu_font.pitch_xy[1]) / 2;
			    menu_font.setXY((int)(((int)menu_font.num_xy[0] - (strlen( save_menu_name )+1) / 2 ) / 2), 0);
			    //Mion: fixed the menu title bug noted in the past by Seung Park:
			    // the menu title must be drawn close to last during refresh,
			    // not in the textwindow, since there could be sprites above the
			    // window if windowback is used.
			    if (null!=system_menu_title){
			        dirty_rect.add( system_menu_title.pos );
			        //delete system_menu_title;
			        system_menu_title = null;
			    }
			    system_menu_title = getSentence( save_menu_name, menu_font, 1, false );
			    menu_font.newLine();
			
			    flush( refreshMode() );
			
			    bool nofile_flag;
			    int slen = (int)strlen(save_item_name);
			    CharPtr buffer = new char[ slen + (slen % 2) + 30 + 3 ];
			
			    for ( uint i=1 ; i<=num_save_file ; i++ ){
			    	
			    	SaveFileInfo save_file_info = new SaveFileInfo();
			        searchSaveFile( save_file_info, (int)i );
			        menu_font.setXY( (int)((menu_font.num_xy[0] - ((strlen( save_item_name )+1) / 2 + 15) ) / 2) );
			
			        if ( save_file_info.valid ){
			            sprintf( buffer, getMessageString(MessageId.MESSAGE_SAVE_EXIST),
			                     save_item_name,
			                     save_file_info.sjis_no,
			                     save_file_info.sjis_month,
			                     save_file_info.sjis_day,
			                     save_file_info.sjis_hour,
			                     save_file_info.sjis_minute );
			            nofile_flag = false;
			        }
			        else{
			            sprintf( buffer, getMessageString(MessageId.MESSAGE_SAVE_EMPTY),
			                     save_item_name,
			                     save_file_info.sjis_no );
			            nofile_flag = true;
			        }
			        ButtonLink button = getSelectableSentence( buffer, menu_font, false, nofile_flag );
			        root_button_link.insert( button );
			        button.no = (int)i;
			        flush( refreshMode() );
			    }
			    buffer = null;//delete[] buffer;
			
			    refreshMouseOverButton();
			
			    event_mode = WAIT_BUTTON_MODE;
			    do waitEventSub(-1);
			    while (current_button_state.button == 0);
			
			    deleteButtonLink();
			    if (null!=system_menu_title){
			        dirty_rect.add( system_menu_title.pos );
			        //delete system_menu_title;
			        system_menu_title = null;
			    }
			
			    if ( current_button_state.button > 0 ){
			        int file_no = current_button_state.button;
			        if (executeSystemYesNo( SYSTEM_SAVE, file_no )){
			            saveSaveFile( file_no );
			            leaveSystemCall();
			        }
			        return;
			    }
			
			    leaveSystemCall();
			}
			
			public bool executeSystemYesNo( int caller, int file_no=0 )
			{
				current_font = menu_font;
			
			    text_info.fill( 0, 0, 0, 0 );
			    dirty_rect.fill( screen_width, screen_height );
			
			    char[] name = new char[64];
			
			    if ( caller == SYSTEM_SAVE ){
			    	SaveFileInfo save_file_info = new SaveFileInfo();
			        searchSaveFile( save_file_info, file_no );
			        sprintf( name, getMessageString(MessageId.MESSAGE_SAVE_CONFIRM),
			                 save_item_name,
			                 save_file_info.sjis_no );
			    }
			    else if ( caller == SYSTEM_LOAD ){
			        SaveFileInfo save_file_info = new SaveFileInfo();
			        searchSaveFile( save_file_info, file_no );
			        sprintf( name, getMessageString(MessageId.MESSAGE_LOAD_CONFIRM),
			                 save_item_name,
			                 save_file_info.sjis_no );
			    }
			    else if ( caller ==  SYSTEM_RESET )
			        strcpy( name, getMessageString(MessageId.MESSAGE_RESET_CONFIRM) );
			    else if ( caller ==  SYSTEM_END )
			        strcpy( name, getMessageString(MessageId.MESSAGE_END_CONFIRM) );
			
			
			    menu_font.num_xy[0] = (int)strlen(name)/2;
			    menu_font.num_xy[1] = 3;
			    menu_font.top_xy[0] = (ContractPos(screen_width) - menu_font.num_xy[0] * menu_font.pitch_xy[0]) / 2;
			    menu_font.top_xy[1] = (ContractPos(screen_height) - menu_font.num_xy[1] * menu_font.pitch_xy[1]) / 2;
			    menu_font.setXY(0, 0);
			    //Mion: fixed the menu title bug noted in the past by Seung Park:
			    // the menu title must be drawn close to last during refresh,
			    // not in the textwindow, since there could be sprites above the
			    // window if windowback is used.
			    if (null!=system_menu_title){
			        dirty_rect.add( system_menu_title.pos );
			        //delete system_menu_title;
			        system_menu_title = null;
			    }
			    system_menu_title = getSentence( name, menu_font, 1, false );
			
			    flush( refreshMode() );
			
			    int offset1 = (int)strlen(name)/5;
			    int offset2 = (int)strlen(name)/2 - offset1;
			    strcpy( name, getMessageString(MessageId.MESSAGE_YES) );
			    menu_font.setXY(offset1-2, 2);
			    ButtonLink button = getSelectableSentence( name, menu_font, false );
			    root_button_link.insert( button );
			    button.no = 1;
			
			    strcpy( name, getMessageString(MessageId.MESSAGE_NO) );
			    menu_font.setXY(offset2, 2);
			    button = getSelectableSentence( name, menu_font, false );
			    root_button_link.insert( button );
			    button.no = 2;
			
			    flush( refreshMode() );
			
			    refreshMouseOverButton();
			
			    event_mode = WAIT_BUTTON_MODE;
			    do waitEventSub(-1);
			    while (current_button_state.button == 0);
			
			    deleteButtonLink();
			    if (null!=system_menu_title){
			        dirty_rect.add( system_menu_title.pos );
			        //delete system_menu_title;
			        system_menu_title = null;
			    }
			
			    if ( current_button_state.button == 1 ){ // yes is selected
			        if ( null!=menuselectvoice_file_name[MENUSELECTVOICE_YES] )
			            playSound(menuselectvoice_file_name[MENUSELECTVOICE_YES],
			                      SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			        return true;
			    }
			    else{
			        if ( null!=menuselectvoice_file_name[MENUSELECTVOICE_NO] )
			            playSound(menuselectvoice_file_name[MENUSELECTVOICE_NO],
			                      SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			        return false;
			    }
			}
			
			public void setupLookbackButton()
			{
			    deleteButtonLink();
			
			    /* ---------------------------------------- */
			    /* Previous button check */
			    if ( (current_page.previous.text_count > 0 ) &&
			         current_page != start_page ){
			        ButtonLink button = new ButtonLink();
			        root_button_link.insert( button );
			
			        button.no = 1;
			        button.select_rect.x = sentence_font_info.pos.x;
			        button.select_rect.y = sentence_font_info.pos.y;
			        button.select_rect.w = sentence_font_info.pos.w;
			        button.select_rect.h = sentence_font_info.pos.h/3;
			
			        if ( lookback_sp[0] >= 0 ){
			            button.button_type = ButtonLink.BUTTON_TYPE.SPRITE_BUTTON;
			            button.sprite_no = lookback_sp[0];
			            sprite_info[ button.sprite_no ].visible = true;
			            button.image_rect = sprite_info[ button.sprite_no ].pos;
			        }
			        else{
			            button.button_type = ButtonLink.BUTTON_TYPE.LOOKBACK_BUTTON;
			            button.show_flag = 2;
			            button.anim[0] = lookback_info[0];
			            button.anim[1] = lookback_info[1];
			            button.image_rect.x = sentence_font_info.pos.x + sentence_font_info.pos.w - button.anim[0].pos.w;
			            button.image_rect.y = sentence_font_info.pos.y;
			            button.image_rect.w = button.anim[0].pos.w;
			            button.image_rect.h = button.anim[0].pos.h;
			            button.anim[0].pos.x = button.anim[1].pos.x = button.image_rect.x;
			            button.anim[0].pos.y = button.anim[1].pos.y = button.image_rect.y;
			        }
			    }
			    else if (lookback_sp[0] >= 0){
			        sprite_info[ lookback_sp[0] ].visible = false;
			    }
			
			    /* ---------------------------------------- */
			    /* Next button check */
			    if ( current_page.next != cached_page ){
			        ButtonLink button = new ButtonLink();
			        root_button_link.insert( button );
			
			        button.no = 2;
			        button.select_rect.x = sentence_font_info.pos.x;
			        button.select_rect.y = sentence_font_info.pos.y + sentence_font_info.pos.h*2/3;
			        button.select_rect.w = sentence_font_info.pos.w;
			        button.select_rect.h = sentence_font_info.pos.h/3;
			
			        if ( lookback_sp[1] >= 0 ){
			            button.button_type = ButtonLink.BUTTON_TYPE.SPRITE_BUTTON;
			            button.sprite_no = lookback_sp[1];
			            sprite_info[ button.sprite_no ].visible = true;
			            button.image_rect = sprite_info[ button.sprite_no ].pos;
			        }
			        else{
			            button.button_type = ButtonLink.BUTTON_TYPE.LOOKBACK_BUTTON;
			            button.show_flag = 2;
			            button.anim[0] = lookback_info[2];
			            button.anim[1] = lookback_info[3];
			            button.image_rect.x = sentence_font_info.pos.x + sentence_font_info.pos.w - button.anim[0].pos.w;
			            button.image_rect.y = sentence_font_info.pos.y + sentence_font_info.pos.h - button.anim[0].pos.h;
			            button.image_rect.w = button.anim[0].pos.w;
			            button.image_rect.h = button.anim[0].pos.h;
			            button.anim[0].pos.x = button.anim[1].pos.x = button.image_rect.x;
			            button.anim[0].pos.y = button.anim[1].pos.y = button.image_rect.y;
			        }
			    }
			    else if (lookback_sp[1] >= 0){
			        sprite_info[ lookback_sp[1] ].visible = false;
			    }
			}
			
			public void executeSystemLookback()
			{
				byte[] color = new byte[3];
			
			    current_font = sentence_font;
			
			    current_page = current_page.previous;
			    if ( current_page.text_count == 0 ){
			        if ( lookback_sp[0] >= 0 )
			            sprite_info[ lookback_sp[0] ].visible = false;
			        if ( lookback_sp[1] >= 0 )
			            sprite_info[ lookback_sp[1] ].visible = false;
			        leaveSystemCall();
			        return;
			    }
			
			    while (true){
			        setupLookbackButton();
			        refreshMouseOverButton();
			
			        setColor(color, current_font.color);
			        setColor(current_font.color, lookback_color);
			        restoreTextBuffer();
			        setColor(current_font.color, color);
			
			        dirty_rect.fill( screen_width, screen_height );
			        flush( refreshMode() );
			
			        event_mode = WAIT_BUTTON_MODE;
			        waitEventSub(-1);
			
			        if ( current_button_state.button == 0 ||
			             ( current_page == start_page &&
			               current_button_state.button == -2 ) ){
			            continue;
			        }
			        if ( current_button_state.button == -1 ||
			             ( current_button_state.button == -3 &&
			               current_page.next == cached_page ) ||
			             current_button_state.button <= -4 )
			        {
			            event_mode = IDLE_EVENT_MODE;
			            deleteButtonLink();
			            if ( lookback_sp[0] >= 0 )
			                sprite_info[ lookback_sp[0] ].visible = false;
			            if ( lookback_sp[1] >= 0 )
			                sprite_info[ lookback_sp[1] ].visible = false;
			            leaveSystemCall();
			            return;
			        }
			
			        if ( current_button_state.button == 1 ||
			             current_button_state.button == -2 ){
			            current_page = current_page.previous;
			        }
			        else
			            current_page = current_page.next;
			    }
			}
		}
	}
}
