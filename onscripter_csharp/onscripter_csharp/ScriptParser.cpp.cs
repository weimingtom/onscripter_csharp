/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 14:35
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
		 *  ScriptParser.cpp - Define block parser of ONScripter
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
		
		// Modified by Haeleth, Autumn 2006, to better support OS X/Linux packaging.
		
		// Modified by Mion, March 2008, to update from
		// Ogapee's 20080121 release source code.
		
		// Modified by Mion, April 2009, to update from
		// Ogapee's 20090331 release source code.
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#endif
//		
//		#include "ScriptParser.h"
//		
//		#ifdef MACOSX
//		#include "cocoa_bundle.h"
//		#include "cocoa_alertbox.h"
//		#endif
//		#ifdef WIN32
//		#include <windows.h>
//		#endif
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
//		
//		#define VERSION_STR1 "ONScripter-EN"
//		#define VERSION_STR2 "Copyright (C) 2001-2010 Studio O.G.A., 2007-2010 \"Uncle\" Mion Sonozaki. All Rights Reserved."
//		
//		#define DEFAULT_TEXT_SPEED_LOW    40
//		#define DEFAULT_TEXT_SPEED_MIDDLE 20
//		#define DEFAULT_TEXT_SPEED_HIGHT  10
//		
//		#define MAX_PAGE_LIST 16
		
		public delegate int FuncList_ScriptParser(ScriptParser this_);
		public class FuncLUT_ScriptParser {
			public char[] command = new char[30];
		    public FuncList_ScriptParser method;
		    public FuncLUT_ScriptParser(CharPtr str, FuncList_ScriptParser method)
		    {
		    	strcpy(this.command, str);
		    	this.method = method;
		    }
		} 
		public static FuncLUT_ScriptParser[] func_lut_ScriptParser = new FuncLUT_ScriptParser[] {
		    new FuncLUT_ScriptParser("zenkakko", delegate (ScriptParser this_) { return this_.zenkakkoCommand(); } ),
		    new FuncLUT_ScriptParser("windoweffect", delegate (ScriptParser this_) { return this_.effectCommand(); } ),
		    new FuncLUT_ScriptParser("windowchip", delegate (ScriptParser this_) { return this_.windowchipCommand(); } ),
		    new FuncLUT_ScriptParser("windowback", delegate (ScriptParser this_) { return this_.windowbackCommand(); } ),
		    new FuncLUT_ScriptParser("versionstr", delegate (ScriptParser this_) { return this_.versionstrCommand(); } ),
		    new FuncLUT_ScriptParser("usewheel", delegate (ScriptParser this_) { return this_.usewheelCommand(); } ),
		    new FuncLUT_ScriptParser("useescspc", delegate (ScriptParser this_) { return this_.useescspcCommand(); } ),
		    new FuncLUT_ScriptParser("underline", delegate (ScriptParser this_) { return this_.underlineCommand(); } ),
		    new FuncLUT_ScriptParser("transmode", delegate (ScriptParser this_) { return this_.transmodeCommand(); } ),
		    new FuncLUT_ScriptParser("time", delegate (ScriptParser this_) { return this_.timeCommand(); } ),
		    new FuncLUT_ScriptParser("textgosub", delegate (ScriptParser this_) { return this_.textgosubCommand(); } ),
		    new FuncLUT_ScriptParser("tan", delegate (ScriptParser this_) { return this_.tanCommand(); } ),
		    new FuncLUT_ScriptParser("sub", delegate (ScriptParser this_) { return this_.subCommand(); } ),
		    new FuncLUT_ScriptParser("stralias", delegate (ScriptParser this_) { return this_.straliasCommand(); } ),
		    new FuncLUT_ScriptParser("spi", delegate (ScriptParser this_) { return this_.soundpressplginCommand(); } ),
		    new FuncLUT_ScriptParser("soundpressplgin", delegate (ScriptParser this_) { return this_.soundpressplginCommand(); } ),
		    new FuncLUT_ScriptParser("skip",     delegate (ScriptParser this_) { return this_.skipCommand(); } ),
		    new FuncLUT_ScriptParser("sin", delegate (ScriptParser this_) { return this_.sinCommand(); } ),
		    new FuncLUT_ScriptParser("shadedistance",     delegate (ScriptParser this_) { return this_.shadedistanceCommand(); } ),
		    new FuncLUT_ScriptParser("setlayer", delegate (ScriptParser this_) { return this_.setlayerCommand(); } ),
		    new FuncLUT_ScriptParser("setkinsoku",   delegate (ScriptParser this_) { return this_.setkinsokuCommand(); } ),
		    new FuncLUT_ScriptParser("selectvoice",     delegate (ScriptParser this_) { return this_.selectvoiceCommand(); } ),
		    new FuncLUT_ScriptParser("selectcolor",     delegate (ScriptParser this_) { return this_.selectcolorCommand(); } ),
		    new FuncLUT_ScriptParser("savenumber",     delegate (ScriptParser this_) { return this_.savenumberCommand(); } ),
		    new FuncLUT_ScriptParser("savename",     delegate (ScriptParser this_) { return this_.savenameCommand(); } ),
		    new FuncLUT_ScriptParser("savedir",     delegate (ScriptParser this_) { return this_.savedirCommand(); } ),
		    new FuncLUT_ScriptParser("sar",    delegate (ScriptParser this_) { return this_.nsaCommand(); } ),
		    new FuncLUT_ScriptParser("rubyon2",    delegate (ScriptParser this_) { return this_.rubyonCommand(); } ),
		    new FuncLUT_ScriptParser("rubyon",    delegate (ScriptParser this_) { return this_.rubyonCommand(); } ),
		    new FuncLUT_ScriptParser("rubyoff",    delegate (ScriptParser this_) { return this_.rubyoffCommand(); } ),
		    new FuncLUT_ScriptParser("roff",    delegate (ScriptParser this_) { return this_.roffCommand(); } ),
		    new FuncLUT_ScriptParser("rmenu",    delegate (ScriptParser this_) { return this_.rmenuCommand(); } ),
		    new FuncLUT_ScriptParser("rgosub",   delegate (ScriptParser this_) { return this_.rgosubCommand(); } ),
		    new FuncLUT_ScriptParser("return",   delegate (ScriptParser this_) { return this_.returnCommand(); } ),
		    new FuncLUT_ScriptParser("pretextgosub", delegate (ScriptParser this_) { return this_.pretextgosubCommand(); } ),
		    new FuncLUT_ScriptParser("pagetag", delegate (ScriptParser this_) { return this_.pagetagCommand(); } ),
		    new FuncLUT_ScriptParser("numalias", delegate (ScriptParser this_) { return this_.numaliasCommand(); } ),
		    new FuncLUT_ScriptParser("nsadir",    delegate (ScriptParser this_) { return this_.nsadirCommand(); } ),
		    new FuncLUT_ScriptParser("nsa",    delegate (ScriptParser this_) { return this_.nsaCommand(); } ),
		    new FuncLUT_ScriptParser("notif",    delegate (ScriptParser this_) { return this_.ifCommand(); } ),
		    new FuncLUT_ScriptParser("next",    delegate (ScriptParser this_) { return this_.nextCommand(); } ),
		    new FuncLUT_ScriptParser("ns3",    delegate (ScriptParser this_) { return this_.nsaCommand(); } ),
		    new FuncLUT_ScriptParser("ns2",    delegate (ScriptParser this_) { return this_.nsaCommand(); } ),
		    new FuncLUT_ScriptParser("mul",      delegate (ScriptParser this_) { return this_.mulCommand(); } ),
		    new FuncLUT_ScriptParser("movl",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov10",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov9",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov8",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov7",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov6",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov5",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov4",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov3",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mov",      delegate (ScriptParser this_) { return this_.movCommand(); } ),
		    new FuncLUT_ScriptParser("mode_wave_demo", delegate (ScriptParser this_) { return this_.mode_wave_demoCommand(); } ),
		    new FuncLUT_ScriptParser("mode_saya", delegate (ScriptParser this_) { return this_.mode_sayaCommand(); } ),
		    new FuncLUT_ScriptParser("mode_ext", delegate (ScriptParser this_) { return this_.mode_extCommand(); } ),
		    new FuncLUT_ScriptParser("mod",      delegate (ScriptParser this_) { return this_.modCommand(); } ),
		    new FuncLUT_ScriptParser("mid",      delegate (ScriptParser this_) { return this_.midCommand(); } ),
		    new FuncLUT_ScriptParser("menusetwindow",      delegate (ScriptParser this_) { return this_.menusetwindowCommand(); } ),
		    new FuncLUT_ScriptParser("menuselectvoice",      delegate (ScriptParser this_) { return this_.menuselectvoiceCommand(); } ),
		    new FuncLUT_ScriptParser("menuselectcolor",      delegate (ScriptParser this_) { return this_.menuselectcolorCommand(); } ),
		    new FuncLUT_ScriptParser("maxkaisoupage",      delegate (ScriptParser this_) { return this_.maxkaisoupageCommand(); } ),
		    new FuncLUT_ScriptParser("luasub",      delegate (ScriptParser this_) { return this_.luasubCommand(); } ),
		    new FuncLUT_ScriptParser("luacall",      delegate (ScriptParser this_) { return this_.luacallCommand(); } ),
		    new FuncLUT_ScriptParser("lookbacksp",      delegate (ScriptParser this_) { return this_.lookbackspCommand(); } ),
		    new FuncLUT_ScriptParser("lookbackcolor",      delegate (ScriptParser this_) { return this_.lookbackcolorCommand(); } ),
		    //new FuncLUT_ScriptParser("lookbackbutton",      delegate (ScriptParser this_) { return this_.lookbackbuttonCommand(); } ),
		    new FuncLUT_ScriptParser("loadgosub",      delegate (ScriptParser this_) { return this_.loadgosubCommand(); } ),
		    new FuncLUT_ScriptParser("linepage2",    delegate (ScriptParser this_) { return this_.linepageCommand(); } ),
		    new FuncLUT_ScriptParser("linepage",    delegate (ScriptParser this_) { return this_.linepageCommand(); } ),
		    new FuncLUT_ScriptParser("len",      delegate (ScriptParser this_) { return this_.lenCommand(); } ),
		    new FuncLUT_ScriptParser("labellog",      delegate (ScriptParser this_) { return this_.labellogCommand(); } ),
		    new FuncLUT_ScriptParser("kidokuskip", delegate (ScriptParser this_) { return this_.kidokuskipCommand(); } ),
		    new FuncLUT_ScriptParser("kidokumode", delegate (ScriptParser this_) { return this_.kidokumodeCommand(); } ),
		    new FuncLUT_ScriptParser("itoa2", delegate (ScriptParser this_) { return this_.itoaCommand(); } ),
		    new FuncLUT_ScriptParser("itoa", delegate (ScriptParser this_) { return this_.itoaCommand(); } ),
		    new FuncLUT_ScriptParser("intlimit", delegate (ScriptParser this_) { return this_.intlimitCommand(); } ),
		    new FuncLUT_ScriptParser("inc",      delegate (ScriptParser this_) { return this_.incCommand(); } ),
		    new FuncLUT_ScriptParser("if",       delegate (ScriptParser this_) { return this_.ifCommand(); } ),
		    new FuncLUT_ScriptParser("humanz",       delegate (ScriptParser this_) { return this_.humanzCommand(); } ),
		    new FuncLUT_ScriptParser("humanpos",       delegate (ScriptParser this_) { return this_.humanposCommand(); } ),
		    new FuncLUT_ScriptParser("goto",     delegate (ScriptParser this_) { return this_.gotoCommand(); } ),
		    new FuncLUT_ScriptParser("gosub",    delegate (ScriptParser this_) { return this_.gosubCommand(); } ),
		    new FuncLUT_ScriptParser("globalon",    delegate (ScriptParser this_) { return this_.globalonCommand(); } ),
		    new FuncLUT_ScriptParser("getparam",    delegate (ScriptParser this_) { return this_.getparamCommand(); } ),
		    //new FuncLUT_ScriptParser("game",    delegate (ScriptParser this_) { return this_.gameCommand(); } ),
		    new FuncLUT_ScriptParser("for",   delegate (ScriptParser this_) { return this_.forCommand(); } ),
		    new FuncLUT_ScriptParser("filelog",   delegate (ScriptParser this_) { return this_.filelogCommand(); } ),
		    new FuncLUT_ScriptParser("errorsave",   delegate (ScriptParser this_) { return this_.errorsaveCommand(); } ),
		    new FuncLUT_ScriptParser("english",   delegate (ScriptParser this_) { return this_.englishCommand(); } ),
		    new FuncLUT_ScriptParser("effectcut",   delegate (ScriptParser this_) { return this_.effectcutCommand(); } ),
		    new FuncLUT_ScriptParser("effectblank",   delegate (ScriptParser this_) { return this_.effectblankCommand(); } ),
		    new FuncLUT_ScriptParser("effect",   delegate (ScriptParser this_) { return this_.effectCommand(); } ),
		    new FuncLUT_ScriptParser("dsound",   delegate (ScriptParser this_) { return this_.dsoundCommand(); } ),
		    new FuncLUT_ScriptParser("div",   delegate (ScriptParser this_) { return this_.divCommand(); } ),
		    new FuncLUT_ScriptParser("dim",   delegate (ScriptParser this_) { return this_.dimCommand(); } ),
		    new FuncLUT_ScriptParser("defvoicevol",   delegate (ScriptParser this_) { return this_.defvoicevolCommand(); } ),
		    new FuncLUT_ScriptParser("defsub",   delegate (ScriptParser this_) { return this_.defsubCommand(); } ),
		    new FuncLUT_ScriptParser("defsevol",   delegate (ScriptParser this_) { return this_.defsevolCommand(); } ),
		    new FuncLUT_ScriptParser("defmp3vol",   delegate (ScriptParser this_) { return this_.defmp3volCommand(); } ),
		    new FuncLUT_ScriptParser("defbgmvol",   delegate (ScriptParser this_) { return this_.defmp3volCommand(); } ),
		    new FuncLUT_ScriptParser("defaultspeed", delegate (ScriptParser this_) { return this_.defaultspeedCommand(); } ),
		    new FuncLUT_ScriptParser("defaultfont", delegate (ScriptParser this_) { return this_.defaultfontCommand(); } ),
		    new FuncLUT_ScriptParser("dec",   delegate (ScriptParser this_) { return this_.decCommand(); } ),
		    new FuncLUT_ScriptParser("dec",   delegate (ScriptParser this_) { return this_.decCommand(); } ),
		    new FuncLUT_ScriptParser("date",   delegate (ScriptParser this_) { return this_.dateCommand(); } ),
		    new FuncLUT_ScriptParser("cos", delegate (ScriptParser this_) { return this_.cosCommand(); } ),
		    new FuncLUT_ScriptParser("cmp",      delegate (ScriptParser this_) { return this_.cmpCommand(); } ),
		    new FuncLUT_ScriptParser("clickvoice",   delegate (ScriptParser this_) { return this_.clickvoiceCommand(); } ),
		    new FuncLUT_ScriptParser("clickstr",   delegate (ScriptParser this_) { return this_.clickstrCommand(); } ),
		    new FuncLUT_ScriptParser("clickskippage", delegate (ScriptParser this_) { return this_.clickskippageCommand(); } ),
		    new FuncLUT_ScriptParser("btnnowindowerase",   delegate (ScriptParser this_) { return this_.btnnowindoweraseCommand(); } ),
		    new FuncLUT_ScriptParser("break",   delegate (ScriptParser this_) { return this_.breakCommand(); } ),
		    new FuncLUT_ScriptParser("automode", delegate (ScriptParser this_) { return this_.mode_extCommand(); } ),
		    new FuncLUT_ScriptParser("atoi",      delegate (ScriptParser this_) { return this_.atoiCommand(); } ),
		    new FuncLUT_ScriptParser("arc",      delegate (ScriptParser this_) { return this_.arcCommand(); } ),
		    new FuncLUT_ScriptParser("addnsadir",    delegate (ScriptParser this_) { return this_.addnsadirCommand(); } ),
		    new FuncLUT_ScriptParser("addkinsoku",   delegate (ScriptParser this_) { return this_.addkinsokuCommand(); } ),
		    new FuncLUT_ScriptParser("add",      delegate (ScriptParser this_) { return this_.addCommand(); } ),
		    new FuncLUT_ScriptParser("", null),
		};
		
		public class FuncHash_ScriptParser {
		    public int start;
		    public int end;
		}
		public static FuncHash_ScriptParser[] func_hash_ScriptParser = new FuncHash_ScriptParser['z'-'a'+1];
		
		
		public partial class ScriptParser {
			public ScriptParser()
//			//Using an initialization list to make sure pointers start out NULL
//			:
//			#ifdef MACOSX
//			  bundle_res_path(NULL), bundle_app_path(NULL), bundle_app_name(NULL),
//			#endif
//			  cmdline_game_id(NULL), last_nest_info(NULL), version_str(NULL),
//			  savedir(NULL), last_effect_link(NULL),
//			#ifndef NO_LAYER_EFFECTS
//			  layer_info(NULL),
//			#endif
//			  save_menu_name(NULL), load_menu_name(NULL), save_item_name(NULL),
//			  save_data_buf(NULL), file_io_buf(NULL), default_env_font(NULL),
//			  page_list(NULL), start_page(NULL), current_page(NULL),
//			  start_kinsoku(NULL), end_kinsoku(NULL), current_font(NULL),
//			  textgosub_label(NULL), pretextgosub_label(NULL),
//			  loadgosub_label(NULL), rgosub_label(NULL), key_table(NULL)
			{
//			    resetDefineFlags();
//			
//			    debug_level = 0;
//			    srand( time(NULL) );
//			    rand();
//			
//			#ifdef MACOSX
//			    is_bundled = false;
//			#endif
//			    nsa_offset = 0;
//			    force_button_shortcut_flag = false;
//			    
//			    file_io_buf_ptr = 0;
//			    file_io_buf_len = 0;
//			    save_data_len = 0;
//			
//			    /* ---------------------------------------- */
//			    /* Sound related variables */
//			    int i;
//			    for ( i=0 ; i<     CLICKVOICE_NUM ; i++ )
//			             clickvoice_file_name[i] = NULL;
//			    for ( i=0 ; i<    SELECTVOICE_NUM ; i++ )
//			            selectvoice_file_name[i] = NULL;
//			    for ( i=0 ; i<MENUSELECTVOICE_NUM ; i++ )
//			        menuselectvoice_file_name[i] = NULL;
//			
//			    //Default kinsoku
//			    num_start_kinsoku = num_end_kinsoku = 0;
//			    setKinsoku(DEFAULT_START_KINSOKU, DEFAULT_END_KINSOKU, false);
//			
//			    //onscripter script syntax options (for running some older nscr games)
//			    allow_color_type_only = false;
//			    set_tag_page_origin_to_1 = false;
//			    answer_dialog_with_yes_ok = false;
//			
//			#ifdef RCA_SCALE
//			    scr_stretch_x = scr_stretch_y = 1.0;
//			#endif
//			    preferred_width = 0;
//			
//			    errorsave = false;
//			
//			    //initialize cmd function table hash
//			    for (i='z'-'a' ; i>=0 ; i--){
//			        func_hash[i].start = -1;
//			        func_hash[i].end = -2;
//			    }
//			    int idx = 0;
//			    while (func_lut[idx].method){
//			        int j = func_lut[idx].command[0]-'a';
//			        if (func_hash[j].start == -1) func_hash[j].start = idx;
//			        func_hash[j].end = idx;
//			        idx++;
//			    }
			}
			
//			ScriptParser::~ScriptParser()
//			{
//			    reset();
//			
//			    if (version_str) delete[] version_str;
//			    if (save_menu_name) delete[] save_menu_name;
//			    if (load_menu_name) delete[] load_menu_name;
//			    if (save_item_name) delete[] save_item_name;
//			
//			    if (file_io_buf) delete[] file_io_buf;
//			    if (save_data_buf) delete[] save_data_buf;
//			
//			    if (start_kinsoku) delete[] start_kinsoku;
//			    if (end_kinsoku) delete[] end_kinsoku;
//			
//			#ifdef MACOSX
//			    if (bundle_res_path) delete[] bundle_res_path;
//			    if (bundle_app_path) delete[] bundle_app_path;
//			    if (bundle_app_name) delete[] bundle_app_name;
//			#endif
//			    if (cmdline_game_id) delete[] cmdline_game_id;
//			    if (savedir) delete[] savedir;
//			}
			
			public void reset()
			{
//			    resetDefineFlags();
//			
//			    int i;
//			    for (i='z'-'a' ; i>=0 ; i--){
//			        UserFuncHash &ufh = user_func_hash[i];
//			        UserFuncLUT *func = ufh.root.next;
//			        while(func){
//			            UserFuncLUT *tmp = func;
//			            func = func->next;
//			            delete tmp;
//			        }
//			        ufh.root.next = NULL;
//			        ufh.last = &ufh.root;
//			    }
//			
//			    // reset misc variables
//			    nsa_path = DirPaths();
//			
//			    if (version_str) delete[] version_str;
//			    version_str = new char[strlen(VERSION_STR1)+
//			                           strlen("\n")+
//			                           strlen(VERSION_STR2)+
//			                           strlen("\n")+
//			                           +1];
//			    sprintf( version_str, "%s\n%s\n", VERSION_STR1, VERSION_STR2 );
//			
//			    /* Text related variables */
//			    sentence_font.reset();
//			    menu_font.reset();
//			    ruby_font.reset();
//			    current_font = &sentence_font;
//			
//			    if (page_list){
//			        delete[] page_list;
//			        page_list = NULL;
//			    }
//			    //current_page & start_page point to page_list elements
//			    current_page = start_page = NULL;
//			    
//			    textgosub_label = NULL;
//			    pretextgosub_label = NULL;
//			    loadgosub_label = NULL;
//			    rgosub_label = NULL;
//			
//			    /* ---------------------------------------- */
//			    /* Sound related variables */
//			    for ( i=0 ; i<     CLICKVOICE_NUM ; i++ )
//			        setStr(&clickvoice_file_name[i], NULL);
//			    for ( i=0 ; i<    SELECTVOICE_NUM ; i++ )
//			        setStr(&selectvoice_file_name[i], NULL);
//			    for ( i=0 ; i<MENUSELECTVOICE_NUM ; i++ )
//			        setStr(&menuselectvoice_file_name[i], NULL);
//			
//			    /* Menu related variables */
//			    setDefaultMenuLabels();
//			    deleteRMenuLink();
//			
//			    /* Effect related variables */
//			    EffectLink *link = root_effect_link.next;
//			    while(link){
//			        EffectLink *tmp = link;
//			        link = link->next;
//			        delete tmp;
//			    }
//			    last_effect_link = &root_effect_link;
//			    last_effect_link->next = NULL;
//			
//			#ifndef NO_LAYER_EFFECTS
//			    deleteLayerInfo();
//			#endif
//			
//			    readLog( script_h.log_info[ScriptHandler::LABEL_LOG] );
			}
			
			public void resetDefineFlags()
			{
//			    globalon_flag = false;
//			    labellog_flag = false;
//			    filelog_flag = false;
//			    kidokuskip_flag = false;
//			    clickskippage_flag = false;
//			
//			    rmode_flag = true;
//			    windowback_flag = false;
//			    btnnowindowerase_flag = false;
//			    usewheel_flag = false;
//			    useescspc_flag = false;
//			    mode_wave_demo_flag = false;
//			    mode_saya_flag = false;
//			    mode_ext_flag = false;
//			    rubyon_flag = rubyon2_flag = false;
//			    zenkakko_flag = false;
//			    pagetag_flag = false;
//			    windowchip_sprite_no = -1;
//			    string_buffer_offset = 0;
//			
//			    break_flag = false;
//			    trans_mode = AnimationInfo::TRANS_TOPLEFT;
//			
//			    z_order = 499;
//			
//			    /* ---------------------------------------- */
//			    /* Lookback related variables */
//			    lookback_sp[0] = lookback_sp[1] = -1;
//			    lookback_color[0] = 0xff;
//			    lookback_color[1] = 0xff;
//			    lookback_color[2] = 0x00;
//			
//			    /* ---------------------------------------- */
//			    /* Save/Load related variables */
//			    num_save_file = 9;
//			
//			    /* ---------------------------------------- */
//			    /* Text related variables */
//			    shade_distance[0] = 1;
//			    shade_distance[1] = 1;
//			    
//			    default_text_speed[0] = DEFAULT_TEXT_SPEED_LOW;
//			    default_text_speed[1] = DEFAULT_TEXT_SPEED_MIDDLE;
//			    default_text_speed[2] = DEFAULT_TEXT_SPEED_HIGHT;
//			    max_page_list = MAX_PAGE_LIST+1;
//			    num_chars_in_sentence = 0;
//			
//			    clickstr_line = 0;
//			    clickstr_state = CLICK_NONE;
//			    linepage_mode = 0;
//			    english_mode = false;
//			    
//			    /* ---------------------------------------- */
//			    /* Menu related variables */
//			    menu_font.font_size_xy[0] = DEFAULT_FONT_SIZE;
//			    menu_font.font_size_xy[1] = DEFAULT_FONT_SIZE;
//			    menu_font.top_xy[0] = 0;
//			    menu_font.top_xy[1] = 16;
//			    menu_font.num_xy[0] = 32;
//			    menu_font.num_xy[1] = 23;
//			    menu_font.pitch_xy[0] = menu_font.font_size_xy[0];
//			    menu_font.pitch_xy[1] = 2 + menu_font.font_size_xy[1];
//			    menu_font.window_color[0] = menu_font.window_color[1] = menu_font.window_color[2] = 0xcc;
//			
//			    /* ---------------------------------------- */
//			    /* Effect related variables */
//			    effect_blank = 10;
//			    effect_cut_flag = false;
//			
//			    window_effect.effect = 1;
//			    window_effect.duration = 0;
//			    root_effect_link.no = 0;
//			    root_effect_link.effect = 0;
//			    root_effect_link.duration = 0;
//			
//			    current_mode = DEFINE_MODE;
			}
			
			public int open()
			{
				return 0;
//			    script_h.cBR = new DirectReader( archive_path, key_table );
//			    script_h.cBR->open();
//			
//			    script_h.game_identifier = cmdline_game_id;
//			    cmdline_game_id = NULL;
//			
//			    if ( script_h.readScript( archive_path ) ) return -1;
//			
//			    switch ( script_h.screen_size ){
//			    //for PDA, set ratios to create a 320x240 screen
//			      case ScriptHandler::SCREEN_SIZE_800x600:
//			#ifdef PDA
//			        screen_ratio1 = 2;
//			        screen_ratio2 = 5;
//			#else
//			        screen_ratio1 = 1;
//			        screen_ratio2 = 1;
//			#endif
//			        script_width = 800;
//			        script_height = 600;
//			        break;
//			      case ScriptHandler::SCREEN_SIZE_400x300:
//			#ifdef PDA
//			        screen_ratio1 = 4;
//			        screen_ratio2 = 5;
//			#else
//			        screen_ratio1 = 1;
//			        screen_ratio2 = 1;
//			#endif
//			        script_width = 400;
//			        script_height = 300;
//			        break;
//			      case ScriptHandler::SCREEN_SIZE_320x240:
//			        screen_ratio1 = 1;
//			        screen_ratio2 = 1;
//			        script_width = 320;
//			        script_height = 240;
//			        break;
//			      case ScriptHandler::SCREEN_SIZE_640x480:
//			      default:
//			#ifdef PDA
//			        screen_ratio1 = 1;
//			        screen_ratio2 = 2;
//			#else
//			        screen_ratio1 = 1;
//			        screen_ratio2 = 1;
//			#endif
//			        script_width = 640;
//			        script_height = 480;
//			        break;
//			    }
//			
//			#ifndef PDA
//			    if (preferred_width > 0) {
//			        screen_ratio1 = preferred_width;
//			        screen_ratio2 = script_width;
//			    }
//			#endif
//			    screen_width  = script_width * screen_ratio1 / screen_ratio2;
//			    screen_height = script_height * screen_ratio1 / screen_ratio2;
//			    underline_value = script_height - 1;
//			    for (int i=0; i<3; i++)
//			        humanpos[i] = (script_width/4) * (i+1);
//			    if (debug_level > 0)
//			        printf("humanpos: %d,%d,%d; underline: %d\n", humanpos[0], humanpos[1],
//			               humanpos[2], underline_value);
//			
//			    return 0;
			}
			
//			#ifdef MACOSX
			public void checkBundled()
			{
			    // check whether this onscripter is bundled, and if so find the
			    // resources and app directories
			    
//			    ONSCocoa::getBundleInfo(&bundle_res_path, &bundle_app_path, &bundle_app_name);
//			    is_bundled = true; // always bundled on OS X
			}
//			#endif
			
			public byte convHexToDec( char ch )
			{
				return 0;
//			    if      ( '0' <= ch && ch <= '9' ) return ch - '0';
//			    else if ( 'a' <= ch && ch <= 'f' ) return ch - 'a' + 10;
//			    else if ( 'A' <= ch && ch <= 'F' ) return ch - 'A' + 10;
//			    else errorAndExit("convHexToDec: not valid character for color.");
//			
//			    return 0;
			}
			
			public void setColor( byte[] dstcolor, byte[] srccolor )
			{
//			    for (int i=0; i<3; i++)
//			        dstcolor[i] = srccolor[i];
			}
			
			public void readColor( ref byte[] color, CharPtr buf ){
//			    if ( buf[0] != '#' ) errorAndExit("readColor: no preceding #.");
//			    (*color)[0] = convHexToDec( buf[1] ) << 4 | convHexToDec( buf[2] );
//			    (*color)[1] = convHexToDec( buf[3] ) << 4 | convHexToDec( buf[4] );
//			    (*color)[2] = convHexToDec( buf[5] ) << 4 | convHexToDec( buf[6] );
			}
			
			public void add_debug_level()
			{
//			    debug_level++;
			}
			
			public void errorAndCont( CharPtr str, CharPtr reason = null, CharPtr title = null, bool is_simple = false )
			{
//			    if (title == NULL)
//			        title = "Parse Issue";
//			    script_h.processError(str, title, reason, true, is_simple);
			}
			
			public void errorAndExit( CharPtr str, CharPtr reason = null, CharPtr title = null, bool is_simple = false )
			{
//			    if (title == NULL)
//			        title = "Parse Error";
//			    script_h.processError(str, title, reason, false, is_simple);
			}
			
			public int parseLine()
			{
				CharPtr cmd = new CharPtr(script_h.getStringBuffer(), string_buffer_offset);
			
				if ( (debug_level > 0) && (cmd[0] != ':') && (cmd[0] != 0x0a) ) {
			        printf("ScriptParser::Parseline %s\n", cmd );
			        fflush(stdout);
			    }
			
			    script_h.current_cmd[0] = '\0';
			    script_h.current_cmd_type = ScriptHandler.CMD_NONE;
			
			    if ( cmd[0] == ';' ) return RET_CONTINUE;
			    else if ( cmd[0] == '*' ) return RET_CONTINUE;
			    else if ( cmd[0] == ':' ) return RET_CONTINUE;
			    else if ( script_h.isText() ) return RET_NOMATCH;
			
			    if (cmd[0] != '_'){
			        snprintf(script_h.current_cmd, 64, "%s", cmd);
			        //Check against user-defined cmds
			        if (cmd[0] >= 'a' && cmd[0] <= 'z'){
						//TODO:
						char[] debugstr = new char[256];
						sprintf(debugstr, "<<<<<<<< %s\n", cmd); 
						OutputDebugString(debugstr);
			
			            UserFuncHash ufh = user_func_hash[cmd[0]-'a'];
			            UserFuncLUT uf = ufh.root.next;
			            while(null!=uf){
			                if (0==strcmp( uf.command, cmd )){
			                    if (uf.lua_flag){
			#if USE_LUA
			                        if (lua_handler.callFunction(false, cmd))
			                            errorAndExit( lua_handler.error_str, NULL, "Lua Error" );
			#endif
			                    }
			                    else{
			                        gosubReal( cmd, script_h.getNext() );
			                    }
			                    return RET_CONTINUE;
			                }
			                uf = uf.next;
			            }
			        }
			    }
			    else{
			    	cmd.inc();
			    }
			
			    //Check against builtin cmds
			    if (cmd[0] >= 'a' && cmd[0] <= 'z'){
			        FuncHash fh = func_hash[cmd[0]-'a'];
			        for (int i=fh.start ; i<=fh.end ; i++){
			            if ( 0==strcmp( func_lut_ScriptParser[i].command, cmd ) ){
			                return func_lut_ScriptParser[i].method(this);
			            }
			        }
			    }
			
			    return RET_NOMATCH;
			}
			
			public void deleteRMenuLink()
			{
			    RMenuLink link = root_rmenu_link.next;
			    while(null!=link){
			        RMenuLink tmp = link;
			        link = link.next;
			        tmp = null;//delete tmp;
			    }
			    root_rmenu_link.next = null;
			
			    rmenu_link_num   = 0;
			    rmenu_link_width = 0;
			}
			
			public int getSystemCallNo( CharPtr buffer )
			{
				if      ( 0==strcmp( buffer, "skip" ) )        return SYSTEM_SKIP;
			    else if ( 0==strcmp( buffer, "reset" ) )       return SYSTEM_RESET;
			    else if ( 0==strcmp( buffer, "save" ) )        return SYSTEM_SAVE;
			    else if ( 0==strcmp( buffer, "load" ) )        return SYSTEM_LOAD;
			    else if ( 0==strcmp( buffer, "lookback" ) )    return SYSTEM_LOOKBACK;
			    else if ( 0==strcmp( buffer, "windowerase" ) ) return SYSTEM_WINDOWERASE;
			    else if ( 0==strcmp( buffer, "rmenu" ) )       return SYSTEM_MENU;
			    else if ( 0==strcmp( buffer, "automode" ) )    return SYSTEM_AUTOMODE;
			    else if ( 0==strcmp( buffer, "end" ) )         return SYSTEM_END;
			    else{
			        printf("Unsupported system call %s\n", buffer );
			        return -1;
			    }
			}
			
			public void setArchivePath(CharPtr path)
			{
			    archive_path = new DirPaths(path);
			    //printf("archive_path: %s\n", archive_path.get_all_paths());
			}
			
			public void setSavePath(CharPtr path)
			{
				if ( (path == null) || (path[0] == '\0') ||
			         (path[strlen(path)-1] == DELIMITER) ) {
			        setStr( ref script_h.save_path, path );
			    } else {
			        if (null!=script_h.save_path) script_h.save_path = null;//delete[] script_h.save_path;
			        script_h.save_path = new char[ strlen(path) + 2 ];
			        sprintf( script_h.save_path, "%s%c", path, DELIMITER );
			    }
			}
			
			public void setNsaOffset(CharPtr off)
			{
			    int offset = atoi(off);
			    if (offset > 0)
			        nsa_offset = offset;
			}
			
			public void saveGlovalData()
			{
			    if ( !globalon_flag ) return;
			
			    file_io_buf_ptr = 0;
			    writeVariables( script_h.global_variable_border, VARIABLE_RANGE, false );
			    allocFileIOBuf();
			    writeVariables( script_h.global_variable_border, VARIABLE_RANGE, true );
			
			    if (0!=saveFileIOBuf( "gloval.sav" ))
			        errorAndExit( "can't open 'gloval.sav' for writing", null, "I/O Error" );
			}
			
			public void allocFileIOBuf()
			{
			    if (file_io_buf_ptr > file_io_buf_len){
			        file_io_buf_len = file_io_buf_ptr;
			        if (null!=file_io_buf) file_io_buf = null;//delete[] file_io_buf;
			        file_io_buf = new UnsignedCharPtr(new byte[file_io_buf_len]);
			
			        if (null!=save_data_buf){
			            memcpy(file_io_buf, save_data_buf, save_data_len);
			            save_data_buf = null;//delete[] save_data_buf;
			        }
			        save_data_buf = new UnsignedCharPtr(new byte[file_io_buf_len]);
			        memcpy(save_data_buf, file_io_buf, save_data_len);
			    }
			    file_io_buf_ptr = 0;
			}
			
			public int saveFileIOBuf( CharPtr filename, int offset=0, CharPtr savestr=null )
			{
				FILEPtr fp;
			    int retval = 0;
			    uint ret = 0;
			    bool usesavedir = true;
			    // all files except envdata go in savedir
			    if (0==strcmp( filename, "envdata" ))
			        usesavedir = false;
			
			    //Mion: create a temporary file, to avoid overwriting valid files
			    // (if an error occurs)
			    CharPtr root = script_h.save_path;
			    if (usesavedir && null!=script_h.savedir)
			        root = script_h.savedir;
			
			    CharPtr fullname = new char[strlen(root)+strlen(filename)+1];
			    sprintf( fullname, "%s%s", root, filename );
			    CharPtr tmp = new char[strlen(fullname) + 9];
			    sprintf(tmp, "%s.tmpfile", fullname);
			
			    if ( (fp = ONScripter.fopen( tmp, "wb" )) == null ) {
			        retval = -1;
			        goto save_io_cleanup;
			    }
			
			    ret = (uint)fwrite(new UnsignedCharPtr(file_io_buf,offset), 1, (uint)(file_io_buf_ptr-offset), fp);
			
			    if (null!=savestr){
			        uint savelen = strlen(savestr);
			        if ( (fputc('"', fp) == EOF) ||
			             (fwrite(savestr, 1, savelen, fp) != savelen) ||
			             (fputs("\"*", fp) == EOF) ) {
			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                     "error writing to '%s'", filename);
			            errorAndCont( script_h.errbuf, null, "I/O Issue" );
			        }
			    }
			
			    fclose(fp);
			
			    if (ret != file_io_buf_ptr-offset) {
			        retval = -2;
			        goto save_io_cleanup;
			    }
			
			    //now rename the tmp file and see if errors occur
			    //(using "ret =" to avoid compiler warnings about unused return values)
			    ret = (uint)remove(fullname); //ignore errors (like if fullname doesn't exist)
			    if (0!=rename(tmp, fullname)) {
			        retval = -1;
			        goto save_io_cleanup;
			    }
			
			  save_io_cleanup:
			    fullname = null;//delete[] fullname;
			    tmp = null;//delete[] tmp;
			
			    return retval;
			}
			
			public int loadFileIOBuf( CharPtr filename )
			{
				FILEPtr fp;
			    bool usesavedir = true;
			    if (0==strcmp( filename, "envdata" ))
			        usesavedir = false;
			    if ( (fp = fopen( filename, "rb", true, usesavedir )) == null )
			        return -1;
			    
			    fseek(fp, 0, SEEK_END);
			    uint len = (uint)ftell(fp);
			    file_io_buf_ptr = len+1;
			    allocFileIOBuf();
			
			    fseek(fp, 0, SEEK_SET);
			    uint ret = fread(file_io_buf, 1, len, fp);
			    fclose(fp);
			    file_io_buf[len] = 0;
			
			    if (ret != len) return -2;
			    
			    return 0;
			}
			
			public void writeChar(char c, bool output_flag)
			{
			    if (output_flag)
			        file_io_buf[file_io_buf_ptr] = (byte)c;
			    file_io_buf_ptr++;
			}
			
			public char readChar()
			{
				if (file_io_buf_ptr >= file_io_buf_len ) return (char)0;
			    return (char)file_io_buf[file_io_buf_ptr++];
			}
			
			public void writeInt(int i, bool output_flag)
			{
			    if (output_flag){
					file_io_buf[file_io_buf_ptr++] = (byte)(i & 0xff);
			        file_io_buf[file_io_buf_ptr++] = (byte)((i >> 8) & 0xff);
			        file_io_buf[file_io_buf_ptr++] = (byte)((i >> 16) & 0xff);
					file_io_buf[file_io_buf_ptr++] = (byte)((i >> 24) & 0xff);
			    }
			    else{
			        file_io_buf_ptr += 4;
			    }
			}
			
			public int readInt()
			{
				if (file_io_buf_ptr+3 >= file_io_buf_len ) return 0;
			    
				int i = (int)(
			        (uint)file_io_buf[file_io_buf_ptr+3] << 24 |
			        (uint)file_io_buf[file_io_buf_ptr+2] << 16 |
			        (uint)file_io_buf[file_io_buf_ptr+1] << 8 |
			        (uint)file_io_buf[file_io_buf_ptr]);
			    file_io_buf_ptr += 4;
			
			    return i;
			}
			
			public void writeStr(CharPtr s, bool output_flag)
			{
			    if ( null!=s && 0!=s[0] ){
			        if (output_flag)
			        	memcpy( new UnsignedCharPtr(file_io_buf, (int)file_io_buf_ptr),
			                    s,
			                    strlen(s) );
			        file_io_buf_ptr += strlen(s);
			    }
				writeChar( (char)0, output_flag );
			}
			
			public void readStr(ref CharPtr s)
			{
			    int counter = 0;
			
			    while (file_io_buf_ptr+counter < file_io_buf_len){
			    	if (file_io_buf[(int)(file_io_buf_ptr+counter++)] == 0) break;
			    }
			    
			    if (null!=s) s = null;//delete[] *s;
			    s = null;
			    
			    if (counter > 1){
			        s = new char[counter+1];
			        memcpy(s, new UnsignedCharPtr(file_io_buf, (int)file_io_buf_ptr), (uint)counter);
			        s[counter] = (char)0;
			    }
			    file_io_buf_ptr = (uint)(file_io_buf_ptr + counter); //file_io_buf_ptr += counter;
			}
			
			public void writeVariables( int from, int to, bool output_flag )
			{
			    for (int i=from ; i<to ; i++){
			        writeInt( script_h.getVariableData(i).num, output_flag );
			        writeStr( script_h.getVariableData(i).str, output_flag );
			    }
			}
			
			public void readVariables( int from, int to )
			{
			    for (int i=from ; i<to ; i++){
			        script_h.getVariableData(i).num = readInt();
			        readStr( ref script_h.getVariableData(i).str );
			    }
			}
			
			public void writeArrayVariable( bool output_flag )
			{
			    ScriptHandler.ArrayVariable av = script_h.getRootArrayVariable();
			
			    while(null!=av){
			        int i, dim = 1;
			        for ( i=0 ; i<av.num_dim ; i++ )
			            dim *= av.dim[i];
			        
			        for ( i=0 ; i<dim ; i++ ){
			        	ulong ch = (ulong)av.data[i];
			            if (output_flag){
			                file_io_buf[file_io_buf_ptr+3] = (byte)((ch>>24) & 0xff);
			                file_io_buf[file_io_buf_ptr+2] = (byte)((ch>>16) & 0xff);
			                file_io_buf[file_io_buf_ptr+1] = (byte)((ch>>8)  & 0xff);
			                file_io_buf[file_io_buf_ptr]   = (byte)(ch & 0xff);
			            }
			            file_io_buf_ptr += 4;
			        }
			        av = av.next;
			    }
			}
			
			public void readArrayVariable()
			{
			    ScriptHandler.ArrayVariable av = script_h.getRootArrayVariable();
			
			    while(null!=av){
			        int i, dim = 1;
			        for ( i=0 ; i<av.num_dim ; i++ )
			            dim *= av.dim[i];
			        
			        for ( i=0 ; i<dim ; i++ ){
			            ulong ret;
			            if (file_io_buf_ptr+3 >= file_io_buf_len ) return;
			            ret = file_io_buf[file_io_buf_ptr+3];
			            ret = ret << 8 | file_io_buf[file_io_buf_ptr+2];
			            ret = ret << 8 | file_io_buf[file_io_buf_ptr+1];
			            ret = ret << 8 | file_io_buf[file_io_buf_ptr];
			            file_io_buf_ptr += 4;
			            av.data[i] = (int)ret;
			        }
			        av = av.next;
			    }
			}
			
			public void writeLog( ScriptHandler.LogInfo info )
			{
			    file_io_buf_ptr = 0;
			    bool output_flag = false;
			    for (int n=0 ; n<2 ; n++){
			        int  i,j;
			        char[] buf = new char[10];
			
			        sprintf( buf, "%d", info.num_logs );
			        for ( i=0 ; i<(int)strlen( buf ) ; i++ ) writeChar( buf[i], output_flag );
			        writeChar( (char)0x0a, output_flag );
			
			        ScriptHandler.LogLink cur = info.root_log.next;
			        for ( i=0 ; i<info.num_logs ; i++ ){
			            writeChar( '"', output_flag );
			            for ( j=0 ; j<(int)strlen( cur.name ) ; j++ )
			            	writeChar( (char)(cur.name[j] ^ 0x84), output_flag );
			            writeChar( '"', output_flag );
			            cur = cur.next;
			        }
			
			        if (n==1) break;
			        allocFileIOBuf();
			        output_flag = true;
			    }
			
			    if (0!=saveFileIOBuf( info.filename )){
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                 "can't write to '%s'", info.filename);
			        errorAndExit( script_h.errbuf, null, "I/O Error" );
			    }
			}
			
			public void readLog( ScriptHandler.LogInfo info )
			{
			    script_h.resetLog( info );
			    
			    if (loadFileIOBuf( info.filename ) == 0){
			        int i, j, ch, count = 0;
			        char[] buf = new char[100];
			
			        while( (ch = readChar()) != 0x0a ){
			            count = count * 10 + ch - '0';
			        }
			
			        for ( i=0 ; i<count ; i++ ){
			            readChar();
			            j = 0; 
			            while( (ch = readChar()) != '"' ) buf[j++] = (char)(ch ^ 0x84);
			            buf[j] = '\0';
			
			            script_h.findAndAddLog( info, buf, true );
			        }
			    }
			}
			
			public void deleteNestInfo()
			{
			    NestInfo info = root_nest_info.next;
			    while(null!=info){
			        NestInfo tmp = info;
			        info = info.next;
			        tmp = null;
			    }
			    root_nest_info.next = null;
			    last_nest_info = root_nest_info;
			}
			
			#if !NO_LAYER_EFFECTS
			public void deleteLayerInfo()
			{
			    while (null!=layer_info) {
			        LayerInfo tmp = layer_info;
			        layer_info = layer_info.next;
			        tmp = null;
			    }
			}
			#endif
			
			public void setStr( ref CharPtr dst, CharPtr src, int num=-1 )
			{
				if ( dst!=null ) dst = null;//delete[] *dst;
			    dst = null;
			    
			    if ( src!=null ){
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
			
			public void setCurrentLabel( CharPtr label )
			{
			    current_label_info = script_h.lookupLabel( label );
			    current_line = script_h.getLineByAddress( current_label_info.start_address );
			    script_h.setCurrent( current_label_info.start_address );
			}
			
			public void readToken()
			{
			    script_h.readToken();
			    string_buffer_offset = 0;
			
			    if (script_h.isText() && (linepage_mode > 0) &&
			        (script_h.getNext()[0] == 0x0a)){
			        // ugly work around
			        uint len = strlen(script_h.getStringBuffer());
			        //Mion: text buffers don't end with newline anymore...
			        // checked next_script for the newline
			        if (script_h.getStringBuffer()[len-1] == '_'){
			            script_h.trimStringBuffer(1);
			        } else if ((script_h.getStringBuffer()[len-1] != '@') &&
			                   (script_h.getStringBuffer()[len-1] != '\\')){
			            if (linepage_mode == 1){
			                script_h.addStringBuffer('\\');
			            }
			            else {
			                // insert a clickwait-or-newpage
			                script_h.addStringBuffer('\\');
			                script_h.addStringBuffer('@');
			            }
			        }
			    }
			}
			
			public int readEffect( EffectLink effect )
			{
				int num = 1;
			    
			    effect.effect = script_h.readInt();
			    if ( (script_h.getEndStatus() & ScriptHandler.END_COMMA)!=0 ){
			        num++;
			        effect.duration = script_h.readInt();
			        if ( (script_h.getEndStatus() & ScriptHandler.END_COMMA)!=0 ){
			            num++;
			            CharPtr buf = script_h.readStr();
			            effect.anim.setImageName( buf );
			        }
			        else
			            effect.anim.remove();
			    }
			    else if (effect.effect < 0 || effect.effect > 255){
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                 "effect %d out of range, changing to 0", effect.effect);
			        errorAndCont( script_h.errbuf );
			        effect.effect = 0; // to suppress error
			    }
			
			    //printf("readEffect %d: %d %d %s\n", num, effect->effect, effect->duration, effect->anim.image_name );
			    return num;
			}
			
			public EffectLink parseEffect(bool init_flag)
			{
				if (init_flag) tmp_effect.anim.remove();
			
			    int num = readEffect(tmp_effect);
			
			    if (num > 1) return tmp_effect;
			    if (tmp_effect.effect == 0 || tmp_effect.effect == 1) return tmp_effect;
			
			    EffectLink link = root_effect_link;
			    while(null!=link){
			        if (link.no == tmp_effect.effect) return link;
			        link = link.next;
			    }
			
			    snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			             "effect %d not found", tmp_effect.effect);
			    errorAndExit( script_h.errbuf );
			
			    return null;
			}
			
			public FILEPtr fopen( CharPtr path, CharPtr mode, bool save = false, bool usesavedir = false )
			{
				CharPtr root;
			    CharPtr file_name;
			    FILEPtr fp = null;
			
			    if (usesavedir && null!=script_h.savedir) {
			        root = script_h.savedir;
			        file_name = new char[strlen(root)+strlen(path)+1];
			        sprintf( file_name, "%s%s", root, path );
			        //printf("parser:fopen(\"%s\")\n", file_name);
			
			        fp = ONScripter.fopen( file_name, mode );
			    } else if (save) {
			        root = script_h.save_path;
			        file_name = new char[strlen(root)+strlen(path)+1];
			        sprintf( file_name, "%s%s", root, path );
			        //printf("parser:fopen(\"%s\")\n", file_name);
			
			        fp = ONScripter.fopen( file_name, mode );
			    } else {
			        // search within archive_path dirs
			        file_name = new char[archive_path.max_path_len()+strlen(path)+1];
			        for (int n=0; n<(archive_path.get_num_paths()); n++) {
			            root = archive_path.get_path(n);
			            //printf("root: %s\n", root);
			            sprintf( file_name, "%s%s", root, path );
			            //printf("parser:fopen(\"%s\")\n", file_name);
			            fp = ONScripter.fopen( file_name, mode );
			            if (fp != null) break;
			        }
			    }
			
			    file_name = null; //delete[] file_name;
			    return fp;
			}
			
			public void createKeyTable( CharPtr key_exe )
			{
			    if (null==key_exe) return;
			    
			    FILEPtr fp = ONScripter.fopen(key_exe, "rb");
			    if (fp == null){
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                 "createKeyTable: can't open EXE file '%s'", key_exe);
			        errorAndCont(script_h.errbuf, null, "Init Issue", true);
			        return;
			    }
			
			    key_table = new UnsignedCharPtr(new byte[256]);
			
			    int i;
			    for (i=0 ; i<256 ; i++) key_table[i] = (byte)i;
			
			    byte[] ring_buffer = new byte[256];
			    int ring_start = 0, ring_last = 0;
			    
			    int ch, count;
			    while((ch = fgetc(fp)) != EOF){
			        i = ring_start;
			        count = 0;
			        while (i != ring_last &&
			               ring_buffer[i] != ch ){
			            count++;
			            i = (i+1)%256;
			        }
			        if (i == ring_last && count == 255) break;
			        if (i != ring_last)
			            ring_start = (i+1)%256;
			        ring_buffer[ring_last] = (byte)ch;
			        ring_last = (ring_last+1)%256;
			    }
			    fclose(fp);
			
			    if (ch == EOF)
			        errorAndExit( "createKeyTable: can't find a key table.", null, "Init Issue", true );
			
			    // Key table creation
			    ring_buffer[ring_last] = (byte)ch;
			    for (i=0 ; i<256 ; i++)
			    	key_table[ring_buffer[(ring_start+i)%256]] = (byte)i;
			}
			
			//Mion: for setting the default Save/Load Menu labels
			//(depending on the current system menu language)
			public void setDefaultMenuLabels()
			{
			    if (script_h.system_menu_script != ScriptHandler.LanguageScript.JAPANESE_SCRIPT) {
			        setStr( ref save_menu_name, "[ Save ]" );
			        setStr( ref load_menu_name, "[ Load ]" );
			        setStr( ref save_item_name, "Slot " );
			    }
			    else {
			        setStr( ref save_menu_name, "亙僙乕僽亜" );
			        setStr( ref load_menu_name, "亙儘乕僪亜" );
			        setStr( ref save_item_name, "偟偍傝" );
			    }
			}
			
			//Mion: for kinsoku
			public void setKinsoku(CharPtr start_chrs, CharPtr end_chrs, bool add)
			{
			    int num_start, num_end, i;
			    CharPtr kchr;
			    Kinsoku[] tmp;
			
			    // count chrs
			    num_start = 0;
			    kchr = start_chrs;
			    while (kchr[0] != '\0') {
			    	if (IS_TWO_BYTE(kchr[0])) kchr.inc();
			        kchr.inc();
			        num_start++;
			    }
			
			    num_end = 0;
			    kchr = end_chrs;
			    while (kchr[0] != '\0') {
			    	if (IS_TWO_BYTE(kchr[0])) kchr.inc();
			        kchr.inc();
			        num_end++;
			    }
			
			    if (add) {
			        if (start_kinsoku != null)
			            tmp = start_kinsoku;
			        else {
			            tmp = new Kinsoku[1];
			            num_start_kinsoku = 0;
			        }
			    } else {
			        if (start_kinsoku != null)
			            start_kinsoku = null; //delete[] start_kinsoku;
			        tmp = new Kinsoku[1];
			        num_start_kinsoku = 0;
			    }
			    start_kinsoku = new Kinsoku[num_start_kinsoku + num_start];
			    kchr = start_chrs;
			    for (i=0; i<num_start_kinsoku+num_start; i++) {
			        if (i < num_start_kinsoku)
			            start_kinsoku[i].chr[0] = tmp[i].chr[0];
			        else
			        	{ start_kinsoku[i].chr[0] = kchr[0]; kchr.inc(); }
			        if (IS_TWO_BYTE(start_kinsoku[i].chr[0])) {
			            if (i < num_start_kinsoku)
			                start_kinsoku[i].chr[1] = tmp[i].chr[1];
			            else
			            	{ start_kinsoku[i].chr[1] = kchr[0]; kchr.inc(); }
			        } else {
			            start_kinsoku[i].chr[1] = '\0';
			        }
			    }
			    num_start_kinsoku += num_start;
			    tmp = null; //delete[] tmp;
			
			    if (add) {
			        if (end_kinsoku != null)
			            tmp = end_kinsoku;
			        else {
			            tmp = new Kinsoku[1];
			            num_end_kinsoku = 0;
			        }
			    } else {
			        if (end_kinsoku != null)
			            end_kinsoku = null; //delete[] end_kinsoku;
			        tmp = new Kinsoku[1];
			        num_end_kinsoku = 0;
			    }
			    end_kinsoku = new Kinsoku[num_end_kinsoku + num_end];
			    kchr = end_chrs;
			    for (i=0; i<num_end_kinsoku+num_end; i++) {
			        if (i < num_end_kinsoku)
			            end_kinsoku[i].chr[0] = tmp[i].chr[0];
			        else
			        	{ end_kinsoku[i].chr[0] = kchr[0]; kchr.inc(); }
			        if (IS_TWO_BYTE(end_kinsoku[i].chr[0])) {
			            if (i < num_end_kinsoku)
			                end_kinsoku[i].chr[1] = tmp[i].chr[1];
			            else
			            	{ end_kinsoku[i].chr[1] = kchr[0]; kchr.inc(); }
			        } else {
			            end_kinsoku[i].chr[1] = '\0';
			        }
			    }
			    num_end_kinsoku += num_end;
			    tmp = null; //delete[] tmp;
			}
			
			public bool isStartKinsoku(CharPtr str)
			{
				for (int i=0; i<num_start_kinsoku; i++) {
					if ((start_kinsoku[i].chr[0] == str[0]) &&
					    (start_kinsoku[i].chr[1] == str[1]))
			            return true;
			    }
			    return false;
			}
			
			public bool isEndKinsoku(CharPtr str)
			{
				for (int i=0; i<num_end_kinsoku; i++) {
					if ((end_kinsoku[i].chr[0] == str[0]) &&
					    (end_kinsoku[i].chr[1] == str[1]))
			            return true;
			    }
			    return false;
			}
		}
	}
}
