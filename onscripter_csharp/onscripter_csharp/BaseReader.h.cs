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
		    
		    public const int NO_COMPRESSION   = 0;
		    public const int SPB_COMPRESSION  = 1;
		    public const int LZSS_COMPRESSION = 2;
		    
		    
		    
		    public const int ARCHIVE_TYPE_NONE  = 0;
		    public const int ARCHIVE_TYPE_SAR   = 1;
		    public const int ARCHIVE_TYPE_NSA   = 2;
		    public const int ARCHIVE_TYPE_NS2   = 3;   //new format since NScr2.91, uses ext ".ns2"
		    
		
		    public class FileInfo{
		    	public CharPtr name = new CharPtr(new char[256]);
		        public int  compression_type;
		        public uint offset;
		        public uint length;
		        public uint original_length;
		        public FileInfo()
		        { compression_type = (NO_COMPRESSION);
		          offset = (0); length = (0); original_length = (0);
		        }
		    }
		
		    public class ArchiveInfo{
		        public ArchiveInfo next = null;
		        public FILEPtr file_handle;
		        public int power_resume_number; // currently only for PSP
		        public CharPtr file_name; //assumed to use SJIS encoding
		        public FileInfo[] fi_list = null;
		        public uint num_of_files;
		        public ulong base_offset;
		
		        public ArchiveInfo()
		        { next = (null); file_handle = (null); file_name = (null);
		          fi_list = (null); num_of_files = (0); base_offset = (0);
		       	}
		        ~ArchiveInfo(){
		            if (null!=file_handle) fclose( file_handle );
		            if (null!=file_name) file_name = null;//delete[] file_name;
		            if (null!=fi_list) fi_list = null;//delete[] fi_list;
		        }
		    }
		
			public static char[] errbuf = new char[MAX_ERRBUF_LEN]; // for passing back error details
		
		    ~BaseReader(){}
		    
		    public abstract int open( CharPtr name=null );
		    public abstract int close();
		    
		    public abstract CharPtr getArchiveName();
		    public abstract int  getNumFiles();
		    public abstract void registerCompressionType( CharPtr ext, int type );
		
		    public abstract FileInfo getFileByIndex( uint index );
		    //file_name parameter is assumed to use SJIS encoding
		    public abstract uint getFileLength( CharPtr file_name );
		    public abstract uint getFile( CharPtr file_name, UnsignedCharPtr buffer, ref int location/*=null*/ );
		}
		
//		#endif // __BASE_READER_H__
	
	
	}
}
