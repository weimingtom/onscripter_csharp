﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 10:53
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
		 *  ONScripterLabel_file.cpp - FILE I/O of ONScripter-EN
		 *
		 *  Copyright (c) 2001-2010 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2008-2010 "Uncle" Mion Sonozaki
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
		
		// Modified by Mion, March 2008, to update from
		// Ogapee's 20080121 release source code.
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#endif
//		
//		#include "ONScripterLabel.h"
//		
//		#if defined(LINUX) || defined(MACOSX)
//		#include <sys/types.h>
//		#include <sys/stat.h>
//		#include <unistd.h>
//		#include <time.h>
//		#elif defined(WIN32)
//		#include <windows.h>
//		#elif defined(MACOS9)
//		#include <DateTimeUtils.h>
//		#include <Files.h>
//		extern "C" void c2pstrcpy(Str255 dst, const char *src);	//#include <TextUtils.h>
//		#elif defined(PSP)
//		#include <pspiofilemgr.h>
//		#endif
		
		private static CharPtr SAVEFILE_MAGIC_NUMBER = "ONS";
		private const int SAVEFILE_VERSION_MAJOR = 2;
		private const int SAVEFILE_VERSION_MINOR = 6;
		
		private const int READ_LENGTH = 4096;
		
		public partial class ONScripterLabel {
			public void searchSaveFile( SaveFileInfo save_file_info, int no )
			{
			    CharPtr file_name = new char[256];
			
			    bool use_fullwidth = (script_h.system_menu_script == ScriptHandler.LanguageScript.JAPANESE_SCRIPT);
			    script_h.getStringFromInteger( save_file_info.sjis_no, no,
			                                  (num_save_file >= 10)?2:1,
			                                  false, use_fullwidth );
			#if LINUX || MACOSX
			    if (script_h.savedir)
			        sprintf( file_name, "%ssave%d.dat", script_h.savedir, no );
			    else
			        sprintf( file_name, "%ssave%d.dat", script_h.save_path, no );
			    struct stat buf;
			    struct tm *tm;
			    if ( stat( file_name, &buf ) != 0 ){
			        save_file_info.valid = false;
			        return;
			    }
			    time_t mtime = buf.st_mtime;
			    tm = localtime( &mtime );
			
			    save_file_info.month  = tm->tm_mon + 1;
			    save_file_info.day    = tm->tm_mday;
			    save_file_info.hour   = tm->tm_hour;
			    save_file_info.minute = tm->tm_min;
			#elif true //defined(WIN32)
			    if (null!=script_h.savedir)
			        sprintf( file_name, "%ssave%d.dat", script_h.savedir, no );
			    else
			        sprintf( file_name, "%ssave%d.dat", script_h.save_path, no );
			    uint  handle;
			    FILETIME    tm = new FILETIME(), ltm = new FILETIME();
			    SYSTEMTIME  stm = new SYSTEMTIME();
			
			    handle = CreateFile( file_name, GENERIC_READ, 0, null,
			                         OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0 );
			    if ( handle == INVALID_HANDLE_VALUE ){
			        save_file_info.valid = false;
			        return;
			    }
			
			    GetFileTime( handle, null, null, tm );
			    FileTimeToLocalFileTime( tm, ltm );
			    FileTimeToSystemTime( ltm, stm );
			    CloseHandle( handle );
			
			    save_file_info.month  = stm.wMonth;
			    save_file_info.day    = stm.wDay;
			    save_file_info.hour   = stm.wHour;
			    save_file_info.minute = stm.wMinute;
			#elif MACOS9
			    if (script_h.savedir)
			        sprintf( file_name, "%ssave%d.dat", script_h.savedir, no );
			    else
			        sprintf( file_name, "%ssave%d.dat", script_h.save_path, no );
				CInfoPBRec  pb;
				Str255      p_file_name;
				FSSpec      file_spec;
				DateTimeRec tm;
				c2pstrcpy( p_file_name, file_name );
				if ( FSMakeFSSpec(0, 0, p_file_name, &file_spec) != noErr ){
					save_file_info.valid = false;
					return;
				}
				pb.hFileInfo.ioNamePtr = file_spec.name;
				pb.hFileInfo.ioVRefNum = file_spec.vRefNum;
				pb.hFileInfo.ioFDirIndex = 0;
				pb.hFileInfo.ioDirID = file_spec.parID;
				if (PBGetCatInfoSync(&pb) != noErr) {
					save_file_info.valid = false;
					return;
				}
				SecondsToDate( pb.hFileInfo.ioFlMdDat, &tm );
				save_file_info.month  = tm.month;
				save_file_info.day    = tm.day;
				save_file_info.hour   = tm.hour;
				save_file_info.minute = tm.minute;
			#elif PSP
			    if (script_h.savedir)
			        sprintf( file_name, "%ssave%d.dat", script_h.savedir, no );
			    else
			        sprintf( file_name, "%ssave%d.dat", script_h.save_path, no );
			    SceIoStat buf;
			    if ( sceIoGetstat(file_name, &buf)<0 ){
			        save_file_info.valid = false;
			        return;
			    }
			
			    save_file_info.month  = buf.st_mtime.month;
			    save_file_info.day    = buf.st_mtime.day;
			    save_file_info.hour   = buf.st_mtime.hour;
			    save_file_info.minute = buf.st_mtime.minute;
			#else
			    sprintf( file_name, "save%d.dat", no );
			    FILE *fp;
			    if ( (fp = fopen( file_name, "rb" )) == NULL ){
			        save_file_info.valid = false;
			        return;
			    }
			    fclose( fp );
			
			    save_file_info.month  = 1;
			    save_file_info.day    = 1;
			    save_file_info.hour   = 0;
			    save_file_info.minute = 0;
			#endif
			    save_file_info.valid = true;
			    script_h.getStringFromInteger( save_file_info.sjis_month,
			                                   save_file_info.month, 2,
			                                   false, use_fullwidth );
			    script_h.getStringFromInteger( save_file_info.sjis_day,
			                                   save_file_info.day, 2,
			                                   false, use_fullwidth );
			    script_h.getStringFromInteger( save_file_info.sjis_hour,
			                                   save_file_info.hour, 2,
			                                   false, use_fullwidth );
			    script_h.getStringFromInteger( save_file_info.sjis_minute,
			                                   save_file_info.minute, 2,
			                                   true, use_fullwidth );
			}
			
			public int loadSaveFile( int no, bool input_flag = true )
			{
				CharPtr filename = new char[16];
			    sprintf( filename, "save%d.dat", no );
			    if (0!=loadFileIOBuf( filename )){
			        //fprintf( stderr, "can't open save file %s\n", filename );
			        return -1;
			    }
			
			    CharPtr str = null;
			    int  i, j, k, address;
			    int  file_version;
			
			    /* ---------------------------------------- */
			    /* Load magic number */
			    for ( i=0 ; i<(int)strlen( SAVEFILE_MAGIC_NUMBER ) ; i++ )
			        if ( readChar() != SAVEFILE_MAGIC_NUMBER[i] ) break;
			    if ( i != (int)strlen( SAVEFILE_MAGIC_NUMBER ) ){ // if not ONS save file
			        file_io_buf_ptr = 0;
			        // check for ONS version 0
			        bool ons_ver0_flag = false;
			        if ( readInt() != 1 ) ons_ver0_flag = true;
			        for (i=0 ; i<4 ; i++) {
			            j = readInt();
			            if ( (j != 1) && (j != 0) ) ons_ver0_flag = true;
			        }
			        for (i=0 ; i<3 ; i++) {
			            j = readInt();
			            if ( (j < 0) || (j > 0xFF) ) ons_ver0_flag = true;
			        }
			
			        file_io_buf_ptr = 0;
			        if ( !ons_ver0_flag ){
			            printf("Save file version is unknown\n" );
			            return loadSaveFile2( fileversion, input_flag );
			        }
			        file_version = 0;
			    }
			    else{
			        file_version = readChar() * 100;
			        file_version += readChar();
			    }
			    //printf("Save file version is %d.%d\n", file_version/100, file_version%100 );
			    if ( file_version > SAVEFILE_VERSION_MAJOR*100 + SAVEFILE_VERSION_MINOR ){
			        fprintf( stderr, "Save file is newer than %d.%d, please use the latest ONScripter.\n", SAVEFILE_VERSION_MAJOR, SAVEFILE_VERSION_MINOR );
			        return -1;
			    }
			
			    if ( file_version >= 200 )
			        return loadSaveFile2( file_version, input_flag );
			
			    //The input_flag is only relevant for file_version 260+,
			    //so no need to check beyond this point
			    deleteNestInfo();
			
			    /* ---------------------------------------- */
			    /* Load text history */
			    if ( file_version >= 107 )
			        i = readInt();
			    else
			        i = 0;
			    sentence_font.setTateyokoMode( i );
			    int num_page = readInt();
			    for ( i=0 ; i<num_page ; i++ ){
			    	int[] num_xy = new int[2];
			        num_xy[0] = readInt();
			        num_xy[1] = readInt();
			        current_page.max_text = (num_xy[0]*2+1)*num_xy[1];
			        if (sentence_font.getTateyokoMode() == Fontinfo.TATE_MODE)
			            current_page.max_text = (num_xy[1]*2+1)*num_xy[0];
			        int[] xy = new int[2];
			        xy[0] = readInt();
			        xy[1] = readInt();
			        if ( null!=current_page.text ) current_page.text = null;//delete[] current_page->text;
			        current_page.text = new char[ current_page.max_text ];
			        current_page.text_count = 0;
			
			        char ch1, ch2;
			        for ( j=0, k=0 ; j<num_xy[0] * num_xy[1] ; j++ ){
			            ch1 = readChar();
			            ch2 = readChar();
			            if ( (byte) ch1 == 0x80 && (byte) ch2 == 0x40 ){
			                k += 2;
			            }
			            else{
			                if ( 0!=ch1 ){
			            		current_page.add( (byte)ch1 );
			                    k++;
			                }
			            	if ( 0!=(ch1 & 0x80) || 0!=ch2 ){
			                    current_page.add( (byte)ch2 );
			                    k++;
			                }
			            }
			            if ( k >= num_xy[0] * 2 ){
			                current_page.add( 0x0a );
			                k = 0;
			            }
			        }
			        current_page = current_page.next;
			        if ( i==0 ){
			            for ( j=0 ; j<max_page_list - num_page ; j++ )
			                current_page = current_page.next;
			            start_page = current_page;
			        }
			    }
			
			    /* ---------------------------------------- */
			    /* Load sentence font */
			    j = readInt();
			    //sentence_font.is_valid = (j==1)?true:false;
			    sentence_font.font_size_xy[0] = readInt();
			    if ( file_version >= 100 ){
			        sentence_font.font_size_xy[1] = readInt();
			    }
			    else{
			        sentence_font.font_size_xy[1] = sentence_font.font_size_xy[0];
			    }
			    sentence_font.top_xy[0] = readInt();
			    sentence_font.top_xy[1] = readInt();
			    sentence_font.num_xy[0] = readInt();
			    sentence_font.num_xy[1] = readInt();
			    sentence_font.xy[0] = readInt()*2;
			    sentence_font.xy[1] = readInt()*2;
			    sentence_font.pitch_xy[0] = readInt();
			    sentence_font.pitch_xy[1] = readInt();
			    sentence_font.wait_time = readInt();
			    sentence_font.is_bold = (readInt()==1)?true:false;
			    sentence_font.is_shadow = (readInt()==1)?true:false;
			    sentence_font.is_transparent = (readInt()==1)?true:false;
			
			    for (j=0, k=0, i=0 ; i<current_page.text_count ; i++){
			        if (j == sentence_font.xy[1] &&
			            (k > sentence_font.xy[0] ||
			             current_page.text[i] == 0x0a)) break;
			
			        if (current_page.text[i] == 0x0a){
			            j+=2;
			            k=0;
			        }
			        else
			            k++;
			    }
			    current_page.text_count = i;
			
			    /* Dummy, must be removed later !! */
			    for ( i=0 ; i<8 ; i++ ){
			        j = readInt();
			        //sentence_font.window_color[i] = j;
			    }
			    /* Should be char, not integer !! */
			    for ( i=0 ; i<3 ; i++ )
			    	sentence_font.window_color[i] = (byte)(readInt() & 0xff);
			    readStr( ref sentence_font_info.image_name );
			
			    sentence_font_info.orig_pos.x = readInt();
			    sentence_font_info.orig_pos.y = readInt();
			    sentence_font_info.orig_pos.w = readInt();
			    sentence_font_info.orig_pos.h = readInt();
			    UpdateAnimPosXY(sentence_font_info);
			    UpdateAnimPosWH(sentence_font_info);
			
			    if ( !sentence_font.is_transparent ){
			        parseTaggedString( sentence_font_info );
			        setupAnimationInfo( sentence_font_info );
			    }
			
			    sentence_font.ttf_font = null;
			
			    clickstr_state = readInt();
			    new_line_skip_flag = (readInt()==1)?true:false;
			    if ( file_version >= 103 ){
			        erase_text_window_mode = readInt();
			    }
			
			    /* ---------------------------------------- */
			    /* Load link label info */
			
			    int offset = 0;
			    while( true ){
			        readStr( ref str );
			        current_label_info = script_h.lookupLabel( str );
			
			        current_line = readInt() + 2;
			        CharPtr buf = current_label_info.label_header;
			        while( CharPtr.isLessThen(buf, current_label_info.start_address) ){
			        	if ( buf[0] == 0x0a ) current_line--;
			        	buf.inc();
			        }
			
			        offset = readInt();
			
			        script_h.setCurrent( current_label_info.label_header );
			        script_h.skipLine( current_line );
			
			        if ( file_version <= 104 )
			        {
			            if ( file_version >= 102 )
			                readInt();
			
			            address = readInt();
			        }
			        else{
			            offset += readInt();
			        }
			
			        if ( readChar() == 0 ) break;
			
			        last_nest_info.next = new NestInfo();
			        last_nest_info.next.previous = last_nest_info;
			        last_nest_info = last_nest_info.next;
			        last_nest_info.next_script = new CharPtr(script_h.getCurrent(), + offset);
			    }
			    script_h.setCurrent( new CharPtr(script_h.getCurrent(), + offset) );
			
			    int tmp_event_mode = readChar();
			
			    /* ---------------------------------------- */
			    /* Load variables */
			    readVariables( 0, 200 );
			
			    /* ---------------------------------------- */
			    /* Load monocro flag */
			    monocro_flag = (readChar()==1)?true:false;
			    if ( file_version >= 101 ){
			        monocro_flag = (readChar()==1)?true:false;
			    }
			    for ( i=0 ; i<3 ; i++ ) monocro_color[i] = (byte)(readChar() & 0xff);
			
			    if ( file_version >= 101 ){
			    	for ( i=0 ; i<3 ; i++ ) monocro_color[i] = (byte)(readChar() & 0xff);
			        readChar(); // obsolete, need_refresh_flag
			    }
			    for ( i=0 ; i<256 ; i++ ){
			    	monocro_color_lut[i][0] = (byte)(((monocro_color[0] * i) >> 8) & 0xff);
			        monocro_color_lut[i][1] = (byte)(((monocro_color[1] * i) >> 8) & 0xff);
			        monocro_color_lut[i][2] = (byte)(((monocro_color[2] * i) >> 8) & 0xff);
			    }
			
			    /* Load nega flag */
			    if ( file_version >= 104 ){
			        nega_mode = (byte)readChar();
			    }
			
			    /* ---------------------------------------- */
			    /* Load current images */
			    bg_info.remove();
			    bg_info.color[0] = (byte)readChar();
			    bg_info.color[1] = (byte)readChar();
			    bg_info.color[2] = (byte)readChar();
			    bg_info.num_of_cells = 1;
			    readStr( ref bg_info.file_name );
			    setupAnimationInfo( bg_info );
			    int bg_effect_image = readChar();
			
			    if (bg_effect_image == 0){ // COLOR_EFFECT_IMAGE
			        bg_info.allocImage( screen_width, screen_height );
			        bg_info.fill( bg_info.color[0], bg_info.color[1], bg_info.color[2], 0xff );
			        bg_info.pos.x = 0;
			        bg_info.pos.y = 0;
			    }
			    bg_info.trans_mode = AnimationInfo.TRANS_COPY;
			
			    for ( i=0 ; i<3 ; i++ )
			        tachi_info[i].remove();
			
			    for ( i=0 ; i<MAX_SPRITE_NUM ; i++ )
			        sprite_info[i].remove();
			
			    /* ---------------------------------------- */
			    /* Load Tachi image and Sprite */
			    for ( i=0 ; i<3 ; i++ ){
			        readStr( ref tachi_info[i].image_name );
			        if ( null!=tachi_info[i].image_name ){
			            parseTaggedString( tachi_info[i] );
			            setupAnimationInfo( tachi_info[ i ] );
			            tachi_info[ i ].pos.x = screen_width * (i+1) / 4 - tachi_info[ i ].pos.w / 2;
			            tachi_info[ i ].orig_pos.x = ContractPos(tachi_info[ i ].pos.x);
			            tachi_info[ i ].pos.y = ExpandPos(underline_value) - SDL_Surface_get_h(tachi_info[ i ].image_surface) + 1;
			            tachi_info[ i ].orig_pos.y = ContractPos(tachi_info[ i ].pos.y);
			        }
			    }
			
			    /* ---------------------------------------- */
			    /* Load current sprites */
			    for ( i=0 ; i<256 ; i++ ){
			        sprite_info[i].visible = (readInt()==1)?true:false;
			        sprite_info[i].orig_pos.x = readInt();
			        sprite_info[i].orig_pos.y = readInt();
			        UpdateAnimPosXY(sprite_info[i]);
			        sprite_info[i].trans = readInt();
			        readStr( ref sprite_info[i].image_name );
			        if ( null!=sprite_info[i].image_name ){
			            parseTaggedString( sprite_info[i] );
			            setupAnimationInfo( sprite_info[i] );
			        }
			    }
			
			    /* ---------------------------------------- */
			    /* Load current playing CD track */
			    stopCommand();
			    loopbgmstopCommand();
			    stopAllDWAVE();
			
			    current_cd_track = (sbyte)readChar();
			    bool play_once_flag = (readChar()==1)?true:false;
			    if ( current_cd_track == -2 ){
			        readStr( ref seqmusic_file_name );
			        seqmusic_play_loop_flag = !play_once_flag;
			        setStr( ref music_file_name, null );
			        music_play_loop_flag = false;
			    }
			    else{
			        readStr( ref music_file_name );
			        if ( null!=music_file_name ){
			            music_play_loop_flag = !play_once_flag;
			            cd_play_loop_flag = false;
			        }
			        else{
			            music_play_loop_flag = false;
			            cd_play_loop_flag = !play_once_flag;
			        }
			        setStr( ref seqmusic_file_name, null );
			        seqmusic_play_loop_flag = false;
			    }
			
			    if ( current_cd_track >= 0 ){
			        playCDAudio();
			    }
			    else if ( null!=seqmusic_file_name && seqmusic_play_loop_flag ){
			        playSound(seqmusic_file_name, SOUND_SEQMUSIC, seqmusic_play_loop_flag);
			    }
			    else if ( null!=music_file_name && music_play_loop_flag ){
			        playSound(music_file_name,
			                  SOUND_WAVE | SOUND_OGG_STREAMING | SOUND_MP3,
			                  music_play_loop_flag, MIX_BGM_CHANNEL);
			    }
			
			    /* ---------------------------------------- */
			    /* Load rmode flag */
			    rmode_flag = (readChar()==1)?true:false;
			
			    /* ---------------------------------------- */
			    /* Load text on flag */
			    text_on_flag = (readChar()==1)?true:false;
			
			
			    restoreTextBuffer();
			    num_chars_in_sentence = 0;
			    cached_page = current_page;
			
			    display_mode = shelter_display_mode = DISPLAY_MODE_TEXT;
			
			    event_mode = tmp_event_mode;
			    if ( 0!=(event_mode & WAIT_BUTTON_MODE) ) event_mode = WAIT_SLEEP_MODE; // Re-execute the selectCommand, etc.
			
			    if ( 0!=(event_mode & WAIT_SLEEP_MODE) )
			        event_mode &= ~WAIT_SLEEP_MODE;
			    else
			        event_mode |= WAIT_TIMER_MODE;
			    if (0!=(event_mode & WAIT_INPUT_MODE)) event_mode |= WAIT_TEXT_MODE;
			
			    draw_cursor_flag = (clickstr_state == CLICK_NONE)?false:true;
			
			    return 0;
			}
			
			public void saveMagicNumber( bool output_flag )
			{
			    for ( uint i=0 ; i<strlen( SAVEFILE_MAGIC_NUMBER ) ; i++ )
			        writeChar( SAVEFILE_MAGIC_NUMBER[i], output_flag );
			    writeChar( (char)SAVEFILE_VERSION_MAJOR, output_flag );
			    writeChar( (char)SAVEFILE_VERSION_MINOR, output_flag );
			}
			
			public int saveSaveFile( int no, CharPtr savestr=null )
			{
				// make save data structure on memory
			    if ((no < 0) || (saveon_flag && internal_saveon_flag)){
			        file_io_buf_ptr = 0;
			        saveMagicNumber( false );
			        saveSaveFile2( false );
			        allocFileIOBuf();
			        saveMagicNumber( true );
			        saveSaveFile2( true );
			        save_data_len = file_io_buf_ptr;
			        memcpy(save_data_buf, file_io_buf, save_data_len);
			    }
			
			    if ( no >= 0 ){
			        saveAll();
			
			        CharPtr filename = new char[16];
			        sprintf( filename, "save%d.dat", no );
			
			        memcpy(file_io_buf, save_data_buf, save_data_len);
			        file_io_buf_ptr = save_data_len;
			        if (0!=saveFileIOBuf( filename, 0, savestr )){
			            return -1;
			        }
			
			        uint magic_len = strlen(SAVEFILE_MAGIC_NUMBER)+2;
			        sprintf( filename, "sav%csave%d.dat", DELIMITER, no );
			        if (0!=saveFileIOBuf( filename, (int)magic_len, savestr ))
			            fprintf( stderr, "can't open save file %s for writing (not an error)\n", filename );
			    }
			
			    return 0;
			}
		}
	}
}
