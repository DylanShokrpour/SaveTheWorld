using System;

namespace GodotExtensions.scripts.onready;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class OnReadyAttribute : Attribute {
	public string NodePath { get; }

	public OnReadyAttribute(string nodePath) {
		NodePath = nodePath;
	}
}