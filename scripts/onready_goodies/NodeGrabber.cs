using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using GodotExtensions.extensions.node;
using GodotExtensions.extensions.type;
using static GodotExtensions.PrintGlobals;

namespace GodotExtensions.scripts.onready;

public static class NodeGrabber {

	/// <param name="type">The type to get all the members from. Must be a subclass of <see cref="Node" />.</param>
	/// <returns>All members with the <see cref="OnReadyAttribute" />; in the type hierarchy.</returns>
	readonly static Dictionary<Type, List<(MemberInfo memberInfo, OnReadyAttribute attribute)>> cachedOnReadyMembers = new();

	public static void SetOnReadyNodes(Node node) {
		Type type = node.GetType();
		SetOnReadyNodesV2(node, type);
		//SetOnReadyNodesRecursively(node, type);
	}

	static void SetFieldOnReadyVariables(Node node, Type type) {
		foreach (FieldInfo fieldInfo in node.GetType().GetFieldsWithAttribute<OnReadyAttribute>()) {
			// get the path on the attribute
			var attr = fieldInfo.GetCustomAttribute<OnReadyAttribute>();
			if (attr == null) {
				// this can literally never happen, but the compiler doesn't know that.
				PrintError($"OnReadyAttribute is null for {fieldInfo.Name} on {type.Name}. This should not happen.");
				continue;
			}

			string nodePath = attr.NodePath;
			Type fieldType = fieldInfo.FieldType;
			// make sure field type is a node, either a node or a subclass of node.
			if (!typeof(Node).IsAssignableFrom(fieldType)) {
				PrintError($"Field \"{fieldInfo.Name}\" of type \"{fieldType.Name}\" is not a class or subclass of Node for on \"{type.Name}\". | {node.DumpNodeDebugInfo()}");
				continue;
			}

			Node? nodeAtPath = node.GetNode(nodePath);
			if (nodeAtPath == null) {
				PrintError($"Node at path {nodePath} is null for {fieldInfo.Name} on {type.Name}. caller: | {node.DumpNodeDebugInfo()}");
				continue;
			}

			try {
				fieldInfo.SetValue(node, nodeAtPath);
			} catch (Exception e) {
				PrintError($"Failed to set field {fieldInfo.Name} on {type.Name} with node at path {nodePath}. caller: | {node.DumpNodeDebugInfo()} | Exception {e}");
				PrintError(e.ToString());
			}
		}
	}

	static void SetPropertyOnReadyVariables(Node node, Type type) {
		foreach (PropertyInfo info in node.GetType().GetPropertiesWithAttribute<OnReadyAttribute>()) {
			// get the path on the attribute
			var attr = info.GetCustomAttribute<OnReadyAttribute>();
			if (attr == null) {
				// this can literally never happen, but the compiler doesn't know that.
				PrintError($"OnReadyAttribute is null for {info.Name} on {type.Name}.");
				continue;
			}

			string nodePath = attr.NodePath;
			Type propertyType = info.PropertyType;


			// skip if it doesn't have a setter
			if (!info.CanWrite) {
				PrintWarning($"Property {info.Name} on {type.Name} does not have a setter | {node.DumpNodeDebugInfo()} | Please add a setter to this property." +
				                 " If it is a derived class, make sure the setter is protected, internal, or public.");
				continue;
			}


			// make sure field type is a node
			if (!typeof(Node).IsAssignableFrom(propertyType)) {
				PrintError($"Property type {propertyType.Name} is not a subclass of Node for {info.Name} on {type.Name}.");
				continue;
			}


			Node? nodeAtPath = node.GetNode(nodePath);
			if (nodeAtPath == null) {
				PrintError($"Node at path {nodePath} is null for {info.Name} on {type.Name}. caller: | {node.DumpNodeDebugInfo()}");
				continue;
			}

			try {
				info.SetValue(node, nodeAtPath);
			} catch (Exception e) {
				PrintError($"Failed to set property {info.Name} on {type.Name} with node at path {nodePath}. caller: | {node.DumpNodeDebugInfo()} | Exception {e}");
				PrintError(e.ToString());
			}
		}
	}

	// Recursive function that traverses type hierarchy
	static void SetOnReadyNodesRecursively(Node node, Type? type) {
		if (type == null || type == typeof(object))
			return;

		SetFieldOnReadyVariables(node, type);
		SetPropertyOnReadyVariables(node, type);

		// Move up the inheritance chain
		SetOnReadyNodesRecursively(node, type.BaseType);
	}

