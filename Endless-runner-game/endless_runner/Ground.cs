using Godot;
using System;

public partial class Ground : Node2D
{
	[Export]
	private float _speed = -125.0f;



	private StaticBody2D _staticBody2D;
	private StaticBody2D _staticBody2D2;
	private int _texturewidth;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get elements
		_staticBody2D = GetNode<StaticBody2D>("Ground_1");
		_staticBody2D2 = GetNode<StaticBody2D>("Ground_2");
		
		//This part works, when picture is not scaled manually
		//From .staticBody2d we want create node, whit sprite value from "GroundSprite1 and get its texture and its widght"
		//_texturewidth = _staticBody2D.GetNode<Sprite2D>("GroundSprite").Texture.GetWidth();

		Sprite2D groundSprite = _staticBody2D.GetNode<Sprite2D>("GroundSprite");
		_texturewidth = (int)(groundSprite.Texture.GetSize().X * groundSprite.Scale.X);

		//_staticBody2D.GlobalPosition = _staticBody2D.GlobalPosition + new Vector2(x: _staticBody2D.GlobalPosition.X + _texturewidth, y: _staticBody2D.GlobalPosition.Y);
		_staticBody2D2.GlobalPosition = _staticBody2D2.GlobalPosition with { X = _staticBody2D.GlobalPosition.X + _texturewidth};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(_texturewidth);
		_staticBody2D.GlobalPosition += new Vector2(_speed * (float)delta, 0);
		_staticBody2D2.GlobalPosition += new Vector2(_speed * (float)delta, 0);

		if (_staticBody2D.GlobalPosition.X < -+_texturewidth) {
			_staticBody2D.GlobalPosition = _staticBody2D2.GlobalPosition + new Vector2(_texturewidth, 0);
			GD.Print(_texturewidth);

		}
		if (_staticBody2D2.GlobalPosition.X < -+_texturewidth) {
			_staticBody2D2.GlobalPosition = _staticBody2D.GlobalPosition + new Vector2(_texturewidth, 0);
		}
	}
}
