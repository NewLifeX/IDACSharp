
#pragma once

#include <ida.hpp>
#include <idp.hpp>

using namespace System;
using namespace System::Drawing;
using namespace System::Runtime::InteropServices;
using namespace System::Windows::Forms;

#include "IdaHelper.h"

namespace IDACSharp 
{
	public  enum class Enum_Ui_Notification_T
	{
	    ui_null = 0,
		ui_range,
		ui_list,
		ui_idcstart, 
		ui_idcstop,
		ui_suspend,
		ui_resume,  
		ui_jumpto, 
		ui_readsel, 
		ui_unmarksel,
		ui_screenea,
		ui_saving,  
		ui_saved,
		ui_refreshmarked,  
		ui_refresh,      
		ui_choose,
		ui_close_chooser, 
		ui_banner,  
		ui_setidle,
		ui_noabort, 
		ui_term, 
		ui_mbox, 
		ui_beep, 
		ui_msg, 
		ui_askyn,  
		ui_askfile,   
		ui_form,
		ui_close_form,
		ui_clearbreak, 
		ui_wasbreak,
		ui_asktext,  
		ui_askstr, 
		ui_askident,   
		ui_askaddr,  
		ui_askseg,  
		ui_asklong,  
		ui_showauto,
		ui_setstate,
		ui_add_idckey,
//#define IDCHK_OK        0       // ok
//#define IDCHK_ARG       -1      // bad argument(s)
//#define IDCHK_KEY       -2      // bad hotkey name
//#define IDCHK_MAX       -3      // too many IDC hotkeys
		ui_del_idckey, 
		ui_get_marker, 
		ui_analyzer_options, 
		ui_is_msg_inited, 
		ui_load_file,
		ui_run_dbg,
		ui_get_cursor,
		ui_get_curline,
		ui_copywarn,
		ui_getvcl, 
		ui_idp_event, 
		ui_lock_range_refresh,
		ui_unlock_range_refresh,
		ui_setbreak, 
		ui_genfile_callback, 
		ui_open_url, 
		ui_hexdumpea, 
		ui_set_xml, 
		ui_get_xml,
		ui_del_xml,
		ui_push_xml, 
		ui_pop_xml,
		ui_get_key_code,
		ui_setup_plugins_menu,
		ui_refresh_navband, 
		ui_new_custom_viewer,
		ui_add_menu_item, 
#define SETMENU_INS 0x0000  
#define SETMENU_APP 0x0001   
	
		ui_del_menu_item,    
		ui_debugger_menu_change, 
		ui_get_curplace,     
		ui_create_tform,
		ui_open_tform,   
#define FORM_MDI      0x01 // start by default as MDI
#define FORM_TAB      0x02 // attached by default to a tab
#define FORM_RESTORE  0x04 // restore state from desktop config
#define FORM_ONTOP    0x08 // form should be "ontop"
#define FORM_MENU     0x10 // form must be listed in the windows menu
#define FORM_CENTERED 0x20 // form will be centered on the screen

		ui_close_tform, 
#define FORM_SAVE           0x1 // save state in desktop config
#define FORM_NO_CONTEXT     0x2 // don't change the current context (useful for toolbars)
#define FORM_DONT_SAVE_SIZE 0x4 // don't save size of the window
		
		ui_switchto_tform,  
		ui_find_tform,    
		ui_get_current_tform,
		ui_get_tform_handle,  
		ui_tform_visible,  
		ui_tform_invisible, 
		ui_get_ea_hint, 
		ui_get_item_hint,  
		ui_set_nav_colorizer, 
		ui_refresh_custom_viewer,
		ui_destroy_custom_viewer, 

		ui_jump_in_custom_viewer, 
		ui_set_custom_viewer_popup,
		ui_add_custom_viewer_popup,
		ui_set_custom_viewer_handlers,
		ui_get_custom_viewer_curline,
		
		ui_get_current_viewer,
		ui_is_idaview,       
		ui_get_custom_viewer_hint,

		ui_readsel2,  
		ui_set_custom_viewer_range,
		
		ui_database_inited, 

		ui_ready_to_run,    
		ui_set_custom_viewer_handler,
		// * set a handler for a custom viewer event
		// TCustomControl *custom_viewer
		// custom_viewer_handler_id_t handler_id
		// void *handler_or_data
		// returns: old value of the handler or data
		// see also: ui_set_custom_viewer_handlers

