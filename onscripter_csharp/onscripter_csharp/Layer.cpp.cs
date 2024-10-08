﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-1
 * Time: 6:24
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
		 *  Layer.cpp - Code for effect layers for ONScripter-EN
		 *
		 *  Copyright (c) 2009-2011 "Uncle" Mion Sonozaki
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
//		#pragma warning(disable:4305)
//		#pragma warning(disable:4244)
//		#pragma warning(disable:4800)
//		#endif
		
		/*
		 *  Emulation of Takashi Toyama's "oldmovie.dll", "snow.dll", and "hana.dll"
		 *  NScripter plugin filters.
		 */
//		#include "Layer.h"
//		
//		#ifndef NO_LAYER_EFFECTS
//		
//		#include <stdio.h>
//		#include <stdlib.h>
//		#include <math.h>
//		#ifndef M_PI
//		#define M_PI 3.14159265358979323846
//		#endif
//		
//		#define RMASK 0x00ff0000
//		#define GMASK 0x0000ff00
//		#define BMASK 0x000000ff
//		#define AMASK 0xff000000
//		
//		#define MAX_SPRITE_NUM 1000
		
		private static void drawTaggedSurface( SDL_Surface dst_surface, AnimationInfo anim, ref SDL_Rect clip )
		{
		        anim.blendOnSurface( dst_surface, anim.pos.x, anim.pos.y,
		                              ref clip, anim.trans );
		}
		
//		#ifndef BPP16 //not supporting 16bpp for "oldmovie" yet
		/*
		 *  Emulation of Takashi Toyama's "oldmovie.dll" NScripter filter for ONScripter.
		 *
		 *  The old-movie effect itself is created in a rather inefficient way, compared
		 *  to Toyama-san's tight ASM original, but we have to run on PowerPC as well. ^^
		 *  Using MMX-optimised composition routines should claw back some of
		 *  the lost performance on x86 systems.
		 *
		 *  Copyright (c) 2006 Peter Jolly.
		 *
		 *  haeleth@haeleth.net
		 *
		 */
		// Modified extensively by Mion, 2008
		// Modified by Mion, Dec 2009, to optimize and cleanup code
		
		private const int MAX_NOISE          = 8; // Number of noise surfaces.
		private const int MAX_GLOW          = 25; // Number of glow levels.
		private const int MAX_DUST_COUNT    = 10; // Number of dust particles.
		private const int MAX_SCRATCH_COUNT  = 6; // Number of scratches.
		private static int scratch_count;    // Number of scratches visible.
		
		public class Scratch {
//		private:
		    public int offs;   // Tint of the line: 64 for light, -64 for dark, 0 for no scratch.
		    public int x1, x2; // Horizontal position of top and bottom of the line.
		    public int dx;     // Distance by which the line moves each frame.
		    public int time;   // Number of frames remaining before reinitialisation.
		    public int width, height;
//		    void init(int level);
//		public:
		    public Scratch() { offs = (0); time = (1); }
		    public void setwindow(int w, int h){ width = w; height = h; }
//		    void update(int level);
//		    void draw(SDL_Surface* surface, SDL_Rect clip);
//		};
		
			// Create a new scratch.
		public void init(int level)
		{
		    // If this scratch was visible, decrement the counter.
		    if (0!=offs) --scratch_count;
		    offs = 0;
		
		    // Each scratch object is reinitialised every 3-9 frames.
		    time = rand() % 7 + 3;
		
		    if ((rand() % 600) < level) {
		        ++scratch_count;
		        offs = 0!=(rand() % 2) ? 64 : -64;
		        x1 = rand() % (width - 20) + 10;
		        dx = rand() % 12 - 6;
		        x2 = x1 - dx; // The angle of the line is determined by the speed of motion.
		    }
		}
		
		// Called each frame.
		public void update(int level)
		{
		    if (--time == 0) 
		        init(level);
		    else if (0!=offs) {
		        x1 += dx;
		        x2 += dx;
		    }
		}
		
		// Called each time the screen is refreshed.  Draws a simple line, without antialiasing.
		public void draw(SDL_Surface surface, SDL_Rect clip) 
		{
		    // Don't draw unless this scratch is visible and likely to pass through the updated rectangle.
		    if ( (offs == 0) || (x1 < clip.x) || (x2 < clip.x) ||
		         (x1 >= (clip.x + clip.w)) || (x2 >= (clip.x + clip.w)) )
		        return;
		
		    int sp = SDL_Surface_get_pitch(surface);
		    float dx = (float)(x2 - x1) / width;
		    float realx = (float) x1;
		    int y = 0;
		    while (y != clip.y) {
		         // Skip all scanlines above the clipping rectangle.
		        ++y;
		        realx += dx;
		    }
		    while (y < clip.y + clip.h) {
		        int lx = (int) floor(realx + 0.5);
		        if (lx >= clip.x && lx < clip.x + clip.w) { // Only draw within the clipping rectangle.
		            // Get pixel...
		            Uint32Ptr p = new Uint32Ptr(new CharPtr(SDL_Surface_get_pixels(surface), + y * sp + lx * 4));
		            UInt32 c = p[0];
		            // ...add to or subtract from its colour...
		            int c1 = (int)((c & 0xff) + offs), c2 = (int)(((c >> 8) & 0xff) + offs), c3 = (int)(((c >> 16) & 0xff) + offs);
		            if (c1 < 0) c1 = 0; else if (c1 > 255) c1 = 255;
		            if (c2 < 0) c2 = 0; else if (c2 > 255) c2 = 255;
		            if (c3 < 0) c3 = 0; else if (c3 > 255) c3 = 255;
		            // ...and put it back.
		            p[0] = (uint)(c1 | c2 << 8 | c3 << 16);
		        }
		        ++y;
		        realx += dx;
		    }
		}
		} //FIXME:added, end class Scratch
		
		private static Scratch[] scratches = scratches_init();
		private static Scratch[] scratches_init() {
			Scratch[] result = new Scratch[MAX_SCRATCH_COUNT];
			for (int i = 0; i < result.Length;++i)
			{
				result[i] = new Scratch();
			}
			return result;
		}
		// We store multiple screens of random noise, and flip between them at random.
		private static SDL_Surface[] NoiseSurface = new SDL_Surface[MAX_NOISE];
