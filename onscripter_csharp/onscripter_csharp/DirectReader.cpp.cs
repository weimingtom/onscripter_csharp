/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-11
 * Time: 18:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	public partial class ONScripter
	{
		//$Id:$ -*- C++ -*-
		/*
		 *  DirectReader.cpp - Reader from independent files for ONScripter-EN
		 *
		 *  Copyright (c) 2001-2010 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2007-2010 "Uncle" Mion Sonozaki
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
		
		// Modified by Mion, December 2009, to support creating new archives
		// via nsamake
		
//		#include "DirectReader.h"
//		#if !defined(WIN32) && !defined(MACOS9) && !defined(PSP) && !defined(__OS2__)
//		#include <dirent.h>
//		#endif
//		
//		#define IS_TWO_BYTE(x) \
//		        ( ((x) & 0xe0) == 0xe0 || ((x) & 0xe0) == 0x80 )
//		
//		#if defined(WIN32)
//		#else
//		extern unsigned short convSJIS2UTF16( unsigned short in );
//		extern int convUTF16ToUTF8( unsigned char dst[4], unsigned short src );
//		#endif
//		
//		#ifdef WIN32
//		//Mion: support for non-ASCII (SJIS) filenames
//		#include <windows.h>
//		#include <wchar.h>
//		#endif
//		
//		#if defined(MACOSX) || defined(LINUX) || defined(UTF8_FILESYSTEM)
//		#define RECODING_FILENAMES
//		#ifdef MACOSX
//		#include "cocoa_encoding.h"
//		#endif
//		#endif
//		
//		#ifndef SEEK_END
//		#define SEEK_END 2
//		#endif
//		
//		#define READ_LENGTH 4096
//		#define WRITE_LENGTH 5000
		
		private const int EI = 8;
		private const int EJ = 4;
		private const int P = 1;  /* If match length <= P then output one character */
		private const int N = (1 << EI);  /* buffer size */
		private const int F = ((1 << EJ) + P);  /* lookahead buffer size */
		
		public partial class DirectReader : BaseReader
		{
			public DirectReader(DirPaths path, UnsignedCharPtr key_table )
			{
			    file_full_path = null;
			    file_sub_path = null;
			    file_path_len = 0;
			
			    capital_name = new char[MAX_FILE_NAME_LENGTH*2+1];
			    capital_name_tmp = new char[MAX_FILE_NAME_LENGTH*3+1];
			
			    archive_path = path;
			
			    int i;
			    if (null!=key_table){
			        key_table_flag = true;
			        for (i=0 ; i<256 ; i++) this.key_table[i] = key_table[i];
			    }
			    else{
			        key_table_flag = false;
			        for (i=0 ; i<256 ; i++) this.key_table[i] = (byte) i;
			    }
			
			    read_buf = new UnsignedCharPtr(new byte[READ_LENGTH]);
			    decomp_buffer = new UnsignedCharPtr(new byte[N*2]);
			    decomp_buffer_len = N*2;
			
			    last_registered_compression_type = root_registered_compression_type;
			    registerCompressionType( "SPB", SPB_COMPRESSION );
			    registerCompressionType( "JPG", NO_COMPRESSION );
			    registerCompressionType( "GIF", NO_COMPRESSION );
			}
			
			~DirectReader()
			{
			    if (null!=file_full_path) file_full_path = null;//delete[] file_full_path;
			    if (null!=file_sub_path)  file_sub_path = null;//delete[] file_sub_path;
			    capital_name = null;//delete[] capital_name;
			    capital_name_tmp = null;//delete[] capital_name_tmp;
			
			    read_buf = null;//delete[] read_buf;
			    decomp_buffer = null;//delete[] decomp_buffer;
			    
			    last_registered_compression_type = root_registered_compression_type.next;
			    while ( null!=last_registered_compression_type ){
			        RegisteredCompressionType cur = last_registered_compression_type;
			        last_registered_compression_type = last_registered_compression_type.next;
			        cur = null;//delete cur;
			    }
			}
			
			public bool hasTwoByteChar(CharPtr str)
			{
				CharPtr ptr = new CharPtr(str);
				while (ptr[0] != 0) {
					if (IS_TWO_BYTE(ptr[0]) )
			            return true;
			        ptr.inc();
			    }
			    return false;
			}
			
			public FILEPtr fopen(CharPtr path, CharPtr mode)
			{
				//NOTE: path is likely SJIS, but if called by getFileHandle on
			    //      a non-Windows system, it could be UTF-8 or EUC-JP
			    FILEPtr fp = null;
			
			    uint len = archive_path.max_path_len() + strlen(path) + 1;
			
			    if (file_path_len < len){
			        file_path_len = len;
			        if (null!=file_full_path) file_full_path = null;//delete[] file_full_path;
			        file_full_path = new char[file_path_len];
			        if (null!=file_sub_path) file_sub_path = null;//delete[] file_sub_path;
			        file_sub_path = new char[file_path_len];
			    }
			    for (int n=0; n<archive_path.get_num_paths(); n++) {
			        sprintf( file_full_path, "%s%s", archive_path.get_path(n), path );
			        //printf("filename: \"%s\": ", file_full_path);
			        fp = fopen( file_full_path, mode );
			        //printf("%s\n", fp ? "found" : "not found");
			        if (null!=fp) return fp;
			#if true//WIN32
			        // Windows uses UTF-16, so convert for Japanese characters
			        else if (hasTwoByteChar(file_full_path)) {
			            UnsignedShortPtr u16_tmp, umode;
			            //convert the file path to from Shift-JIS to Wide chars (Unicode)
			            int wc_size = MultiByteToWideChar(932, 0, file_full_path, -1, null, 0);
			            u16_tmp = new UnsignedShortPtr(new ushort[wc_size]);
			            MultiByteToWideChar(932, 0, file_full_path, -1, u16_tmp, wc_size);
			            //need to convert the file mode too
			            wc_size = MultiByteToWideChar(932, 0, mode, -1, null, 0);
			            umode = new UnsignedShortPtr(new ushort[wc_size]);
			            MultiByteToWideChar(932, 0, mode, -1, umode, wc_size);
			            fp = _wfopen( u16_tmp, umode );
			            //printf("checking utf16 filename: %s\n", fp ? "found" : "not found");
			            u16_tmp = null;//delete[] u16_tmp;
			            umode = null;//delete[] umode;
			            if (null!=fp) return fp;
			        }
			#endif
			    }
			
			#if false//!WIN32 && !MACOS9 && !PSP && !__OS2__
			    // If the file wasn't found, try a case-insensitive search.
			    char *cur_p = NULL;
			    DIR *dp = NULL;
			    
			    len = 0;
			    int n = archive_path->get_num_paths();
			    int i=1;
			    if (n > 0)
			        len = strlen(archive_path->get_path(0));
			    if (len > 0) {
			        dp = opendir(archive_path->get_path(0));
			        sprintf( file_full_path, "%s%s", archive_path->get_path(0), path );
			    } else {
			        dp = opendir(".");
			        sprintf( file_full_path, "%s", path );
			    }
			    cur_p = file_full_path+len;
			
			    struct dirent *entp = NULL;
			    while (1){
			        if (dp == NULL) {
			            if (i < n) {
			                len = strlen(archive_path->get_path(i));
			                dp = opendir(archive_path->get_path(i));
			                sprintf( file_full_path, "%s%s",
			                        archive_path->get_path(i), path );
			                cur_p = file_full_path+len;
			                i++;
			            } else
			                return NULL;
			        }
			        if (dp == NULL) continue;
			
			        char *delim_p = NULL;
			        while(1){
			            delim_p = strchr( cur_p, (char)DELIMITER );
			            if (delim_p != cur_p) break;
			            cur_p++;
			        }
			        
			        if (delim_p) len = delim_p - cur_p;
			        else         len = strlen(cur_p);
			        memcpy(file_sub_path, cur_p, len);
			        file_sub_path[len] = '\0';
			        
			        while ( (entp = readdir(dp)) != NULL ){
			            if ( !strcasecmp( file_sub_path, entp->d_name ) ){
			                memcpy(cur_p, entp->d_name, len);
			                break;
			            }
			        }
			        closedir( dp );
			        dp = NULL;
			
			        if (entp == NULL) continue;
			        if (delim_p == NULL) break;
			
			        memcpy(file_sub_path, file_full_path, delim_p-file_full_path);
			        file_sub_path[delim_p-file_full_path]='\0';
			        dp = opendir(file_sub_path);
			
			        cur_p = delim_p+1;
			    }
			    if (entp == NULL) return NULL;
			
			    fp = ::fopen( file_full_path, mode );
			#endif
			
			    return fp;
			}
			
			public byte readChar( FILEPtr fp )
			{
				//FIXME: added
				UnsignedCharPtr ret = new UnsignedCharPtr(new byte[1]);
				
			    if (fread( ret, 1, 1, fp ) == 1)
			    	ret[0] = key_table[ret[0]];
			
			    return ret[0];
			}
			
			public ushort readShort( FILEPtr fp )
			{
				ushort ret = 0;
				UnsignedCharPtr buf = new UnsignedCharPtr(new byte[2]);
			    
			    if (fread( buf, 2, 1, fp ) == 1)
			    	ret = (ushort)(key_table[buf[0]] << 8 | key_table[buf[1]]);
			
			    return ret;
			}
			
			public ulong readLong( FILEPtr fp )
			{
				ulong ret = 0;
				UnsignedCharPtr buf = new UnsignedCharPtr(new byte[4]);
			
			    if (fread( buf, 4, 1, fp ) == 1) {
			        ret = key_table[buf[0]];
			        ret = ret << 8 | key_table[buf[1]];
			        ret = ret << 8 | key_table[buf[2]];
			        ret = ret << 8 | key_table[buf[3]];
			    }
			
			    return ret;
			}
			
			public void writeChar( FILEPtr fp, byte ch )
			{
				//FIXME: added
				UnsignedCharPtr buf = new UnsignedCharPtr(new byte[1]);
				buf[0] = (byte)(ch & 0xff);
				
			    if (fwrite( buf, 1, 1, fp ) != 1)
			        fputs("Warning: writeChar failed\n", stderr);
			}
			
			public void writeShort( FILEPtr fp, ushort ch )
			{
				UnsignedCharPtr buf = new UnsignedCharPtr(new byte[2]);
			
				buf[0] = (byte)((ch>>8) & 0xff);
			    buf[1] = (byte)(ch & 0xff);
			    if (fwrite( buf, 2, 1, fp ) != 1)
			        fputs("Warning: writeShort failed\n", stderr);
			}
			
			public void writeLong( FILEPtr fp, ulong ch )
			{
				UnsignedCharPtr buf = new UnsignedCharPtr(new byte[4]);
			    
			    buf[0] = (byte)((ch>>24) & 0xff);
			    buf[1] = (byte)((ch>>16) & 0xff);
			    buf[2] = (byte)((ch>>8)  & 0xff);
			    buf[3] = (byte)(ch & 0xff);
			    if (fwrite( buf, 4, 1, fp ) != 1)
			        fputs("Warning: writeLong failed\n", stderr);
			}
			
			public ushort swapShort( ushort ch )
			{
				return (ushort)(((ch & 0xff00) >> 8) | ((ch & 0x00ff) << 8));
			}
			
			public ulong swapLong( ulong ch )
			{
				return ((ch & 0xff000000) >> 24) | ((ch & 0x00ff0000) >> 8) |
			           ((ch & 0x0000ff00) << 8) | ((ch & 0x000000ff) << 24);
			}
			
			public override int open( CharPtr name )
			{
			    return 0;
			}
			
			public override int close()
			{
			    return 0;
			}

			
			public override CharPtr getArchiveName()
			{
				return "direct";
			}
			
			public override int getNumFiles()
			{
			    return 0;
			}
			    
			public override void registerCompressionType( CharPtr ext, int type )
			{
			    last_registered_compression_type.next = new RegisteredCompressionType(ext, type);
			    last_registered_compression_type = last_registered_compression_type.next;
			}
			    
			public int getRegisteredCompressionType( CharPtr file_name )
			{
				CharPtr ext_buf = new CharPtr(file_name, (int)+ strlen(file_name));
				while( ext_buf[0] != '.' && ext_buf != file_name ) ext_buf.dec();
				ext_buf.inc();
			    
			    strcpy( capital_name, ext_buf );
			    for ( uint i=0 ; i<strlen(ext_buf)+1 ; i++ )
			        if ( capital_name[i] >= 'a' && capital_name[i] <= 'z' )
			    		capital_name[i] = (char)(capital_name[i] + ('A' - 'a'));
			    
			    RegisteredCompressionType reg = root_registered_compression_type.next;
			    while (null!=reg){
			        if ( 0==strcmp( capital_name, reg.ext ) ) return reg.type;
			
			        reg = reg.next;
			    }
			
			    return NO_COMPRESSION;
			}
			    
			public override FileInfo getFileByIndex( uint index )
			{
				DirectReader.FileInfo fi = new DirectReader.FileInfo();
				memset(fi, 0, (uint)sizeof_DirectReader_FileInfo());
			    return fi;
			}
			
			public FILEPtr getFileHandle( CharPtr file_name, ref int compression_type, ref uint length )
			{
				//NOTE: file_name is assumed to use SJIS encoding
			    FILEPtr fp = null;
			    uint i;
			
			    compression_type = NO_COMPRESSION;
			    uint len = strlen( file_name );
			    if ( len > MAX_FILE_NAME_LENGTH ) len = MAX_FILE_NAME_LENGTH;
			    memcpy( capital_name, file_name, len );
			    capital_name[ len ] = '\0';
			//Mion: need to do more careful SJIS checking in this next part
			    bool has_nonascii = false;
			    for ( i=0 ; i<len ; i++ ){
			        if ((byte)capital_name[i] >= 0x80)
			            has_nonascii = true;
			        if (IS_TWO_BYTE(capital_name[i])) {
			            i++;
			        } else if ( capital_name[i] == '/' || capital_name[i] == '\\' )
			            capital_name[i] = (char)DELIMITER;
			    }
			    //added
			    __unused(has_nonascii);
			
			    // *** Haeleth rant follows ***
			    // On non-Windows platforms it's hard to predict how filenames
			    // will be encoded.
			    //
			    // What does this mean?  Simple: if you have any choice in the
			    // matter, DON'T USE JAPANESE IN FILENAMES.  It's nothing but
			    // trouble.  Games that directly access files with Japanese names
			    // should be considered not just essentially unsupported, but
			    // unsupportable.  We do our best, but it's basically impossible
			    // to guarantee that someone on OS X or Linux will manage to
			    // install the game in such a way that it's even _possible_ to
			    // access the filenames (e.g. they may be mangled irretrievably
			    // during the installation process, if the user has a Latin-1
			    // locale...)
			    //
			    // But we DO do our best, which is to try the likely options in
			    // turn; this is inefficent, but hopefully a bit more robust than
			    // requiring a single encoding to be compiled in.
			#if RECODING_FILENAMES
			    // First try Shift_JIS, if we need to
			    if (has_nonascii && !(fp = fopen(capital_name, "rb"))) {
			        // Try UTF-8
			        convertFromSJISToUTF8(capital_name_tmp, capital_name);
			        if ((fp = fopen(capital_name_tmp, "rb"))) {
			            strcpy(capital_name, capital_name_tmp);
			            len = strlen(capital_name);
			        }
			#if LINUX
			        else {
			            // Try EUC-JP
			            convertFromSJISToEUC(capital_name);
			            if ((fp = fopen(capital_name, "rb"))) {
			                len = strlen(capital_name);
			            } else {
			                //fprintf(stderr, "Warning: couldn't access %s even after "
			                //    "transcoding the filename.\n", file_name);
			            }
			        }
			#endif
			    }
			    if (fp) fclose(fp);
			#endif
			    
			    length = 0;
			    if ( ((fp = fopen( capital_name, "rb" )) != null) && (len >= 3) ){
			        compression_type = getRegisteredCompressionType( capital_name );
			        if ( compression_type == SPB_COMPRESSION ){
			            length = getDecompressedFileLength( compression_type, fp, 0 );
			        }
			        else{
			            fseek( fp, 0, SEEK_END );
			            length = (uint)ftell( fp );
			        }
			    }
			
			    return fp;
			}
			
			public override uint getFileLength( CharPtr file_name )
			{
				int compression_type = 0;
			    uint len = 0;
			    FILEPtr fp = getFileHandle( file_name, ref compression_type, ref len );
			
			    if ( null!=fp ) fclose( fp );
			    
			    return len;
			}
			
			public override uint getFile( CharPtr file_name, UnsignedCharPtr buffer,
			                              ref int location, bool is_location_null=true )
			{
				int compression_type = 0;
			    uint len = 0, c = 0, total = 0;
			    FILEPtr fp = getFileHandle( file_name, ref compression_type, ref len );
			    
			    if ( null!=fp ){        
			    	if ( 0!=(compression_type & SPB_COMPRESSION) )
			            return decodeSPB( fp, 0, buffer );
			
			        fseek( fp, 0, SEEK_SET );
			        total = len;
			        while( len > 0 ){
			            if ( len > READ_LENGTH ) c = READ_LENGTH;
			            else                     c = len;
			            len -= c;
			            if (fread( buffer, 1, c, fp ) < c) {
			                if (0!=ferror( fp ))
			                    fprintf(stderr, "Error reading %s\n", file_name);
			            }
			            buffer.inc((int)c);
			        }
			        fclose( fp );
			        if ( !is_location_null ) location = ARCHIVE_TYPE_NONE;
			    }
			
			    return total;
			}
			
			public static void convertFromSJISToEUC( CharPtr buf )
			{
			    int i = 0;
			    while ( 0!=buf[i] ) {
			        if ( (byte)buf[i] > 0x80 ) {
			            byte c1, c2;
			            c1 = (byte)buf[i];
			            c2 = (byte)buf[i+1];
			
			            c1 -= (byte)((c1 <= 0x9f) ? 0x71 : 0xb1);
			            c1 = (byte)(c1 * 2 + 1);
			            if (c2 > 0x9e) {
			                c2 -= 0x7e;
			                c1++;
			            }
			            else if (c2 >= 0x80) {
			                c2 -= 0x20;
			            }
			            else {
			                c2 -= 0x1f;
			            }
			
			            buf[i]   = (char)(c1 | 0x80);
			            buf[i+1] = (char)(c2 | 0x80);
			            i++;
			        }
			        i++;
			    }
			}
			
			public static void convertFromSJISToUTF8( CharPtr dst_buf, CharPtr src_buf )
			{
			#if (RECODING_FILENAMES) || (UTF8_FILESYSTEM)
			#if (MACOSX)
			    ONSCocoa::sjis_to_utf8(dst_buf, src_buf);
			#else
			    //Mion: ogapee 20100711a
			    int i, c;
			    unsigned short unicode;
			    unsigned char utf8_buf[4];
			    
			    while(*src_buf){
			        if (IS_TWO_BYTE(*src_buf)){
			            unsigned short index = *(unsigned char*)src_buf++;
			            index = index << 8 | (*(unsigned char*)src_buf++);
			            unicode = convSJIS2UTF16( index );
			            c = convUTF16ToUTF8(utf8_buf, unicode);
			            for (i=0 ; i<c ; i++)
			                *dst_buf++ = utf8_buf[i];
			        }
			        else{
			            *dst_buf++ = *src_buf++;
			        }
			    }
			    *dst_buf++ = 0;
			#endif //MACOSX
			#elif true//(WIN32)
			    int wc_size = MultiByteToWideChar(936/*932*/, 0, src_buf, -1, null, 0);
			    UnsignedShortPtr u16_tmp = new UnsignedShortPtr(new ushort[wc_size]);
			    MultiByteToWideChar(936/*932*/, 0, src_buf, -1, u16_tmp, wc_size);
			    int mb_size = WideCharToMultiByte(CP_UTF8, 0, u16_tmp, wc_size, dst_buf, 0, null, null);
			    WideCharToMultiByte(CP_UTF8, 0, u16_tmp, wc_size, dst_buf, mb_size, null, null);
			    u16_tmp = null;//delete[] u16_tmp;
			#endif //RECODING_FILENAMES || UTF8_FILESYSTEM, WIN32
			}
			
			private static int getbit_buf;
			public int getbit( FILEPtr fp, int n )
			{
				int i, x = 0;
//			    static int getbit_buf;
			    
			    for ( i=0 ; i<n ; i++ ){
			        if ( getbit_mask == 0 ){
			            if (getbit_len == getbit_count){
			                getbit_len = fread(read_buf, 1, READ_LENGTH, fp);
			                if (getbit_len == 0) return EOF;
			                getbit_count = 0;
			            }
			
			            getbit_buf = key_table[read_buf[getbit_count++]];
			            getbit_mask = 128;
			        }
			        x <<= 1;
			        if ( 0!=(getbit_buf & getbit_mask) ) x |= 1;
			        getbit_mask >>= 1;
			    }
			    return x;
			}
			
			public uint decodeSPB( FILEPtr fp, uint offset, UnsignedCharPtr buf )
			{
				uint count;
			    UnsignedCharPtr pbuf, psbuf;
			    uint i, j, k;
			    int c, n, m;
			
			    getbit_mask = 0;
			    getbit_len = getbit_count = 0;
			    
			    fseek( fp, (int)offset, SEEK_SET );
			    uint width  = readShort( fp );
			    uint height = readShort( fp );
			
			    uint width_pad  = (4 - width * 3 % 4) % 4;
			
			    uint total_size = (width * 3 + width_pad) * height + 54;
			
			    /* ---------------------------------------- */
			    /* Write header */
			    memset( buf, 0, 54 );
			    buf[0] = (byte)'B'; buf[1] = (byte)'M';
			    buf[2] = (byte)(total_size & 0xff);
			    buf[3] = (byte)((total_size >>  8) & 0xff);
			    buf[4] = (byte)((total_size >> 16) & 0xff);
			    buf[5] = (byte)((total_size >> 24) & 0xff);
			    buf[10] = 54; // offset to the body
			    buf[14] = 40; // header size
			    buf[18] = (byte)(width & 0xff);
			    buf[19] = (byte)((width >> 8)  & 0xff);
			    buf[22] = (byte)(height & 0xff);
			    buf[23] = (byte)((height >> 8)  & 0xff);
			    buf[26] = 1; // the number of the plane
			    buf[28] = 24; // bpp
			    buf[34] = (byte)(total_size - 54); // size of the body
			
			    buf.inc(54);
			
			    if (decomp_buffer_len < width*height+4){
			        if (null!=decomp_buffer) decomp_buffer = null;//delete[] decomp_buffer;
			        decomp_buffer_len = width*height+4;
			        decomp_buffer = new UnsignedCharPtr(new byte[decomp_buffer_len]);
			    }
			    
			    for ( i=0 ; i<3 ; i++ ){
			        count = 0;
			        c = getbit( fp, 8 );decomp_buffer[count++] = (byte)c;
			        while ( count < (uint)(width * height) ){
			            n = getbit( fp, 3 );
			            if ( n == 0 ){
			            	decomp_buffer[count++] = (byte)c;
			                decomp_buffer[count++] = (byte)c;
			                decomp_buffer[count++] = (byte)c;
			                decomp_buffer[count++] = (byte)c;
			                continue;
			            }
			            else if ( n == 7 ){
			                m = getbit( fp, 1 ) + 1;
			            }
			            else{
			                m = n + 2;
			            }
			
			            for ( j=0 ; j<4 ; j++ ){
			                if ( m == 8 ){
			                    c = getbit( fp, 8 );
			                }
			                else{
			            		k = (uint)getbit( fp, m );
			            		if ( 0!=(k & 1) ) c = (int)(c + (k>>1) + 1);
			                    else         c = (int)(c -(k>>1));
			                }
			            	decomp_buffer[count++] = (byte)c;
			            }
			        }
			
			        pbuf  = new UnsignedCharPtr(buf,  (int)(+ (width * 3 + width_pad)*(height-1) + i));
			        psbuf = decomp_buffer;
			
			        for ( j=0 ; j<height ; j++ ){
			        	if ( 0!=(j & 1) ){
			        		for ( k=0 ; k<width ; k++, pbuf.inc(-3) ) {pbuf[0] = psbuf[0]; psbuf.inc(); }
			        		pbuf.inc((int)-( width * 3 + width_pad - 3));
			            }
			            else{
			        		for ( k=0 ; k<width ; k++, pbuf.inc(3) ) { pbuf[0] = psbuf[0]; psbuf.inc(); }
			        		pbuf.inc((int)-( width * 3 + width_pad + 3));
			            }
			        }
			    }
			    
			    return total_size;
			}
			
			public uint decodeLZSS( ArchiveInfo ai, int no, UnsignedCharPtr buf )
			{
				uint count = 0;
			    int i, j, k, r, c;
			
			    getbit_mask = 0;
			    getbit_len = getbit_count = 0;
			
			    fseek( ai.file_handle, (int)ai.fi_list[no].offset, SEEK_SET );
			    memset( decomp_buffer, 0, N-F );
			    r = N - F;
			
			    while ( count < ai.fi_list[no].original_length ){
			        if ( 0!=getbit( ai.file_handle, 1 ) ) {
			            if ((c = getbit( ai.file_handle, 8 )) == EOF) break;
			            buf[ count++ ] = (byte)c;
			            decomp_buffer[r++] = (byte)c;  r &= (N - 1);
			        } else {
			            if ((i = getbit( ai.file_handle, EI )) == EOF) break;
			            if ((j = getbit( ai.file_handle, EJ )) == EOF) break;
			            for (k = 0; k <= j + 1  ; k++) {
			                c = decomp_buffer[(i + k) & (N - 1)];
			                buf[ count++ ] = (byte)c;
			                decomp_buffer[r++] = (byte)c;  r &= (N - 1);
			            }
			        }
			    }
			
			    return count;
			}
			
			public uint getDecompressedFileLength( int type, FILEPtr fp, uint offset )
			{
				uint length=0;
				fseek( fp, (int)offset, SEEK_SET );
			    
			    if ( type == SPB_COMPRESSION ){
			        uint width  = readShort( fp );
			        uint height = readShort( fp );
			        uint width_pad  = (4 - width * 3 % 4) % 4;
			            
			        length = (width * 3 +width_pad) * height + 54;
			    }
			
			    return length;
			}
			
			public uint DirectReader_getFile( CharPtr file_name, UnsignedCharPtr buffer,
			                              ref int location, bool is_location_null=true )
			{
				return this.getFile(file_name, buffer, ref location, is_location_null);
			}
			
			public uint DirectReader_getFileLength(CharPtr file_name)
			{
				return this.getFileLength(file_name);
			}
		}
	}
}
