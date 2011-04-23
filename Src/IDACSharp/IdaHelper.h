
#pragma once

//#include <msclr/marshal.h>

using namespace System;
using namespace System::Runtime::InteropServices;
//using namespace msclr::interop;

namespace IDACSharp 
{
#ifndef __IDAHELPER__
#define __IDAHELPER__
	// IDA
	ref class IdaHelper
	{
	internal:
		// 转换字符串为char*
		static const char* CastStringToChar(String^ str){
			if(String::IsNullOrEmpty(str)) return NULL;

			//marshal_context context;
			//return context.marshal_as<char*>(str);
			IntPtr ip = Marshal::StringToHGlobalAnsi(str);
			return static_cast<const char*>(ip.ToPointer());
		}

		// 转换char*为字符串
		static String^ CastCharToString(char* str){
			//return marshal_as<String^>(str);
			return Marshal::PtrToStringAnsi(static_cast<IntPtr>(str));
		}

		static String^ CastCharToString(const char* str){
			//return marshal_as<String^>(str);
			char* str2 = const_cast<char*>(str);
			return Marshal::PtrToStringAnsi(static_cast<IntPtr>(str2));
		}

		static String^ CastCharToString(wchar_t* str){
			return Marshal::PtrToStringUni(static_cast<IntPtr>(str));
		}

		static String^ CastCharToString(const wchar_t* str){
			wchar_t* str2 = const_cast<wchar_t*>(str);
			return Marshal::PtrToStringAnsi(static_cast<IntPtr>(str2));
		}

		//将托管数组填充到非托管
		static int PadingToUnmanagedArray(array<uint16>^ managedArray,uint16 ValList[],int iLen)
		{
			if (nullptr == managedArray)
			{
				return -1;
			}
			if (managedArray->Length!=iLen)
			{
				return -2;
			}
			pin_ptr<uint16> p = &managedArray[0];
			uint16* pSource = p;
			//uint16* pDestination = new uint16[managedArray->Length];
			::memcpy( ValList, pSource, managedArray->Length * sizeof(uint16) );
			return 1;
		}

		//将非托管数组封底到托管
		static int PaddingToManagedArray(uint16 ValList[],int iLen,array<uint16>^ mArray)
		{
			if(nullptr==ValList)
			{
				return -1;
			}
			if(iLen!=mArray->Length)
			{
				return -2;
			}
			
			pin_ptr<uint16> p=&mArray[0];
			uint16 *pDest=p;
			uint16 *pSrc=ValList;
			::memcpy(pDest,pSrc,iLen*sizeof(uint16));
			return 1;
		}
	};
#endif
}