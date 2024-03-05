using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GodotExtensions.extensions.type;

public static class TypeExtensions {
	public static bool HasAttribute<T>(this Type type) where T : Attribute => type.GetCustomAttribute<T>() is not null;

	public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type, BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public) where T : Attribute {
		return type
			.GetProperties(bindingFlags)
			.Where(prop => Attribute.IsDefined(prop, typeof(T)))
			.ToList();
	}

	public static List<FieldInfo> GetFieldsWithAttribute<T>(this Type type) where T : Attribute {
		return type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(prop => Attribute.IsDefined(prop, typeof(T))).ToList();
	}

	public static List<(FieldInfo fieldInfo, TAttribute attribute)> GetFieldsWithAttributeTuples<TAttribute>(this Type type) where TAttribute : Attribute {
		List<(FieldInfo fieldInfo, TAttribute attribute)> result = new();

		FieldInfo[] allFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		foreach (FieldInfo fieldInfo in allFields) {
			var attribute = fieldInfo.GetCustomAttribute<TAttribute>();
			if (attribute != null)
				result.Add((fieldInfo, attribute));
		}

		return result;
	}

	public static List<(PropertyInfo propertyInfo, TAttribute attribute)> GetPropertiesWithAttributeTuples<TAttribute>(
		this Type type,
		BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public
	) where TAttribute : Attribute {
		List<(PropertyInfo propertyInfo, TAttribute attribute)> result = new();

		PropertyInfo[] allProperties = type.GetProperties(bindingFlags);
		foreach (PropertyInfo propertyInfo in allProperties) {
			var attribute = propertyInfo.GetCustomAttribute<TAttribute>();
			if (attribute != null)
				result.Add((propertyInfo, attribute));
		}

		return result;
	}

	public static List<(MemberInfo memberInfo, TAttribute attribute)> GetMembersWithAttributeTuples<TAttribute>(this Type type) where TAttribute : Attribute {
		List<(MemberInfo memberInfo, TAttribute attribute)> result = new();

		MemberInfo[] allMembers = type.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
		foreach (MemberInfo memberInfo in allMembers) {
			var attribute = memberInfo.GetCustomAttribute<TAttribute>();
			if (attribute != null)
				result.Add((memberInfo, attribute));
		}

		return result;
	}

	public static List<MemberInfo> GetMembersWithAttribute<TAttribute>(this Type type) where TAttribute : Attribute {
		return type
			.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
			.Where(prop => Attribute.IsDefined(prop, typeof(TAttribute)))
			.ToList();
	}
}