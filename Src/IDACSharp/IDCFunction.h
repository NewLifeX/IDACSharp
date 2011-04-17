
#pragma once

using namespace System;

#include <expr.hpp>

#include "IDA.h"
#include "Bytes.h"

namespace IDACSharp {

	// IDC值类型
	public enum class IDCValueTypes : short {
		STR = 1,
		LONG = 2,
		FLOAT = 3,
		// 可变参数
		WILD = 4
	};

	// IDC值
	public ref class IDCValue
	{
	private:

	public:
		idc_value_t* ptr;

		IDCValue(){
			ptr = new idc_value_t();
		}

		property IDCValueTypes Type { IDCValueTypes get(){ return (IDCValueTypes)(ptr->vtype); } void set(IDCValueTypes value) {ptr->vtype=(char)value;} }

		property String^ StringValue {
			String^ get(){
				if (Type!=IDCValueTypes::STR) return nullptr;
				return IDA::CastCharToString(ptr->str);
			}
			void set(String^ value){
				Type = IDCValueTypes::STR;
				ptr->str = (char*)IDA::CastStringToChar(value);
			}
		}

		property sval_t LongValue {
			sval_t get(){
				if (Type!=IDCValueTypes::LONG) return 0;
				return ptr->num;
			}
			void set(sval_t value){
				Type = IDCValueTypes::LONG;
				ptr->num = value;
			}
		}

		//property ushort[6] FloatValue {
		//	ushort[6] get(){
		//		if (Type!=IDCValueTypes::FLOAT) return 0;
		//		return ptr->e;
		//	}
		//	void set(ushort[6] value){
		//		Type = IDCValueTypes::FLOAT;
		//		ptr->e = value;
		//	}
		//}


		~IDCValue(){
			if (ptr != NULL) delete ptr;
		}
	protected:
		!IDCValue(){
			if (ptr != NULL) delete ptr;
		}
	};

	// 脚本函数
	public ref class IDCFunction
	{
	private:
		extfun_t* ptr;

	public:
		property String^ Name { String^ get(){ return IDA::CastCharToString(ptr->name); }}

		property List<IDCValueTypes>^ Args { 
			List<IDCValueTypes>^ get(){
				if (ptr->args==NULL || ptr->args[0]=='\0') return nullptr;

				List<IDCValueTypes>^ cs = gcnew List<IDCValueTypes>();
				for (const char* p = ptr->args; p!=NULL&&*p!='\0'; p++) cs->Add((IDCValueTypes)(*p));
				return cs;
			}
		}

		property int Flag { int get(){ return ptr->flags; }}

		static int TotalCount(){ return IDCFuncs.qnty; }

		static List<IDCFunction^>^ FindAll(){ 
			List<IDCFunction^>^ list = gcnew List<IDCFunction^>();
			for (int i=0; i<IDCFuncs.qnty; i++){
				IDCFunction^ entity = gcnew IDCFunction();
				entity->ptr = IDCFuncs.f + i;
				list->Add(entity);
			}
			return list;
		}

		static IDCValue^ Eval(ea_t ea, String^ idcexp){
			IDCValue^ rv = gcnew IDCValue();
			char* errbuf = new char[256];
			if(!calc_idc_expr(ea, IDA::CastStringToChar(idcexp), rv->ptr, errbuf, 255)) throw gcnew Exception(IDA::CastCharToString(errbuf));
			return rv;
		}

		static String^ EvalAndReturnString(String^ idcexp){
			IDCValue^ rv = Eval(Bytes::BadAddress, idcexp);

			if (rv->Type == IDCValueTypes::STR)
				return rv->StringValue;
			else
				return nullptr;
		}

		static sval_t EvalAndReturnLong(String^ idcexp){
			IDCValue^ rv = Eval(Bytes::BadAddress, idcexp);

			if(rv->Type == IDCValueTypes::LONG)
				return rv->LongValue;
			else
				return 0;
		}

		static bool Execute(String^ idcexp){ return execute(IDA::CastStringToChar(idcexp)); }

		static bool CompileFile(String^ file){ 
			char* errbuf = new char[256];
			bool b = compile_script_file(IDA::CastStringToChar(file), errbuf, 255); 
			if (!b) throw gcnew Exception(IDA::CastCharToString(errbuf));
			return b;
		}

		static bool DoSysFile(String^ file){ return dosysfile(true, IDA::CastStringToChar(file)); }
	};
}