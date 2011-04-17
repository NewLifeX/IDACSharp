
#pragma once

// The header file of the LIB to be linked
#include <ida.hpp>
#include <ua.hpp>
#include <lines.hpp>

using namespace System;


namespace IDACSharp {
	//操作类型
	public enum class OperandsType : short
	{
		Void     =  0, // No Operand                           ----------
		Reg      =  1, // General Register (al,ax,es,ds...)    reg
		Mem      =  2, // Direct Memory Reference  (DATA)      addr
		Phrase   =  3, // Memory Ref [Base Reg + Index Reg]    phrase
		Displ    =  4, // Memory Reg [Base Reg + Index Reg + Displacement] phrase+addr
		imm      =  5, // Immediate Value                      value
		Far      =  6, // Immediate Far Address  (CODE)        addr
		Near     =  7, // Immediate Near Address (CODE)        addr
		Idpspec0 =  8, // IDP specific type
		Idpspec1 =  9, // IDP specific type
		Idpspec2 = 10, // IDP specific type
		Idpspec3 = 11, // IDP specific type
		Idpspec4 = 12, // IDP specific type
		Idpspec5 = 13, // IDP specific type
		Last     = 14, // first unused type
	};

	// 操作数值类型
	public enum class OperandsValueType : short
	{
		d_byte        = 0 ,      // 8 bit
		d_word        = 1 ,      // 16 bit
		d_dword       = 2 ,      // 32 bit
		d_float       = 3 ,      // 4 byte
		d_double      = 4 ,      // 8 byte
		d_tbyte       = 5 ,      // variable size (ph.tbyte_size)
		d_packreal    = 6 ,      // packed real format for mc68040
		d_qword       = 7 ,      // 64 bit
		d_byte16      = 8 ,      // 128 bit
		d_code        = 9 ,      // ptr to code (not used?)
		d_void        = 10,      // none
		d_fword       = 11,      // 48 bit
		d_bitfild     = 12,      // bit field (mc680x0)
		d_string      = 13,      // pointer to asciiz string
		d_unicode     = 14,      // pointer to unicode string
		d_3byte       = 15,      // 3-byte data
	};

	//操作
	public ref class Operands {
	private:
		op_t* op;

	public:
		ushort Num;	// 操作数个数
		OperandsType Type;	// 操作数类型
		ushort Offb;
		ushort Offo;

		OperandsValueType ValueType;
		ulong Value;

		ushort Reg;                 // number of register (o_reg)

		Int64 Address;
	};

	//指令类型
	public enum class InstructionType : short
	{
		Void     =  0, // No Operand                           ----------
		Reg      =  1, // General Register (al,ax,es,ds...)    reg
		Mem      =  2, // Direct Memory Reference  (DATA)      addr
		Phrase   =  3, // Memory Ref [Base Reg + Index Reg]    phrase
		Displ    =  4, // Memory Reg [Base Reg + Index Reg + Displacement] phrase+addr
		imm      =  5, // Immediate Value                      value
		Far      =  6, // Immediate Far Address  (CODE)        addr
		Near     =  7, // Immediate Near Address (CODE)        addr
		Idpspec0 =  8, // IDP specific type
		Idpspec1 =  9, // IDP specific type
		Idpspec2 = 10, // IDP specific type
		Idpspec3 = 11, // IDP specific type
		Idpspec4 = 12, // IDP specific type
		Idpspec5 = 13, // IDP specific type
		Last     = 14, // first unused type
	};

	//指令
	public ref class Instruction {
	private:
		insn_t* ins;

	public:
		Instruction (insn_t* i) {
			ins = i;

			Type = (InstructionType)ins->itype;
		}
		
		InstructionType Type;
		ushort Size;

		Int64 CS;
		Int64 IP;
		Int64 EA;
		
		Operands^ Operands1;
		Operands^ Operands2;
		Operands^ Operands3;
		Operands^ Operands4;
		Operands^ Operands5;
		Operands^ Operands6;

		int GetImmVals(int n, ulong* v) {
			return get_operand_immvals(ins->ea, n, v);
		}

		static Instruction^ GetCurrent() {
			return gcnew Instruction(&cmd);
		}

		static String^ GetOpnd(ea_t ea, int n) {
			char buf[256];
			ua_outop(ea, buf, sizeof(buf) - 1, n);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}

		static String^ GetDisasm(ea_t ea) {
			char buf[256];
			generate_disasm_line(ea, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}
	};
}