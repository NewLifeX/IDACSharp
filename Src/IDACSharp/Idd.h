//-----------------------------------------------------------------------------
//封装记录
//
//注：目前仅封装了调试dbg.hpp所需要用到的结构体以及枚举等
//    
//
//-----------------------------------------------------------------------------

using namespace System::Runtime::InteropServices;

namespace IDACSharp
{
#ifndef __IDD_H__
#define __IDD_H__

//====================================================================
//
//                       Process and Threads
//


	public ref struct Struct_Process_Info_T
	{
		pid_t pid;
		String^ strName;
	};

	public ref struct Struct_Module_Info_T
	{
	public:
		String^ strName;    // full name of the module.
		ea_t m_base;            // module base address. if unknown pass BADADDR
		asize_t size;         // module size. if unknown pass 0
		ea_t rebase_to;       // if not BADADDR, then rebase the program to the specified address
	};

	public enum class Enum_Member_Regval_T
	{
		iValue=0,	//表示使用Struct_Regval_T结构中的iValue成员值
		fValue=1    //表示使用Struct_Regval_T结构中的fValue成员值
	};

	//[StructLayout(LayoutKind::Explicit)]  
	public ref struct Struct_Regval_T
	{
	public :
		//[FieldOffsetAttribute(0)]  
		uint64 iValue;        // 8:  integer value
		//[FieldOffsetAttribute(0)]  
		array<uint16>^ fValue;     // 12: floating point value in the internal representation (see ieee.h)
		
		//[FieldOffsetAttribute(13)]
		Enum_Member_Regval_T UseWhich ;//标志着使用哪个成员

		Struct_Regval_T()
		{
			iValue=~(uint64(0));
			fValue=gcnew array<uint16>(6);
		}
	};

	//断点类型
	public enum class Enum_BptType_T
	{
		Bpt_Exec=0,		//Execute instruction
		Bpt_Write=1,	//Write access
		Bpt_Rdwr=3,		//Read/Write access
		Bpt_Soft=4		//SoftWare breakpoint
	};

	public enum class Enum_Event_Id_T
	{
		NO_EVENT       = 0x00000000,	// Not an interesting event. This event can be
										// used if the debugger module needs to return
										// an event but there are no valid events.
		PROCESS_START  = 0x00000001,	// New process has been started.
		PROCESS_EXIT   = 0x00000002,	// Process has been stopped.
		THREAD_START   = 0x00000004,	// New thread has been started.
		THREAD_EXIT    = 0x00000008,	// Thread has been stopped.
		BREAKPOINT     = 0x00000010,	// Breakpoint has been reached. IDA will complain
										// about unknown breakpoints, they should be reported
										// as exceptions.
		STEP           = 0x00000020,	// One instruction has been executed. Spurious
										// events of this kind are silently ignored by IDA.
		EXCEPTION      = 0x00000040,	// Exception.
		LIBRARY_LOAD   = 0x00000080,	// New library has been loaded.
		LIBRARY_UNLOAD = 0x00000100,	// Library has been unloaded.
		INFORMATION    = 0x00000200,	// User-defined information.
										// This event can be used to return empty information
										// This will cause IDA to call get_debug_event()
										// immediately once more.
		SYSCALL        = 0x00000400,	// Syscall (not used yet).
		WINMESSAGE     = 0x00000800,	// Window message (not used yet).
		PROCESS_ATTACH = 0x00001000,	// Successfully attached to running process.
		PROCESS_DETACH = 0x00002000,	// Successfully detached from process.
		PROCESS_SUSPEND= 0x00004000,	// Process has been suspended..
										// This event can be used by the debugger module
										// to signal if the process spontaneously gets
										// suspended (not because of an exception,
										// breakpoint, or single step). IDA will silently
										// switch to the 'suspended process' mode without
										// displaying any messages.
	};

	public ref struct Struct_E_BreakPoint_T
	{
	public:
		ea_t Hea;		// Possible address referenced by hardware breakpoints
		ea_t Kea;		// Address of the triggered bpt from the kernel's point
						// of view (for some systems with special memory mappings,
						// the triggered ea might be different from event ea).
						// Use to BADADDR for flat memory model.
	};

	public ref struct Struct_E_Exception_T
	{
	public:
		int Code;			// Exception code
		bool Can_Cout;		// Execution of the process can continue after this exception?
		ea_t Ea;			// Possible address referenced by the exception
		String^ strInfo;	// Exception message
	};

	public ref struct Struct_Debug_Event_T
	{
	public:
		Struct_Debug_Event_T(void)
		{
			Eid=Enum_Event_Id_T::NO_EVENT;
		}
		// The following fields must be filled for all events:
		Enum_Event_Id_T Eid;			// Event code (used to decipher 'info' union)
		pid_t Pid;						// Process where the event occured
		thid_t Tid;						// Thread where the event occured
		ea_t Ea;						// Address where the event occured
		bool Handled;					// Is event handled by the debugger?
										// (from the system's point of view)
										// Meaningful for EXCEPTION events
		Struct_Module_Info_T Union_ModInfo;	// PROCESS_START, PROCESS_ATTACH, LIBRARY_LOAD
		int Union_Exit_Code;					// PROCESS_EXIT, THREAD_EXIT
		String^ Union_strInfo;				// LIBRARY_UNLOAD (unloaded library name)
										// INFORMATION (will be displayed in the
										//              messages window if not empty)
		Struct_E_BreakPoint_T Union_Bpt;		//BREAKPOINT
		Struct_E_Exception_T Union_Exc;		//EXCEPTION

		ea_t Bpt_Ea(void)
		{
			return (int)Eid == BREAKPOINT && Union_Bpt.Kea != BADADDR?Union_Bpt.Kea:Ea;
		}
	};
#endif
}