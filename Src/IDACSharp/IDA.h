
#pragma once

//#include <msclr/marshal.h>

using namespace System;
using namespace System::Runtime::InteropServices;
//using namespace msclr::interop;

namespace IDACSharp {

	// IDA
	ref class IDA
	{
	internal:
		// ×ª»»×Ö·û´®Îªchar*
		static const char* CastStringToChar(String^ str){
			if(String::IsNullOrEmpty(str)) return NULL;

			//marshal_context context;
			//return context.marshal_as<char*>(str);
			IntPtr ip = Marshal::StringToHGlobalAnsi(str);
			return static_cast<const char*>(ip.ToPointer());
		}

		// ×ª»»char*Îª×Ö·û´®
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
	};
}