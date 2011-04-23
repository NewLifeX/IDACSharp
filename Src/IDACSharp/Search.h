
#pragma once

using namespace System;

#include <search.hpp>

#include "IdaHelper.h"

namespace IDACSharp {

	// ËÑË÷
	public ref class Search
	{
	public:
		static ea_t FindText(ea_t ea, long flag, long y, long x, String^ str){ return find_text(ea, y, x, IdaHelper::CastStringToChar(str), flag); }
		static ea_t FindTextUp(ea_t ea, String^ str){ return FindText(ea, 0, 0, 0, str); }
		static ea_t FindTextDown(ea_t ea, String^ str){ return FindText(ea, 1, 0, 0, str); }
		static ea_t FindTextNext(ea_t ea, String^ str){ return FindText(ea, 2, 0, 0, str); }
	};
}