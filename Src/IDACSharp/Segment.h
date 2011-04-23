
#pragma once

using namespace System;
using namespace System::Text;

#include <segment.hpp>

#include "IdaHelper.h"

namespace IDACSharp {

	// 段
	public ref class Segment : public Area {
	private:
		segment_t* ptr;

		Segment(segment_t* p){
			ptr = p;
		}

	protected:
		!Segment() {
			if (ptr != NULL) delete ptr;
		}

	public:
		static Segment^ FindByAddress(ulong ea){
			segment_t* f = getseg(ea);
			if (f == NULL) return nullptr;
			
			return gcnew Segment(f);
		}

		static Segment^ FindByName(String^ name){
			segment_t* f = get_segm_by_name(IdaHelper::CastStringToChar(name));
			if (f == NULL) return nullptr;
			
			return gcnew Segment(f);
		}

		~Segment() {
			if (ptr != NULL) delete ptr;
		}

		//开始
		virtual property ulong Start {
			ulong get() override { return ptr->startEA; }
			void set(ulong value) override {
				if(ptr->startEA != value) set_segm_start(ptr->startEA, value, SEGMOD_KEEP);
				ptr->startEA = value;
			}
		}

		//结束
		virtual property ulong End {
			ulong get() override { return ptr->endEA; }
			void set(ulong value) override {
				if(ptr->endEA != value) set_segm_end(ptr->endEA, value, SEGMOD_KEEP);
				ptr->endEA = value;
			}
		}

		//名称
		property String^ Name {
			String^ get(){
				char temp[256];
				get_segm_name(ptr, temp, sizeof(temp) - 1);

				return IdaHelper::CastCharToString(temp);
			}
		}

		//名称
		property String^ TrueName {
			String^ get(){
				char temp[256];
				get_true_segm_name(ptr, temp, sizeof(temp) - 1);

				return IdaHelper::CastCharToString(temp);
			}
		}

		static String^ SegName(ea_t ea){
			char temp[256];
			get_segm_name(ea, temp, sizeof(temp) - 1);

			return IdaHelper::CastCharToString(temp);
		}

		//注释
		property String^ Comment {
			String^ get(){
				return gcnew String(get_segment_cmt(ptr, false));
			}
			void set(String^ value){
				if(String::IsNullOrEmpty(value))
					del_segment_cmt(ptr, false);
				else {
					set_segment_cmt(ptr, IdaHelper::CastStringToChar(value), false);
				}
			}
		}

		//可重复注释
		property String^ RepeatableComment {
			String^ get(){
				return gcnew String(get_segment_cmt(ptr, true));
			}
			void set(String^ value){
				if(String::IsNullOrEmpty(value))
					del_segment_cmt(ptr, true);
				else {
					set_segment_cmt(ptr, IdaHelper::CastStringToChar(value), true);
				}
			}
		}
		//***************************扩展******************************************

		// 上一个
		property Segment^ Previous { 
			Segment^ get() {
				segment_t* p = get_prev_seg(ptr->startEA);
				if (p == NULL) return nullptr;

				return gcnew Segment(p);
			}
		}
		// 下一个
		property Segment^ Next { 
			Segment^ get() {
				segment_t* p = get_next_seg(ptr->startEA);
				if (p == NULL) return nullptr;

				return gcnew Segment(p);
			}
		}

		// 第一个
		static property Segment^ First { 
			Segment^ get() {
				segment_t* p = get_first_seg();
				if (p == NULL) return nullptr;

				return gcnew Segment(p);
			}
		}
		// 最后一个
		static property Segment^ Last { 
			Segment^ get() {
				segment_t* p = get_last_seg();
				if (p == NULL) return nullptr;

				return gcnew Segment(p);
			}
		}


		static int TotalCount(){
			return get_segm_qty();
		}

		static Segment^ GetItem(int n){
			segment_t* f = getnseg(n);
			if (f == NULL) return nullptr;

			return gcnew Segment(f);
		}
	};
}