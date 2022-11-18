using System;

namespace PaGG.Core
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class SomethingSomethingAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SomethingSomethingClassAttribute : Attribute
	{
	}
}
