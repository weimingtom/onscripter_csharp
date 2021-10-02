﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-8-4
 * Time: 9:10
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
		 *  NsaReader.cpp - Reader from a NSA archive
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
		// creating new archives via nsamake and ns2make
		
//		#include "NsaReader.h"
//		#include <cstdio>
//		#include <string.h>
//		#define NSA_ARCHIVE_NAME "arc"
//		#define NSA_ARCHIVE_NAME2 "arc%d"
		
		public partial class NsaReader : SarReader
		{
			public NsaReader( DirPaths path, int nsaoffset, UnsignedCharPtr key_table )
			        :base( path, key_table )
			{
//			    sar_flag = true;
//			    nsa_offset = nsaoffset;
//			    num_of_nsa_archives = num_of_ns2_archives = 0;
//			
//			    if (key_table)
//			        nsa_archive_ext = "___";
//			    else
//			        nsa_archive_ext = "nsa";
//			
//			    ns2_archive_ext = "ns2";
			}
			
//			NsaReader::~NsaReader()
//			{
//			}
			
//			#ifndef TOOLS_BUILD
			
			public override int open( CharPtr nsa_path )
			{
				return 0;
//			    const DirPaths paths = DirPaths(nsa_path);
//			    return processArchives(paths);
			}
			
			public int processArchives( DirPaths path )
			{
				return 0;
//			    int i,j,k,n,nd;
//			    FILE *fp;
//			    char archive_name[256], archive_name2[256];
//			
//			    if ( !SarReader::open( "arc.sar" ) ) {
//			        sar_flag = true;
//			    }
//			    else {
//			        sar_flag = false;
//			    }
//			
//			    const DirPaths *nsa_path = &path;
//			
//			    i = j = -1;
//			    n = nd = 0;
//			    while ((i<MAX_EXTRA_ARCHIVE) && (n<archive_path->get_num_paths())) {
//			        if (j < 0) {
//			            sprintf( archive_name, "%s%s.%s", nsa_path->get_path(nd), NSA_ARCHIVE_NAME, nsa_archive_ext );
//			            sprintf(archive_name2, "%s%s", archive_path->get_path(n), archive_name);
//			        } else {
//			            sprintf( archive_name2, NSA_ARCHIVE_NAME2, j+1 );
//			            sprintf( archive_name, "%s%s.%s", nsa_path->get_path(nd), archive_name2, nsa_archive_ext );
//			            sprintf(archive_name2, "%s%s", archive_path->get_path(n), archive_name);
//			        }
//			#if _MSC_VER <= 1200 //VC 6
//			        fp = fopen(archive_name2, "rb");
//			#else
//			        fp = std::fopen(archive_name2, "rb");
//			#endif
//			        if (fp != NULL) {
//			            //printf("Found archive %s\n", archive_name2); fflush(stdout);
//			            if (i < 0) {
//			                archive_info_nsa.file_handle = fp;
//			                archive_info_nsa.file_name = new char[strlen(archive_name2)+1];
//			                strcpy(archive_info_nsa.file_name, archive_name2);
//			                readArchive( &archive_info_nsa, ARCHIVE_TYPE_NSA, nsa_offset );
//			            } else {
//			                archive_info2[i].file_handle = fp;
//			                archive_info2[i].file_name = new char[strlen(archive_name2)+1];
//			                strcpy(archive_info2[i].file_name, archive_name2);
//			                readArchive( &archive_info2[i], ARCHIVE_TYPE_NSA, nsa_offset );
//			            }
//			            i++;
//			            j++;
//			        } else {
//			            j = -1;
//			            nd++;
//			            if (nd >= nsa_path->get_num_paths()) {
//			                nd = 0;
//			                n++;
//			            }
//			        }
//			    }
//			
//			    k = 0;
//			    n = nd = 0;
//			    while ((k<MAX_NS2_ARCHIVE) && (n<archive_path->get_num_paths())) {
//			        sprintf( archive_name, "%s00.%s", nsa_path->get_path(nd), ns2_archive_ext );
//			        sprintf(archive_name2, "%s%s", archive_path->get_path(n), archive_name);
//			#if _MSC_VER <= 1200 //VC 6
//					fp = fopen(archive_name2, "rb");
//			#else
//			        fp = std::fopen(archive_name2, "rb");
//			#endif
//			        if (fp != NULL) {
//			            fclose(fp);
//			            for (j=MAX_NS2_ARCHIVE_NUM; (j>=0) && (k<MAX_NS2_ARCHIVE); j--) {
//			                sprintf( archive_name, "%s%02d.%s", nsa_path->get_path(nd), j, ns2_archive_ext );
//			                sprintf(archive_name2, "%s%s", archive_path->get_path(n), archive_name);
//			#if _MSC_VER <= 1200
//			                fp = fopen(archive_name2, "rb");
//			#else
//			                fp = std::fopen(archive_name2, "rb");
//			#endif
//			                if (fp == NULL) {
//			                    if (j == 0) break;
//			                } else {
//			                    archive_info_ns2[k].file_handle = fp;
//			                    archive_info_ns2[k].file_name = new char[strlen(archive_name2)+1];
//			                    strcpy(archive_info_ns2[k].file_name, archive_name2);
//			                    readArchive( &archive_info_ns2[k], ARCHIVE_TYPE_NS2 );
//			                    k++;
//			                }
//			            }
//			        }
//			        nd++;
//			        if (nd > nsa_path->get_num_paths()) {
//			            nd = 0;
//			            n++;
//			        }
//			    }
//			
//			    if ((i < 0) && (k < 0)) {
//			        // didn't find any (main) archive files
//			        fprintf( stderr, "can't open nsa archive file %s.%s ", NSA_ARCHIVE_NAME, nsa_archive_ext );
//			        fprintf( stderr, "or ns2 archive file 00.%s ", ns2_archive_ext );
//			        return -1;
//			    } else {
//			        num_of_nsa_archives = i+1;
//			        num_of_ns2_archives = k;
//			        return 0;
//			    }
			}
			
//			#else //def TOOLS_BUILD
			
			public ArchiveInfo openForCreate( CharPtr nsa_name, int archive_type, int nsaoffset )
			{
				return null;
//			    sar_flag = false;
//			    nsa_offset = nsaoffset;
//			    if ( ( archive_info_nsa.file_handle = ::fopen( nsa_name, "wb" ) ) == NULL ){
//			        fprintf( stderr, "can't open file %s\n", nsa_name );
//			        return NULL;
//			    }
//			
//			    return &archive_info_nsa;
			}
			
			public int openForConvert( CharPtr nsa_name, int archive_type, int nsaoffset )
			{
				return 0;
//			    sar_flag = false;
//			    nsa_offset = nsaoffset;
//			    if ( ( archive_info_nsa.file_handle = ::fopen( nsa_name, "rb" ) ) == NULL ){
//			        fprintf( stderr, "can't open file %s\n", nsa_name );
//			        return -1;
//			    }
//			
//			    return readArchive( &archive_info_nsa, archive_type, nsa_offset );
			}
			
			int writeHeader( FILEPtr fp, int archive_type, int nsaoffset )
			{
				return 0;
//			    ArchiveInfo *ai = &archive_info_nsa;
//			    return writeHeaderSub( ai, fp, archive_type, nsaoffset );
			}
			
			public uint putFile( FILEPtr fp, int no, uint offset, uint length, uint original_length, int compression_type, bool modified_flag, UnsignedCharPtr buffer )
			{
				return 0;
//			    ArchiveInfo *ai = &archive_info_nsa;
//			    return putFileSub( ai, fp, no, offset, length, original_length , compression_type, modified_flag, buffer );
			}
			
//			#endif //TOOLS_BUILD
			
			public override CharPtr getArchiveName()
			{
				return null;
//			    return "nsa";
			}
			
			public override int getNumFiles(){
				return 0;
//			    int i;
//			    int total = archive_info.num_of_files; // start with sar files, if any
//			
//			    total += archive_info_nsa.num_of_files; // add in the arc.nsa files
//			
//			    for ( i=0 ; i<num_of_nsa_archives-1 ; i++ ) total += archive_info2[i].num_of_files; // add in the arc?.nsa files
//			
//			    for ( i=0 ; i<num_of_ns2_archives ; i++ ) total += archive_info_ns2[i].num_of_files; // add in the ##.ns2 files
//			
//			    return total;
			}
			
			uint getFileLengthSub( ArchiveInfo ai, CharPtr file_name )
			{
				return 0;
//			    unsigned int i = getIndexFromFile( ai, file_name );
//			
//			    if ( i == ai->num_of_files ) return 0;
//			
//			    if ( ai->fi_list[i].original_length != 0 ){
//			        return ai->fi_list[i].original_length;
//			    }
//			
//			    int type = ai->fi_list[i].compression_type;
//			    if ( type == NO_COMPRESSION )
//			        type = getRegisteredCompressionType( file_name );
//			    if (type == SPB_COMPRESSION ) {
//			        ai->fi_list[i].original_length = getDecompressedFileLength( type, ai->file_handle, ai->fi_list[i].offset );
//			    }
//			    
//			    return ai->fi_list[i].original_length;
			}
			
			public override uint getFileLength( CharPtr file_name )
			{
				return 0;
//			    size_t ret;
//			    int i;
//			    
//			#ifndef TOOLS_BUILD
//			    // direct read
//			    if ( ( ret = DirectReader::getFileLength( file_name ) ) ) return ret;
//			#endif
//			    // ns2 read
//			    for ( i=0 ; i<num_of_ns2_archives ; i++ ){
//			        if ( (ret = getFileLengthSub( &archive_info_ns2[i], file_name )) ) return ret;
//			    }
//			    
//			    // nsa read
//			    if ( ( ret = getFileLengthSub( &archive_info_nsa, file_name )) ) return ret;
//			
//			    // nsa? read
//			    for ( i=0 ; i<num_of_nsa_archives-1 ; i++ ){
//			        if ( (ret = getFileLengthSub( &archive_info2[i], file_name )) ) return ret;
//			    }
//			    
//			    // sar read
//			    if ( sar_flag ) return SarReader::getFileLength( file_name );
//			
//			    return 0;
			}
			
			public override uint getFile( CharPtr file_name, UnsignedCharPtr buffer, ref int location )
			{
				return 0;
//			    size_t ret;
//			
//			    // direct read
//			    if ( ( ret = DirectReader::getFile( file_name, buffer, location ) ) ) return ret;
//			
//			    // ns2 read
//			    for ( int i=0 ; i<num_of_ns2_archives ; i++ ){
//			        if ( (ret = getFileSub( &archive_info_ns2[i], file_name, buffer )) ){
//			            if ( location ) *location = ARCHIVE_TYPE_NS2;
//			            return ret;
//			        }
//			    }
//			
//			    // nsa read
//			    if ( (ret = getFileSub( &archive_info_nsa, file_name, buffer )) ){
//			        if ( location ) *location = ARCHIVE_TYPE_NSA;
//			        return ret;
//			    }
//			
//			#ifdef _MSC_VER
//				{
//			#endif
//			    // nsa? read
//			    for ( int i=0 ; i<num_of_nsa_archives-1 ; i++ ){
//			        if ( (ret = getFileSub( &archive_info2[i], file_name, buffer )) ){
//			            if ( location ) *location = ARCHIVE_TYPE_NSA;
//			            return ret;
//			        }
//			    }
//			#ifdef _MSC_VER
//				}
//			#endif
//			
//			    // sar read
//			    if ( sar_flag ) return SarReader::getFile( file_name, buffer, location );
//			
//			    return 0;
			}
			
			public override FileInfo getFileByIndex( uint index )
			{
				return null;
//			    int i;
//			    
//			    for ( i=0 ; i<num_of_ns2_archives ; i++ ){
//			        if ( index < archive_info_ns2[i].num_of_files ) return archive_info_ns2[i].fi_list[index];
//			        index -= archive_info_ns2[i].num_of_files;
//			    }
//			    if ( index < archive_info_nsa.num_of_files ) return archive_info_nsa.fi_list[index];
//			    index -= archive_info_nsa.num_of_files;
//			
//			    for ( i=0 ; i<num_of_nsa_archives-1 ; i++ ){
//			        if ( index < archive_info2[i].num_of_files ) return archive_info2[i].fi_list[index];
//			        index -= archive_info2[i].num_of_files;
//			    }
//			    fprintf( stderr, "NsaReader::getFileByIndex  Index %d is out of range\n", index );
//			
//			    return archive_info.fi_list[0];
			}
		}
	}
}
