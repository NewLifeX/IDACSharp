
#pragma once

using namespace System;
using namespace System::Text;

// The header file of the LIB to be linked
#include <ida.hpp>

#include "IDA.h"

namespace IDACSharp {
	//文件类型
	public enum class FileType : short
	{
		EXE_old,            // MS DOS EXE File
		COM_old,            // MS DOS COM File
		BIN,                // Binary File
		DRV,                // MS DOS Driver
		WIN,                // New Executable (NE)
		HEX,                // Intel Hex Object File
		MEX,                // MOS Technology Hex Object File
		LX,                 // Linear Executable (LX)
		LE,                 // Linear Executable (LE)
		NLM,                // Netware Loadable Module (NLM)
		COFF,               // Common Object File Format (COFF)
		PE,                 // Portable Executable (PE)
		OMF,                // Object Module Format
		SREC,               // R-records
		ZIP,                // ZIP file (this file is never loaded to IDA database)
		OMFLIB,             // Library of OMF Modules
		AR,                 // ar library
		LOADER,             // file is loaded using LOADER DLL
		ELF,                // Executable and Linkable Format (ELF)
		W32RUN,             // Watcom DOS32 Extender (W32RUN)
		AOUT,               // Linux a.out (AOUT)
		PRC,                // PalmPilot program file
		EXE,                // MS DOS EXE File
		COM,                // MS DOS COM File
		AIXAR,              // AIX ar library
		MACHO               // Max OS X
	};

	// 编译器信息
	public ref struct CompilerInfo
	{
		uchar id;             // compiler id (see typeinf.hpp, COMP_...)
		uchar cm;             // memory model and calling convention (typeinf.hpp, CM_...)
		uchar size_i;         // sizeof(int)
		uchar size_b;         // sizeof(bool)
		uchar size_e;         // sizeof(enum)
		uchar defalign;       // default alignment for structures
		uchar size_s;         // short
		uchar size_l;         // long
		uchar size_ll;        // longlong
							  // NB: size_ldbl is stored separately!

		virtual String^ ToString() override {
			StringBuilder^ sb = gcnew StringBuilder();
			sb->AppendFormat("{0}={1}", "id", id);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "cm", cm);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "size_i", size_i);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "size_b", size_b);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "defalign", defalign);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "size_s", size_s);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "size_s", size_s);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "size_l", size_l);
			sb->AppendLine();
			sb->AppendFormat("{0}={1}", "size_ll", size_ll);
			sb->AppendLine();
			return sb->ToString();
		}
	};

	// IDA信息
	public ref class IdaInfo {
	private:
		IdaInfo() { } //私有构造函数，不允许外部创建实例
		static IdaInfo^ _Instance;

	public:
		ushort Version;
		String^ Processor;
		FileType Type;

		Int64 StartSP;
		Int64 StartIP;

		Int64 BeginEA;
		Int64 MinEA;
		Int64 MaxEA;
		Int64 Main;

		uchar AsmType;
		UInt32 BaseAddress;

		CompilerInfo^ Compiler;

		static property IdaInfo^ Instance {
			IdaInfo^ get() {
				if (_Instance == nullptr) {
					_Instance = gcnew IdaInfo();

					_Instance->Version = inf.version;
					_Instance->Version = inf.version;
					_Instance->Type = (FileType)inf.filetype;

					_Instance->StartSP = inf.startSP;
					_Instance->StartIP = inf.startIP;

					_Instance->BeginEA = inf.beginEA;
					_Instance->MinEA = inf.minEA;
					_Instance->MaxEA = inf.maxEA;
					_Instance->Main = inf.main;

					_Instance->AsmType = inf.asmtype;
					_Instance->BaseAddress = inf.baseaddr;
					CompilerInfo^ ci = gcnew CompilerInfo();
					_Instance->Compiler = ci;
					ci->id = inf.cc.id;
					ci->cm = inf.cc.cm;
					ci->size_i = inf.cc.size_i;
					ci->size_b = inf.cc.size_b;
					ci->size_e = inf.cc.size_e;
					ci->defalign = inf.cc.defalign;
					ci->size_s = inf.cc.size_s;
					ci->size_l = inf.cc.size_l;
					ci->size_ll = inf.cc.size_ll;
				}
				return _Instance;
			}
		}

		static String^ GetInputFilePath(){
			char ch[256];
			get_input_file_path(ch, sizeof(ch) - 1);
			return IDA::CastCharToString(ch);
		}
	};
}