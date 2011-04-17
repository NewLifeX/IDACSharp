
#pragma once

#include <ida.hpp>
#include <idp.hpp>

using namespace System;
using namespace System::Drawing;
using namespace System::Runtime::InteropServices;
using namespace System::Windows::Forms;

#include "IDA.h"

namespace IDACSharp {

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
			const char* ch = IDA::CastStringToChar(str);
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
			const char* ch = IDA::CastStringToChar(str);
			if(ch != NULL) info(ch);
		}

		static void Warning(String^ str){
			const char* ch = IDA::CastStringToChar(str);
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
			return add_idc_hotkey(IDA::CastStringToChar(hotkey), IDA::CastStringToChar(idcfunc));
		}

		// 删除热键
		static int DelHotkey(String^ hotkey){
			return del_idc_hotkey(IDA::CastStringToChar(hotkey));
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
			return IDA::CastCharToString(get_curline());
		}

		// 取得选择范围
		static Point^ ReadSelection(){
			ea_t x, y;
			read_selection(&x, &y);
			return gcnew Point(x, y);
		}
	};
}