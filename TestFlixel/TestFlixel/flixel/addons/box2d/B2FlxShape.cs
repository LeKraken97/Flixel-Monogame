using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel.physics
{
	public class B2FlxShape : FlxSprite
	{
		private Shape shape;
		private FixtureDef fixtureDef;
		private BodyDef bodyDef;
		private Body body;

		public B2FlxShape (float x,float y,Shape shapeType) : base(x,y)
		{
			shape = shapeType;
			fixtureDef = new FixtureDef ();
			bodyDef = new BodyDef ();
			body = B2FlxWorld.world.CreateBody(bodyDef);
		}

		public B2FlxShape (float x,float y) : base(x,y)
		{
			shape = new PolygonShape();
		}

		public void makeBox(BodyType type,float width,float height,float elasticity,float friction,float mass)
		{
			shape = new PolygonShape();
			((PolygonShape)(shape)).SetAsBox (width, height);

			fixtureDef.shape = shape;
			fixtureDef.restitution = elasticity;
			fixtureDef.friction = friction;
			fixtureDef.density = mass;

			bodyDef.type = type;
			bodyDef.position = new Vector2(x,y);

			body = B2FlxWorld.world.CreateBody(bodyDef);
			body.CreateFixture(fixtureDef);
		}

		public void makeCircle(BodyType type,float radius,float elasticity,float friction,float mass)
		{
			shape = new CircleShape();
			((CircleShape)(shape))._radius = radius;

			fixtureDef.shape = shape;
			fixtureDef.restitution = elasticity;
			fixtureDef.friction = friction;
			fixtureDef.density = mass;

			bodyDef.type = type;
			bodyDef.position = new Vector2(x,y);

			body = B2FlxWorld.world.CreateBody(bodyDef);
			body.CreateFixture(fixtureDef);
		}

		/*
		 * Is this shape static or dynamic?
		 */ 
		public void setShapeType(BodyType type)
		{
			bodyDef.type = type;
		}

		/*
		 *Set the shape of this object.Ex: Polygon,Circle
		 */
		public void setShapeType(Shape shape)
		{
			this.shape = shape;
			fixtureDef.shape = shape;
		}

		public void setPosition(Vector2 pos)
		{
			bodyDef.position = pos;
		}

		public void setPosition(float x,float y)
		{
			bodyDef.position.X = x;
			bodyDef.position.Y = y;	
		}

		public override void update ()
		{
			base.update ();
		}

		public void setElasticity(float e)
		{
			fixtureDef.restitution = e;
		}

		public void setMass(float m)
		{
			fixtureDef.density = m;
		}

		public void setFriction(float f)
		{
			fixtureDef.friction = f;
		}

		public Body getBody()
		{
			return body;
		}
		public BodyDef getBodyDef()
		{
			return bodyDef;
		}
		public FixtureDef getFixtureDef()
		{
			return fixtureDef;
		}
		public Shape getShape()
		{
			return shape;
		}
	}
}

