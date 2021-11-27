/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 8:44
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
		 *  AnimationInfo.h - General image storage class of ONScripter-EN
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
		//#ifndef __ANIMATION_INFO_H__
		//#define __ANIMATION_INFO_H__
		
		//#include <SDL.h>
		//#include <SDL_image.h>
		//#include <string.h>
		//#include "BaseReader.h"
		
		//#if defined (USE_X86_GFX) && !defined(MACOSX)
		//#include <cpuid.h>
		//#endif
		
		private const int TRANSBTN_CUTOFF = 15; //alpha threshold for ignoring transparent areas
		
		//typedef unsigned char uchar3[3];
		
		//useful utility functions
//		inline bool equalStrings(const char *str1, const char *str2) {
//		    return ( ((str1 == NULL) && (str2 == NULL)) ||
//		             ((str1 != NULL) && (str2 != NULL) &&
//		              (str1[0] == str2[0]) && (strcmp(str1,str2) == 0)) );
//		}
//		
//		inline bool equalColors(const uchar3 &color1, const uchar3 &color2) {
//		    return ((color1[0] == color2[0]) && (color1[1] == color2[1]) &&
//		            (color1[2] == color2[2]));
//		}
		
		public partial class AnimationInfo{
//		public:
//		#ifdef BPP16
//		    typedef Uint16 ONSBuf;
//		#else
//		    typedef Uint32 ONSBuf;
//		#endif    
		    public const int TRANS_ALPHA          = 1;
		    public const int TRANS_TOPLEFT        = 2;
		    public const int TRANS_COPY           = 3;
		    public const int TRANS_STRING         = 4;
		    public const int TRANS_DIRECT         = 5;
		    public const int TRANS_PALLETTE       = 6;
		    public const int TRANS_TOPRIGHT       = 7;
		    public const int TRANS_MASK           = 8;
		#if !NO_LAYER_EFFECTS
		    public const int TRANS_LAYER          = 9;
		#endif
		    
		
		    public bool is_copy; // allocated buffers should not be deleted from a copied instance
		    public bool stale_image; //set to true when the image needs to be created/redone
		
		    public SDL_Rect orig_pos = new SDL_Rect(); //Mion: position and size of the image before resizing
		    public SDL_Rect pos = new SDL_Rect(); // position and size of the current cell
		
		    /* variables set from the image tag */
		    public int trans_mode;
		    public byte[] direct_color = new byte[3];
		    public int pallette_number;
		    public byte[] color = new byte[3];
		    public int num_of_cells;
		    public int current_cell;
		    public int direction;
		    public int[] duration_list;
		    public byte[][] color_list;
		    public int loop_mode;
		    public bool is_animatable;
		    public bool is_single_line;
		    public bool is_tight_region; // valid under TRANS_STRING
		    public bool is_ruby_drawable;
		    public bool skip_whitespace;
		#if !NO_LAYER_EFFECTS
		    public int layer_no; //Mion: for Layer effects
		#endif
		    public CharPtr file_name;
		    public CharPtr mask_file_name;
		
		    //Mion: for special graphics routine handling
		    
		    public const int CPUF_NONE           =  0;
		    public const int CPUF_X86_MMX        =  1;
		    public const int CPUF_X86_SSE        =  2;
		    public const int CPUF_X86_SSE2       =  4;
		    public const int CPUF_PPC_ALTIVEC    =  8;
		    
		
		    /* Variables from AnimationInfo */
		    public bool visible;
		    public bool abs_flag;
		    public bool affine_flag;
		    public int trans;
		    public CharPtr image_name;
		    public SDL_Surface image_surface;
		    public byte[] alpha_buf;
		    /* Variables for extended sprite (lsp2, drawsp2, etc.) */
		    public int scale_x, scale_y, rot;
		    public int[][] mat = mat_init(), inv_mat = mat_init();
		    private static int[][] mat_init() {
		    	int[][] ret = new int[2][];
		    	for (int i = 0; i < ret.Length; ++i)
		    	{
		    		ret[i] = new int[2];
		    	}
		    	return ret;
		    }
		    public int[][] corner_xy = corner_xy_init();
		    private static int[][] corner_xy_init() {
		    	int[][] ret = new int[4][];
		    	for (int i = 0; i < ret.Length; ++i)
		    	{
		    		ret[i] = new int[2];
		    	}
		    	return ret;
		    }
		    public SDL_Rect bounding_rect = new SDL_Rect();
		
		    public const int BLEND_NORMAL      = 0;
		    public const int BLEND_ADD         = 1;
		    public const int BLEND_SUB         = 2;
		    
		    public int blending_mode;
		    public int cos_i, sin_i;
		    
		    public int[] font_size_xy = new int[2]; // used by prnum and lsp string
		    public int font_pitch; // used by lsp string
		    public int remaining_time;
		
		    public int param; // used by prnum and bar
		    public int max_param; // used by bar
		    public int max_width; // used by bar
		    
//		    AnimationInfo();
//		    AnimationInfo(const AnimationInfo &anim);
//		    ~AnimationInfo();
//		
//		    AnimationInfo& operator =(const AnimationInfo &anim);
//		    void deepcopyTag(const AnimationInfo &anim);
//		    void deepcopy(const AnimationInfo &anim);
//		
//		    void reset();
//		    
//		    void deleteImageName();
//		    void setImageName( const char *name );
//		    void deleteImage();
//		    void remove();
//		    void removeTag();
//		
//		    bool proceedAnimation();
//		
//		    void setCell(int cell);
//		    static int doClipping( SDL_Rect *dst, SDL_Rect *clip, SDL_Rect *clipped=NULL );
//		    SDL_Rect findOpaquePoint(SDL_Rect *clip=NULL);
//		    int getPixelAlpha( int x, int y );
//		    void blendOnSurface( SDL_Surface *dst_surface, int dst_x, int dst_y,
//		                         SDL_Rect &clip, int alpha=256 );
//		    void blendOnSurface2( SDL_Surface *dst_surface, int dst_x, int dst_y,
//		                          SDL_Rect &clip, int alpha=256 );
//		    void blendText( SDL_Surface *surface, int dst_x, int dst_y,
//		                    SDL_Color &color, SDL_Rect *clip, bool rotate_flag );
//		    void calcAffineMatrix();
//		    
//		    static SDL_Surface *allocSurface( int w, int h );
//		    void allocImage( int w, int h );
//		    void copySurface( SDL_Surface *surface, SDL_Rect *src_rect, SDL_Rect *dst_rect = NULL );
//		    void fill( Uint8 r, Uint8 g, Uint8 b, Uint8 a );
//		    SDL_Surface *setupImageAlpha( SDL_Surface *surface, SDL_Surface *surface_m, bool has_alpha );
//		#ifdef RCA_SCALE
//		    SDL_Surface *resize( SDL_Surface *surface, int ratio1=1, int ratio2=1, float stretch_x=1.0, float stretch_y=1.0 );
//		#else
//		    SDL_Surface *resize( SDL_Surface *surface, int ratio1=1, int ratio2=1 );
//		#endif
//		    void setImage( SDL_Surface *surface );
//		    static void setCpufuncs(unsigned int func);
//		    static unsigned int getCpufuncs();
//		    static void imageFilterMean(unsigned char *src1, unsigned char *src2, unsigned char *dst, int length);
//		    static void imageFilterAddTo(unsigned char *dst, unsigned char *src, int length);
//		    static void imageFilterSubFrom(unsigned char *dst, unsigned char *src, int length);
//		    static void imageFilterBlend(Uint32 *dst_buffer, Uint32 *src_buffer, Uint8 *alphap, int alpha, int length);
//		
//		    //Mion: for resizing (moved from ONScripterLabel)
//		    static void resetResizeBuffer();
//		    static int resizeSurface( SDL_Surface *src, SDL_Surface *dst, int num_cells=1 );
		}
		
		//#endif // __ANIMATION_INFO_H__
	}
}
