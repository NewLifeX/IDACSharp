
#pragma once

using namespace System;
using namespace System::Collections::Generic;

#include <struct.hpp>

#include "IdaHelper.h"
#include "Bytes.h"

namespace IDACSharp {

	// 结构体成员
	public ref class Member {
	private:
		struc_t* sptr;
		member_t* ptr;

	internal:
		Member (struc_t* sentity, member_t* entity){
			ID = entity->id;
			Start = entity->soff;
			End = entity->eoff;
			Flag = entity->flag;
			Props = entity->props;

			sptr = sentity;
			ptr = entity;
		}

		// 根据序数找结构体的名字
		static String^ GetMemberName(tid_t id) {
			char buf[256];
			get_member_name(id, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}

		// 根据序数找结构体的完全名字
		static String^ GetMemberFullName(tid_t id) {
			char buf[256];
			get_member_fullname(id, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}

		// 根据序数找结构体的注释
		static String^ GetMemberComment(tid_t id, bool repeatable) {
			char buf[256];
			get_member_cmt(id, repeatable, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}

	public:
		tid_t ID;
		ea_t Start;
		ea_t End;
		flags_t Flag;
		uint32 Props;

		//***************************基础属性******************************************

		// 大小
		property asize_t Size { 
			asize_t get() {
				return get_member_size(ptr); 
			}
		}
		// 位移
		property ea_t Offset { 
			ea_t get() {
				return ptr->get_soff();
			}
		}
		// 名字
		property String^ Name {
			String^ get() {
				return GetMemberName(ID);
			}
			void set(String^ value) {
				set_member_name(sptr, Offset, IdaHelper::CastStringToChar(value));
			}
		}
		// 完全名字
		property String^ FullName {
			String^ get() {
				return GetMemberFullName(ID);
			}
		}
		// 注释
		property String^ Comment { 
			String^ get() {
				return GetMemberComment(ID, false);
			}
			void set(String^ value) {
				set_member_cmt(ptr, IdaHelper::CastStringToChar(value), false);
			}
		}

		//***************************扩展******************************************

		//// 上一个
		//property Member^ Previous { 
		//	Member^ get() {
		//		tid_t id = get_prev_struc_idx(ID);
		//		return FindStructByID(id);
		//	}
		//}
		//// 下一个
		//property Member^ Next { 
		//	Member^ get() {
		//		tid_t id = get_next_struc_idx(ID);
		//		return FindStructByID(id);
		//	}
		//}

		//***************************方法******************************************

		// 转为结构体。这里因为没办法试用Struct类，所以字节返回一个ID，自己构造Struct
		tid_t AsStructID() {
			struc_t* entity = get_sptr(ptr);
			if (entity == NULL) return 0;

			return entity->id;
		}
	};

	// 结构体
	public ref class Struct
	{
	private:
		tid_t _ID;
		struc_t* ptr;

		Struct (struc_t* entity){
			ID = entity->id;
			Props = entity->props;

			if (entity->memqty > 0) {
				Members = gcnew List<Member^>();
				for(size_t i=0; i<entity->memqty; i++) {
					Member^ item = gcnew Member(entity, entity->members + i);
					Members->Add(item);
				}
			}

			ptr = entity;
		}

		// 根据序数找结构体的名字
		static String^ GetStructName(tid_t id) {
			char buf[256];
			get_struc_name(id, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}

		// 根据序数找结构体的注释
		static String^ GetStructComment(tid_t id, bool repeatable) {
			char buf[256];
			get_struc_cmt(id, repeatable, buf, sizeof(buf) - 1);
			String^ str = gcnew String(buf);
			//delete[] buf;
			return str;
		}

		// 准备类型信息
		static const typeinfo_t* PrepareStrucMemberTypeinfo(flags_t flag, ea_t tid, ea_t target, adiff_t tdelta, uchar reftype) {
			typeinfo_t* ti = new typeinfo_t();
			//#define FF_DATA 0x00000400L             // Data ?
			flag |= 0x00000400L;

			if (isOff0(flag)){
				refinfo_t* ri = new refinfo_t();
				ri->base = tid;

				if (target > Bytes::BadAddress){
					ri->target = target;
					ri->tdelta = 0;
					ri->flags = reftype;
				}else{
					ri->target = Bytes::BadAddress;
					ri->tdelta = 0;

					if (isWord(flag))
						ri->flags = REF_OFF16;
					else if (isByte(flag))
						ri->flags = REF_OFF8;
					else if (isQwrd(flag))
						ri->flags = REF_OFF64;
					else
						ri->flags = REF_OFF32;
				}

				ti->ri = *ri;
			} else if (isEnum0(flag)){
				enum_const_t* ec = new enum_const_t();
				ec->tid = tid;
				ec->serial = 0;
				ti->ec = *ec;
			} else if (isStroff0(flag)){
				ti->path.len = 1;
				ti->path.ids[0] = tid;
			} else {
				ti->tid = tid;
			}

			return ti;
		}

	public:
		List<Member^>^ Members;
		uint32 Props;

		//***************************基础属性******************************************

		// 序数
		property tid_t ID { 
			tid_t get() {
				return _ID; 
			}
			void set(tid_t value) {
				if (_ID != value) {
					if (_ID != 0) {
						set_struc_idx(ptr, value);
					}
					_ID = value;
				}
			}
		}
		// 大小
		property asize_t Size { 
			asize_t get() {
				return get_struc_size(ID); 
			}
		}
		// 名字
		property String^ Name {
			String^ get() {
				return GetStructName(ID);
			}
			void set(String^ value) {
				set_struc_name(ID, IdaHelper::CastStringToChar(value));
			}
		}
		// 注释
		property String^ Comment { 
			String^ get() {
				return GetStructComment(ID, false);
			}
			void set(String^ value) {
				set_struc_cmt(ID, IdaHelper::CastStringToChar(value), false);
			}
		}

		//***************************扩展******************************************

		// 上一个
		property Struct^ Previous { 
			Struct^ get() {
				tid_t id = get_prev_struc_idx(ID);
				return FindStructByID(id);
			}
		}
		// 下一个
		property Struct^ Next { 
			Struct^ get() {
				tid_t id = get_next_struc_idx(ID);
				return FindStructByID(id);
			}
		}

		//***************************方法******************************************

		// 添加成员。实际比较复杂，这里仅处理常用的
		Member^ Add(String^ name, ea_t offset, flags_t flag, const typeinfo_t *mt, asize_t nbytes) {
			if(add_struc_member(ptr, IdaHelper::CastStringToChar(name), offset, flag, mt, nbytes) != 0) return nullptr;
			return GetMember(offset);
		}

		// 添加成员
		Member^ Add(String^ name, ea_t offset, flags_t flag, ea_t tid, asize_t nbytes, ea_t target, adiff_t tdelta, uchar reftype) {
			const typeinfo_t* ti = PrepareStrucMemberTypeinfo(flag, tid, target, tdelta, reftype);
			if(add_struc_member(ptr, IdaHelper::CastStringToChar(name), offset, flag, ti, nbytes) != 0) return nullptr;
			return GetMember(offset);
		}

		// 添加成员
		Member^ Add(String^ name, ea_t offset, flags_t flag, ea_t tid, asize_t nbytes) {
			return Add(name, offset, flag, tid, nbytes, Bytes::BadAddress, 0, 2);
		}

		// 添加成员
		Member^ Add(String^ name, ea_t offset, DataType dt, ea_t tid, asize_t nbytes) {
			return Add(name, offset, (flags_t)dt, tid, nbytes);
		}

		// 添加成员
		Member^ Add(String^ name, DataType dt, asize_t nbytes) {
			return Add(name, Bytes::BadAddress, (flags_t)dt, -1, nbytes);
		}

		// 删除成员
		bool Delete(ea_t offset) {
			return del_struc_member(ptr, offset);
		}

		// 删除成员
		int Delete(ea_t off1, ea_t off2) {
			return del_struc_members(ptr, off1, off2);
		}

		// 取得结构体成员
		Member^ GetMember(asize_t offset) {
			member_t* entity = get_member(ptr, offset);
			if (entity == NULL) return nullptr;

			return gcnew Member(ptr, entity);
		}

		// 取得结构体成员
		Member^ GetMemberByName(String^ name) {
			member_t* entity = get_member_by_name(ptr, IdaHelper::CastStringToChar(name));
			if (entity == NULL) return nullptr;

			return gcnew Member(ptr, entity);
		}

		// 设置成员名称
		bool SetMemberName(ea_t offset, String^ name) {
			return set_member_name(ptr, offset, IdaHelper::CastStringToChar(name));
		}

		//***************************静态成员******************************************

		// 根据序数找结构体
		static Struct^ FindStructByID(tid_t id) { 
			struc_t* entity = get_struc(id);
			if (entity == NULL) return nullptr;

			return gcnew Struct(entity);
		}

		// 根据名称找结构体
		static Struct^ FindStructByName(String^ name) { 
			tid_t id = GetStructID(name);
			if (id == Bytes::BadAddress) return nullptr;

			return FindStructByID(id);
		}

		// 创建结构体
		static Struct^ Create(String^ name, uval_t index, bool isUnion) {
			// 创建
			tid_t id = add_struc(index, IdaHelper::CastStringToChar(name), isUnion);

			struc_t* entity = get_struc(id);
			if (entity == NULL) return nullptr;

			return gcnew Struct(entity);
		}

		//***************************静态成员******************************************

		// 结构体数量
		static property size_t TotalCount { size_t get() { return get_struc_qty(); }}
		// 第一个
		static property Struct^ First { 
			Struct^ get() {
				tid_t id = get_first_struc_idx();
				return FindStructByID(id);
			}
		}
		// 最后一个
		static property Struct^ Last { 
			Struct^ get() {
				tid_t id = get_last_struc_idx();
				return FindStructByID(id);
			}
		}

		// 结构体序数
		//static uval_t GetFirstStructIndex() { return get_first_struc_idx(); }
		//static uval_t GetLastStructIndex() { return get_last_struc_idx(); }
		//static uval_t GetPrevStructIndex(uval_t idx) { return get_prev_struc_idx(idx); }
		//static uval_t GetNextStructIndex(uval_t idx) { return get_next_struc_idx(idx); }
		static uval_t GetStructIndex(tid_t id) { return get_struc_idx(id); }
		static tid_t GetStructByIndex(uval_t index) { return get_struc_by_idx(index); }

		// 根据名字找结构体的序数
		static tid_t GetStructID(String^ name) { return get_struc_id(IdaHelper::CastStringToChar(name)); }
	};
}