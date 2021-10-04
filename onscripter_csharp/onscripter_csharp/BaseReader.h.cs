/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 8:48
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
		 *  BaseReader.h - Base class of archive reader
		 *
		 *  Copyright (c) 2001-2010 Ogapee. All rights reserved.
		 *  (original ONScripter, of which this is a fork).
		 *
		 *  ogapee@aqua.dti2.ne.jp
		 *
		 *  Copyright (c) 2009-2010 "Uncle" Mion Sonozaki
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
		 *  along with this program; if not, write to the Free Software
		 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
		 */
		
//		#ifndef __BASE_READER_H__
//		#define __BASE_READER_H__
//		
//		#include <stdio.h>
		
//		#ifndef SEEK_END
//		#define SEEK_END 2
//		#endif
//		
//		#ifdef WIN32
		public const char DELIMITER = '\\';
//		#else
//		#define DELIMITER '/'
//		#endif
//		
		public const int MAX_ERRBUF_LEN = 512;
		
		public abstract class BaseReader
		{
//		    enum {
//		        NO_COMPRESSION   = 0,
//		        SPB_COMPRESSION  = 1,
//		        LZSS_COMPRESSION = 2
//		    };
//		    
//		    enum {
//		        ARCHIVE_TYPE_NONE  = 0,
//		        ARCHIVE_TYPE_SAR   = 1,
//		        ARCHIVE_TYPE_NSA   = 2,
//		        ARCHIVE_TYPE_NS2   = 3   //new format since NScr2.91, uses ext ".ns2"
//		    };
//		
//		    struct FileInfo{
//		        char name[256];
//		        int  compression_type;
//		        size_t offset;
//		        size_t length;
//		        size_t original_length;
//		        FileInfo()
//		        : compression_type(NO_COMPRESSION),
//		          offset(0), length(0), original_length(0)
//		        {}
//		    };
//		
//		    struct ArchiveInfo{
//		        struct ArchiveInfo *next;
//		        FILE *file_handle;
//		        int power_resume_number; // currently only for PSP
//		        char *file_name; //assumed to use SJIS encoding
//		        struct FileInfo *fi_list;
//		        unsigned int num_of_files;
//		        unsigned long base_offset;
//		
//		        ArchiveInfo()
//		        : next(NULL), file_handle(NULL), file_name(NULL),
//		          fi_list(NULL), num_of_files(0), base_offset(0)
//		        {}
//		        ~ArchiveInfo(){
//		            if (file_handle) fclose( file_handle );
//		            if (file_name) delete[] file_name;
//		            if (fi_list) delete[] fi_list;
//		        }
//		    };
		
			public static char[] errbuf = new char[MAX_ERRBUF_LEN]; // for passing back error details
		
//		    virtual ~BaseReader(){}
		    
		    public abstract int open( CharPtr name=null );
		    public abstract int close();
		    
		    public abstract CharPtr getArchiveName();
		    public abstract int  getNumFiles();
		    public abstract void registerCompressionType( CharPtr ext, int type );
		
		    public abstract FileInfo getFileByIndex( uint index );
		    //file_name parameter is assumed to use SJIS encoding
		    public abstract uint getFileLength( CharPtr file_name );
		    public abstract uint getFile( CharPtr file_name, UnsignedCharPtr buffer, IntPtr location=null );
		}
		
//		#endif // __BASE_READER_H__
	
	
	}
}