		ui_refresh_chooser,   // * Mark a non-modal custom chooser for a refresh
		// Parameters:
		//      const char *title
		// Returns: bool success

		ui_add_chooser_cmd,   // * add a menu item to a chooser window
		// const char *chooser_caption
		// const char *cmd_caption
		// chooser_cb_t *chooser_cb
		// int menu_index
		// int icon
		// int flags
		// Returns: bool success

		ui_open_builtin,      // * open a window of a built-in type
		// int window_type (one of BWN_... constants)
		// additional params depend on the window type
		// see below for the inline convenience functions
		// Returns: TForm * window pointer

		ui_preprocess,        // cb: ida ui is about to handle a user command
		// const char *name: ui command name
		//   these names can be looked up in ida[tg]ui.cfg
		// returns: int 0-ok, nonzero: a plugin has handled the command

		ui_postprocess,       // cb: an ida ui command has been handled

		ui_set_custom_viewer_mode,
		// * switch between graph/text modes
		// TCustomControl *custom_viewer
		// bool graph_view
		// bool silent
		// Returns: bool success

		ui_gen_disasm_text,   // * generate disassembly text for a range
		// ea_t ea1
		// ea_t ea2
		// text_t *text
		// bool truncate_lines (on inf.margin)
		// returns: nothing, appends lines to 'text'

		ui_gen_idanode_text,  // cb: generate disassembly text for a node
		// qflow_chart_t *fc
		// int node
		// text_t *text
		// Plugins may intercept this event and provide
		// custom text for an IDA graph node
		// They may use gen_disasm_text() for that.
		// Returns: bool text_has_been_generated

		ui_install_cli,       // * install command line interpreter
		// cli_t *cp,
		// bool install
		// Returns: nothing

		ui_execute_sync,      // * execute code in the main thread
		// exec_request_t *req
		// Returns: int code

		ui_enable_input_hotkeys,
		// * enable or disable alphanumeric hotkeys
		//   which can interfere with user input
		// bool enable
		// Returns: bool new_state

		ui_last,              // The last notification code

		// debugger callgates. should not be used directly, see dbg.hpp for details

		ui_dbg_begin = 1000,
		ui_dbg_run_requests = ui_dbg_begin,
		ui_dbg_get_running_request,
		ui_dbg_get_running_notification,
		ui_dbg_clear_requests_queue,
		ui_dbg_get_process_state,
		ui_dbg_start_process,
		ui_dbg_request_start_process,
		ui_dbg_suspend_process,
		ui_dbg_request_suspend_process,
		ui_dbg_continue_process,
		ui_dbg_request_continue_process,
		ui_dbg_exit_process,
		ui_dbg_request_exit_process,
		ui_dbg_get_thread_qty,
		ui_dbg_getn_thread,
		ui_dbg_select_thread,
		ui_dbg_request_select_thread,
		ui_dbg_step_into,
		ui_dbg_request_step_into,
		ui_dbg_step_over,
		ui_dbg_request_step_over,
		ui_dbg_run_to,
		ui_dbg_request_run_to,
		ui_dbg_step_until_ret,
		ui_dbg_request_step_until_ret,
		ui_dbg_get_reg_val,
		ui_dbg_set_reg_val,
		ui_dbg_request_set_reg_val,
		ui_dbg_get_bpt_qty,
		ui_dbg_getn_bpt,
		ui_dbg_get_bpt,
		ui_dbg_add_bpt,
		ui_dbg_request_add_bpt,
		ui_dbg_del_bpt,
		ui_dbg_request_del_bpt,
		ui_dbg_update_bpt,
		ui_dbg_enable_bpt,
		ui_dbg_request_enable_bpt,
		ui_dbg_set_trace_size,
		ui_dbg_clear_trace,
		ui_dbg_request_clear_trace,
		ui_dbg_is_step_trace_enabled,
		ui_dbg_enable_step_trace,
		ui_dbg_request_enable_step_trace,
		ui_dbg_get_step_trace_options,
		ui_dbg_set_step_trace_options,
		ui_dbg_request_set_step_trace_options,
		ui_dbg_is_insn_trace_enabled,
		ui_dbg_enable_insn_trace,
		ui_dbg_request_enable_insn_trace,
		ui_dbg_get_insn_trace_options,
		ui_dbg_set_insn_trace_options,
		ui_dbg_request_set_insn_trace_options,
		ui_dbg_is_func_trace_enabled,
		ui_dbg_enable_func_trace,
		ui_dbg_request_enable_func_trace,
		ui_dbg_get_func_trace_options,
		ui_dbg_set_func_trace_options,
		ui_dbg_request_set_func_trace_options,
		ui_dbg_get_tev_qty,
		ui_dbg_get_tev_info,
		ui_dbg_get_insn_tev_reg_val,
		ui_dbg_get_insn_tev_reg_result,
		ui_dbg_get_call_tev_callee,
		ui_dbg_get_ret_tev_return,
		ui_dbg_get_bpt_tev_ea,
		ui_dbg_is_reg_integer,
		ui_dbg_get_process_qty,
		ui_dbg_get_process_info,
		ui_dbg_attach_process,
		ui_dbg_request_attach_process,
		ui_dbg_detach_process,
		ui_dbg_request_detach_process,
		ui_dbg_get_first_module,
		ui_dbg_get_next_module,
		ui_dbg_bring_to_front,
		ui_dbg_get_current_thread,
		ui_dbg_wait_for_next_event,
		ui_dbg_get_debug_event,
		ui_dbg_set_debugger_options,
		ui_dbg_set_remote_debugger,
		ui_dbg_load_debugger,
		ui_dbg_retrieve_exceptions,
		ui_dbg_store_exceptions,
		ui_dbg_define_exception,
		ui_dbg_suspend_thread,
		ui_dbg_request_suspend_thread,
		ui_dbg_resume_thread,
		ui_dbg_request_resume_thread,
		ui_dbg_get_process_options,
		ui_dbg_check_bpt,
		ui_dbg_set_process_state,
		ui_dbg_get_manual_regions,
		ui_dbg_set_manual_regions,
		ui_dbg_enable_manual_regions,
		ui_dbg_set_process_options,