	static void SetOnReadyNodesV2(Node node, Type type) {
		List<(MemberInfo memberInfo, OnReadyAttribute attribute)> allMembers = GetAllOnReadyMembers(type).ToList();

		// now set them.
		foreach ((MemberInfo mi, OnReadyAttribute attr) in allMembers)
			try {
				AttemptSetMemberValue(mi, attr, node);
			} catch (Exception e) {
				ReportError($"Failed to set member-name: \"{mi.Name}\" on type-name: {type.Name}", node, e);
			}

		return;

		static void AttemptSetMemberValue(MemberInfo info, OnReadyAttribute attr, Node instance) {
			Type? memberType = GetMemberType(info);
			if (memberType is null)
				throw new NodeInitializationException($"Member type is null for member-name: \"{info.Name}\" " +
				                                      $"on type-name: {instance.GetType().Name}. " +
				                                      $"It should be either a PropertyInfo, or FieldInfo");

			bool isMemberTypeANode = typeof(Node).IsAssignableFrom(memberType);
			if (!isMemberTypeANode)
				throw new NodeInitializationException($"Member type: \"{memberType.Name}\" is not a class or subclass of Node");


			Node? nodeAtPath = instance.GetNodeOrNull(attr.NodePath);
			if (nodeAtPath is null)
				throw new NodeInitializationException($"Node at path: \"{attr.NodePath}\" is null " +
				                                      $"for member-name: \"{info.Name}\" " +
				                                      $"on type-name: {instance.GetType().Name}");

			if (!memberType.IsInstanceOfType(nodeAtPath))
				throw new NodeInitializationException($"Node at path: \"{attr.NodePath}\" " +
				                                      $"is of type: \"{nodeAtPath.GetType().Name}\" " +
				                                      $"which is not assignable to member-type: \"{memberType.Name}\" " +
				                                      $"for member-name: \"{info.Name}\".");

			try {
				SetMemberValue(info, instance, nodeAtPath);
				ConfirmMemberValueWasSet(info, instance, nodeAtPath);
			} catch (Exception e) {
				ReportError($"Failed to set member-name: \"{info.Name}\" on type-name: {instance.GetType().Name}", instance, e);
				throw;
			}

			return;


			static Type? GetMemberType(MemberInfo memberInfo) {
				if (memberInfo is FieldInfo fieldInfo)
					return fieldInfo.FieldType;
				if (memberInfo is PropertyInfo propertyInfo)
					return propertyInfo.PropertyType;
				return null;
			}

			static void SetMemberValue(MemberInfo info, Node instance, Node nodeAtPath) {
				if (info is FieldInfo fieldInfo)
					fieldInfo.SetValue(instance, nodeAtPath);
				else if (info is PropertyInfo propertyInfo) {
					if (!propertyInfo.CanWrite)
						throw new NodeInitializationException($"Property \"{propertyInfo.Name}\" on type \"{instance.GetType().Name}\" does not have a setter. " +
						                                      $"Please add a setter to this property. " +
						                                      $"If it is a derived class, make sure the setter is protected, internal, or public.");

					propertyInfo.SetValue(instance, nodeAtPath);
				}
			}

			static void ConfirmMemberValueWasSet(MemberInfo info, Node instance, Node nodeAtPath) {
				switch (info) {
					case FieldInfo fieldInfo: {
						var value = fieldInfo.GetValue(instance) as Node;
						if (value != nodeAtPath)
							throw new NodeInitializationException($"Failed to set member-name: \"{info.Name}\" " +
							                                      $"on type-name: {instance.GetType().Name} " +
							                                      $"with node at path: \"{nodeAtPath.Name}\".");
						break;
					}
					case PropertyInfo propertyInfo: {
						var value = propertyInfo.GetValue(instance) as Node;
						if (value != nodeAtPath)
							throw new NodeInitializationException($"Failed to set member-name: \"{info.Name}\" " +
							                                      $"on type-name: {instance.GetType().Name} " +
							                                      $"with node at path: \"{nodeAtPath.Name}\".");
						break;
					}
				}
			}
		}
	}


	static IEnumerable<(MemberInfo memberInfo, OnReadyAttribute attribute)> GetAllOnReadyMembers(Type? type) {
		while (type != null && type != typeof(Node)) {
			foreach ((MemberInfo memberInfo, OnReadyAttribute attribute) member in GetOrCacheOnReadyMembers(type))
				yield return member;

			type = type.BaseType;
		}

		static List<(MemberInfo memberInfo, OnReadyAttribute attribute)> GetOrCacheOnReadyMembers(Type type) {
			if (cachedOnReadyMembers.TryGetValue(type, out List<(MemberInfo memberInfo, OnReadyAttribute attribute)>? cachedMembers))
				return cachedMembers;

			List<(MemberInfo memberInfo, OnReadyAttribute attribute)> members = GetOnReadyMembersWithReflection(type).ToList();
			cachedOnReadyMembers[type] = members;
			return members;


			static IEnumerable<(MemberInfo memberInfo, OnReadyAttribute attribute)> GetOnReadyMembersWithReflection(Type type) {
				MemberInfo[] members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				foreach (MemberInfo memberInfo in members) {
					var attr = memberInfo.GetCustomAttribute<OnReadyAttribute>();
					if (attr != null)
						yield return (memberInfo, attr);
				}
			}
		}
	}


	static void ReportError(string message, Node node, Exception? exception = null) {
		string exceptionDetails = exception != null ? $" |\n Exception: {exception}" : string.Empty;
		PrintError($"{message} |\n Node Debug: {node.DumpNodeDebugInfo()}{exceptionDetails}");
	}

	class NodeInitializationException : Exception {
		public NodeInitializationException(string message) : base(message) { }
	}
}

