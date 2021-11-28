/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 11:47
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
		 *  ONScripterLabel_sound.cpp - Methods for playing sound for ONScripter-EN
		 *
		 *  Copyright (c) 2001-2011 Ogapee. All rights reserved.
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
		
		// Modified by Haeleth, Autumn 2006, to better support OS X/Linux packaging.
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#pragma warning(disable:4717)
//		#endif
//		
//		#include "ONScripterLabel.h"
//		#include <new>
//		#ifdef LINUX
//		#include <signal.h>
//		#endif
//		
//		#ifdef USE_AVIFILE
//		#include "AVIWrapper.h"
//		#endif
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
		
		public class WAVE_HEADER{
		    public char[] chunk_riff = new char[4];
		    public char[] riff_length = new char[4];
		    public char[] fmt_id = new char[8];
		    public char[] fmt_size = new char[4];
		    public char[] data_fmt = new char[2];
		    public char[] channels = new char[2];
		    public char[] frequency = new char[4];
		    public char[] byte_size = new char[4];
		    public char[] sample_byte_size = new char[2];
		    public char[] sample_bit_size = new char[2];
		
		    public char[] chunk_id = new char[4];
		    public char[] data_length = new char[4];
		}
		private static WAVE_HEADER header = new WAVE_HEADER();
		
//		static inline void clearTimer(SDL_TimerID &timer_id)
//		{
//		    clearTimer( timer_id );
//		}
//		
//		extern bool ext_music_play_once_flag;
//		
//		extern "C"{
//		    extern void mp3callback( void *userdata, Uint8 *stream, int len );
//		    extern void oggcallback( void *userdata, Uint8 *stream, int len );
//		    extern Uint32 silentmovieCallback( Uint32 interval, void *param );
//		#if defined(MACOSX) //insani
//		    extern Uint32 seqmusicSDLCallback( Uint32 interval, void *param );
//		#endif
//		}
//		extern void seqmusicCallback( int sig );
//		extern void musicCallback( int sig );
//		extern SDL_TimerID timer_cdaudio_id;
//		extern SDL_TimerID timer_silentmovie_id;
//		
//		#if defined(MACOSX) //insani
//		extern SDL_TimerID timer_seqmusic_id;
//		#endif
		
		public const string TMP_SEQMUSIC_FILE = "tmp.mus";
		public const string TMP_MUSIC_FILE = "tmp.mus";
		
