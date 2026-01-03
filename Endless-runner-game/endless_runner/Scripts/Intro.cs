using Godot;
using System;

public partial class Intro : Node
{

	private TextureButton _startButton;

	private AudioStreamPlayer2D _soundEffect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startButton = GetNode<TextureButton>("Panel/VBoxContainer2/TextureButton");
		_soundEffect = GetNode<AudioStreamPlayer2D>("Panel/VBoxContainer2/AudioStreamPlayer2D");
		//_backButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
		_startButton.Pressed += OnStartPressed;
	}

	 private async void OnStartPressed()
    {
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
