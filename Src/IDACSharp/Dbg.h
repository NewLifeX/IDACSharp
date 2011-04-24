//-----------------------------------------------------------------------------
//封装记录
//
//注：Q:(对Struct_RegVal_T这些寄存器操作，关于浮点数值的准确性有待测试
//    Q:(全局的idaman debugger_t ida_export_data *dbg还不确定如何进行封装使用
//    Q:(未封装函数
//      get_process_options
//      retrieve_exceptions
//      have_set_options
//      set_dbg_options
//    
//
//-----------------------------------------------------------------------------


//******************************************************************************
//对 dbg.hpp 头文件进行封装  write by dreamzgj 
//******************************************************************************
#include<dbg.hpp>
#include<idp.hpp>
#include<idd.hpp>
#include"Idd.h"
#include<kernwin.hpp>
#include"KernelWin.h"
#include<pro.h>

#include "IdaHelper.h"

using namespace System::Runtime::InteropServices;

namespace IDACSharp
{
#ifndef __DBG_H__
#define __DBG_H__

	public enum class Enum_Dbg_Notification_T
	{
		dbg_null = 0,
		dbg_process_start,   
		dbg_process_exit,   
		dbg_process_attach,
		dbg_process_detach,  
		dbg_thread_start,
		dbg_thread_exit,   
		dbg_library_load,   
		dbg_library_unload, 
		dbg_information,  
		dbg_exception,     
		dbg_suspend_process, 
		dbg_bpt,          
		dbg_trace,   
		dbg_request_error, 
		dbg_step_into,     
		dbg_step_over,   
		dbg_run_to,     
		dbg_step_until_ret, 
		dbg_last,        
	};
	public enum class Enum_ProcessState
	{
		DState_Susp_For_Event=-2,	// process is currently suspended to react to a debug event
									// but it will continue when the kernel gets back the control
		DState_Susp=-1,				// process is suspended and will not continue
		Dstate_Notask=0,			// no process is currently debugged
		DState_Run=1,				// process is running
		DState_Run_Wait_Attach=2,	// process is running, waiting for process properly attached
		DState_Run_Wait_End=3		// process is running, but the user asked to kill/detach the process
									//   remark: in this case, most events are ignored
	};
	public enum class Enum_DbgInv
	{
		DbgInv_Memory=0x0001,  // invalidate cached memory contents
		DbgInv_Memcfg=0x0002,  // invalidate cached process segmentation
		DbgInv_Regs=0x0004,  // invalidate cached register values
		DbgInv_All=(-1),    // invalidate everything
		DbgInv_None=0       // invalidate nothing
	};
	public enum class Enum_BptCk
	{
		BptCk_None=-1,	// breakpoint does not exist
		BptCk_No=0,		// breakpoint is disabled
		BptCk_Yes=1,	// breakpoint is enabled
		BptCk_Act=2		// breakpoint is active (written to the process)
	};
	public enum class Enum_BptFlags
	{
		Bpt_Brk=0x01, // the debugger stops on this breakpoint
		Bpt_Trace=0x02// the debugger adds trace information when this breakpoint is reached
	};
	//Step Trace 选项
	public enum class Enum_StTraceOption
	{
		St_Over_Debug_Seg=0x01,// step tracing will be disabled when IP is in a debugger segment
		St_Over_Lib_Func=0x02 // step tracing will be disabled when IP is in a library function
	};
	//Instructions Trace 选项
	public enum class Enum_ItTraceOption
	{
		It_Unknown=0x0,		//sdk 中没有此项，假设一个0的项
		It_Log_Same_Ip=0x01 // instruction tracing will log instructions whose IP doesn't change
	};
	//Function Trace 选项
	public enum class Enum_FtTraceOption
	{
		Ft_Unknown=0x0,	//sdk中没有此项，假设一个0的项
		Ft_Log_Ret=0x01 // function tracing will log returning instructions
	};
	//Trace Event Types
	public enum class Enum_Tev_Type_T
	{
		Tev_None = 0, // no event
		Tev_Insn,     // instruction trace event
		Tev_Call,     // function call trace event
		Tev_Ret,      // function return trace event
		Tev_Bpt,      // write, read/write, execution trace event
	};

