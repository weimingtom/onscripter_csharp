﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-9-12
 * Time: 10:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace onscripter_csharp
{
	public partial class ONScripter {
		public class time_t {
			
		}
		
		public class tm {
			public int tm_year, tm_mon, tm_mday;
			public int tm_hour, tm_min, tm_sec;
		}
		
		public static time_t time(time_t t) {
			return new time_t();
		}
		
		public static tm localtime(time_t t) {
			return new tm();
		}
		
		public static CharPtr strcpy(CharPtr dst, CharPtr src)
		{
			return null;
		}
		
		public static uint strlen(CharPtr str)
		{
			return 0;
		}
		
				
		public static CharPtr strcat(CharPtr dst, CharPtr src)
		{
			return null;	
		}
		
		public static void printf(string str, params Object[] args) 
		{
			
		}
		
		public static void snprintf(CharPtr dst, int n, string str, params Object[] args)
		{
			
		}
		
		public static int strcmp(CharPtr s1, CharPtr s2)
		{
			return 0;
		}
		
		public static int atoi(CharPtr str)
		{
			return 0;
		}
		
		public static double cos(double x)
		{
			return 0;
		}
		
		public static void sprintf(CharPtr dst, string str, params Object[] args)
		{
			
		}
		
		public static void memcpy(CharPtr s1, CharPtr s2, uint length)
		{
			
		}
		
		public static CharPtr strrchr(CharPtr str, char ch)
		{
			return null;
		}
		
		public static int strncmp(CharPtr str1, CharPtr str2, int n)
		{
			return 0;
		}
		
		public static double sin(double d)
		{
			return 0;
		}
		
		public static double tan(double d)
		{
			return 0;
		}
	}
}
