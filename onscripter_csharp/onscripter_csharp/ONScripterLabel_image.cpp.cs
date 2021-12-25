/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 11:22
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
		 *  ONScripterLabel_image.cpp - Image processing in ONScripter-EN
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
		
		// Modified by Mion, November 2009, to update from
		// Ogapee's 20091115 release source code.
		
//		#ifdef _MSC_VER
//		#pragma warning(disable:4067)
//		#pragma warning(disable:4244)
//		#endif
//		
//		#include "ONScripterLabel.h"
//		#include <new>
//		#include <cstdio>
//		
//		#include "graphics_common.h"
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
		
		public partial class ONScripterLabel {
			public SDL_Surface loadImage(CharPtr filename, ref bool has_alpha, bool has_alpha_is_not_null )
			{
				if ( null==filename ) return null;
			
			    SDL_Surface tmp = null;
			    int location = BaseReader.ARCHIVE_TYPE_NONE;
			
			    if (filename[0] == '>')
			        tmp = createRectangleSurface(filename);
			    else if (filename[0] != '*') // layers begin with *
			        tmp = createSurfaceFromFile(filename, ref location);
			    if (tmp == null) return null;
			
			    bool has_colorkey = false;
			    UInt32 colorkey = 0;
			
			    if ( has_alpha_is_not_null ){
			        has_alpha = (SDL_Surface_get_format(tmp).Amask != 0);
			    }
			
			    SDL_Surface ret = SDL_ConvertSurface( tmp, SDL_Surface_get_format(image_surface), SDL_SWSURFACE );
			    SDL_FreeSurface( tmp );
			
			    //  A PNG image may contain an alpha channel, which complicates
			    // handling loaded images when the ":a" alphablend tag is used,
			    // since the standard method was to assume the right half of the image
			    // contains an alpha data mask for the left half.
			    //  The current default behavior is to use the PNG image's alpha
			    // channel if available, and only process for an old-style mask
			    // when no alpha channel was provided.
			    // However, this could cause problems running older NScr games
			    // which have PNG images containing old-style masks but also an
			    // opaque alpha channel.
			    //  Therefore, we provide a hack, set with the --detect-png-nscmask
			    // command-line option, to auto-detect if a PNG image is likely to
			    // have an old-style mask.  We assume that an old-style mask is intended
			    // if the image either has no alpha channel, or the alpha channel it has
			    // is completely opaque.  (Note that this used to be the default
			    // behavior for onscripter-en.)
			    //  Note that using the --force-png-nscmask option will always assume
			    // old-style masks, while --force-png-alpha will produce the current
			    // default behavior.
			    if ((png_mask_type != PNG_MASK_USE_ALPHA) &&
			        has_alpha_is_not_null && has_alpha) {
			        if (png_mask_type == PNG_MASK_USE_NSCRIPTER)
			            has_alpha = false;
			        else if (png_mask_type == PNG_MASK_AUTODETECT) {
			            SDL_LockSurface(ret);
			            UInt32 aval = new Uint32Ptr(SDL_Surface_get_pixels(ret))[0] & SDL_Surface_get_format(ret).Amask;
			            if (aval != SDL_Surface_get_format(ret).Amask) goto breakalpha;
			            has_alpha = false;
			#if _MSC_VER
				{
			#endif
			            for (int y=0; y < SDL_Surface_get_h(ret); ++y) {
							Uint32Ptr pixbuf = new Uint32Ptr(SDL_Surface_get_pixels(ret), + y * SDL_Surface_get_pitch(ret));
							for (int x = SDL_Surface_get_w(ret); x>0; --x, pixbuf.inc()) {
			                    // Resolving ambiguity per Tatu's patch, 20081118.
			                    // I note that this technically changes the meaning of the
			                    // code, since != is higher-precedence than &, but this
			                    // version is obviously what I intended when I wrote this.
			                    // Has this been broken all along?  :/  -- Haeleth
			                    if ((pixbuf[0] & SDL_Surface_get_format(ret).Amask) != aval) {
			                        has_alpha = true;
			                        goto breakalpha;
			                    }
			                }
			            }
			#if _MSC_VER
				}
			#endif
			          breakalpha:
			            if (!has_alpha && has_colorkey) {
			                // has a colorkey, so run a match against rgb values
			                UInt32 aval_ = colorkey & ~(SDL_Surface_get_format(ret).Amask);
			                if (aval_ == ((new Uint32Ptr(SDL_Surface_get_pixels(ret)))[0] & ~(SDL_Surface_get_format(ret).Amask)))
			                    goto breakkey;
			                has_alpha = false;
			                for (int y = 0; y < SDL_Surface_get_h(ret); ++y) {
			                    Uint32Ptr pixbuf = new Uint32Ptr(SDL_Surface_get_pixels(ret), + y * SDL_Surface_get_pitch(ret));
			                    for (int x = SDL_Surface_get_w(ret); x>0; --x, pixbuf.inc()) {
			                    	if ((pixbuf[0] & ~(SDL_Surface_get_format(ret).Amask)) == aval_) {
			                            has_alpha = true;
			                            goto breakkey;
			                        }
			                    }
			                }
			            }
			          breakkey:
			            SDL_UnlockSurface(ret);
			        }
			    }
			    
			    return ret;
			}
			
			public SDL_Surface createRectangleSurface(CharPtr filename)
			{
				int c=1, w=0, h=0;
			    while (filename[c] != 0x0a && filename[c] != 0x00){
			        if (filename[c] >= '0' && filename[c] <= '9')
			            w = w*10 + filename[c]-'0';
			        if (filename[c] == ','){
			            c++;
			            break;
			        }
			        c++;
			    }
			
			    while (filename[c] != 0x0a && filename[c] != 0x00){
			        if (filename[c] >= '0' && filename[c] <= '9')
			            h = h*10 + filename[c]-'0';
			        if (filename[c] == ','){
			            c++;
			            break;
			        }
			        c++;
			    }
			        
			    while (filename[c] == ' ' || filename[c] == '\t') c++;
			    int n=0, c2 = c;
			    while(filename[c] == '#'){
			    	byte[] col = new byte[3];
			    	readColor(ref col, new CharPtr(filename,+c));
			        n++;
			        c += 7;
			        while (filename[c] == ' ' || filename[c] == '\t') c++;
			    }
			
			    SDL_PixelFormat fmt = SDL_Surface_get_format(image_surface);
			    SDL_Surface tmp = SDL_CreateRGBSurface(SDL_SWSURFACE, w, h,
			                                            fmt.BitsPerPixel, fmt.Rmask, fmt.Gmask, fmt.Bmask, fmt.Amask);
			
			    c = c2;
			    for (int i=0 ; i<n ; i++){
			    	byte[] col = new byte[3];
			    	readColor(ref col, new CharPtr(filename,+c));
			        c += 7;
			        while (filename[c] == ' ' || filename[c] == '\t') c++;
			        
			        SDL_Rect rect = new SDL_Rect();
			        rect.x = w*i/n;
			        rect.y = 0;
			        rect.w = w*(i+1)/n - rect.x;
			        if (i == n-1) rect.w = w - rect.x;
			        rect.h = h;
			        SDL_FillRect(tmp, rect, SDL_MapRGBA( SDL_Surface_get_format(accumulation_surface), col[0], col[1], col[2], 0xff));
			    }
			    
			    return tmp;
			}
			
			public SDL_Surface createSurfaceFromFile(CharPtr filename, ref int location)
			{
				CharPtr alt_buffer = null;
			    ulong length = script_h.cBR.getFileLength( filename );
			
			    if (length == 0) {
			        alt_buffer = new char[strlen(filename) + strlen(script_h.save_path) + 1];
			        sprintf(alt_buffer, "%s%s", script_h.save_path, filename);
			        CharPtr si = new CharPtr(alt_buffer);
			        do { if (si[0] == '\\') si[0] = DELIMITER; 
			        	si.inc(); if (si[0]==0) break;
			        } while (true);
			#if true //_MSC_VER <= 1200
			        FILEPtr fp = fopen(alt_buffer, "rb");
			#else
			        FILE* fp = std::fopen(alt_buffer, "rb");
			#endif
			        if (null!=fp) {
			            fseek(fp, 0, SEEK_END);
			            length = (ulong)ftell(fp);
			            fclose(fp);
			        }
			        else alt_buffer = null;//delete[] alt_buffer;
			    }
			
			    if ( length == 0 ){
			        //don't complain about missing cursors
			        if (0!=strcmp(filename, "uoncur.bmp" ) &&
			            0!=strcmp(filename, "uoffcur.bmp") &&
			            0!=strcmp(filename, "doncur.bmp" ) &&
			            0!=strcmp(filename, "doffcur.bmp") &&
			            0!=strcmp(filename, "cursor0.bmp") &&
			            0!=strcmp(filename, "cursor1.bmp")) {
			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                     "can't find file [%s]", filename);
			            errorAndCont( script_h.errbuf, null, "I/O Issue" );
			        }
			        return null;
			    }
			    if ( filelog_flag )
			        script_h.findAndAddLog( script_h.log_info[ScriptHandler.FILE_LOG], filename, true );
			    //printf(" ... loading %s length %ld\n", filename, length );
			
			    mean_size_of_loaded_images += length*6/5; // reserve 20% larger size
			    num_loaded_images++;
			    if (tmp_image_buf_length < mean_size_of_loaded_images/num_loaded_images){
			        tmp_image_buf_length = mean_size_of_loaded_images/num_loaded_images;
			        if (null!=tmp_image_buf) tmp_image_buf = null;//delete[] tmp_image_buf;
			        tmp_image_buf = null;
			    }
			
			    UnsignedCharPtr buffer = null;
			    if (length > tmp_image_buf_length){
			    	buffer = new UnsignedCharPtr(new byte[length]);
			        if (buffer == null) {
			            snprintf(script_h.errbuf, MAX_ERRBUF_LEN,
			                     "failed to load image file [%s] (%lu bytes)",
			                     filename, length);
			            errorAndCont( script_h.errbuf, "unable to allocate buffer", "Memory Issue" );
			            return null;
			        }
			    }
			    else{
			    	if (null==tmp_image_buf) tmp_image_buf = new UnsignedCharPtr(new byte[tmp_image_buf_length]);
			        buffer = tmp_image_buf;
			    }
			
			    if (null==alt_buffer) {
			        script_h.cBR.getFile( filename, buffer, ref location );
			    }
			    else {
			        FILEPtr fp;
			#if true //_MSC_VER <= 1200
			        if (null!=(fp = fopen(alt_buffer, "rb"))) {
			#else
			        if ((fp = std::fopen(alt_buffer, "rb"))) {
			#endif
						if (fread(buffer, 1, (uint)length, fp) != length)
			                fprintf(stderr, "Warning: error reading from %s\n", alt_buffer);
			            fclose(fp);
			        }
			        alt_buffer = null;//delete[] alt_buffer;
			    }
			    CharPtr ext = strrchr(filename, '.');
			
			    SDL_RWops src = SDL_RWFromMem(buffer, (int)length);
			    SDL_Surface tmp = IMG_Load_RW(src, 0);
			#if false
			    if (!tmp && ext && (!strcmp(ext+1, "JPG") || !strcmp(ext+1, "jpg"))){
			        fprintf(stderr, " *** force-loading a JPG image [%s]\n", filename);
			        tmp = IMG_LoadJPG_RW(src);
			
			    }
			#endif
			    SDL_RWclose(src);
			
			    if (buffer != tmp_image_buf) buffer = null;//delete[] buffer;
			    if (null==tmp)
			        fprintf( stderr, " *** can't load file [%s]: %s ***\n", filename, IMG_GetError() );
			
			    return tmp;
			}
			
			
			// alphaMaskBlend
			// dst: accumulation_surface
			// src1: effect_src_surface
			// src2: effect_dst_surface
			public void alphaMaskBlend( SDL_Surface mask_surface, int trans_mode,
			                                      uint mask_value=255, SDL_Rect clip=null,
			                                      SDL_Surface src1=null, SDL_Surface src2=null, SDL_Surface dst=null )
			{
				SDL_Rect rect = new SDL_Rect(0, 0, screen_width, screen_height);
			
			    if (src1 == null)
			        src1 = effect_src_surface;
			    if (src2 == null)
			        src2 = effect_dst_surface;
			    if (dst == null)
			        dst = accumulation_surface;
			
			    /* ---------------------------------------- */
			    /* clipping */
			    if ( null!=clip ){
			        if ( 0!=AnimationInfo.doClipping( rect, clip ) ) return;
			    }
			
			    /* ---------------------------------------- */
			
			    SDL_LockSurface( src1 );
			    SDL_LockSurface( src2 );
			    SDL_LockSurface( dst );
			    if ( null!=mask_surface ) SDL_LockSurface( mask_surface );
			    
			    Uint32Ptr src1_buffer = new Uint32Ptr(SDL_Surface_get_pixels(src1), + SDL_Surface_get_w(src1) * rect.y + rect.x);
                Uint32Ptr src2_buffer = new Uint32Ptr(SDL_Surface_get_pixels(src2), + SDL_Surface_get_w(src2) * rect.y + rect.x);
                Uint32Ptr dst_buffer  = new Uint32Ptr(SDL_Surface_get_pixels(dst), + SDL_Surface_get_w(dst) * rect.y + rect.x);
			
			    int rwidth = screen_width - rect.w;
			    SDL_PixelFormat fmt = SDL_Surface_get_format(dst);
			    UInt32 overflow_mask = 0xffffffff;
			    if ( trans_mode != ALPHA_BLEND_FADE_MASK )
			        overflow_mask = ~fmt.Bmask;
			
			    mask_value >>= fmt.Bloss;
			
			    if (( trans_mode == ALPHA_BLEND_FADE_MASK ||
			          trans_mode == ALPHA_BLEND_CROSSFADE_MASK ) && null!=mask_surface) {
			        for ( int i=0 ; i<rect.h ; i++ ) {
			    		Uint32Ptr mask_buffer = new Uint32Ptr(SDL_Surface_get_pixels(mask_surface), + SDL_Surface_get_w(mask_surface) * ((rect.y+i) % SDL_Surface_get_h(mask_surface)));
			
			            int offset = rect.x % SDL_Surface_get_w(mask_surface);
			            for ( int j=rect.w ; j!=0 ; j-- ){
			                UInt32 mask2 = 0;
			                UInt32 mask = mask_buffer[ + offset] & fmt.Bmask;
			                if ( mask_value > mask ){
			                    mask2 = mask_value - mask;
			                    if ( 0!=(mask2 & overflow_mask) ) mask2 = fmt.Bmask;
			                }
			                BLEND_MASK_PIXEL();
			                dst_buffer.inc(); src1_buffer.inc(); src2_buffer.inc(); ++offset;
			                if (offset >= SDL_Surface_get_w(mask_surface)) offset = 0;
			            }
			            src1_buffer.inc(rwidth);
                        src2_buffer.inc(rwidth);
                        dst_buffer.inc(rwidth);
			        }
			    }
			    else{ // ALPHA_BLEND_CONST
			        UInt32 mask2 = mask_value & fmt.Bmask;
			
			        for ( int i=rect.h ; i!=0 ; i-- ) {
			            for ( int j=rect.w ; j!=0 ; j-- ){
			                BLEND_MASK_PIXEL();
			                dst_buffer.inc(); src1_buffer.inc(); src2_buffer.inc();
			            }
			        	src1_buffer.inc(rwidth);
			        	src2_buffer.inc(rwidth);
			        	dst_buffer.inc(rwidth);
			        }
			    }
			    
			    if ( null!=mask_surface ) SDL_UnlockSurface( mask_surface );
			    SDL_UnlockSurface( dst );
			    SDL_UnlockSurface( src2 );
			    SDL_UnlockSurface( src1 );
			}
			
			// alphaBlendText
			// dst: ONSBuf surface (accumulation_surface)
			// txt: 8bit surface (TTF_RenderGlyph_Shaded())
			public void alphaBlendText( SDL_Surface dst_surface, SDL_Rect dst_rect,
			                                      SDL_Surface txt_surface, SDL_Color color, SDL_Rect clip, bool rotate_flag )
			{
			    int x2=0, y2=0;
			    SDL_Rect clipped_rect = new SDL_Rect();
			
			    /* ---------------------------------------- */
			    /* 1st clipping */
			    if ( null!=clip ){
			        if ( 0!=AnimationInfo.doClipping( dst_rect, clip, clipped_rect ) ) return;
			
			        x2 += clipped_rect.x;
			        y2 += clipped_rect.y;
			    }
			
			    /* ---------------------------------------- */
			    /* 2nd clipping */
			    SDL_Rect clip_rect = new SDL_Rect(0, 0, SDL_Surface_get_w(dst_surface), SDL_Surface_get_h(dst_surface));
			    if ( 0!=AnimationInfo.doClipping( dst_rect, clip_rect, clipped_rect ) ) return;
			    
			    x2 += clipped_rect.x;
			    y2 += clipped_rect.y;
			
			    /* ---------------------------------------- */
			
			    SDL_LockSurface( dst_surface );
			    SDL_LockSurface( txt_surface );
			
			#if BPP16
			    Uint32 src_color = ((color.r >> RLOSS) << RSHIFT) |
			                       ((color.g >> GLOSS) << GSHIFT) |
			                       (color.b >> BLOSS);
			    src_color = (src_color | src_color << 16) & BLENDMASK;
			#else
				UInt32 src_color1 = (UInt32)(color.r << RSHIFT | color.b);
			    UInt32 src_color2 = (UInt32)(color.g << GSHIFT);
			    UInt32 src_color3 = (UInt32)(src_color1 | src_color2);
			#endif
			
			    Uint32Ptr dst_buffer = new Uint32Ptr(SDL_Surface_get_pixels(dst_surface), +
			                                     SDL_Surface_get_w(dst_surface) * dst_rect.y + dst_rect.x);
			
			    if (!rotate_flag){
			        UnsignedCharPtr src_buffer = new UnsignedCharPtr(SDL_Surface_get_pixels(txt_surface), +
			    	                                                 SDL_Surface_get_pitch(txt_surface) * y2 + x2);
			        for ( int i=dst_rect.h ; i!=0 ; i-- ){
						for ( int j=dst_rect.w ; j!=0 ; j--, dst_buffer.inc(), src_buffer.inc() ){
			                BLEND_TEXT();
			            }
						dst_buffer.inc(SDL_Surface_get_w(dst_surface) - dst_rect.w);
					    src_buffer.inc(SDL_Surface_get_pitch(txt_surface) - dst_rect.w);
			        }
			    }
			    else{
			        for ( int i=0 ; i<dst_rect.h ; i++ ){
			    		UnsignedCharPtr src_buffer = new UnsignedCharPtr(SDL_Surface_get_pixels(txt_surface), + SDL_Surface_get_pitch(txt_surface) * (SDL_Surface_get_h(txt_surface) - x2 - 1) + y2 + i);
			    		for ( int j=dst_rect.w ; j!=0 ; j--, dst_buffer.inc() ){
			                BLEND_TEXT();
			                src_buffer.minus(SDL_Surface_get_pitch(txt_surface));
			            }
			    		dst_buffer.inc(SDL_Surface_get_w(dst_surface) - dst_rect.w);
			        }
			    }
			    
			    SDL_UnlockSurface( txt_surface );
			    SDL_UnlockSurface( dst_surface );
			}
			
			public void makeNegaSurface( SDL_Surface surface, SDL_Rect clip )
			{
			    SDL_LockSurface( surface );
			    Uint32Ptr buf = new Uint32Ptr(SDL_Surface_get_pixels(surface), + clip.y * SDL_Surface_get_w(surface) + clip.x);
			
			    UInt32 mask = SDL_Surface_get_format(surface).Rmask | SDL_Surface_get_format(surface).Gmask | SDL_Surface_get_format(surface).Bmask;
			    for ( int i=clip.h ; i>0 ; i-- ){
			        for ( int j=clip.w ; j>0 ; j-- )
			        {
			        	buf[0] ^= mask; buf.inc();
			        }
			        buf.inc(SDL_Surface_get_w(surface) - clip.w);
			    }
			
			    SDL_UnlockSurface( surface );
			}
			
			public void makeMonochromeSurface( SDL_Surface surface, SDL_Rect clip )
			{
			    SDL_LockSurface( surface );
			    Uint32Ptr buffer = new Uint32Ptr(SDL_Surface_get_pixels(surface),  + clip.y * SDL_Surface_get_w(surface) + clip.x);
			
			    for ( int i=clip.h ; i>0 ; i-- ){
			    	for ( int j=clip.w ; j>0 ; j--, buffer.inc() ){
			            //Mion: NScr seems to use more "equal" 85/86/85 RGB blending, instead
			            // of the 77/151/28 that onscr used to have. Using 85/86/85 now,
			            // might add a parameter to "monocro" to allow choosing 77/151/28
			            MONOCRO_PIXEL();
			        }
			    	buffer.inc(SDL_Surface_get_w(surface) - clip.w);
			    }
			
			    SDL_UnlockSurface( surface );
			}
			
			public void refreshSurface( SDL_Surface surface, SDL_Rect clip_src, int refresh_mode )
			{
			    if (refresh_mode == REFRESH_NONE_MODE) return;
			
			    SDL_Rect clip = new SDL_Rect(0, 0, SDL_Surface_get_w(surface), SDL_Surface_get_h(surface));
			    if (null!=clip_src) if ( 0!=AnimationInfo.doClipping( clip, clip_src ) ) return;
			
			    int i, top;
			    #if false
					SDL_BlitSurface( bg_info.image_surface, clip, surface, clip );
				#else
					//???why this work???
					bg_info.blendOnSurface( surface, 0, 0, ref clip );
				#endif
			
			    if ( !all_sprite_hide_flag ){
			    	if ( z_order < 10 && 0!=(refresh_mode & REFRESH_SAYA_MODE) )
			            top = 9;
			        else
			            top = z_order;
			        for ( i=MAX_SPRITE_NUM-1 ; i>top ; i-- ){
			            if ( null!=sprite_info[i].image_surface && sprite_info[i].visible ){
			                drawTaggedSurface( surface, sprite_info[i], clip );
			            }
			        }
			    }
			
			    for ( i=0 ; i<3 ; i++ ){
			        if (human_order[2-i] >= 0 && null!=tachi_info[human_order[2-i]].image_surface){
			            drawTaggedSurface( surface, tachi_info[human_order[2-i]], clip );
			        }
			    }
			
			    if ( windowback_flag ){
			        if ( nega_mode == 1 ) makeNegaSurface( surface, clip );
			        if ( monocro_flag )   makeMonochromeSurface( surface, clip );
			        if ( nega_mode == 2 ) makeNegaSurface( surface, clip );
			
			        if (!all_sprite2_hide_flag){
			            for ( i=MAX_SPRITE2_NUM-1 ; i>=0 ; i-- ){
			                if ( null!=sprite2_info[i].image_surface && sprite2_info[i].visible ){
			                    drawTaggedSurface( surface, sprite2_info[i], clip );
			                }
			            }
			        }
			
			        if (0!=(refresh_mode & REFRESH_WINDOW_MODE))
			            displayTextWindow( surface, clip );
			        if (0!=(refresh_mode & REFRESH_TEXT_MODE))
			            text_info.blendOnSurface( surface, 0, 0, ref clip );
			    }
			
			    if ( !all_sprite_hide_flag ){
			    	if ( 0!=(refresh_mode & REFRESH_SAYA_MODE) )
			            top = 10;
			        else
			            top = 0;
			        for ( i=z_order ; i>=top ; i-- ){
			            if ( null!=sprite_info[i].image_surface && sprite_info[i].visible ){
			                drawTaggedSurface( surface, sprite_info[i], clip );
			            }
			        }
			    }
			
			    if ( !windowback_flag ){
			        //Mion - ogapee2008
			        if (!all_sprite2_hide_flag){
			            for ( i=MAX_SPRITE2_NUM-1 ; i>=0 ; i-- ){
			                if ( null!=sprite2_info[i].image_surface && sprite2_info[i].visible ){
			                    drawTaggedSurface( surface, sprite2_info[i], clip );
			                }
			            }
			        }
			        if ( nega_mode == 1 ) makeNegaSurface( surface, clip );
			        if ( monocro_flag )   makeMonochromeSurface( surface, clip );
			        if ( nega_mode == 2 ) makeNegaSurface( surface, clip );
			    }
			    
			    if ( 0==( refresh_mode & REFRESH_SAYA_MODE ) ){
			        for ( i=0 ; i<MAX_PARAM_NUM ; i++ ){
			            if ( null!=bar_info[i] ) {
			                drawTaggedSurface( surface, bar_info[i], clip );
			            }
			        }
			        for ( i=0 ; i<MAX_PARAM_NUM ; i++ ){
			            if ( null!=prnum_info[i] ){
			                drawTaggedSurface( surface, prnum_info[i], clip );
			            }
			        }
			    }
			
			    if ( !windowback_flag ){
			    	if (0!=(refresh_mode & REFRESH_WINDOW_MODE))
			            displayTextWindow( surface, clip );
			    	if (0!=(refresh_mode & REFRESH_TEXT_MODE))
			            text_info.blendOnSurface( surface, 0, 0, ref clip );
			    }
			
			    if ( 0!=(refresh_mode & REFRESH_CURSOR_MODE) && null==textgosub_label ){
			        if ( clickstr_state == CLICK_WAIT )
			            drawTaggedSurface( surface, cursor_info[CURSOR_WAIT_NO], clip );
			        else if ( clickstr_state == CLICK_NEWPAGE )
			            drawTaggedSurface( surface, cursor_info[CURSOR_NEWPAGE_NO], clip );
			    }
			
			    //Mion: fix for the menu title bug noted in the past by Seung Park:
			    // the right-click menu title must be drawn close to last during refresh,
			    // not in the textwindow, since there could be sprites above the
			    // textwindow if windowback is used.
			    if (null!=system_menu_title)
			        drawTaggedSurface( surface, system_menu_title, clip );
			
			    ButtonLink p_button_link = root_button_link.next;
			    while( null!=p_button_link ){
			        ButtonLink cur_button_link = p_button_link;
			        while (null!=cur_button_link) {
			            if (cur_button_link.show_flag > 0){
			                drawTaggedSurface( surface, cur_button_link.anim[cur_button_link.show_flag-1], clip );
			            }
			            cur_button_link = cur_button_link.same;
			        }
			        p_button_link = p_button_link.next;
			    }
			}
			
			public void refreshSprite( int sprite_no, bool active_flag,
							     int cell_no, SDL_Rect check_src_rect,
							     SDL_Rect check_dst_rect )
			{
			    if (( null!=sprite_info[sprite_no].image_name ||
			          ((sprite_info[sprite_no].trans_mode == AnimationInfo.TRANS_STRING) &&
			           null!=sprite_info[sprite_no].file_name) ) && 
			        ( (sprite_info[ sprite_no ].visible != active_flag) ||
			          ((cell_no >= 0) && (sprite_info[ sprite_no ].current_cell != cell_no)) ||
			          (AnimationInfo.doClipping(check_src_rect, sprite_info[ sprite_no ].pos) == 0) ||
			          (AnimationInfo.doClipping(check_dst_rect, sprite_info[ sprite_no ].pos) == 0) ))
			    {
			        if ( cell_no >= 0 )
			            sprite_info[ sprite_no ].setCell(cell_no);
			
			        sprite_info[ sprite_no ].visible = active_flag;
			
			        dirty_rect.add( sprite_info[ sprite_no ].pos );
			    }
			}
			
			public void createBackground()
			{
			    bg_info.num_of_cells = 1;
			    bg_info.trans_mode = AnimationInfo.TRANS_COPY;
			    bg_info.pos.x = 0;
			    bg_info.pos.y = 0;
			    bg_info.allocImage( screen_width, screen_height );
			
			    if ( 0==strcmp( bg_info.file_name, "white" ) ){
			        bg_info.color[0] = bg_info.color[1] = bg_info.color[2] = 0xff;
			    }
			    else if ( 0==strcmp( bg_info.file_name, "black" ) ){
			        bg_info.color[0] = bg_info.color[1] = bg_info.color[2] = 0x00;
			    }
			    else if ( bg_info.file_name[0] == '#' ){
			        readColor( ref bg_info.color, bg_info.file_name );
			    }
			    else{
			    	AnimationInfo anim = new AnimationInfo();
			        setStr( ref anim.image_name, bg_info.file_name );
			        parseTaggedString( anim );
			        anim.trans_mode = AnimationInfo.TRANS_COPY;
			        anim.num_of_cells = 1;
			#if RCA_SCALE
			        //stretch the bg image to match against the original screen sizing
			        if ( scr_stretch_y > 1.0 || scr_stretch_x > 1.0 )
			            setupAnimationInfo( &anim, NULL, scr_stretch_x, scr_stretch_y );
			        else
			#endif
			        setupAnimationInfo( anim );
			
			        bg_info.fill(0, 0, 0, 0xff);
			        if (null!=anim.image_surface){
			        	SDL_Rect src_rect = new SDL_Rect(0, 0, SDL_Surface_get_w(anim.image_surface), SDL_Surface_get_h(anim.image_surface));
			        	SDL_Rect dst_rect = new SDL_Rect(0, 0, screen_width, screen_height);
			            if (screen_width >= SDL_Surface_get_w(anim.image_surface)){
			                dst_rect.x = (screen_width - SDL_Surface_get_w(anim.image_surface)) / 2;
			            }
			            else{
			                src_rect.x = (SDL_Surface_get_w(anim.image_surface) - screen_width) / 2;
			                src_rect.w = screen_width;
			            }
			            if (screen_height >= SDL_Surface_get_h(anim.image_surface)){
			                dst_rect.y = (screen_height - SDL_Surface_get_h(anim.image_surface)) / 2;
			            }
			            else{
			                src_rect.y = (SDL_Surface_get_h(anim.image_surface) - screen_height) / 2;
			                src_rect.h = screen_height;
			            }
			            bg_info.copySurface(anim.image_surface, src_rect, dst_rect);
			        }
			        return;
			    }
			
			    bg_info.fill(bg_info.color[0], bg_info.color[1], bg_info.color[2], 0xff);
			}
		}
	}
}
