using Godot;
using System;

public partial class Finish : CanvasLayer
{

	private TextureButton _finishButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		_finishButton = GetNode<TextureButton>("%Finish");
		
		 _finishButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
