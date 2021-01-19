using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
// currently unused
    public class Trampoline : Grabable
    {
        public override void OnDrop()
        {
        }

        public override void OnGrab()
        {
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Rigidbody2D r = collision.GetComponent<Rigidbody2D>();
            if (r != null)
            {
                r.velocity = new Vector2(r.velocity.x, -r.velocity.y);
            }
        }
        private void OnTriggerEnter(Collider collision)
        {
            Rigidbody2D r = collision.GetComponent<Rigidbody2D>();
            if(r != null)
            {
                r.velocity = new Vector2(r.velocity.x, -r.velocity.y-10f);
                r.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            }
        }
    }
}
