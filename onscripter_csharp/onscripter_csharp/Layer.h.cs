/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:12
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
		 *  Layer.h - Base class for effect layers for ONScripter-EN
		 *
		 *  Copyright (c) 2009 "Uncle" Mion Sonozaki
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
		
		//#ifndef __LAYER_H__
		//#define __LAYER_H__
		
		//#include "AnimationInfo.h"
		//#include "BaseReader.h"
		
		//#ifndef NO_LAYER_EFFECTS
		
		public class Pt { public int x; public int y; public int type; public int cell; };
		
		public abstract class Layer
		{
		    public BaseReader reader;
		    public AnimationInfo[] sprite_info; public AnimationInfo sprite;
		    public int width, height;
		
		    ~Layer(){}
		    
		    public void setSpriteInfo( AnimationInfo[] sinfo, AnimationInfo anim ){
		        sprite_info = sinfo;
		        sprite = anim;
		    }
		    public abstract void update( );
		    public abstract CharPtr message( CharPtr message, ref int ret_int );
		    public abstract void refresh( SDL_Surface surface, ref SDL_Rect clip );
		}
//		
//		#ifndef BPP16
//		// OldMovieLayer: emulation of Takashi Toyama's "oldmovie.dll" NScripter plugin filter
//		class OldMovieLayer : public Layer
//		{
//		public:
//		    OldMovieLayer( int w, int h );
//		    ~OldMovieLayer();
//		    void update();
//		    char* message( const char *message, int &ret_int );
//		    void refresh( SDL_Surface* surface, SDL_Rect &clip );
//		
//		private:
//		    // message parameters
//		    int blur_level;
//		    int noise_level;
//		    int glow_level;
//		    int scratch_level;
//		    int dust_level;
//		    AnimationInfo *dust_sprite;
//		    AnimationInfo *dust;
//		
//		    Pt *dust_pts;
//		    int rx, ry, // Offset of blur (second copy of background image)
//		        ns;     // Current noise surface
//		    int gv, // Current glow level
//		        go; // Glow delta: flips between 1 and -1 to fade the glow in and out.
//		    bool initialized;
//		
//		    void om_init();
//		    //void BlendOnSurface(SDL_Surface* src, SDL_Surface* dst, SDL_Rect clip);
//		};
//		#endif //BPP16
		
		public const int N_FURU_ELEMENTS = 3;
		public const int FURU_ELEMENT_BUFSIZE = 512; // should be a power of 2
		public const int FURU_AMP_TABLE_SIZE = 256; // should also be power of 2, it helps
		
		// FuruLayer: emulation of Takashi Toyama's "snow.dll" and "hana.dll" NScripter plugin filters
		public partial class FuruLayer : Layer
		{
//		public:
//		    FuruLayer( int w, int h, bool animated, BaseReader *br=NULL );
//		    ~FuruLayer();
//		    void update();
//		    char* message( const char *message, int &ret_int );
//		    void refresh( SDL_Surface* surface, SDL_Rect &clip );
//		
//		private:
		    public bool tumbling; // true (hana) or false (snow)
		
		    // message parameters
		    public int interval; // 1 ~ 10000; # frames between a new element release
		    public int fall_velocity; // 1 ~ screen_height; pix/frame
		    public int wind; // -screen_width/2 ~ screen_width/2; pix/frame 
		    public int amplitude; // 0 ~ screen_width/2; pix/frame
		    public int freq; // 0 ~ 359; degree/frame
		    public int angle;
		    public bool paused, halted;
		
		    public class OscPt { // point plus base oscillation angle
		        public int base_angle;
		        public Pt pt = new Pt();
		    }
		    public class Element {
		        public AnimationInfo sprite;
		        public int[] amp_table;
		        // rolling buffer
		        public OscPt[] points;
		        public int pstart, pend, frame_cnt, fall_speed;
		        public Element(){
		            sprite = null;
		            amp_table = null;
		            points = null;
		            pstart = pend = frame_cnt = fall_speed = 0;
		        }
		        ~Element(){
		            if (null!=sprite) sprite = null;//delete sprite;
		            if (null!=amp_table) amp_table = null;//delete[] amp_table;
		            if (null!=points) points = null;//delete[] points;
		        }
		        public void init(){
		            if (null==points) points = new OscPt[FURU_ELEMENT_BUFSIZE];
		            pstart = pend = frame_cnt = 0;
		        }
		        public void clear(){
		            if (null!=sprite) sprite = null;//delete sprite;
		            sprite = null;
		            if (null!=amp_table) amp_table = null;//delete[] amp_table;
		            amp_table = null;
		            if (null!=points) points = null;//delete[] points;
		            points = null;
		            pstart = pend = frame_cnt = 0;
		        }
		        public void setSprite(AnimationInfo anim){
		            if (null!=sprite) sprite = null;//delete sprite;
		            sprite = anim;
		        }
			}
			public Element[] elements = elements_init();
			private static Element[] elements_init() {
				Element[] result = new Element[N_FURU_ELEMENTS];
				for (int i = 0; i < result.Length; ++i)
				{
					result[i] = new Element();
				}
				return result;
			}
		    public int max_sp_w;
		
		    public bool initialized;
		
//		    void furu_init();
//		    void validate_params();
//		    void buildAmpTables();
		}
		
//		#endif //ndef NO_LAYER_EFFECTS
//		
//		#endif // __LAYER_H__
	}
}
