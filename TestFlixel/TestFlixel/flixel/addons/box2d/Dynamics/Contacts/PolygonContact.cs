using System.Diagnostics;

namespace org.flixel.physics
{
    internal class PolygonContact : Contact
    {
	    internal PolygonContact(Fixture fixtureA, Fixture fixtureB)
            : base(fixtureA, fixtureB)
        {
            Debug.Assert(_fixtureA.ShapeType == ShapeType.Polygon);
            Debug.Assert(_fixtureB.ShapeType == ShapeType.Polygon);
        }

        internal override void Evaluate(ref Manifold manifold, ref Transform xfA, ref Transform xfB)
        {
	        Collision.CollidePolygons(ref manifold,
                        (PolygonShape)_fixtureA.GetShape(), ref xfA,
                        (PolygonShape)_fixtureB.GetShape(), ref xfB);
        }
    }
}