//		#define SWAP_SHORT_BYTES(sptr){          \
//		            Uint8 *bptr = (Uint8 *)sptr; \
//		            Uint8 tmpb = *bptr;          \
//		            *bptr = *(bptr+1);           \
//		            *(bptr+1) = tmpb;            \
//		        }
		
		//WMA header format
		public static bool IS_ASF_HDR(UnsignedCharPtr buf) { return
		         ((buf[0] == 0x30) && (buf[1] == 0x26) &&
		          (buf[2] == 0xb2) && (buf[3] == 0x75) &&
		          (buf[4] == 0x8e) && (buf[5] == 0x66) &&
		          (buf[6] == 0xcf) && (buf[7] == 0x11)); }
		
		//AVI header format
		public static bool IS_AVI_HDR(UnsignedCharPtr buf) { return
		         ((buf[0] == 'R') && (buf[1] == 'I') &&
		          (buf[2] == 'F') && (buf[3] == 'F') &&
		          (buf[8] == 'A') && (buf[9] == 'V') &&
		          (buf[10] == 'I')); }
		
		public static long decodeOggVorbis(ONScripterLabel.MusicStruct music_struct, Uint8Ptr buf_dst, long len, bool do_rate_conversion)
		{
			long total_len = 0;
		
		    return total_len;
		}
		
		public partial class ONScripterLabel {
			public int playSound(CharPtr filename, int format, bool loop_flag, int channel=0)
			{
				if ( !audio_open_flag ) return SOUND_NONE;
			
			    long length = script_h.cBR.getFileLength( filename );
			    if (length == 0) return SOUND_NONE;
			
			    //Mion: account for mode_wave_demo setting
			    //(i.e. if not set, then don't play non-bgm wave/ogg during skip mode)
			    if (!mode_wave_demo_flag &&
			        ( 0!=(skip_mode & SKIP_NORMAL) || 0!=ctrl_pressed_status )) {
			        if (0!=(format & (SOUND_OGG | SOUND_WAVE)) &&
			            ((channel < ONS_MIX_CHANNELS) || (channel == MIX_WAVE_CHANNEL) ||
			             (channel == MIX_CLICKVOICE_CHANNEL)))
			            return SOUND_NONE;
			    }
			
			    UnsignedCharPtr buffer;
			
			    if (0!=(format & (SOUND_MP3 | SOUND_OGG_STREAMING)) && 
			        (length == music_buffer_length) &&
			        null!=music_buffer ){
			        buffer = music_buffer;
			    }
			    else{
			    	buffer = new UnsignedCharPtr(new byte[length]);
			        if (buffer == null) {
			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                     "failed to load sound file [%s] (%lu bytes)",
			                     filename, length);
			            errorAndCont( script_h.errbuf, "unable to allocate buffer", "Memory Issue" );
			            return SOUND_NONE;
			        }
			    	int _temp = 0;
			        script_h.cBR.getFile( filename, buffer, ref _temp);
			    }
			
			    if (0!=(format & (SOUND_OGG | SOUND_OGG_STREAMING))){
			        int ret = playOGG(format, buffer, length, loop_flag, channel);
			        if (0!=(ret & (SOUND_OGG | SOUND_OGG_STREAMING))) return ret;
			    }
			
			    /* check for WMA (i.e. ASF header format) */
			    if ( IS_ASF_HDR(buffer) ){
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			        "sound file '%s' is in WMA format, skipping", filename);
			        errorAndCont(script_h.errbuf);
			        buffer = null;//delete[] buffer;
			        return SOUND_OTHER;
			    }
			
			    /* check for AVI header format */
			    if ( IS_AVI_HDR(buffer) ){
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			        "sound file '%s' is in AVI format, skipping", filename);
			        errorAndCont(script_h.errbuf);
			        buffer = null;//delete[] buffer;
			        return SOUND_OTHER;
			    }
			
			    if (0!=(format & SOUND_WAVE)){
			    	if (strncmp(CharPtr.fromUnsignedCharPtr(buffer), "RIFF", 4) != 0) {
			            // bad (encrypted?) header; need to recreate
			            // assume the first 64 bytes are bad (encrypted)
			            int channels, rate, bits;
			            CharPtr fmtname = new char[strlen(filename) + strlen(".fmt") + 1];
			            sprintf(fmtname, "%s.fmt", filename);
			            uint fmtlen = script_h.cBR.getFileLength( fmtname );
			            if ( fmtlen > 0) {
			                // a file called filename + ".fmt" exists; read fmt info
			                UnsignedCharPtr buffer2 = new UnsignedCharPtr(new byte[fmtlen]);
			                int _temp = 0;
			                script_h.cBR.getFile( fmtname, buffer2, ref _temp );
			                channels = buffer2[0];
			                rate = 0;
			                for (int i=5; i>1; i--) {
			                    rate = (rate << 8) + buffer2[i];
			                }
			                bits = buffer2[6];
			                buffer2 = null;//delete[] buffer2;
			#if _MSC_VER
							{
			#endif
			                for (int i=0; i<64; i++) {
			                    buffer[i] = 0;
			                }
			#if _MSC_VER
							}
			#endif
			                setupWaveHeader(buffer, channels, rate, bits,
			                	(ulong)(length - sizeof_WAVE_HEADER()));
			            }
			            fmtname = null;//delete[] fmtname;
			        }
			    }
			
			    if (0!=(format & SOUND_MP3)){
			        if (null!=music_cmd){
			            FILEPtr fp;
			            if ( (fp = fopen(TMP_MUSIC_FILE, "wb", true)) == null){
			                snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                         "can't open temporary music file %s", TMP_MUSIC_FILE);
			                errorAndCont(script_h.errbuf);
			            }
			            else{
			            	if (fwrite(buffer, 1, (uint)length, fp) != (uint)length){
			                    snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                             "can't write to temporary music file %s", TMP_MUSIC_FILE);
			                    errorAndCont(script_h.errbuf);
			                }
			                fclose( fp );
			                ext_music_play_once_flag = !loop_flag;
			                if (playExternalMusic(loop_flag) == 0){
			                    music_buffer = buffer;
			                    music_buffer_length = length;
			                    return SOUND_MP3;
			                }
			            }
			        }
			    }
			
			    if (0!=(format & SOUND_SEQMUSIC)){
			        FILEPtr fp;
			        if ( (fp = fopen(TMP_SEQMUSIC_FILE, "wb", true)) == null){
			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                     "can't open temporary music file %s", TMP_SEQMUSIC_FILE);
			            errorAndCont(script_h.errbuf);
			        }
			        else{
			        	if (fwrite(buffer, 1, (uint)length, fp) != (uint)length){
			                snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                         "can't write to temporary music file %s",
			                         TMP_SEQMUSIC_FILE);
			                errorAndCont(script_h.errbuf);
			            }
			            fclose( fp );
			            ext_music_play_once_flag = !loop_flag;
			            if (playSequencedMusic(loop_flag) == 0){
			                buffer = null;//delete[] buffer;
			                return SOUND_SEQMUSIC;
			            }
			        }
			    }
			
			    buffer = null; //delete[] buffer;
			
			    return SOUND_OTHER;
			}
			
			public void playCDAudio()
			{
			    if (!audio_open_flag) return;
			
			    if ( cdaudio_flag ) {
			
			    }
			    else{
			        //if CD audio is not available, search the "cd" subfolder
			        //for a file named "track01.mp3" or similar, depending on the
			        //track number; check for mp3, ogg and wav files
			        CharPtr filename = new char[256];
			        sprintf( filename, "cd\\track%2.2d.mp3", current_cd_track );
			        int ret = playSound( filename, SOUND_MP3, cd_play_loop_flag );
			        if (ret == SOUND_MP3) return;
			
			        sprintf( filename, "cd\\track%2.2d.ogg", current_cd_track );
			        ret = playSound( filename, SOUND_OGG_STREAMING, cd_play_loop_flag );
			        if (ret == SOUND_OGG_STREAMING) return;
			
			        sprintf( filename, "cd\\track%2.2d.wav", current_cd_track );
			        ret = playSound( filename, SOUND_WAVE, cd_play_loop_flag, MIX_BGM_CHANNEL );
			    }
			}
			
			public int playMP3()
			{
			    return 0;
			}
			
			public int playOGG(int format, UnsignedCharPtr buffer, long length, bool loop_flag, int channel)
			{
				return SOUND_OTHER;
			}
			
			public int playExternalMusic(bool loop_flag)
			{
				int music_looping = loop_flag ? -1 : 0;
			#if LINUX
			    signal(SIGCHLD, musicCallback);
			    if (music_cmd) music_looping = 0;
			#endif
			
			    CharPtr music_filename = new char[256];
			    sprintf(music_filename, "%s%s", script_h.save_path, TMP_MUSIC_FILE);
			
			    // Mix_VolumeMusic( music_volume );
			
			    return 0;
			}
			
			public int playSequencedMusic(bool loop_flag)
			{
				CharPtr seqmusic_filename = new char[256];
			    sprintf(seqmusic_filename, "%s%s", script_h.save_path, TMP_SEQMUSIC_FILE);
			    int seqmusic_looping = loop_flag ? -1 : 0;
			
			#if LINUX
			    signal(SIGCHLD, seqmusicCallback);
			    if (seqmusic_cmd) seqmusic_looping = 0;
			#endif
			
			#if MACOSX //insani
			    // Emulate looping on MacOS ourselves to work around bug in SDL_Mixer
			    seqmusic_looping = 0;
			    timer_seqmusic_id = SDL_AddTimer(1000, seqmusicSDLCallback, NULL);
			#else
			#endif
			    current_cd_track = -2;
			
			    return 0;
			}
			
			public int playingMusic()
			{
				return 0;
			}
			
			public int setCurMusicVolume( int volume )
			{
				if (!audio_open_flag) return 0;
			
			    return 0;
			}
			
			public int setVolumeMute( bool do_mute )
			{
				if (!audio_open_flag) return 0;
			
			    int music_vol = music_volume;
			
			    return 0;
			}
			
			public int playMPEG( CharPtr filename, bool async_flag, bool use_pos, int xpos, int ypos, int width, int height )
			{
				int ret = 0;
			
			    return ret;
			}
			
			public int playAVI( CharPtr filename, bool click_flag )
			{
			#if USE_AVIFILE
			    char *absolute_filename = new char[ strlen(archive_path) + strlen(filename) + 1 ];
			    sprintf( absolute_filename, "%s%s", archive_path, filename );
			    for ( unsigned int i=0 ; i<strlen( absolute_filename ) ; i++ )
			        if ( absolute_filename[i] == '/' ||
			             absolute_filename[i] == '\\' )
			            absolute_filename[i] = DELIMITER;
			
			    AVIWrapper *avi = new AVIWrapper();
			    if ( avi->init( absolute_filename, false ) == 0 &&
			         avi->initAV( screen_surface, audio_open_flag ) == 0 ){
			        if (avi->play( click_flag )) return 1;
			    }
			    delete avi;
			    delete[] absolute_filename;
			
			    if ( audio_open_flag ){
			    }
			#else
			    errorAndCont( "avi: avi video playback is disabled." );
			#endif
			
			    return 0;
			}
			
			public void stopBGM( bool continue_flag )
			{
			    if ( !continue_flag ){
			        setStr( ref music_file_name, null );
			        music_play_loop_flag = false;
			        if ( null!=music_buffer ){
			            //delete[] music_buffer;
			            music_buffer = null;
			        }
			    }
			
			    if ( !continue_flag ){
			        setStr( ref seqmusic_file_name, null );
			        seqmusic_play_loop_flag = false;
			    }
			
			    if ( !continue_flag ) current_cd_track = -1;
			}
			
			public void stopAllDWAVE()
			{
			    if (!audio_open_flag) return;
			
			    // just in case the bgm was turned down for the voice channel,
			    // set the bgm volume back to normal
			    if (bgmdownmode_flag)
			        setCurMusicVolume( music_volume );
			}
			
			public void playClickVoice()
			{
			    if ( clickstr_state == CLICK_NEWPAGE ){
			        if ( null!=clickvoice_file_name[CLICKVOICE_NEWPAGE] )
			            playSound(clickvoice_file_name[CLICKVOICE_NEWPAGE],
			                      SOUND_WAVE|SOUND_OGG, false, MIX_CLICKVOICE_CHANNEL);
			    }
			    else if ( clickstr_state == CLICK_WAIT ){
			        if ( null!=clickvoice_file_name[CLICKVOICE_NORMAL] )
			            playSound(clickvoice_file_name[CLICKVOICE_NORMAL],
			                      SOUND_WAVE|SOUND_OGG, false, MIX_CLICKVOICE_CHANNEL);
			    }
			}
			
			public void setupWaveHeader( UnsignedCharPtr buffer, int channels, int rate, int bits, ulong data_length )
			{
			    memcpy( header.chunk_riff, "RIFF", 4 );
			    int riff_length = (int)(sizeof_WAVE_HEADER() + (int)data_length - 8);
			    header.riff_length[0] = (char)(riff_length & 0xff);
			    header.riff_length[1] = (char)((riff_length >> 8) & 0xff);
			    header.riff_length[2] = (char)((riff_length >> 16) & 0xff);
			    header.riff_length[3] = (char)((riff_length >> 24) & 0xff);
			    memcpy( header.fmt_id, "WAVEfmt ", 8 );
			    header.fmt_size[0] = (char)0x10;
			    header.fmt_size[1] = header.fmt_size[2] = header.fmt_size[3] = (char)0;
			    header.data_fmt[0] = (char)(1); header.data_fmt[1] = (char)0; // PCM format
			    header.channels[0] = (char)channels; header.channels[1] = (char)0;
			    header.frequency[0] = (char)(rate & 0xff);
			    header.frequency[1] = (char)((rate >> 8) & 0xff);
			    header.frequency[2] = (char)((rate >> 16) & 0xff);
			    header.frequency[3] = (char)((rate >> 24) & 0xff);
			
			    int sample_byte_size = channels * bits / 8;
			    int byte_size = sample_byte_size * rate;
			    header.byte_size[0] = (char)(byte_size & 0xff);
			    header.byte_size[1] = (char)((byte_size >> 8) & 0xff);
			    header.byte_size[2] = (char)((byte_size >> 16) & 0xff);
			    header.byte_size[3] = (char)((byte_size >> 24) & 0xff);
                header.sample_byte_size[0] = (char)(sample_byte_size);
                header.sample_byte_size[1] = (char)(0);
                header.sample_bit_size[0] = (char)(bits);
                header.sample_bit_size[1] = (char)(0);
			
			    memcpy( header.chunk_id, "data", 4 );
			    header.data_length[0] = (char)(data_length & 0xff);
			    header.data_length[1] = (char)((data_length >> 8) & 0xff);
			    header.data_length[2] = (char)((data_length >> 16) & 0xff);
			    header.data_length[3] = (char)((data_length >> 24) & 0xff);
			
			    memcpy( buffer, header, (uint)(sizeof_WAVE_HEADER()) );
			}
			
			public OVInfo openOggVorbis( UnsignedCharPtr buf, long len, ref int channels, ref int rate )
			{
				OVInfo ovi = null;
			    return ovi;
			}
			
			public int closeOggVorbis(OVInfo ovi)
			{
				if (null!=ovi.buf){
			        ovi.buf = null;
			    }
			    ovi = null;//delete ovi;
			    return 0;
			}
		}
	}
}
