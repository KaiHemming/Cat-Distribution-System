using Godot;
using System;

public partial class player : Node2D
{
	private AnimatedSprite2D animatedSprite2D;
	public float Acceleration { get; set; } = 3000;
	public float ImpulseDamping { get; set; } = 12f;
	public float StopDamping { get; set; } = 15f;
	public int MaxSpeed { get; set; } = 300;
	public String Colour = "Black";
	private Vector2 PressDirection = Vector2.Zero;
	private Vector2 Velocity = Vector2.Zero;
	private Vector2 ScreenSize;
	private AnimatedSprite2D Sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite = GetNode<Area2D>("Cat").GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		ScreenSize = GetViewportRect().Size;
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Movement
		Vector2 movement = getMovement();
		if (movement.Length() > 0) 
		{
			Velocity += movement.Normalized() * (float)(Acceleration * delta);
			if (Velocity.Length() > MaxSpeed) 
			{
				Velocity *= (float) (1 - (ImpulseDamping * delta));
			}
		}
		else 
		{
			Velocity *= (float) (1 - (StopDamping * delta));
			if (Velocity.Length() < 10)
			{
				Velocity = Vector2.Zero;
			}
		}
		Position += Velocity * (float)delta;

		// Animation
		if (movement.Length() > 0) 
		{
			if (movement.Y > 0) {
				if (movement.X != 0) {
					Sprite.Play(Colour + "_Diagonal_Towards");
					Sprite.FlipH = movement.X < 0;
				} else Sprite.Play(Colour + "_Walk_Down");
			}
			else if (movement.Y < 0) {
				if (movement.X != 0) {
					Sprite.Play(Colour + "_Diagonal_Away");
					Sprite.FlipH = movement.X < 0;
				} else Sprite.Play(Colour + "_Walk_Up");
			}
			else {
				Sprite.Play(Colour + "_Walk_Horizontal");
				Sprite.FlipH = movement.X > 0;
			}
		} 
		else {
			Sprite.Stop();
			// if (Sprite.Animation != Colour + "_Sit") Sprite.Play(Colour + "_Sit");
		}
	}

	private Vector2 getMovement() 
	{
		Vector2 movement = Vector2.Zero;

		if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_left"))
		{
			if (Input.IsActionJustPressed("ui_right"))
				PressDirection.X = 1;
			if (Input.IsActionJustPressed("ui_left"))
				PressDirection.X = -1;
			movement.X += PressDirection.X;
		}
		else if (Input.IsActionPressed("ui_right"))
			movement += Vector2.Right;
		else if (Input.IsActionPressed("ui_left"))
			movement += Vector2.Left;
		else
			PressDirection.Y = 0;

		if (Input.IsActionPressed("ui_down") && Input.IsActionPressed("ui_up"))
		{
			if (Input.IsActionJustPressed("ui_down"))
				PressDirection.Y = 1;
			if (Input.IsActionJustPressed("ui_up"))
				PressDirection.Y = -1;
			movement.Y += PressDirection.Y;
		}
		else if (Input.IsActionPressed("ui_down"))
			movement += Vector2.Down;
		else if (Input.IsActionPressed("ui_up"))
			movement += Vector2.Up;
		else
			PressDirection.Y = 0;

		return movement;
	}
}
