
#pragma once

using namespace System;

#include <entry.hpp>

#include "Bytes.h"

namespace IDACSharp {

	// Èë¿Ú
	public ref class Entry
	{
	public:
		static size_t TotalCount(){ return get_entry_qty(); }

		static bool Add(uval_t ord, ea_t ea, String^ name, bool makecode){
			return add_entry(ord, ea, IDA::CastStringToChar(name), makecode);
		}

		static uval_t GetEntryOrdinal(size_t idx){ return get_entry_ordinal(idx); }
		static ea_t GetEntryPoint(uval_t ord){ return get_entry(ord); }
		
		static String^ GetEntryName(uval_t ord){
			char ch[256];
			get_entry_name(ord, ch, sizeof(ch) - 1);
			return IDA::CastCharToString(ch);
		}

		static bool RenameEntryPoint(uval_t ord, String^ name){ return rename_entry(ord, IDA::CastStringToChar(name)); }
	};
}