	//debugger event codes:
	public enum class Enum_Dbg_Event_Code_T
	{
		Dec_Notask  = -2,  // process does not exist
		Dec_Error   = -1,  // error
		Dec_Timeout = 0,   // timeout
	};
	// wfne flag is combination of the following:
	public enum class Enum_WfneFlag
	{
		Wfne_Any=0x0001,	// return the first event (even if it doesn't suspend the process)
		Wfne_Susp=0x0002,	// wait until the process gets suspended
		Wfne_Silent=0x0004,	// 1: be slient, 0:display modal boxes if necessary
		Wfne_Cont=0x0008,	// continue from the suspended state
		Wfne_NoWait=0x0010,	// do not wait for any event, immediately return DEC_TIMEOUT
							// (to be used with WFNE_CONT)
		Wfne_Usec=0x0020	// timeout is specified in microseconds (minimum non-zero timeout is 40000us)
	};
	//调试器选项 debugger_options
	public enum class Enum_DbgOptions
	{
		Dopt_Segm_Msgs=0x00000001, // log debugger segments modifications
		Dopt_Start_Bpt=0x00000002, // break on process start
		Dopt_Thread_Msgs=0x00000004, // log thread starts/exits
		Dopt_Thread_Bpt=0x00000008, // break on thread start/exit
		Dopt_Bpt_Msgs=0x00000010, // log breakpoints
		//#define DOPT_BINS_BPT     0x00000020 // break on breakpoint instruction
		Dopt_Lib_Msgs=0x00000040, // log library loads/unloads
		Dopt_Lib_Bpt=0x00000080, // break on library load/unlad
		Dopt_Info_Msgs=0x00000100, // log debugging info events
		Dopt_Info_Bpt=0x00000200, // break on debugging information
		Dopt_Real_Memory=0x00000400, // do not hide breakpoint instructions
		Dopt_Redo_Stack=0x00000800, // reconstruct the stack
		Dopt_Entry_Bpt=0x00001000, // break on program entry point
		Dopt_ExcDlg=0x00006000, // exception dialogs:
		ExcDlg_Never=0x00000000, // never display exception dialogs
		ExcDlg_Unknown=0x00002000, // display for unknown exceptions
		ExcDlg_Always=0x00006000, // always display
		Dopt_Load_Dinfo=0x00008000 // automatically load debug files (pdb)
	};
	//BreakPoints 断点的结构体
	public ref struct Struct_Bpt_T
	{
	public: 
		ea_t Ea;
		asize_t Size;
		Enum_BptType_T Type;
		int Pass_Count;
		Enum_BptFlags Flags;
		//[MarshalAs(UnmanagedType::ByValTStr, SizeConst = MAXSTR)]
		String ^Condition;
	};


