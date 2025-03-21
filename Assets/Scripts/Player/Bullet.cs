using UnityEngine;

public class Bullet : MonoBehaviour
{
    //tells the bullet how fast to move
    public float speed = 2000f;
    //tells the bullet how long to live
    public float lifeTime = 2f;
    //tells the bullet how much damage to do
    public int damage = 1;

    //tells the bullet the target
    public Vector3 target;
}
