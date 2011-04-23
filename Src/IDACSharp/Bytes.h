
#pragma once

using namespace System;

#include <bytes.hpp>
#include <name.hpp>
#include <struct.hpp>

#include "IdaHelper.h"

namespace IDACSharp 
{

	// 数据类型
	[Flags]
	public enum class DataType : uint64 {
BYTE     = 0x00000000L,         // byte
WORD     = 0x10000000L,         // word
DWRD     = 0x20000000L,         // double word
QWRD     = 0x30000000L,         // quadro word
TBYT     = 0x40000000L,         // tbyte
ASCI     = 0x50000000L,         // ASCII ?
STRU     = 0x60000000L,         // Struct ?
OWRD     = 0x70000000L,         // octaword (16 bytes)
FLOAT    = 0x80000000L,         // float
DOUBLE   = 0x90000000L,         // double
PACKREAL = 0xA0000000L,         // packed decimal real
ALIGN    = 0xB0000000L,         // alignment directive
F3BYTE   = 0xC0000000L,         // 3-byte data (only with support from
                                     // the processor module)
F0OFF    = 0x00500000L,         // byte
	};

	// 字符串类型
	public enum class StringType : short {
		C        =0,       // C-style ASCII string Character-terminated ASCII string
		Pascal   =1,       // Pascal-style ASCII string (length byte)
		Len2     =2,       // Pascal-style, length is 2 bytes
		Unicode  =3,       // Unicode string
		Len4     =4,       // Pascal-style, length is 4 bytes
		ULen2    =5,       // Pascal-style Unicode, length is 2 bytes
		ULen4    =6        // Pascal-style Unicode, length is 4 bytes
	};


