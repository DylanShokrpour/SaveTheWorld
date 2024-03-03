using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

public partial class ChangeMapButton : TextureButton {
	TextureRect editedImage;
	[Export] Tween.TransitionType transitionType = Tween.TransitionType.Expo;
	[Export] bool direction; // goes right if true, goes left if false
	Tween tween;
	//This is a label that will be used to introduce the level(I AM A TROLL OK)

	public override void _Ready() {
		editedImage = GetNode<TextureRect>("%CurrentImage");
		Pressed += ShowLocationAnimation;
		//Color = new Color(1, 1, 1,1);
	}

	public async void ShowLocationAnimation() {
		await GoOffScreen();
		await this.WaitForSeconds(0.1f);
		GetOwner<WorldSelector>().EmitSignal("onLevelChange", direction); // await does not really matter on the last one.
		ComeOnScreen();
	}

	async Task GoOffScreen() {
		int x = 1500;
		if (direction == false) {
			x = -1500;
		}
		GD.Print(GlobalPosition.Y);
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(editedImage, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(x, 0),
			1);
		await ToSignal(tween, Tween.SignalName.Finished);
		GD.Print(Position.Y);
		GD.Print(GlobalPosition.Y);
	}


	async Task ComeOnScreen() {
		int x = -1500;
		if (direction == false) {
			x = 1500;
		}
		editedImage.GlobalPosition = new Vector2(x, 0); // when off screen it returns back on the opposite side
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(editedImage, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(0, 0),
			1);
		await ToSignal(tween, Tween.SignalName.Finished);
		// Return back to normal
	}

	void RefreshTween() {
		if (tween != null) {
			tween.Stop();
			tween.Kill();
		}

		tween = CreateTween();
	}
}