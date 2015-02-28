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

		public B2FlxShape (float x, float y) : base (x, y)
		{
		
		}

		public void makeBox(BodyType type,float width,float height,float elasticity,float friction,float mass)
		{
			shape = new PolygonShape();
			((PolygonShape)(shape)).SetAsBox (width, height);

			fixtureDef = new FixtureDef ();
			fixtureDef.shape = shape;
			fixtureDef.restitution = elasticity;
			fixtureDef.friction = friction;
			fixtureDef.density = mass;

			bodyDef = new BodyDef ();
			bodyDef.type = type;
			bodyDef.position = new Vector2(x,y);

			body = B2FlxWorld.world.CreateBody(bodyDef);
			body.CreateFixture(fixtureDef);
		}

		public void makeCircle(BodyType type,float radius,float elasticity,float friction,float mass)
		{
			shape = new CircleShape();
			((CircleShape)(shape))._radius = radius;

			fixtureDef = new FixtureDef ();
			fixtureDef.shape = shape;
			fixtureDef.restitution = elasticity;
			fixtureDef.friction = friction;
			fixtureDef.density = mass;

			bodyDef = new BodyDef ();
			bodyDef.type = type;
			bodyDef.position = new Vector2(x,y);

			body = B2FlxWorld.world.CreateBody(bodyDef);
			body.CreateFixture(fixtureDef);
		}

		/**
		 * Is this shape static or dynamic?
		 */ 
		public void setShapeType(BodyType type)
		{
			bodyDef.type = type;
		}

		/**
		 *Set the shape of this object.Ex: Polygon,Circle
		 */
		public void setShapeType(Shape shape)
		{
			this.shape = shape;
			fixtureDef.shape = shape;
		}

		/**
		 *Set the position of this Shape
		 */
		public void setPosition(Vector2 pos)
		{
			body.Position = pos;
		}

		public void move(Vector2 speed,float angle)
		{
			body.SetTransform (new Vector2 (body.Position.X + speed.X, body.Position.Y + speed.Y), angle);
		}
			
		public void move(Vector2 speed)
		{
			body.SetTransform (new Vector2 (body.Position.X + speed.X, body.Position.Y + speed.Y), 0);
		}
			
		public void move(float speedX,float speedY)
		{
			body.SetTransform (new Vector2 (body.Position.X + speedX, body.Position.Y + speedY), 0);
		}

		public override void update ()
		{
			base.x = body.Position.X;
			base.y = body.Position.Y;
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

