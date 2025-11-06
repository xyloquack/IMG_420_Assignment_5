using Godot;
using System.Collections.Generic;

public partial class PhysicsChain : Node2D
{
	[Export] public int ChainSegments = 5;
	[Export] public float SegmentDistance = 20f;
	[Export] public PackedScene SegmentScene;
	private List<RigidBody2D> _segments = new List<RigidBody2D>();
	private List<Joint2D> _joints = new List<Joint2D>();
	
	[Export] public float SpringStiffness = 100f;
	[Export] public float SpringDamping = 1f;
	[Export] public float SpringRestLength = 0f;
	[Export] public bool DisableCollisionBetweenLinks = true;
	
	[Export] public float SegmentMass = 1f;
	[Export] public float LinearDamp = 1f;
	[Export] public float AngularDamp = 2f;
	
	public override void _Ready()
	{
		if (SegmentScene != null)
		{
			CallDeferred(MethodName.CreateChain);
		}
		else
		{
			GD.PrintErr("SegmentScene is not set in the inspector. Cannot create chain.");
		}
	}
	
	private void CreateChain()
	{
		RigidBody2D previousSegment = null;
		
		for (int i = 0; i < ChainSegments; i++)
		{
			RigidBody2D segment = SegmentScene.Instantiate<RigidBody2D>();
			segment.Position = new Vector2(0, i * SegmentDistance);
			
			segment.Mass = SegmentMass;
			segment.LinearDamp = LinearDamp;
			segment.AngularDamp = AngularDamp;
			
			AddChild(segment);
			_segments.Add(segment);
			
			if (i == 0)
			{
				segment.Freeze = true;
			}
			
			if (previousSegment != null)
			{
				DampedSpringJoint2D springJoint = new DampedSpringJoint2D();
				float restLength = SpringRestLength > 0 ? SpringRestLength : SegmentDistance;
				springJoint.Length = restLength;
				springJoint.RestLength = restLength;
				springJoint.Stiffness = SpringStiffness;
				springJoint.Damping = SpringDamping;
				
				if (DisableCollisionBetweenLinks)
				{
					springJoint.DisableCollision = true;
				}
				
				AddChild(springJoint);
				_joints.Add(springJoint);
				springJoint.NodeA = previousSegment.GetPath();
				springJoint.NodeB = segment.GetPath();
				
				springJoint.Position = previousSegment.Position + new Vector2(0, SegmentDistance / 2.0f);
				
				GD.Print($"Joint {i-1}: Type=DampedSpring, Length={restLength}, Stiffness={SpringStiffness}");
			}
			
			previousSegment = segment;
		}
	}
	
	public void ApplyForceToSegment(int segmentIndex, Vector2 force)
	{
		if (segmentIndex >= 0 && segmentIndex < _segments.Count)
		{
			_segments[segmentIndex].ApplyImpulse(force);
		}
	}
	
	public void OnBodyEntered(RigidBody2D segment, Node body) 
	{
		if (body is RigidBody2D otherBody && !_segments.Contains(otherBody))
		{
			Vector2 direction = segment.GlobalPosition - otherBody.GlobalPosition;
			Vector2 repulsionForce = direction.Normalized() * 500f;
			segment.CallDeferred(RigidBody2D.MethodName.ApplyCentralImpulse, repulsionForce);
			GD.Print("Collision!");
		}
	}
}
