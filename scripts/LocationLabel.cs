using Godot;
using GodotExtensions.extensions.node;

public partial class LocationLabel : RichTextLabel {
	[Export] Tween.TransitionType transitionType = Tween.TransitionType.Expo;
	[Export] string Font = "[center]";

	Tween tween;
	//This is a label that will be used to introduce the level(I AM A TROLL OK)

	public override void _Ready() {
		//Color = new Color(1, 1, 1,1);
	}

	public void Activate() {
		Text = Font + GetOwner<Node3D>().Name;
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(this, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(Position.X, 20),
			2);
		DeActivate();
	}

	async void DeActivate() {
		await this.YieldForSeconds(2);
		RefreshTween();
		tween.SetTrans(transitionType);
		tween.TweenProperty(this, Control.PropertyName.GlobalPosition.ToString(),
			new Vector2(Position.X, -100),
			2);
	}

	void RefreshTween() {
		if (tween != null) {
			tween.Stop();
			tween.Kill();
		}

		tween = CreateTween();
	}
}