﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-3
 * Time: 9:24
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
		 *  AnimationInfo.cpp - General image storage class of ONScripter-EN
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
		
		//#ifdef _MSC_VER
		//#pragma warning(disable:4244)
		//#endif
		
		//#include "AnimationInfo.h"
		//#include "BaseReader.h"
		
		//#include "graphics_common.h"
		
		//#if defined(USE_X86_GFX)
		//#include "graphics_mmx.h"
		//#include "graphics_sse2.h"
		//#endif
		
		//#if defined(USE_PPC_GFX)
		//#include "graphics_altivec.h"
		//#endif
		
		//#include <math.h>
		//#ifndef M_PI
		//#define M_PI 3.14159265358979323846
		private const double M_PI = 3.14159265358979323846;
		//#endif
		
		//Mion: for special graphics routine handling
		private static uint cpufuncs;
		
		//#if !defined(BPP16)
		public static bool is_inv_alpha_lut_initialized = false;
		private static UInt32[] inv_alpha_lut = new UInt32[256];
		//#endif
		
		
		public partial class AnimationInfo
		{
			public AnimationInfo()
				//Using an initialization list to make sure pointers start out NULL
			{ duration_list = (null); color_list = (null);
				file_name = (null); mask_file_name = (null); image_name = (null);
				image_surface = (null); alpha_buf = (null);
			//{
			    is_copy = false;
			    stale_image = true;

			    trans_mode = TRANS_TOPLEFT;
			    affine_flag = false;

				#if !BPP16
			    if (!is_inv_alpha_lut_initialized){
			        inv_alpha_lut[0] = 255;
			        for (int i=1; i<255; i++)
			            inv_alpha_lut[i] = (UInt32)(0xffff / i);
			        is_inv_alpha_lut_initialized = true;
			    }
				#endif

			    reset();
			}
			
			public AnimationInfo(AnimationInfo anim)
			{
				memcpy(this, anim, (uint)sizeof_AnimationInfo());
				is_copy = true;
			}
			
			~AnimationInfo()
			{
				if (!is_copy) reset();
			}
			
			public AnimationInfo /*operator =*/copy(AnimationInfo anim)
			{
			    if (this != anim){
					memcpy(this, anim, (uint)sizeof_AnimationInfo());
			        is_copy = true;
			    }
			    return this;
			}
			
			//deepcopy everything but the image surface
			public void deepcopyTag(AnimationInfo anim)
			{
			    if (this == anim) return;

			    //clear old stuff first
			    reset();
			    //copy the whole object
			    memcpy(this, anim, (uint)sizeof_AnimationInfo());
			    //unset the image_surface due to danger of accidental deletion
			    image_surface = null;
			    alpha_buf = null;

			    //now set dynamic variables
			    if (null!=anim.duration_list){
			        duration_list = new int[num_of_cells];
			        memcpy(duration_list, anim.duration_list,
			               (uint)(sizeof_int()*num_of_cells));
			    }
			    if (null!=anim.color_list){
			    	color_list = new byte[num_of_cells][];
			    	for (int i = 0; i < color_list.Length; ++i)
			    	{
			    		color_list[i] = new byte[3];
			    	}
			        memcpy(color_list, anim.color_list,
			               (uint)(sizeof_uchar3()*num_of_cells));
			    }
			    if (null!=anim.image_name){
			        image_name = new char[ strlen(anim.image_name) + 1 ];
			        strcpy( image_name, anim.image_name );
			    }
			    if (null!=anim.file_name){
			        file_name = new char[ strlen(anim.file_name) + 1 ];
			        strcpy( file_name, anim.file_name );
			    }
			    if (null!=anim.mask_file_name){
			        mask_file_name = new char[ strlen(anim.mask_file_name) + 1 ];
			        strcpy( mask_file_name, anim.mask_file_name );
			    }
			}
			
			public void deepcopy(AnimationInfo anim)
			{
			    if (this == anim) return;

			    deepcopyTag(anim);

			    if (null!=anim.image_surface){
			        int w = SDL_Surface_get_w(anim.image_surface), h = SDL_Surface_get_h(anim.image_surface);
			        allocImage( w, h );
			        copySurface(anim.image_surface, null);
				#if BPP16
			        if (anim.alpha_buf)
			            memcpy(alpha_buf, anim.alpha_buf, w*h);
				#endif
			    }
			}
			
			public void reset()
			{
			    remove();

			    trans = 256;
			    orig_pos.x = orig_pos.y = 0;
			    orig_pos.w = orig_pos.h = 0;
			    pos.x = pos.y = 0;
			    pos.w = pos.h = 0;
			    visible = false;
			    abs_flag = true;
			    scale_x = scale_y = rot = 0;
			    blending_mode = BLEND_NORMAL;

			    font_size_xy[0] = font_size_xy[1] = -1;
			    font_pitch = -1;

			    mat[0][0] = 1024;
			    mat[0][1] = 0;
			    mat[1][0] = 0;
			    mat[1][1] = 1024;

			#if !NO_LAYER_EFFECTS
			    layer_no = -1;
			#endif
			}
			
			public void deleteImageName(){
			    if ( !is_copy && null!=image_name ) image_name = null;//delete[] image_name;
			    image_name = null;
			}
			
			public void setImageName( CharPtr name ){
			    deleteImageName();
			    image_name = new char[ strlen(name) + 1 ];
			    strcpy( image_name, name );
			}
			
			public void deleteImage(){
			    if (!is_copy) {
			        if ( null!=image_surface ) SDL_FreeSurface( image_surface );
			#if BPP16
			        if (null!=alpha_buf) delete[] alpha_buf;
			#endif
			    }
			    image_surface = null;
			    alpha_buf = null;
			    stale_image = true;
			}
			
			public void remove(){
			    deleteImageName();
			    deleteImage();
			    removeTag();
			}
			
			public void removeTag(){
			    if (!is_copy) {
			        if ( null!=duration_list ) duration_list = null;//delete[] duration_list;
			        if ( null!=color_list ) color_list = null;//delete[] color_list;
			        if ( null!=file_name ) file_name = null;//delete[] file_name;
			        if ( null!=mask_file_name ) mask_file_name = null;//delete[] mask_file_name;
			    }
			    duration_list = null;
			    color_list = null;
			    file_name = null;
			    mask_file_name = null;
			    current_cell = 0;
			    num_of_cells = 0;
			    remaining_time = 0;
			    is_animatable = false;
			    is_single_line = true;
			    is_tight_region = true;
			    is_ruby_drawable = false;
			    skip_whitespace = true;

			    direction = 1;

			    color[0] = color[1] = color[2] = 0;
			}
			
			// 0 ... restart at the end
			// 1 ... stop at the end
			// 2 ... reverse at the end
			// 3 ... no animation
			public bool proceedAnimation()
			{
				bool is_changed = false;

			    if ( loop_mode != 3 && num_of_cells > 1 ){
			        current_cell += direction;
			        is_changed = true;
			    }

			    if ( current_cell < 0 ){ // loop_mode must be 2
			        current_cell = 1;
			        direction = 1;
			    }
			    else if ( current_cell >= num_of_cells ){
			        if ( loop_mode == 0 ){
			            current_cell = 0;
			        }
			        else if ( loop_mode == 1 ){
			            current_cell = num_of_cells - 1;
			            is_changed = false;
			        }
			        else{
			            current_cell = num_of_cells - 2;
			            direction = -1;
			        }
			    }

			    remaining_time = duration_list[ current_cell ];

			    return is_changed;
			}
			
			public void setCell(int cell)
			{
			    if (cell < 0) cell = 0;
			    else if (cell >= num_of_cells) cell = num_of_cells - 1;

			    current_cell = cell;
			}
			
			public static int doClipping( SDL_Rect dst, SDL_Rect clip, SDL_Rect clipped=null )
			{
			    if ( null!=clipped ) clipped.x = clipped.y = 0;

			    if ( null==dst ||
			         dst.x >= clip.x + clip.w || dst.x + dst.w <= clip.x ||
			         dst.y >= clip.y + clip.h || dst.y + dst.h <= clip.y )
			        return -1;

			    if ( dst.x < clip.x ){
			        dst.w -= clip.x - dst.x;
			        if ( null!=clipped ) clipped.x = clip.x - dst.x;
			        dst.x = clip.x;
			    }
			    if ( clip.x + clip.w < dst.x + dst.w ){
			        dst.w = clip.x + clip.w - dst.x;
			    }

			    if ( dst.y < clip.y ){
			        dst.h -= clip.y - dst.y;
			        if ( null!=clipped ) clipped.y = clip.y - dst.y;
			        dst.y = clip.y;
			    }
			    if ( clip.y + clip.h < dst.y + dst.h ){
			        dst.h = clip.y + clip.h - dst.y;
			    }
			    if ( null!=clipped ){
			        clipped.w = dst.w;
			        clipped.h = dst.h;
			    }

			    return 0;
			}
			
			public SDL_Rect findOpaquePoint(SDL_Rect clip)
				//find the first opaque-enough pixel position for transbtn
			{
			    int cell_width = SDL_Surface_get_w(image_surface) / num_of_cells;
			    SDL_Rect cliprect = new SDL_Rect(0, 0, cell_width, SDL_Surface_get_h(image_surface));
			    if (null!=clip) cliprect.copy(clip);

			#if BPP16
			    const int psize = 1;
			    unsigned char *alphap = alpha_buf;
			#else
			    const int psize = 4;
			    UnsignedCharPtr alphap = new UnsignedCharPtr(SDL_Surface_get_pixels(image_surface));
			#if true//SDL_BYTEORDER == SDL_LIL_ENDIAN
				alphap.inc(3);
			#endif
			#endif

				SDL_Rect ret = new SDL_Rect(0, 0, 0, 0);

			    for (int i=cliprect.y ; i<cliprect.h ; ++i){
			        for (int j=cliprect.x ; j<cliprect.w ; ++j){
						int alpha = (new UnsignedCharPtr(alphap, + (SDL_Surface_get_w(image_surface) * i + j) * psize))[0];
			            if (alpha > TRANSBTN_CUTOFF){
			                ret.x = j;
			                ret.y = i;
			                //want to break out of the for loops
			                i = cliprect.h;
			                break;
			            }
			        }
			    }
			    //want to find a pixel that's opaque across all cells, if possible
			    int xstart = ret.x;
			#if _MSC_VER
				{
			#endif
			    for (int i=ret.y ; i<cliprect.h ; ++i){
			        for (int j=xstart ; j<cliprect.w ; ++j){
			            bool is_opaque = true;
			            for (int k=0 ; k<num_of_cells ; ++k){
			            	int alpha = (new UnsignedCharPtr(alphap, + (SDL_Surface_get_w(image_surface) * i + cell_width * k + j) * psize))[0];
			                if (alpha <= TRANSBTN_CUTOFF){
			                    is_opaque = false;
			                    break;
			                }
			            }
			            if (is_opaque){
			                ret.x = j;
			                ret.y = i;
			                //want to break out of the for loops
			                i = cliprect.h;
			                break;
			            }
			            xstart = cliprect.x;
			        }
			    }
			#if _MSC_VER
				}
			#endif

			    return ret;
			}
			
			public int getPixelAlpha( int x, int y )
			{
			#if BPP16
			    unsigned char *alphap = alpha_buf + image_surface->w * y + x +
			                            image_surface->w*current_cell/num_of_cells;
			#else
			    int psize = 4;
			    int total_width = SDL_Surface_get_w(image_surface) * psize;
			    UnsignedCharPtr alphap = new UnsignedCharPtr(SDL_Surface_get_pixels(image_surface), +
			                            total_width * current_cell/num_of_cells +
			                            total_width * y + x * psize);
			#if true//SDL_BYTEORDER == SDL_LIL_ENDIAN
				alphap.inc(3);
			#endif
			#endif
			return (int) alphap[0];
			}
			
			
			public void blendOnSurface( SDL_Surface dst_surface, int dst_x, int dst_y,
			                           ref SDL_Rect clip, int alpha=256 )
			{
			    if ( image_surface == null ) return;

			    SDL_Rect dst_rect = new SDL_Rect(dst_x, dst_y, pos.w, pos.h), src_rect = new SDL_Rect();
			    if ( 0!=doClipping( dst_rect, clip, src_rect ) ) return;

			    /* ---------------------------------------- */

			    SDL_LockSurface( dst_surface );
			    SDL_LockSurface( image_surface );

			#if BPP16
			    const int total_width = SDL_Surface_get_pitch(image_surface) / 2;
			#else
			    int total_width = SDL_Surface_get_pitch(image_surface) / 4;
			#endif
				Uint32Ptr src_buffer = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), + total_width * src_rect.y + SDL_Surface_get_w(image_surface) * current_cell/num_of_cells + src_rect.x);
				Uint32Ptr dst_buffer = new Uint32Ptr(SDL_Surface_get_pixels(dst_surface), + SDL_Surface_get_w(dst_surface) * dst_rect.y + dst_rect.x);
			#if BPP16
			    unsigned char *alphap = alpha_buf + image_surface->w * src_rect.y + image_surface->w*current_cell/num_of_cells + src_rect.x;
			#else
			#if true//SDL_BYTEORDER == SDL_LIL_ENDIAN
				UnsignedCharPtr alphap = new UnsignedCharPtr(src_buffer, + 3);
			#else
			    unsigned char *alphap = (unsigned char *)src_buffer;
			#endif
			#endif

			    if (blending_mode == BLEND_NORMAL) {
			        if ((trans_mode == TRANS_COPY) && (alpha == 256)) {
			            Uint32Ptr srcmax = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
					                                 SDL_Surface_get_w(image_surface) * SDL_Surface_get_h(image_surface));

			            for (int i=dst_rect.h ; i!=0 ; i--){
							for (int j=dst_rect.w ; j!=0 ; j--, src_buffer.inc(), dst_buffer.inc()){
			                    // If we've run out of source area, ignore the remainder.
			                    if (Uint32Ptr.isLargerEqual(src_buffer, srcmax)) goto break2;
			                    SET_PIXEL(src_buffer[0], 0xff);
			                }
							src_buffer.inc(total_width - dst_rect.w);
			#if BPP16
			                alphap += image_surface->w - dst_rect.w;
			#else
							alphap.inc((SDL_Surface_get_w(image_surface) - dst_rect.w) * 4);
			#endif
							dst_buffer.inc(SDL_Surface_get_w(dst_surface)  - dst_rect.w);
			            }
			        } else if (alpha != 0) {
			            Uint32Ptr srcmax = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
					                                 SDL_Surface_get_w(image_surface) * SDL_Surface_get_h(image_surface));

			            for (int i=dst_rect.h ; i!=0 ; i--){
			#if BPP16
			                for (int j=dst_rect.w ; j!=0 ; j--, src_buffer++, dst_buffer++){
			                    // If we've run out of source area, ignore the remainder.
			                    if (src_buffer >= srcmax) goto break2;
			                    BLEND_PIXEL();
			                }
			                src_buffer += total_width - dst_rect.w;
			                dst_buffer += dst_surface->w - dst_rect.w;
			                alphap += image_surface->w - dst_rect.w;
			#else
							if (Uint32Ptr.isLargerEqual(src_buffer, srcmax)) goto break2;
			#if false//!_MSC_VER
			                imageFilterBlend(dst_buffer, src_buffer, alphap, alpha, dst_rect.w);
			#else
							imageFilterBlend(dst_buffer, src_buffer, new Uint8Ptr(alphap), alpha, dst_rect.w);
			#endif
							src_buffer.inc(total_width);
			               	dst_buffer.inc(SDL_Surface_get_w(dst_surface));
			                alphap.inc((SDL_Surface_get_w(image_surface)) * 4);
			#endif
			            }
			        }
			    } else if (blending_mode == BLEND_ADD) {
			#if !BPP16
			        if ((trans_mode == TRANS_COPY) && (alpha == 256)) {
			            // "add" the src pix value to the dst
			            Uint8Ptr srcmax = new Uint8Ptr(new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
			                                           SDL_Surface_get_w(image_surface) * SDL_Surface_get_h(image_surface)));
			            Uint8Ptr src_buf = new Uint8Ptr(src_buffer);
			            Uint8Ptr dst_buf = new Uint8Ptr(dst_buffer);

			            for (int i=dst_rect.h ; i!=0 ; i--){
			            	if (Uint8Ptr.isLargerEqual(src_buf, srcmax)) goto break2;
			#if false//!_MSC_VER
			                imageFilterAddTo(dst_buf, src_buf, dst_rect.w*4);
			#else
							imageFilterAddTo(new UnsignedCharPtr(dst_buf), new UnsignedCharPtr(src_buf), dst_rect.w*4);
			#endif
							src_buf.inc(total_width * 4);
			            	dst_buf.inc(SDL_Surface_get_w(dst_surface) * 4);
			            }
			        } else
			#endif
			        if (alpha != 0) {
			            // gotta do additive alpha blending
			            Uint32Ptr srcmax = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
			                                             SDL_Surface_get_w(image_surface) * SDL_Surface_get_h(image_surface));

			            for (int i=dst_rect.h ; i!=0 ; i--){
			                for (int j=dst_rect.w ; j!=0 ; j--, src_buffer.inc(), dst_buffer.inc()){
			                    // If we've run out of source area, ignore the remainder.
			                    if (Uint32Ptr.isLargerEqual(src_buffer, srcmax)) goto break2;
			                    ADDBLEND_PIXEL();
			                }
			            	src_buffer.inc(total_width - dst_rect.w);
			#if BPP16
			                alphap += image_surface->w - dst_rect.w;
			#else
							alphap.inc((SDL_Surface_get_w(image_surface) - dst_rect.w) * 4);
			#endif
							dst_buffer.inc(SDL_Surface_get_w(dst_surface)  - dst_rect.w);
			            }
			        }
			    } else if (blending_mode == BLEND_SUB) {
			#if !BPP16
			        if ((trans_mode == TRANS_COPY) && (alpha == 256)) {
			            // "subtract" the src pix value from the dst
			            Uint8Ptr srcmax = new Uint8Ptr(new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
			                                                         SDL_Surface_get_w(image_surface) * SDL_Surface_get_h(image_surface)));
			            Uint8Ptr src_buf = new Uint8Ptr(src_buffer);
			            Uint8Ptr dst_buf = new Uint8Ptr(dst_buffer);

			            for (int i=dst_rect.h ; i!=0 ; i--){
			            	if (Uint8Ptr.isLargerEqual(src_buf, srcmax)) goto break2;
			#if false//!_MSC_VER
			                imageFilterSubFrom(dst_buf, src_buf, dst_rect.w*4);
			#else
							imageFilterSubFrom(new UnsignedCharPtr(dst_buf), new UnsignedCharPtr(src_buf), dst_rect.w*4);
			#endif
							src_buf.inc(total_width * 4);
			            	dst_buf.inc(SDL_Surface_get_w(dst_surface) * 4);
			            }
			        } else
			#endif
			        if (alpha != 0) {
			            // gotta do subtractive alpha blending
			            Uint32Ptr srcmax = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
			                                             SDL_Surface_get_w(image_surface) * SDL_Surface_get_h(image_surface));

			            for (int i=dst_rect.h ; i!=0 ; i--){
			            	for (int j=dst_rect.w ; j!=0 ; j--, src_buffer.inc(), dst_buffer.inc()){
			                    // If we've run out of source area, ignore the remainder.
			                    if (Uint32Ptr.isLargerEqual(src_buffer, srcmax)) goto break2;
			                    SUBBLEND_PIXEL();
			                }
			            	src_buffer.inc(total_width - dst_rect.w);
			#if BPP16
			                alphap += SDL_Surface_get_w(image_surface) - dst_rect.w;
			#else
							alphap.inc((SDL_Surface_get_w(image_surface) - dst_rect.w) * 4);
			#endif
							dst_buffer.inc(SDL_Surface_get_w(dst_surface) - dst_rect.w);
			            }
			        }
			    }

			break2:
			    SDL_UnlockSurface( image_surface );
			    SDL_UnlockSurface( dst_surface );
			}
			
			public void blendOnSurface2( SDL_Surface dst_surface, int dst_x, int dst_y,
			                            ref SDL_Rect clip, int alpha=256 )
			{
			    if ( image_surface == null ) return;
			    if (scale_x == 0 || scale_y == 0) return;

			    int i, x=0, y=0;

			    // project corner point and calculate bounding box
			    int[] min_xy = new int[2]{bounding_rect.x, bounding_rect.y};
			    int[] max_xy = new int[2]{bounding_rect.x+bounding_rect.w-1,
			                   bounding_rect.y+bounding_rect.h-1};

			    // clip bounding box
			    if (max_xy[0] < clip.x) return;
			    if (max_xy[0] >= (clip.x + clip.w)) max_xy[0] = clip.x + clip.w - 1;
			    if (min_xy[0] >= (clip.x + clip.w)) return;
			    if (min_xy[0] < clip.x) min_xy[0] = clip.x;
			    if (max_xy[1] < clip.y) return;
			    if (max_xy[1] >= (clip.y + clip.h)) max_xy[1] = clip.y + clip.h - 1;
			    if (min_xy[1] >= (clip.y + clip.h)) return;
			    if (min_xy[1] < clip.y) min_xy[1] = clip.y;

			    if (min_xy[1] < 0)               min_xy[1] = 0;
			    if (max_xy[1] >= SDL_Surface_get_h(dst_surface)) max_xy[1] = SDL_Surface_get_h(dst_surface) - 1;

			    SDL_LockSurface( dst_surface );
			    SDL_LockSurface( image_surface );

			#if BPP16
			    int total_width = SDL_Surface_get_pitch(image_surface) / 2;
			#else
			    int total_width = SDL_Surface_get_pitch(image_surface) / 4;
			#endif
			    // set pixel by inverse-projection with raster scan
			    for (y=min_xy[1] ; y<= max_xy[1] ; y++){
			        // calculate the start and end point for each raster scan
			        int raster_min = min_xy[0], raster_max = max_xy[0];
			        for (i=0 ; i<4 ; i++){
			            int i2 = (i+1)&3; // = (i+1)%4
			            if (corner_xy[i][1] == corner_xy[i2][1]) continue;
			            x = (corner_xy[i2][0] - corner_xy[i][0])*(y-corner_xy[i][1])/(corner_xy[i2][1] - corner_xy[i][1]) + corner_xy[i][0];
			            if (scale_x*scale_y*(corner_xy[i2][1] - corner_xy[i][1]) > 0){
			                if (raster_min < x) raster_min = x;
			            }
			            else{
			                if (raster_max > x) raster_max = x;
			            }
			        }

			        if (raster_min < 0)               raster_min = 0;
			        if (raster_max >= SDL_Surface_get_w(dst_surface)) raster_max = SDL_Surface_get_w(dst_surface) - 1;

			        Uint32Ptr dst_buffer = new Uint32Ptr(SDL_Surface_get_pixels(dst_surface), + SDL_Surface_get_w(dst_surface) * y + raster_min);

			        // inverse-projection
			        int x_offset2 = (inv_mat[0][1] * (y-dst_y) >> 9) + pos.w;
			        int y_offset2 = (inv_mat[1][1] * (y-dst_y) >> 9) + pos.h;
			        for (x=raster_min-dst_x ; x<=raster_max-dst_x ; x++, dst_buffer.inc()){
			            int x2 = ((inv_mat[0][0] * x >> 9) + x_offset2) / 2;
			            int y2 = ((inv_mat[1][0] * x >> 9) + y_offset2) / 2;

			            if (x2 < 0 || x2 >= pos.w ||
			                y2 < 0 || y2 >= pos.h) continue;

			            Uint32Ptr src_buffer = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), + total_width * y2 + x2 + pos.w*current_cell);
			#if BPP16
			            unsigned char *alphap = alpha_buf + image_surface->w * y2 + x2 + pos.w*current_cell;
			#else
			#if true//SDL_BYTEORDER == SDL_LIL_ENDIAN
						UnsignedCharPtr alphap = new UnsignedCharPtr(src_buffer, + 3);
			#else
			            unsigned char *alphap = (unsigned char *)src_buffer;
			#endif
			#endif
			            if (blending_mode == BLEND_NORMAL) {
			                if ((trans_mode == TRANS_COPY) && (alpha == 256)) {
								SET_PIXEL(src_buffer[0], 0xff);
			                } else {
			                    BLEND_PIXEL();
			                }
			            } else if (blending_mode == BLEND_ADD) {
			                ADDBLEND_PIXEL();
			            } else if (blending_mode == BLEND_SUB) {
			                SUBBLEND_PIXEL();
			            }
			        }
			    }

			    // unlock surface
			    SDL_UnlockSurface( image_surface );
			    SDL_UnlockSurface( dst_surface );
			}

			// used to draw characters on text_surface
			// Alpha = 1 - (1-Da)(1-Sa)
			// Color = (DaSaSc + Da(1-Sa)Dc + Sa(1-Da)Sc)/A
			public void blendText( SDL_Surface surface, int dst_x, int dst_y,
			                               ref SDL_Color color, SDL_Rect clip,
			                               bool rotate_flag )
			{
			    if (image_surface == null || surface == null) return;

			    SDL_Rect dst_rect = new SDL_Rect(dst_x, dst_y, SDL_Surface_get_w(surface), SDL_Surface_get_h(surface));
			    if (rotate_flag){
			        dst_rect.w = SDL_Surface_get_h(surface);
			        dst_rect.h = SDL_Surface_get_w(surface);
			    }
				SDL_Rect src_rect = new SDL_Rect(0, 0, 0, 0);
				SDL_Rect clipped_rect = new SDL_Rect();

			    /* ---------------------------------------- */
			    /* 1st clipping */
			    if ( null!=clip ){
			        if ( 0!=doClipping( dst_rect, clip, clipped_rect ) ) return;

			        src_rect.x += clipped_rect.x;
			        src_rect.y += clipped_rect.y;
			    }

			    /* ---------------------------------------- */
			    /* 2nd clipping */
			    SDL_Rect clip_rect = new SDL_Rect(0, 0, SDL_Surface_get_w(image_surface), SDL_Surface_get_h(image_surface));
			    if ( 0!=doClipping( dst_rect, clip_rect, clipped_rect ) ) return;

			    src_rect.x += clipped_rect.x;
			    src_rect.y += clipped_rect.y;

			    /* ---------------------------------------- */

			    SDL_LockSurface( surface );
			    SDL_LockSurface( image_surface );

			#if BPP16
			    int total_width = image_surface->pitch / 2;
			    Uint32 src_color = ((color.r >> RLOSS) << RSHIFT) |
			                       ((color.g >> GLOSS) << GSHIFT) |
			                       (color.b >> BLOSS);
			    src_color = (src_color | src_color << 16) & BLENDMASK;
			#else
			    int total_width = SDL_Surface_get_pitch(image_surface) / 4;
			    UInt32 src_color1 = (UInt32)(color.r << RSHIFT | color.b);
			    UInt32 src_color2 = (UInt32)(color.g << GSHIFT);
			    UInt32 src_color3 = src_color1 | src_color2 | AMASK;
			#endif
			    Uint32Ptr dst_buffer = new Uint32Ptr(SDL_Surface_get_pixels(image_surface), +
			                         total_width * dst_rect.y +
			                         SDL_Surface_get_w(image_surface) * current_cell / num_of_cells +
			                         dst_rect.x);
			#if BPP16
			    unsigned char *alphap = alpha_buf + image_surface->w * dst_rect.y +
			                            image_surface->w*current_cell/num_of_cells +
			                            dst_rect.x;
			#endif
			    if (!rotate_flag){
			        UnsignedCharPtr src_buffer = new UnsignedCharPtr(SDL_Surface_get_pixels(surface), +
				                                                 SDL_Surface_get_pitch(surface) * src_rect.y + src_rect.x);
			        for ( int i=dst_rect.h ; i!=0 ; i-- ){
						for ( int j=dst_rect.w ; j!=0 ; j--, dst_buffer.inc(), src_buffer.inc() ){
			                BLEND_TEXT_ALPHA();
			            }
						dst_buffer.inc(total_width - dst_rect.w);
			#if BPP16
			            alphap += SDL_Surface_get_w(image_surface) - dst_rect.w;
			#endif
						src_buffer.inc(SDL_Surface_get_pitch(surface) - dst_rect.w);
			        }
			    }
			    else{
			        for ( int i=0 ; i<dst_rect.h ; i++ ){
			            UnsignedCharPtr src_buffer = new UnsignedCharPtr(SDL_Surface_get_pixels(surface), +
			                                        SDL_Surface_get_pitch(surface) * (SDL_Surface_get_h(surface) - src_rect.x - 1) +
			                                        src_rect.y + i);
						for ( int j=dst_rect.w ; j!=0 ; j--, dst_buffer.inc() ){
			                BLEND_TEXT_ALPHA();
			                src_buffer.inc(-SDL_Surface_get_pitch(surface));
			            }
						dst_buffer.inc(total_width - dst_rect.w);
			#if BPP16
			            alphap += SDL_Surface_get_w(image_surface) - dst_rect.w;
			#endif
			        }
			    }

			    SDL_UnlockSurface( image_surface );
			    SDL_UnlockSurface( surface );
			}
			
			//Mion - ogapee2008
			public void calcAffineMatrix()
			{
			    // calculate forward matrix
			    // |mat[0][0] mat[0][1]|
			    // |mat[1][0] mat[1][1]|
			    int cos_i = 1024, sin_i = 0;
			    if (rot != 0){
			        cos_i = (int)(1024.0 * cos(-M_PI*rot/180));
			        sin_i = (int)(1024.0 * sin(-M_PI*rot/180));
			    }
			    mat[0][0] =  cos_i*scale_x/100;
			    mat[0][1] = -sin_i*scale_y/100;
			    mat[1][0] =  sin_i*scale_x/100;
			    mat[1][1] =  cos_i*scale_y/100;

			    // calculate bounding box
			    int[] min_xy = new int[]{ 0, 0 }, max_xy = new int[]{ 0, 0 };
			    for (int i=0 ; i<4 ; i++){
			        //Mion: need to make sure corners are in the right order
			        //(UL,LL,LR,UR of the original image)
			        int c_x = (i<2)?(-pos.w/2):(pos.w/2);
			        int c_y = (0!=((i+1)&2))?(pos.h/2):(-pos.h/2);
			        if (scale_x < 0) c_x = -c_x;
			        if (scale_y < 0) c_y = -c_y;
			        corner_xy[i][0] = (mat[0][0] * c_x + mat[0][1] * c_y) / 1024 + pos.x;
			        corner_xy[i][1] = (mat[1][0] * c_x + mat[1][1] * c_y) / 1024 + pos.y;

			        if (i==0 || min_xy[0] > corner_xy[i][0]) min_xy[0] = corner_xy[i][0];
			        if (i==0 || max_xy[0] < corner_xy[i][0]) max_xy[0] = corner_xy[i][0];
			        if (i==0 || min_xy[1] > corner_xy[i][1]) min_xy[1] = corner_xy[i][1];
			        if (i==0 || max_xy[1] < corner_xy[i][1]) max_xy[1] = corner_xy[i][1];
			    }

			    bounding_rect.x = min_xy[0];
			    bounding_rect.y = min_xy[1];
			    bounding_rect.w = max_xy[0]-min_xy[0]+1;
			    bounding_rect.h = max_xy[1]-min_xy[1]+1;

			    // calculate inverse matrix
			    int denom = scale_x*scale_y;
			    if (denom == 0) return;

			    inv_mat[0][0] =  mat[1][1] * 10000 / denom;
			    inv_mat[0][1] = -mat[0][1] * 10000 / denom;
			    inv_mat[1][0] = -mat[1][0] * 10000 / denom;
			    inv_mat[1][1] =  mat[0][0] * 10000 / denom;
			}
			
			public static SDL_Surface allocSurface( int w, int h )
			{
				return SDL_CreateRGBSurface(SDL_SWSURFACE, w, h, BPP, RMASK, GMASK, BMASK, AMASK);
			}
			
			public void allocImage( int w, int h )
			{
			    if (null==image_surface ||
			        SDL_Surface_get_w(image_surface) != w ||
			        SDL_Surface_get_h(image_surface) != h){
			        deleteImage();

			        image_surface = allocSurface( w, h );
			#if BPP16
			        if (image_surface)
			            alpha_buf = new unsigned char[w * h];
			#endif
			    }

			    abs_flag = true;
			    pos.w = w / num_of_cells;
			    pos.h = h;
			}
			
			public void copySurface( SDL_Surface surface, SDL_Rect src_rect, SDL_Rect dst_rect = null )
			{
			    if (null==image_surface || null==surface) return;

			    SDL_Rect _dst_rect = new SDL_Rect(0, 0, SDL_Surface_get_w(image_surface), SDL_Surface_get_h(image_surface));
			    if (null!=dst_rect) _dst_rect.copy(dst_rect);

			    SDL_Rect _src_rect = new SDL_Rect(0, 0, SDL_Surface_get_w(surface), SDL_Surface_get_h(surface));
			    if (null!=src_rect) _src_rect.copy(src_rect);

			    if (_src_rect.x >= SDL_Surface_get_w(surface)) return;
			    if (_src_rect.y >= SDL_Surface_get_h(surface)) return;

			    if (_src_rect.x+_src_rect.w >= SDL_Surface_get_w(surface))
			        _src_rect.w = SDL_Surface_get_w(surface) - _src_rect.x;
			    if (_src_rect.y+_src_rect.h >= SDL_Surface_get_h(surface))
			        _src_rect.h = SDL_Surface_get_h(surface) - _src_rect.y;

			    if (_dst_rect.x+_src_rect.w > SDL_Surface_get_w(image_surface))
			        _src_rect.w = SDL_Surface_get_w(image_surface) - _dst_rect.x;
			    if (_dst_rect.y+_src_rect.h > SDL_Surface_get_h(image_surface))
			        _src_rect.h = SDL_Surface_get_h(image_surface) - _dst_rect.y;

			    SDL_LockSurface( surface );
			    SDL_LockSurface( image_surface );

			    int i;
			    for (i=0 ; i<_src_rect.h ; i++)
			    	memcpy( new Uint32Ptr(new UnsignedCharPtr(SDL_Surface_get_pixels(image_surface), + SDL_Surface_get_pitch(image_surface) * (_dst_rect.y + i)), + _dst_rect.x),
			    	       new Uint32Ptr(new UnsignedCharPtr(SDL_Surface_get_pixels(surface), + SDL_Surface_get_pitch(surface) * (_src_rect.y + i)), + _src_rect.x),
			    	       (uint)(_src_rect.w * sizeof_ONSBuf()) );
			#if BPP16
			    for (i=0 ; i<_src_rect.h ; i++)
			        memset( alpha_buf + SDL_Surface_get_w(image_surface) * (_dst_rect.y+i) + _dst_rect.x, 0xff, _src_rect.w );
			#endif

			    SDL_UnlockSurface( image_surface );
			    SDL_UnlockSurface( surface );
			}
			
			public static int _test_time = 0; //FIXME:not used
			public void fill( Byte r, Byte g, Byte b, Byte a )
			{
				//static int _test_time = 0;

			    if (null==image_surface) return;


			#if true
			    SDL_LockSurface( image_surface );
			    Uint32Ptr dst_buffer = new Uint32Ptr(SDL_Surface_get_pixels(image_surface));

			#if BPP16
			    Uint32 rgb = ((r>>RLOSS) << RSHIFT) | ((g>>GLOSS) << GSHIFT) | (b>>BLOSS);
			    unsigned char *alphap = alpha_buf;
			    int dst_margin = image_surface->w % 2;
			#else
				UInt32 rgb = (UInt32)((r << RSHIFT) | (g << GSHIFT) | b);
			#if false
				/*test code*/
				int test_time = _test_time;
				//if (test_time == 4) /*0,1,2,(3),4*/
				if (0)//if (a == 0)
				{
					//test_time == 2 || test_time == 1 || test_time == 0 || test_time == 3 ||
					rgb = 0xffffff;
					a = 0xff;
				}
				_test_time++;
			#endif

			#if true//SDL_BYTEORDER == SDL_LIL_ENDIAN
				UnsignedCharPtr alphap = new UnsignedCharPtr(dst_buffer, + 3);
			#else
			    unsigned char *alphap = (unsigned char *)dst_buffer;
			#endif
			    int dst_margin = 0;
			#endif

			    for (int i = SDL_Surface_get_h(image_surface) ; i != 0 ; i--) {
					for (int j = SDL_Surface_get_w(image_surface) ; j != 0 ; j--, dst_buffer.inc())
			#if true
			            SET_PIXEL(rgb, a);
			#else
						/*test code*/
						//*dst_buffer=0xffff0000; //SET_PIXEL(rgb, a);
						{
							*dst_buffer = rgb;//0x00ffffff;
							*alphap = a;
							alphap += 4;
						}
			#endif
					dst_buffer.inc(dst_margin);
			    }
			    SDL_UnlockSurface(image_surface);





			#else //test
				if (a != 0) {
					SDL_FillRect(image_surface, NULL, 0xffff0000);
				} else {
					SDL_FillRect(image_surface, NULL, 0x0000ff00);
					//SDL_FillRect(image_surface, NULL, 0xffff0000);
				}
			#endif
			}
			
			public SDL_Surface setupImageAlpha( SDL_Surface surface,
			                                   SDL_Surface surface_m,
			                                   bool has_alpha )
			{
				if (surface == null) return null;

			    SDL_LockSurface( surface );
			    Uint32Ptr buffer = new Uint32Ptr(SDL_Surface_get_pixels(surface));
			    SDL_PixelFormat fmt = SDL_Surface_get_format(surface);

			    int w = SDL_Surface_get_w(surface);
			    int h = SDL_Surface_get_h(surface);
			    int w2 = w / num_of_cells;
			    orig_pos.w = w;
			    orig_pos.h = h;

			#if SDL_BYTEORDER == SDL_LIL_ENDIAN
				UnsignedCharPtr alphap = new UnsignedCharPtr(buffer, + 3);
			#else
			    unsigned char *alphap = (unsigned char *)buffer;
			#endif

			    UInt32 ref_color=0;
			    if ( trans_mode == TRANS_TOPLEFT ){
			    	ref_color = buffer[0];
			    }
			    else if ( trans_mode == TRANS_TOPRIGHT ){
			    	ref_color = (new Uint32Ptr(buffer, + SDL_Surface_get_w(surface) - 1))[0];
			    }
			    else if ( trans_mode == TRANS_DIRECT ) {
			    	ref_color = (uint)(direct_color[0] << fmt.Rshift |
			            direct_color[1] << fmt.Gshift |
			            direct_color[2] << fmt.Bshift);
			    }
			    ref_color &= RGBMASK;

			    int i, j, c;
			    if ( trans_mode == TRANS_ALPHA && !has_alpha ){
			        int w22 = w2/2;
			        int w3 = w22 * num_of_cells;
			        orig_pos.w = w3;
			        SDL_PixelFormat fmt_ = SDL_Surface_get_format(surface);
			        SDL_Surface surface2 = SDL_CreateRGBSurface(SDL_SWSURFACE, w3, h, fmt_.BitsPerPixel, fmt_.Rmask, fmt_.Gmask, fmt_.Bmask, fmt_.Amask);
			        SDL_LockSurface( surface2 );
			        Uint32Ptr buffer2 = new Uint32Ptr(SDL_Surface_get_pixels(surface2));

			#if SDL_BYTEORDER == SDL_LIL_ENDIAN
					alphap = new UnsignedCharPtr(buffer2, + 3);
			#else
			        alphap = (unsigned char *)buffer2;
			#endif
			        for (i=h ; i!=0 ; i--){
			            for (c=num_of_cells ; c!=0 ; c--){
							for (j=w22 ; j!=0 ; j--, buffer.inc(), alphap.inc(4)){
								buffer2[0] = buffer[0]; buffer2.inc();
								alphap[0] = (byte)((new Uint32Ptr(buffer, + w22)[0] & 0xff) ^ 0xff);
			                }
							buffer.inc((w2 - w22));
			            }
						buffer.inc(SDL_Surface_get_w(surface) - w2 * num_of_cells);
			            buffer2.inc(SDL_Surface_get_w(surface2) - w22 * num_of_cells);
			            alphap.inc((SDL_Surface_get_w(surface2) - w22 * num_of_cells) * 4);
			        }

			        SDL_UnlockSurface( surface );
			        SDL_FreeSurface( surface );
			        surface = surface2;
			    }
			    else if ( trans_mode == TRANS_MASK ){
			        if (null!=surface_m){
			            //apply mask (replacing existing alpha values, if any)
			            SDL_LockSurface( surface_m );
			            int mw  = SDL_Surface_get_w(surface_m);
			            int mwh = SDL_Surface_get_w(surface_m) * SDL_Surface_get_h(surface_m);
			            int cw  = mw / num_of_cells;
			            int i2 = 0;
			            for (i=h ; i!=0 ; i--){
			            	Uint32Ptr buffer_m = new Uint32Ptr(SDL_Surface_get_pixels(surface_m), + i2);
			                for (c=num_of_cells ; c!=0 ; c--){
			                    int j2 = 0;
			                    for (j=w2 ; j!=0 ; j--, buffer.inc(), alphap.inc(4)){
			                    	alphap[0] = (byte)(((new Uint32Ptr(buffer_m, + j2))[0] & 0xff) ^ 0xff);
			                        if (j2 >= mw) j2 = 0;
			                        else          j2++;
			                    }
			                }
			            	buffer.inc(mw - (cw * num_of_cells));
			                i2 += mw;
			                if (i2 >= mwh) i2 = 0;
			            }
			            SDL_UnlockSurface( surface_m );
			        }
			    }
			    else if ( trans_mode == TRANS_TOPLEFT ||
			              trans_mode == TRANS_TOPRIGHT ||
			              trans_mode == TRANS_DIRECT ){
			    	int trans_value = (int)(RGBMASK & MEDGRAY);
			        for (i=h ; i!=0 ; i--){
			        	for (j=w ; j!=0 ; j--, buffer.inc(), alphap.inc(4)){
			                if ( (buffer[0] & RGBMASK) == ref_color )
			                	buffer[0] = (uint)trans_value;
			                else
			                	alphap[0] = 0xff;
			            }
			        }
			    }
			    else if ( trans_mode == TRANS_STRING ){
			        for (i=h ; i!=0 ; i--){
			    		for (j=w ; j!=0 ; j--, buffer.inc(), alphap.inc(4))
			    			alphap[0] = (byte)(buffer[0] >> 24);
			        }
			    }
			    else if ( trans_mode != TRANS_ALPHA ){ // TRANS_COPY
			        for (i=h ; i!=0 ; i--){
			    		for (j=w ; j!=0 ; j--, buffer.inc(), alphap.inc(4))
			    			alphap[0] = 0xff;
			        }
			    }

			    SDL_UnlockSurface( surface );

			    return surface;
			}
			
			//#ifdef RCA_SCALE
			//SDL_Surface *AnimationInfo::resize( SDL_Surface *surface, int ratio1, int ratio2,
			//                                    float stretch_x, float stretch_y )
			//#else
			public SDL_Surface resize( SDL_Surface surface, int ratio1, int ratio2 )
				//#endif
			{
			#if RCA_SCALE
			    if ( !surface || ((ratio1 == ratio2) && (stretch_x == 1.0) &&
			                     (stretch_y == 1.0)) )
			#else
			    if ( null==surface || (ratio1 == ratio2))
			#endif
			        return surface;

			    SDL_Surface src_s = surface;
			    SDL_PixelFormat fmt = SDL_Surface_get_format(surface);

			    const int MAX_PITCH = 16384;
			    int h = 0;
			    int w = ((SDL_Surface_get_w(src_s) / num_of_cells) * ratio1 / ratio2) * num_of_cells;
			#if RCA_SCALE
			    if (stretch_x > 1.0)
			        w = int((src_s->w / num_of_cells) * ratio1 * stretch_x / ratio2 + 0.5) * num_of_cells;
			#endif
			    if (w >= MAX_PITCH){
			        //too wide for SDL_Surface pitch (Uint16) at 32bpp; size differently
			#if RCA_SCALE
			        if (stretch_y > 1.0)
			            fprintf(stderr, " *** image '%s' is too wide to resize to (%d,%d); ",
			                    file_name, w, int(src_s->h * ratio1 * stretch_y / ratio2 + 0.5));
			        else
			#endif
			        fprintf(stderr, " *** image '%s' is too wide to resize to (%d,%d); ",
			                file_name, w, SDL_Surface_get_h(src_s) * ratio1 / ratio2);
			        w = MAX_PITCH;
			#if RCA_SCALE
			        if (stretch_y > 1.0)
			            h = src_s->h * MAX_PITCH * stretch_y / stretch_x / src_s->w + 0.5;
			        else
			#endif
			        h = SDL_Surface_get_h(src_s) * MAX_PITCH / SDL_Surface_get_w(src_s);
			        if ( h == 0 ) h = 1;
			        fprintf(stderr, "resizing to (%d,%d) instead *** \n", w, h);
			    }else{
			        if ( w == 0 ) w = num_of_cells;
			#if RCA_SCALE
			        if (stretch_y > 1.0)
			            h = src_s->h * ratio1 * stretch_y / ratio2 + 0.5;
			        else
			#endif
			        h = SDL_Surface_get_h(src_s) * ratio1 / ratio2;
			        if ( h == 0 ) h = 1;
			    }
			    surface = SDL_CreateRGBSurface( SDL_SWSURFACE, w, h,
			                                    fmt.BitsPerPixel, fmt.Rmask, fmt.Gmask, fmt.Bmask, fmt.Amask);
			    resizeSurface( src_s, surface, num_of_cells );
			    SDL_FreeSurface( src_s );
			    return surface;
			}
			
			public void setImage( SDL_Surface surface )
			{
			    if (surface == null) return;

			#if !BPP16
			    image_surface = surface;
			#endif
			    allocImage(SDL_Surface_get_w(surface), SDL_Surface_get_h(surface));

			#if BPP16
			    SDL_LockSurface( surface );
			    Uint32 *buffer = (Uint32 *)surface->pixels;

			    ONSBuf *img_buffer = (ONSBuf *)image_surface->pixels;
			    unsigned char *alphap = alpha_buf;
			    const int dst_margin = surface->w % 2;

			    for (int i=surface->h ; i!=0 ; i--){
			        for (int j=surface->w ; j!=0 ; j--, buffer++, img_buffer++)
			            SET_PIXEL32TO16(*buffer, *buffer >> 24);
			        img_buffer += dst_margin;
			    }

			    SDL_UnlockSurface( surface );
			    SDL_FreeSurface( surface );
			#endif
			}
			
			
			public static void setCpufuncs(uint func)
			{
				cpufuncs = func;
			}
			
			public uint getCpufuncs()
			{
				return cpufuncs;
			}
			
			
			public static void imageFilterMean(UnsignedCharPtr src1, UnsignedCharPtr src2, UnsignedCharPtr dst, int length)
			{
			#if USE_PPC_GFX
			    if(cpufuncs & CPUF_PPC_ALTIVEC) {
			        imageFilterMean_Altivec(src1, src2, dst, length);
			    } else {
			        int n = length + 1;
			        BASIC_MEAN();
			    }
			#elif USE_X86_GFX

			#if !MACOSX
			    if (cpufuncs & CPUF_X86_SSE2) {
			#endif // !MACOSX

			        imageFilterMean_SSE2(src1, src2, dst, length);

			#if !MACOSX
			    } else if (cpufuncs & CPUF_X86_MMX) {

			        imageFilterMean_MMX(src1, src2, dst, length);

			    } else {
			        int n = length + 1;
			        BASIC_MEAN();
			    }
			#endif // !MACOSX

			#else // no special gfx handling
			    int n = length + 1;
			    BASIC_MEAN();
			#endif
			}
			
			public static void imageFilterAddTo(UnsignedCharPtr dst, UnsignedCharPtr src, int length)
			{
			#if USE_PPC_GFX
			    if(cpufuncs & CPUF_PPC_ALTIVEC) {
			        imageFilterAddTo_Altivec(dst, src, length);
			    } else {
			        int n = length + 1;
			        BASIC_ADDTO();
			    }
			#elif USE_X86_GFX

			#if !MACOSX
			    if (cpufuncs & CPUF_X86_SSE2) {
			#endif // !MACOSX

			        imageFilterAddTo_SSE2(dst, src, length);

			#if !MACOSX
			    } else if (cpufuncs & CPUF_X86_MMX) {

			        imageFilterAddTo_MMX(dst, src, length);

			    } else {
			        int n = length + 1;
			        BASIC_ADDTO();
			    }
			#endif // !MACOSX

			#else // no special gfx handling
			    int n = length + 1;
			    BASIC_ADDTO();
			#endif
			}
			
			public static void imageFilterSubFrom(UnsignedCharPtr dst, UnsignedCharPtr src, int length)
			{
				#if USE_PPC_GFX
				    if(cpufuncs & CPUF_PPC_ALTIVEC) {
				        imageFilterSubFrom_Altivec(dst, src, length);
				    } else {
				        int n = length + 1;
				        BASIC_SUBFROM();
				    }
				#elif USE_X86_GFX

				#if !MACOSX
				    if (cpufuncs & CPUF_X86_SSE2) {
				#endif // !MACOSX

				        imageFilterSubFrom_SSE2(dst, src, length);

				#if !MACOSX
				    } else if (cpufuncs & CPUF_X86_MMX) {

				        imageFilterSubFrom_MMX(dst, src, length);

				    } else {
				        int n = length + 1;
				        BASIC_SUBFROM();
				    }
				#endif // !MACOSX

				#else // no special gfx handling
				    int n = length + 1;
				    BASIC_SUBFROM();
				#endif
			}
			
			
			public void imageFilterBlend(Uint32Ptr dst_buffer, Uint32Ptr src_buffer,
			                             Uint8Ptr alphap, int alpha, int length)
			{
				//Mion: imageFilterBlend_SSE2 seems to be slower than BASIC_BLEND
				//      commenting out for now
				/*
				#if USE_X86_GFX
				#if !MACOSX
				    if (cpufuncs & CPUF_X86_SSE2) {
				#endif // !MACOSX

				        imageFilterBlend_SSE2(dst_buffer, src_buffer, alphap, alpha, length);

				#if !MACOSX
				    } else {
				        int n = length + 1;
				        BASIC_BLEND();
				    }
				#endif // !MACOSX

				#else // no special gfx handling
				*/
				    int n = length + 1;
				    BASIC_BLEND();
				//#endif
			}
			
			
			//#include "resize_image.h"
			
			private static UnsignedCharPtr resize_buffer = null;
			private static uint resize_buffer_size = 0;
			
			public static void resetResizeBuffer() {
			    if (resize_buffer_size != 16){
			        if (null!=resize_buffer) resize_buffer = null;//delete[] resize_buffer;
			        resize_buffer = new UnsignedCharPtr(new byte[16]);
			        resize_buffer_size = 16;
			    }
			}
			
			
			// resize 32bit surface to 32bit surface
			public static int resizeSurface( SDL_Surface src, SDL_Surface dst, int num_cells=1 )
			{
				SDL_LockSurface( dst );
			    SDL_LockSurface( src );
			    Uint32Ptr src_buffer = new Uint32Ptr(SDL_Surface_get_pixels(src));
			    Uint32Ptr dst_buffer = new Uint32Ptr(SDL_Surface_get_pixels(dst));

			    /* size of tmp_buffer must be larger than 16 bytes */
			    uint len = (uint)(SDL_Surface_get_w(src) * (SDL_Surface_get_h(src) + 1) * 4 + 4);
			    if (resize_buffer_size < len){
			        if (null!=resize_buffer) resize_buffer = null;//delete[] resize_buffer;
			        resize_buffer = new UnsignedCharPtr(new byte[len]);
			        resize_buffer_size = len;
			    }
			    resizeImage( new UnsignedCharPtr(dst_buffer), SDL_Surface_get_w(dst), SDL_Surface_get_h(dst), SDL_Surface_get_w(dst) * 4,
			                new UnsignedCharPtr(src_buffer), SDL_Surface_get_w(src), SDL_Surface_get_h(src), SDL_Surface_get_w(src) * 4,
			                 4, resize_buffer, SDL_Surface_get_w(src) * 4, num_cells );

			    SDL_UnlockSurface( src );
			    SDL_UnlockSurface( dst );

			    return 0;
			}
		}
	}
}
