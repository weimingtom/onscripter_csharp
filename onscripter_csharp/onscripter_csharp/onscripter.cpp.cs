/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-15
 * Time: 4:35
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
		 *  onscripter.cpp -- main function of ONScripter-EN
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
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4244)
//		#endif
//		
//		
//		#include "ONScripterLabel.h"
//		#include "version.h"
//		
//		#include <cstdio>
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
		
		private const string CFG_FILE = "ons.cfg";
		
		private static void optionHelp()
		{
		    printf( "Usage: onscripter [option ...]\n" );
		    printf( "      --cdaudio\t\tuse CD audio if available\n");
		    printf( "      --cdnumber no\tchoose the CD-ROM drive number\n");
		    printf( "  -f, --font file\tset a TTF font file\n");
		    printf( "      --registry file\tset a registry file\n");
		    printf( "      --dll file\tset a dll file\n");
		    printf( "      --english\t\tset preferred text mode to English (default)\n");
		    printf( "      --japanese\tset preferred text mode to Japanese\n");
		    printf( "      --english-menu\tuse English system menu messages (default)\n");
		    printf( "      --japanese-menu\tuse Japanese system menu messages\n");
		#if   true//defined WIN32
		    printf( "  -r, --root path\tset the root path to the archives\n");
		    printf( "  -s, --save path\tset the path to use for saved games (default: folder in All Users profile)\n");
		    printf( "      --current-user-appdata\tuse the current user's AppData folder instead of AllUsers' AppData\n");
		#elif MACOSX
		    printf( "  -r, --root path\tset the root path to the archives (default: Resources in ONScripter bundle)\n");
		    printf( "  -s, --save path\tset the path to use for saved games (default: folder in ~/Library/Application Support)\n");
		#elif LINUX
		    printf( "  -r, --root path\tset the root path to the archives\n");
		    printf( "  -s, --save path\tset the path to use for saved games (default: hidden subdirectory in ~)\n");
		#else
		    printf( "  -r, --root path\tset the root path to the archives\n");
		    printf( "  -s, --save path\tset the path to use for saved games (default: same as root path)\n");
		#endif
		    printf( "      --use-app-icons\tuse the icns for the current application, if bundled/embedded\n");
		    printf( "      --fullscreen\tstart in fullscreen mode\n");
		    printf( "      --window\t\tstart in window mode\n");
		#if !PDA
		    printf( "      --window-width width\t\tset preferred window width\n");
		#endif
		    printf( "      --gameid id\t\tset game identifier (like with game.id)\n");
		#if RCA_SCALE
		    printf( "      --widescreen\ttransform game to match widescreen monitors\n");
		#endif
		    printf( "      --scale\t\tscale game to native display size. Yields small sharp text.\n");
		    printf( "      --force-png-alpha\t\talways use PNG alpha channels\n");
		    printf( "      --force-png-nscmask\talways use NScripter-style masks\n");
		    printf( "      --detect-png-nscmask\tdetect PNG alpha images that actually use masks\n");
		    printf( "      --force-button-shortcut\tignore useescspc and getenter command\n");
		#if USE_X86_GFX
		    printf( "      --disable-cpu-gfx\tdo not use MMX/SSE2 graphics acceleration routines\n");
		#elif  USE_PPC_GFX
		    printf( "      --disable-cpu-gfx\tdo not use Altivec graphics acceleration routines\n");
		#endif
		    printf( "      --enable-wheeldown-advance\tadvance the text on mouse wheeldown event\n");
		    printf( "      --disable-rescale\tdo not rescale the images in the archives when compiled with -DPDA\n");
		    printf( "      --edit\t\tenable editing the volumes and the variables when 'z' is pressed\n");
		    printf( "      --fileversion\tset the ONS file version for loading unversioned files\n");
		    printf( "      --key-exe file\tset a file (*.EXE) that includes a key table\n");
		    printf( "      --nsa-offset offset\tuse byte offset x when reading arc*.nsa files\n");
		    printf( "      --allow-color-type-only\tsyntax option for only recognizing color type for color arguments\n");
		    printf( "      --set-tag-page-origin-to-1\tsyntax option for setting 'gettaglog' origin to 1 instead of 0\n");
		    printf( "      --answer-dialog-with-yes-ok\thave 'yesnobox' and 'okcancelbox' give 'yes/ok' result\n");
		    printf( "      --ignore-textgosub-newline\tignore newline after a clickwait when in textgosub mode\n");
		    printf( "      --skip-past-newline\twhen doing a 'click to skip', don't leave that skip mode at newlines\n");
		    printf( "      --strict\t\ttreat warnings more like errors\n");
		    printf( "      --debug\t\tgenerate runtime debugging output (use multiple times to increase debug level)\n");
		    printf( "  -h, --help\t\tshow this help and exit\n");
		    printf( "  -v, --version\t\tshow the version information and exit\n");
		    exit(0);
		}
		
		private static void optionVersion()
		{
		    printf("ONScripter-EN version %s (%d.%02d)\n", ONS_VERSION, NSC_VERSION/100, NSC_VERSION%100 );
		    printf("Original written by Ogapee <ogapee@aqua.dti2.ne.jp>,\n");
		    printf("English fork maintained by \"Uncle\" Mion Sonozaki <UncleMion@gmail.com>\n\n");
		    printf("Copyright (c) 2001-2011 Ogapee, 2007-2011 Sonozaki\n");
		    printf("This is free software; see the source for copying conditions.\n");
		    exit(0);
		}
		
		private static void parseOptions(int argc, CharPtr[] argvOri, ONScripterLabel ons, ref bool hasArchivePath)
		{
			int argv_ = 0;
			argv_++; 
			CharPtr argv_0 = argvOri[argv_];
		    while( argc > 1 ){
		        if ( argv_0[0] == '-' ){
					if ( 0==strcmp( new CharPtr(argv_0,+1), "h" ) || 0==strcmp( new CharPtr(argv_0,+1), "-help" ) ){
		                optionHelp();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "v" ) || 0==strcmp( new CharPtr(argv_0,+1), "-version" ) ){
		                optionVersion();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-cdaudio" ) ){
		                ons.enableCDAudio();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-cdnumber" ) ){
		                argc--;
		                argv_++;
		                ons.setCDNumber(atoi(argv_0));
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "f" ) || 0==strcmp( new CharPtr(argv_0,+1), "-font" ) ){
		                argc--;
		                argv_++;
		                ons.setFontFile(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-registry" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setRegistryFile(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-dll" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setDLLFile(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-english" ) ){
		                ons.setEnglishPreferred();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-japanese" ) ){
		                ons.setJapanesePreferred();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-english-menu" ) ){
		                ons.setEnglishMenu();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-japanese-menu" ) ){
		                ons.setJapaneseMenu();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "r" ) || 0==strcmp( new CharPtr(argv_0,+1), "-root" ) ){
		                hasArchivePath = true;
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setArchivePath(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "s" ) || 0==strcmp( new CharPtr(argv_0,+1), "-save" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setSavePath(argv_0);
		            }
		#if true// WIN32
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-current-user-appdata" ) ){
		                ons.setUserAppData();
		            }
		#endif
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-use-app-icons" ) ){
		                ons.setUseAppIcons();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-fileversion" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setFileVersion(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-fullscreen" ) ){
		                ons.setFullscreenMode();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-window" ) ){
		                ons.setWindowMode();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-window-width" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setPreferredWidth(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-gameid" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setGameIdentifier(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-nsa-offset" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setNsaOffset(argv_0);
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-force-button-shortcut" ) ){
		                ons.enableButtonShortCut();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-enable-wheeldown-advance" ) ){
		                ons.enableWheelDownAdvance();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-debug" ) ){
		                ons.add_debug_level();
		            }
		#if USE_X86_GFX || USE_PPC_GFX
		            else if ( 0==strcmp( argv[0]+1, "-disable-cpu-gfx" ) ){
		                ons.disableCpuGfx();
		                printf("disabling CPU accelerated graphics routines\n");
		            }
		#endif
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-disable-rescale" ) ){
		                ons.disableRescale();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-allow-color-type-only" ) ){
		                ons.allow_color_type_only = true;
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-set-tag-page-origin-to-1" ) ){
		                ons.set_tag_page_origin_to_1 = true;
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-answer-dialog-with-yes-ok" ) ){
		                ons.answer_dialog_with_yes_ok = true;
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-ignore-textgosub-newline" ) ){
		                ons.setIgnoreTextgosubNewline();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-skip-past-newline" ) ){
		                ons.setSkipPastNewline();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-edit" ) ){
		                ons.enableEdit();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-strict" ) ){
		                ons.setStrict();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-key-exe" ) ){
		                argc--;
		                argv_++;argv_0 = argvOri[argv_];
		                ons.setKeyEXE(argv_0);
		            }
		#if RCA_SCALE
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-widescreen" ) ){
		                ons.setWidescreen();
		            }
		#endif
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-scale" ) ){
		                ons.setScaled();
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-detect-png-nscmask" ) ){
		                ons.setMaskType( ONScripterLabel.PNG_MASK_AUTODETECT );
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-force-png-alpha" ) ){
		                ons.setMaskType( ONScripterLabel.PNG_MASK_USE_ALPHA );
		            }
		            else if ( 0==strcmp( new CharPtr(argv_0,+1), "-force-png-nscmask" ) ){
		                ons.setMaskType( ONScripterLabel.PNG_MASK_USE_NSCRIPTER );
		            }
		            else{
		                CharPtr errstr = new char[256];
		                snprintf(errstr, 256, "unknown option %s", argv_0);
		                ons.errorAndCont(errstr, null, "Command-Line Issue", true);
		            }
		        }
		        else if (!hasArchivePath) {
		            hasArchivePath = true;
		            ons.setArchivePath(argv_0);
		            argc--;
		            argv_++;argv_0 = argvOri[argv_];
		        }
		        else{
		            optionHelp();
		        }
		        argc--;
		        argv_++;argv_0 = argvOri[argv_];
		    }
		}
		
		private static bool parseOptionFile(CharPtr filename, ONScripterLabel ons, ref bool hasArchivePath)
		{
			int argc;
			CharPtr[] argv = null;
		
		    argc = 1;
		    FILEPtr fp = fopen(filename, "r");
		    if (null==fp) {
		        //printf("Couldn't open option file '%s'\n", filename);
		        return false;
		    }
		
		    printf("Reading command-line options from '%s'\n", filename);
		    int numlines = 1;
		    int curlen = 0, maxlen = 0;
		    while (0==feof(fp)) {
		        char ch = (char) fgetc(fp);
		        ++curlen;
		        if ((ch == '\0') || (ch == '\r') || (ch == '\n')) {
		            ++numlines;
		            if (curlen > maxlen)
		                maxlen = curlen;
		            curlen = 0;
		        }
		    }
		    if (curlen > 0) {
		        if (curlen > maxlen)
		            maxlen = curlen;
		        ++numlines;
		    }
		    if (numlines > 0) {
		        fseek(fp, 0, SEEK_SET);
		        numlines *= 2;
		        argv = new CharPtr[numlines+1];
		        for (int i=0; i<=numlines; i++)
		            argv[i] = null;
		        CharPtr tmp = new char[maxlen+1];
		        while (0==feof(fp) && (argc<numlines)) {
		        	CharPtr ptmp = new CharPtr(tmp);
		            if (fgets(ptmp,maxlen+1,fp) == null)
		                break;
		            curlen = (int)strlen(tmp);
		            while ((curlen > 0) && ((tmp[curlen-1] == '\n') || (tmp[curlen-1] == '\r'))) {
		                tmp[curlen-1] = '\0';
		                curlen = (int)strlen(tmp);
		            }
		            if (curlen == 0) continue;
		            if (ptmp[0] == '#') continue;
		            ptmp = strchr(tmp, '=');
		            if (ptmp != null) {
		            	ptmp[0] = '\0'; ptmp.inc();
		            	curlen = (int)strlen(tmp);
		                argv[argc] = new char[curlen+3];
		                sprintf(argv[argc], "--%s", tmp);
		                curlen = (int)strlen(ptmp);
		                argv[argc+1] = new char[curlen+1];
		                sprintf(argv[argc+1], "%s", ptmp);
		                //printf("Got option '%s'='%s'\n", argv[argc], argv[argc+1]);
		                argc += 2;
		            } else {
		                argv[argc] = new char[curlen+3];
		                sprintf(argv[argc], "--%s", tmp);
		                //printf("Got option '%s'\n", argv[argc]);
		                ++argc;
		            }
		        }
		        tmp = null;//delete [] tmp;
		    }
		    fclose(fp);
		
		    // now parse the options
		    if ((argv != null)) {
		        if ((argc > 1) && (argv[1] != null))
		            parseOptions(argc, argv, ons, ref hasArchivePath);
		        for (int i=0; i<=numlines; i++)
		            if (argv[i] != null)
		                argv[i] = null;//delete[] argv[i];
		        argv = null;//delete argv;
		        return true;
		    }
		    return false;
		}
		
