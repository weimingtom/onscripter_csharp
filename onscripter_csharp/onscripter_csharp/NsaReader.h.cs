﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:14
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
		 *  NsaReader.h - Reader from a NSA archive
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
		
		// Modified by Mion, December 2009, to add NS2 archive support.
		
		//#ifndef __NSA_READER_H__
		//#define __NSA_READER_H__
		
//		#include "SarReader.h"
		private const int MAX_EXTRA_ARCHIVE = 9;
		private const int MAX_NS2_ARCHIVE_NUM = 99;
		private const int MAX_NS2_ARCHIVE = 100;
		
		public partial class NsaReader : SarReader
		{
//		public:
//		    NsaReader( DirPaths &path, int nsaoffset = 0, const unsigned char *key_table=NULL );
//		    ~NsaReader();
//		
//		#ifndef TOOLS_BUILD
//		    int open( const char *nsa_path=NULL );
//		    int processArchives( const DirPaths &path );
//		#endif
//		    const char *getArchiveName() const;
//		    int getNumFiles();
//		    
//		    size_t getFileLength( const char *file_name );
//		    size_t getFile( const char *file_name, unsigned char *buf, int *location=NULL );
//		    struct FileInfo getFileByIndex( unsigned int index );
//		
//		#ifdef TOOLS_BUILD
//		    int openForConvert( const char *nsa_name, int archive_type=ARCHIVE_TYPE_NSA, int nsaoffset = 0 );
//		    ArchiveInfo* openForCreate( const char *nsa_name, int archive_type=ARCHIVE_TYPE_NSA, int nsaoffset = 0 );
//		    int writeHeader( FILE *fp, int archive_type=ARCHIVE_TYPE_NSA, int nsaoffset = 0 );
//		    size_t putFile( FILE *fp, int no, size_t offset, size_t length, size_t original_length, int compression_type, bool modified_flag, unsigned char *buffer );
//		#endif
//		private:
		    public bool sar_flag;
		    public int nsa_offset;
		    public int num_of_nsa_archives;
		    public int num_of_ns2_archives;
		    public CharPtr nsa_archive_ext;
		    public CharPtr ns2_archive_ext;
		    public ArchiveInfo archive_info_nsa = new ArchiveInfo(); // for the arc.nsa file
		    public ArchiveInfo[] archive_info2 = archive_info2_init(); //[MAX_EXTRA_ARCHIVE]; // for the arc1.nsa, arc2.nsa files
		    private static ArchiveInfo[] archive_info2_init()
		    {
		    	ArchiveInfo[] result = new ArchiveInfo[MAX_EXTRA_ARCHIVE];
		    	for (int i = 0; i < result.Length; ++i)
		    	{
		    		result[i] = new ArchiveInfo();
		    	}
		    	return result;
		    }
		    public ArchiveInfo[] archive_info_ns2 = archive_info_ns2_init(); //[MAX_NS2_ARCHIVE]; // for the ##.ns2 files
		    private static ArchiveInfo[] archive_info_ns2_init()
		    {
		    	ArchiveInfo[] result = new ArchiveInfo[MAX_NS2_ARCHIVE];
		    	for (int i = 0; i < result.Length; ++i)
		    	{
		    		result[i] = new ArchiveInfo();
		    	}
		    	return result;
		    }
		    
//		    size_t getFileLengthSub( ArchiveInfo *ai, const char *file_name );
		}
		
		//#endif // __NSA_READER_H__
	}
}
