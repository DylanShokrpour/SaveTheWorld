using System.Threading.Tasks;
using Godot;
using GodotExtensions.extensions.node;

public partial class LocationLabel : RichTextLabel {
	[Export] Tween.TransitionType transitionType = Tween.TransitionType.Expo;
	[Export] string Font = "[center]";

	Tween tween;
	//This is a label that will be used to introduce the level(I AM A TROLL OK)

	public async Task ShowLocationAnimation() {
		await DropDown();
		await this.WaitForSeconds(2);
		await RetractUp(); // await does not really matter on the last one.
	}

	async Task DropDown() {
		Text = Font + GetOwner<Node3D>().Name;
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(this, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(Position.X, 20),
			2);
		await ToSignal(tween, Tween.SignalName.Finished);
	}

	async Task RetractUp() {
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(this, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(Position.X, -100),
			2);
		await ToSignal(tween, Tween.SignalName.Finished);
	}

	void RefreshTween() {
		if (tween != null) {
			tween.Stop();
			tween.Kill();
		}

		tween = CreateTween();
	}
}