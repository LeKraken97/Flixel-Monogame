﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace org.flixel.physics
{
    internal struct TOIConstraint
    {
	    public FixedArray2<Vector2> localPoints;
        public Vector2 localNormal;
        public Vector2 localPoint;
        public ManifoldType type;
        public float radius;
        public int pointCount;
        public Body bodyA;
        public Body bodyB;
    }

    internal struct TOISolverManifold
    {
        public TOISolverManifold(ref TOIConstraint cc, int index)
	    {
		    Debug.Assert(cc.pointCount > 0);

		    switch (cc.type)
		    {
		    case ManifoldType.Circles:
			    {
				    Vector2 pointA = cc.bodyA.GetWorldPoint(cc.localPoint);
				    Vector2 pointB = cc.bodyB.GetWorldPoint(cc.localPoints[0]);
                    if ((pointA - pointB).LengthSquared() > Settings.b2_epsilon * Settings.b2_epsilon)
				    {
					    normal = pointB - pointA;
					    normal.Normalize();
				    }
				    else
				    {
					    normal = new Vector2(1.0f, 0.0f);
				    }

				    point = 0.5f * (pointA + pointB);
				    separation = Vector2.Dot(pointB - pointA, normal) - cc.radius;
			    }
			    break;

		    case ManifoldType.FaceA:
			    {
				    normal = cc.bodyA.GetWorldVector(cc.localNormal);
				    Vector2 planePoint = cc.bodyA.GetWorldPoint(cc.localPoint);

				    Vector2 clipPoint = cc.bodyB.GetWorldPoint(cc.localPoints[index]);
				    separation = Vector2.Dot(clipPoint - planePoint, normal) - cc.radius;
				    point = clipPoint;
			    }
			    break;

		    case ManifoldType.FaceB:
			    {
				    normal = cc.bodyB.GetWorldVector(cc.localNormal);
				    Vector2 planePoint = cc.bodyB.GetWorldPoint(cc.localPoint);

				    Vector2 clipPoint = cc.bodyA.GetWorldPoint(cc.localPoints[index]);
				    separation = Vector2.Dot(clipPoint - planePoint, normal) - cc.radius;
				    point = clipPoint;

				    // Ensure normal points from A to B
				    normal = -normal;
			    }
			    break;
            default:
                normal = Vector2.UnitY;
                point = Vector2.Zero;
                separation = 0.0f;
                break;
		    }
	    }

        internal Vector2 normal;
        internal Vector2 point;
        internal float separation;
    };


    internal class TOISolver
    {
        public TOISolver() { }

        public void Initialize(Contact[] contacts, int count, Body toiBody)
        {
            _count = count;
            _toiBody = toiBody;
#warning "remove alloc"
            _constraints = new TOIConstraint[_count];

            for (int i = 0; i < _count; ++i)
            {
                Contact contact = contacts[i];

                Fixture fixtureA = contact.GetFixtureA();
                Fixture fixtureB = contact.GetFixtureB();
                Shape shapeA = fixtureA.GetShape();
                Shape shapeB = fixtureB.GetShape();
                float radiusA = shapeA._radius;
                float radiusB = shapeB._radius;
                Body bodyA = fixtureA.GetBody();
                Body bodyB = fixtureB.GetBody();
                Manifold manifold;
                contact.GetManifold(out manifold);

                Debug.Assert(manifold._pointCount > 0);

                TOIConstraint constraint = _constraints[i];
                constraint.bodyA = bodyA;
                constraint.bodyB = bodyB;
                constraint.localNormal = manifold._localNormal;
                constraint.localPoint = manifold._localPoint;
                constraint.type = manifold._type;
                constraint.pointCount = manifold._pointCount;
                constraint.radius = radiusA + radiusB;

                for (int j = 0; j < constraint.pointCount; ++j)
                {
                    constraint.localPoints[j] = manifold._points[j].LocalPoint;
                }

                _constraints[i] = constraint;
            }
        }

        // Perform one solver iteration. Returns true if converged.
        public bool Solve(float baumgarte)
        {
            float minSeparation = 0.0f;

            for (int i = 0; i < _count; ++i)
            {
                TOIConstraint c = _constraints[i];
                Body bodyA = c.bodyA;
                Body bodyB = c.bodyB;

                float massA = bodyA._mass;
                float massB = bodyB._mass;

                // Only the TOI body should move.
                if (bodyA == _toiBody)
                {
                    massB = 0.0f;
                }
                else
                {
                    massA = 0.0f;
                }

                float invMassA = massA * bodyA._invMass;
                float invIA = massA * bodyA._invI;
                float invMassB = massB * bodyB._invMass;
                float invIB = massB * bodyB._invI;

                // Solve normal constraints
                for (int j = 0; j < c.pointCount; ++j)
                {
                    TOISolverManifold psm = new TOISolverManifold(ref c, j);

                    Vector2 normal = psm.normal;
                    Vector2 point = psm.point;
                    float separation = psm.separation;

                    Vector2 rA = point - bodyA._sweep.c;
                    Vector2 rB = point - bodyB._sweep.c;

                    // Track max constraint error.
                    minSeparation = Math.Min(minSeparation, separation);

                    // Prevent large corrections and allow slop.
                    float C = MathUtils.Clamp(baumgarte * (separation + Settings.b2_linearSlop), -Settings.b2_maxLinearCorrection, 0.0f);

                    // Compute the effective mass.
                    float rnA = MathUtils.Cross(rA, normal);
                    float rnB = MathUtils.Cross(rB, normal);
                    float K = invMassA + invMassB + invIA * rnA * rnA + invIB * rnB * rnB;

                    // Compute normal impulse
                    float impulse = K > 0.0f ? -C / K : 0.0f;

                    Vector2 P = impulse * normal;

                    bodyA._sweep.c -= invMassA * P;
                    bodyA._sweep.a -= invIA * MathUtils.Cross(rA, P);
                    bodyA.SynchronizeTransform();

                    bodyB._sweep.c += invMassB * P;
                    bodyB._sweep.a += invIB * MathUtils.Cross(rB, P);
                    bodyB.SynchronizeTransform();
                }
            }

            // We can't expect minSpeparation >= -b2_linearSlop because we don't
            // push the separation above -b2_linearSlop.
            return minSeparation >= -1.5f * Settings.b2_linearSlop;
        }

	    TOIConstraint[] _constraints;
	    int _count;
	    Body _toiBody;
    }
}
