using Godot;
using System;

namespace ErkbergsGodotLibrary
{
    public partial class FirstPersonCharacter3D : CharacterBody3D
    {
        [ExportGroup("Input")]
        [Export] protected string moveLeftAction = "MoveLeft";
        [Export] protected string moveRightAction = "MoveRight";
        [Export] protected string moveUpAction = "MoveUp";
        [Export] protected string moveDownAction = "MoveDown";
        [Export] protected string jumpAction = "Jump";

        [ExportGroup("Refs")]
        [Export] protected Node3D camPivot;
        [Export] protected Camera3D cam;

        [ExportGroup("Timers")]
        [Export] protected Timer coyoteTimer;
        [Export] protected Timer jumpBufferTimer;

        [ExportGroup("Config")]
        [Export] protected float maxCamAngle = 86;

        [ExportSubgroup("Move")]
        [Export] protected float moveSpeed = 8;
        [Export] protected float accelerationFloor = 32;
        [Export] protected float accelerationAir = 8;
        [Export] protected float decelerationFloor = 32;
        [Export] protected float decelerationAir = 8;

        [ExportSubgroup("Jump")]
        [Export] protected float jumpHeight = 1;
        [Export] protected float jumpVelocityMultiplier = 1.2f;
        [Export] protected float fallMultiplier = 2;

        [ExportSubgroup("Look")]
        [Export] protected float mouseSensitivity = 0.1f;
        [Export] protected bool captureMouse = true;

        protected float jumpStrength;
        protected Vector2 mouseMotion;
        protected Vector3 previousVelocity;

        protected float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

        public override void _Ready()
        {
            jumpStrength = GetJumpStrengthFromHeight(jumpHeight);

            if (captureMouse)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }

        public override void _Process(double delta)
        {
            Gravity(delta);
            Jump(delta);
            Move(delta);
            Look(delta);
            MoveAndSlide();

            previousVelocity = Velocity;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion)
            {
                mouseMotion = -(@event as InputEventMouseMotion).Relative;
            }
        }

        protected virtual void Move(double delta)
        {
            Vector3 velocity = Velocity;

            // Get input dir and handle movement / deceleration
            Vector2 inputDir = Input.GetVector(moveLeftAction, moveRightAction, moveUpAction, moveDownAction);
            Vector2 velo2D = new Vector2(velocity.X, velocity.Z);
            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
            Vector2 dir2D = new Vector2(direction.X, direction.Z);

            if (direction != Vector3.Zero)
            {
                float acceleration = IsOnFloor() ? accelerationFloor : accelerationAir;
                velo2D = velo2D.MoveToward(dir2D * moveSpeed, acceleration * (float)delta);
            }
            else
            {
                float deceleration = IsOnFloor() ? decelerationFloor : decelerationAir;
                velo2D = velo2D.MoveToward(Vector2.Zero, deceleration * (float)delta);
            }

            velocity.X = velo2D.X;
            velocity.Z = velo2D.Y;
            Velocity = velocity;
        }

        protected virtual void Jump(double delta)
        {
            Vector3 velocity = Velocity;

            // Handle jump and doublejump
            // (max height = velocity^2 / 2g, velocity = sqrt(max height * 2 * g))
            if (Input.IsActionPressed(jumpAction))
            {
                if (IsOnFloor())
                {
                    // regular jump from floor
                    DoJump();
                }
                else if (CanUseCoyoteJump(velocity.Y))
                {
                    // coyote jump
                    coyoteTimer.Stop();
                    DoJump();
                }
                else if (jumpBufferTimer.IsStopped())
                {
                    jumpBufferTimer.Start();
                }
            }

            // check jump buffer
            if (IsOnFloor())
            {
                if (previousVelocity.Y < 0)
                {
                    if (!jumpBufferTimer.IsStopped())
                    {
                        DoJump();
                    }
                    else
                    {
                        // just landed
                    }
                }

                coyoteTimer.Start();
                jumpBufferTimer.Stop();
            }
        }

        protected virtual void DoJump()
        {
            Vector3 velocity = Velocity;
            velocity *= jumpVelocityMultiplier;
            velocity.Y = jumpStrength;
            Velocity = velocity;
        }

        protected bool CanUseCoyoteJump(float velocityY)
        {
            return !coyoteTimer.IsStopped() && velocityY <= 0;
        }

        protected virtual void Gravity(double delta)
        {
            Vector3 velocity = Velocity;

            if (!IsOnFloor())
            {
                if (velocity.Y >= 0)
                {
                    velocity.Y -= gravity * (float)delta;
                }
                else
                {
                    velocity.Y -= gravity * (float)delta * fallMultiplier;
                }
            }

            Velocity = velocity;
        }

        protected virtual void Look(double delta)
        {
            Vector2 lookDir = mouseMotion * mouseSensitivity * (float)delta;
            mouseMotion = Vector2.Zero;
            RotateY(lookDir.X);
            camPivot.RotateX(lookDir.Y);
            float clampedX = Mathf.Clamp(camPivot.RotationDegrees.X, -maxCamAngle, maxCamAngle);
            camPivot.RotationDegrees = new Vector3(clampedX, camPivot.RotationDegrees.Y, camPivot.RotationDegrees.Z);
        }

        protected float GetJumpStrengthFromHeight(float jumpHeight)
        {
            return Mathf.Sqrt(jumpHeight * 2 * gravity);
        }

        protected Vector3 GetCamForward()
        {
            return -camPivot.GlobalTransform.Basis.Z;
        }
    }
}