/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:16
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
		 *  ONScripterLabel.h - Execution block parser of ONScripter-EN
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
		
		// Modified by Haeleth, autumn 2006, to remove unnecessary diagnostics,
		// and on many occasions thereafter; see SVN logs for all changes
		
		// Modified by Mion, March 2008, to update from
		// Ogapee's 20080121 release source code.
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
//		#ifndef __ONSCRIPTER_LABEL_H__
//		#define __ONSCRIPTER_LABEL_H__
//		
//		#include "DirPaths.h"
//		#include "ScriptParser.h"
//		#include "DirtyRect.h"
//		#include <SDL.h>
//		#include <SDL_image.h>
//		#include <SDL_ttf.h>
//		
//		#define DEFAULT_VIDEO_SURFACE_FLAG (SDL_SWSURFACE)
		
		public const int DEFAULT_BLIT_FLAG = (0);
		//#define DEFAULT_BLIT_FLAG (SDL_RLEACCEL)
		
		public const int MAX_SPRITE_NUM = 1000;
		public const int MAX_SPRITE2_NUM = 256;
		public const int MAX_PARAM_NUM = 100;
		public const int CUSTOM_EFFECT_NO = 100;
		
		public const int DEFAULT_VOLUME = 100;
		public const int ONS_MIX_CHANNELS = 50;
		public const int ONS_MIX_EXTRA_CHANNELS = 5;
		public const int MIX_WAVE_CHANNEL = (ONS_MIX_CHANNELS+0);
		public const int MIX_CLICKVOICE_CHANNEL = (ONS_MIX_CHANNELS+1);
		public const int MIX_BGM_CHANNEL = (ONS_MIX_CHANNELS+2);
		public const int MIX_LOOPBGM_CHANNEL0 = (ONS_MIX_CHANNELS+3);
		public const int MIX_LOOPBGM_CHANNEL1 = (ONS_MIX_CHANNELS+4);
		
