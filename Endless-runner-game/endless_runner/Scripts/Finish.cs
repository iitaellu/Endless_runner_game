using Godot;
using System;

public partial class Finish : CanvasLayer
{

	private Label _first;
    private Label _second;
	private Label _third;
	
	private GlobalScore _globalScore;

	private TextureButton _finishButton;
	private TextureButton _startAgainButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		_finishButton = GetNode<TextureButton>("%Finish");
		_startAgainButton = GetNode<TextureButton>("%StartNext");
		_first = GetNode<Label>("%1place");
        _second = GetNode<Label>("%2place");
		_third = GetNode<Label>("%3place");

		_globalScore = GetNode<GlobalScore>("/root/GlobalScore");

		// Save latest score if not already
		//_globalScore.SaveScore();

		var scores = _globalScore.TopScores;
		
		_first.Text = $"1st place: {scores[0]}";
        _second.Text = $"2nd place: {scores[1]}";
        _third.Text = $"3rd place: {scores[2]}";

		_finishButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
		_startAgainButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
    }
}
