/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 13:20
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
		 *  resize_image.cpp - resize image using smoothing and resampling
		 *
		 *  Copyright (c) 2001-2010 Ogapee. All rights reserved.
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
		
		// Modified by Uncle Mion (UncleMion@gmail.com) Nov 2009 - Jan 2010,
		//   to account for multicell images during resizing and optimize code
		
//		#include <stdio.h>
//		#include <string.h>
		
		private static UnsignedLongPtr pixel_accum=null;
		private static UnsignedLongPtr pixel_accum_num=null;
		private static int pixel_accum_size=0;
		private static ulong[] tmp_acc = new ulong[4];
		private static ulong[] tmp_acc_num = new ulong[4];
		
		private static void calcWeightedSumColumnInit(ref UnsignedCharPtr src,
		                                      int interpolation_height,
		                                      int image_width, int image_height,
		                                      int image_pixel_width, int byte_per_pixel)
		{
		    int y_end   = -interpolation_height/2+interpolation_height;
		
		    memset(pixel_accum, 0, (uint)(image_width*byte_per_pixel*sizeof(ulong)));
		    memset(pixel_accum_num, 0, (uint)(image_width*byte_per_pixel*sizeof(ulong)));
		    for (int s=0 ; s<byte_per_pixel ; s++){
		        for (int i=0 ; i<y_end-1 ; i++){
		            if (i >= image_height) break;
		            UnsignedLongPtr pa = new UnsignedLongPtr(pixel_accum, + image_width*s);
		            UnsignedLongPtr pan = new UnsignedLongPtr(pixel_accum_num, + image_width*s);
		            UnsignedCharPtr p = new UnsignedCharPtr(src, +image_pixel_width*i+s);
		            for (int j=image_width ; j!=0 ; j--, p.inc(byte_per_pixel)){
		            	pa[0] += p[0];pa.inc();
		            	pan[0]++; pan.inc();
		            }
		        }
		    }
		}
		
		private static void calcWeightedSumColumn(ref UnsignedCharPtr src, int y,
		                                  int interpolation_height,
		                                  int image_width, int image_height,
		                                  int image_pixel_width, int byte_per_pixel)
		{
		    int y_start = y-interpolation_height/2;
		    int y_end   = y-interpolation_height/2+interpolation_height;
		
		    for (int s=0 ; s<byte_per_pixel ; s++){
		        if ((y_start-1)>=0 && (y_start-1)<image_height){
		    		UnsignedLongPtr pa = new UnsignedLongPtr(pixel_accum, + image_width*s);
		    		UnsignedLongPtr pan = new UnsignedLongPtr(pixel_accum_num, + image_width*s);
		    		UnsignedCharPtr p = new UnsignedCharPtr(src, +image_pixel_width*(y_start-1)+s);
		    		for (int j=image_width ; j!=0 ; j--, p.inc(byte_per_pixel)){
		    			pa[0] -= p[0]; pa.inc();
		    			pan[0]--; pan.inc();
		            }
		        }
		        
		        if ((y_end-1)>=0 && (y_end-1)<image_height){
		    		UnsignedLongPtr pa = new UnsignedLongPtr(pixel_accum, + image_width*s);
		    		UnsignedLongPtr pan = new UnsignedLongPtr(pixel_accum_num, + image_width*s);
		    		UnsignedCharPtr p = new UnsignedCharPtr(src, +image_pixel_width*(y_end-1)+s);
		    		for (int j=image_width ; j!=0 ; j--, p.inc(byte_per_pixel)){
		            	pa[0] += p[0];pa.inc();
		            	pan[0]++; pan.inc();
		            }
		        }
		    }
		}
		
		private static void calcWeightedSum(ref UnsignedCharPtr dst, int x_start, int x_end,
		                            int image_width, int cell_start, int next_cell_start,
		                            int byte_per_pixel)
		{
		    for (int s=0 ; s<byte_per_pixel ; s++){
		        // avoid interpolating data from other cells or outside the image
		        if (x_start>=cell_start && x_start<next_cell_start){
		            tmp_acc[s] -= pixel_accum[image_width*s+x_start];
		            tmp_acc_num[s] -= pixel_accum_num[image_width*s+x_start];
		        }
		        if (x_end>=cell_start && x_end<next_cell_start){
		            tmp_acc[s] += pixel_accum[image_width*s+x_end];
		            tmp_acc_num[s] += pixel_accum_num[image_width*s+x_end];
		        }
		        switch (tmp_acc_num[s]){
		            //avoid a division op if possible
		            case 1: dst[0] = (byte)tmp_acc[s]; dst.inc();
		                    break;
		            case 2: dst[0] = (byte)(tmp_acc[s]>>1); dst.inc();
		                    break;
		            default:
		            case 3: dst[0] = (byte)(tmp_acc[s]/tmp_acc_num[s]); dst.inc();
		                    break;
		            case 4: dst[0] = (byte)(tmp_acc[s]>>2); dst.inc();
		                    break;
		        }
		    }
		}
		
		public void resizeImage( UnsignedCharPtr dst_buffer, int dst_width, int dst_height, int dst_total_width,
		                  UnsignedCharPtr src_buffer, int src_width, int src_height, int src_total_width,
		                  int byte_per_pixel, UnsignedCharPtr tmp_buffer, int tmp_total_width,
		                  int num_cells, bool no_interpolate )
		{
		    if (dst_width == 0 || dst_height == 0) return;
		    
		    UnsignedCharPtr tmp_buf = new UnsignedCharPtr(tmp_buffer);
		    UnsignedCharPtr src_buf = new UnsignedCharPtr(src_buffer);
		
		    int i, j, s, c;
		
		    int mx=0, my=0;
		
		    if ( src_width  > 1 ) mx = byte_per_pixel;
		    if ( src_height > 1 ) my = tmp_total_width;
		
		    int interpolation_width = src_width / dst_width;
		    if ( interpolation_width == 0 ) interpolation_width = 1;
		    int interpolation_height = src_height / dst_height;
		    if ( interpolation_height == 0 ) interpolation_height = 1;
		
		    int cell_width = src_width / num_cells;
		    src_width = cell_width * num_cells; //in case width is not a multiple of num_cells
		    int tmp_offset = tmp_total_width - src_width * byte_per_pixel;
		
		    if (pixel_accum_size < src_width*byte_per_pixel){
		        pixel_accum_size = src_width*byte_per_pixel;
		        if (null!=pixel_accum) pixel_accum = null;//delete[] pixel_accum;
		        pixel_accum = new UnsignedLongPtr(new ulong[pixel_accum_size]);
		        if (null!=pixel_accum_num) pixel_accum_num = null;//delete[] pixel_accum_num;
		        pixel_accum_num = new UnsignedLongPtr(new ulong[pixel_accum_size]);
		    }
		    /* smoothing */
		    if (!no_interpolate && (byte_per_pixel >= 3)){
		        calcWeightedSumColumnInit(ref src_buf, interpolation_height, src_width,
		                                  src_height, src_total_width, byte_per_pixel );
		        for ( i=0 ; i<src_height ; i++ ){
		            calcWeightedSumColumn(ref src_buf, i, interpolation_height, src_width,
		                                  src_height, src_total_width, byte_per_pixel );
		            for ( c=0 ; c<src_width ; c+=cell_width ) {
		                // do a separate set of smoothings for each cell,
		                // to avoid interpolating data from other cells
		                for ( s=0 ; s<byte_per_pixel ; s++ ){
		                    tmp_acc[s]=0;
		                    tmp_acc_num[s]=0;
		                    for (j=0 ; j<-interpolation_width/2+interpolation_width-1 ; j++){
		                        if (j >= cell_width) break;
		                        tmp_acc[s] += pixel_accum[src_width*s+c+j];
		                        tmp_acc_num[s] += pixel_accum_num[src_width*s+c+j];
		                    }
		                }
		
		                int x_start = c - interpolation_width/2 - 1;
		                int x_end   = x_start + interpolation_width;
		                for ( j=cell_width ; j!=0 ; j--, x_start++, x_end++ )
		                    calcWeightedSum(ref tmp_buf, x_start, x_end,
		                                    src_width, c, c+cell_width,
		                                    byte_per_pixel );
		            }
		    		tmp_buf.inc(tmp_offset);
		        }
		    }
		    else{
		        tmp_buffer = src_buffer;
		    }
		    
		    /* resampling */
		    int[] dst_to_src = new int[dst_width]; //lookup table for horiz resampling loop
		    for ( j=0 ; j<dst_width ; j++ )
		        dst_to_src[j] = (j<<3) * src_width / dst_width;
		    UnsignedCharPtr dst_buf = new UnsignedCharPtr(dst_buffer);
		    if (!no_interpolate && (byte_per_pixel >= 3)){
		        for ( i=0 ; i<dst_height ; i++ ){
		            int y = (i<<3) * src_height / dst_height;
		            int dy = y & 0x7;
		            y >>= 3;
		            //avoid resampling outside the image
		            int iy = 0;
		            if (y<src_height-1) iy = my;
		
		            for ( j=0 ; j<dst_width ; j++ ){
		                int x = dst_to_src[j];
		                int dx = x & 0x7;
		                x >>= 3;
		                //avoid resampling from outside the current cell
		                int ix = mx;
		                if (((x+1)%cell_width)==0) ix = 0;
		
		                int k = tmp_total_width * y + x * byte_per_pixel;
		
		                for ( s=byte_per_pixel ; s!=0 ; s--, k++ ){
		                    uint p;
		                    p =  (uint)((8-dx)*(8-dy)*tmp_buffer[ k ]);
		                    p =  (uint)(p +(  dx *(8-dy)*tmp_buffer[ k+ix ]));
		                    p =  (uint)(p +( (8-dx)*   dy *tmp_buffer[ k+iy ]));
		                    p =  (uint)(p +( dx *   dy *tmp_buffer[ k+ix+iy ]));
		                    dst_buf[0] = (byte)(p>>6); dst_buf.inc();
		                }
		            }
		            for ( j=dst_total_width - dst_width*byte_per_pixel ; j>0 ; j-- ) {
		            	dst_buf[0] = 0; dst_buf.inc();
		            }
		        }
		    }
		    else{
		        for ( i=0 ; i<dst_height ; i++ ){
		            int y = (i<<3) * src_height / dst_height;
		            y >>= 3;
		
		            for ( j=0 ; j<dst_width ; j++ ){
		                int x = dst_to_src[j] >> 3;
		                int k = src_total_width * y + x * byte_per_pixel;
		
		                for ( s=byte_per_pixel ; s!=0 ; s--, k++ ){
		                	dst_buf[0] = tmp_buffer[ k ]; dst_buf.inc();
		                }
		            }
		            for ( j=dst_total_width - dst_width*byte_per_pixel ; j!=0 ; j-- ) {
		            	dst_buf[0] = 0; dst_buf.inc();
		            }
		        }
		    }
		    dst_to_src = null;//delete[] dst_to_src;
		
		    /* pixels at the corners (of each cell) are preserved */
		    int dst_cell_width = byte_per_pixel * dst_width / num_cells;
		    cell_width *= byte_per_pixel;
		    for ( c=0 ; c<num_cells ; c++ ){
		        for ( i=0 ; i<byte_per_pixel ; i++ ){
		            dst_buffer[c*dst_cell_width+i] = src_buffer[c*cell_width+i];
		            dst_buffer[(c+1)*dst_cell_width-byte_per_pixel+i] =
		                src_buffer[(c+1)*cell_width-byte_per_pixel+i];
		            dst_buffer[(dst_height-1)*dst_total_width+c*dst_cell_width+i] =
		                src_buffer[(src_height-1)*src_total_width+c*cell_width+i];
		            dst_buffer[(dst_height-1)*dst_total_width+(c+1)*dst_cell_width-byte_per_pixel+i] =
		                src_buffer[(src_height-1)*src_total_width+(c+1)*cell_width-byte_per_pixel+i];
		        }
		    }
		}
	
	}
}