//		#if defined(PDA) && !defined(PSP)
//		#define DEFAULT_AUDIO_RATE 22050
//		#else
//		#define DEFAULT_AUDIO_RATE 44100
//		#endif
		
		public static CharPtr DEFAULT_WM_TITLE = "ONScripter-EN";
		public static CharPtr DEFAULT_WM_ICON = "Ons-en";
		
		private const int NUM_GLYPH_CACHE = 30;
		
		public const SDLKey KEYPRESS_NULL = ((SDLKey)(SDLKey.SDLK_LAST+1)); // "null" for keypress variables
		
		public partial class ONScripterLabel : ScriptParser
		{			
//		public:
//		    typedef AnimationInfo::ONSBuf ONSBuf;
//		
//		    ONScripterLabel();
//		    ~ONScripterLabel();
//		
//		    void executeLabel();
//		    void runScript();
//		
//		    // ----------------------------------------
//		    // start-up options
//		    void enableCDAudio();
//		    void setCDNumber(int cdrom_drive_number);
//		    void setFontFile(const char *filename);
//		    void setRegistryFile(const char *filename);
//		    void setDLLFile(const char *filename);
//		    void setFileVersion(const char *ver);
//		    void setFullscreenMode();
//		    void setWindowMode();
//		#ifdef WIN32
//		    void setUserAppData();
//		#endif
//		    void setUseAppIcons();
//		    void setIgnoreTextgosubNewline();
//		    void setSkipPastNewline();
//		    void setPreferredWidth(const char *widthstr);
//		    void enableButtonShortCut();
//		    void enableWheelDownAdvance();
//		    void disableCpuGfx();
//		    void disableRescale();
//		    void enableEdit();
//		    void setKeyEXE(const char *path);
//		#ifdef RCA_SCALE
//		    void setWidescreen();
//		#endif
//		    void setScaled();
//		    inline void setStrict() { script_h.strict_warnings = true; }
//		    void setGameIdentifier(const char *gameid);
		    
		    public const int PNG_MASK_AUTODETECT    = 0;
		    public const int PNG_MASK_USE_ALPHA     = 1;
		    public const int PNG_MASK_USE_NSCRIPTER = 2;
		    
//		    inline void setMaskType( int mask_type ) { png_mask_type = mask_type; }
		    public void setEnglishDefault()
		        { script_h.default_script = ScriptHandler.LanguageScript.LATIN_SCRIPT; }
//		    inline void setJapaneseDefault()
//		        { script_h.default_script = ScriptHandler::JAPANESE_SCRIPT; }
//		    inline void setEnglishPreferred()
//		        { script_h.preferred_script = ScriptHandler::LATIN_SCRIPT; }
//		    inline void setJapanesePreferred()
//		        { script_h.preferred_script = ScriptHandler::JAPANESE_SCRIPT; }
		    public void setEnglishMenu()
		        { script_h.system_menu_script = ScriptHandler.LanguageScript.LATIN_SCRIPT; }
//		    inline void setJapaneseMenu()
//		        { script_h.system_menu_script = ScriptHandler::JAPANESE_SCRIPT; }
//		
//		    int  init();
//		    void runEventLoop();
//		
//		    void reset(); // used if definereset
//		    void resetSub(); // used if reset
//		    void resetFlags(); // for resetting (non-pointer) definereset variables
//		    void resetFlagsSub(); // for resetting (non-pointer) reset variables
//		
//		    //Mion: routines for error handling & cleanup
//		    bool doErrorBox( const char *title, const char *errstr, bool is_simple=false, bool is_warning=false );
//		#ifdef WIN32
//		    void openDebugFolders();
//		#endif
//		    /* ---------------------------------------- */
//		    /* Commands */
//		    int yesnoboxCommand();
//		    int wavestopCommand();
//		    int waveCommand();
//		    int waittimerCommand();
//		    int waitCommand();
//		    int vspCommand();
//		    int voicevolCommand();
//		    int vCommand();
//		    int trapCommand();
//		    int transbtnCommand();
//		    int textspeeddefaultCommand();
//		    int textspeedCommand();
//		    int textshowCommand();
//		    int textonCommand();
//		    int textoffCommand();
//		    int texthideCommand();
//		    int textexbtnCommand();
//		    int textclearCommand();
//		    int textbtnstartCommand();
//		    int textbtnoffCommand();
//		    int texecCommand();
//		    int tateyokoCommand();
//		    int talCommand();
//		    int tablegotoCommand();
//		    int systemcallCommand();
//		    int strspCommand();
//		    int stopCommand();
//		    int sp_rgb_gradationCommand();
//		    int spstrCommand();
//		    int spreloadCommand();
//		    int splitCommand();
//		    int spclclkCommand();
//		    int spbtnCommand();
//		    int skipoffCommand();
//		    int shellCommand();
//		    int sevolCommand();
//		    int setwindow3Command();
//		    int setwindow2Command();
//		    int setwindowCommand();
//		    int seteffectspeedCommand();
//		    int setcursorCommand();
//		    int selectCommand();
//		    int savetimeCommand();
//		    int saveonCommand();
//		    int saveoffCommand();
//		    int savegameCommand();
//		    int savefileexistCommand();
//		    int savescreenshotCommand();
//		    int rndCommand();
//		    int rmodeCommand();
//		    int resettimerCommand();
//		    int resetCommand();
//		    int repaintCommand();
//		    int quakeCommand();
//		    int puttextCommand();
//		    int prnumclearCommand();
//		    int prnumCommand();
//		    int printCommand();
//		    int playstopCommand();
//		    int playonceCommand();
//		    int playCommand();
//		    int ofscopyCommand();
//		    int negaCommand();
//		    int mvCommand();
//		    int mspCommand();
//		    int mp3volCommand();
//		    int mp3stopCommand();
//		    int mp3fadeoutCommand();
//		    int mp3fadeinCommand();
//		    int mp3Command();
//		    int movieCommand();
//		    int movemousecursorCommand();
//		    int mousemodeCommand();
//		    int monocroCommand();
//		    int minimizewindowCommand();
//		    int mesboxCommand();
//		    int menu_windowCommand();
//		    int menu_waveonCommand();
//		    int menu_waveoffCommand();
//		    int menu_fullCommand();
//		    int menu_click_pageCommand();
//		    int menu_click_defCommand();
//		    int menu_automodeCommand();
//		    int lsp2Command();
//		    int lspCommand();
//		    int loopbgmstopCommand();
//		    int loopbgmCommand();
//		    int lookbackflushCommand();
//		    int lookbackbuttonCommand();
//		    int logspCommand();
//		    int locateCommand();
//		    int loadgameCommand();
//		    int linkcolorCommand();
//		    int ldCommand();
//		    int languageCommand();
//		    int jumpfCommand();
//		    int jumpbCommand();
//		    int ispageCommand();
//		    int isfullCommand();
//		    int isskipCommand();
//		    int isdownCommand();
//		    int inputCommand();
//		    int indentCommand();
//		    int humanorderCommand();
//		    int getzxcCommand();
//		    int getvoicevolCommand();
//		    int getversionCommand();
//		    int gettimerCommand();
//		    int gettextbtnstrCommand();
//		    int gettextCommand();
//		    int gettaglogCommand();
//		    int gettagCommand();
//		    int gettabCommand();
//		    int getspsizeCommand();
//		    int getspmodeCommand();
//		    int getskipoffCommand();
//		    int getsevolCommand();
//		    int getscreenshotCommand();
//		    int getsavestrCommand();
//		    int getretCommand();
//		    int getregCommand();
//		    int getpageupCommand();
//		    int getpageCommand();
//		    int getmp3volCommand();
//		    int getmouseposCommand();
//		    int getmouseoverCommand();
//		    int getmclickCommand();
//		    int getlogCommand();
//		    int getinsertCommand();
//		    int getfunctionCommand();
//		    int getenterCommand();
//		    int getcursorposCommand();
//		    int getcursorCommand();
//		    int getcselstrCommand();
//		    int getcselnumCommand();
//		    int gameCommand();
//		    int flushoutCommand();
//		    int fileexistCommand();
//		    int exec_dllCommand();
//		    int exbtnCommand();
//		    int erasetextwindowCommand();
//		    int erasetextbtnCommand();
//		    int endCommand();
//		    int effectskipCommand();
//		    int dwavestopCommand();
//		    int dwaveCommand();
//		    int dvCommand();
//		    int drawtextCommand();
//		    int drawsp3Command();
//		    int drawsp2Command();
//		    int drawspCommand();
//		    int drawfillCommand();
//		    int drawclearCommand();
//		    int drawbg2Command();
//		    int drawbgCommand();
//		    int drawCommand();
//		    int deletescreenshotCommand();
//		    int delayCommand();
//		    int defineresetCommand();
//		    int cspCommand();
//		    int cselgotoCommand();
//		    int cselbtnCommand();
//		    int clickCommand();
//		    int clCommand();
//		    int chvolCommand();
//		    int checkpageCommand();
//		    int checkkeyCommand();
//		    int cellCommand();
//		    int captionCommand();
//		    int btnwait2Command();
//		    int btnwaitCommand();
//		    int btntime2Command();
//		    int btntimeCommand();
//		    int btndownCommand();
//		    int btndefCommand();
//		    int btnareaCommand();
//		    int btnCommand();
//		    int brCommand();
//		    int bltCommand();
//		    int bgmdownmodeCommand();
//		    int bgcopyCommand();
//		    int bgCommand();
//		    int barclearCommand();
//		    int barCommand();
//		    int aviCommand();
//		    int automode_timeCommand();
//		    int autoclickCommand();
//		    int allsp2resumeCommand();
//		    int allspresumeCommand();
//		    int allsp2hideCommand();
//		    int allsphideCommand();
//		    int amspCommand();
//		
//		    int insertmenuCommand();
//		    int resetmenuCommand();
//		    int layermessageCommand();
//		
//		protected:
		    /* ---------------------------------------- */
		    /* Event related variables */
		    
		    public const int NOT_EDIT_MODE            = 0;
		    public const int EDIT_SELECT_MODE         = 1;
		    public const int EDIT_VOLUME_MODE         = 2;
		    public const int EDIT_VARIABLE_INDEX_MODE = 3;
		    public const int EDIT_VARIABLE_NUM_MODE   = 4;
		    public const int EDIT_MP3_VOLUME_MODE     = 5;
		    public const int EDIT_VOICE_VOLUME_MODE   = 6;
		    public const int EDIT_SE_VOLUME_MODE      = 7;
		    
		
		    public int variable_edit_mode;
		    //Mion: These 3 variables are only used in _event;
		    //  could they be static variables there instead of
		    //  instance variables here?
		    public int variable_edit_index;
		    public int variable_edit_num;
		    public int variable_edit_sign;
		
		    public bool key_pressed_flag;
		    public int  shift_pressed_status;
		    public int  ctrl_pressed_status;
//		#ifdef MACOSX
//		    int apple_pressed_status;
//		#endif
		    public bool bgmdownmode_flag;
		    // the default behavior when in "click to skip" mode is to stop
		    // the skip at the next clickwait or newline, whichever comes first.
		    //Since some very old onscripters didn't stop at newlines,
		    // setting skip_past_newline will help when playing older onscripter games.
		    public bool skip_past_newline; // don't leave 'click to skip' mode at a newline in text cmds
		
//		    SDL_keysym transKey(SDL_keysym key, bool isdown);
//		    void variableEditMode( SDL_KeyboardEvent *event );
//		    bool keyDownEvent( SDL_KeyboardEvent *event );
//		    void keyUpEvent( SDL_KeyboardEvent *event );
//		    bool keyPressEvent( SDL_KeyboardEvent *event );
//		    bool mousePressEvent( SDL_MouseButtonEvent *event );
//		    bool mouseMoveEvent( SDL_MouseMotionEvent *event );
//		    void animEvent();
//		    void timerEvent();
//		    void flushEventSub( SDL_Event &event );
//		    void flushEvent();
//		    void advancePhase( int count=0 );
//		    void advanceAnimPhase( int count=0 );
//		    void waitEventSub(int count);
//		    bool waitEvent(int count);
//		    void trapHandler();
//		    void initSDL();
		
//		private:
		    
		    public const int DISPLAY_MODE_NORMAL  = 0; 
		    public const int DISPLAY_MODE_TEXT    = 1;
		    public const int DISPLAY_MODE_UPDATED = 2;

		    
		    public const int IDLE_EVENT_MODE      = 0;
		    public const int WAIT_RCLICK_MODE     = 1;   // for lrclick
		    public const int WAIT_BUTTON_MODE     = 2;   // For select, btnwait and rmenu.
		    public const int WAIT_INPUT_MODE      = 4;   // can be skipped by a click
		    public const int WAIT_TEXTOUT_MODE    = 8;   // can be skipped by a click
		    public const int WAIT_SLEEP_MODE      = 16;  // cannot be skipped by ctrl but not click
		    public const int WAIT_TIMER_MODE      = 32;
		    public const int WAIT_TEXTBTN_MODE    = 64;
		    public const int WAIT_VOICE_MODE      = 128;
		    public const int WAIT_TEXT_MODE       = 256; // clickwait, newpage, select
		    public const int WAIT_NO_ANIM_MODE    = 512;
		    
		    
		    public const int EFFECT_DST_GIVEN     = 0;
		    public const int EFFECT_DST_GENERATE  = 1;
		    
		    
		    public const int ALPHA_BLEND_CONST          = 1;
		    public const int ALPHA_BLEND_MULTIPLE       = 2;
		    public const int ALPHA_BLEND_FADE_MASK      = 3;
		    public const int ALPHA_BLEND_CROSSFADE_MASK = 4;
		    
		
		    // ----------------------------------------
		    // start-up options
		    public bool cdaudio_flag;
		    public CharPtr default_font;
		    public CharPtr registry_file;
		    public CharPtr dll_file;
		    public CharPtr getret_str;
		    public int  getret_int;
		    public bool enable_wheeldown_advance_flag;
		    public bool disable_rescale_flag;
		    public bool edit_flag;
		    public CharPtr key_exe_file;
		#if RCA_SCALE
		    bool widescreen_flag;
		#endif
		    public bool scaled_flag;
		
//		    //Mion: inlines for image/screen resizing & scaling
//		    int ExpandPos(int val);
//		    int ContractPos(int val);
//		#ifdef RCA_SCALE
//		    int StretchPosX(int val);
//		    int StretchPosY(int val);
//		#else
//		    inline int StretchPosX(int val) { return ExpandPos(val); }
//		    inline int StretchPosY(int val) { return ExpandPos(val); }
//		#endif
		    public void UpdateAnimPosXY(AnimationInfo animp) {
		        animp.pos.x = ExpandPos(animp.orig_pos.x);
		        animp.pos.y = ExpandPos(animp.orig_pos.y);
		    }
		    public void UpdateAnimPosWH(AnimationInfo animp) {
		        animp.pos.w = ExpandPos(animp.orig_pos.w);
		        animp.pos.h = ExpandPos(animp.orig_pos.h);
		    }
		    public void UpdateAnimPosStretchXY(AnimationInfo animp) {
		        animp.pos.x = StretchPosX(animp.orig_pos.x);
		        animp.pos.y = StretchPosY(animp.orig_pos.y);
		    }
		    public void UpdateAnimPosStretchWH(AnimationInfo animp) {
		        animp.pos.w = StretchPosX(animp.orig_pos.w);
		        animp.pos.h = StretchPosY(animp.orig_pos.h);
		    }
		
		    // ----------------------------------------
		    // Global definitions
		    public long internal_timer;
		    public bool automode_flag;
		    public long automode_time;
		    public long autoclick_time;
		
		    public bool saveon_flag;
		    public bool internal_saveon_flag; // to saveoff at the head of text
		
		    public bool monocro_flag;
		    public byte[] monocro_color = new byte[3];
		    public byte[][] monocro_color_lut = init_monocro_color_lut();
		    private static byte[][] init_monocro_color_lut() {
		    	byte[][] ret = new byte[256][];
		    	for (int i = 0; i < ret.Length; ++i)
		    	{
		    		ret[i] = new byte[3];
		    	}
		    	return ret;
		    }
		    public int  nega_mode;
		
		    
		    public const int TRAP_NONE        = 0;
		    public const int TRAP_LEFT_CLICK  = 1;
		    public const int TRAP_RIGHT_CLICK = 2;
		    public const int TRAP_NEXT_SELECT = 4;
		    public const int TRAP_STOP        = 8;
		    
		    public int  trap_mode;
		    public CharPtr trap_dest; //label to jump to when trapped
		    public CharPtr wm_title_string;
		    public CharPtr wm_icon_string;
		    public CharPtr wm_edit_string = new char[256];
		    public bool fullscreen_mode;
		    public bool window_mode; //ons-specific, for cmd-line option --window
		    public int fileversion;
		#if true//def WIN32
		    public bool current_user_appdata;
		#endif
		    public bool use_app_icons;
		
		    public bool btntime2_flag;
		    public long btntime_value;
		    public long btnwait_time;
		    public bool btndown_flag;
		    public bool transbtn_flag;
		
		    public SDLKey last_keypress;
		
//		    void quit();
		
		    /* ---------------------------------------- */
		    /* Script related variables */
		    
		    public const int REFRESH_NONE_MODE   = 0;
		    public const int REFRESH_NORMAL_MODE = 1;
		    public const int REFRESH_SAYA_MODE   = 2;
		    public const int REFRESH_WINDOW_MODE = 4;  //show textwindow background
		    public const int REFRESH_TEXT_MODE   = 8;  //show textwindow text
		    public const int REFRESH_CURSOR_MODE = 16; //show textwindow cursor
		    
		
		    public int refresh_window_text_mode;
		    public int display_mode;
		    public bool did_leavetext;
		    public int event_mode;
		    public SDL_Surface accumulation_surface; // Final image, i.e. picture_surface (+ text_window + text_surface)
		    public SDL_Surface backup_surface; // Final image w/o (text_window + text_surface) used in leaveTextDisplayMode()
		    public SDL_Surface screen_surface; // Text + Select_image + Tachi image + background
		    public SDL_Surface effect_dst_surface; // Intermediate source buffer for effect
		    public SDL_Surface effect_src_surface; // Intermediate destination buffer for effect
		    public SDL_Surface effect_tmp_surface; // Intermediate buffer for effect
		    public SDL_Surface screenshot_surface; // Screenshot
		    public SDL_Surface image_surface; // Reference for loadImage() - 32bpp
		
		    public UnsignedCharPtr tmp_image_buf;
		    public ulong tmp_image_buf_length;
		    public ulong mean_size_of_loaded_images;
		    public ulong num_loaded_images;
		
		    /* ---------------------------------------- */
		    /* Button related variables */
		    public AnimationInfo btndef_info = new AnimationInfo();
		
		    public class ButtonState{
		        public int x, y, button;
		        public bool down_flag, valid_flag;
		        //Mion - initialize the button
		        public ButtonState()
		        { x = (0); y = (0); button = (0); down_flag = (false); valid_flag = (false);
		        }
		        public void reset(){ //Mion - clear the button state
		            button = 0;
		            valid_flag = false;
		        }
		        public void set(int val){ //Mion - set button & valid_flag
		            button = val;
		            valid_flag = true;
		        }
		    } 
		    ButtonState current_button_state = new ButtonState(), volatile_button_state = new ButtonState(), last_mouse_state = new ButtonState(), shelter_mouse_state = new ButtonState();
		
		    public class ButtonLink{
		        public enum BUTTON_TYPE {
		            NORMAL_BUTTON     = 0,
		            SPRITE_BUTTON     = 1,
		            EX_SPRITE_BUTTON  = 2,
		            LOOKBACK_BUTTON   = 3,
		            TMP_SPRITE_BUTTON = 4,
		            TEXT_BUTTON       = 5
		        }
		
		        public ButtonLink next;
		        public ButtonLink same; //Mion: to link buttons that act in concert
		        public BUTTON_TYPE button_type;
		        public int no;
		        public int sprite_no;
		        public CharPtr exbtn_ctl;
		        public int show_flag; // 0...show nothing, 1... show anim[0], 2 ... show anim[1]
		        public SDL_Rect select_rect = new SDL_Rect();
		        public SDL_Rect image_rect = new SDL_Rect();
		        public AnimationInfo[] anim = new AnimationInfo[2];
		
		        public ButtonLink(){
		            button_type = BUTTON_TYPE.NORMAL_BUTTON;
		            next = null;
		            same = null;
		            exbtn_ctl = null;
		            anim[0] = anim[1] = null;
		            show_flag = 0;
		        }
		        ~ButtonLink(){
		            if ((button_type == BUTTON_TYPE.NORMAL_BUTTON || 
		                 button_type == BUTTON_TYPE.TMP_SPRITE_BUTTON ||
		                 button_type == BUTTON_TYPE.TEXT_BUTTON) && null!=anim[0]) anim[0] = null;//delete anim[0];
		            if ( null!=exbtn_ctl ) exbtn_ctl = null;//delete[] exbtn_ctl;
		        }
		        public void insert( ButtonLink button ){
		            button.next = this.next;
		            this.next = button;
		        }
		        public void connect( ButtonLink button ){
		            button.same = this.same;
		            this.same = button;
		        }
		        public void removeSprite( int no ){
		            ButtonLink p = this;
		            while(null!=p.next){
		                if ((p.next.sprite_no == no) &&
		                    ( (p.next.button_type == BUTTON_TYPE.SPRITE_BUTTON) ||
		                      (p.next.button_type == BUTTON_TYPE.EX_SPRITE_BUTTON) )){
		                    ButtonLink p2 = p.next;
		                    p.next = p.next.next;
		                    p2 = null;//delete p2;
		                }
		                else{
		                    p = p.next;
		                }
		            }
		        }
		    } 
		    public ButtonLink root_button_link = new ButtonLink(), current_button_link = null, shelter_button_link = null;
		    public ButtonLink exbtn_d_button_link = new ButtonLink(), exbtn_d_shelter_button_link = new ButtonLink(), text_button_link = new ButtonLink();
		    public bool is_exbtn_enabled;
		
		    public bool current_button_valid;
		    public int current_over_button;
		
		    /* ---------------------------------------- */
		    /* Mion: textbtn related variables */
		    public class TextButtonInfoLink{
		        public TextButtonInfoLink next = null;
		        public CharPtr text; //actual "text" of the button
		        public CharPtr prtext; // button text as printed (w/linebreaks)
		        public ButtonLink button = null;
		        public int[] xy = new int[2];
		        public int no;
		        public TextButtonInfoLink()
		        { next = (null); text = (null); prtext = (null); button = (null);
		            xy[0] = xy[1] = -1;
		            no = -1;
		        }
		        ~TextButtonInfoLink(){
		            if (null!=text) text = null;//delete[] text;
		            if (null!=prtext) prtext = null;//delete[] prtext;
		        }
		        public void insert( TextButtonInfoLink info ){
		            info.next = this.next;
		            this.next = info;
		        }
		    } 
		    public TextButtonInfoLink text_button_info = new TextButtonInfoLink();
		    public int txtbtn_start_num;
		    public int next_txtbtn_num;
		    public bool in_txtbtn;
		    public bool txtbtn_show;
		    public bool txtbtn_visible;
			public byte[][] linkcolor = linkcolor_init();
			private static byte[][] linkcolor_init() 
			{
				byte[][] linkcolor_ = new byte[2][];
				for (int i = 0; i < linkcolor_.Length; ++i)
				{
					linkcolor_[i] = new byte[3];
				}	
				return linkcolor_;
			}
			
		    public bool getzxc_flag;
		    public bool gettab_flag;
		    public bool getpageup_flag;
		    public bool getpagedown_flag;
		    public bool getinsert_flag;
		    public bool getfunction_flag;
		    public bool getenter_flag;
		    public bool getcursor_flag;
		    public bool spclclk_flag;
		    public bool getmclick_flag;
		    public bool getskipoff_flag;
		    public bool getmouseover_flag;
		    public int  getmouseover_min, getmouseover_max;
		    public bool btnarea_flag;
		    public int  btnarea_pos;
		
//		    void resetSentenceFont();
//		    void deleteButtonLink();
//		    void processTextButtonInfo();
//		    void deleteTextButtonInfo();
//		    void terminateTextButton();
//		    void textbtnColorChange();
//		    void refreshMouseOverButton();
//		    void refreshSprite( int sprite_no, bool active_flag, int cell_no, SDL_Rect *check_src_rect, SDL_Rect *check_dst_rect );
//		
//		    void decodeExbtnControl( const char *ctl_str, SDL_Rect *check_src_rect=NULL, SDL_Rect *check_dst_rect=NULL );
//		
//		    void disableGetButtonFlag();
//		    int getNumberFromBuffer( const char **buf );
		
		    /* ---------------------------------------- */
		    /* General image-related variables */
		    public int png_mask_type;
		
		    /* ---------------------------------------- */
		    /* Background related variables */
		    public AnimationInfo bg_info = new AnimationInfo();
		
		    /* ---------------------------------------- */
		    /* Tachi-e related variables */
		    /* 0 ... left, 1 ... center, 2 ... right */
		    public AnimationInfo[] tachi_info = tachi_info_init();
		    private static AnimationInfo[] tachi_info_init() {
		    	AnimationInfo[] ret = new AnimationInfo[3];
		    	for (int i = 0; i < ret.Length; ++i)
		    	{
		    		ret[i] = new AnimationInfo();
		    	}
		    	return ret;
		    }
		    public int[] human_order = new int[3];
		
		    /* ---------------------------------------- */
		    /* Sprite related variables */
		    public AnimationInfo[] sprite_info;
		    public AnimationInfo[] sprite2_info;
		    public bool all_sprite_hide_flag;
		    public bool all_sprite2_hide_flag;
		
		    //Mion: track the last few sprite numbers loaded, for sprite data reuse
		    
		    public const int SPRITE_NUM_LAST_LOADS = 4;
		    
		    public int[] last_loaded_sprite = new int[SPRITE_NUM_LAST_LOADS];
		    public int last_loaded_sprite_ind;
		
		    /* ---------------------------------------- */
		    /* Parameter related variables */
		    public AnimationInfo[] bar_info = new AnimationInfo[MAX_PARAM_NUM], prnum_info = new AnimationInfo[MAX_PARAM_NUM];
		
		    /* ---------------------------------------- */
		    /* Cursor related variables */
		    
		    public const int CURSOR_WAIT_NO    = 0;
		    public const int CURSOR_NEWPAGE_NO = 1;
		    
		    public AnimationInfo[] cursor_info = cursor_info_init();
		    private static AnimationInfo[] cursor_info_init()
		    {
		    	AnimationInfo[] ret = new AnimationInfo[2];
		    	for (int i = 0; i < ret.Length; ++i)
		    	{
		    		ret[i] = new AnimationInfo();
		    	}
		    	return ret;
		    }
		
//		    void loadCursor( int no, const char *str, int x, int y, bool abs_flag = false );
//		    void saveAll();
//		    void loadEnvData();
//		    void saveEnvData();
		
		    /* ---------------------------------------- */
		    /* Lookback related variables */
		    public AnimationInfo[] lookback_info = new AnimationInfo[4];
		
		    /* ---------------------------------------- */
		    /* Text related variables */
			public AnimationInfo text_info = new AnimationInfo(), shelter_text_info = new AnimationInfo();
			public AnimationInfo sentence_font_info = new AnimationInfo();
		    public CharPtr font_file;
		    public int erase_text_window_mode;
		    public bool text_on_flag; // suppress the effect of erase_text_window_mode
		    public bool draw_cursor_flag;
		    public int  textgosub_clickstr_state;
		    public int  indent_offset;
		    public int  line_enter_status; // 0 - no enter, 1 - pretext, 2 - body start, 3 - within body
		    public int  page_enter_status; // 0 ... no enter, 1 ... body
		    public class GlyphCache{
		        public GlyphCache next;
		        public UInt16 text;
		        public TTF_Font font;
		        public SDL_Surface surface;
		        public GlyphCache()
		        { next = (null); text = (0); font = (null); surface = (null); }
		        ~GlyphCache() { SDL_FreeSurface(surface); }
		    } 
		    public GlyphCache root_glyph_cache = null; 
		    public GlyphCache[] glyph_cache = init_glyph_cache();
		    private static GlyphCache[] init_glyph_cache() {
		    	GlyphCache[] ret = new GlyphCache[NUM_GLYPH_CACHE];
		    	for (int i = 0; i < ret.Length; ++i)
		    	{
		    		ret[i] = new GlyphCache();
		    	}
		    	return ret;
		    }
		    public int[] last_textpos_xy = new int[2];
		
//		    int  refreshMode();
//		    void setwindowCore();
//		
//		    SDL_Surface *renderGlyph(TTF_Font *font, Uint16 text);
//		    void drawGlyph( SDL_Surface *dst_surface, Fontinfo *info, SDL_Color &color, char *text, int xy[2], bool shadow_flag, AnimationInfo *cache_info, SDL_Rect *clip, SDL_Rect &dst_rect );
//		    void drawChar( char* text, Fontinfo *info, bool flush_flag, bool lookback_flag, SDL_Surface *surface, AnimationInfo *cache_info, int abs_offset=0, SDL_Rect *clip=NULL );
//		    void drawString( const char *str, uchar3 color, Fontinfo *info, bool flush_flag, SDL_Surface *surface, int abs_offset=0, SDL_Rect *rect = NULL, AnimationInfo *cache_info=NULL, bool skip_whitespace_flag=true );
//		    void restoreTextBuffer();
//		    void enterTextDisplayMode(bool text_flag = true);
//		    void leaveTextDisplayMode(bool force_leave_flag = false);
//		    bool doClickEnd();
//		    bool clickWait();
//		    bool clickNewPage();
//		    void startRuby(char *buf, Fontinfo &info);
//		    void endRuby(bool flush_flag, bool lookback_flag, SDL_Surface *surface, AnimationInfo *cache_info);
//		    int  textCommand();
//		    void processEOT();
//		    bool processText();
		
		    //Mion: variables & functions for special text processing
		    public bool[] string_buffer_breaks;  // can it break before a particular offset?
		    public CharPtr string_buffer_margins; // where are the ruby margins, how long (in pixels)
		    public bool line_has_nonspace;
		    public enum LineBreakType {
		        SPACEBREAK = 1, // Western-style, break before spaces
		        KINSOKU    = 2  // Eastern-style, break anywhere except before/after forbidden chars
		    } 
		    public LineBreakType last_line_break_type;
//		    char doLineBreak(bool isHardBreak=false);
//		    int isTextCommand(const char *buf);
//		    void processRuby(unsigned int i, int cmd);
//		    bool processBreaks(bool cont_line, LineBreakType style);
//		    int findNextBreak(int offset, int &len);
		
		    /* ---------------------------------------- */
		    /* Skip mode */
		    
		    public const int SKIP_NONE    = 0;
		    public const int SKIP_NORMAL  = 1; // skip endlessly/to unread text (press 's' button)
		    public const int SKIP_TO_EOP  = 2; // skip to end of page (press 'o' button)
		    public const int SKIP_TO_WAIT = 4; // skip to next clickwait
		    public const int SKIP_TO_EOL  = 8; // skip to end of line
		    
		    public int skip_mode;
		
		    /* ---------------------------------------- */
		    /* Effect related variables */
		    public DirtyRect dirty_rect = new DirtyRect(), dirty_rect_tmp = new DirtyRect(); // only this region is updated
		    public int effect_counter, effect_duration; // counter in each effect
		    public int effect_timer_resolution;
		    public int effect_start_time;
		    public int effect_start_time_old;
		    public int effect_tmp; //tmp variable for use by effect routines
		    public bool in_effect_blank;
		    public bool effectskip_flag;
		    public bool skip_effect;
		    
		    public const int EFFECTSPEED_NORMAL  = 0;
		    public const int EFFECTSPEED_QUICKER = 1;
		    public const int EFFECTSPEED_INSTANT = 2;
		    
		    public int effectspeed;
		
		    
		    //some constants for trig tables
		    public const int TRIG_TABLE_SIZE = 256;
		    public const int TRIG_FACTOR = 16384;
		    
		    public int[] sin_table, cos_table;
		    public int[] whirl_table;
		
//		    void buildSinTable();
//		    void buildCosTable();
//		    void buildWhirlTable();
//		    bool setEffect( EffectLink *effect, bool generate_effect_dst, bool update_backup_surface );
//		    bool doEffect( EffectLink *effect, bool clear_dirty_region=true );
//		    void drawEffect( SDL_Rect *dst_rect, SDL_Rect *src_rect, SDL_Surface *surface );
//		    void generateMosaic( SDL_Surface *src_surface, int level );
//		    void doFlushout( int level );
//		    void effectCascade( char *params, int duration );
//		    void effectTrvswave( char *params, int duration );
//		    void effectWhirl( char *params, int duration );
		
		    public class BreakupCell {
		        public int cell_x, cell_y;
		        public int dir;
		        public int state;
		        public int radius;
		        BreakupCell()
		        { cell_x = (0); cell_y = (0);
		          dir = (0); state = (0); radius = (0);
		        }
		    } 
		    public BreakupCell[] breakup_cells;
		    public bool[] breakup_cellforms, breakup_mask;
//		    void buildBreakupCellforms();
//		    void buildBreakupMask();
//		    void initBreakup( char *params );
//		    void effectBreakup( char *params, int duration );
		
		    /* ---------------------------------------- */
		    /* Select related variables */
		    
		    public const int SELECT_GOTO_MODE  = 0;
		    public const int SELECT_GOSUB_MODE = 1;
		    public const int SELECT_NUM_MODE   = 2;
		    public const int SELECT_CSEL_MODE  = 3;
		    
		    public class SelectLink{
		        public SelectLink next;
		        public CharPtr text;
		        public CharPtr label;
		
		        public SelectLink()
		        { next = (null); text = (null); label = (null);
		        }
		        ~SelectLink(){
		            if ( null!=text )  text = null;//delete[] text;
		            if ( null!=label ) label = null;//delete[] label;
		        }
		    } 
		    public SelectLink root_select_link = new SelectLink(), shelter_select_link = null;
		    public NestInfo select_label_info = new NestInfo();
		    public int shortcut_mouse_line;
		
//		    void deleteSelectLink();
//		    AnimationInfo *getSentence( char *buffer, Fontinfo *info, int num_cells, bool flush_flag = true, bool nofile_flag = false, bool skip_whitespace = true );
//		    struct ButtonLink *getSelectableSentence( char *buffer, Fontinfo *info, bool flush_flag = true, bool nofile_flag = false, bool skip_whitespace = true );
		
		    /* ---------------------------------------- */
		    /* Sound related variables */
		    
		    public const int SOUND_NONE          =  0;
		    public const int SOUND_PRELOAD       =  1;
		    public const int SOUND_WAVE          =  2;
		    public const int SOUND_OGG           =  4;
		    public const int SOUND_OGG_STREAMING =  8;
		    public const int SOUND_MP3           = 16;
		    public const int SOUND_SEQMUSIC      = 32; //MIDI/XM/MOD
		    public const int SOUND_OTHER         = 64;
		    
		    public int  cdrom_drive_number;
		    public CharPtr default_cdrom_drive;
		    public bool cdaudio_on_flag; // false if mute
		    public bool volume_on_flag; // false if mute
		    //SDL_AudioSpec audio_format;
		    public bool audio_open_flag;
		
		    public bool wave_play_loop_flag;
		    public CharPtr wave_file_name;
		
		    public bool seqmusic_play_loop_flag;
		    public CharPtr seqmusic_file_name;
		
		    public int current_cd_track;
		    public bool cd_play_loop_flag;
		    public bool music_play_loop_flag;
		    public bool mp3save_flag;
		    public CharPtr music_file_name;
		    public UnsignedCharPtr music_buffer; // for looped music
		    public long music_buffer_length;
		    public UInt32 mp3fade_start;
		    public UInt32 mp3fadeout_duration;
		    public UInt32 mp3fadein_duration;
		    public CharPtr[] loop_bgm_name = new CharPtr[2];
		
		    public int[] channelvolumes = new int[ONS_MIX_CHANNELS]; //insani's addition
		
		    public CharPtr music_cmd;
		    public CharPtr seqmusic_cmd;
		
//		    int playSound(const char *filename, int format, bool loop_flag, int channel=0);
//		    void playCDAudio();
//		    int playMP3();
//		    int playOGG(int format, unsigned char *buffer, long length, bool loop_flag, int channel);
//		    int playExternalMusic(bool loop_flag);
//		    int playSequencedMusic(bool loop_flag);
//		    // Mion: for music status and fades
//		    int playingMusic();
//		    int setCurMusicVolume(int volume);
//		    int setVolumeMute(bool do_mute);
		
		    public const int WAVE_PLAY        = 0;
		    public const int WAVE_PRELOAD     = 1;
		    public const int WAVE_PLAY_LOADED = 2;
		    
//		    void stopBGM( bool continue_flag );
//		    void stopAllDWAVE();
//		    void playClickVoice();
//		    void setupWaveHeader( unsigned char *buffer, int channels, int rate, int bits, unsigned long data_length );
//		    OVInfo *openOggVorbis(unsigned char *buf, long len, int &channels, int &rate);
//		    int  closeOggVorbis(OVInfo *ovi);
//		
//		    /* ---------------------------------------- */
//		    /* Movie related variables */
		    public UnsignedCharPtr movie_buffer;
		    public SDL_Surface async_movie_surface;
//		    SDL_Rect async_movie_rect;
		    public SDL_Rect[] surround_rects;
		    public bool movie_click_flag, movie_loop_flag;
//		    int playMPEG( const char *filename, bool async_flag, bool use_pos=false, int xpos=0, int ypos=0, int width=0, int height=0 );
//		    int playAVI( const char *filename, bool click_flag );
		
		    /* ---------------------------------------- */
		    /* Text event related variables */
		    public TTF_Font text_font;
		    public bool new_line_skip_flag;
		    public int text_speed_no;
		
//		    void displayTextWindow( SDL_Surface *surface, SDL_Rect &clip );
//		    void clearCurrentPage();
//		    void newPage( bool next_flag );
//		
//		    void flush( int refresh_mode, SDL_Rect *rect=NULL, bool clear_dirty_flag=true, bool direct_flag=false );
//		    void flushDirect( SDL_Rect &rect, int refresh_mode, bool updaterect=true );
//		    int parseLine();
//		
//		    void mouseOverCheck( int x, int y );
//		
//		    /* ---------------------------------------- */
//		    /* Animation */
//		    int  proceedAnimation();
//		    int  proceedCursorAnimation();
//		    int  estimateNextDuration( AnimationInfo *anim, SDL_Rect &rect, int minimum );
//		    void resetRemainingTime( int t );
//		    void resetCursorTime( int t );
//		#ifdef RCA_SCALE
//		    void setupAnimationInfo( AnimationInfo *anim, Fontinfo *info=NULL, float stretch_x=1.0, float stretch_y=1.0 );
//		#else
//		    void setupAnimationInfo( AnimationInfo *anim, Fontinfo *info=NULL );
//		#endif
//		    bool sameImageTag(const AnimationInfo &anim1, const AnimationInfo &anim2);
//		    void parseTaggedString( AnimationInfo *anim, bool is_mask=false );
//		    void drawTaggedSurface( SDL_Surface *dst_surface, AnimationInfo *anim, SDL_Rect &clip );
//		    void stopCursorAnimation( int click );
//		
//		    /* ---------------------------------------- */
//		    /* File I/O */
//		    void searchSaveFile( SaveFileInfo &info, int no );
//		    int  loadSaveFile( int no, bool input_flag=true );
//		    void saveMagicNumber( bool output_flag );
//		    int  saveSaveFile( int no, const char *savestr=NULL );
//		
//		    int  loadSaveFile2( int file_version, bool input_flag=true );
//		    void saveSaveFile2( bool output_flag );
//		
//		    /* ---------------------------------------- */
//		    /* Image processing */
//		    SDL_Surface *loadImage(char *filename, bool *has_alpha=NULL);
//		    SDL_Surface *createRectangleSurface(char *filename);
//		    SDL_Surface *createSurfaceFromFile(char *filename, int *location);
//		
//		    void shiftCursorOnButton( int diff );
//		    void alphaMaskBlend( SDL_Surface *mask_surface, int trans_mode,
//		                         Uint32 mask_value = 255, SDL_Rect *clip=NULL,
//		                         SDL_Surface *src1=NULL, SDL_Surface *src2=NULL,
//		                         SDL_Surface *dst=NULL );
//		    void alphaBlendText( SDL_Surface *dst_surface, SDL_Rect dst_rect,
//		                         SDL_Surface *txt_surface, SDL_Color &color,
//		                         SDL_Rect *clip, bool rotate_flag );
//		    void makeNegaSurface( SDL_Surface *surface, SDL_Rect &clip );
//		    void makeMonochromeSurface( SDL_Surface *surface, SDL_Rect &clip );
//		    void refreshSurface( SDL_Surface *surface, SDL_Rect *clip_src, int refresh_mode = REFRESH_NORMAL_MODE );
//		    void createBackground();
		
		    /* ---------------------------------------- */
		    /* rmenu and system call */
		    public bool system_menu_enter_flag;
		    public int  system_menu_mode;
		
		    public int  shelter_event_mode;
		    public int  shelter_display_mode;
		    public bool shelter_draw_cursor_flag;
		    public Page cached_page = null;
		    public AnimationInfo system_menu_title;
		
		    public enum MessageId {
		        MESSAGE_SAVE_EXIST,
		        MESSAGE_SAVE_EMPTY,
		        MESSAGE_SAVE_CONFIRM,
		        MESSAGE_LOAD_CONFIRM,
		        MESSAGE_RESET_CONFIRM,
		        MESSAGE_END_CONFIRM,
		        MESSAGE_YES,
		        MESSAGE_NO
		    }
//		    const char* getMessageString( MessageId which );
//		    
//		    void enterSystemCall();
//		    void leaveSystemCall( bool restore_flag = true );
//		    bool executeSystemCall();
//		
//		    void executeSystemMenu();
//		    void executeSystemSkip();
//		    void executeSystemAutomode();
//		    bool executeSystemReset();
//		    void executeSystemEnd();
//		    void executeWindowErase();
//		    bool executeSystemLoad();
//		    void executeSystemSave();
//		    bool executeSystemYesNo( int caller, int file_no=0 );
//		    void setupLookbackButton();
//		    void executeSystemLookback();
		}
		
//		#endif // __ONSCRIPTER_LABEL_H__
	}
}