	// Common information for all trace events:
	public ref struct Struct_Tev_Info_T
	{
		Enum_Tev_Type_T  type; // trace event type
		thid_t tid;  // thread where the event was recorded
		ea_t        ea;   // address where the event occured
	};
	//-----------------------------------------------------------------------------
	//dbg.hpp 封装为 Ida_Dbg 类
	//-----------------------------------------------------------------------------
	public ref class Ida_Dbg
	{
		//-----------------------------------------------------------------------------
		//My_xxxx 函数为自己写的，仅供封装时，内部使用
		//-----------------------------------------------------------------------------
	private:
		//判断传进来的寄存器是否为 浮点寄存器【注：这里的判断暂时还不准确】
		static bool My_IsFPU_Reg(String^ RegName)
		{
			if(RegName=="ST0"|RegName=="st0"|
				RegName=="ST1"|RegName=="st1"|
				RegName=="ST2"|RegName=="st2"|
				RegName=="ST3"|RegName=="st3"|
				RegName=="ST4"|RegName=="st4"|
				RegName=="ST5"|RegName=="st5"|
				RegName=="ST6"|RegName=="st6"|
				RegName=="ST7"|RegName=="st7"|
				RegName=="CTRL"|RegName=="ctrl"|
				RegName=="STAT"|RegName=="stat"|
				RegName=="TAGS"|RegName=="tags")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	public:
		//--------------------------------------------------------------------
		//               D E B U G G E R    F U N C T I O N S
		//--------------------------------------------------------------------

		// Execute requests until all requests are processed or an asynchronous function is called.
		static bool Run_Requests(void)
		{
			return run_requests();
		}

		// Get the current running request.
		static Enum_Ui_Notification_T Get_Running_Request(void)
		{
			return (Enum_Ui_Notification_T)get_running_request();
		}
		static bool Is_Request_Running()
		{
			return get_running_request()!=ui_null;
		}
		
		// Get the notification associated (if any) with the current running request.
		static Enum_Dbg_Notification_T Get_Running_Notification(void)
		{ 
			return (Enum_Dbg_Notification_T)get_running_notification();
		}
		
		// Clear the queue of waiting requests.
		static void Clear_Request_Queue(void)
		{
			return clear_requests_queue();
		}

		//--------------------------------------------------------------------
		//                P R O C E S S   C O M M A N D S
		//--------------------------------------------------------------------

		// Return the state of the currently debugged process.
		static Enum_ProcessState Get_Process_State(void)
		{
			return (Enum_ProcessState)get_process_state();
		}
		
		// Set new state for the debugged process.
		static Enum_DbgInv Set_Process_State(int NewState,thid_t *P_Thid,int DbgInv)
		{ 
			return (Enum_DbgInv)set_process_state(NewState,P_Thid,DbgInv);
		}
	
		// Start a process in the debugger
		static int Start_Process(String^ strPath,String^ strArgs,String^ strSDir)
		{
			return start_process(IDACSharp::IdaHelper::CastStringToChar(strPath),
				IDACSharp::IdaHelper::CastStringToChar(strArgs),
				IDACSharp::IdaHelper::CastStringToChar(strSDir)
				);
		}
		static int Request_Start_Process(String^ strPath,String^ strArgs,String^ strSDir)
		{
			return request_start_process(IDACSharp::IdaHelper::CastStringToChar(strPath),
				IDACSharp::IdaHelper::CastStringToChar(strArgs),
				IDACSharp::IdaHelper::CastStringToChar(strSDir)
				);
		}

		// Suspend the process in the debugger.
		static bool Suspend_Process(void)
		{
			return suspend_process();
		}
		static bool Request_Suspend_Process(void)
		{
			return request_suspend_process();
		}

		// Continue the execution of the process in the debugger.
		static bool Continue_Process(void)
		{
			return continue_process();
		}
		static bool Request_Continue_Process(void)
		{
			return request_continue_process();
		}

		// Terminate the debugging of the current process.
		static bool Exit_Process(void)
		{
			return exit_process();
		}
		static bool Request_Exit_Process(void)
		{
			return request_exit_process();
		}

		// Take a snapshot of running processes and return their number.
		static int Get_Process_Qty(void)
		{
			return get_process_qty();
		}

		// Get information about a running process
		static pid_t Get_Process_Info(int n,Struct_Process_Info_T^ Process_Info)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(process_info_t));
			process_info_t *Ida_process=(process_info_t*)(ip.ToPointer());
			pid_t Pid=get_process_info(n,Ida_process);
			try
			{
				Process_Info->strName=IDACSharp::IdaHelper::CastCharToString(Ida_process->name);
				Process_Info->pid=Ida_process->pid;
				Marshal::FreeHGlobal(ip);
				return Pid;
			}
			catch (...)
			{
				Marshal::FreeHGlobal(ip);
				return NO_PROCESS;
			}
		}
		static pid_t Getn_Process(int n)
		{
			return getn_process(n);
		}

		// Attach the debugger to a running process
		static int Attach_Process(pid_t pid,int event_id)
		{
			return attach_process(pid,event_id);
		}
		static int Request_Attach_Process(pid_t pid,int event_id)
		{
			return request_attach_process(pid,event_id);
		}

		// Detach the debugger from the debugged process.
		static bool Detach_Process(void)
		{
			return detach_process();
		}
		static bool Request_Detach_Process(void)
		{
			return request_detach_process();
		}

		//--------------------------------------------------------------------
		//                         T H R E A D S
		//--------------------------------------------------------------------

		// Get number of threads.
		static int Get_Thread_Qty(void)
		{
			return get_thread_qty();
		}

		// Get the ID of a thread
		static thid_t Getn_Thread(int n)
		{
			return getn_thread(n);
		}

		// Get current thread ID
		static thid_t Get_Current_Thread(void)
		{
			return get_current_thread();
		}

		// Select the given thread as the current debugged thread.
		static bool Select_Thread(thid_t tid)
		{
			return select_thread(tid);
		}
		static bool Request_Select_Thread(thid_t tid)
		{
			return request_select_thread(tid);
		}

		// Suspend thread
		static int Suspend_Thread(thid_t tid)
		{
			return suspend_thread(tid);
		}
		static int Request_Suspend_Thread(thid_t tid)
		{
			return request_suspend_thread(tid);
		}

		// Resume thread
		static int Resume_Thread(thid_t tid)
		{
			return resume_thread(tid);
		}
		static int Request_Resume_Thread(thid_t tid)
		{
			return request_resume_thread(tid);
		}
		
		//--------------------------------------------------------------------
		//                         M O D U L E S
		//--------------------------------------------------------------------
		
