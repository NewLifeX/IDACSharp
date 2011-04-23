
#pragma once

using namespace System;
using namespace System::Text;

#include <xref.hpp>

namespace IDACSharp {

	// 代码引用类型
	public enum class CodeRefType:short
	{
		fl_U,                         // unknown -- for compatibility with old
		// versions. Should not be used anymore.
		fl_CF = 16,                   // Call Far
		// This xref creates a function at the
		// referenced location
		fl_CN,                        // Call Near
		// This xref creates a function at the
		// referenced location
		fl_JF,                        // Jump Far
		fl_JN,                        // Jump Near
		fl_USobsolete,                // User specified (obsolete)
		fl_F,                         // Ordinary flow: used to specify execution
		// flow to the next instruction.
	};

	// 数据引用类型
	public enum class DataRefType:short
	{
		dr_U,                         // Unknown -- for compatibility with old
		// versions. Should not be used anymore.
		dr_O,                         // Offset
		// The reference uses 'offset' of data
		// rather than its value
		//    OR
		// The reference appeared because the "OFFSET"
		// flag of instruction is set.
		// The meaning of this type is IDP dependent.
		dr_W,                         // Write access
		dr_R,                         // Read access
		dr_T,                         // Text (for forced operands only)
		// Name of data is used in manual operand
		dr_I,                         // Informational
		// (a derived java class references its base
		//  class informatonally)
	};

	// 引用
	public ref class Ref
	{
	public:

		static String^ SegName(ea_t ea)
		{
			segment_t* seg = getseg(ea);
			if (seg == NULL)
			{ 
				return String::Empty;
			}
			char temp[256];
			get_true_segm_name(seg, temp, sizeof(temp));
			String^ str = gcnew String(temp);
			//delete[] temp;
			return str;
		}

		static bool AddCode(ea_t from, ea_t to, CodeRefType type) {
			return add_cref(from, to, (cref_t)(short)type);
		}

		static int DelCode(ea_t from, ea_t to, bool expand) {
			return del_cref(from, to, expand);
		}

		static bool AddData(ea_t from, ea_t to, DataRefType type) {
			return add_dref(from, to, (dref_t)(short)type);
		}

		static void DelData(ea_t from, ea_t to) {
			del_dref(from, to);
		}

		// 引用查找
		static ea_t GetFirstDataRefFrom(ea_t ea) { return get_first_dref_from(ea); }
		static ea_t GetNextDataRefFrom(ea_t ea, ea_t current) { return get_next_dref_from(ea, current); }
		static ea_t GetFirstDataRefTo(ea_t ea) { return get_first_dref_to(ea); }
		static ea_t GetNextDataRefTo(ea_t ea,ea_t current) { return get_next_dref_to(ea, current); }
		static ea_t GetFirstCodeRefFrom(ea_t ea) { return get_first_cref_from(ea); }
		static ea_t GetNextCodeRefFrom(ea_t ea, ea_t current) { return get_next_cref_from(ea, current); }
		static ea_t GetFirstCodeRefTo(ea_t ea) { return get_first_cref_to(ea); }
		static ea_t GetNextCodeRefTo(ea_t ea,ea_t current) { return get_next_cref_to(ea, current); }
	};
}