﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace org.flixel.physics
{
    public class CircleShape : Shape
    {
        public CircleShape()
        {
	        ShapeType = ShapeType.Circle;
	        _radius = 0.0f;
	        _p = Vector2.Zero;
        }

        /// Implement Shape.
        public override Shape Clone()
        {
            CircleShape shape = new CircleShape();
            shape.ShapeType = ShapeType;
            shape._radius = _radius;
            shape._p = _p;

            return shape;
        }

        /// @see Shape.TestPoint
        public override bool TestPoint(ref Transform transform, Vector2 p)
        {
            Vector2 center = transform.Position + MathUtils.Multiply(ref transform.R, _p);
	        Vector2 d = p - center;
	        return Vector2.Dot(d, d) <= _radius * _radius;
        }

        // Collision Detection in Interactive 3D Environments by Gino van den Bergen
        // From Section 3.1.2
        // x = s + a * r
        // norm(x) = radius
        public override bool RayCast(out RayCastOutput output, ref RayCastInput input, ref Transform transform)
        {
            output = new RayCastOutput();

	        Vector2 position = transform.Position + MathUtils.Multiply(ref transform.R, _p);
	        Vector2 s = input.p1 - position;
	        float b = Vector2.Dot(s, s) - _radius * _radius;

	        // Solve quadratic equation.
            Vector2 r = input.p2 - input.p1;
	        float c =  Vector2.Dot(s, r);
	        float rr = Vector2.Dot(r, r);
	        float sigma = c * c - rr * b;

	        // Check for negative discriminant and short segment.
	        if (sigma < 0.0f || rr < Settings.b2_epsilon)
	        {
                return false;
	        }

	        // Find the point of intersection of the line with the circle.
	        float a = -(c + (float)Math.Sqrt((double)sigma));

	        // Is the intersection point on the segment?
            if (0.0f <= a && a <= input.maxFraction * rr)
	        {
		        a /= rr;
                output.fraction = a;
                Vector2 norm = (s + a * r);
                norm.Normalize();
                output.normal = norm;
                return true;
            }

            return false;
        }

        /// @see Shape.ComputeAABB
        public override void ComputeAABB(out AABB aabb, ref Transform transform)
        {
            Vector2 p = transform.Position + MathUtils.Multiply(ref transform.R, _p);
	        aabb.lowerBound = new Vector2(p.X - _radius, p.Y - _radius);
	        aabb.upperBound = new Vector2(p.X + _radius, p.Y + _radius);
        }

        /// @see Shape.ComputeMass
        public override void ComputeMass(out MassData massData, float density)
        {
            massData.mass = density * Settings.b2_pi * _radius * _radius;
	        massData.center = _p;

	        // inertia about the local origin
	        massData.I = massData.mass * (0.5f * _radius * _radius + Vector2.Dot(_p, _p));
        }

        /// Get the supporting vertex index in the given direction.
        public override int GetSupport(Vector2 d)
        {
            return 0;
        }

        /// Get the supporting vertex in the given direction.
        public override Vector2 GetSupportVertex(Vector2 d)
        {
            return _p;
        }

        /// Get the vertex count.
        public override int GetVertexCount() { return 1; }

        /// Get a vertex by index. Used by b2Distance.
        public override Vector2 GetVertex(int index)
        {
            Debug.Assert(index == 0);
            return _p;
        }

        /// Position
        public Vector2 _p;
    }
}
