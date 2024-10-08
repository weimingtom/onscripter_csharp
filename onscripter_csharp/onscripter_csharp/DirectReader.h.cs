﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 9:04
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
		 *  DirectReader.h - Reader from independent files for ONScripter-EN
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
		
		//#ifndef __DIRECT_READER_H__
		//#define __DIRECT_READER_H__
		
		//#include "BaseReader.h"
		//#include "DirPaths.h"
		//#include <string.h>
		
		private const int MAX_FILE_NAME_LENGTH = 256;
		
		public partial class DirectReader : BaseReader
		{
//		public:
//		    DirectReader( DirPaths &path, const unsigned char *key_table=NULL );
//		    ~DirectReader();
//		
//		    int open( const char *name=NULL );
//		    int close();
//		
//		    const char *getArchiveName() const;
//		    int getNumFiles();
//		    void registerCompressionType( const char *ext, int type );
//		
//		    struct FileInfo getFileByIndex( unsigned int index );
//		    //file_name parameter is assumed to use SJIS encoding
//		    size_t getFileLength( const char *file_name );
//		    size_t getFile( const char *file_name, unsigned char *buffer, int *location=NULL );
//		
//		    static void convertFromSJISToEUC( char *buf );
//		    static void convertFromSJISToUTF8( char *dst_buf, char *src_buf );
//		    
//		protected:
		    public CharPtr file_full_path;
		    public CharPtr file_sub_path;
		    public uint file_path_len;
		    public CharPtr capital_name;
		    public CharPtr capital_name_tmp;
		
		    public DirPaths archive_path;
			public UnsignedCharPtr key_table = new UnsignedCharPtr(new byte[256]);
		    public bool key_table_flag;
		    public int  getbit_mask;
		    public uint getbit_len, getbit_count;
		    public UnsignedCharPtr read_buf;
		    public UnsignedCharPtr decomp_buffer;
		    public uint decomp_buffer_len;
		    
		    public class RegisteredCompressionType{
		        public RegisteredCompressionType next;
		        public CharPtr ext;
		        public int type;
		        public RegisteredCompressionType(){
		            ext = null;
		            next = null;
		        }
		        public RegisteredCompressionType( CharPtr ext, int type ){
		            this.ext = new char[ strlen(ext)+1 ];
		            for ( uint i=0 ; i<strlen(ext)+1 ; i++ ){
		                this.ext[i] = ext[i];
		                if ( this.ext[i] >= 'a' && this.ext[i] <= 'z' )
		                	this.ext[i] = (char)(this.ext[i] + ('A' - 'a'));
		            }
		            this.type = type;
		            this.next = null;
		        }
		        ~RegisteredCompressionType(){
		            if (null!=ext) ext = null;//delete[] ext;
		        }
		    }
		    public RegisteredCompressionType root_registered_compression_type = new RegisteredCompressionType(), last_registered_compression_type = null;
		
//		    FILE *fopen(const char *path, const char *mode);
//		    unsigned char readChar( FILE *fp );
//		    unsigned short readShort( FILE *fp );
//		    unsigned long readLong( FILE *fp );
//		    void writeChar( FILE *fp, unsigned char ch );
//		    void writeShort( FILE *fp, unsigned short ch );
//		    void writeLong( FILE *fp, unsigned long ch );
//		    static unsigned short swapShort( unsigned short ch );
//		    static unsigned long swapLong( unsigned long ch );
//		    int getbit( FILE *fp, int n );
//		    size_t decodeSPB( FILE *fp, size_t offset, unsigned char *buf );
//		    size_t decodeLZSS( struct ArchiveInfo *ai, int no, unsigned char *buf );
//		    int getRegisteredCompressionType( const char *file_name );
//		    size_t getDecompressedFileLength( int type, FILE *fp, size_t offset );
//		    
//		private:
//		    //file_name parameter is assumed to use SJIS encoding
//		    FILE *getFileHandle( const char *file_name, int &compression_type, size_t *length );
		}
		
//		#endif // __DIRECT_READER_H__

	}
}
