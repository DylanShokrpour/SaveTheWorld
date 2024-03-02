using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace GodotExtensions.extensions.node;

public static class NodeExtensions {
	/* Unimplemented
public enum MaxAncestorRecursionDepth {
	Parent = 0,
	Grandparent = 1,
	GreatGrandparent = 2,
	GreatGreatGrandparent = 3,
	Unlimited = -1
}

public enum MaxDescendantRecursionDepth {
	Children = 0,
	Grandchildren = 1,
	GreatGrandchildren = 2,
	GreatGreatGrandchildren = 3,
	Unlimited = -1
}
*/


	public static void AddChildWithProperName(this Node node, Node child) {
		node.AddChild(child, true);
	}

	public static void CopyMultiplayerAuthorityFrom(this Node node, Node from) {
		node.SetMultiplayerAuthority(from.GetMultiplayerAuthority());
	}

	public static void QueueFreeChildren(this Node node) {
		foreach (Node child in node.GetChildren())
			child.QueueFree();
	}

	// a node can only have 1 parent, so we don't have get first parent.
	// also no GetFirstChildWithName, because they are all unique. (no same name)
	public static T? GetFirstChildWhichIsA<T>(this Node node) where T : Node {
		foreach (Node child in node.GetChildren())
			if (child is T t)
				return t;
		return null;
	}

	public static T? GetFirstChildOfClass<T>(this Node node) where T : Node {
		return node.GetChildren().FirstOrDefault(IsRightClass) as T;

		bool IsRightClass(Node child) => child.GetType() == typeof(T);
	}


	// maybe add a optional recursion limit? ----

	// will fail if the child is not the correct type.
	public static T? FindFirstDescendantWithName<T>(this Node node, string name) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if (child.Name == name)
				return child as T; // can fail!

