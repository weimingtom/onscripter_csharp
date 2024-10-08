﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-26
 * Time: 1:12
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
		 *  ONScripterLabel.cpp - Execution block parser of ONScripter-EN
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
		
		// Modified by Haeleth, autumn 2006, to remove unnecessary diagnostics and support OS X/Linux packaging better.
		
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
//		
//		#include "ONScripterLabel.h"
//		#include <cstdio>
//		
//		#ifdef MACOSX
//		#include "cocoa_alertbox.h"
//		#include "cocoa_directories.h"
//		
//		#ifdef USE_PPC_GFX
//		#include <sys/types.h>
//		#include <sys/sysctl.h>
//		#endif // USE_PPC_GFX
//		
//		#endif // MACOSX
//		
//		
//		#ifdef WIN32
//		#include <windows.h>
//		#include "SDL_syswm.h"
//		#include "winres.h"
//		typedef HRESULT (WINAPI *GETFOLDERPATH)(HWND, int, HANDLE, DWORD, LPTSTR);
//		#endif
//		#ifdef LINUX
//		#include <unistd.h>
//		#include <sys/stat.h>
//		#include <sys/types.h>
//		#include <pwd.h>
//		#endif
//		#if !defined(WIN32) && !defined(MACOSX)
//		#include "resources.h"
//		#endif
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
//		
//		#if defined(WIN32)
//		#else
//		extern void initSJIS2UTF16();
//		#endif
//		extern "C" void waveCallback( int channel );
//		
//		#define DEFAULT_AUDIOBUF 2048
		
		private const string FONT_FILE = "default.ttf";
		private const string REGISTRY_FILE = "registry.txt";
		private const string DLL_FILE = "dll.txt";
		//haeleth change to use English-language font name
		private const string DEFAULT_ENV_FONT = "MS Gothic";
		
