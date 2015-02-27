using System.Diagnostics;

namespace org.flixel.physics
{
    internal class CircleContact : Contact
    {
	    internal CircleContact(Fixture fixtureA, Fixture fixtureB)
            : base(fixtureA, fixtureB)
        {
	        Debug.Assert(_fixtureA.ShapeType == ShapeType.Circle);
	        Debug.Assert(_fixtureB.ShapeType == ShapeType.Circle);
        }

	    internal override void Evaluate(ref Manifold manifold, ref Transform xfA, ref Transform xfB)
        {
	        Collision.CollideCircles(ref manifold,
						        (CircleShape)_fixtureA.GetShape(), ref xfA,
                                (CircleShape)_fixtureB.GetShape(), ref xfB);
        }
    }
}
