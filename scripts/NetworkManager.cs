using System.Linq;
using Godot;

[GlobalClass]

public partial class NetworkManager : Node {
	public const int SERVER_PEER_ID = 1;
	[Export] int port = 4005;
	[Export] bool useEncryption;
	// Provide the root node of the scene, to allow RPC's and MultiplayerSynchronizers and MultiplayerSpawners -
	// underneath (descendants) to function.
	[Export] Node rootSceneNode;
	NodePath rootScenePath => rootSceneNode.GetPath();

	SceneMultiplayer? multiplayer;
	ENetMultiplayerPeer? network;
	NetworkManager? instance;

	public NetworkManager() {
		instance = this;
	}

	public bool IsPeerConnected(int peerId) => multiplayer.GetPeers().Contains(peerId);

	/// <summary>
	///     Singleton, to get the instance of the NetworkManager.
	/// </summary>
	public NetworkManager GetInstance() {
		if (instance is null) {
			LogError("NetworkManager instance is null. Did you forget to add it to the scene?");
			return null;
		}

		return instance;
	}

	public override void _EnterTree() {
		// Prints for debugging (Will only run on clients)
		ServerConnected += () => { LogInfo("Connected to the server."); };
		ServerDisconnected += () => { LogInfo("Disconnected from server."); };
		ServerConnectionFailed += () => { LogInfo("Failed to connect to server."); };
		PeerConnected += peerId => { LogInfo($"Peer {peerId} connected."); };
		PeerDisconnected += peerId => { LogInfo($"Peer {peerId} disconnected."); };
	}

	public override void _ExitTree() {
		if (multiplayer is null)
			return;

		if (multiplayer.IsServer())
			CloseServer();
		else
			DisconnectFromServer();
	}

	void Close() {
		if (multiplayer is not null)
			multiplayer.MultiplayerPeer = null;

		if (GetTree().GetMultiplayer(rootScenePath) is not null)
			// set back to defaults...
			GetTree().SetMultiplayer(MultiplayerApi.CreateDefaultInterface(), rootScenePath);

		multiplayer = null;
		network?.Close();
		network = null;
	}

	void InitializeSceneMultiplayer() {
		multiplayer = new();
		multiplayer.RootPath = rootScenePath;

		GetTree().SetMultiplayer(multiplayer, rootScenePath);
	}


	void LogInfo(params object[] message) => GD.Print(GetLogPrefix("INFO" + CreateLogMessage(message)));

	void LogError(params object[] message) {
		string msg = CreateLogMessage(message);
		string prefix = GetLogPrefix("ERROR");
		string logMessage = prefix + msg;
		GD.PrintErr(logMessage);
		GD.PushError(logMessage);
	}

	void LogWarning(params object[] message) {
		string msg = CreateLogMessage(message);
		string prefix = GetLogPrefix("WARN");
		string logMessage = prefix + msg;
		GD.PrintRich("[color=yellow]" + logMessage + "[/color]");
		GD.PushWarning(logMessage);
	}

	string GetLogPrefix(string level) {
		string prefix = $"{level} | ";
		if (multiplayer is not null) {
			int peerId = multiplayer.GetUniqueId();
			if (multiplayer.IsServer())
				prefix += $"Server.{peerId} | "; // Always 1.
			else
				prefix += $"Client.{peerId} | ";
		} else
			prefix += "Not Connected | ";

		return prefix;
	}

	static string CreateLogMessage(params object[] message) {
		string msg = "";
		foreach (object o in message)
			msg += o + " ";
		return msg;
	}
}

// SERVER
partial class NetworkManager {
	[Signal] public delegate void PeerConnectedEventHandler(int peerId);
	[Signal] public delegate void PeerDisconnectedEventHandler(int peerId);
	[Export] public int MaxPeers { get; private set; } = 32;

	public void KickPeer(int peerId) {
		const bool FORCE_REMOVE = true;

		multiplayer.DisconnectPeer(peerId);

		// forcefully disconnect peer after 5 seconds
		GetTree().CreateTimer(5).Timeout += () => {
			if (!IsPeerConnected(peerId))
				return;

			network.DisconnectPeer(peerId, FORCE_REMOVE);
			LogInfo($"Forcefully disconnected peer {peerId} after 5 seconds");
		};
	}

