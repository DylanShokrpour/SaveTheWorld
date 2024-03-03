using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

public partial class ChangeMapButton : TextureButton {
	TextureRect editedImage;
	[Export] Tween.TransitionType transitionType = Tween.TransitionType.Expo;

	Tween tween;
	//This is a label that will be used to introduce the level(I AM A TROLL OK)

	public override void _Ready() {
		editedImage = GetNode<TextureRect>("%CurrentImage");
		Pressed += ShowLocationAnimation;
		//Color = new Color(1, 1, 1,1);
	}

	public async void ShowLocationAnimation() {
		await GoRight();
		await this.WaitForSeconds(1);
		GetOwner<WorldSelector>().EmitSignal("onLevelChange"); // await does not really matter on the last one.
		ComeOnScreen();
	}

	async Task GoRight() {
		GD.Print(GlobalPosition.Y);
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(editedImage, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(1500, 0),
			2);
		await ToSignal(tween, Tween.SignalName.Finished);
		GD.Print(Position.Y);
		GD.Print(GlobalPosition.Y);
	}


	async Task ComeOnScreen() {
		editedImage.GlobalPosition = new Vector2(-1500, 0); // when off screen it returns back on the opposite side
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(editedImage, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(0, 0),
			2);
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