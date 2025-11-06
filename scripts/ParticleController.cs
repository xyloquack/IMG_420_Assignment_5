using Godot;
public partial class ParticleController : GpuParticles2D
{
	private ShaderMaterial _shaderMaterial;
	private float _time_passed = 0f;
	public override void _Ready()
	{
		_shaderMaterial = (ShaderMaterial)Material;
	}
	public override void _Process(double delta)
	{
		_time_passed += (float)delta;
		_shaderMaterial.SetShaderParameter("time_passed", _time_passed);
	}
}
