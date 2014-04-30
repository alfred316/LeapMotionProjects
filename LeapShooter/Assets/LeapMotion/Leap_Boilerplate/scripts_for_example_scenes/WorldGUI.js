#pragma strict

function Start () {

}

function Update () {

}

function OnGUI() {

var scoreNumber = targetHealth.score;
var scoreName = scoreNumber.ToString();

GUI.Box(Rect(300,30,50,25), scoreName);
GUI.Box(Rect(300, 10, 50, 50), "Score");

}