//		#define SAVEFILE_VERSION_MAJOR 2
//		#define SAVEFILE_VERSION_MINOR 6
		
		public delegate int FuncList(ONScripterLabel this_);
		public class FuncLUT{
			public char[] command = new char[30];
		    public FuncList method;
		    public FuncLUT(CharPtr str, FuncList method)
		    {
		    	strcpy(this.command, str);
		    	this.method = method;
		    }
		};
		public static FuncLUT[] func_lut = new FuncLUT[] {
			new FuncLUT("yesnobox",   delegate (ONScripterLabel this_) { return this_.yesnoboxCommand(); } ),
		    new FuncLUT("wavestop",   delegate (ONScripterLabel this_) { return this_.wavestopCommand(); } ),
		    new FuncLUT("waveloop",   delegate (ONScripterLabel this_) { return this_.waveCommand(); } ),
		    new FuncLUT("wave",   delegate (ONScripterLabel this_) { return this_.waveCommand(); } ),
		    new FuncLUT("waittimer",   delegate (ONScripterLabel this_) { return this_.waittimerCommand(); } ),
		    new FuncLUT("wait",   delegate (ONScripterLabel this_) { return this_.waitCommand(); } ),
		    new FuncLUT("vsp2",   delegate (ONScripterLabel this_) { return this_.vspCommand(); } ),
		    new FuncLUT("vsp",   delegate (ONScripterLabel this_) { return this_.vspCommand(); } ),
		    new FuncLUT("voicevol",   delegate (ONScripterLabel this_) { return this_.voicevolCommand(); } ),
		    new FuncLUT("trap",   delegate (ONScripterLabel this_) { return this_.trapCommand(); } ),
		    new FuncLUT("transbtn",  delegate (ONScripterLabel this_) { return this_.transbtnCommand(); } ),
		    new FuncLUT("textspeeddefault",   delegate (ONScripterLabel this_) { return this_.textspeeddefaultCommand(); } ),
		    new FuncLUT("textspeed",   delegate (ONScripterLabel this_) { return this_.textspeedCommand(); } ),
		    new FuncLUT("textshow",   delegate (ONScripterLabel this_) { return this_.textshowCommand(); } ),
		    new FuncLUT("texton",   delegate (ONScripterLabel this_) { return this_.textonCommand(); } ),
		    new FuncLUT("textoff",   delegate (ONScripterLabel this_) { return this_.textoffCommand(); } ),
		    new FuncLUT("texthide",   delegate (ONScripterLabel this_) { return this_.texthideCommand(); } ),
		    new FuncLUT("textexbtn",   delegate (ONScripterLabel this_) { return this_.textexbtnCommand(); } ),
		    new FuncLUT("textclear",   delegate (ONScripterLabel this_) { return this_.textclearCommand(); } ),
		    new FuncLUT("textbtnwait",   delegate (ONScripterLabel this_) { return this_.btnwaitCommand(); } ),
		    new FuncLUT("textbtnstart",   delegate (ONScripterLabel this_) { return this_.textbtnstartCommand(); } ),
		    new FuncLUT("textbtnoff",   delegate (ONScripterLabel this_) { return this_.textbtnoffCommand(); } ),
		    new FuncLUT("texec",   delegate (ONScripterLabel this_) { return this_.texecCommand(); } ),
		    new FuncLUT("tateyoko",   delegate (ONScripterLabel this_) { return this_.tateyokoCommand(); } ),
		    new FuncLUT("tal", delegate (ONScripterLabel this_) { return this_.talCommand(); } ),
		    new FuncLUT("tablegoto",   delegate (ONScripterLabel this_) { return this_.tablegotoCommand(); } ),
		    new FuncLUT("systemcall",   delegate (ONScripterLabel this_) { return this_.systemcallCommand(); } ),
		    new FuncLUT("strsph",   delegate (ONScripterLabel this_) { return this_.strspCommand(); } ),
		    new FuncLUT("strsp",   delegate (ONScripterLabel this_) { return this_.strspCommand(); } ),
		    new FuncLUT("stop",   delegate (ONScripterLabel this_) { return this_.stopCommand(); } ),
		    new FuncLUT("sp_rgb_gradation",   delegate (ONScripterLabel this_) { return this_.sp_rgb_gradationCommand(); } ),
		    new FuncLUT("spstr",   delegate (ONScripterLabel this_) { return this_.spstrCommand(); } ),
		    new FuncLUT("spreload",   delegate (ONScripterLabel this_) { return this_.spreloadCommand(); } ),
		    new FuncLUT("splitstring",   delegate (ONScripterLabel this_) { return this_.splitCommand(); } ),
		    new FuncLUT("split",   delegate (ONScripterLabel this_) { return this_.splitCommand(); } ),
		    new FuncLUT("spclclk",   delegate (ONScripterLabel this_) { return this_.spclclkCommand(); } ),
		    new FuncLUT("spbtn",   delegate (ONScripterLabel this_) { return this_.spbtnCommand(); } ),
		    new FuncLUT("skipoff",   delegate (ONScripterLabel this_) { return this_.skipoffCommand(); } ),
		    new FuncLUT("shell",   delegate (ONScripterLabel this_) { return this_.shellCommand(); } ),
		    new FuncLUT("sevol",   delegate (ONScripterLabel this_) { return this_.sevolCommand(); } ),
		    new FuncLUT("setwindow3",   delegate (ONScripterLabel this_) { return this_.setwindow3Command(); } ),
		    new FuncLUT("setwindow2",   delegate (ONScripterLabel this_) { return this_.setwindow2Command(); } ),
		    new FuncLUT("setwindow",   delegate (ONScripterLabel this_) { return this_.setwindowCommand(); } ),
		    new FuncLUT("seteffectspeed",   delegate (ONScripterLabel this_) { return this_.seteffectspeedCommand(); } ),
		    new FuncLUT("setcursor",   delegate (ONScripterLabel this_) { return this_.setcursorCommand(); } ),
		    new FuncLUT("selnum",   delegate (ONScripterLabel this_) { return this_.selectCommand(); } ),
		    new FuncLUT("selgosub",   delegate (ONScripterLabel this_) { return this_.selectCommand(); } ),
		    new FuncLUT("selectbtnwait", delegate (ONScripterLabel this_) { return this_.btnwaitCommand(); } ),
		    new FuncLUT("select",   delegate (ONScripterLabel this_) { return this_.selectCommand(); } ),
		    new FuncLUT("savetime",   delegate (ONScripterLabel this_) { return this_.savetimeCommand(); } ),
		    new FuncLUT("savescreenshot2",   delegate (ONScripterLabel this_) { return this_.savescreenshotCommand(); } ),
		    new FuncLUT("savescreenshot",   delegate (ONScripterLabel this_) { return this_.savescreenshotCommand(); } ),
		    new FuncLUT("saveon",   delegate (ONScripterLabel this_) { return this_.saveonCommand(); } ),
		    new FuncLUT("saveoff",   delegate (ONScripterLabel this_) { return this_.saveoffCommand(); } ),
		    new FuncLUT("savegame2",   delegate (ONScripterLabel this_) { return this_.savegameCommand(); } ),
		    new FuncLUT("savegame",   delegate (ONScripterLabel this_) { return this_.savegameCommand(); } ),
		    new FuncLUT("savefileexist",   delegate (ONScripterLabel this_) { return this_.savefileexistCommand(); } ),
		    new FuncLUT("r_trap",   delegate (ONScripterLabel this_) { return this_.trapCommand(); } ),
		    new FuncLUT("rnd",   delegate (ONScripterLabel this_) { return this_.rndCommand(); } ),
		    new FuncLUT("rnd2",   delegate (ONScripterLabel this_) { return this_.rndCommand(); } ),
		    new FuncLUT("rmode",   delegate (ONScripterLabel this_) { return this_.rmodeCommand(); } ),
		    new FuncLUT("resettimer",   delegate (ONScripterLabel this_) { return this_.resettimerCommand(); } ),
		    new FuncLUT("resetmenu", delegate (ONScripterLabel this_) { return this_.resetmenuCommand(); } ),
		    new FuncLUT("reset",   delegate (ONScripterLabel this_) { return this_.resetCommand(); } ),
		    new FuncLUT("repaint",   delegate (ONScripterLabel this_) { return this_.repaintCommand(); } ),
		    new FuncLUT("quakey",   delegate (ONScripterLabel this_) { return this_.quakeCommand(); } ),
		    new FuncLUT("quakex",   delegate (ONScripterLabel this_) { return this_.quakeCommand(); } ),
		    new FuncLUT("quake",   delegate (ONScripterLabel this_) { return this_.quakeCommand(); } ),
		    new FuncLUT("puttext",   delegate (ONScripterLabel this_) { return this_.puttextCommand(); } ),
		    new FuncLUT("prnumclear",   delegate (ONScripterLabel this_) { return this_.prnumclearCommand(); } ),
		    new FuncLUT("prnum",   delegate (ONScripterLabel this_) { return this_.prnumCommand(); } ),
		    new FuncLUT("print",   delegate (ONScripterLabel this_) { return this_.printCommand(); } ),
		    new FuncLUT("language", delegate (ONScripterLabel this_) { return this_.languageCommand(); } ),
		    new FuncLUT("playstop",   delegate (ONScripterLabel this_) { return this_.playstopCommand(); } ),
		    new FuncLUT("playonce",   delegate (ONScripterLabel this_) { return this_.playCommand(); } ),
		    new FuncLUT("play",   delegate (ONScripterLabel this_) { return this_.playCommand(); } ),
		    new FuncLUT("okcancelbox",   delegate (ONScripterLabel this_) { return this_.yesnoboxCommand(); } ),
		    new FuncLUT("ofscpy", delegate (ONScripterLabel this_) { return this_.ofscopyCommand(); } ),
		    new FuncLUT("ofscopy", delegate (ONScripterLabel this_) { return this_.ofscopyCommand(); } ),
		    new FuncLUT("nega", delegate (ONScripterLabel this_) { return this_.negaCommand(); } ),
		    new FuncLUT("msp2", delegate (ONScripterLabel this_) { return this_.mspCommand(); } ),
		    new FuncLUT("msp", delegate (ONScripterLabel this_) { return this_.mspCommand(); } ),
		    new FuncLUT("mpegplay", delegate (ONScripterLabel this_) { return this_.movieCommand(); } ),
		    new FuncLUT("mp3vol", delegate (ONScripterLabel this_) { return this_.mp3volCommand(); } ),
		    new FuncLUT("mp3stop", delegate (ONScripterLabel this_) { return this_.mp3stopCommand(); } ),
		    new FuncLUT("mp3save", delegate (ONScripterLabel this_) { return this_.mp3Command(); } ),
		    new FuncLUT("mp3loop", delegate (ONScripterLabel this_) { return this_.mp3Command(); } ),
		    new FuncLUT("mp3fadeout", delegate (ONScripterLabel this_) { return this_.mp3fadeoutCommand(); } ),
		    new FuncLUT("mp3fadein", delegate (ONScripterLabel this_) { return this_.mp3fadeinCommand(); } ),
		    new FuncLUT("mp3", delegate (ONScripterLabel this_) { return this_.mp3Command(); } ),
		    new FuncLUT("movie", delegate (ONScripterLabel this_) { return this_.movieCommand(); } ),
		    new FuncLUT("movemousecursor", delegate (ONScripterLabel this_) { return this_.movemousecursorCommand(); } ),
		    new FuncLUT("mousemode", delegate (ONScripterLabel this_) { return this_.mousemodeCommand(); } ),
		    new FuncLUT("monocro", delegate (ONScripterLabel this_) { return this_.monocroCommand(); } ),
		    new FuncLUT("minimizewindow", delegate (ONScripterLabel this_) { return this_.minimizewindowCommand(); } ),
		    new FuncLUT("mesbox", delegate (ONScripterLabel this_) { return this_.mesboxCommand(); } ),
		    new FuncLUT("menu_window", delegate (ONScripterLabel this_) { return this_.menu_windowCommand(); } ),
		    new FuncLUT("menu_waveon", delegate (ONScripterLabel this_) { return this_.menu_waveonCommand(); } ),
		    new FuncLUT("menu_waveoff", delegate (ONScripterLabel this_) { return this_.menu_waveoffCommand(); } ),
		    new FuncLUT("menu_full", delegate (ONScripterLabel this_) { return this_.menu_fullCommand(); } ),
		    new FuncLUT("menu_click_page", delegate (ONScripterLabel this_) { return this_.menu_click_pageCommand(); } ),
		    new FuncLUT("menu_click_def", delegate (ONScripterLabel this_) { return this_.menu_click_defCommand(); } ),
		    new FuncLUT("menu_automode", delegate (ONScripterLabel this_) { return this_.menu_automodeCommand(); } ),
		    new FuncLUT("lsph2sub", delegate (ONScripterLabel this_) { return this_.lsp2Command(); } ),
		    new FuncLUT("lsph2add", delegate (ONScripterLabel this_) { return this_.lsp2Command(); } ),
		    new FuncLUT("lsph2", delegate (ONScripterLabel this_) { return this_.lsp2Command(); } ),
		    new FuncLUT("lsph", delegate (ONScripterLabel this_) { return this_.lspCommand(); } ),
		    new FuncLUT("lsp2sub", delegate (ONScripterLabel this_) { return this_.lsp2Command(); } ),
		    new FuncLUT("lsp2add", delegate (ONScripterLabel this_) { return this_.lsp2Command(); } ),
		    new FuncLUT("lsp2", delegate (ONScripterLabel this_) { return this_.lsp2Command(); } ),
		    new FuncLUT("lsp", delegate (ONScripterLabel this_) { return this_.lspCommand(); } ),
		    new FuncLUT("lr_trap",   delegate (ONScripterLabel this_) { return this_.trapCommand(); } ),
		    new FuncLUT("lrclick",   delegate (ONScripterLabel this_) { return this_.clickCommand(); } ),
		    new FuncLUT("loopbgmstop", delegate (ONScripterLabel this_) { return this_.loopbgmstopCommand(); } ),
		    new FuncLUT("loopbgm", delegate (ONScripterLabel this_) { return this_.loopbgmCommand(); } ),
		    new FuncLUT("lookbackflush", delegate (ONScripterLabel this_) { return this_.lookbackflushCommand(); } ),
		    new FuncLUT("lookbackbutton",      delegate (ONScripterLabel this_) { return this_.lookbackbuttonCommand(); } ),
		    new FuncLUT("logsp2", delegate (ONScripterLabel this_) { return this_.logspCommand(); } ),
		    new FuncLUT("logsp", delegate (ONScripterLabel this_) { return this_.logspCommand(); } ),
		    new FuncLUT("locate", delegate (ONScripterLabel this_) { return this_.locateCommand(); } ),
		    new FuncLUT("loadgame", delegate (ONScripterLabel this_) { return this_.loadgameCommand(); } ),
		    new FuncLUT("linkcolor", delegate (ONScripterLabel this_) { return this_.linkcolorCommand(); } ),
		    new FuncLUT("ld", delegate (ONScripterLabel this_) { return this_.ldCommand(); } ),
		    new FuncLUT("layermessage", delegate (ONScripterLabel this_) { return this_.layermessageCommand(); } ),
		    new FuncLUT("jumpf", delegate (ONScripterLabel this_) { return this_.jumpfCommand(); } ),
		    new FuncLUT("jumpb", delegate (ONScripterLabel this_) { return this_.jumpbCommand(); } ),
		    new FuncLUT("isfull", delegate (ONScripterLabel this_) { return this_.isfullCommand(); } ),
		    new FuncLUT("isskip", delegate (ONScripterLabel this_) { return this_.isskipCommand(); } ),
		    new FuncLUT("ispage", delegate (ONScripterLabel this_) { return this_.ispageCommand(); } ),
		    new FuncLUT("isdown", delegate (ONScripterLabel this_) { return this_.isdownCommand(); } ),
		    new FuncLUT("insertmenu", delegate (ONScripterLabel this_) { return this_.insertmenuCommand(); } ),
		    new FuncLUT("input", delegate (ONScripterLabel this_) { return this_.inputCommand(); } ),
		    new FuncLUT("indent", delegate (ONScripterLabel this_) { return this_.indentCommand(); } ),
		    new FuncLUT("humanorder", delegate (ONScripterLabel this_) { return this_.humanorderCommand(); } ),
		    new FuncLUT("getzxc", delegate (ONScripterLabel this_) { return this_.getzxcCommand(); } ),
		    new FuncLUT("getvoicevol", delegate (ONScripterLabel this_) { return this_.getvoicevolCommand(); } ),
		    new FuncLUT("getversion", delegate (ONScripterLabel this_) { return this_.getversionCommand(); } ),
		    new FuncLUT("gettimer", delegate (ONScripterLabel this_) { return this_.gettimerCommand(); } ),
		    new FuncLUT("gettextbtnstr", delegate (ONScripterLabel this_) { return this_.gettextbtnstrCommand(); } ),
		    new FuncLUT("gettext", delegate (ONScripterLabel this_) { return this_.gettextCommand(); } ),
		    new FuncLUT("gettaglog", delegate (ONScripterLabel this_) { return this_.gettaglogCommand(); } ),
		    new FuncLUT("gettag", delegate (ONScripterLabel this_) { return this_.gettagCommand(); } ),
		    new FuncLUT("gettab", delegate (ONScripterLabel this_) { return this_.gettabCommand(); } ),
		    new FuncLUT("getspsize", delegate (ONScripterLabel this_) { return this_.getspsizeCommand(); } ),
		    new FuncLUT("getspmode", delegate (ONScripterLabel this_) { return this_.getspmodeCommand(); } ),
		    new FuncLUT("getskipoff", delegate (ONScripterLabel this_) { return this_.getskipoffCommand(); } ),
		    new FuncLUT("getsevol", delegate (ONScripterLabel this_) { return this_.getsevolCommand(); } ),
		    new FuncLUT("getscreenshot", delegate (ONScripterLabel this_) { return this_.getscreenshotCommand(); } ),
		    new FuncLUT("getsavestr", delegate (ONScripterLabel this_) { return this_.getsavestrCommand(); } ),
		    new FuncLUT("getret", delegate (ONScripterLabel this_) { return this_.getretCommand(); } ),
		    new FuncLUT("getreg", delegate (ONScripterLabel this_) { return this_.getregCommand(); } ),
		    new FuncLUT("getpageup", delegate (ONScripterLabel this_) { return this_.getpageupCommand(); } ),
		    new FuncLUT("getpage", delegate (ONScripterLabel this_) { return this_.getpageCommand(); } ),
		    new FuncLUT("getnextline", delegate (ONScripterLabel this_) { return this_.getcursorposCommand(); } ),
		    new FuncLUT("getmp3vol", delegate (ONScripterLabel this_) { return this_.getmp3volCommand(); } ),
		    new FuncLUT("getmousepos", delegate (ONScripterLabel this_) { return this_.getmouseposCommand(); } ),
		    new FuncLUT("getmouseover", delegate (ONScripterLabel this_) { return this_.getmouseoverCommand(); } ),
		    new FuncLUT("getmclick", delegate (ONScripterLabel this_) { return this_.getmclickCommand(); } ),
		    new FuncLUT("getlogtext", delegate (ONScripterLabel this_) { return this_.gettextCommand(); } ),
		    new FuncLUT("getlog", delegate (ONScripterLabel this_) { return this_.getlogCommand(); } ),
		    new FuncLUT("getinsert", delegate (ONScripterLabel this_) { return this_.getinsertCommand(); } ),
		    new FuncLUT("getfunction", delegate (ONScripterLabel this_) { return this_.getfunctionCommand(); } ),
		    new FuncLUT("getenter", delegate (ONScripterLabel this_) { return this_.getenterCommand(); } ),
		    new FuncLUT("getcursorpos2", delegate (ONScripterLabel this_) { return this_.getcursorposCommand(); } ),
		    new FuncLUT("getcursorpos", delegate (ONScripterLabel this_) { return this_.getcursorposCommand(); } ),
		    new FuncLUT("getcursor", delegate (ONScripterLabel this_) { return this_.getcursorCommand(); } ),
		    new FuncLUT("getcselstr", delegate (ONScripterLabel this_) { return this_.getcselstrCommand(); } ),
		    new FuncLUT("getcselnum", delegate (ONScripterLabel this_) { return this_.getcselnumCommand(); } ),
		    new FuncLUT("getbtntimer", delegate (ONScripterLabel this_) { return this_.gettimerCommand(); } ),
		    new FuncLUT("getbgmvol", delegate (ONScripterLabel this_) { return this_.getmp3volCommand(); } ),
		    new FuncLUT("game", delegate (ONScripterLabel this_) { return this_.gameCommand(); } ),
		    new FuncLUT("flushout", delegate (ONScripterLabel this_) { return this_.flushoutCommand(); } ),
		    new FuncLUT("fileexist", delegate (ONScripterLabel this_) { return this_.fileexistCommand(); } ),
		    new FuncLUT("existspbtn", delegate (ONScripterLabel this_) { return this_.spbtnCommand(); } ),
		    new FuncLUT("exec_dll", delegate (ONScripterLabel this_) { return this_.exec_dllCommand(); } ),
		    new FuncLUT("exbtn_d", delegate (ONScripterLabel this_) { return this_.exbtnCommand(); } ),
		    new FuncLUT("exbtn", delegate (ONScripterLabel this_) { return this_.exbtnCommand(); } ),
		    new FuncLUT("erasetextwindow", delegate (ONScripterLabel this_) { return this_.erasetextwindowCommand(); } ),
		    new FuncLUT("erasetextbtn", delegate (ONScripterLabel this_) { return this_.erasetextbtnCommand(); } ),
		    new FuncLUT("effectskip", delegate (ONScripterLabel this_) { return this_.effectskipCommand(); } ),
		    new FuncLUT("end", delegate (ONScripterLabel this_) { return this_.endCommand(); } ),
		    new FuncLUT("dwavestop", delegate (ONScripterLabel this_) { return this_.dwavestopCommand(); } ),
		    new FuncLUT("dwaveplayloop", delegate (ONScripterLabel this_) { return this_.dwaveCommand(); } ),
		    new FuncLUT("dwaveplay", delegate (ONScripterLabel this_) { return this_.dwaveCommand(); } ),
		    new FuncLUT("dwaveloop", delegate (ONScripterLabel this_) { return this_.dwaveCommand(); } ),
		    new FuncLUT("dwaveload", delegate (ONScripterLabel this_) { return this_.dwaveCommand(); } ),
		    new FuncLUT("dwave", delegate (ONScripterLabel this_) { return this_.dwaveCommand(); } ),
		    new FuncLUT("drawtext", delegate (ONScripterLabel this_) { return this_.drawtextCommand(); } ),
		    new FuncLUT("drawsp3", delegate (ONScripterLabel this_) { return this_.drawsp3Command(); } ),
		    new FuncLUT("drawsp2", delegate (ONScripterLabel this_) { return this_.drawsp2Command(); } ),
		    new FuncLUT("drawsp", delegate (ONScripterLabel this_) { return this_.drawspCommand(); } ),
		    new FuncLUT("drawfill", delegate (ONScripterLabel this_) { return this_.drawfillCommand(); } ),
		    new FuncLUT("drawclear", delegate (ONScripterLabel this_) { return this_.drawclearCommand(); } ),
		    new FuncLUT("drawbg2", delegate (ONScripterLabel this_) { return this_.drawbg2Command(); } ),
		    new FuncLUT("drawbg", delegate (ONScripterLabel this_) { return this_.drawbgCommand(); } ),
		    new FuncLUT("draw", delegate (ONScripterLabel this_) { return this_.drawCommand(); } ),
		    new FuncLUT("deletescreenshot", delegate (ONScripterLabel this_) { return this_.deletescreenshotCommand(); } ),
		    new FuncLUT("delay", delegate (ONScripterLabel this_) { return this_.delayCommand(); } ),
		    new FuncLUT("definereset", delegate (ONScripterLabel this_) { return this_.defineresetCommand(); } ),
		    new FuncLUT("csp2", delegate (ONScripterLabel this_) { return this_.cspCommand(); } ),
		    new FuncLUT("csp", delegate (ONScripterLabel this_) { return this_.cspCommand(); } ),
		    new FuncLUT("cselgoto", delegate (ONScripterLabel this_) { return this_.cselgotoCommand(); } ),
		    new FuncLUT("cselbtn", delegate (ONScripterLabel this_) { return this_.cselbtnCommand(); } ),
		    new FuncLUT("csel", delegate (ONScripterLabel this_) { return this_.selectCommand(); } ),
		    new FuncLUT("click", delegate (ONScripterLabel this_) { return this_.clickCommand(); } ),
		    new FuncLUT("cl", delegate (ONScripterLabel this_) { return this_.clCommand(); } ),
		    new FuncLUT("chvol", delegate (ONScripterLabel this_) { return this_.chvolCommand(); } ),
		    new FuncLUT("checkpage", delegate (ONScripterLabel this_) { return this_.checkpageCommand(); } ),
		    new FuncLUT("checkkey", delegate (ONScripterLabel this_) { return this_.checkkeyCommand(); } ),
		    new FuncLUT("cellcheckspbtn", delegate (ONScripterLabel this_) { return this_.spbtnCommand(); } ),
		    new FuncLUT("cellcheckexbtn", delegate (ONScripterLabel this_) { return this_.exbtnCommand(); } ),
		    new FuncLUT("cell", delegate (ONScripterLabel this_) { return this_.cellCommand(); } ),
		    new FuncLUT("caption", delegate (ONScripterLabel this_) { return this_.captionCommand(); } ),
		    new FuncLUT("btnwait2", delegate (ONScripterLabel this_) { return this_.btnwaitCommand(); } ),
		    new FuncLUT("btnwait", delegate (ONScripterLabel this_) { return this_.btnwaitCommand(); } ),
		    new FuncLUT("btntime2", delegate (ONScripterLabel this_) { return this_.btntimeCommand(); } ),
		    new FuncLUT("btntime", delegate (ONScripterLabel this_) { return this_.btntimeCommand(); } ),
		    new FuncLUT("btndown",  delegate (ONScripterLabel this_) { return this_.btndownCommand(); } ),
		    new FuncLUT("btndef",  delegate (ONScripterLabel this_) { return this_.btndefCommand(); } ),
		    new FuncLUT("btnarea",  delegate (ONScripterLabel this_) { return this_.btnareaCommand(); } ),
		    new FuncLUT("btn",     delegate (ONScripterLabel this_) { return this_.btnCommand(); } ),
		    new FuncLUT("br",      delegate (ONScripterLabel this_) { return this_.brCommand(); } ),
		    new FuncLUT("blt",      delegate (ONScripterLabel this_) { return this_.bltCommand(); } ),
		    new FuncLUT("bgmvol", delegate (ONScripterLabel this_) { return this_.mp3volCommand(); } ),
		    new FuncLUT("bgmstop", delegate (ONScripterLabel this_) { return this_.mp3stopCommand(); } ),
		    new FuncLUT("bgmonce", delegate (ONScripterLabel this_) { return this_.mp3Command(); } ),
		    new FuncLUT("bgmfadeout", delegate (ONScripterLabel this_) { return this_.mp3fadeoutCommand(); } ),
		    new FuncLUT("bgmfadein", delegate (ONScripterLabel this_) { return this_.mp3fadeinCommand(); } ),
		    new FuncLUT("bgmdownmode", delegate (ONScripterLabel this_) { return this_.bgmdownmodeCommand(); } ),
		    new FuncLUT("bgm", delegate (ONScripterLabel this_) { return this_.mp3Command(); } ),
		    new FuncLUT("bgcpy",      delegate (ONScripterLabel this_) { return this_.bgcopyCommand(); } ),
		    new FuncLUT("bgcopy",      delegate (ONScripterLabel this_) { return this_.bgcopyCommand(); } ),
		    new FuncLUT("bg",      delegate (ONScripterLabel this_) { return this_.bgCommand(); } ),
		    new FuncLUT("barclear",      delegate (ONScripterLabel this_) { return this_.barclearCommand(); } ),
		    new FuncLUT("bar",      delegate (ONScripterLabel this_) { return this_.barCommand(); } ),
		    new FuncLUT("avi",      delegate (ONScripterLabel this_) { return this_.aviCommand(); } ),
		    new FuncLUT("automode_time",      delegate (ONScripterLabel this_) { return this_.automode_timeCommand(); } ),
		    new FuncLUT("autoclick",      delegate (ONScripterLabel this_) { return this_.autoclickCommand(); } ),
		    new FuncLUT("amsp2",      delegate (ONScripterLabel this_) { return this_.amspCommand(); } ),
		    new FuncLUT("amsp",      delegate (ONScripterLabel this_) { return this_.amspCommand(); } ),
		    new FuncLUT("allsp2resume",      delegate (ONScripterLabel this_) { return this_.allsp2resumeCommand(); } ),
		    new FuncLUT("allspresume",      delegate (ONScripterLabel this_) { return this_.allspresumeCommand(); } ),
		    new FuncLUT("allsp2hide",      delegate (ONScripterLabel this_) { return this_.allsp2hideCommand(); } ),
		    new FuncLUT("allsphide",      delegate (ONScripterLabel this_) { return this_.allsphideCommand(); } ),
		    new FuncLUT("abssetcursor", delegate (ONScripterLabel this_) { return this_.setcursorCommand(); } ),
		    new FuncLUT("", null),
		};
		
		public class FuncHash {
		    public int start;
		    public int end;
		    public FuncHash()
		    { start = (-1); end = (-2); }
		} 
		public static FuncHash[] func_hash = func_hash_init(); //= new FuncHash['z'-'a'+1];
		private static FuncHash[] func_hash_init() {
			FuncHash[] result = new FuncHash['z'-'a'+1];
			for (int i = 0; i < result.Length; ++i)
			{
				result[i] = new FuncHash();
			}
			return result;
		}
			
		private static void SDL_Quit_Wrapper()
		{
		    SDL_Quit();
		}
		
		public partial class ONScripterLabel {
			public void initSDL()
			{
			    /* ---------------------------------------- */
			    /* Initialize SDL */
			
			    if ( SDL_Init( SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_AUDIO ) < 0 ){
			        errorAndExit("Couldn't initialize SDL", SDL_GetError(), "Init Error", true);
			        return; //dummy
			    }
			    atexit(SDL_Quit_Wrapper); // work-around for OS/2
			
			    if( cdaudio_flag && SDL_InitSubSystem( SDL_INIT_CDROM ) < 0 ){
			        errorAndExit("Couldn't initialize CD-ROM", SDL_GetError(), "Init Error", true);
			        return; //dummy
			    }
			
			    SDL_EnableUNICODE(1);
			
			    /* ---------------------------------------- */
			    /* Initialize SDL */
			    if ( TTF_Init() < 0 ){
			        errorAndExit("can't initialize SDL TTF", null, "Init Error", true);
			        return; //dummy
			    }
			
			#if BPP16
			    screen_bpp = 16;
			#else
			    screen_bpp = 32;
			#endif
			
			#if PDA && PDA_WIDTH
			    screen_ratio1 *= PDA_WIDTH;
			    screen_ratio2 *= 320;
			    screen_width   = screen_width  * PDA_WIDTH / 320;
			    screen_height  = screen_height * PDA_WIDTH / 320;
			#elif PDA && PDA_AUTOSIZE
			    SDL_Rect **modes;
			    modes = SDL_ListModes(NULL, 0);
			    if (modes == (SDL_Rect **)0){
			        errorAndExit("No Video mode available.", NULL, "Init Error", true);
			        return; //dummy
			    }
			    else if (modes == (SDL_Rect **)-1){
			        // no restriction
			    }
			 	else{
			        int width;
			        if (modes[0]->w * 3 > modes[0]->h * 4)
			            width = (modes[0]->h / 3) * 4;
			        else
			            width = (modes[0]->w / 4) * 4;
			        screen_ratio1 *= width;
			        screen_ratio2 *= 320;
			        screen_width   = screen_width  * width / 320;
			        screen_height  = screen_height * width / 320;
			    }
			#endif
			
			#if RCA_SCALE
			    scr_stretch_x = 1.0;
			    scr_stretch_y = 1.0;
			#endif
			    if (scaled_flag) {
			        SDL_VideoInfo info = SDL_GetVideoInfo();
			        int native_width = info.current_w;
			        int native_height = info.current_h;
			        
			        // Resize up to fill screen
			#if !RCA_SCALE
			        float scr_stretch_x, scr_stretch_y;
			#endif
			        scr_stretch_x = (float)native_width / (float)screen_width;
			        scr_stretch_y = (float)native_height / (float)screen_height;
			#if RCA_SCALE
			        if (widescreen_flag) {
			            if (scr_stretch_x > scr_stretch_y) {
			                screen_ratio1 = native_height;
			                screen_ratio2 = script_height;
			                scr_stretch_x /= scr_stretch_y;
			                scr_stretch_y = 1.0;
			            } else { 
			                screen_ratio1 = native_width;
			                screen_ratio2 = script_width;
			                scr_stretch_y /= scr_stretch_x;
			                scr_stretch_x = 1.0;
			            }
			            screen_width  = StretchPosX(script_width);
			            screen_height = StretchPosY(script_height);
			        } else
			#endif
			        {
			            // Constrain aspect to same as game
			            if (scr_stretch_x > scr_stretch_y) {
			                screen_ratio1 = native_height;
			                screen_ratio2 = script_height;
			            } else { 
			                screen_ratio1 = native_width;
			                screen_ratio2 = script_width;
			            }
			            scr_stretch_x = scr_stretch_y = (float)1.0;
			            screen_width  = ExpandPos(script_width);
			            screen_height = ExpandPos(script_height);
			        }
			    }
			#if RCA_SCALE
			    else if (widescreen_flag) {
			        const SDL_VideoInfo* info = SDL_GetVideoInfo();
			        int native_width = info->current_w;
			        int native_height = info->current_h;
			        
			        // Resize to screen aspect ratio
			        const float screen_asp = (float)screen_width / (float)screen_height;
			        const float native_asp = (float)native_width / (float)native_height;
			        const float aspquot = native_asp / screen_asp;
			        if (aspquot >1.01) {
			            // Widescreen; make gamearea wider
			            scr_stretch_x = (float)screen_height * native_asp / (float)screen_width;
			            screen_width = screen_height * native_asp;
			        } else if (aspquot < 0.99) {
			            scr_stretch_y = (float)screen_width / native_asp / (float)screen_height;
			            screen_height = screen_width / native_asp;
			        }
			    }
			#endif
			    screen_surface = SDL_SetVideoMode( screen_width, screen_height, screen_bpp, DEFAULT_VIDEO_SURFACE_FLAG );
			
			    /* ---------------------------------------- */
			    /* Check if VGA screen is available. */
			#if PDA && PDA_WIDTH//==640
			    if ( screen_surface == NULL ){
			        screen_ratio1 /= 2;
			        screen_width  /= 2;
			        screen_height /= 2;
			        screen_surface = SDL_SetVideoMode( screen_width, screen_height, screen_bpp, DEFAULT_VIDEO_SURFACE_FLAG );
			    }
			#endif
			
			    if ( screen_surface == null ) {
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                 "Couldn't set %dx%dx%d video mode",
			                 screen_width, screen_height, screen_bpp);
			        errorAndExit(script_h.errbuf, SDL_GetError(), "Init Error", true);
			        return; //dummy
			    }
			    //printf("Display: %d x %d (%d bpp)\n", screen_width, screen_height, screen_bpp);
			    dirty_rect.setDimension(screen_width, screen_height);
			#if true//defined(WIN32)
			#else
			    initSJIS2UTF16();
			#endif
			
			    setStr(ref wm_title_string, DEFAULT_WM_TITLE);
			    setStr(ref wm_icon_string, DEFAULT_WM_ICON);
			    SDL_WM_SetCaption( wm_title_string, wm_icon_string );
			}
			
			public int ExpandPos(int val) {
				return (int)((float)(val * screen_ratio1) / screen_ratio2 + 0.5);
			}
			
			public int ContractPos(int val) {
				return (int)((float)(val * screen_ratio2) / screen_ratio1 + 0.5);
			}
			#if true//RCA_SCALE
			public int StretchPosX(int val) {
				return (int)((float)(val * screen_ratio1) * scr_stretch_x / screen_ratio2 + 0.5);
			}
			public int StretchPosY(int val) {
				return (int)((val * screen_ratio1) * scr_stretch_y / screen_ratio2 + 0.5);
			}
			#endif
			
			public ONScripterLabel()
			//Using an initialization list to make sure pointers start out NULL
			{ default_font = (null); registry_file = (null); dll_file = (null);
			  getret_str = (null); key_exe_file = (null); trap_dest = (null);
			  wm_title_string = (null); wm_icon_string = (null);
			  accumulation_surface = (null); backup_surface = (null);
			  effect_dst_surface = (null); effect_src_surface = (null); effect_tmp_surface = (null);
			  screenshot_surface = (null); image_surface = (null); tmp_image_buf = (null);
			  sprite_info = (null); sprite2_info = (null);
			  font_file = (null); root_glyph_cache = (null);
			  string_buffer_breaks = (null); string_buffer_margins = (null);
			  sin_table = (null); cos_table = (null); whirl_table = (null);
			  breakup_cells = (null); breakup_cellforms = (null); breakup_mask = (null);
			  shelter_select_link = (null); default_cdrom_drive = (null);
			  wave_file_name = (null); seqmusic_file_name = (null); 
			  music_file_name = (null); music_buffer = (null);
			  music_cmd = (null); seqmusic_cmd = (null);
			  movie_buffer = (null); async_movie_surface = (null);
			  surround_rects = (null);
			  text_font = (null); cached_page = (null); system_menu_title = (null);
			  scaled_flag = (false);
		
			    //first initialize *everything* (static) to base values
			
			    resetFlags();
			    resetFlagsSub();
			
			    //init envdata variables
			    fullscreen_mode = false;
			    volume_on_flag = true;
			    text_speed_no = 1;
			    cdaudio_on_flag = false;
			
			    //init onscripter-specific variables
			    skip_past_newline = false;
			    cdaudio_flag = false;
			    enable_wheeldown_advance_flag = false;
			    disable_rescale_flag = false;
			    edit_flag = false;
			#if RCA_SCALE
			    widescreen_flag = false;
			    scaled_flag = false;
			#endif
			    window_mode = false;
			    use_app_icons = false;
			    cdrom_drive_number = 0;
			#if true// WIN32
			    current_user_appdata = false;
			#endif
			
			    //init various internal variables
			    audio_open_flag = false;
			    getret_int = 0;
			    variable_edit_index = variable_edit_num = variable_edit_sign = 0;
			    tmp_image_buf_length = mean_size_of_loaded_images = 0;
			    num_loaded_images = 1; //avoid possible div by zero
			    effect_counter = effect_timer_resolution = 0;
			    effect_start_time = effect_start_time_old = 0;
			    effect_duration = 1; //avoid possible div by zero
			    effect_tmp = 0;
			    skip_effect = in_effect_blank = false;
			    effectspeed = EFFECTSPEED_NORMAL;
			    shortcut_mouse_line = -1;
			    skip_mode = SKIP_NONE;
			    music_buffer_length = 0;
			    mp3fade_start = 0;
			    wm_edit_string[0] = '\0';
			#if PNG_AUTODETECT_NSCRIPTER_MASKS
			    png_mask_type = PNG_MASK_AUTODETECT;
			#elif PNG_FORCE_NSCRIPTER_MASKS
			    png_mask_type = PNG_MASK_USE_NSCRIPTER;
			#else
			    png_mask_type = PNG_MASK_USE_ALPHA;
			#endif
			    
			    //init arrays
			    int i=0;
			    for (i=0 ; i<MAX_PARAM_NUM ; i++) bar_info[i] = prnum_info[i] = null;
			    last_textpos_xy[0] = last_textpos_xy[1] = 0;
			    loop_bgm_name[0] = loop_bgm_name[1] = null;
			    for ( i=0 ; i<ONS_MIX_CHANNELS ; i++ )
			        channelvolumes[i] = DEFAULT_VOLUME;
			
			    fileversion = SAVEFILE_VERSION_MAJOR*100 + SAVEFILE_VERSION_MINOR;
			
			    internal_timer = SDL_GetTicks();
			
			    //setting this to let script_h call error message popup routines
			    script_h.setOns(this);
			    
			#if USE_X86_GFX && !MACOSX
			    // determine what functions the cpu supports (Mion)
			    {
			        unsigned int func, eax, ebx, ecx, edx;
			        func = AnimationInfo::CPUF_NONE;
			        if (__get_cpuid(1, &eax, &ebx, &ecx, &edx) != 0) {
			            printf("System info: Intel CPU, with functions: ");
			            if (edx & bit_MMX) {
			                func |= AnimationInfo::CPUF_X86_MMX;
			                printf("MMX ");
			            }
			            if (edx & bit_SSE) {
			                func |= AnimationInfo::CPUF_X86_SSE;
			                printf("SSE ");
			            }
			            if (edx & bit_SSE2) {
			                func |= AnimationInfo::CPUF_X86_SSE2;
			                printf("SSE2 ");
			            }
			            printf("\n");
			        }
			        AnimationInfo::setCpufuncs(func);
			    }
			#elif USE_X86_GFX && MACOSX
			    // x86 CPU on Mac OS X all support SSE2
			    AnimationInfo::setCpufuncs(AnimationInfo::CPUF_X86_SSE2);
			    printf("System info: Intel CPU with SSE2 functionality\n");
			#elif USE_PPC_GFX && MACOSX
			    // Determine if this PPC CPU supports AltiVec (Roto)
			    {
			        unsigned int func = AnimationInfo::CPUF_NONE;
			        int altivec_present = 0;
			    
			        size_t length = sizeof(altivec_present);
			        int error = sysctlbyname("hw.optional.altivec", &altivec_present, &length, NULL, 0);
			        if(error) {
			            AnimationInfo::setCpufuncs(AnimationInfo::CPUF_NONE);
			            return;
			        }
			        if(altivec_present) {
			            func |= AnimationInfo::CPUF_PPC_ALTIVEC;
			            printf("System info: PowerPC CPU, supports altivec\n");
			        } else {
			            printf("System info: PowerPC CPU, DOES NOT support altivec\n");
			        }
			        AnimationInfo::setCpufuncs(func);
			    }
			#else
			    AnimationInfo.setCpufuncs(AnimationInfo.CPUF_NONE);
			#endif
			
			    //since we've made it this far, let's init some dynamic variables
			    setStr( ref registry_file, REGISTRY_FILE );
			    setStr( ref dll_file, DLL_FILE );
			    readColor( ref linkcolor[0], "#FFFF22" ); // yellow - link color
			    readColor( ref linkcolor[1], "#88FF88" ); // cyan - mouseover link color
			    sprite_info  = new AnimationInfo[MAX_SPRITE_NUM];
			    for (int i_ = 0; i_ < sprite_info.Length; ++i_)
			    {
			    	sprite_info[i_] = new AnimationInfo();
			    }
			    sprite2_info = new AnimationInfo[MAX_SPRITE2_NUM];
				for (int i_ = 0; i_ < sprite2_info.Length; ++i_)
			    {
			    	sprite2_info[i_] = new AnimationInfo();
			    }
				
			    for (i=0 ; i<MAX_SPRITE2_NUM ; i++)
			        sprite2_info[i].affine_flag = true;
			    for (i=0 ; i<NUM_GLYPH_CACHE ; i++){
			        if (i != NUM_GLYPH_CACHE-1) glyph_cache[i].next = glyph_cache[i+1];
			    }
			    glyph_cache[NUM_GLYPH_CACHE-1].next = null;
			    root_glyph_cache = glyph_cache[0];
			
			    // External Players
			    music_cmd = getenv("PLAYER_CMD");
			    seqmusic_cmd  = getenv("MUSIC_CMD");
			}
			
			~ONScripterLabel()
			{
			    reset();
			
			    sprite_info = null;//delete[] sprite_info;
			    sprite2_info = null;//delete[] sprite2_info;
			
			    if (null!=default_font) default_font = null;//delete[] default_font;
			    if (null!=font_file) font_file = null;//delete[] font_file;
			}
			
			public void enableCDAudio(){
			    cdaudio_flag = true;
			}
			
			public void setCDNumber(int cdrom_drive_number)
			{
			    this.cdrom_drive_number = cdrom_drive_number;
			}
			
			public void setFontFile(CharPtr filename)
			{
			    setStr(ref default_font, filename);
			}
			
			public void setRegistryFile(CharPtr filename)
			{
			    setStr(ref registry_file, filename);
			}
			
			public void setDLLFile(CharPtr filename)
			{
			    setStr(ref dll_file, filename);
			}
			
			public void setFileVersion(CharPtr ver)
			{
			    int verno = atoi(ver);
			    if ((verno >= 199) && (verno <= fileversion))
			        fileversion = verno;
			}
			
			public void setFullscreenMode()
			{
			    fullscreen_mode = true;
			}
			
			public void setWindowMode()
			{
			    window_mode = true;
			}
			
			#if true//def WIN32
			public void setUserAppData()
			{
			    current_user_appdata = true;
			}
			#endif
			
			public void setUseAppIcons()
			{
			    use_app_icons = true;
			}
			
			public void setIgnoreTextgosubNewline()
			{
			    script_h.ignore_textgosub_newline = true;
			}
			
			public void setSkipPastNewline()
			{
			    skip_past_newline = true;
			}
			
			public void setPreferredWidth(CharPtr widthstr)
			{
			    int width = atoi(widthstr);
			    //minimum preferred window width of 160 (gets ridiculous if smaller)
			    if (width > 160)
			        preferred_width = width;
			    else if (width > 0)
			        preferred_width = 160;
			}
			
			public void enableButtonShortCut()
			{
			    force_button_shortcut_flag = true;
			}
			
			public void enableWheelDownAdvance()
			{
			    enable_wheeldown_advance_flag = true;
			}
			
			public void disableCpuGfx()
			{
			    AnimationInfo.setCpufuncs(AnimationInfo.CPUF_NONE);
			}
			
			public void disableRescale()
			{
			    disable_rescale_flag = true;
			}
			
			public void enableEdit()
			{
			    edit_flag = true;
			}
			
			public void setKeyEXE(CharPtr filename)
			{
			    setStr(ref key_exe_file, filename);
			}
			
			#if RCA_SCALE
			public void setWidescreen()
			{
			    widescreen_flag = true;
			}
			#endif
			
			public void setScaled()
			{
			    scaled_flag = true;
			}
			
			public void setGameIdentifier(CharPtr gameid)
			{
			    setStr(ref cmdline_game_id, gameid);
			}
			
			public int init()
			{
				if (archive_path.get_num_paths() == 0) {
			    
			        //default archive_path is current directory ".", followed by parent ".."
			        DirPaths default_path = new DirPaths(".");
			        default_path.add("..");
			#if MACOSX
			        // On Mac OS X, store archives etc in the application bundle by default,
			        // but also check the application root directory and current directory.
			        if (isBundled()) {
			            archive_path.add(bundleResPath());
			
			            // Now add the application path.
			            char *path = bundleAppPath();
			            if (path) {
			                archive_path.add(path);
			                // add the next directory up as a fallback.
			                char tmp[strlen(path) + 4];
			                sprintf(tmp, "%s%c%s", path, DELIMITER, "..");
			                archive_path.add(tmp);
			            } else {
			                //if we couldn't find the application path, we still need
			                //something - use current dir and parent (default)
			                archive_path.add(default_path);
			            }
			        }
			        else {
			            // Not in a bundle: just use current dir and parent as normal.
			            archive_path.add(default_path);
			        }
			#else
			        // On Linux, the path is unpredictable and should be set by
			        // using "-r PATH" or "--root PATH" in a launcher script.
			        // On other platforms it's the same place as the executable.
			        archive_path.add(default_path);
			        //printf("init:archive_paths: \"%s\"\n", archive_path->get_all_paths());
			#endif
			    }
			    
			    if (null!=key_exe_file){
			        createKeyTable( key_exe_file );
			        script_h.setKeyTable( key_table );
			    }
			    
			    if ( 0!=open() ) return -1;
			
			    if ( script_h.save_path == null ){
			    	CharPtr gameid = ((script_h.game_identifier != null) ? new CharPtr(script_h.game_identifier) : null);
			        CharPtr gamename = new char[20];
			        if (null==gameid) {
			        	gameid=new CharPtr(gamename);
			            snprintf(gameid, 20, "ONScripter-%x", script_h.game_hash);
			        }
			#if true// WIN32
			        // On Windows, store in [Profiles]/All Users/Application Data.
			        // Permit saves to be per-user rather than shared if
			        // option --current-user-appdata is specified
//			        HMODULE shdll = LoadLibrary("shfolder");
//			        if (shdll) {
//			            GETFOLDERPATH gfp = GETFOLDERPATH(GetProcAddress(shdll, "SHGetFolderPathA"));
//			            if (gfp) {
			                CharPtr hpath = new char[MAX_PATH];
			//#define CSIDL_COMMON_APPDATA 0x0023 // for [Profiles]/All Users/Application Data
			//#define CSIDL_APPDATA 0x001A // for [Profiles]/[User]/Application Data
			                long res;
			                if (current_user_appdata)
			                    res = SHGetFolderPathA(0, CSIDL_APPDATA, 0, 0, hpath);
			                else
			                    res = SHGetFolderPathA(0, CSIDL_COMMON_APPDATA, 0, 0, hpath);
			                if (res != S_FALSE && res != E_FAIL && res != E_INVALIDARG) {
			                    script_h.save_path = new char[strlen(hpath) + strlen(gameid) + 3];
			                    sprintf(script_h.save_path, "%s%c%s%c",
			                            hpath, DELIMITER, gameid, DELIMITER);
			                    CreateDirectory(script_h.save_path, 0);
			                }
//			            }
//			            FreeLibrary(shdll);
//			        }
			        if (script_h.save_path == null) {
			            // Error; assume ancient Windows. In this case it's safe
			            // to use the archive path!
			            setSavePath(archive_path.get_path(0));
			        }
			#elif MACOSX
			        // On Mac OS X, place in ~/Library/Application Support/<gameid>/
			        char *path;
			        ONSCocoa::getGameAppSupportPath(&path, gameid);
			        setSavePath(path);
			        delete[] path;
			#elif LINUX
			        // On Linux (and similar *nixen), place in ~/.gameid
			        passwd* pwd = getpwuid(getuid());
			        if (pwd) {
			            script_h.save_path = new char[strlen(pwd->pw_dir) + strlen(gameid) + 4];
			            sprintf(script_h.save_path, "%s%c.%s%c", 
			                    pwd->pw_dir, DELIMITER, gameid, DELIMITER);
			            mkdir(script_h.save_path, 0755);
			        }
			        else setSavePath(archive_path.get_path(0));
			#else
			        // Fall back on default ONScripter behaviour if we don't have
			        // any better ideas.
			        setSavePath(archive_path.get_path(0));
			#endif
			    }
			    if ( null!=script_h.game_identifier ) {
			        //delete[] script_h.game_identifier; 
			        script_h.game_identifier = null; 
			    }
			
			    if (strcmp(script_h.save_path, archive_path.get_path(0)) != 0) {
			        // insert save_path onto the front of archive_path
			        DirPaths new_path = new DirPaths(script_h.save_path);
			        new_path.add(archive_path);
			        archive_path = new_path;
			    }
			
			#if USE_LUA
			    lua_handler.init(this, &script_h);
			#endif
			
			    //initialize cmd function table hash
			    int idx = 0;
			    while (null!=func_lut[idx].method){
			        int j = func_lut[idx].command[0]-'a';
			        if (func_hash[j].start == -1) func_hash[j].start = idx;
			        func_hash[j].end = idx;
			        idx++;
			    }
			
			#if true// WIN32
			    if (debug_level > 0) {
			        openDebugFolders();
			    }
			#endif
			
			    initSDL();
			
			    image_surface = SDL_CreateRGBSurface( SDL_SWSURFACE, 1, 1, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000 );
			
			    accumulation_surface = AnimationInfo.allocSurface( screen_width, screen_height );
			    backup_surface       = AnimationInfo.allocSurface( screen_width, screen_height );
			    effect_src_surface   = AnimationInfo.allocSurface( screen_width, screen_height );
			    effect_dst_surface   = AnimationInfo.allocSurface( screen_width, screen_height );
			    effect_tmp_surface   = AnimationInfo.allocSurface( screen_width, screen_height );
			    SDL_SetAlpha( accumulation_surface, 0, SDL_ALPHA_OPAQUE );
			    SDL_SetAlpha( backup_surface, 0, SDL_ALPHA_OPAQUE );
			    SDL_SetAlpha( effect_src_surface, 0, SDL_ALPHA_OPAQUE );
			    SDL_SetAlpha( effect_dst_surface, 0, SDL_ALPHA_OPAQUE );
			    SDL_SetAlpha( effect_tmp_surface, 0, SDL_ALPHA_OPAQUE );
			
			    num_loaded_images = 10; // to suppress temporal increase at the start-up
			
			    text_info.num_of_cells = 1;
			    text_info.allocImage( screen_width, screen_height );
			    text_info.fill(0, 0, 0, 0);
			
			    // ----------------------------------------
			    // Initialize font
			    font_file = null;//delete[] font_file;
			    if ( null!=default_font ){
			        font_file = new char[ strlen(default_font) + 1 ];
			        sprintf( font_file, "%s", default_font );
			    }
			    else{
			        FILEPtr fp;
			        font_file = new char[ archive_path.max_path_len() + strlen(FONT_FILE) + 1 ];
			        for (int i=0; i<(archive_path.get_num_paths()); i++) {
			            // look through archive_path(s) for the font file
			            sprintf( font_file, "%s%s", archive_path.get_path(i), FONT_FILE );
			            //printf("font file: %s\n", font_file);
			#if true//_MSC_VER <= 1200
			            fp = fopen(font_file, "rb");
			#else
			            fp = std::fopen(font_file, "rb");
			#endif
			            if (fp != null) {
			                fclose(fp);
			                break;
			            }
			        }
			        //sprintf( font_file, "%s%s", archive_path->get_path(0), FONT_FILE );
			        setStr(ref default_font, FONT_FILE);
			    }
			
			    // ----------------------------------------
			    // Sound related variables
			    //this.cdaudio_flag = cdaudio_flag; //FIXME:nosense
			
			    // ----------------------------------------
			    // Initialize misc variables
			
			    internal_timer = SDL_GetTicks();
			
			    loadEnvData();
			
			    ScriptHandler.LanguageScript cur_pref = script_h.preferred_script;
			    defineresetCommand();
			    readToken();
			
			    if ( sentence_font.openFont( font_file, screen_ratio1, screen_ratio2 ) == null ){
			#if MACOSX
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN, "Could not find the font file '%s'.\n"
			                 "Please ensure it is present with the game data.", default_font);
			#else
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN, "Could not find the font file '%s'.", default_font);
			#endif
			        errorAndExit(script_h.errbuf, null, "Missing font file", true);
			        return -1;
			    }
			
			    //Do a little check for whether the font supports Japanese glyphs,
			    //if either system-menu or preferred text mode is Japanese
			    if ( (script_h.system_menu_script == ScriptHandler.LanguageScript.JAPANESE_SCRIPT) ||
			         (cur_pref == ScriptHandler.LanguageScript.JAPANESE_SCRIPT) ) {
			        if (debug_level > 0)
			            printf("Checking font for Japanese support\n");
			        UInt16 test = 0x300c; // Unicode JP start-quote
			        int error = 0, minx1 = 0, maxx1 = 0, miny1 = 0, maxy1 = 0;
			        int minx2 = 0, maxx2 = 0, miny2 = 0, maxy2 = 0;
			        int minx3 = 0, maxx3 = 0, miny3 = 0, maxy3 = 0;
			        int null_temp = 0;
			        error = TTF_GlyphMetrics(TTF_Font.fromUnsignedCharPtr(sentence_font.ttf_font), test,
			                                 ref minx1, ref maxx1, ref miny1, ref maxy1, ref null_temp);
			        if (debug_level > 0)
			            printf("JP start-quote glyph metrics: x=(%d,%d), y=(%d,%d)\n",
			                   minx1, maxx1, miny1, maxy1);
			        test = 0x300d; // Unicode JP end-quote
			        error = TTF_GlyphMetrics(TTF_Font.fromUnsignedCharPtr(sentence_font.ttf_font), test,
			                                     ref minx2, ref maxx2, ref miny2, ref maxy2, ref null_temp);
			        if (debug_level > 0)
			            printf("JP end-quote glyph metrics: x=(%d,%d), y=(%d,%d)\n",
			                   minx2, maxx2, miny2, maxy2);
			        test = 0x3042; // Unicode Hiragana letter A
			        error = TTF_GlyphMetrics(TTF_Font.fromUnsignedCharPtr(sentence_font.ttf_font), test,
			                                     ref minx3, ref maxx3, ref miny3, ref maxy3, ref null_temp);
			        if (debug_level > 0)
			            printf("JP hiragana A glyph metrics: x=(%d,%d), y=(%d,%d)\n",
			                   minx3, maxx3, miny3, maxy3);
			        if (error != 0) {
			            // font doesn't have the glyph, so set to use English mode default
			            setEnglishDefault();
			            setEnglishMenu();
			            setDefaultMenuLabels();
			            printf("Font file doesn't support Japanese; reverting to English\n");
			        } else if ((minx1 == minx2) && (maxx1 == maxx2) && (miny1 == miny2) && 
			                   (maxy1 == maxy2) && (minx1 == minx3) && (maxx1 == maxx3) &&
			                   (miny1 == miny3) && (maxy1 == maxy3)) {
			            // font has equal metrics for quotes, so assume glyphs are both null
			            setEnglishDefault();
			            setEnglishMenu();
			            setDefaultMenuLabels();
			            printf("Font file has equivalent metrics for 3 Japanese glyphs - ");
			            printf("assuming it doesn't support Japanese; reverting to English\n");
			        } else if (debug_level > 0) {
			            printf("Ok, the font appears to support Japanese\n");
			        }
			    }
			
			    // preferred was set by command-line option if it was set, so
			    // make it the default from now on; default to English otherwise
			    if (script_h.default_script == ScriptHandler.LanguageScript.NO_SCRIPT_PREF) {
			        if (cur_pref == ScriptHandler.LanguageScript.NO_SCRIPT_PREF)
			            script_h.preferred_script = ScriptHandler.LanguageScript.LATIN_SCRIPT;
			        else
			            script_h.preferred_script = cur_pref;
			        script_h.default_script = script_h.preferred_script;
			    }
			    if (script_h.system_menu_script == ScriptHandler.LanguageScript.NO_SCRIPT_PREF) {
			        setEnglishMenu();
			        setDefaultMenuLabels();
			    }
			
			    return 0;
			}
			
			public /*override*/new void reset() //FIXME:not override!!!!
			{
			    resetFlags();
			
			    if (null!=sin_table) sin_table = null;//delete[] sin_table;
			    if (null!=cos_table) cos_table = null;//delete[] cos_table;
			    sin_table = cos_table = null;
			    if (null!=whirl_table) whirl_table = null;//delete[] whirl_table;
			    whirl_table = null;
			
			    if (null!=breakup_cells) breakup_cells = null;//delete[] breakup_cells;
			    if (null!=breakup_mask) breakup_mask = null;//delete[] breakup_mask;
			    if (null!=breakup_cellforms) breakup_cellforms = null;//delete[] breakup_cellforms;
			
			    AnimationInfo.resetResizeBuffer();
			
			    if (null!=string_buffer_breaks) string_buffer_breaks = null;//delete[] string_buffer_breaks;
			    string_buffer_breaks = null;
			
			    resetSentenceFont();
			
			    setStr(ref getret_str, null);
			    getret_int = 0;
			
			    if (null!=movie_buffer) movie_buffer = null;//delete[] movie_buffer;
			    movie_buffer = null;
			    if (null!=surround_rects) surround_rects = null;//delete[] surround_rects;
			    surround_rects = null;
			
			    resetSub();
			
			    /* ---------------------------------------- */
			    /* Load global variables if available */
			    if ( loadFileIOBuf( "gloval.sav" ) == 0 ||
			         loadFileIOBuf( "global.sav" ) == 0 )
			        readVariables( script_h.global_variable_border, VARIABLE_RANGE );
			
			}
			
			public void resetSub()
			{
			    int i;
			
			    for ( i=0 ; i<script_h.global_variable_border ; i++ )
			        script_h.getVariableData(i).reset(false);
			
			    resetFlagsSub();
			
			    skip_mode = 0!=(skip_mode & SKIP_TO_EOP) ? SKIP_TO_EOP : SKIP_NONE;
			    setStr(ref trap_dest, null);
			
			    resetSentenceFont();
			
			    deleteNestInfo();
			    deleteButtonLink();
			    deleteSelectLink();
			
			    stopCommand();
			    loopbgmstopCommand();
			    stopAllDWAVE();
			    setStr(ref loop_bgm_name[1], null);
			
			    // ----------------------------------------
			    // reset AnimationInfo
			    btndef_info.reset();
			    bg_info.reset();
			    setStr( ref bg_info.file_name, "black" );
			    createBackground();
			    for (i=0 ; i<3 ; i++) tachi_info[i].reset();
			    for (i=0 ; i<MAX_SPRITE_NUM ; i++) sprite_info[i].reset();
			    for (i=0 ; i<MAX_SPRITE2_NUM ; i++) sprite2_info[i].reset();
			    barclearCommand();
			    prnumclearCommand();
			    for (i=0 ; i<2 ; i++) cursor_info[i].reset();
			    for (i=0 ; i<4 ; i++) lookback_info[i].reset();
			
			    //Mion: reset textbtn
			    deleteTextButtonInfo();
			    readColor( ref linkcolor[0], "#FFFF22" ); // yellow - link color
			    readColor( ref linkcolor[1], "#88FF88" ); // cyan - mouseover link color
			
			    dirty_rect.fill( screen_width, screen_height );
			}
			
			public void resetFlags()
			{
			    automode_flag = false;
			    automode_time = 3000;
			    autoclick_time = 0;
			    btntime2_flag = false;
			    btntime_value = 0;
			    btnwait_time = 0;
			    
			    is_exbtn_enabled = false;
			
			    system_menu_enter_flag = false;
			    system_menu_mode = SYSTEM_NULL;
			    key_pressed_flag = false;
			    shift_pressed_status = 0;
			    ctrl_pressed_status = 0;
			#if MACOSX
			    apple_pressed_status = 0;
			#endif
			    display_mode = shelter_display_mode = DISPLAY_MODE_NORMAL;
			    event_mode = shelter_event_mode = IDLE_EVENT_MODE;
			    did_leavetext = false;
			    in_effect_blank = false;
			    skip_effect = false;
			    effectskip_flag = true; //on by default
			
			    current_over_button = 0;
			    current_button_valid = false;
			    variable_edit_mode = NOT_EDIT_MODE;
			
			    new_line_skip_flag = false;
			    text_on_flag = true;
			    draw_cursor_flag = shelter_draw_cursor_flag = false;
			
			    // ----------------------------------------
			    // Sound related variables
			
			    wave_play_loop_flag = false;
			    seqmusic_play_loop_flag = false;
			    music_play_loop_flag = false;
			    cd_play_loop_flag = false;
			    mp3save_flag = false;
			    current_cd_track = -1;
			    mp3fadeout_duration = mp3fadein_duration = 0;
			    bgmdownmode_flag = false;
			
			    movie_click_flag = movie_loop_flag = false;
			
			    disableGetButtonFlag();
			}
			
			public void resetFlagsSub()
			{
			    int i=0;
			    
			    for ( i=0 ; i<3 ; i++ ) human_order[i] = 2-i; // "rcl"
			
			    all_sprite_hide_flag = false;
			    all_sprite2_hide_flag = false;
			
			    refresh_window_text_mode = REFRESH_NORMAL_MODE | REFRESH_WINDOW_MODE |
			                               REFRESH_TEXT_MODE;
			    erase_text_window_mode = 1;
			
			    monocro_flag = false;
			    monocro_color[0] = monocro_color[1] = monocro_color[2] = 0;
			    nega_mode = 0;
			    clickstr_state = CLICK_NONE;
			    trap_mode = TRAP_NONE;
			
			    last_keypress = KEYPRESS_NULL;
			
			    saveon_flag = true;
			    internal_saveon_flag = true;
			
			    textgosub_clickstr_state = CLICK_NONE;
			    indent_offset = 0;
			    line_enter_status = 0;
			    page_enter_status = 0;
			    last_textpos_xy[0] = last_textpos_xy[1] = 0;
			    line_has_nonspace = false;
			
			    for (i=0 ; i<SPRITE_NUM_LAST_LOADS ; i++) last_loaded_sprite[i] = -1;
			    last_loaded_sprite_ind = 0;
			
			    txtbtn_start_num = next_txtbtn_num = 1;
			    in_txtbtn = false;
			    txtbtn_show = false;
			    txtbtn_visible = false;
			}
			
			public void resetSentenceFont()
			{
			    sentence_font.reset();
			    sentence_font.font_size_xy[0] = DEFAULT_FONT_SIZE;
			    sentence_font.font_size_xy[1] = DEFAULT_FONT_SIZE;
			    sentence_font.top_xy[0] = 21;
			    sentence_font.top_xy[1] = 16;// + sentence_font.font_size;
			    sentence_font.num_xy[0] = 23;
			    sentence_font.num_xy[1] = 16;
			    sentence_font.pitch_xy[0] = sentence_font.font_size_xy[0];
			    sentence_font.pitch_xy[1] = 2 + sentence_font.font_size_xy[1];
			    sentence_font.wait_time = 20;
			    sentence_font.window_color[0] = sentence_font.window_color[1] = sentence_font.window_color[2] = 0x99;
			    sentence_font.color[0] = sentence_font.color[1] = sentence_font.color[2] = 0xff;
			    sentence_font_info.reset();
			    sentence_font_info.pos.x = 0;
			    sentence_font_info.pos.y = 0;
			    sentence_font_info.pos.w = screen_width;
			    sentence_font_info.pos.h = screen_height;
			}
			
			public bool doErrorBox( CharPtr title, CharPtr errstr, bool is_simple, bool is_warning )
			//returns true if we need to exit
			{
				//The OS X dialog box routines are crashing when in fullscreen mode,
			    //so let's switch to windowed mode just in case
			    menu_windowCommand();
			
			#if MACOSX
			    if (is_simple && !is_warning)
			        ONSCocoa::alertbox(title, errstr);
			    else {
			        if (ONSCocoa::scriptErrorBox(title, errstr, is_warning, ONSCocoa::ENC_SJIS) == SCRIPTERROR_IGNORE)
			            return false;
			    }
			
			#elif WIN32 && USE_MESSAGEBOX
			    char errtitle[256];
			    HWND pwin = NULL;
			    SDL_SysWMinfo info;
			    UINT mb_type = MB_OK;
			    SDL_VERSION(&info.version);
			    SDL_GetWMInfo(&info);
			
			    if (SDL_GetWMInfo(&info) == 1) {
			        pwin = info.window;
			        snprintf(errtitle, 256, "%s", title);
			    } else {
			        snprintf(errtitle, 256, "ONScripter-EN: %s", title);
			    }
			
			    if (is_warning) {
			        //Retry and Ignore both continue, Abort exits
			        //would rather do an Ignore/Exit button set, oh well
			        mb_type = MB_ABORTRETRYIGNORE|MB_DEFBUTTON3|MB_ICONWARNING;
			    }
			    else
			        mb_type |= MB_ICONERROR;
			    int res = MessageBox(pwin, errstr, errtitle, mb_type);
			    if (is_warning)
			        return (res == IDABORT); //should do exit if got Abort
			#else
			    //no errorbox support; at least send the info to stderr
			    fprintf(stderr, " ***[Info] %s *** \n%s\n", title, errstr);
			#endif
			
			    // get affairs in order
			    if (errorsave) {
			        saveon_flag = internal_saveon_flag = true;
			        saveSaveFile( 999 ); //save current game state to save999.dat
			    }
			#if true //WIN32
			    openDebugFolders();
			#endif
			
			    quit();
			
			    return true; //should do exit
			}
			
