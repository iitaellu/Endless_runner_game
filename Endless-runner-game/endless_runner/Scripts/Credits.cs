using Godot;
using System;

public partial class Credits : Node
{

	[Export]

	private TextureButton _backButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_backButton = GetNode<TextureButton>("%Back");
		_backButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
		
		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