		ui_dbg_end,

	};


	// 内核窗口
	public ref class KernelWin
	{
	private:


	internal:
		//// 转换字符串为char*
		//static char* CastStringToChar(String^ str){
		//	if(String::IsNullOrEmpty(str)) return NULL;

		//	IntPtr ip = Marshal::StringToHGlobalAnsi(str);
		//	return static_cast<char*>(ip.ToPointer());
		//}


	public:
		static void MsgBox(String^ str){
			MessageBox::Show(str);
		}

		// 信息
		static int Msg(String^ str){
			const char* ch = IdaHelper::CastStringToChar(str);
			if(ch == NULL) return 0;
			int rt = msg(ch);
			return rt;
		}

		static void Msg(String^ format, ...array<Object^>^ args){
			Msg(String::Format(format, args));
		}

		static void WriteLine(String^ str){
			Msg(str + System::Environment::NewLine);
		}

		static void WriteLine(String^ format, ...array<Object^>^ args){
			Msg(String::Format(format + System::Environment::NewLine, args));
		}

		static void Info(String^ str){
			const char* ch = IdaHelper::CastStringToChar(str);
			if(ch != NULL) info(ch);
		}

		static void Warning(String^ str){
			const char* ch = IdaHelper::CastStringToChar(str);
			if(ch != NULL) warning(ch);
		}

		// 跳转
		static bool Jump(ea_t ea){ 
			//Msg("跳往：0x{0:X8}\n", ea);
			return jumpto(ea);
		}

		// 跳转
		static bool Jump(ea_t ea, int opnum){ 
			//Msg("跳往：0x{0:X8}\n", ea);
			return jumpto(ea, opnum);
		}

		// 等待
		static bool AutoWait(){ return autoWait(); }

		// 为IDC函数添加热键
		static int AddHotkey(String^ hotkey, String^ idcfunc){
			return add_idc_hotkey(IdaHelper::CastStringToChar(hotkey), IdaHelper::CastStringToChar(idcfunc));
		}

		// 删除热键
		static int DelHotkey(String^ hotkey){
			return del_idc_hotkey(IdaHelper::CastStringToChar(hotkey));
		}

		// 取得当前行地址
		static ea_t GetScreen(){
			return get_screen_ea();
		}

		// 取得当前位置
		static Point^ GetCursor(){
			int x, y;
			get_cursor(&x, &y);
			return gcnew Point(x, y);
		}

		static bool GetCursor(int* x, int* y){
			return get_cursor(x, y);
		}

		// 取得当前行
		static String^ GetCurLine(){
			return IdaHelper::CastCharToString(get_curline());
		}

		// 取得选择范围
		static Point^ ReadSelection(){
			ea_t x, y;
			read_selection(&x, &y);
			return gcnew Point(x, y);
		}
	};
}