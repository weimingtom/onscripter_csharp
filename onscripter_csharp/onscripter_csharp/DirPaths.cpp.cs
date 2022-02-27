/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-18
 * Time: 16:53
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
		 *  DirPaths.cpp - class that provides multiple directory paths for ONScripter-EN
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
		
//		#include "DirPaths.h"
//		
//		
//		#ifdef _MSC_VER
//		#define snprintf _snprintf
//		#endif
			
		public partial class DirPaths
		{
			public DirPaths( CharPtr new_paths = null )
			{ num_paths = (0); paths = null; all_paths = null;
			
			    //printf("DirPaths cons\n");
			    add(new_paths);
			}
			
			public DirPaths( DirPaths dp )
			{
			    //printf("DirPaths copy cons\n");
			    set(dp);
			}
			
			public DirPaths /*operator =*/copy( DirPaths dp )
			{
			    if (this != dp){
			        //printf("DirPaths =op\n");
			        set(dp);
			    }
			    return this;
			}
			
			public void set( DirPaths dp )
			{
				memcpy(this, dp, (uint)sizeof_DirPaths());
			    if (paths != null) {
			    	CharPtr[] old_paths = paths;
			        paths = new CharPtr[num_paths + 1];
			        for (int i=0; i<=num_paths; i++) {
			            if (old_paths[i] != null) {
			                paths[i] = new char[strlen(old_paths[i]) + 1];
			                strcpy(paths[i], old_paths[i]);
			            } else
			                paths[i] = null;
			        }
			    }
			    if (all_paths != null) {
			        CharPtr old_all_paths = all_paths;
			        all_paths = new char[strlen(old_all_paths) + 1];
			        strcpy(all_paths, old_all_paths);
			    }
			}
			
			~DirPaths()
			{
			    if (paths != null) {
					int ptr = 0;//paths;
			        for (int i=0; i<num_paths; i++) {
						if (paths[ptr] != null) {
							paths[ptr] = null;//delete[] *ptr;
			            }
			            ptr++;
			        }
			        paths = null;//delete[] paths;
			    }
			    if (all_paths != null) {
			        all_paths = null;//delete[] all_paths;
			    }
			}
			
			public void add( DirPaths dp )
			{
			    add(dp.get_all_paths());
			}
			
			public void add( CharPtr new_paths )
			{
			    //don't add null paths
			    if (new_paths == null) return;
			    
			    if (new_paths[0] == '\0') {
			        //don't add empty string paths, unless there are none
			        if (num_paths == 0) num_paths = 1;
			        return;
			    }
			
			    fprintf(stderr, "Adding path: %s\n", new_paths);
			
			    if (all_paths != null) all_paths = null;//delete[] all_paths;
			    all_paths = null;
			
			    if (paths == null) num_paths = 0;
			
			    int cur_num = num_paths;
			    num_paths++;
			    CharPtr ptr1 = new CharPtr(new_paths);
			    while (ptr1[0] != '\0') {
			    	char tmp = ptr1[0]; ptr1.inc();
			    	if ((tmp == PATH_DELIMITER) && (ptr1[0] != '\0') &&
			            (ptr1[0] != PATH_DELIMITER)) {
			            // ignore empty string paths
			            num_paths++;
			        }
			    }
			
			    if (paths != null) {
			        // allocate a new "paths", copy over any existing ones
			        //printf("DirPaths::add(\"%s\")\n", new_paths);
			        CharPtr[] old_paths = paths;
			        paths = new CharPtr[num_paths + 1];
			        for (int i=0; i<cur_num; i++) {
			            paths[i] = old_paths[i];
			        }
			        old_paths = null;//delete[] old_paths;
			    } else {
			        //printf("DirPaths(\"%s\")\n", new_paths);
			        paths = new CharPtr[num_paths + 1];
			        if (cur_num == 1) {
			            //was an "empty path"
			            //keep the "" as the first of the paths by making it a "."
			            paths[0] = new char[3];
			            snprintf(paths[0], 2, "%s%c", ".", DELIMITER);
			        }
			    }
			    CharPtr ptr2 = new CharPtr(new_paths); ptr1 = new CharPtr(new_paths);
			    do {
			    	while ((ptr2[0] != '\0') && (ptr2[0] != PATH_DELIMITER)) ptr2.inc();
			        if (ptr2 == ptr1) {
			            if (ptr2[0] == '\0') break;
			            ptr1.inc();
			            ptr2.inc();
			            continue;
			        } else {
			        	paths[cur_num] = new char[CharPtr.minus(ptr2, ptr1) + 2];
			            CharPtr dptr = paths[cur_num];
			            if (ptr1 != ptr2) {
			            	while (ptr1 != ptr2) { dptr[0] = ptr1[0]; dptr.inc(); ptr1.inc(); }
			            	if ((new CharPtr(dptr, -1))[0] != DELIMITER) {
			                    // put a slash on the end if there isn't one already
			                    dptr[0] = DELIMITER; dptr.inc();
			                }
			            }
			            dptr[0] = '\0';
			        }
			        if (ptr2[0] != '\0') {
			    		ptr1.inc();
			    		ptr2.inc();
			        }
			        //printf("added path: \"%s\"\n", paths[cur_num]);
			        cur_num++;
			    } while (ptr2[0] != '\0');
			
			    num_paths = cur_num;
			    paths[num_paths] = null;
			
			    // construct all_paths
			    uint len = 0;
			    for (cur_num=0; cur_num<num_paths; cur_num++) {
			        len += strlen(paths[cur_num]) + 1;
			    }
			
			    if (all_paths != null) {
			        all_paths = null;//delete[] all_paths;
			    }
			    all_paths = new char[len];
			
			    CharPtr dptr_ = new CharPtr(all_paths);
			    for (cur_num=0; cur_num<(num_paths-1); cur_num++) {
			        uint curlen = strlen(paths[cur_num]) + 1;
			        snprintf(dptr_, (int)len, "%s%c", paths[cur_num], PATH_DELIMITER);
			        dptr_.inc((int)curlen);
			        len -= curlen;
			    }
			    snprintf(dptr_, (int)len, "%s", paths[cur_num]);
			}
			
			public CharPtr get_path( int n )
			{
				if ((n == 0) || ((n > 0) && (n < num_paths))) {
			        if (null!=paths)
			            return paths[n];
			        else
			            return "";
			    }
			    return null;
			}
			
			// Returns a delimited string containing all paths
			public CharPtr get_all_paths()
			{
				if (all_paths != null)
			        return all_paths;
			    else
			        return "";
			}
			
			public int get_num_paths() 
			{
				return num_paths;
			}
			
			// Returns the length of the longest path
			public uint max_path_len()
			{
				uint len = 0;
			    if (paths != null) {
			        for (int i=0; i<num_paths;i++) {
			            if (strlen(paths[i]) > len)
			                len = strlen(paths[i]);
			        }
			    }
			    return len;
			}
		}
	}
}