//		#ifdef QWS
//		int SDL_main( int argc, char **argv )
//		#elif defined(PSP)
//		extern "C" int main( int argc, char **argv )
//		#elif !IS_LIB
//		int main_temp( int argc, char **argv )
//		#else
		int main( int argc, CharPtr[] argv )
//		#endif
		{
			ONScripterLabel ons = new ONScripterLabel();
		
		#if PSP
		    ons.disableRescale();
		    ons.enableButtonShortCut();
		#endif
		
		#if MACOSX
		    //Check for application bundle on Mac OS X
		    ons.checkBundled();
		#endif
		
		    // ----------------------------------------
		    // Parse options
		    bool hasArchivePath = false;
		#if MACOSX
		    if (ons.isBundled()) {
		        const int maxpath=32768;
		        char cfgpath[maxpath];
		        char *tmp = ons.bundleResPath();
		        if (tmp) {
		            sprintf(cfgpath, "%s/%s", tmp, CFG_FILE);
		            parseOptionFile(cfgpath, ons, hasArchivePath);
		        }
		        tmp = ons.bundleAppPath();
		        if (tmp) {
		            sprintf(cfgpath, "%s/%s", tmp, CFG_FILE);
		            parseOptionFile(cfgpath, ons, hasArchivePath);
		        }
		    } else
		#endif
		    parseOptionFile(CFG_FILE, ons, ref hasArchivePath);
		    parseOptions(argc, argv, ons, ref hasArchivePath);
		
		    // ----------------------------------------
		    // Run ONScripter
		
		    if (0!=ons.init()) exit(-1);
		    ons.executeLabel();
		    
		    exit(0);
			return 0;
		}

	}
}
