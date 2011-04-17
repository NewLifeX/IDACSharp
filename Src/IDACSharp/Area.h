
#pragma once

using namespace System;

// The header file of the LIB to be linked
#include <ida.hpp>
#include <idp.hpp>


namespace IDACSharp {

	//区域
	public ref class Area
	{
	private:
		//area_t* _area;
		ulong _Start;
		ulong _End;

		////内部变量
		//virtual property area_t* area
		//{
		//	area_t* get()
		//	{
		//		return _area;
		//	}
		//	void set(area_t* value)
		//	{
		//		_area = value;
		//	}
		//}

	public:
		Area() {}

		//// Allocate the native object on the C++ Heap via a constructor
		//Area()
		//{
		//	area = new area_t();
		//}

		Area(ulong ea1, ulong ea2)
		{
			//area = new area_t(ea1, ea2);
			Start = ea1;
			End = ea2;
		}

		//// Deallocate the native object on a destructor
		//~Area() {
		//	delete area;
		//}

		//开始
		virtual property ulong Start {
			ulong get() { return _Start; }
			void set(ulong value) { _Start = value; }
		}

		//结束
		virtual property ulong End {
			ulong get() { return _End; }
			void set(ulong value) { _End; }
		}

		virtual bool Equals(Object^ area) override{
			//必须通过Object::ReferenceEquals来判空，而不能使用==，因为下面重载==的时候调用了本函数，形成循环调用
			if (Object::ReferenceEquals(area, nullptr)) return false;
			return Start==((Area^)area)->Start && End==((Area^)area)->End;
		}

		virtual int GetHashCode() override{
			return Object::GetHashCode();
		}

		static bool operator==(Area^ area1, Area^ area2) {
			if (Object::ReferenceEquals(area1, nullptr)) {
				return Object::ReferenceEquals(area2, nullptr);
			}else{
				if (Object::ReferenceEquals(area2, nullptr)) return false;
				return area1->Equals(area2);
			}
		}

		static bool operator!=(Area^ area1, Area^ area2) {
			return !(area1 == area2);
		}

		static bool operator> (Area^ area1, Area^ area2) {
			return area1->Start > area2->Start;
		}

		static bool operator< (Area^ area1, Area^ area2) {
			return area1->Start < area2->Start;
		}

		//int Compare(Area r) { return Start > r.Start ? 1 : Start < r.Start ? -1 : 0; }
		//bool operator ==(Area r) { return Compare(r) == 0; }
		//bool operator !=(Area r) { return Compare(r) != 0; }
		//bool operator > (Area r) { return Compare(r) >  0; }
		//bool operator < (Area r) { return Compare(r) <  0; }

		bool Contains(ulong ea) { return Start <= ea && End > ea; }
		bool Contains(Area^ area) { return area->Start >= Start && area->End <= End; }
		void Clear() { Start = 0; End = 0; }
		property bool Empty {
			bool get() { return Start >= End; }
		}
		property ulong Size {
			ulong get() { return End - Start; }
		}

		// 交集
		void Intersect(Area^ area)
		{
			if ( Start < area->Start ) Start = area->Start;
			if ( End   > area->End   ) End   = area->End;
			if ( End   < Start   ) End   = Start;
		}
		void Extend(ulong ea)
		{
			if ( Start > ea ) Start = ea;
			if ( End < ea ) End = ea;
		}
	//protected:

	//	// Deallocate the native object on the finalizer just in case no 
	//	// destructor is called
	//	!Area() {
	//		delete area;
	//	}
	};
}