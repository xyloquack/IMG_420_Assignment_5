using Godot;
public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 200f;
	public override void _PhysicsProcess(double delta)
	{
		int direction = 0;
		if (Input.IsActionPressed("left"))
		{
			direction -= 1;
		}
		if (Input.IsActionPressed("right"))
		{
			direction += 1;
		}
		
		Vector2 velocity = Velocity;
		velocity.X = direction * Speed;
		
		if (!IsOnFloor())
		{
			velocity.Y += 1000f * (float)delta;
		}
		else if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.Y = -1000f;
			GD.Print("Jump!");
		}
		else
		{
			velocity.Y = 0;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
}