	public void StartServer() {
		InitializeSceneMultiplayer();
		SetupNetworkCallbacks();
		InitializeNetwork();
		
		void InitializeNetwork() {
			network = new();

			Error error = network.CreateServer(port, MaxPeers);
			if (error != Error.Ok)
				LogError("Failed to create server: ", error);

			multiplayer.MultiplayerPeer = network;
			LogInfo($"Server listening on port {port}, but refusing new connections");
		}

		void SetupNetworkCallbacks() {
			multiplayer.PeerConnected += HandlePeerConnected;
			multiplayer.PeerDisconnected += HandlePeerDisconnected;

			void HandlePeerConnected(long peerIdLong) {
				int peerId = (int)peerIdLong;

				// double the defaults ( found from C++ source )
				// allows laggy people to stay connected for longer.
				const int ENET_PEER_TIMEOUT_LIMIT = 32 * 2;
				const int ENET_PEER_TIMEOUT_MINIMUM = 5000 * 2;
				const int ENET_PEER_TIMEOUT_MAXIMUM = 30000 * 2;

				ENetPacketPeer packetPeer = network.GetPeer(peerId);
				packetPeer.SetTimeout(ENET_PEER_TIMEOUT_LIMIT, ENET_PEER_TIMEOUT_MINIMUM, ENET_PEER_TIMEOUT_MAXIMUM);

				EmitSignal(SignalName.PeerConnected, peerId);
			}

			void HandlePeerDisconnected(long peerIdLong) {
				int peerId = (int)peerIdLong;
				EmitSignal(SignalName.PeerDisconnected, peerId);
			}
		}
	}

	public void CloseServer() {
		Close();
	}
}

partial class NetworkManager {
	[Signal] public delegate void ServerConnectedEventHandler();
	[Signal] public delegate void ServerConnectionFailedEventHandler();
	[Signal] public delegate void ServerDisconnectedEventHandler();
	[Export] string ipAddressOfServer = "127.0.0.1";

	public bool IsConnectedToServer => multiplayer is not null && (multiplayer.GetPeers().Length > 0);
	ENetPacketPeer serverPacketPeer;

	public Error ConnectToServer() {
		InitializeSceneMultiplayer();

		// ------ Setup Network ------
		network = new();
		network.RefuseNewConnections = true;

		// ------ Multiplayer Callbacks ------
		multiplayer.ConnectedToServer += () => {
			const int ENET_PEER_TIMEOUT_LIMIT = 32 * 2; // double the defaults
			const int ENET_PEER_TIMEOUT_MINIMUM = 5000 * 2; // double the defaults
			const int ENET_PEER_TIMEOUT_MAXIMUM = 30000 * 2; // double the defaults

			ENetPacketPeer packetPeer = network.GetPeer(1);
			packetPeer.SetTimeout(ENET_PEER_TIMEOUT_LIMIT, ENET_PEER_TIMEOUT_MINIMUM, ENET_PEER_TIMEOUT_MAXIMUM);

			EmitSignal(SignalName.ServerConnected);
		};
		multiplayer.ServerDisconnected += () => EmitSignal(SignalName.ServerDisconnected);
		multiplayer.ConnectionFailed += () => EmitSignal(SignalName.ServerConnectionFailed);

		// ------ Connect to server ------
		LogInfo($"Connecting to server at: \"{ipAddressOfServer}:{port}\"");
		network.RefuseNewConnections = false;
		Error error = network.CreateClient(ipAddressOfServer, port); // TODO: bandwidth limits
		if (error != Error.Ok)
			LogError("Failed to connect to server: ", error);

		multiplayer.MultiplayerPeer = network;
		return error;
	}

	public void DisconnectFromServer() {
		const float GRACEFUL_DISCONNECT_DURATION_SECONDS = 3f;

		if (!IsConnectedToServer) {
			LogError("Not connected to server, cannot disconnect.");
			return;
		}

		multiplayer.DisconnectPeer(SERVER_PEER_ID);
		Close();

		// In case they don't disconnect for some reason.
		GetTree().CreateTimer(GRACEFUL_DISCONNECT_DURATION_SECONDS).Timeout += () => {
			if (serverPacketPeer is not null && serverPacketPeer.IsActive())
				serverPacketPeer.PeerDisconnectNow();
		};
	}
}