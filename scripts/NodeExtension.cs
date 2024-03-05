#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using GodotExtensions.scripts.onready;

namespace GodotExtensions.extensions.node;

public static class NodeExtensions {
	public static void InitializeOnReadyVariables(this Node node) {
		NodeGrabber.SetOnReadyNodes(node);
	}
	
	public static T? FindFirstChild<T>(this Node node) where T : Node {
		foreach (Node child in node.GetChildren())
			if (child is T t)
				return t;
		return null;
	}

	public static T? FindFirstChildOfClass<T>(this Node node) where T : Node {
		return node.GetChildren().FirstOrDefault(IsRightClass) as T;
		bool IsRightClass(Node child) => child.GetType() == typeof(T);
	}


	#region Time Related

	/// <summary>
	///     Waits for a specified number of process frames.
	/// </summary>
	/// <param name="node"></param>
	/// <param name="count">The number of process frames to wait for.</param>
	public async static Task WaitForProcessFrame(this Node node, int count = 1) {
		for (var i = 0; i < count; i++)
			await node.ToSignal(node.GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	/// <summary>
	///     Waits for a specified number of physics frames.
	/// </summary>
	/// <param name="node"></param>
	/// <param name="count">The number of physics frames to wait for.</param>
	public async static Task WaitForPhysicsFrame(this Node node, int count = 1) {
		for (var i = 0; i < count; i++)
			await node.ToSignal(node.GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	/// <example>
	///     <code lang="cs">
	/// 	public partial class MyNode : Node {
	/// 		public async override void _Ready() {
	///   
	///  		GD.Print("Waiting for 1 second...");
	///  		await this.WaitForSeconds(1);
	///  		GD.Print("Done waiting!"); // Prints after 1 second
	///  
	/// 		}
	/// 	}
	///           </code>
	/// </example>
	public async static Task WaitForSeconds(this Node node, float seconds) {
		await node.ToSignal(node.GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}

	#endregion
	

	public static IEnumerable<T> GetDescendants<T>(this Node node) where T : Node {
		return GetDescendants(node).Where(descendant => descendant is T).Cast<T>();
	}

	/// <summary>
	///     Gets all descendants of a node with a specific name.
	/// </summary>
	/// <param name="node">The node to search in.</param>
	/// <param name="name">The name of the descendants to find.</param>
	/// <returns>An enumerable of all descendants with the specified name.</returns>
	public static IEnumerable<Node> GetDescendants(this Node node, string name) {
		return GetDescendants(node).Where(descendant => descendant.Name == name);
	}

	/// <summary>
	///     Gets all descendants of a node.
	/// </summary>
	/// <param name="node">The node to search in.</param>
	/// <returns>An enumerable of all descendants.</returns>
	public static IEnumerable<Node> GetDescendants(this Node node) {
		// using the C++ method for optimization.
		var descendants = node.FindChildren("*", "", true, false);
		return descendants;
	}
	
	public static string DumpNodeDebugInfo(this Node node) {
		string nodeName = node.Name;
		string typeName = node.GetType().Name;

		var message = $"Node with name: \"{nodeName}\" of type: \"({typeName})\"";

		if (node.IsInsideTree()) {
			string path = node.GetPath();
			message += $" at node-path: \"{path}\"";
		} else
			message += " (not in tree)";

		// if it has a script
		Script script;
		Variant scriptVariant = node.GetScript();
		if (scriptVariant.VariantType != Variant.Type.Nil) {
			script = scriptVariant.As<Script>();
			string scriptPath = script.ResourcePath;
			message += $" with script: \"{scriptPath}\"";
		} else
			message += " (no script)";

		return message;
	}
}