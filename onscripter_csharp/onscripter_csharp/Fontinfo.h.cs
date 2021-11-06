/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:09
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
		 *  FontInfo.h - Font information storage class of ONScripter
		 *
		 *  Copyright (c) 2001-2007 Ogapee. All rights reserved.
		 *
		 *  ogapee@aqua.dti2.ne.jp
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
		 *  along with this program; if not, write to the Free Software
		 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
		 */
		
		//#ifndef __FONT_INFO_H__
		//#define __FONT_INFO_H__
		
		//#include <SDL.h>
		
		//typedef unsigned char uchar3[3];
		
		// OS X pollutes the main namespace with its own FontInfo type, so we
		// have to use something else.
		public partial class Fontinfo {

		    public const int YOKO_MODE = 0;
		    public const int TATE_MODE = 1;
		    
		    public UnsignedCharPtr ttf_font;
		    public byte[] color = new Byte[3];
			public byte[] on_color = new Byte[3], off_color = new Byte[3], nofile_color = new Byte[3];
			public int[] font_size_xy = new int[2];
			public int[] top_xy = new int[2]; // Top left origin
			public int[] num_xy = new int[2]; // Row and column of the text windows
		    public int[] xy = new int[2]; // Current position
		    public int[] pitch_xy = new int[2]; // Width and height of a character
		    public int wait_time;
		    public bool is_bold;
		    public bool is_shadow;
		    public bool is_transparent;
		    public bool is_newline_accepted;
		    public byte[]  window_color = new byte[3];
		
		    public int[] line_offset_xy = new int[2]; // ruby offset for each line
		    public int[] ruby_offset_xy = new int[2]; // ruby offset for the whole sentence
		    public bool rubyon_flag;
		    public int tateyoko_mode;
		
//		    Fontinfo();
//		    void reset();
//		    void *openFont( char *font_file, int ratio1, int ratio2 );
//		    void setTateyokoMode( int tateyoko_mode );
//		    int getTateyokoMode();
//		    int getRemainingLine();
//		    
//		    int x();
//		    int y();
//		    void setXY( int x=-1, int y=-1 );
//		    void clear();
//		    void newLine();
//		    void setLineArea(int num);
//		
//		    bool isEndOfLine(int margin=0);
//		    bool isLineEmpty();
//		    void advanceCharInHankaku(int offest);
//		    void addLineOffset(int margin);
//		    void setRubyOnFlag(bool flag);
//		
//		    SDL_Rect calcUpdatedArea(int start_xy[2], int ratio1, int ratio2 );
//		    void addShadeArea(SDL_Rect &rect, int shade_distance[2] );
//		    int initRuby(Fontinfo &body_info, int body_count, int ruby_count);
		}
		
		//#endif // __FONT_INFO_H__
	}
}
