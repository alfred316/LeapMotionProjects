﻿#pragma strict

public var targetHealth = 2.0;
public static var score = 0;
//var destroyedParticles : Transform;

function Update () {

if (targetHealth <= 0.0)
{
//Instantiate(destroyedParticles, transform.position, transform.rotation);
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

