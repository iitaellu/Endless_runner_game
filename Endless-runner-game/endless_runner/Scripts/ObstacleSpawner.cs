using Godot;
using System;

public partial class ObstacleSpawner : Node
{


	[Export]
	private float _changeToSpawnHawk = 0.1f;

	private PackedScene _bushScene = GD.Load<PackedScene>("res://Scenes/bush.tscn");
	private PackedScene _hawkScene = GD.Load<PackedScene>("res://Scenes/hawk.tscn");

	private StaticBody2D _ground1;
	private StaticBody2D _ground2;

	private CollisionShape2D _collisionShape;
	private Sprite2D _sprite2D;

	private Timer _obstaclespawnTimer;
	private Node2D _spawnPoint;
	private Node main;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		main = GetNode<Node>("/root/Main");
		_spawnPoint = GetNode<Node2D>("SpawnPoint");
		_obstaclespawnTimer = GetNode<Timer>("Timer");
		_ground1 = GetParent().GetNode<StaticBody2D>("Ground_1");
		_ground2 = GetParent().GetNode<StaticBody2D>("Ground_2");

		//_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		//_sprite2D = GetNode<Sprite2D>("Sprite2D");

		_obstaclespawnTimer.Timeout += SpawnObstacle;

	}

private void SpawnObstacle(){

	Random random = new Random();
	float randomValue = (float)random.NextDouble();

	if (randomValue <= _changeToSpawnHawk){

		SpawnHawk();

	} else {
		SpawnBush();
	}
}

private void SpawnHawk(){
	var hawk = _hawkScene.Instantiate<Hawk>();

	main.AddChild(hawk);
	var positonY = GetViewport().GetVisibleRect().Size.Y - (float)GD.RandRange(190.0, 200.0);
	hawk.Position = new Vector2(_spawnPoint.Position.X, positonY);
}

private void SpawnBush(){
	var bush = _bushScene.Instantiate<Bush>();

	var parentGround = _ground1.GlobalPosition.X > _ground2.GlobalPosition.X ? _ground1 : _ground2;

	parentGround.AddChild(bush);

	//bush=_sprite2D;

	bush.GlobalPosition = new Vector2(_spawnPoint.Position.X, parentGround.GlobalPosition.Y - 100);
}

}
