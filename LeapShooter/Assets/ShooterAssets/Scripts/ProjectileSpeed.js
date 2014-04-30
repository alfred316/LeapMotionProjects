#pragma strict

var speed = 20.0;
var time = 0.0;

function Start () {

}

function Update () {

time += Time.deltaTime;
transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

if (time > 3)
{

Destroy(gameObject);

}

}

function OnCollisionEnter (projectileCollision : Collision)
{

Destroy(gameObject);

}