//			#ifdef WIN32
			public void openDebugFolders()
			{
			    // to make it easier to debug user issues on Windows, open
			    // the current directory, save_path and ONScripter output folders
			    // in Explorer
//			    HMODULE shdll = LoadLibrary("shell32");
//			    if (shdll) {
			        CharPtr hpath = new char[MAX_PATH];
			        bool havefp = false;
//			        GETFOLDERPATH gfp = GETFOLDERPATH(GetProcAddress(shdll, "SHGetFolderPathA"));
//			        if (gfp) {
			            long res = SHGetFolderPathA(0, CSIDL_APPDATA, 0, 0, hpath); //now user-based
			            if (res != S_FALSE && res != E_FAIL && res != E_INVALIDARG) {
			                havefp = true;
			                sprintf(new CharPtr(hpath, + (int)(strlen(hpath))), "%c%s",
			                        DELIMITER, "ONScripter-EN");
			            }
//			        }
//			        typedef HINSTANCE (WINAPI *SHELLEXECUTE)(HWND, LPCSTR, LPCSTR,
//			                           LPCSTR, LPCSTR, int);
//			        SHELLEXECUTE shexec =
//			            SHELLEXECUTE(GetProcAddress(shdll, "ShellExecuteA"));
//			        if (shexec) {
			            ShellExecuteA(null, "open", "", null, null, SW_SHOWNORMAL);
			            ShellExecuteA(null, "open", script_h.save_path, null, null, SW_SHOWNORMAL);
			            if (havefp)
			                ShellExecuteA(null, "open", hpath, null, null, SW_SHOWNORMAL);
//			        }
//			        FreeLibrary(shdll);
//			    }
			}
