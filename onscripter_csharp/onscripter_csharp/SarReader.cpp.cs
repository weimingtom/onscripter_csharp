﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-31
 * Time: 13:24
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
		 *  SarReader.cpp - Reader from a SAR archive for ONScripter-EN
		 *
		 *  Copyright (c) 2001-2008 Ogapee. All rights reserved.
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
		
		// Modified by Mion, December 2009, to support NS2 archives and allow
		// creating new archives via nsamake, ns2make & sarmake
		
//		#include "SarReader.h"
		public const int WRITE_LENGTH = 4096; //FIXME:???5000
		
		public partial class SarReader : DirectReader {
			public SarReader( DirPaths path, UnsignedCharPtr key_table/*=null*/ )
			        :base( path, key_table )
			{
			    root_archive_info = last_archive_info = archive_info;
			    num_of_sar_archives = 0;
			}
			
			~SarReader()
			{
			    close();
			}
			
			public override int open( CharPtr name/*=null*/ )
			{
				ArchiveInfo info = new ArchiveInfo();
			
			    if ( (info.file_handle = fopen( name, "rb" ) ) == null ){
			        info = null;//delete info;
			        return -1;
			    }
			
			    info.file_name = new char[strlen(name)+1];
			    memcpy(info.file_name, name, strlen(name)+1);
			    
			    readArchive( info );
			
			    last_archive_info.next = info;
			    last_archive_info = last_archive_info.next;
			    num_of_sar_archives++;
			
			    return 0;
			}
			
			#if TOOLS_BUILD
			
			public int openForConvert( CharPtr name )
			{
				ArchiveInfo info = new ArchiveInfo();
			
			    if ( (info.file_handle = ONScripter.fopen( name, "rb" ) ) == null ){
			        info = null;//delete info;
			        return -1;
			    }
			
			    info.file_name = new char[strlen(name)+1];
			    memcpy(info.file_name, name, strlen(name)+1);
			    
			    readArchive( info );
			
			    last_archive_info.next = info;
			    last_archive_info = last_archive_info.next;
			    num_of_sar_archives++;
			
			    return 0;
			}
			
			public ArchiveInfo openForCreate( CharPtr name )
			{
				ArchiveInfo info = new ArchiveInfo();
			
			    if ( (info.file_handle = ONScripter.fopen( name, "wb" ) ) == null ){
			        info = null;//delete info;
			        return null;
			    }
			
			    info.file_name = new char[strlen(name)+1];
			    memcpy(info.file_name, name, strlen(name)+1);
			    
			    last_archive_info.next = info;
			    last_archive_info = last_archive_info.next;
			    num_of_sar_archives++;
			
			    return info;
			}
			
			#endif //TOOLS_BUILD
			
			public int readArchive( ArchiveInfo ai, int archive_type = ARCHIVE_TYPE_SAR, int offset = 0 )
			{
			    uint i=0;
			    
			    /* Read header */
			    for (int j=0; j<offset; j++)
			        i = readChar( ai.file_handle );
			    if ( archive_type == ARCHIVE_TYPE_NS2 ) {
			        // new archive type since NScr2.91
			        // - header starts with base_offset (byte-swapped), followed by
			        //   filename data - doesn't tell how many files!
			        // - filenames are surrounded by ""s
			        // - new NS2 filename def: "filename", length (4bytes, swapped)
			        // - no compression type? really, no compression.
			        // - not sure if NS2 uses key_table or not, using default funcs for now
			        ai.base_offset = swapLong( readLong( ai.file_handle ) );
			        ai.base_offset = (ulong)((long)ai.base_offset + offset);
			
			        // need to parse the whole header to see how many files there are
			        ai.num_of_files = 0;
			        ulong cur_offset = (ulong)(offset + 5);
			        // there's an extra byte at the end of the header, not sure what for
			        while (cur_offset < ai.base_offset){
			            //skip the beginning double-quote
			            byte ch = key_table[fgetc( ai.file_handle )];
			            cur_offset++;
			            do cur_offset++;
			            while( (ch = key_table[fgetc( ai.file_handle )] ) != '"' );
			            i = (uint)readLong( ai.file_handle );
			            cur_offset += 4;
			            ai.num_of_files++;
			        }
			        ai.fi_list = new FileInfo[ ai.num_of_files ];
			
			        // now go back to the beginning and read the file info
			        cur_offset = (ulong)ai.base_offset;
			        fseek( ai.file_handle, 4 + offset, SEEK_SET );
			        for ( i=0 ; i<ai.num_of_files ; i++ ){
			            uint count = 0;
			            //skip the beginning double-quote
			            byte ch = key_table[fgetc( ai.file_handle )];
			            //error if _not_ a double-quote
			            if (ch != '"') {
			                fprintf(stderr, "file does not seem to be a valid NS2 archive\n");
			                return -1;
			            }
			            while( (ch = key_table[fgetc( ai.file_handle )] ) != '"' ){
			            	if ( 'a' <= ch && ch <= 'z' ) ch = (byte)(ch + ('A' - 'a'));
			            	ai.fi_list[i].name[count++] = (char)ch;
			            }
			            ai.fi_list[i].name[count] = '\0';
			            ai.fi_list[i].compression_type = getRegisteredCompressionType( ai.fi_list[i].name );
			            ai.fi_list[i].offset = (uint)cur_offset;
			            ai.fi_list[i].length = (uint)swapLong( readLong( ai.file_handle ) );
			            ai.fi_list[i].original_length = ai.fi_list[i].length;
			            cur_offset += ai.fi_list[i].length;
			        }
			        //
			        // old NSA filename def: filename, ending '\0' byte , compr-type byte,
			        // start (4byte), length (4byte))
			    } else {
			        ai.num_of_files = readShort( ai.file_handle );
			        ai.fi_list = new FileInfo[ ai.num_of_files ];
			
			        ai.base_offset = readLong( ai.file_handle );
			        ai.base_offset = (uint)((long)ai.base_offset + offset);
			
			        for ( i=0 ; i<ai.num_of_files ; i++ ){
			            byte ch;
			            int count = 0;
			
			            while( 0!=(ch = key_table[fgetc( ai.file_handle )] ) ){
			            	if ( 'a' <= ch && ch <= 'z' ) ch = (byte)(ch + ('A' - 'a'));
			            	ai.fi_list[i].name[count++] = (char)ch;
			            }
			            ai.fi_list[i].name[count] = (char)ch;
			
			            if ( archive_type == ARCHIVE_TYPE_NSA )
			                ai.fi_list[i].compression_type = readChar( ai.file_handle );
			            else
			                ai.fi_list[i].compression_type = NO_COMPRESSION;
			            ai.fi_list[i].offset = (uint)(readLong( ai.file_handle ) + ai.base_offset);
			            ai.fi_list[i].length = (uint)readLong( ai.file_handle );
			
			            if ( archive_type == ARCHIVE_TYPE_NSA ){
			            	ai.fi_list[i].original_length = (uint)readLong( ai.file_handle );
			            }
			            else{
			                ai.fi_list[i].original_length = ai.fi_list[i].length;
			            }
			
			            /* Registered Plugin check */
			            if ( ai.fi_list[i].compression_type == NO_COMPRESSION )
			                ai.fi_list[i].compression_type = getRegisteredCompressionType( ai.fi_list[i].name );
			
			            //Mion: delaying checking decompressed file length until
			            // file is opened for real: original_length = 0 means
			            // it hasn't been checked yet
			            // (checking every compressed file in this function caused
			            //  a massive slowdown at program start when an archive had
			            //  many compressed images...)
			            if ( (ai.fi_list[i].compression_type == SPB_COMPRESSION) ){
			                ai.fi_list[i].original_length = 0;
			            }
			        }
			    }
			    
			    return 0;
			}
			
			#if TOOLS_BUILD
			
			public int writeHeaderSub( ArchiveInfo ai, FILEPtr fp, int archive_type = ARCHIVE_TYPE_SAR, int offset = 0 )
			{
				uint i, j;
			
				fseek( fp, (int)0L, SEEK_SET );
			    for (int k=0; k<offset; k++)
			    	fputc( (char)0, fp );
			    if ( archive_type == ARCHIVE_TYPE_NS2 ) {
			    	writeLong( fp, swapLong((ulong)((long)ai.base_offset - offset)) );
			    } else {
			    	writeShort( fp, (ushort)ai.num_of_files );
			    	writeLong( fp, (ulong)((long)ai.base_offset - offset) );
			    }
			
			    for ( i=0 ; i<ai.num_of_files ; i++ ){
			        if ( archive_type == ARCHIVE_TYPE_NS2 )
			            fputc( '"', fp );
			
			        for ( j=0 ; 0!=ai.fi_list[i].name[j] ; j++ ){
			            if ((ai.fi_list[i].name[j] >= 'A') &&
			                (ai.fi_list[i].name[j] <= 'Z'))
			        		fputc( (char)(ai.fi_list[i].name[j] - 'A' + 'a'), fp );
			            else
			                fputc( ai.fi_list[i].name[j], fp );
			        }
			        if ( archive_type == ARCHIVE_TYPE_NS2 )
			            fputc( '"', fp );
			        else
			            fputc( ai.fi_list[i].name[j], fp );
			
			        if ( archive_type == ARCHIVE_TYPE_NSA )
			        	writeChar( fp, (byte)ai.fi_list[i].compression_type );
			
			        if ( archive_type == ARCHIVE_TYPE_NS2 )
			            writeLong( fp, swapLong(ai.fi_list[i].length) );
			        else {
			            writeLong( fp, ai.fi_list[i].offset - ai.base_offset );
			            writeLong( fp, ai.fi_list[i].length );
			            if ( archive_type == ARCHIVE_TYPE_NSA ){
			                writeLong( fp, ai.fi_list[i].original_length );
			            }
			        }
			    }
			    if ( archive_type == ARCHIVE_TYPE_NS2 )
			        fputc( 'e', fp ); //'e' for 'end', maybe? seems constant
			
			    return 0;
			}
			
			public int writeHeader( FILEPtr fp )
			{
				ArchiveInfo ai = archive_info.next;
			    return writeHeaderSub( ai, fp );
			}
			
			public uint addFile( ArchiveInfo ai, FILEPtr newfp, int no, uint offset, UnsignedCharPtr buffer )
			{
				fseek( newfp, (int)0L, SEEK_SET );
			    if (fread( buffer, 1, ai.fi_list[no].length, newfp ) !=
			        ai.fi_list[no].length) {
			        if (0!=ferror(newfp))
			            fprintf(stderr, "Read error on adding item %d\n", no);
			    }
			
			    uint len = ai.fi_list[no].length, c;
			    fseek( ai.file_handle, (int)offset, SEEK_SET );
			    while( len > 0 ){
			        if ( len > WRITE_LENGTH ) c = WRITE_LENGTH;
			        else                      c = len;
			        len -= c;
			        if ( fwrite( buffer, 1, c, ai.file_handle ) != c )
			            fprintf(stderr, "Write error adding archive item %d\n", no);
			        buffer.inc((int)c);
			    }
			
			    return ai.fi_list[no].length;
			}
			
			public uint putFileSub( ArchiveInfo ai, FILEPtr fp, int no, uint offset, uint length, uint original_length, int compression_type, bool modified_flag, UnsignedCharPtr buffer )
			{
				ai.fi_list[no].compression_type = compression_type;
			    ai.fi_list[no].length = length;
			    ai.fi_list[no].original_length = original_length;
			
			    fseek( fp, (int)offset, SEEK_SET );
			    if ( modified_flag ){
			        {
			            ai.fi_list[no].compression_type = NO_COMPRESSION;
			        }
			    }
			    else{
			    	fseek( ai.file_handle, (int)ai.fi_list[no].offset, SEEK_SET );
			        if (fread( buffer, 1, ai.fi_list[no].length, ai.file_handle ) !=
			            ai.fi_list[no].length) {
			            if (0!=ferror(ai.file_handle))
			                fprintf(stderr, "Read error extracting archive item %d\n", no);
			        }
			    }
			
			    uint len = ai.fi_list[no].length, c;
			    while( len > 0 ){
			        if ( len > WRITE_LENGTH ) c = WRITE_LENGTH;
			        else                      c = len;
			        len -= c;
			        if ( fwrite( buffer, 1, c, fp ) != c )
			            fprintf(stderr, "Write error extracting archive item %d\n", no);
			        buffer.inc((int)c);
			    }
			
			    ai.fi_list[no].offset = offset;
			    
			    return ai.fi_list[no].length;
			}
			
			public uint putFile( FILEPtr fp, int no, uint offset, uint length, uint original_length, bool modified_flag, UnsignedCharPtr buffer )
			{
				ArchiveInfo ai = archive_info.next;
			    return putFileSub( ai, fp, no, offset, length, original_length, ai.fi_list[no].compression_type, modified_flag, buffer );
			}
			
			#endif //TOOLS_BUILD
			
			public override int close()
			{
				ArchiveInfo info = archive_info.next;
			    
			    for ( int i=0 ; i<num_of_sar_archives ; i++ ){
			        last_archive_info = info;
			        info = info.next;
			        last_archive_info = null;//delete last_archive_info;
			    }
			    num_of_sar_archives = 0;
			
			    return 0;
			}
			
			public override CharPtr getArchiveName()
			{
			    return "sar";
			}
			
			public override int getNumFiles(){
				ArchiveInfo info = archive_info.next;
			    int num = 0;
			    
			    for ( int i=0 ; i<num_of_sar_archives ; i++ ){
			    	num = (int)(num + info.num_of_files);
			        info = info.next;
			    }
			    
			    return num;
			}
			
			public int getIndexFromFile( ArchiveInfo ai, CharPtr file_name )
			{
				uint i, len;
			
			    len = strlen( file_name );
			    if ( len > MAX_FILE_NAME_LENGTH ) len = MAX_FILE_NAME_LENGTH;
			    memcpy( capital_name, file_name, len );
			    capital_name[ len ] = '\0';
			
			    for ( i=0 ; i<len ; i++ ){
			    	if ( 'a' <= capital_name[i] && capital_name[i] <= 'z' ) capital_name[i] = (char)(capital_name[i] + ('A' - 'a'));
			        else if ( capital_name[i] == '/' ) capital_name[i] = '\\';
			    }
			    for ( i=0 ; i<ai.num_of_files ; i++ ){
			        if ( 0==strcmp( capital_name, ai.fi_list[i].name ) ) break;
			    }
			
			    return (int)i;
			}
			
			public override uint getFileLength( CharPtr file_name )
			{
			#if !TOOLS_BUILD
			    uint ret;
			    if ( 0!=( ret = base.getFileLength( file_name ) ) ) return ret;
			#endif
			    ArchiveInfo info = archive_info.next;
			    uint j = 0;
			    for ( int i=0 ; i<num_of_sar_archives ; i++ ){
			    	j = (uint)getIndexFromFile( info, file_name );
			        if ( j != info.num_of_files ) break;
			        info = info.next;
			    }
			    if ( null==info ) return 0;
			    
			    if ( info.fi_list[j].original_length != 0 ){
			        return info.fi_list[j].original_length;
			    }
			
			    int type = info.fi_list[j].compression_type;
			    if ( type == NO_COMPRESSION )
			        type = getRegisteredCompressionType( file_name );
			    if ( type == SPB_COMPRESSION ) {
			        info.fi_list[j].original_length = getDecompressedFileLength( type, info.file_handle, info.fi_list[j].offset );
			    }
			    
			    return info.fi_list[j].original_length;
			}
			
			public uint getFileSub( ArchiveInfo ai, CharPtr file_name, UnsignedCharPtr buf )
			{
				uint i = (uint)getIndexFromFile( ai, file_name );
			    if ( i == ai.num_of_files ) return 0;
			
			    int type = ai.fi_list[i].compression_type;
			    if ( type == NO_COMPRESSION ) type = getRegisteredCompressionType( file_name );
			
			    if ( type == LZSS_COMPRESSION ){
			    	return decodeLZSS( ai, (int)i, buf );
			    }
			    else if ( type == SPB_COMPRESSION ){
			        return decodeSPB( ai.file_handle, ai.fi_list[i].offset, buf );
			    }
			
			    fseek( ai.file_handle, (int)ai.fi_list[i].offset, SEEK_SET );
			    uint ret = fread( buf, 1, ai.fi_list[i].length, ai.file_handle );
			    for (uint j=0 ; j<ret ; j++) buf[j] = key_table[buf[j]];
			    return ret;
			}
			
			public override uint getFile( CharPtr file_name, UnsignedCharPtr buf, ref int location )
			{
				uint ret;
			    if ( 0!=( ret = base.getFile( file_name, buf, ref location ) ) ) return ret;
			
			    ArchiveInfo info = archive_info.next;
			    uint j = 0;
			    for ( int i=0 ; i<num_of_sar_archives ; i++ ){
			        if ( (j = getFileSub( info, file_name, buf )) > 0 ) break;
			        info = info.next;
			    }
			    location = ARCHIVE_TYPE_SAR;
			    
			    return j;
			}
			
			public override FileInfo getFileByIndex( uint index )
			{
				ArchiveInfo info = archive_info.next;
			    for ( int i=0 ; i<num_of_sar_archives ; i++ ){
			        if ( index < info.num_of_files ) return info.fi_list[index];
			        index -= info.num_of_files;
			        info = info.next;
			    }
			    fprintf( stderr, "SarReader::getFileByIndex  Index %d is out of range\n", index );
			
			    return archive_info.fi_list[index];
			}
		}
	}
}