//		// For the glow effect, we store a single surface with a scanline for each glow level.
		private static SDL_Surface GlowSurface;
		private static int om_count = 0;
		private static bool initialized_om_surfaces = false;

		public partial class OldMovieLayer : Layer {
			public OldMovieLayer( int w, int h )
			{
			    width = w;
			    height = h;
			
			    blur_level = noise_level = glow_level = scratch_level = dust_level = 0;
			    dust_sprite = dust = null;
			    dust_pts = null;
			
			    initialized = false;
			}
			
			~OldMovieLayer() {
			    if (initialized) {
			        --om_count;
			        if (om_count == 0) {
			            for (int i=0; i<MAX_NOISE; i++) {
			                SDL_FreeSurface(NoiseSurface[i]);
			                NoiseSurface[i] = null;
			            }
			            SDL_FreeSurface(GlowSurface);
			            GlowSurface = null;
			            initialized_om_surfaces = false;
			        }
			        if (null!=dust) dust = null;//delete dust;
			        if (null!=dust_pts) dust_pts = null;//delete[] dust_pts;
			    }
			}
			
			public void om_init()
			{
			    ++om_count;
			
			    gv = 0;
			    go = 1;
			    rx = ry = 0;
			    ns = 0;
			
			    if (null!=dust_sprite) {
			        // Copy dust sprite to dust
			        if (null!=dust) dust = null;//delete dust;
			        dust = new AnimationInfo(dust_sprite);
			        dust.visible = true;
			    }
			    if (null!=dust_pts) dust_pts = null;//delete[] dust_pts;
			    dust_pts = new Pt[MAX_DUST_COUNT];
			
			    initialized = true;
			
			    //don't reinitialise existing noise and glow surfaces or scratches
			    if (initialized_om_surfaces) return;
			
			    // set up scratches
			    for (int i = 0; i < MAX_SCRATCH_COUNT; i++)
			        scratches[i].setwindow(width, height);
			
			#if _MSC_VER
				{
			#endif
			    // Generate screens of random noise.
			    for (int i = 0; i < MAX_NOISE; i++) {
			        NoiseSurface[i] = AnimationInfo.allocSurface(width, height);
			        SDL_LockSurface(NoiseSurface[i]);
			        CharPtr px = CharPtr.fromUnsignedCharPtr(SDL_Surface_get_pixels(NoiseSurface[i]));
			        int pt = SDL_Surface_get_pitch(NoiseSurface[i]);
			        for (int y = 0; y < height; ++y, px.inc(pt)) {
			        	Uint32Ptr row = new Uint32Ptr( px);
			            for (int x = 0; x < width; ++x, row.inc()) {
			                int rm = (rand() % (noise_level + 1)) * 2;
			                row[0] = (uint)(0 | (rm << 16) | (rm << 8) | rm);
			            }
			        }
			        SDL_UnlockSurface(NoiseSurface[i]);
			    }
			#if _MSC_VER
				}
			#endif
			
			    // Generate scanlines of solid greyscale, used for the glow effect.
			    GlowSurface = AnimationInfo.allocSurface(width, MAX_GLOW);
			#if false//!_MSC_VER
				for (SDL_Rect r = { 0, 0, width, 1 }; r.y < MAX_GLOW; r.y++) {
			#else
				SDL_Rect r = new SDL_Rect( 0, 0, width, 1 );
				for (; r.y < MAX_GLOW; r.y++) {
			#endif
			        int ry_ = (r.y * 30 / MAX_GLOW) + 4;
			        SDL_FillRect(GlowSurface, r, SDL_MapRGB(SDL_Surface_get_format(GlowSurface), (byte)ry_, (byte)ry_, (byte)ry_));
			    }
			}
			
			// Called once each frame.  Updates effect parameters.
			public override void update()
			{
			    if (!initialized) return;
			
			    int last_x = rx, last_y = ry, last_n = ns;
			    // Generate blur offset and noise screen randomly.
			    // Ensure neither setting is the same two frames running.
			    if (blur_level > 0) {
			        do {
			            rx = rand() % (blur_level + 1) - 1;
			            ry = rand() % (blur_level + 1);
			        } while (rx == last_x && ry == last_y);
			    }
			    do {
			        ns = rand() % MAX_NOISE;
			    } while (ns == last_n);
			
			    // Increment glow; reverse direction if we've reached either limit.
			    gv += go;
			    if (gv >= 5) { gv = 3; go = -1; }
			    if (gv < 0) { gv = 1; go = 1; }
			
			    // Update scratches.
			    for (int i=0; i<MAX_SCRATCH_COUNT; i++)
			        scratches[i].update(scratch_level);
			
			    // Update dust
			    if (dust.num_of_cells > 0) {
			        for (int i=0; i<MAX_DUST_COUNT; i++) {
			            dust_pts[i].cell = rand() % (dust.num_of_cells);
			            dust_pts[i].x = rand() % (width + 10) - 5;
			            dust_pts[i].y = rand() % (height + 10) - 5;
			        }
			    }
			}

			public override CharPtr message( CharPtr message, ref int ret_int )
			{
				int sprite_no = 0;
			    ret_int = 0;
			    if (null==sprite_info)
			        return null;
			
			    printf("OldMovieLayer: got message '%s'\n", message);
			    if (null!=sscanf(message, "s|%d,%d,%d,%d,%d,%d", 
			               blur_level, noise_level, glow_level, 
			               scratch_level, dust_level, sprite_no)) {
			        if (blur_level < 0) blur_level = 0;
			        else if (blur_level > 3) blur_level = 3;
			        if (noise_level < 0) noise_level = 0;
			        else if (noise_level > 24) noise_level = 24;
			        if (glow_level < 0) glow_level = 0;
			        else if (glow_level > 24) glow_level = 24;
			        if (scratch_level < 0) scratch_level = 0;
			        else if (scratch_level > 400) scratch_level = 400;
			        if (dust_level < 0) dust_level = 0;
			        else if (dust_level > 400) dust_level = 400;
			        if ((sprite_no >= 0) && (sprite_no < MAX_SPRITE_NUM))
			            dust_sprite = sprite_info[sprite_no];
			        om_init();
			    }
			    return null;
			}
		}
		
		// Apply blur effect by averaging two offset copies of a source surface together.
		static void BlurOnSurface(SDL_Surface src, SDL_Surface dst, SDL_Rect clip, int rx, int ry, int width)
		{
		    // Calculate clipping bounds to avoid reading outside the source surface.
		    int srcx = clip.x - rx;
		    int srcy = clip.y - ry;
		    int length = ((srcx + clip.w > width) ? (width - srcx) : clip.w) * 4;
		    int rows = clip.h;
		    int skipfirstrows = (srcy < 0) ? -srcy : 0;
		    int srcp = SDL_Surface_get_pitch(src);
		    int dstp = SDL_Surface_get_pitch(dst);
		
		    SDL_LockSurface(src);
		    SDL_LockSurface(dst);
		    UnsignedCharPtr src1px = new UnsignedCharPtr((UnsignedCharPtr) SDL_Surface_get_pixels(src), + srcx * 4 + srcy * srcp);
		    UnsignedCharPtr src2px = new UnsignedCharPtr((UnsignedCharPtr) SDL_Surface_get_pixels(src), + clip.x * 4 + clip.y * srcp);
		    UnsignedCharPtr dstpx = new UnsignedCharPtr((UnsignedCharPtr) SDL_Surface_get_pixels(dst), + clip.x * 4 + clip.y * dstp);
		
		    // If the vertical offset is positive, we are reading one copy from (x, -1), so we need to
		    // skip the first scanline to avoid reading outside the source surface.
		    for (int i=skipfirstrows; i != 0; --i) {
		        --rows;
		        src1px.inc(srcp);
		        src2px.inc(srcp);
		        dstpx.inc(dstp);
		    }
		
		    // Blend the remaining scanlines.
		    while (rows-- != 0) {
		        AnimationInfo.imageFilterMean(src1px, src2px, dstpx, length);
		        src1px.inc(srcp);
				src2px.inc(srcp);
				dstpx.inc(dstp);
		    }
		
		    // If the horizontal offset is -1, the rightmost column has not been written to.
		    // Rectify that by copying it directly from the source image.
		    if (rx != 0 && (clip.x + clip.w >= width)) {
		    	Uint32Ptr r = new Uint32Ptr(SDL_Surface_get_pixels(src), + (width - 1) + clip.y * width);
		    	Uint32Ptr d = new Uint32Ptr(SDL_Surface_get_pixels(dst), + (width - 1) + clip.y * width);
		        while (clip.h-- != 0) {
		    		d[0] = r[0];
		    		d.inc(width);
		    		r.inc(width);
		        }
		    }
		
		    SDL_UnlockSurface(src);
		    SDL_UnlockSurface(dst);
		
		    // If we skipped the first scanlines, rectify that by copying directly from the source image.
		    if (0!=skipfirstrows) {
		        clip.h = skipfirstrows;
		        SDL_BlitSurface(src, clip, dst, clip);
		    }
		}
		
		public partial class OldMovieLayer {
			// Called every time the screen is refreshed.
			// Draws the background image with the old-movie effect applied, using the settings adopted at the
			// last call to updateOldMovie().
			public override void refresh(SDL_Surface surface, ref SDL_Rect clip)
			{
			    if (!initialized) return;
			
			    // Blur background.
			    // If no offset is applied, we can just copy the given surface directly.
			    // If an offset is present, we average the given surface with an offset version
			
			    if ( (rx != 0) || (ry != 0) ) {
			        SDL_BlitSurface(surface, clip, sprite.image_surface, clip);
			        BlurOnSurface(sprite.image_surface, surface, clip, rx, ry, width);
			    }
			
			    // Add noise and glow.
			    SDL_LockSurface(surface);
			    SDL_LockSurface(NoiseSurface[ns]);
			    SDL_LockSurface(GlowSurface);
			    UnsignedCharPtr g = new UnsignedCharPtr(SDL_Surface_get_pixels(GlowSurface),  + (gv * glow_level / 4) * SDL_Surface_get_pitch(GlowSurface));
			    int sp = SDL_Surface_get_pitch(surface);
			    if ((clip.x == 0) && (clip.y == 0) && (clip.w == width) && (clip.h == height)) {
			        // If no clipping rectangle is defined, we can apply the noise in one go.
			        UnsignedCharPtr s = (UnsignedCharPtr) SDL_Surface_get_pixels(surface);
			        if (noise_level > 0)
			            AnimationInfo.imageFilterSubFrom(s, (UnsignedCharPtr) SDL_Surface_get_pixels(NoiseSurface[ns]), sp * SDL_Surface_get_h(surface));
			        // Since the glow is stored as a single scanline for each level, we always apply
			        // the glow scanline by scanline.
			        if (glow_level > 0) {
			        	for (int i = height; i != 0; --i, s.inc(sp))
			                AnimationInfo.imageFilterAddTo(s, g, width * 4);
			        }
			    }
			    else {
			        // Otherwise we do everything scanline by scanline.
			        int length = clip.w * 4;
			        if (noise_level > 0) {
			            int np = SDL_Surface_get_pitch(NoiseSurface[ns]);
			            UnsignedCharPtr s = new UnsignedCharPtr(SDL_Surface_get_pixels(surface), + clip.x * 4 + clip.y * sp);
			            UnsignedCharPtr n = new UnsignedCharPtr(SDL_Surface_get_pixels(NoiseSurface[ns]), + clip.x * 4 + clip.y * np);
			            for (int i = clip.h; i != 0; --i, s.inc(sp), n.inc(np))
			                AnimationInfo.imageFilterSubFrom(s, n, length); // subtract noise
			        }
			        if (glow_level > 0) {
			        	UnsignedCharPtr s = new UnsignedCharPtr( SDL_Surface_get_pixels(surface), + clip.x * 4 + clip.y * sp);
			        	for (int i = clip.h; i != 0; --i, s.inc(sp))
			                AnimationInfo.imageFilterAddTo(s, g, length); // add glow
			        }
			    }
			    SDL_UnlockSurface(NoiseSurface[ns]);
			    SDL_UnlockSurface(GlowSurface);
			
			    // Add scratches.
			    if (scratch_level > 0)
			        for (int i = 0; i < MAX_SCRATCH_COUNT; i++)
			            scratches[i].draw(surface, clip);
			
			    // Add dust specks.
			    if (dust != null && (dust_level > 0)) {
			        for (int i=0; i<MAX_DUST_COUNT; i++) {
			            if ((rand() & 1023) < dust_level) {
			                dust.current_cell = dust_pts[i].cell;
			                dust.pos.x = dust_pts[i].x;
			                dust.pos.y = dust_pts[i].y;
			                drawTaggedSurface( surface, dust, ref clip );
			            }
			        }
			    }
			
			    // And we're done.
			    SDL_UnlockSurface(surface);
			
			}
		}
		
//		#endif //BPP16
		
		/*
		 * FuruLayer: for snow & hana layer effects (falling stuff)
		 * Emulation of Takashi Toyama's "snow.dll" & "hana.dll" NScripter filters
		 *
		 * C++ coding by Mion, Sep 2008
		 */
		public partial class FuruLayer : Layer {
			private const double FURU_RATE_COEF = 0.2;
			
			static float[] base_disp_table = null;
			static int furu_count = 0;
			static float[] fall_mult = new float[N_FURU_ELEMENTS]{0.9f, 0.7f, 0.6f};
			
			static void buildBaseDispTable()
			{
			    if (null!=base_disp_table) return;
			
			    base_disp_table = new float[FURU_AMP_TABLE_SIZE];
			    // a = sin? * Z(cos?)
			    // Z(z) = rate_z * z +1
			    for (int i=0; i<FURU_AMP_TABLE_SIZE; ++i) {
			    	float rad = (float)((float) i * M_PI * 2 / FURU_AMP_TABLE_SIZE);
			    	base_disp_table[i] = (float)(sin(rad) * (FURU_RATE_COEF * cos(rad) + 1));
			    }
			}
			
			public FuruLayer( int w, int h, bool animated, BaseReader br )
			{
			    width = w;
			    height = h;
			    tumbling = animated;
			    reader = br;
			
			    interval = fall_velocity = wind = amplitude = freq = angle = 0;
			    paused = halted = false;
			    max_sp_w = 0;
			    
			    initialized = false;
			}
			
			~FuruLayer(){
			    if (initialized) {
			        --furu_count;
			        if (furu_count == 0) {
			            //delete[] base_disp_table;
			            base_disp_table = null;
			        }
			    }
			}
			
			public void furu_init()
			{
			    for (int i=0; i<N_FURU_ELEMENTS; i++) {
			        elements[i].init();
			    }
			    angle = 0;
			    halted = false;
			    paused = false;
			
			    buildBaseDispTable();
			
			    ++furu_count;
			    initialized = true;
			}
			
			public override void update()
			{
			    if (initialized && !paused) {
			        if (amplitude != 0)
			            angle = (angle - freq + FURU_AMP_TABLE_SIZE) % FURU_AMP_TABLE_SIZE;
			        for (int j=0; j<N_FURU_ELEMENTS; ++j) {
			            Element cur = elements[j];
			            int i = cur.pstart;
			            int virt_w = width + max_sp_w;
			            while (i != cur.pend) {
			                cur.points[i].pt.x = (cur.points[i].pt.x + wind + virt_w) % virt_w;
			                cur.points[i].pt.y += cur.fall_speed;
			                ++(cur.points[i].pt.cell); cur.points[i].pt.cell %= cur.sprite.num_of_cells;
			                ++i; i %= FURU_ELEMENT_BUFSIZE;
			            }
			            if (!halted) {
			                if (--(cur.frame_cnt) <= 0) {
			                    int tmp = (cur.pend + 1) % FURU_ELEMENT_BUFSIZE;
			                    cur.frame_cnt += interval;
			                    if (tmp != cur.pstart) {
			                        // add a point for this element
			                        OscPt item = cur.points[cur.pend];
			                        item.pt.x = rand() % virt_w;
			                        item.pt.y = -(cur.sprite.pos.h);
			                        item.pt.type = j;
			                        item.pt.cell = 0;
			                        item.base_angle = rand() % FURU_AMP_TABLE_SIZE;
			                        cur.pend = tmp;
			                    }
			                }
			            }
			            while ((cur.pstart != cur.pend) &&
			                   (cur.points[cur.pstart].pt.y >= height))
			                ++(cur.pstart); cur.pstart %= FURU_ELEMENT_BUFSIZE;
			        }
			    }
			}
			
			public static void setStr( ref CharPtr dst, CharPtr src, int num=-1 )
			{
			    if ( null!=dst ) dst = null;//delete[] *dst;
			    dst = null;
			    
			    if ( null!=src ){
			        if (num >= 0){
			            dst = new char[ num + 1 ];
			            memcpy( dst, src, (uint)num );
			            (dst)[num] = '\0';
			        }
			        else{
			            dst = new char[ strlen( src ) + 1];
			            strcpy( dst, src );
			        }
			    }
			}
			
			public static SDL_Surface loadImage( CharPtr file_name, ref bool has_alpha, SDL_Surface surface, BaseReader br )
			{
				if ( null==file_name ) return null;
			    ulong length = br.getFileLength( file_name );
			
			    if ( length == 0 )
			        return null;
			    UnsignedCharPtr buffer = new UnsignedCharPtr(new byte[length]);
			    int location = 0;
			    br.getFile( file_name, buffer, ref location );
			    SDL_Surface tmp = IMG_Load_RW(SDL_RWFromMem( buffer, (int)length ), 1);
			
			#if false
			    char *ext = strrchr(file_name, '.');
			    if ( !tmp && ext && (!strcmp( ext+1, "JPG" ) || !strcmp( ext+1, "jpg" ) ) ){
			        fprintf( stderr, " *** force-loading a JPG image [%s]\n", file_name );
			        SDL_RWops *src = SDL_RWFromMem( buffer, length );
			        tmp = IMG_LoadJPG_RW(src);
			        SDL_RWclose(src);
			    }
			#endif
			    if ( null!=tmp /*&& has_alpha*/ ) has_alpha = 0!=SDL_Surface_get_format(tmp).Amask;
			
			    buffer = null;//delete[] buffer;
			    if ( null==tmp ){
			        fprintf( stderr, " *** can't load file [%s] ***\n", file_name );
			        return null;
			    }
			
			    SDL_Surface ret = SDL_ConvertSurface( tmp, SDL_Surface_get_format(surface), SDL_SWSURFACE );
			    SDL_FreeSurface( tmp );
			    return ret;
			}
			
			public void buildAmpTables()
			{
				float[] amp = new float[N_FURU_ELEMENTS];
			    amp[0] = (float) amplitude;
			    for (int i=1; i<N_FURU_ELEMENTS; ++i)
			    	amp[i] = (float)(amp[i-1] * 0.8);
			
			#if _MSC_VER
				{
			#endif
			    for (int i=0; i<N_FURU_ELEMENTS; ++i) {
			        Element cur = elements[i];
			        if (null==cur.amp_table)
			            cur.amp_table = new int[FURU_AMP_TABLE_SIZE];
			        for (int j=0; j<FURU_AMP_TABLE_SIZE; ++j)
			            cur.amp_table[j] = (int) (amp[i] * base_disp_table[j]);
			    }
			#if _MSC_VER
				}
			#endif
			}
			
			public void validate_params()
			{
			    int half_wx = width / 2;
			
			    if (interval < 1) interval = 1;
			    else if (interval > 10000) interval = 10000;
			    if (fall_velocity < 1) fall_velocity = 1;
			    else if (fall_velocity > height) fall_velocity = height;
			    for (int i=0; i<N_FURU_ELEMENTS; i++)
			        elements[i].fall_speed = (int)(fall_mult[i] * (fall_velocity+1));
			    if (wind < -half_wx) wind = -half_wx;
			    else if (wind > half_wx) wind = half_wx;
			    if (amplitude < 0) amplitude = 0;
			    else if (amplitude > half_wx) amplitude = half_wx;
			    if (amplitude != 0) buildAmpTables();
			    if (freq < 0) freq = 0;
			    else if (freq > 359) freq = 359;
			    //adjust the freq to range 0-FURU_AMP_TABLE_SIZE-1
			    freq = freq * FURU_AMP_TABLE_SIZE / 360;
			}
			
			public override CharPtr message( CharPtr message, ref int ret_int )
			{
				int[] num_cells = new int[3], tmp = new int[5];
				char[][] buf = new char[3][];
				for (int i = 0; i < buf.Length; ++i)
				{
					buf[i] = new char[128];
				}
			
			    CharPtr ret_str = null;
			    ret_int = 0;
			
			    if (null==sprite)
			        return null;
			
			    //printf("FuruLayer: got message '%s'\n", message);
			//Image loading
			    if (0==strncmp(message, "i|", 2)) {
			        max_sp_w = 0;
			        SDL_Surface ref_surface = SDL_CreateRGBSurface( SDL_SWSURFACE, 1, 1, 32, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000 );
			        if (tumbling) {
			        // "Hana"
			            if (null!=sscanf(message, "i|%d,%d,%d,%d,%d,%d",
			                       tmp[0], num_cells[0],
			                       tmp[1], num_cells[1],
			                       tmp[2], num_cells[2])) {
			                for (int i=0; i<3; i++) {
			                    elements[i].setSprite(new AnimationInfo(sprite_info[tmp[i]]));
			                    elements[i].sprite.num_of_cells = num_cells[i];
			                    if (elements[i].sprite.pos.w > max_sp_w)
			                        max_sp_w = elements[i].sprite.pos.w;
			                }
			            } else
			            if (null!=sscanf(message, "i|%120[^,],%d,%120[^,],%d,%120[^,],%d",
			        	                  new CharPtr(buf[0], 0), num_cells[0],
			                              new CharPtr(buf[1], 0), num_cells[1],
			                              new CharPtr(buf[2], 0), num_cells[2])) {
			                for (int i=0; i<3; i++) {
			                    bool has_alpha = false;
			                    SDL_Surface img = loadImage( new CharPtr(buf[i], 0), ref has_alpha, ref_surface, reader );
			                    AnimationInfo anim = new AnimationInfo();
			                    anim.num_of_cells = num_cells[i];
			                    anim.duration_list = new int[ anim.num_of_cells ];
			                    for (int j=anim.num_of_cells - 1; j>=0; --j )
			                        anim.duration_list[j] = 0;
			                    anim.loop_mode = 3; // not animatable
			                    anim.trans_mode = AnimationInfo.TRANS_TOPLEFT;
			                    setStr( ref anim.file_name, new CharPtr(buf[i], 0) );
			                    anim.setImage(anim.setupImageAlpha(img, null, has_alpha));
			                    elements[i].setSprite(anim);
			                    if (anim.pos.w > max_sp_w)
			                        max_sp_w = anim.pos.w;
			                }
			            }
			        } else {
			        // "Snow"
			            if (null!=sscanf(message, "i|%d,%d,%d", 
			                       tmp[0], tmp[1], tmp[2])) {
			                for (int i=0; i<3; i++) {
			                    elements[i].setSprite(new AnimationInfo(sprite_info[tmp[i]]));
			                    if (elements[i].sprite.pos.w > max_sp_w)
			                        max_sp_w = elements[i].sprite.pos.w;
			                }
			            } else if (null!=sscanf(message, "i|%[^,],%[^,],%[^,]",
			                          new CharPtr(buf[0], 0), new CharPtr(buf[1], 0), new CharPtr(buf[2], 0))) {
			                for (int i=0; i<3; i++) {
			                    UInt32 firstpix = 0;
			                    bool has_alpha = false;
			                    SDL_Surface img = loadImage( new CharPtr(buf[i], 0), ref has_alpha, ref_surface, reader );
			                    AnimationInfo anim = new AnimationInfo();
			                    anim.num_of_cells = 1;
			                    SDL_LockSurface( img );
			                    firstpix = (new Uint32Ptr(SDL_Surface_get_pixels(img)))[0] & ~AMASK;
			                    if (firstpix > 0) {
			                        anim.trans_mode = AnimationInfo.TRANS_TOPLEFT;
			                    } else {
			                        // if first pix is black, this is an "additive" sprite
			                        anim.trans_mode = AnimationInfo.TRANS_COPY;
			                        anim.blending_mode = AnimationInfo.BLEND_ADD;
			                    }
			                    SDL_UnlockSurface( img );
			                    setStr( ref anim.file_name, new CharPtr(buf[i], 0) );
			                    anim.setImage(anim.setupImageAlpha(img, null, has_alpha));
			                    elements[i].setSprite(anim);
			                    if (anim.pos.w > max_sp_w)
			                        max_sp_w = anim.pos.w;
			                }
			            }
			        }
			        SDL_FreeSurface(ref_surface);
			//Set Parameters
			    } else if (null!=sscanf(message, "s|%d,%d,%d,%d,%d", 
			                      interval, fall_velocity, wind, 
			                      amplitude, freq)) {
			        furu_init();
			        validate_params();
			//Transition (adjust) Parameters
			    } else if (null!=sscanf(message, "t|%d,%d,%d,%d,%d", 
			                      tmp[0], tmp[1], tmp[2], tmp[3], tmp[4])) {
			        interval += tmp[0];
			        fall_velocity += tmp[1];
			        wind += tmp[2];
			        amplitude += tmp[3];
			        freq += tmp[4];
			        validate_params();
			//Fill Screen w/Elements
			    } else if (0==strcmp(message, "f")) {
			        if (initialized) {
			            for (int j=0; j<N_FURU_ELEMENTS; j++) {
			                Element cur = elements[j];
			                int y = 0;
			                while (y < height) {
			                    int tmp_ = (cur.pend + 1) % FURU_ELEMENT_BUFSIZE;
			                    if (tmp_ != cur.pstart) {
			                    // add a point for each element
			                        OscPt item = cur.points[cur.pend];
			                        item.pt.x = rand() % (width + max_sp_w);
			                        item.pt.y = y;
			                        item.pt.type = j;
			                        item.pt.cell = rand() % cur.sprite.num_of_cells;
			                        item.base_angle = rand() % FURU_AMP_TABLE_SIZE;
			                        cur.pend = tmp_;
			                    }
			                    y += interval * cur.fall_speed;
			                }
			            }
			        }
			//Get Parameters
			    } else if (0==strcmp(message, "g")) {
			        ret_int = paused ? 1 : 0;
			        sprintf(new CharPtr(buf[0], 0), "s|%d,%d,%d,%d,%d", interval, fall_velocity,
			                wind, amplitude, (freq * 360 / FURU_AMP_TABLE_SIZE));
			        setStr( ref ret_str, new CharPtr(buf[0], 0));
			//Halt adding new elements
			    } else if (0==strcmp(message, "h")) {
			        halted = true;
			//Get number of elements displayed
			    } else if (0==strcmp(message, "n")) {
			        for (int i=0; i<N_FURU_ELEMENTS; i++)
			            ret_int += (elements[i].pend - elements[i].pstart + FURU_ELEMENT_BUFSIZE)
			                % FURU_ELEMENT_BUFSIZE;
			//Pause
			    } else if (0==strcmp(message, "p")) {
			        paused = true;
			//Restart
			    } else if (0==strcmp(message, "r")) {
			        paused = false;
			//eXtinguish
			    } else if (0==strcmp(message, "x")) {
			        for (int i=0; i<N_FURU_ELEMENTS; i++)
			            elements[i].clear();
			        initialized = false;
			    }
			    return ret_str;
			}
			
			public override void refresh(SDL_Surface surface, ref SDL_Rect clip)
			{
			    if (initialized) {
			        int virt_w = width + max_sp_w;
			        for (int j=0; j<N_FURU_ELEMENTS; j++) {
			            Element cur = elements[j];
			            if (null!=cur.sprite) {
			                cur.sprite.visible = true;
			                int n = (cur.pend - cur.pstart + FURU_ELEMENT_BUFSIZE) % FURU_ELEMENT_BUFSIZE;
			                int p = cur.pstart;
			                if (amplitude == 0) {
			                    //no need to mess with angles if no displacement
			                    for (int i=n; i>0; i--) {
			                        OscPt curpt = cur.points[p];
			                        ++p; p %= FURU_ELEMENT_BUFSIZE;
			                        cur.sprite.current_cell = curpt.pt.cell;
			                        cur.sprite.pos.x = ((curpt.pt.x + virt_w) % virt_w) - max_sp_w;
			                        cur.sprite.pos.y = curpt.pt.y;
			                        drawTaggedSurface( surface, cur.sprite, ref clip );
			                    }
			                } else {
			                    for (int i=n; i>0; i--) {
			                        OscPt curpt = cur.points[p];
			                        ++p; p %= FURU_ELEMENT_BUFSIZE;
			                        int disp_angle = (angle + curpt.base_angle + FURU_AMP_TABLE_SIZE) % FURU_AMP_TABLE_SIZE;
			                        cur.sprite.current_cell = curpt.pt.cell;
			                        cur.sprite.pos.x = ((curpt.pt.x + cur.amp_table[disp_angle] + virt_w) % virt_w) - max_sp_w;
			                        cur.sprite.pos.y = curpt.pt.y;
			                        drawTaggedSurface( surface, cur.sprite, ref clip );
			                    }
			                }
			            }
			        }
			    }
			}
		}
		
//		#endif // ndef NO_LAYER_EFFECTS


	}
}