//			#endif
			
			public bool intersectRects( SDL_Rect result, SDL_Rect rect1, SDL_Rect rect2) {
				if ( (rect1.w == 0) || (rect1.h == 0) ) {
			        result = rect1;
			        return false;
			    } else if ( (rect2.w == 0) || (rect2.h == 0) ) {
			        result = rect2;
			        return false;
			    }
			    if (rect1.x < rect2.x) {
			        result = rect2;
			        if ((rect1.x + rect1.w) < rect2.x) {
			            result.w = 0;
			            return false;
			        } else if ((rect1.x + rect1.w) < (rect2.x + rect2.w)){
			            result.w = rect1.x + rect1.w - rect2.x;
			        }
			    } else {
			        result = rect1;
			        if ((rect2.x + rect2.w) < rect1.x) {
			            result.w = 0;
			            return false;
			        } else if ((rect2.x + rect2.w) < (rect1.x + rect1.w)){
			            result.w = rect2.x + rect2.w - rect1.x;
			        }
			    }
			    if (rect1.y < rect2.y) {
			        result.y = rect2.y;
			        if ((rect1.y + rect1.h) < rect2.y) {
			            result.h = 0;
			            return false;
			        } else if ((rect1.y + rect1.h) < (rect2.y + rect2.h)){
			            result.h = rect1.y + rect1.h - rect2.y;
			        } else
			            result.h = rect2.h;
			    } else {
			        result.y = rect1.y;
			        if ((rect2.y + rect2.h) < rect1.y) {
			            result.h = 0;
			            return false;
			        } else if ((rect2.y + rect2.h) < (rect1.y + rect1.h)){
			            result.h = rect2.y + rect2.h - rect1.y;
			        } else
			            result.h = rect1.h;
			    }
			
			    return true;
			}
			
			public void flush( int refresh_mode, SDL_Rect rect = null, bool clear_dirty_flag = true, bool direct_flag = false )
			{
			    if ( direct_flag ){
			        flushDirect( rect, refresh_mode );
			    }
			    else{
			        if ( null!=rect ) dirty_rect.add( rect );
			
			        if (dirty_rect.bounding_box.w * dirty_rect.bounding_box.h > 0)
			            flushDirect( dirty_rect.bounding_box, refresh_mode );
			    }
			
			    if ( clear_dirty_flag ) dirty_rect.clear();
			}
			
			public void flushDirect( SDL_Rect rect, int refresh_mode, bool updaterect=true )
			{
				CharPtr str = new char[256];// = {0};
			    sprintf(str, "flush %d: %d %d %d %d\n", refresh_mode, rect.x, rect.y, rect.w, rect.h );
				OutputDebugString(str);
			
				//for simple test msdl implement
				//FIXME:???
			#if true //defined(_MSC_VER)
				const int USE_FULL_FLASH_PORT = 1;
				if (0!=USE_FULL_FLASH_PORT) { 
					rect.x = 0;
					rect.y = 0;
					rect.w = 640;
					rect.h = 480;
				}
			#endif    
				
				if (null!=surround_rects) {
			        // playing a movie, need to avoid overpainting it
			        SDL_Rect[] tmp_rects = new SDL_Rect[4];
			        for (int i = 0; i < tmp_rects.Length; ++i)
			        {
			        	tmp_rects[i] = new SDL_Rect();
			        }
			        for (int i=0; i<4; ++i) {
			            if (intersectRects(tmp_rects[i], rect, surround_rects[i])) {
			                refreshSurface( accumulation_surface, tmp_rects[i], refresh_mode );
			                SDL_BlitSurface( accumulation_surface, tmp_rects[i], screen_surface, tmp_rects[i] );
			            }
			        }
			        if (updaterect) SDL_UpdateRects( screen_surface, 4, tmp_rects );
			    } else { 
			        refreshSurface( accumulation_surface, rect, refresh_mode );
			        SDL_BlitSurface( accumulation_surface, rect, screen_surface, rect );
					//SDL_SaveBMP(
					SDL_savebmp(accumulation_surface._surf, "accumulation");
					if (updaterect) SDL_UpdateRect( screen_surface, rect.x, rect.y, (uint)rect.w, (uint)rect.h );
			    }
			}
			
			public void mouseOverCheck( int x, int y )
			{
			    int c = -1;
			
			    last_mouse_state.x = x;
			    last_mouse_state.y = y;
			
			    /* ---------------------------------------- */
			    /* Check button */
			    int button = 0;
			    bool found = false;
			    ButtonLink p_button_link = root_button_link.next;
			    ButtonLink cur_button_link = null;
			    while( null!=p_button_link ){
			        c++;
			        cur_button_link = p_button_link;
			        while (null!=cur_button_link) {
			            if ( x >= cur_button_link.select_rect.x &&
			                 x < cur_button_link.select_rect.x + cur_button_link.select_rect.w &&
			                 y >= cur_button_link.select_rect.y &&
			                 y < cur_button_link.select_rect.y + cur_button_link.select_rect.h &&
			                 ( cur_button_link.button_type != ButtonLink.BUTTON_TYPE.TEXT_BUTTON ||
			                   ( txtbtn_visible && txtbtn_show ) )){
			                bool in_button = true;
			                if (transbtn_flag){
			                    in_button = false;
			                    AnimationInfo anim = null;
			                    if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.SPRITE_BUTTON ||
			                         cur_button_link.button_type == ButtonLink.BUTTON_TYPE.EX_SPRITE_BUTTON )
			                        anim = sprite_info[ cur_button_link.sprite_no ];
			                    else
			                        anim = cur_button_link.anim[0];
			                    int alpha = anim.getPixelAlpha(x - cur_button_link.select_rect.x,
			                                                    y - cur_button_link.select_rect.y);
			                    if (alpha > TRANSBTN_CUTOFF)
			                        in_button = true;
			                }
			                if (in_button){
			                    button = cur_button_link.no;
			                    found = true;
			                    break;
			                }
			            }
			            cur_button_link = cur_button_link.same;
			        }
			        if (found) break;
			        p_button_link = p_button_link.next;
			    }
			
			    if ( (current_button_valid != found) ||
			         (current_over_button != button) ) {
			//printf("mouseOverCheck: refresh (current=%d, button=%d)", current_over_button, button);
			//if (current_button_valid) printf(" valid");
			//printf (" cur_link=%d, p_link=%d\n", (unsigned int)current_button_link, (unsigned int)p_button_link);
			        DirtyRect dirty = dirty_rect;
			        dirty_rect.clear();
			
			        SDL_Rect check_src_rect = new SDL_Rect(0, 0, 0, 0);
			    	SDL_Rect check_dst_rect = new SDL_Rect(0, 0, 0, 0);
			        if (current_button_valid){
			            cur_button_link = current_button_link;
			            while (null!=cur_button_link) {
			                cur_button_link.show_flag = 0;
			                check_src_rect = cur_button_link.image_rect;
			                if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.SPRITE_BUTTON ||
			                     cur_button_link.button_type == ButtonLink.BUTTON_TYPE.EX_SPRITE_BUTTON ){
			                    sprite_info[ cur_button_link.sprite_no ].visible = true;
			                    sprite_info[ cur_button_link.sprite_no ].setCell(0);
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.TMP_SPRITE_BUTTON ){
			                    cur_button_link.show_flag = 1;
			                    cur_button_link.anim[0].visible = true;
			                    cur_button_link.anim[0].setCell(0);
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.TEXT_BUTTON ){
			                    if (txtbtn_visible) {
			                        cur_button_link.show_flag = 1;
			                        cur_button_link.anim[0].visible = true;
			                        cur_button_link.anim[0].setCell(0);
			                    }
			                }
			                else if ( cur_button_link.anim[1] != null ){
			                    cur_button_link.show_flag = 2;
			                }
			                dirty_rect.add( cur_button_link.image_rect );
			                if ( is_exbtn_enabled && null!=exbtn_d_button_link.exbtn_ctl ){
			                    decodeExbtnControl( exbtn_d_button_link.exbtn_ctl, check_src_rect, check_dst_rect );
			                }
			
			                cur_button_link = cur_button_link.same;
			            }
			        } else {
			            if ( is_exbtn_enabled && null!=exbtn_d_button_link.exbtn_ctl ){
			                decodeExbtnControl( exbtn_d_button_link.exbtn_ctl, check_src_rect, check_dst_rect );
			            }
			        }
			
			        if ( null!=p_button_link ){
			            if ( system_menu_mode != SYSTEM_NULL ){
			                if ( null!=menuselectvoice_file_name[MENUSELECTVOICE_OVER] )
			                    playSound(menuselectvoice_file_name[MENUSELECTVOICE_OVER],
			                              SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			            }
			            else{
			                if ( null!=selectvoice_file_name[SELECTVOICE_OVER] )
			                    playSound(selectvoice_file_name[SELECTVOICE_OVER],
			                              SOUND_WAVE|SOUND_OGG, false, MIX_WAVE_CHANNEL);
			            }
			            cur_button_link = p_button_link;
			            while (null!=cur_button_link) {
			                check_dst_rect = cur_button_link.image_rect;
			                if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.SPRITE_BUTTON ||
			                     cur_button_link.button_type == ButtonLink.BUTTON_TYPE.EX_SPRITE_BUTTON ){
			                    sprite_info[ cur_button_link.sprite_no ].setCell(1);
			                    sprite_info[ cur_button_link.sprite_no ].visible = true;
			                    if ((cur_button_link == p_button_link) && is_exbtn_enabled &&
			                        (cur_button_link.button_type == ButtonLink.BUTTON_TYPE.EX_SPRITE_BUTTON)){
			                        decodeExbtnControl( cur_button_link.exbtn_ctl, check_src_rect, check_dst_rect );
			                    }
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.TMP_SPRITE_BUTTON){
			                    cur_button_link.show_flag = 1;
			                    cur_button_link.anim[0].visible = true;
			                    cur_button_link.anim[0].setCell(1);
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.TEXT_BUTTON &&
			                          txtbtn_show && txtbtn_visible ){
			                    cur_button_link.show_flag = 1;
			                    cur_button_link.anim[0].visible = true;
			                    cur_button_link.anim[0].setCell(1);
			                    if ((cur_button_link == p_button_link) &&
			                        is_exbtn_enabled && null!=cur_button_link.exbtn_ctl ){
			                        decodeExbtnControl( cur_button_link.exbtn_ctl, check_src_rect, check_dst_rect );
			                    }
			                }
			                else if ( cur_button_link.button_type == ButtonLink.BUTTON_TYPE.NORMAL_BUTTON ||
			                          cur_button_link.button_type == ButtonLink.BUTTON_TYPE.LOOKBACK_BUTTON ){
			                    cur_button_link.show_flag = 1;
			                }
			                dirty_rect.add( cur_button_link.image_rect );
			                cur_button_link = cur_button_link.same;
			            }
			            if (c>=0)
			                shortcut_mouse_line = c;
			        }
			        current_button_link = p_button_link;
			
			        flush( refreshMode() );
			        dirty_rect = dirty;
			    }
			    current_button_valid = found;
			    current_over_button = button;
			}
			
			public void executeLabel()
			{
			    int last_token_line = -1;
			
			  executeLabelTop:
			
			    while ( current_line<current_label_info.num_of_lines ){
			        if ( (debug_level > 0) && (last_token_line != current_line) )
			            printf("\n*****  executeLabel %s:%d/%d:%d:%d *****\n",
			                   current_label_info.name,
			                   current_line,
			                   current_label_info.num_of_lines,
			                   string_buffer_offset, display_mode );
			            fflush(stdout);
			        last_token_line = current_line;
			
			        if ( script_h.getStringBuffer()[0] == '~' ){
			            last_tilde.next_script = script_h.getNext();
			            readToken();
			            continue;
			        }
			        if ( break_flag && !script_h.isName("next") ){
			            if ( script_h.getStringBuffer()[0] == 0x0a )
			                current_line++;
			
			            if ((script_h.getStringBuffer()[0] != ':') &&
			                (script_h.getStringBuffer()[0] != ';') &&
			                (script_h.getStringBuffer()[0] != 0x0a))
			                script_h.skipToken();
			
			            readToken();
			            continue;
			        }
			
			        if ( kidokuskip_flag && 0!=(skip_mode & SKIP_NORMAL) &&
			             kidokumode_flag && !script_h.isKidoku() )
			            skip_mode &= ~SKIP_NORMAL;
			
			        //check for quit event before running each command, for safety
			        //(this won't prevent all window lockups, but should give some
			        // greater chance of the user being able to quit when one happens)
			        SDL_PumpEvents();
			        if (0!=SDL_PeepEvents( null, 1, SDL_eventaction.SDL_PEEKEVENT, SDL_QUITMASK) )
			            endCommand();
			
			        int ret = base.parseLine();
			        if ( ret == RET_NOMATCH ) ret = this.parseLine();
			
			        if ( 0!=(ret & (RET_SKIP_LINE | RET_EOL)) ){
			        	if (0!=(ret & RET_SKIP_LINE)) script_h.skipLine();
			            if (++current_line >= current_label_info.num_of_lines) break;
			        }
			
			        if ( 0!=(ret & RET_EOT) ) processEOT();
			        
			        if (0==(ret & RET_NO_READ)) readToken();
			    }
			
			    current_label_info = script_h.lookupLabelNext( current_label_info.name );
			    current_line = 0; last_token_line = -1;
			
			    if ( current_label_info.start_address != null ){
			        script_h.setCurrent( current_label_info.label_header );
			        readToken();
			        goto executeLabelTop;
			    }
			
			    fprintf( stderr, " ***** End *****\n");
			    endCommand();
			}
			
			public void runScript()
			{
			    readToken();
			
			    int ret = base.parseLine();
			    if ( ret == RET_NOMATCH ) ret = this.parseLine();
			}
			
			public override int parseLine( )
			{
				int ret;
				CharPtr cmd = new CharPtr(script_h.getStringBuffer());
			    if (cmd[0] == '_'){
			        int c=0;
			        while (cmd[c+1] != 0) {
			            cmd[c] = cmd[c+1];
			            c++;
			        }
			        cmd[c] = '\0';
			    }
			    CharPtr s_buf = script_h.getStringBuffer();
			    if ( !script_h.isText() ){
			        snprintf(script_h.current_cmd, 64, "%s", s_buf);
			        //Check against builtin cmds
			        if (cmd[0] >= 'a' && cmd[0] <= 'z'){
						//TODO:
						CharPtr debugstr = new char[256];
						for (int i = 0; i < 256; ++i)
						{
							debugstr[i] = (char)0;
						}
						sprintf(debugstr, ">>>>>>>> %s\n", cmd); 
						OutputDebugString(debugstr);
			
			            FuncHash fh = func_hash[cmd[0]-'a'];
			            for (int i=fh.start ; i<=fh.end ; i++){
			                if ( 0==strcmp( func_lut[i].command, cmd ) ){
			                    return func_lut[i].method(this);
			                }
			            }
			        }
			
			        script_h.current_cmd_type = ScriptHandler.CMD_BUILTIN;
			        if ( s_buf[0] == 0x0a ){
			            script_h.current_cmd_type = ScriptHandler.CMD_NONE;
			            return RET_CONTINUE | RET_EOL;
			        }
			        else if ((s_buf[0] == 'v') && (s_buf[1] >= '0') && (s_buf[1] <= '9')){
			            strcpy(script_h.current_cmd, "vNUM");
			            return vCommand();
			        }
			        else if ((s_buf[0] == 'd') && (s_buf[1] == 'v') &&
			                 (s_buf[2] >= '0') && (s_buf[2] <= '9')){
			            strcpy(script_h.current_cmd, "dvNUM");
			            return dvCommand();
			        }
			        else if ((s_buf[0] == 'm') && (s_buf[1] == 'v') &&
			                 (s_buf[2] >= '0') && (s_buf[2] <= '9')){
			            strcpy(script_h.current_cmd, "mvNUM");
			            return mvCommand();
			        }
			
			        script_h.current_cmd_type = ScriptHandler.CMD_UNKNOWN;
			        snprintf(script_h.errbuf, MAX_ERRBUF_LEN, "command [%s] is not supported yet!!", s_buf );
			        errorAndCont(script_h.errbuf);
			
			        script_h.skipToken();
			
			        return RET_CONTINUE;
			    }
			
			    /* Text */
			    script_h.current_cmd_type = ScriptHandler.CMD_TEXT;
			
			    if ( current_mode == DEFINE_MODE )
			        errorAndExit( "text cannot be displayed while in the define section." );
			    ret = textCommand();
			    //Mion: moved all text processing into textCommand & its subfunctions
			
			    return ret;
			}
			
			/* ---------------------------------------- */
			public void processTextButtonInfo()
			{
			    TextButtonInfoLink info = text_button_info.next;
			
			    if (null!=info) txtbtn_show = true;
			    while (null!=info) {
			        ButtonLink firstbtn = null;
			        CharPtr text = new CharPtr(info.prtext);
			        CharPtr text2;
			        Fontinfo f_info = sentence_font;
			        //f_info.clear();
			        f_info.xy[0] = info.xy[0];
			        f_info.xy[1] = info.xy[1];
			        setColor(f_info.off_color, linkcolor[0]);
			        setColor(f_info.on_color, linkcolor[1]);
			        do {
			            text2 = strchr(text, 0x0a);
			            if (null!=text2) {
			            	text2[0] = '\0';
			            }
			            ButtonLink txtbtn = getSelectableSentence(text, f_info, true, false, false);
			            //printf("made txtbtn: %d '%s'\n", info->no, text);
			            txtbtn.button_type = ButtonLink.BUTTON_TYPE.TEXT_BUTTON;
			            txtbtn.no = info.no;
			            if (!txtbtn_visible)
			                txtbtn.show_flag = 0;
			            if (null!=firstbtn)
			                firstbtn.connect(txtbtn);
			            else
			                firstbtn = txtbtn;
			            f_info.xy[0] = info.xy[0];
			            f_info.xy[1] = info.xy[1];
			            f_info.newLine();
			            if (null!=text2) {
			            	text2[0] = (char)0x0a;
			            	text2.inc();
			            }
			            text = new CharPtr(text2);
			        } while (null!=text2);
			        root_button_link.insert(firstbtn);
			        info.button = firstbtn;
			        info = info.next;
			    }
			}
			
			public void deleteTextButtonInfo()
			{
			    TextButtonInfoLink i1 = text_button_info.next;
			
			    while( null!=i1 ){
			        TextButtonInfoLink i2 = i1;
			        // need to hide textbtn links
			        ButtonLink cur_button_link = i2.button;
			        while (null!=cur_button_link) {
			            cur_button_link.show_flag = 0;
			            cur_button_link = cur_button_link.same;
			        }
			        i1 = i1.next;
			        i2 = null;//delete i2;
			    }
			    text_button_info.next = null;
			    txtbtn_visible = false;
			    next_txtbtn_num = txtbtn_start_num;
			}
			
			public void deleteButtonLink()
			{
			    ButtonLink b1 = root_button_link.next;
			
			    while( null!=b1 ){
			        ButtonLink b2 = b1.same;
			        while ( null!=b2 ) {
			            ButtonLink b3 = b2;
			            b2 = b2.same;
			            b3 = null;//delete b3;
			        }
			        b2 = b1;
			        b1 = b1.next;
			        if ( b2.button_type == ButtonLink.BUTTON_TYPE.TEXT_BUTTON ) {
			            // Need to delete ref to button from text_button_info
			            TextButtonInfoLink i1 = text_button_info.next;
			            while (null!=i1) {
			                if (i1.button == b2)
			                    i1.button = null;
			                i1 = i1.next;
			            }
			        }
			        b2 = null;//delete b2;
			    }
			    root_button_link.next = null;
			    current_button_link = null;
			    current_button_valid = false;
			
			    if ( null!=exbtn_d_button_link.exbtn_ctl ) exbtn_d_button_link.exbtn_ctl = null;//delete[] exbtn_d_button_link.exbtn_ctl;
			    exbtn_d_button_link.exbtn_ctl = null;
			    is_exbtn_enabled = false;
			}
			
			void refreshMouseOverButton()
			{
			    int mx = 0, my = 0;
			    current_over_button = -1;
			    current_button_valid = false;
			    current_button_link = null;
			    SDL_GetMouseState( ref mx, ref my );
			    mouseOverCheck( mx, my );
			}
			
			/* ---------------------------------------- */
			/* Delete select link */
			public void deleteSelectLink()
			{
			    SelectLink link, last_select_link = root_select_link.next;
			
			    while ( null!=last_select_link ){
			        link = last_select_link;
			        last_select_link = last_select_link.next;
			        link = null;//delete link;
			    }
			    root_select_link.next = null;
			}
			
			public void clearCurrentPage()
			{
			    sentence_font.clear();
			
			    int num = (sentence_font.num_xy[0]*2+1)*sentence_font.num_xy[1];
			    if (sentence_font.getTateyokoMode() == Fontinfo.TATE_MODE)
			        num = (sentence_font.num_xy[1]*2+1)*sentence_font.num_xy[1];
			
			// TEST for ados backlog cutoff problem
			    num *= 2;
			
			    if ( null!=current_page.text &&
			         current_page.max_text != num ){
			        //delete[] current_page->text;
			        current_page.text = null;
			    }
			    if ( null==current_page.text ){
			        current_page.text = new char[num];
			        current_page.max_text = num;
			    }
			    current_page.text_count = 0;
			
			    if (null!=current_page.tag){
			        //delete[] current_page->tag;
			        current_page.tag = null;
			    }
			
			    num_chars_in_sentence = 0;
			    internal_saveon_flag = true;
			
			    text_info.fill( 0, 0, 0, 0 );
			    cached_page = current_page;
			
			    deleteTextButtonInfo();
			}
			
			public void displayTextWindow( SDL_Surface surface, SDL_Rect clip )
			{
			    if ( current_font.is_transparent ){
			
					SDL_Rect rect = new SDL_Rect(0, 0, screen_width, screen_height);
			        if ( current_font == sentence_font )
			            rect = sentence_font_info.pos;
			
			        if ( 0!=AnimationInfo.doClipping( rect, clip ) ) return;
			
			        if ( rect.x + rect.w > SDL_Surface_get_w(surface) ) rect.w = SDL_Surface_get_w(surface) - rect.x;
			        if ( rect.y + rect.h > SDL_Surface_get_h(surface) ) rect.h = SDL_Surface_get_h(surface) - rect.y;
			
			        SDL_LockSurface( surface );
			        Uint32Ptr buf = new Uint32Ptr(SDL_Surface_get_pixels(surface), + rect.y * SDL_Surface_get_w(surface) + rect.x);
			
			        SDL_PixelFormat fmt = SDL_Surface_get_format(surface);
			        int[] color = new int[3];
			        color[0] = current_font.window_color[0] + 1;
			        color[1] = current_font.window_color[1] + 1;
			        color[2] = current_font.window_color[2] + 1;
			
			        for ( int i=rect.y ; i<rect.y + rect.h ; i++ ){
			        	for ( int j=rect.x ; j<rect.x + rect.w ; j++, buf.inc() ){
			        		buf[0] = (uint)((((buf[0] & fmt.Rmask) >> fmt.Rshift) * color[0] >> 8) << fmt.Rshift |
			                    (((buf[0] & fmt.Gmask) >> fmt.Gshift) * color[1] >> 8) << fmt.Gshift |
			                    (((buf[0] & fmt.Bmask) >> fmt.Bshift) * color[2] >> 8) << fmt.Bshift);
			            }
			        	buf.inc(SDL_Surface_get_w(surface) - rect.w);
			        }
			
			        SDL_UnlockSurface( surface );
			    }
			    else if ( null!=sentence_font_info.image_surface ){
			        drawTaggedSurface( surface, sentence_font_info, clip );
			    }
			}
			
			public void newPage( bool next_flag )
			{
			    /* ---------------------------------------- */
			    /* Set forward the text buffer */
			    if ( current_page.text_count != 0 ){
			        current_page = current_page.next;
			        if ( start_page == current_page )
			            start_page = start_page.next;
			    }
			
			    if ( next_flag ){
			        indent_offset = 0;
			        page_enter_status = 0;
			    }
			    
			    clearCurrentPage();
			    txtbtn_visible = false;
			    txtbtn_show = false;
			
			    flush( refreshMode(), sentence_font_info.pos );
			}
			
			public AnimationInfo getSentence( CharPtr buffer, Fontinfo info, int num_cells, bool flush_flag = true, bool nofile_flag = false, bool skip_whitespace = true )
			{
				//Mion: moved from getSelectableSentence and modified
				int[] current_text_xy = new int[2];
			    current_text_xy[0] = info.xy[0];
			    current_text_xy[1] = info.xy[1];
			
			    AnimationInfo anim = new AnimationInfo();
			
			    anim.trans_mode = AnimationInfo.TRANS_STRING;
			    anim.is_single_line = false;
			    anim.num_of_cells = num_cells;
			    anim.color_list = new byte[ num_cells ][];
			    for (int i = 0; i < anim.color_list.Length; ++i)
			    {
			    	anim.color_list[i] = new byte[3];
			    }
			    for (int i=0 ; i<3 ; i++){
			        if (nofile_flag)
			            anim.color_list[0][i] = info.nofile_color[i];
			        else
			            anim.color_list[0][i] = info.off_color[i];
			        if (num_cells > 1)
			            anim.color_list[1][i] = info.on_color[i];
			    }
			    anim.skip_whitespace = skip_whitespace;
			    setStr( ref anim.file_name, buffer );
			    anim.orig_pos.x = info.x();
			    anim.orig_pos.y = info.y();
			    UpdateAnimPosXY(anim);
			    anim.visible = true;
			
			    setupAnimationInfo( anim, info );
			
			    info.newLine();
			    if (info.getTateyokoMode() == Fontinfo.YOKO_MODE)
			        info.xy[0] = current_text_xy[0];
			    else
			        info.xy[1] = current_text_xy[1];
			
			    dirty_rect.add( anim.pos );
			
			    return anim;
			}
			
			public ButtonLink getSelectableSentence( CharPtr buffer, Fontinfo info, bool flush_flag = true, bool nofile_flag = false, bool skip_whitespace = true )
			{
				ButtonLink button_link = new ButtonLink();
			    button_link.button_type = ButtonLink.BUTTON_TYPE.TMP_SPRITE_BUTTON;
			    button_link.show_flag = 1;
			
			    AnimationInfo anim = getSentence(buffer, info, 2, flush_flag,
			                                      nofile_flag, skip_whitespace);
			    button_link.anim[0] = anim;
			    button_link.select_rect = button_link.image_rect = anim.pos;
			
			    return button_link;
			}
			
			public void decodeExbtnControl( CharPtr ctl_str, SDL_Rect check_src_rect = null, SDL_Rect check_dst_rect = null )
			{
				ctl_str = new CharPtr(ctl_str); //FIXME:added
			    CharPtr sound_name = new char[256];
			    int i, sprite_no, sprite_no2, cell_no;
			
			    while( true ){ char com = ctl_str[0]; ctl_str.inc(); if (0!=com) {/*loop*/;}else{break;}
			        if (com == 'C' || com == 'c'){
			            sprite_no = getNumberFromBuffer( ref ctl_str );
			            sprite_no2 = sprite_no;
			            cell_no = -1;
			            if ( ctl_str[0] == '-' ){
			            	ctl_str.inc();
			                sprite_no2 = getNumberFromBuffer( ref ctl_str );
			            }
			            for (i=sprite_no ; i<=sprite_no2 ; i++)
			                refreshSprite( i, false, cell_no, null, null );
			        }
			        else if (com == 'P' || com == 'p'){
			            sprite_no = getNumberFromBuffer( ref ctl_str );
			            if ( ctl_str[0] == ',' ){
			            	ctl_str.inc();
			                cell_no = getNumberFromBuffer( ref ctl_str );
			            }
			            else
			                cell_no = 0;
			            refreshSprite( sprite_no, true, cell_no, check_src_rect, check_dst_rect );
			        }
			        else if (com == 'S' || com == 's'){
			            sprite_no = getNumberFromBuffer( ref ctl_str );
			            if      (sprite_no < 0) sprite_no = 0;
			            else if (sprite_no >= ONS_MIX_CHANNELS) sprite_no = ONS_MIX_CHANNELS-1;
			            if ( ctl_str[0] != ',' ) continue;
			            ctl_str.inc();
			            if ( ctl_str[0] != '(' ) continue;
			            ctl_str.inc();
			            CharPtr buf = new CharPtr(sound_name);
			            while (ctl_str[0] != ')' && ctl_str[0] != '\0' ) { buf[0] = ctl_str[0]; ctl_str.inc(); buf.inc();}
			            buf[0] = '\0'; buf.inc();
			            playSound(sound_name, SOUND_WAVE|SOUND_OGG, false, sprite_no);
			            if ( ctl_str[0] == ')' ) ctl_str.inc();
			        }
			        else if (com == 'M' || com == 'm'){
			            sprite_no = getNumberFromBuffer( ref ctl_str );
			            SDL_Rect rect = sprite_info[ sprite_no ].pos;
			            if ( ctl_str[0] != ',' ) continue;
			            ctl_str.inc(); // skip ','
			            sprite_info[ sprite_no ].orig_pos.x = getNumberFromBuffer( ref ctl_str );
			            if ( ctl_str[0] != ',' ) {
			                UpdateAnimPosXY(sprite_info[ sprite_no ]);
			                continue;
			            }
			            ctl_str.inc(); // skip ','
			            sprite_info[ sprite_no ].orig_pos.y = getNumberFromBuffer( ref ctl_str );
			            UpdateAnimPosXY(sprite_info[ sprite_no ]);
			            dirty_rect.add( rect );
			            sprite_info[ sprite_no ].visible = true;
			            dirty_rect.add( sprite_info[ sprite_no ].pos );
			        }
			    }
			}
			
			public void loadCursor( int no, CharPtr str, int x, int y, bool abs_flag = false )
			{
			    cursor_info[ no ].setImageName( str );
			    cursor_info[ no ].orig_pos.x = x;
			    cursor_info[ no ].orig_pos.y = y;
			    UpdateAnimPosXY(cursor_info[ no ]);
			
			    parseTaggedString( cursor_info[ no ] );
			    setupAnimationInfo( cursor_info[ no ] );
			    if ( filelog_flag )
			        script_h.findAndAddLog( script_h.log_info[ScriptHandler.FILE_LOG], cursor_info[ no ].file_name, true ); // a trick for save file
			    cursor_info[ no ].abs_flag = abs_flag;
			    if ( null!=cursor_info[ no ].image_surface )
			        cursor_info[ no ].visible = true;
			    else
			        cursor_info[ no ].remove();
			}
			
			public void saveAll()
			{
			    // only save the game state if save_path is set
			    if (script_h.save_path != null) {
			        saveEnvData();
			        saveGlovalData();
			        if ( filelog_flag )  writeLog( script_h.log_info[ScriptHandler.FILE_LOG] );
			        if ( labellog_flag ) writeLog( script_h.log_info[ScriptHandler.LABEL_LOG] );
			        if ( kidokuskip_flag ) script_h.saveKidokuData();
			    }
			}
			
			public void loadEnvData()
			{
			    volume_on_flag = true;
			    text_speed_no = 1;
			    skip_mode &= ~SKIP_TO_EOP;
			    setStr( ref default_env_font, null );
			    cdaudio_on_flag = true;
			    setStr( ref default_cdrom_drive, null );
			    kidokumode_flag = true;
			    use_default_volume = true;
			    bgmdownmode_flag = false;
			    setStr( ref savedir, null );
			
			    if (loadFileIOBuf( "envdata" ) == 0){
			        use_default_volume = false;
			        if (readInt() == 1 && window_mode == false) menu_fullCommand();
			        if (readInt() == 0) volume_on_flag = false;
			        text_speed_no = readInt();
			        if (readInt() == 1) skip_mode |= SKIP_TO_EOP;
			        readStr( ref default_env_font );
			        if (default_env_font == null)
			            setStr(ref default_env_font, DEFAULT_ENV_FONT);
			        if (readInt() == 0) cdaudio_on_flag = false;
			        readStr( ref default_cdrom_drive );
			        voice_volume = DEFAULT_VOLUME - readInt();
			        se_volume = DEFAULT_VOLUME - readInt();
			        music_volume = DEFAULT_VOLUME - readInt();
			        if (readInt() == 0) kidokumode_flag = false;
			        if (readInt() == 1) {
			            bgmdownmode_flag = true;
			        }
			        readStr( ref savedir );
			        if (null!=savedir)
			            script_h.setSavedir(savedir);
			        else
			            setStr( ref savedir, "" ); //prevents changing savedir
			    }
			    else{
			        setStr( ref default_env_font, DEFAULT_ENV_FONT );
			        voice_volume = se_volume = music_volume = DEFAULT_VOLUME;
			    }
			    // set the volumes of channels
			    channelvolumes[0] = voice_volume;
			    for ( int i=1 ; i<ONS_MIX_CHANNELS ; i++ )
			        channelvolumes[i] = se_volume;
			}
			
			public void saveEnvData()
			{
			    file_io_buf_ptr = 0;
			    bool output_flag = false;
			    for (int i=0 ; i<2 ; i++){
			        writeInt( fullscreen_mode?1:0, output_flag );
			        writeInt( volume_on_flag?1:0, output_flag );
			        writeInt( text_speed_no, output_flag );
			        writeInt( 0!=(skip_mode & SKIP_TO_EOP)?1:0, output_flag );
			        writeStr( default_env_font, output_flag );
			        writeInt( cdaudio_on_flag?1:0, output_flag );
			        writeStr( default_cdrom_drive, output_flag );
			        writeInt( DEFAULT_VOLUME - voice_volume, output_flag );
			        writeInt( DEFAULT_VOLUME - se_volume, output_flag );
			        writeInt( DEFAULT_VOLUME - music_volume, output_flag );
			        writeInt( kidokumode_flag?1:0, output_flag );
			        writeInt( bgmdownmode_flag?1:0, output_flag );
			        writeStr( savedir, output_flag );
			        writeInt( 1000, output_flag );
			
			        if (i==1) break;
			        allocFileIOBuf();
			        output_flag = true;
			    }
			
			    saveFileIOBuf( "envdata" );
			}
			
			public int refreshMode()
			{
				if (0!=(display_mode & DISPLAY_MODE_TEXT))
			        return refresh_window_text_mode;
			
			    return REFRESH_NORMAL_MODE;
			}
			
			public void quit()
			{
			    saveAll();
			}
			
			public void disableGetButtonFlag()
			{
			    btndown_flag = false;
			    transbtn_flag = false;
			
			    getzxc_flag = false;
			    gettab_flag = false;
			    getpageup_flag = false;
			    getpagedown_flag = false;
			    getinsert_flag = false;
			    getfunction_flag = false;
			    getenter_flag = false;
			    getcursor_flag = false;
			    spclclk_flag = false;
			    getmclick_flag = false;
			    getskipoff_flag = false;
			    getmouseover_flag = false;
			    getmouseover_min = getmouseover_max = 0;
			    btnarea_flag = false;
			    btnarea_pos = 0;
			}
			
			public int getNumberFromBuffer( ref CharPtr buf )
			{
				int ret = 0;
				while ( buf[0] >= '0' && buf[0] <= '9' ) {
					ret = ret*10 + buf[0] - '0'; buf.inc();
				}
			
			    return ret;
			}
		}
	}
}
