/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-7-11
 * Time: 18:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of FILEPtr.
	/// </summary>
	public class FILEPtr
	{
		public Stream stream;
		
		public FILEPtr()
		{
			
		}
		
		public FILEPtr(Stream stream)
		{
			this.stream = stream;
		}
	}
}
