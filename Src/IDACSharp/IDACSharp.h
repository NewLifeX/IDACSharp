// IDACSharp.h

#include <kernwin.hpp>

#include "KernelWin.h"

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;

namespace IDACSharp {

	// 加载器
	public ref class Loader
	{
	private:
		// 加载器类型
		static Type^ loaderType = nullptr;

		// 私有构造函数，用于加载CSharp加载器
		static Loader(){
			msg("加载CSharp加载器...\n");

			String^ path = Path::Combine(AppDomain::CurrentDomain->BaseDirectory, "CSharp");
			msg("CSharp目录：%s\n", path);

			if(!Directory::Exists(path)){
				msg("目录%s不存在！\n", path);
				return;
			}

			String^ file = Path::Combine(path, "CSharpLoader.dll");
			if(!File::Exists(file)){
				msg("未找到CSharp加载器%s！请把CShapLoader.dll文件拷贝到该位置！\n", file);
				return;
			}

			msg("准备加载文件%s\n", file);

			try{
				Assembly^ a = Assembly::LoadFile(file);
				if(a == nullptr) {
					msg("加载程序集失败！\n");
					return;
				}

				array<Type^>^ types = a->GetTypes();

				loaderType = a->GetType("CSharpLoader.Loader");
				if(loaderType == nullptr) {
					msg("加载类型失败！\n");
					return;
				}
			}
			catch(ReflectionTypeLoadException^ e){
				msg("加载异常！\n");

				WriteLine(e->ToString());
				WriteLine(e->LoaderExceptions[0]->ToString());
			}
			catch(Exception^ e){
				msg("加载异常！\n");

				WriteLine(e->ToString());
			}

			msg("加载完成！\n");
		}

		static MethodInfo^ FindMethod(String^ name){
			if(loaderType == nullptr){
				msg("未加载类型！\n");
				return nullptr;
			}

			return loaderType->GetMethod(name);
		}

	public:
		// 初始化
		static bool Init(){
			MethodInfo^ method = FindMethod("Init");
			if(loaderType == nullptr){
				msg("未找到方法%s！请确认已把IDACSharp.dll放到IDA根目录下！\n", "Init");
				return false;
			}

			return (bool)method->Invoke(nullptr, nullptr);
		}

		// 启动
		static void Start(){
			MethodInfo^ method = FindMethod("Start");
			if(loaderType == nullptr){
				msg("未找到方法%s！请确认已把IDACSharp.dll放到IDA根目录下！\n", "Start");
				return;
			}

			method->Invoke(nullptr, nullptr);
		}

		// 结束
		static void Term(){
			MethodInfo^ method = FindMethod("Term");
			if(loaderType == nullptr){
				msg("未找到方法%s！请确认已把IDACSharp.dll放到IDA根目录下！\n", "Term");
				return;
			}

			method->Invoke(nullptr, nullptr);
		}

		// 输出一行日志
		static void WriteLine(String^ str){
			File::AppendAllText("csharp.log", str);
		}

		static void WriteLine(String^ format, ...array<Object^>^ args){
			WriteLine(String::Format(format + "\r\n", args));
		}
	};

	// 插件接口。
	// 所有CSharp插件都必须实现该接口，CSharpLoader才能加载插件
	public interface class IPlugin{
		bool Init();
		void Start();
		void Term();

		property String^ Name;
	};
}
