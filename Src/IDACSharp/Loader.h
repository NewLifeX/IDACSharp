//-----------------------------------------------------------------------------
//封装记录
//
//注：目前对loader.hpp的封装是由于控制调试器dbg.hpp的需要,其中涉及到消息的Hook
//    以消息Hook的卸载，还有对消息的回调处理
//    
//    没有封装成功，注释掉代码
//
//-----------------------------------------------------------------------------

//******************************************************************************
//对 loader.hpp 头文件进行封装  write by dreamzgj 
//******************************************************************************

#include <stdarg.h>
#include <loader.hpp>

int __stdcall Internal_CallBack(void *User_Data,int Notification_Code,va_list VaList)
{
	msg("printf notification code:%d\n",Notification_Code);

	

	return 0;
}


using namespace System;
using namespace System::Runtime::InteropServices;


namespace IDACSharp
{
#ifndef __LOADER_H__
#define __LOADER_H__


	public enum class Enum_Hook_Type_T
	{
		HT_IDP,         // Hook to the processor module.
		// The callback will receive all idp_notify events.
		// See file idp.hpp for the list of events.
		HT_UI,          // Hook to the user interface.
		// The callback will receive all ui_notification_t events.
		// See file kernwin.hpp for the list of events.
		HT_DBG,         // Hook to the debugger.
		// The callback will receive all dbg_notification_t events.
		// See file dbg.hpp for the list of events.
		HT_IDB,         // Hook to the database events.
		// These events are separated from the HT_IDP group
		// to speed up things (there are too many plugins and
		// modules hooking to the HT_IDP). Some essential events
		// are still generated in th HT_IDP group:
		// make_code, make_data, undefine, rename, add_func, del_func.
		// This list is not exhaustive.
		// A common trait of all events in this group: the kernel
		// does not expect any reaction to the event and does not
		// check the return code. For event names, see the idp_event_t
		// in idp.hpp
		HT_DEV,         // Internal debugger events.
		// Not stable and undocumented for the moment
		HT_LAST
	};

	public ref class Ida_Loader
	{
	private:
		[UnmanagedFunctionPointer(CallingConvention::StdCall)]
		delegate int InternalEventHandler(Object^ User_Data,int Notification_Code, va_list VaList);

		int __stdcall Internal_CallBack(ref class Object^ User_Data,int Notification_Code, va_list VaList)
		{
			msg("callback_func is called back\n");
			return 0;
		}
	public:
		 delegate  bool IdaEventHandler(Object^ User_Data,int Notification_Code, ...array<Object^>^ ArgsList);
		
		static bool Hook_To_Notification_Point(Enum_Hook_Type_T Hook_Type,IdaEventHandler^ Handler)
		{
			//InternalEventHandler^ h=gcnew InternalEventHandler();
			//IntPtr ip=Marshal::GetFunctionPointerForDelegate(h);
			//return hook_to_notification_point((hook_type_t)(int)Hook_Type,(hook_cb_t(__stdcall *))ip.ToPointer(),NULL);
			return true;
		}
		static bool Hook_To_Notification_Point(Enum_Hook_Type_T Hook_Type,IdaEventHandler^ Handler,Object^ User_Data){return false;}


		static int UnHook_From_Notification_Point(Enum_Hook_Type_T Hook_Type,IdaEventHandler^ Handler)
		{
			//return unhook_from_notification_point((hook_type_t)Hook_Type,Cb,NULL);
			//return true;
			return true;
		}
		static int UnHook_From_Notification_Point(Enum_Hook_Type_T Hook_Type,IdaEventHandler^ Handler,Object^ User_Data){return false;}

	};
	

#endif
}