		// Functions to enumerate modules loaded into the process
		static bool Get_First_Module(Struct_Module_Info_T^ ModInfo)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(module_info_t));
			module_info_t *Ida_modinfo=(module_info_t*)(ip.ToPointer());
			if(get_first_module(Ida_modinfo))
			{
				ModInfo->m_base=Ida_modinfo->base;
				ModInfo->rebase_to=Ida_modinfo->rebase_to;
				ModInfo->size=Ida_modinfo->size;
				ModInfo->strName=IDACSharp::IdaHelper::CastCharToString(Ida_modinfo->name);
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Get_Next_Module(Struct_Module_Info_T^ ModInfo)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(module_info_t));
			module_info_t *Ida_modinfo=(module_info_t*)(ip.ToPointer());
			if(get_next_module(Ida_modinfo))
			{
				ModInfo->m_base=Ida_modinfo->base;
				ModInfo->rebase_to=Ida_modinfo->rebase_to;
				ModInfo->size=Ida_modinfo->size;
				ModInfo->strName=IDACSharp::IdaHelper::CastCharToString(Ida_modinfo->name);
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		//--------------------------------------------------------------------
		//    E X E C U T I O N   F L O W   C O N T R O L   C O M M A N D S
		//--------------------------------------------------------------------
		// Use the following functions to run instructions in the debugged process.

		// Execute one instruction in the current thread.
		static bool Step_Into(void)
		{
			return step_into();
		}
		static bool Request_Step_Into(void)
		{
			return request_step_into();
		}

		// Execute one instruction in the current thread,but without entering into functions
		static bool Step_Over(void)
		{
			return step_over();
		}
		static bool Request_Step_Over(void)
		{
			return request_step_over();
		}

		// Execute the process until the given address is reached.
		// If no process is active, a new process is started.
		static bool Run_To(ea_t Ea)
		{
			return run_to(Ea);
		}
		static bool Request_Run_To(ea_t Ea)
		{
			return request_run_to(Ea);
		}

		// Execute instructions in the current thread until
		// a function return instruction is reached.
		static bool Step_Until_Ret(void)
		{
			return step_until_ret();
		}
		static bool Request_Step_Until_Ret(void)
		{
			return request_step_over();
		}

		//--------------------------------------------------------------------
		//                       R E G I S T E R S
		//--------------------------------------------------------------------
		// The debugger structure defines a set of hardware registers in dbg->registers.
		// IDA also recognizes register names for each defined bit in bit registers.
		// You can use all these names to set or get a register value.
		//
		// For example, with the x86 Userland Win32 debugger you can use
		// register names like:
		//  - "EAX", ... "EBP", "ESP", "EFL": for classical integer registers
		//  - "CS", "DS", ...               : for segment registers
		//  - "ST0", "ST1", ...             : for FPU registers
		//  - "CF", "PF", "AF", "ZF", ...   : for special bit values

		// Read a register value from the current thread.
		static bool Get_Reg_Val(String^ RegName,Struct_Regval_T^ RegVal)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(regval_t));
			regval_t *Ida_regval_t=(regval_t*)(ip.ToPointer());
			if(get_reg_val(IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_regval_t))
			{
				//浮点寄存器
				if(My_IsFPU_Reg(RegName))
				{
					RegVal->UseWhich=Enum_Member_Regval_T::fValue;
					if(IDACSharp::IdaHelper::PaddingToManagedArray(Ida_regval_t->fval,6,RegVal->fValue)!=1)
					{
						Marshal::FreeHGlobal(ip);
						return false;
					}
				}
				else
				{
					RegVal->UseWhich=Enum_Member_Regval_T::iValue;
					RegVal->iValue=Ida_regval_t->ival;
				}
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Get_Reg_Val(String^ RegName,uint64^ iValue)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(uint64));
			uint64 *Ida_ival=(uint64*)(ip.ToPointer());
			if(get_reg_val(IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_ival))
			{
				iValue=Convert::ToUInt64(*Ida_ival);
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		// Write a register value to the current thread.
		static bool Set_Reg_Val(String^ RegName,Struct_Regval_T^ RegVal)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(regval_t));
			regval_t *Ida_regval_t=(regval_t*)(ip.ToPointer());
	
			if(My_IsFPU_Reg(RegName))
			{
				if(IDACSharp::IdaHelper::PadingToUnmanagedArray(RegVal->fValue,Ida_regval_t->fval,6)!=1)
				{
					Marshal::FreeHGlobal(ip);
					return false;
				}
			}
			else
			{
				Ida_regval_t->ival=RegVal->iValue;
			}

			if(set_reg_val(IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_regval_t))
			{
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Request_Set_Reg_Val(String^ RegName,Struct_Regval_T^ RegVal)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(regval_t));
			regval_t *Ida_regval_t=(regval_t*)(ip.ToPointer());

			if(My_IsFPU_Reg(RegName))
			{
				if(IDACSharp::IdaHelper::PadingToUnmanagedArray(RegVal->fValue,Ida_regval_t->fval,6)!=1)
				{
					Marshal::FreeHGlobal(ip);
					return false;
				}
			}
			else
			{
				Ida_regval_t->ival=RegVal->iValue;
			}
			if(request_set_reg_val(IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_regval_t))
			{
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Set_Reg_Val(String^ RegName,uint64^ iValue)
		{
			uint64 Ida_ival=(uint64)iValue;
			return set_reg_val(IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_ival);
		}
		// Does a register contain an integer value?
		static bool Is_Reg_Integer(String^ RegName)
		{
			return is_reg_integer(IDACSharp::IdaHelper::CastStringToChar(RegName));
		}

		//--------------------------------------------------------------------
		//                     B R E A K P O I N T S
		//--------------------------------------------------------------------

		//Get number of breakpoints
		static int Get_Bpt_Qty(void){return get_bpt_qty();}

		//Get the characteristics of a breakpoint
		static bool Getn_bpt(int n,Struct_Bpt_T^ Bpt)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(bpt_t));
			bpt_t *Ida_bpt=(bpt_t*)(ip.ToPointer());
			if(getn_bpt(n,Ida_bpt))
			{
				Bpt->Condition=IDACSharp::IdaHelper::CastCharToString(Ida_bpt->condition);
				Bpt->Ea=Ida_bpt->ea;
				Bpt->Flags=(Enum_BptFlags)Ida_bpt->flags;
				Bpt->Pass_Count=Ida_bpt->pass_count;
				Bpt->Size=Ida_bpt->size;
				Bpt->Type=(Enum_BptType_T)Ida_bpt->type;
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		//Get the characteristics of a breakpoint
		static bool Get_Bpt(ea_t Ea,Struct_Bpt_T^ Bpt)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(bpt_t));
			bpt_t *Ida_bpt=(bpt_t*)(ip.ToPointer());
			if(get_bpt(Ea,Ida_bpt))
			{
				Bpt->Condition=IDACSharp::IdaHelper::CastCharToString(Ida_bpt->condition);
				Bpt->Ea=Ida_bpt->ea;
				Bpt->Flags=(Enum_BptFlags)Ida_bpt->flags;
				Bpt->Pass_Count=Ida_bpt->pass_count;
				Bpt->Size=Ida_bpt->size;
				Bpt->Type=(Enum_BptType_T)Ida_bpt->type;
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Exist_Bpt(ea_t Ea)
		{
			return exist_bpt(Ea);
		}

		// Add a new breakpoint in the debugged process
		static bool Add_Bpt(ea_t Ea,asize_t Size,Enum_BptType_T Type)
		{
			return add_bpt(Ea,Size,(bpttype_t)Type);
		}
		static bool Request_Add_Bpt(ea_t Ea,asize_t Size,Enum_BptType_T Type)
		{
			return request_add_bpt(Ea,Size,(bpttype_t)Type);
		}

		// Delete an existing breakpoint in the debugged process
		static bool Del_Bpt(ea_t Ea)
		{
			return del_bpt(Ea);
		}
		static bool Request_Del_Bpt(ea_t Ea)
		{
			return request_del_bpt(Ea);
		}

		// Update modifiable characteristics of an existing breakpoint.
		static bool Update_Bpt(Struct_Bpt_T^ Bpt)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(bpt_t));
			bpt_t *Ida_bpt=(bpt_t*)(ip.ToPointer());

			const char* str=IDACSharp::IdaHelper::CastStringToChar(Bpt->Condition);
			if(NULL!=str)
			{
				qstrncpy(Ida_bpt->condition,str,qstrlen(str));
			}	
			Ida_bpt->ea=Bpt->Ea;
			Ida_bpt->flags=(int)Bpt->Flags;
			Ida_bpt->pass_count=Bpt->Pass_Count;
			Ida_bpt->size=Bpt->Size;
			Ida_bpt->type=(bpttype_t)Bpt->Type;
   
			if(update_bpt(Ida_bpt))
			{
				Bpt->Condition=IDACSharp::IdaHelper::CastCharToString(Ida_bpt->condition);
				Bpt->Ea=Ida_bpt->ea;
				Bpt->Flags=(Enum_BptFlags)Ida_bpt->flags;
				Bpt->Pass_Count=Ida_bpt->pass_count;
				Bpt->Size=Ida_bpt->size;
				Bpt->Type=(Enum_BptType_T)Ida_bpt->type;
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		// Enable or disable an existing breakpoint.
		static bool Enable_Bpt(ea_t Ea,bool Enable)
		{
			return enable_bpt(Enable);
		}
		static bool Disable_Bpt(ea_t Ea)
		{
			return disable_bpt(Ea);
		}
		static bool Request_Enable_Bpt(ea_t Ea,bool Enable)
		{
			return request_enable_bpt(Ea,Enable);
		}
		static bool Request_Disable_Bpt(ea_t Ea)
		{
			return request_disable_bpt(Ea);
		}
		// Check the breakpoint at the specified address
		// Return value: one of BPTCK_...
		static Enum_BptCk Check_Bpt(ea_t Ea)
		{
			return (Enum_BptCk)check_bpt(Ea);
		}
		

		//--------------------------------------------------------------------
		//                    T R A C I N G   B U F F E R
		//--------------------------------------------------------------------

		// Specify the new size of the circular buffer
		static bool Set_Trace_Size(int Size)
		{
			return set_trace_size(Size);
		}

		// Clear all events in the trace buffer
		static void Clear_Trace(void)
		{
			clear_trace();
		}
		static void Request_Clear_Trace(void)
		{
			request_clear_trace();
		}

		//--------------------------------------------------------------------
		//                        S T E P    T R A C I N G
		//--------------------------------------------------------------------

		// Get current state of step tracing.
		static bool Is_Step_Trace_Enabled(void)
		{
			return is_step_trace_enabled();
		}

		// Enable or disable the step tracing
		static bool Enable_Step_Trace(int Enable)
		{
			return enable_step_trace(Enable);
		}
		static bool Disable_Step_Trace(void)
		{
			return disable_step_trace();
		}
		static bool Request_Enable_Step_Trace(int Enable)
		{
			return request_enable_step_trace(Enable);
		}
		static bool Request_Disable_Step_Trace(void)
		{
			return request_disable_step_trace();
		}

		// Get current step tracing options.
		// Type:         Synchronous function
		static Enum_StTraceOption Get_Step_Trace_Options(void)
		{
			return (Enum_StTraceOption)get_step_trace_options();
		}

		// Modify step tracing options.
		static void Set_Step_Trace_Options(Enum_StTraceOption Option)
		{
			set_step_trace_options((int)Option);
		}
		static void Request_Set_Step_Trace_Options(Enum_StTraceOption Option)
		{
			request_set_step_trace_options((int)Option);
		}

		//--------------------------------------------------------------------
		//               I N S T R U C T I O N S   T R A C I N G
		//--------------------------------------------------------------------

		// Get current state of instructions tracing.
		static bool Is_Insn_Trace_Enabled(void)
		{
			return is_insn_trace_enabled();
		}
		
		// Enable or disable the instructions tracing
		static bool Enable_Insn_Trace(int Enable)
		{
			return enable_insn_trace(Enable);
		}
		static bool Disable_Insn_Trace(void)
		{
			return disable_insn_trace();
		}
		static bool Request_Enable_Insn_Trace(int Enable)
		{
			return request_enable_insn_trace(Enable);
		}
		static bool Request_Disable_Insn_Trace(void)
		{
			return request_disable_insn_trace();
		}

		// Get current instruction tracing options.
		static Enum_ItTraceOption Get_Insn_Trace_Options(void)
		{
			return (Enum_ItTraceOption)get_insn_trace_options();
		}

		// Modify instruction tracing options.
		static void Set_Insn_Trace_Options(Enum_ItTraceOption Option)
		{
			set_insn_trace_options((int)Option);
		}
		static void Request_Set_Insn_Trace_Options(Enum_ItTraceOption Option)
		{
			request_set_insn_trace_options((int)Option);
		}

		//--------------------------------------------------------------------
		//                 F U N C T I O N S   T R A C I N G
		//--------------------------------------------------------------------

		// Get current state of functions tracing.
		static bool Is_Func_Trace_Enabled(void)
		{
			return is_func_trace_enabled();
		}

		// Enable or disable the functions tracing
		static bool Enable_Func_Trace(int Enable)
		{
			return enable_func_trace(Enable);
		}
		static bool Disable_Func_Trace(void)
		{
			return disable_func_trace();
		}
		static bool Request_Enable_Func_Trace(int Enable)
		{
			return request_enable_func_trace(Enable);
		}
		static bool Request_Disable_Func_Trace(void)
		{
			return request_disable_func_trace();
		}

		// Get current function tracing options.
		static Enum_FtTraceOption Get_Func_Trace_Options(void)
		{
			return (Enum_FtTraceOption)get_func_trace_options();
		}

		// Modify function tracing options.
		static void Set_Func_Trace_Options(Enum_FtTraceOption Option)
		{
			set_func_trace_options((int)Option);
		}
		static void Request_Set_Func_Trace_Options(Enum_FtTraceOption Option)
		{
			request_set_func_trace_options((int)Option);
		}

		//--------------------------------------------------------------------
		//                   T R A C I N G   E V E N T S
		//--------------------------------------------------------------------

		// Get number of trace events available in trace buffer.
		static int Get_Tev_Qty(void)
		{
			return get_tev_qty();
		}

		// Get main information about a trace event.
		static bool Get_Tev_Info(int n,Struct_Tev_Info_T^ Tev_Info)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(tev_info_t));
			tev_info_t *Ida_tev_info=(tev_info_t*)(ip.ToPointer());
			if(get_tev_info(n,Ida_tev_info))
			{
				Tev_Info->ea=Ida_tev_info->ea;
				Tev_Info->tid=Ida_tev_info->tid;
				Tev_Info->type=(Enum_Tev_Type_T)Ida_tev_info->type;
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		// Read a register value from an instruction trace event.
		static bool Get_Insn_Tev_Reg_Val(int n,String^ RegName,	Struct_Regval_T^ RegVal)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(regval_t));
			regval_t *Ida_regval=(regval_t*)(ip.ToPointer());
			if(get_insn_tev_reg_val(n,IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_regval))
			{
				if(My_IsFPU_Reg(RegName))
				{
					RegVal->UseWhich=Enum_Member_Regval_T::fValue;
					if(IDACSharp::IdaHelper::PaddingToManagedArray(Ida_regval->fval,6,RegVal->fValue)!=1)
					{
						Marshal::FreeHGlobal(ip);
						return false;
					}
				}
				else
				{
					RegVal->UseWhich=Enum_Member_Regval_T::iValue;
					RegVal->iValue=Ida_regval->ival;
				}
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Get_Insn_Tev_Reg_Val(int n,String^ RegName,uint64^ iValue)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(uint64));
			uint64 *Ida_ival=(uint64*)(ip.ToPointer());
			if(get_insn_tev_reg_val(n,IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_ival))
			{
				iValue=Convert::ToUInt64(*Ida_ival);
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		// Read the resulting register value from an instruction trace event.
		static bool Get_Insn_Tev_Reg_Result(int n,String^ RegName,Struct_Regval_T^ RegVal)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(regval_t));
			regval_t *Ida_regval=(regval_t*)(ip.ToPointer());
			if(get_insn_tev_reg_result(n,IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_regval))
			{
				if(My_IsFPU_Reg(RegName))
				{
					RegVal->UseWhich=Enum_Member_Regval_T::fValue;
					if(IDACSharp::IdaHelper::PaddingToManagedArray(Ida_regval->fval,6,RegVal->fValue)!=1)
					{
						Marshal::FreeHGlobal(ip);
						return false;
					}
				}
				else
				{
					RegVal->UseWhich=Enum_Member_Regval_T::iValue;
					RegVal->iValue=Ida_regval->ival;
				}
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}
		static bool Get_Insn_Tev_Reg_Result(int n,String^ RegName,uint64^ iValue)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(uint64));
			uint64 *Ida_ival=(uint64*)(ip.ToPointer());
			if(get_insn_tev_reg_result(n,IDACSharp::IdaHelper::CastStringToChar(RegName),Ida_ival))
			{
				iValue=Convert::ToUInt64(*Ida_ival);
				Marshal::FreeHGlobal(ip);
				return true;
			}
			else
			{
				Marshal::FreeHGlobal(ip);
				return false;
			}
		}

		// Return the called function from a function call trace event.
		static ea_t Get_Call_Tev_Callee(int n)
		{
			return get_call_tev_callee(n);
		}

		// Return the return address from a function return trace event.
		static ea_t Get_Ret_Tev_Return(int n)
		{
			return get_ret_tev_return(n);
		}

		// Return the address associated to a read, read/write or execution trace event.
		static ea_t Get_Bpt_Tev_Ea(int n)
		{
			return get_bpt_tev_ea(n);
		}

		//---------------------------------------------------------------------------
		//      High level functions (usable from scripts)
		//--------------------------------------------------------------------
		
		//If the retured value > 0, then it is the event id (see idd.hpp, event_id_t)
		//如果返回值小于0,则返回为Enum_Dbg_Event_Code_T类型
		//如果返回值大于0，则返回的为Event ID 即Enum_Event_Id_T类型值
		static int Wait_For_Next_Event(Enum_WfneFlag wfne,int TimeOut_In_Secs)
		{
			return (int)wait_for_next_event((int)wfne,TimeOut_In_Secs);
		}

		
		// Get the current debugger event
		static void Get_Debug_Event(Struct_Debug_Event_T^ DbgEvent)
		{
			IntPtr ip=Marshal::AllocHGlobal(sizeof(debug_event_t));
			const debug_event_t *Ida_dbg_event=(debug_event_t*)(ip.ToPointer());
			Ida_dbg_event=get_debug_event();

			DbgEvent->Ea=Ida_dbg_event->ea;
			DbgEvent->Eid=(Enum_Event_Id_T)Ida_dbg_event->eid;			
			DbgEvent->Handled=Ida_dbg_event->handled;
			DbgEvent->Pid=Ida_dbg_event->pid;
			DbgEvent->Tid=Ida_dbg_event->tid;

			DbgEvent->Union_Bpt.Hea=Ida_dbg_event->bpt.hea;
			DbgEvent->Union_Bpt.Kea=Ida_dbg_event->bpt.kea;

			DbgEvent->Union_Exc.Can_Cout=Ida_dbg_event->exc.can_cont;
			DbgEvent->Union_Exc.Code=Ida_dbg_event->exc.code;
			DbgEvent->Union_Exc.Ea=Ida_dbg_event->exc.ea;
			DbgEvent->Union_Exc.strInfo=IDACSharp::IdaHelper::CastCharToString(Ida_dbg_event->exc.info);

			DbgEvent->Union_Exit_Code=Ida_dbg_event->exit_code;

			DbgEvent->Union_ModInfo.m_base=Ida_dbg_event->modinfo.base;
			DbgEvent->Union_ModInfo.rebase_to=Ida_dbg_event->modinfo.rebase_to;
			DbgEvent->Union_ModInfo.size=Ida_dbg_event->modinfo.size;
			DbgEvent->Union_ModInfo.strName=IDACSharp::IdaHelper::CastCharToString(Ida_dbg_event->modinfo.name);

			DbgEvent->Union_strInfo=IDACSharp::IdaHelper::CastCharToString(Ida_dbg_event->info);

			Marshal::FreeHGlobal(ip);
		}

		// Set debugger options
		// Replaces debugger options with the specifiction combination of DOPT_..
		// Returns the old debugger options
		static uint Set_Debugger_Options(Enum_DbgOptions opts)
		{
			return set_debugger_options((uint)opts);
		}

		// Set remote debugging options
		// Should be used before starting the debugger. If host is empty,
		// then IDA will use local debugger.
		static void Set_Remote_Debugger(String^ strHost,String^ strPass,int Port)
		{
			set_remote_debugger(IDACSharp::IdaHelper::CastStringToChar(strHost),
				IDACSharp::IdaHelper::CastStringToChar(strPass),
				Port
				);
		}

		// Get process options.
		// Any of the arguments may be NULL
		//static void Get_Process_Options..............

		// Set process options.
		// Any of the arguments may be NULL, which means 'do not modify'
		static void Set_Process_Options(String^ strPath,
			String^ strArgs,String^ strSdir,String^ strHost,
			String^ strPass,int Port)
		{
			set_process_options(IDACSharp::IdaHelper::CastStringToChar(strPath),
				IDACSharp::IdaHelper::CastStringToChar(strArgs),
				IDACSharp::IdaHelper::CastStringToChar(strSdir),
				IDACSharp::IdaHelper::CastStringToChar(strHost),
				IDACSharp::IdaHelper::CastStringToChar(strPass),
				Port
				);
		}

		//store exception information
		static bool Store_Exceptions(void)
		{
			return store_exceptions();
		}

		// Convenience function: define new exception code
		//      code - exception code (can not be 0)
		//      name - exception name (can not be empty or NULL)
		//      desc - exception description (maybe NULL)
		//      flags - combination of EXC_... flags
		// returns: failure message or NULL
		// you must call store_exceptions() if this function succeeds
		static String^ Define_Exception(uint Code,String^ Name,String^ Desc,int Flags)
		{
			return IDACSharp::IdaHelper::CastCharToString(
				define_exception(Code,
					IDACSharp::IdaHelper::CastStringToChar(Name),
					IDACSharp::IdaHelper::CastStringToChar(Desc),
					Flags
					)
				);
		}

		// Convenience function to set debugger specific options. It checks if the debugger
		// is present and the function is present and calls it.
		//static String^ Set_Dbg_Options(String^ KeyWord,int Value_Type,Object^ objValue)
	};
#endif
}