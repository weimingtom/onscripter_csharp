/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-25
 * Time: 9:29
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
		 *  FontInfo.cpp - Font information storage class of ONScripter-EN
		 *
		 *  Copyright (c) 2001-2007 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2008 "Uncle" Mion Sonozaki
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
		
//		#include "FontInfo.h"
//		#include <stdio.h>
//		#include <SDL_ttf.h>
		
		private class FontContainer{
		    public FontContainer next;
		    public int size;
		    public TTF_Font font;
		
		    public FontContainer(){
		        size = 0;
		        next = null;
		        font = null;
		    }
		};
		private static FontContainer root_font_container = new FontContainer();
		
		public partial class Fontinfo {
			public Fontinfo()
			{
//			    ttf_font = NULL;
//			
//			    color[0]        = color[1]        = color[2]        = 0xff;
//			    on_color[0]     = on_color[1]     = on_color[2]     = 0xff;
//			    off_color[0]    = off_color[1]    = off_color[2]    = 0xaa;
//			    nofile_color[0] = 0x55;
//			    nofile_color[1] = 0x55;
//			    nofile_color[2] = 0x99;
//			
//			    reset();
			}
			
			public void reset()
			{
//			    tateyoko_mode = YOKO_MODE;
//			    rubyon_flag = false;
//			    ruby_offset_xy[0] = ruby_offset_xy[1] = 0;
//			    clear();
//			
//			    is_bold = true;
//			    is_shadow = true;
//			    is_transparent = true;
//			    is_newline_accepted = false;
			}
			
			public object openFont( CharPtr font_file, int ratio1, int ratio2 )
			{
				return null;
//			    int font_size;
//			    if ( font_size_xy[0] < font_size_xy[1] )
//			        font_size = font_size_xy[0];
//			    else
//			        font_size = font_size_xy[1];
//			
//			    FontContainer *fc = &root_font_container;
//			    while( fc->next ){
//			        if ( fc->next->size == font_size ) break;
//			        fc = fc->next;
//			    }
//			    if ( !fc->next ){
//			        fc->next = new FontContainer();
//			        fc->next->size = font_size;
//			        FILE *fp = fopen( font_file, "r" );
//			        if ( fp == NULL ) return NULL;
//			        fclose( fp );
//			        fc->next->font = TTF_OpenFont( font_file, font_size * ratio1 / ratio2 );
//			    }
//			
//			    ttf_font = (void*)fc->next->font;
//			    
//			    return fc->next->font;
			}
			
			public void setTateyokoMode( int tateyoko_mode )
			{
//			    this->tateyoko_mode = tateyoko_mode;
//			    clear();
			}
			
			public int getTateyokoMode()
			{
				return 0;
//			    return tateyoko_mode;
			}
			
			public int getRemainingLine()
			{
				return 0;
//			    if (tateyoko_mode == YOKO_MODE)
//			        return num_xy[1] - xy[1]/2;
//			    else
//			        return num_xy[1] - num_xy[0] + xy[0]/2 + 1;
			}
			
			public int x()
			{
				return 0;
//			    return xy[0]*pitch_xy[0]/2 + top_xy[0] + line_offset_xy[0] + ruby_offset_xy[0];
			}
			
			public int y()
			{
				return 0;
//			    return xy[1]*pitch_xy[1]/2 + top_xy[1] + line_offset_xy[1] + ruby_offset_xy[1];
			}
			
			public void setXY( int x=-1, int y=-1 )
			{
//			    if ( x != -1 ) xy[0] = x*2;
//			    if ( y != -1 ) xy[1] = y*2;
			}
			
			public void clear()
			{
//			    if (tateyoko_mode == YOKO_MODE)
//			        setXY(0, 0);
//			    else
//			        setXY(num_xy[0]-1, 0);
//			    line_offset_xy[0] = line_offset_xy[1] = 0;
//			    setRubyOnFlag(rubyon_flag);
			}
			
			public void newLine()
			{
//			    if (tateyoko_mode == YOKO_MODE){
//			        xy[0] = 0;
//			        xy[1] += 2;
//			    }
//			    else{
//			        xy[0] -= 2;
//			        xy[1] = 0;
//			    }
//			    line_offset_xy[0] = line_offset_xy[1] = 0;
			}
			
			public void setLineArea(int num)
			{
//			    num_xy[tateyoko_mode] = num;
//			    num_xy[1-tateyoko_mode] = 1;
			}
			
			public bool isEndOfLine(int margin=0)
			{
				return false;
//			    if (pitch_xy[tateyoko_mode] == 0)
//			        return false;
//			
//			    int offset = 2 * line_offset_xy[tateyoko_mode] / pitch_xy[tateyoko_mode];
//			    if (((2 * line_offset_xy[tateyoko_mode]) % pitch_xy[tateyoko_mode]) > 0)
//			        offset++;
//			    if (xy[tateyoko_mode] + offset + margin >= num_xy[tateyoko_mode]*2)
//			        return true;
//			
//			    return false;
			}
			
			public bool isLineEmpty()
			{
				return false;
//			    if (xy[tateyoko_mode] == 0) return true;
//			
//			    return false;
			}
			
			public void advanceCharInHankaku(int offset)
			{
//			    xy[tateyoko_mode] += offset;
			}
			
			public void addLineOffset(int offset)
			{
//			    line_offset_xy[tateyoko_mode] += offset;
			}
			
			public void setRubyOnFlag(bool flag)
			{
//			    rubyon_flag = flag;
//			    ruby_offset_xy[0] = ruby_offset_xy[1] = 0;
//			    if (rubyon_flag && tateyoko_mode == TATE_MODE)
//				ruby_offset_xy[0] = font_size_xy[0]-pitch_xy[0];
//			    if (rubyon_flag && tateyoko_mode == YOKO_MODE)
//				ruby_offset_xy[1] = pitch_xy[1] - font_size_xy[1];
			}
			
			public SDL_Rect calcUpdatedArea(int[] start_xy, int ratio1, int ratio2)
			{
				return null;
//			    SDL_Rect rect;
//			    
//			    if (tateyoko_mode == YOKO_MODE){
//			        if (start_xy[1] == xy[1]){
//			            rect.x = top_xy[0] + pitch_xy[0]*start_xy[0]/2;
//			            rect.w = pitch_xy[0]*(xy[0]-start_xy[0])/2+1;
//			        }
//			        else{
//			            rect.x = top_xy[0];
//			            rect.w = pitch_xy[0]*num_xy[0];
//			        }
//			        rect.y = top_xy[1] + start_xy[1]*pitch_xy[1]/2;
//			        rect.h = pitch_xy[1]*(xy[1]-start_xy[1]+2)/2;
//			    }
//			    else{
//			        rect.x = top_xy[0] + pitch_xy[0]*xy[0]/2;
//			        rect.w = pitch_xy[0]*(start_xy[0]-xy[0]+2)/2;
//			        if (start_xy[0] == xy[0]){
//			            rect.y = top_xy[1] + pitch_xy[1]*start_xy[1]/2;
//			            rect.h = pitch_xy[1]*(xy[1]-start_xy[1])/2+1;
//			        }
//			        else{
//			            rect.y = top_xy[1];
//			            rect.h = pitch_xy[1]*num_xy[1];
//			        }
//			        num_xy[0] = (xy[0]-start_xy[0])/2+1;
//			    }
//			
//			    rect.x = rect.x * ratio1 / ratio2;
//			    rect.y = rect.y * ratio1 / ratio2;
//			    if (((rect.w*ratio1)%ratio2)==0)
//			        rect.w = rect.w * ratio1 / ratio2;
//			    else
//			        rect.w = rect.w * ratio1 / ratio2 + 1;
//			    if (((rect.h*ratio1)%ratio2)==0)
//			        rect.h = rect.h * ratio1 / ratio2;
//			    else
//			        rect.h = rect.h * ratio1 / ratio2 + 1;
//			    
//			    return rect;
			}
			
			public void addShadeArea(SDL_Rect rect, int[] shade_distance)
			{
//			    if (is_shadow){
//			        if (shade_distance[0]>0)
//			            rect.w += shade_distance[0];
//			        else{
//			            rect.x += shade_distance[0];
//			            rect.w -= shade_distance[0];
//			        }
//			        if (shade_distance[1]>0)
//			            rect.h += shade_distance[1];
//			        else{
//			            rect.y += shade_distance[1];
//			            rect.h -= shade_distance[1];
//			        }
//			    }
			}
			
			public int initRuby(Fontinfo body_info, int body_count, int ruby_count)
			{
				return 0;
//			    top_xy[0] = body_info.x();
//			    top_xy[1] = body_info.y();
//			    pitch_xy[0] = font_size_xy[0];
//			    pitch_xy[1] = font_size_xy[1];
//			
//			    int margin=0;
//			    
//			    if (tateyoko_mode == YOKO_MODE){
//			        top_xy[1] -= font_size_xy[1];
//			        num_xy[0] = ruby_count;
//			        num_xy[1] = 1;
//			    }
//			    else{
//			        top_xy[0] += body_info.font_size_xy[0];
//			        num_xy[0] = 1;
//			        num_xy[1] = ruby_count;
//			    }
//			    
//			    if (ruby_count*font_size_xy[tateyoko_mode] >= body_count*body_info.pitch_xy[tateyoko_mode]){
//			        margin = (ruby_count*font_size_xy[tateyoko_mode] - body_count*body_info.pitch_xy[tateyoko_mode] + 1)/2;
//			    }
//			    else{
//			        int offset = 0;
//			        if (ruby_count > 0) 
//			            offset = (body_count*body_info.pitch_xy[tateyoko_mode] - ruby_count*font_size_xy[tateyoko_mode]) / ruby_count;
//			        top_xy[tateyoko_mode] += (offset+1)/2;
//			        pitch_xy[tateyoko_mode] += offset;
//			    }
//			    //body_info.line_offset_xy[tateyoko_mode] += margin;
//			    
//			    clear();
//			
//			    return margin;
			}	
		}
	}
}
