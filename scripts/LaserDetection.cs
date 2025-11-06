using Godot;
public partial class LaserDetection : Node2D
{
	[Export] public float LaserLength = 500f;
	[Export] public Color LaserColorNormal = Colors.Green;
	[Export] public Color LaserColorAlert = Colors.Red;
	[Export] public NodePath PlayerPath;
	[Export] public Vector2 LaserDirection = Vector2.Down;
	private RayCast2D _rayCast;
	private bool _isColliding;
	private Line2D _laserBeam;
	private CharacterBody2D _player;
	private bool _isAlarmActive = false;
	private Timer _alarmTimer;
	private float _alarmStrength;
	private ColorRect _alarmFeedback;
	private ShaderMaterial _alarmShader;
	
	public override void _Ready()
	{
		_alarmTimer = new Timer();
		_player = GetParent().GetNode<CharacterBody2D>("Player");
		_alarmFeedback = GetNode<ColorRect>("AlarmCanvas/AlarmFeedback");
		_alarmShader = (ShaderMaterial)_alarmFeedback.Material;
		_alarmTimer.WaitTime = 5f;
		_alarmTimer.OneShot = true;
		_alarmTimer.Timeout += ResetAlarm;
		AddChild(_alarmTimer);
		SetupRaycast();
		SetupVisuals();
	}
	
	private void SetupRaycast()
	{
		_rayCast = new RayCast2D();
		_rayCast.TargetPosition = LaserDirection.Normalized() * LaserLength;
		_rayCast.CollisionMask = _player.CollisionMask;
		AddChild(_rayCast);
	}
	
	private void SetupVisuals()
	{
		// TODO: Create Line2D for laser visualization
		// TODO: Set width and color
		// TODO: Add points for the line
		_laserBeam = new Line2D();
		_laserBeam.AddPoint(Vector2.Zero);
		_laserBeam.AddPoint(GlobalPosition - _rayCast.TargetPosition);
		_laserBeam.Width = 5f;
		_laserBeam.DefaultColor = LaserColorNormal;
		AddChild(_laserBeam);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_rayCast.ForceRaycastUpdate();
		_isColliding = _rayCast.IsColliding();
		UpdateLaserBeam();
		if (_isColliding)
		{
			if (_rayCast.GetCollider() == _player)
			{
				TriggerAlarm();
			}
		}
		
		if (!_alarmTimer.IsStopped())
		{
			Alarm(delta);
		}
	}
	
	private void UpdateLaserBeam()
	{
		if (_isColliding)
		{
			Vector2 collisionPosition = _rayCast.GetCollisionPoint();
			_laserBeam.SetPointPosition(1, collisionPosition - GlobalPosition);
		}
		else
		{
			_laserBeam.SetPointPosition(1, _rayCast.TargetPosition);
		}
	}
	
	private void TriggerAlarm()
	{
		_laserBeam.DefaultColor = LaserColorAlert;
		_alarmShader.SetShaderParameter("enable", true);
		_alarmTimer.Start();
		GD.Print("ALARM! Player detected!");
	}
	
	private void Alarm(double delta)
	{
		if (_alarmStrength <= 0f)
		{
			_alarmStrength = 1f;
		}
		_alarmStrength -= (float)delta;
		_alarmShader.SetShaderParameter("strength", _alarmStrength);
	}
	
	private void ResetAlarm()
	{
		_alarmShader.SetShaderParameter("enable", false);
		_alarmTimer.Stop();
		_laserBeam.DefaultColor = LaserColorNormal;
	}
}
