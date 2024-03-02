using Godot;
using GodotExtensions.extensions.node;

namespace SaveTheWorld.scenes.SplashScreen;

public partial class Splash : Control {
	[Export(PropertyHint.File)] string gamePackedScenePath;

	[Export] bool forcePlayersToWatchSplashScreen = true;
	ProgressBar progressBar;
	VideoStreamPlayer video;


	public  override async void _Ready() {
		progressBar = GetNode<ProgressBar>("Progress");
		video = this.FindFirstChildOfClass<VideoStreamPlayer>();
	
		GD.Print("Loading game.");

		if (forcePlayersToWatchSplashScreen) {
			video.Finished += () => {
				GD.Print("Video finished");
				ChangeToGameScene();
			};
			return;
		}

		await this.WaitForProcessFrame();

		if (OS.IsDebugBuild() || !forcePlayersToWatchSplashScreen) {
			ChangeToGameScene();
		}
		else {
			float funnyAmount = 1;

			// make the progress bar go higher artificially, to make it look like it's doing something.
			void Handler() {
				progressBar.Value += GetProcessDeltaTime() * funnyAmount;
				var next = (float)Mathf.Abs(GD.Randfn(4, 10));
				funnyAmount = Mathf.MoveToward(funnyAmount, next, 25f * (float)GetProcessDeltaTime());
				bool completed = progressBar.Value >= progressBar.MaxValue;
				if (completed) {
					GetTree().ProcessFrame -= Handler;
					GetTree().ChangeSceneToFile(gamePackedScenePath);
				}
			}

			GetTree().ProcessFrame += Handler;
		}
	}

	void ChangeToGameScene() {
		var err = GetTree().ChangeSceneToFile(gamePackedScenePath);
		if (err != Error.Ok)
			GD.PrintErr("Error loading game: " + err);
	}
	
	void ProgressChanged(int percentage) {
		progressBar.Value = percentage;
	}

	public override void _Process(double delta) {
		progressBar.Ratio = progressBar.Ratio + 0.025f * (float)delta;
	}
}