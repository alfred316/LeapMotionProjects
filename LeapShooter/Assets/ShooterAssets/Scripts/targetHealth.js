#pragma strict

public var targetHealth = 2.0;
public static var score = 0;

function Update () {

if (targetHealth <= 0.0)
{

Destroy(gameObject);
score += 1;

}

}

function OnCollisionEnter(projectileCollision : Collision)
{

if (projectileCollision.transform.name == ("bullet(Clone)"))
{


targetHealth -=1;


}

}

