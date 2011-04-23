
#pragma once

using namespace System;

#include <bytes.hpp>

#include "Bytes.h"

namespace IDACSharp {

	// ½Å±¾
	public ref class IDC
	{
	public:
		static property ulong BadAddress { ulong get(){ return Bytes::BadAddress; }}
		static property ulong BadSelect { ulong get(){ return Bytes::BadSelect; }}
		static property ulong MaxAddress { ulong get(){ return Bytes::MaxAddress; }}

		static bool HasValue(flags_t ea){ return Bytes::HasValue(ea); }

		static bool MakeName(ea_t ea, String^ name, int flag){ return set_name(ea, IdaHelper::CastStringToChar(name), flag); }
		static bool MakeName(ea_t ea, String^ name){ return set_name(ea, IdaHelper::CastStringToChar(name), SN_CHECK); }

		static int MakeCode(ea_t ea){ return create_insn(ea); }

		static int AnalyzeArea(ea_t start, ea_t end){ return analyze_area(start, end); }
	};
}