			var descendant = child.FindFirstDescendantWithName<T>(name);
			if (descendant != null)
				return descendant;
		}

		return null;
	}


	public static T? FindFirstDescendantWithNameWhichIsA<T>(this Node node, string name) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if (child is T t && (child.Name == name))
				return t;

			var descendant = child.FindFirstDescendantWithNameWhichIsA<T>(name);
			if (descendant != null)
				return descendant;
		}

		return null;
	}

	public static T? FindFirstDescendantWithNameOfClass<T>(this Node node, string name) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if ((child.GetType() == typeof(T)) && (child.Name == name))
				return child as T;

			var descendant = child.FindFirstDescendantWithNameOfClass<T>(name);
			if (descendant != null)
				return descendant;
		}

		return null;
	}


	public static T? FindFirstDescendantWhichIsA<T>(this Node node) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if (child is T t)
				return t;
			var descendant = child.FindFirstDescendantWhichIsA<T>();
			if (descendant != null)
				return descendant;
		}

		return null;
	}

	public static T? FindFirstDescendantOfClass<T>(this Node node) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if (IsRightClass(child))
				return child as T;

			var descendant = child.FindFirstDescendantOfClass<T>();
			if (descendant != null)
				return descendant;
		}

		return null;

		bool IsRightClass(Node child) => child.GetType() == typeof(T);
	}

	// this can fail if the node is not the correct type.
	public static T? FindFirstAncestorWithName<T>(this Node node, string name) where T : Node {
		Node parent = node.GetParent();

		while (parent != null) {
			// because we can reach the end of the scene tree.
			if (parent.Name == name)
				return parent as T; // can fail!
			parent = parent.GetParent();
		}

		return null; // we searched the whole tree and didn't find anything.
	}

	public static T? FindFirstAncestorWhichIsA<T>(this Node node) where T : Node {
		Node parent = node.GetParent();

		while (parent != null) {
			// because we can reach the end of the scene tree.
			if (parent is T t)
				return t;
			parent = parent.GetParent();
		}

		return null; // we searched the whole tree and didn't find anything.
	}

	public static T? FindFirstAncestorOfClass<T>(this Node node) where T : Node {
		Node parent = node.GetParent();

		while (parent != null) {
			// because we can reach the end of the scene tree.
			if (parent.GetType() == typeof(T))
				return parent as T;
			parent = parent.GetParent();
		}

		return null; // we searched the whole tree and didn't find anything.
	}

	// --- lists/arrays, not using Array<> because of marshalling (will make it slower), also lists are annoying to use.
	// apparently IEnumerable is faster than List<> and Array<> (https://stackoverflow.com/questions/1577822/ienumerable-vs-list-what-to-use-how-do-they-work)
	// consider using .ToArray() or .ToList() if you need to use it as a list or array, also sometimes better for speed https://stackoverflow.com/questions/19689328/why-ienumerable-slow-and-list-is-fast

	public static IEnumerable<T> GetChildrenWhichAreA<T>(this Node node) where T : Node {
		return node.GetChildren().Where(IsClassOrSubclassOrImplementsT).Cast<T>();

		bool IsClassOrSubclassOrImplementsT(Node child) => child is T;
	}

	/// make sure you use .ToList() or .ToArray(), if you do not use LINQ. Because it will be faster.
	public static IEnumerable<T> GetChildrenOfClass<T>(this Node node) where T : Node {
		return node.GetChildren().Where(IsRightClass).Cast<T>();

		bool IsRightClass(Node child) => child.GetType() == typeof(T);
	}

	/// make sure you use .ToList() or .ToArray(), if you do not use LINQ. Because it will be faster.?
	public static IEnumerable<T?> GetDescendantsWhichAreA<T>(this Node node) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if (IsClassOrSubclassOrImplementsT(child))
				yield return child as T;

			foreach (T? descendant in child.GetDescendantsWhichAreA<T>())
				yield return descendant;
		}

		yield break;

		bool IsClassOrSubclassOrImplementsT(Node child) => child is T;
	}

	/// make sure you use .ToList() or .ToArray(), if you do not use LINQ. Because it will be faster.?
	public static IEnumerable<T?> GetDescendantsOfClass<T>(this Node node) where T : Node {
		foreach (Node child in node.GetChildren()) {
			if (IsRightClass(child))
				yield return child as T;

			foreach (T? descendant in child.GetDescendantsOfClass<T>())
				yield return descendant;
		}

		yield break;

		bool IsRightClass(Node child) => child.GetType() == typeof(T);
	}

	// make sure you use .ToList() or .ToArray(), if you do not use LINQ. Because it will be faster.?
	public static IEnumerable<T> GetAncestorsWhichAreA<T>(this Node node) where T : Node {
		Node parent = node.GetParent();

		while (parent != null) {
			// because we can reach the end of the scene tree.
			if (parent is T t)
				yield return t;
			parent = parent.GetParent();
		}
	}

	// make sure you use .ToList() or .ToArray(), if you do not use LINQ. Because it will be faster.?
	public static IEnumerable<T?> GetAncestorsOfClass<T>(this Node node) where T : Node {
		Node parent = node.GetParent();

		while (parent != null) {
			// because we can reach the end of the scene tree.
			if (parent.GetType() == typeof(T))
				yield return parent as T;
			parent = parent.GetParent();
		}
	}

	// The name and type must be correct.
	public static Task<T> WaitForChildWithName<T>(this Node node, string name) where T : Node {
		TaskCompletionSource<T> tcs = new();

		// because can break if node-paths are used.
		var existing = node.GetChildWithName<T>(name);
		if (existing != null)
			return Task.FromResult(existing);

		node.ChildEnteredTree += Handler;
		return tcs.Task;

		void Handler(Node child) {
			if (child.Name != name)
				return;
			if (child is not T t)
				return;

			node.ChildEnteredTree -= Handler;
			tcs.TrySetResult(t);
		}
	}

	static T? GetChildWithName<T>(this Node node, string name) where T : Node {
		foreach (Node child in node.GetChildren())
			if (child.Name == name)
				return child as T;
		return null;
	}

	// returns the child if it already exists, otherwise waits for it to be added to the tree.
	public static Task<T> WaitForChildWhichIsA<T>(this Node node) where T : Node {
		TaskCompletionSource<T?> tcs = new();

		var existing = node.GetFirstChildWhichIsA<T>();
		if (existing != null)
			return Task.FromResult(existing);

		node.ChildEnteredTree += Handler;
		return tcs.Task;

		void Handler(Node child) {
			if (child is not T t)
				return;

			node.ChildEnteredTree -= Handler;
			tcs.TrySetResult(t);
		}
	}

	// returns the child if it already exists, otherwise waits for it to be added to the tree.
	public static Task<T> WaitForChildOfClass<T>(this Node node) where T : Node {
		TaskCompletionSource<T> tcs = new();

		var existing = node.GetFirstChildOfClass<T>();
		if (existing != null)
			return Task.FromResult(existing);

		node.ChildEnteredTree += Handler;
		return tcs.Task;

		void Handler(Node child) {
			if (child.GetType() != typeof(T))
				return;

			node.ChildEnteredTree -= Handler;
			tcs.TrySetResult(child as T);
		}
	}

	// --

	public static bool IsChildOf(this Node node, Node parent) => node.GetParent() == parent;

	public static bool IsDescendantOf(this Node node, Node ancestor) {
		Node parent = node.GetParent();

		while (parent != null) {
			// because we can reach the end of the scene tree.
			if (parent == ancestor)
				return true;
			parent = parent.GetParent();
		}

		return false;
	}

	public static bool IsAncestorOf(this Node node, Node descendant) => descendant.IsDescendantOf(node);

	// public static Array<T> GetChildrenWhichAreA<[MustBeVariant] T>(this Node node) where T : Node {
	// 	Array<T> children = new();
	// 	foreach (Node child in node.GetChildren())
	// 		if (child is T t)
	// 			children.Add(t);
	// 	return children;
	// }


	public async static Task YieldForProcessFrame(this Node node) {
		await node.ToSignal(node.GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	public async static Task YieldForPhysicsFrame(this Node node) {
		await node.ToSignal(node.GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	public async static Task YieldForSeconds(this Node node, float seconds) {
		await node.ToSignal(node.GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}

	public static void UnParent(this Node node) {
		node.GetParent()?.RemoveChild(node);
	}

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
		ArgumentNullException.ThrowIfNull(node, nameof(node));
		List<Node> descendants = new();
		foreach (Node child in node.GetChildren()) {
			descendants.Add(child);
			descendants.AddRange(child.GetDescendants());
		}

		return descendants;
	}

	/// <summary>
	///     Gets all descendants of a node.
	/// </summary>
	/// <param name="node">The node to search in.</param>
	/// <returns>An enumerable of all descendants.</returns>
	public static IEnumerable<Node> GetDescendants(this Node node, Func<Node, bool> predicate) {
		ArgumentNullException.ThrowIfNull(node, nameof(node));
		List<Node> descendants = new();
		foreach (Node child in node.GetChildren()) {
			if (predicate(child))
				descendants.Add(child);
			descendants.AddRange(child.GetDescendants(predicate));
		}

		return descendants;
	}

	public static void AddEnteredExitedHandlers<T>(
		this Node node,
		List<T> collection) where T : Node {
		node.ChildEnteredTree += child => {
			if (child is T typedChild)
				collection.Add(typedChild);
		};

		node.ChildExitingTree += child => {
			if (child is T typedChild)
				collection.Remove(typedChild);
		};
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

public struct NodeSetupResult {
	public bool ValidationSuccess { get; }

	public NodeSetupResult(bool success) {
		ValidationSuccess = success;
	}
}