	// 字节
	public ref class Bytes
	{
	public:
		// 地址常量
		static const ulong BadAddress = BADADDR;
		static const ulong BadSelect  = BADSEL;
		static const ulong MaxAddress = MAXADDR;

		// 标识常量
		static bool HasValue(flags_t F){ return hasValue(F); }
		static flags_t GetFlags(ea_t ea){ return getFlags(ea); }
		static void SetFlags(ea_t ea, flags_t flags){ return setFlags(ea, flags); }

		// 字节常量
		static bool IsCode(flags_t F){ return isCode(F); }
		static bool IsData(flags_t F){ return isData(F); }
		static bool IsTail(flags_t F){ return isTail(F); }
		static bool IsUnknown(flags_t F){ return isUnknown(F); }
		static bool IsHead(flags_t F){ return isHead(F); }

		// Common bits
		static bool IsFlow(flags_t F){ return isFlow(F); }
		static bool IVar(flags_t F){ return isVar(F); }
		static bool IsExtra(flags_t F){ return hasExtra(F); }
		static bool HasComment(flags_t F){ return has_cmt(F); }
		static bool HasRef(flags_t F){ return hasRef(F); }
		static bool HasName(flags_t F){ return has_name(F); }
		static bool HasDumyName(flags_t F){ return has_dummy_name(F); }
		static bool HasAutoName(flags_t F){ return has_auto_name(F); }
		static bool HasAnyName(flags_t F){ return has_any_name(F); }
		static bool HasUserName(flags_t F){ return has_user_name(F); }

		// Bits for DATA bytes
		static bool IsByte(flags_t F){ return isByte(F); }
		static bool IsWord(flags_t F){ return isWord(F); }
		static bool IsDwrd(flags_t F){ return isDwrd(F); }
		static bool IsQwrd(flags_t F){ return isQwrd(F); }
		static bool IsOwrd(flags_t F){ return isOwrd(F); }
		static bool IsTbyt(flags_t F){ return isTbyt(F); }
		static bool IsFloat(flags_t F){ return isFloat(F); }
		static bool IsDouble(flags_t F){ return isDouble(F); }
		static bool IsPackReal(flags_t F){ return isPackReal(F); }
		static bool IsASCII(flags_t F){ return isASCII(F); }
		static bool IsStruct(flags_t F){ return isStruct(F); }
		static bool IsAlign(flags_t F){ return isAlign(F); }
		static bool Is3byte(flags_t F){ return is3byte(F); }

		// 读取数据
		static uchar IdbByte(ea_t ea) { return get_db_byte(ea); }
		static uchar Byte(ea_t ea) { return get_byte(ea); }
		static uchar GetOriginalByte(ea_t ea) { return get_original_byte(ea); }
		static ushort Word(ea_t ea) { return get_word(ea); }
		static uint32 Dword(ea_t ea) { return get_long(ea); }
		static uint64 Qword(ea_t ea) { return get_qword(ea); }

		// 名称
		static bool MakeName(ea_t ea, String^ name, int flag){ return set_name(ea, IdaHelper::CastStringToChar(name), flag); }
		static bool MakeName(ea_t ea, String^ name){ return set_name(ea, IdaHelper::CastStringToChar(name), SN_CHECK); }
		static bool MakeNameAnyway(ea_t ea, String^ name){ return do_name_anyway(ea, IdaHelper::CastStringToChar(name)); }

		static String^ GetName(ea_t from, ea_t ea) {
			char buf[256];
			get_name(from, ea, buf, sizeof(buf) - 1);
			return IdaHelper::CastCharToString(buf);
		}

		static String^ GetName(ea_t ea) {
			return GetName(BadAddress, ea);
		}

		static String^ GetTrueName(ea_t from, ea_t ea) {
			char buf[256];
			get_true_name(from, ea, buf, sizeof(buf) - 1);
			return IdaHelper::CastCharToString(buf);
		}

		static String^ GetTrueName(ea_t ea) {
			return GetName(BadAddress, ea);
		}

		// 标签
		static bool MakeLabel(ea_t ea, String^ label) {
			if (HasName(GetFlags(ea)) != 1)
			{
				//注释下面，强制命名
				if(isTail(GetFlags(ea))==1)
					do_unknown(ea, 0);
			} 	
			return MakeName(ea, label);
		}

		static bool MakeLabelAnyway(ea_t ea, String^ label) {
			if (HasName(GetFlags(ea)) != 1)
			{
				//注释下面，强制命名
				if(isTail(GetFlags(ea))==1)
					do_unknown(ea, 0);
			} 	
			return MakeNameAnyway(ea, label);
		}

		// 代码
		static int MakeCode(ea_t ea){ return create_insn(ea); }

		static int AnalyzeArea(ea_t start, ea_t end){ return analyze_area(start, end); }

		// Comments
		static bool MakeComment(ea_t ea, String^ comm){ return set_cmt(ea, IdaHelper::CastStringToChar(comm), 0); }
		static bool MakeRepeatableComment(ea_t ea, String^ comm){ return set_cmt(ea, IdaHelper::CastStringToChar(comm), 1); }
		static String^ GetComment(ea_t ea, bool repeatable){
			char chs[1024];
			get_cmt(ea, repeatable, chs, sizeof(chs) - 1); 
			String^ str = gcnew String(chs);
			//delete[] chs;
			return str;
		}
		//static String^ GetRepeatableComment(ea_t ea) {
		//	return gcnew String(get_repeatable_cmt(ea));
		//}

		// 字符串
		static size_t GetMaxAsciiLength(ea_t ea, StringType strtype) { return get_max_ascii_length(ea, (int32)strtype); }
		static String^ GetAscii(ea_t ea, int32 len, StringType type) {
			char buf[1024];
			if (len <= 1) len = GetMaxAsciiLength(ea, type);
			get_ascii_contents(ea, len, (int32)type, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}
		static bool MakeAscii(ea_t ea, int32 len, StringType type) {
			if (len <= 1) len = GetMaxAsciiLength(ea, type);
			return make_ascii_string(ea, len, (int32)type);
		}

		// 数据
		static int MakeData(ea_t ea, long flags, long size, long tid){ return do_data_ex(ea, flags, size, tid); }

		// 数组
		static int MakeArray(ea_t ea, long nitems){
			flags_t flags = GetFlags(ea);

			long itemsize = 0;
			long tid = 0;

			if (isUnknown(flags)) flags = 0;

			if (isStruct(flags)){
				typeinfo_t* ti = new typeinfo_t();
				get_typeinfo(ea, 0, flags, ti);
				itemsize = get_data_elsize(ea,flags, ti);
				tid = ti->tid;
			} else {
				itemsize = get_item_size(ea);
				tid = BADADDR;
			}
			return do_data_ex(ea, flags, itemsize, tid); 
		}

		// 结构体
		static bool MakeStruct(long ea, long size, tid_t tid){
			if (size == -1) size = get_struc_size(tid);

			do_unknown_range(ea, size, 0);

			return doStruct(ea, size, tid);
		}		
		static bool MakeStruct(long ea, long size, String^ name){
			tid_t tid = get_struc_id(IdaHelper::CastStringToChar(name));

			if (size == -1) size = get_struc_size(tid);

			do_unknown_range(ea, size, 0);

			return doStruct(ea, size, tid);
		}

		// 取消数据
		static void MakeUnknown(ea_t ea, int flags) { return do_unknown(ea, flags); }
		static void MakeUnknown(ea_t ea, size_t size, int flags) { return do_unknown_range(ea, size, flags); }

		// 地址
		static ea_t NextAddress(ea_t ea) { return nextaddr(ea); }
		static ea_t PrevAddress(ea_t ea) { return prevaddr(ea); }

		static ea_t NextHead(ea_t ea, int maxea) { return next_head(ea, maxea); }
		static ea_t PrevHead(ea_t ea, int minea) { return prev_head(ea, minea); }

		static ea_t NextNotTail(ea_t ea) { return next_not_tail(ea); }
		static ea_t PrevNotTail(ea_t ea) { return prev_not_tail(ea); }

		// 根据名称取得地址
		static ea_t LocByName(ea_t from, String^ name) { return get_name_ea(from, IdaHelper::CastStringToChar(name)); }
		static ea_t LocByName(String^ name) { return LocByName(Bytes::BadAddress, name); }

	};
}