
#pragma once

using namespace System;

// The header file of the LIB to be linked
#include <ida.hpp>
#include <idp.hpp>

#include "area.h"
#include "IdaHelper.h"


namespace IDACSharp {
	[Flags]
	public enum class FunctionFlag : short
	{
		NoRet = 1,
		Far = 2,
		Lib = 4,
		Static = 8,
		Frame = 0x10,
		UserFar = 0x20,
		Hidden = 0x40,
		Thunk = 0x80,
		ButtomBP = 0x100,
		NoRetPending = 0x200,
		SPReady = 0x400,
		PurgedOK = 0x800,
		Tail = 0x1000
	};

	// 函数
	public ref class Function : public Area {
	private:
		Function(func_t* fun){
			func = fun;
		}

		func_t* _func;
		// 内部变量
		property func_t* func
		{
			func_t* get()
			{
				if (_func == NULL){
					_func = get_func(Start);
				}
				return _func;
			}
			void set(func_t* value)
			{
				_func = value;
				Flag = (FunctionFlag)value->flags;
			}
		}

	protected:
		!Function() {
			if (_func != NULL) delete func;
		}

	public:
		static Function^ FindByAddress(ulong ea){
			func_t* f = get_func(ea);
			if (f == NULL) return nullptr;
			
			return gcnew Function(f);
		}

		~Function() {
			if (_func != NULL) delete func;
		}

		//开始
		virtual property ulong Start {
			ulong get() override { return func->startEA; }
			void set(ulong value) override {
				if(func->startEA != value) func_setstart(func->startEA, value);
				func->startEA = value;
			}
		}

		//结束
		virtual property ulong End {
			ulong get() override { return func->endEA; }
			void set(ulong value) override {
				if(func->endEA != value) func_setend(func->endEA, value);
				func->endEA = value;
			}
		}

		//标志
		property FunctionFlag Flag {
			FunctionFlag get() { return (FunctionFlag)func->flags; }
			void set(FunctionFlag value) { func->flags = (ushort)value; }
		}

		//某个区域是否完全包含在函数内
		bool Contain(Area^ area){
			area_t* limits = new area_t(area->Start, area->End);
			return get_func_limits(func, limits);
		}

		//名称
		property String^ Name {
			String^ get(){
				return GetFunctionName(Start);
			}
		}

		// 取得函数名
		static String^ GetFunctionName(ea_t ea){
			char chs[255];
			get_func_name(ea, chs, sizeof(chs) - 1);
			String^ str = gcnew String(chs);
			//delete[] chs;
			return str;
		}

		//注释
		property String^ Comment {
			String^ get(){
				return gcnew String(get_func_cmt(func, false));
			}
			void set(String^ value){
				if(String::IsNullOrEmpty(value))
					del_func_cmt(func, false);
				else {
					//char* chs = new char[value->Length + 1];
					//for	(int i=0; i<value->Length; i++) {
					//	chs[i] = value[i];
					//}
					set_func_cmt(func, IdaHelper::CastStringToChar(value), false);
					//delete chs;
				}
			}
		}

		//可重复注释
		property String^ RepeatableComment {
			String^ get(){
				return gcnew String(get_func_cmt(func, true));
			}
			void set(String^ value){
				if(String::IsNullOrEmpty(value))
					del_func_cmt(func, true);
				else {
					//char* chs = new char[value->Length + 1];
					//for	(int i=0; i<value->Length; i++) {
					//	chs[i] = value[i];
					//}
					set_func_cmt(func, IdaHelper::CastStringToChar(value), true);
					//delete chs;
				}
			}
		}

		void ReAnalyze() {
			ReAnalyze(0, -1, false);
		}

		void ReAnalyze(ulong start, ulong end, bool analyze_parents) {
			reanalyze_function(func, start, end, analyze_parents);
		}

		bool Update(){
			return update_func(func);
		}

		bool Add(){
			return add_func_ex(func);
		}

		static bool Add(ulong ea1, ulong ea2){
			return add_func(ea1, ea2);
		}

		bool Delete(){
			return Delete(Start);
		}

		static bool Delete(ulong ea){
			return del_func(ea);
		}

		static Function^ GetItem(int n){
			func_t* f = getn_func(n);
			if (f == NULL) return nullptr;

			return gcnew Function(f);
		}

		static int TotalCount(){
			return get_func_qty();
		}

		static int IndexOf(ulong ea){
			return get_func_num(ea);
		}

		static Function^ GetPrev(ulong ea){
			func_t* f = get_prev_func(ea);
			if (f == NULL) return nullptr;

			return gcnew Function(f);
		}

		static Function^ GetNext(ulong ea){
			func_t* f = get_next_func(ea);
			if (f == NULL) return nullptr;

			return gcnew Function(f);
		}
	};
}