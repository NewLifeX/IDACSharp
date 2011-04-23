#include <ida.hpp>
#include <idp.hpp>
#include <expr.hpp>
#include <bytes.hpp>
#include <loader.hpp>
#include <kernwin.hpp>

#include "IDACSharp.h"

using namespace IDACSharp;

int  idaapi init(void)  { 
	msg("---------------------------------------------------------------------------\n");
	msg("正在初始化 IDACSharp...\n");

	bool b = Loader::Init();
	msg("---------------------------------------------------------------------------\n");

	if (b)
		return PLUGIN_KEEP;
	else
		return PLUGIN_SKIP;
}

void idaapi term(void)  { 
	msg("正在结束 IDACSharp...\n");

	Loader::Term();
}

void idaapi run(int arg){ 
	msg("启动 IDACSharp...\n");

	try{
		//warning("IDACSharp插件启动！");
		Loader::Start();                                   
	}
	catch(...)
	{
		msg("启动IDACSharp异常！");
		//Loader::Term();
		//Loader::Init();
	}
}

char comment[]		 = "IDACSharp插件";
char help[]			 = "这是一个IDACSharp插件，用于向C#提供IDA函数接口，以方便使用C#开发IDA插件";
char wanted_name[]	 = "IDACSharp";
char wanted_hotkey[] = "Alt-8";

extern "C"
{
	plugin_t PLUGIN =	{
		IDP_INTERFACE_VERSION,
		0,                    // plugin flags
		init,                 // initialize
		term,                 // terminate. this pointer may be NULL.
		run,                  // invoke plugin
		comment,              // long comment about the plugin (status line or hint)
		help,                 // multiline help about the plugin
		wanted_name,          // the preferred short name of the plugin
		wanted_hotkey         // the preferred hotkey to run the plugin
	};
}