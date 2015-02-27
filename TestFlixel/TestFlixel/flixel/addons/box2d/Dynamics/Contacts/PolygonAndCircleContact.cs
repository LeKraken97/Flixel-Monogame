using System.Diagnostics;

namespace org.flixel.physics
{
    internal class PolygonAndCircleContact : Contact
    {
	    internal PolygonAndCircleContact(Fixture fixtureA, Fixture fixtureB)
            : base(fixtureA, fixtureB)
        {
            Debug.Assert(_fixtureA.ShapeType == ShapeType.Polygon);
	        Debug.Assert(_fixtureB.ShapeType == ShapeType.Circle);
        }

        internal override void Evaluate(ref Manifold manifold, ref Transform xfA, ref Transform xfB)
        {
	        Collision.CollidePolygonAndCircle(ref manifold,
                                        (PolygonShape)_fixtureA.GetShape(), ref xfA,
                                        (CircleShape)_fixtureB.GetShape(), ref xfB);
        }
    }
}
