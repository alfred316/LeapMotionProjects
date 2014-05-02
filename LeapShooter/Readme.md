Leap Motion Shooting Range Game
By: Alfred Shaker

Source Code: https://github.com/alfred316/LeapMotionProjects

This is a write-up/tutorial of my Leap Motion enabled shooting range game. Basically what this demo project is, is a representation of how the Leap Motion controller can be integrated into Unity3D games. I will go over my process of how I got it to work and by the end of this, you should be able to have your first Leap Motion game up and running and have a good idea of the Leap Motion API and how it works. 

First, you will need to go to the Leap Motion website, create a developer account, and download their SDK for your system (Windows, Mac, or Linux). The SDK contains examples for using the Leap with a variety of languages, including JavaScript for web apps, and C# for Unity games. There is also a sample example that demonstrates how the leap motion tracks your hand movement and gives you the co-ordinates at which your hands are positioned, their velocity, how many hands/fingers are up, amongst other things. The best way to get an idea of this would be to use the sample.html and use it in your web browser to get an idea how the Leap sees you. Using this code, you can make your game or app do whatever you want based on those values. 

Now that you are acquainted with the Leap, let’s go ahead and set up our Unity project. The Leap requires you to have Unity Pro so as to use plug-ins. But, have no fear, broke college student! There is a way to use it with the free version. Simply take the libLeap.dylib and libLeapCSharp.dylib files and copy them into your Unity Project’s ROOT directory. After that, take the LeapCSharp.NET3.5.dll and copy it itno your ASSETS folder. All of these folders are provided in the Leap SDK folder. You are now ready to use the Leap Motion API in your C# Unity code.
Note: The Leap only works in C# for Unity. Trying to use JavaScript in Unity to incorporate the Leap WILL NOT WORK.

Now, let’s get to the good stuff! First, create an empty game object and put the Leap Manager script on it. That also should be in the SDK folder, if not found there, download the Leap Boilerplate package and it will be in the script folder for that. Now, create a game object from which you would like to shoot your bullets/objects. We will use two C# scripts that will incorporate the Leap Motion into our awesome game. Call them whatever you want, but make it something you can look at later and remember what they were for (so don’t call it “stuff 1” and “stuff 2”). I have mine set up to fire from a circle-like crosshair, which is basically a transparent image of a circle I drew up in my completely legal copy of Photoshop CS5 and put on a small plane that I tilted to have it look as if it were a gun’s crosshair. We now need to position this game object with respect to our camera so that it is in the center of our view. The script we will use for this is going to be modeled after my CrossHairScript in my github. Please refer to that repository as we go, as it contains well commented and formatted code that should help you along the way. I will put snippets of the code here for convenience. First thing’s first, DON’T forget to add this line to your “using” lines: 

using Leap;

this will enable you to use the Leap Motion API from the lib folders and Leap Manager we added earlier. Now you’re all set up to really start coding this!

	The first thing we have to do, now that we are all set up, is to create an instance of the Controller class, which basically lets you access everything in the Leap API from frames, to gestures, and so on. Declare variables for your “Controller”, “Main Camera”, and “Leap Manager”,  and chDist(the distance of your crosshair from the camera), like so:

	public float chDist = 10;
	Controller controller;
    	private Camera mainCam;
    	private LeapManager leapManager;

Now, in our void Start () function, we will initialize these variables so that they are doing meaningful things in our code. 

mainCam = (GameObject.FindGameObjectWithTag("MainCamera")as GameObject).GetComponent(typeof(Camera)) as Camera;
leapManager = (GameObject.Find("LeapManager")as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;
controller = new Controller();

What this code does is basically initialize the mainCam variable to find the MainCamera object in your scene and use it so as to make sure the crosshair is always a certain distance away from the camera (in later part of the code). Initializing the leapManager lets us use an instance of that class so that we can detect pointers and fingers and such so as to use them to control this game object. And finally the controller is initialized so as to show an example of it’s initialization, but since we are using the leapManager in this script, we will not be using it. We will, however, use it in our next script. 

	Next we look at our update function, where we will implement the rest of this code to make the motion of our finger control where this crosshair (or whatever you choose for a shooting instantiator) moves. The function is pretty simple, it consists of an if-else statement that first checks if the leapManager is available and then finds out if a pointer (a slim object or your finger) is available, and if that’s true, it renders the game object, otherwise it will not render it and the crosshair will be invisible. After that it sets it a certain distance from the camera (based on your chDist variable). After that it changes it’s transform to follow the pointer/finger around using a function from the leapManager class. The code is as follows:

	if (leapManager != null){

            if (leapManager.pointerAvailible){

                leapManager.screenToWorldDistance = chDist;
                this.transform.position = leapManager.pointerPositionScreenToWorld;
                if (!renderer.enabled) renderer.enabled = true;

            }
            else
                renderer.enabled = false;

        }

Great! If you did that all correctly, the crosshair should follow your finger around. This script was an example of using a pre-built class like the Leap Manager to handle most things for you using it’s predefined functions. Now, for our shooting script, named “ShooterObject.cs”, we will call on the lib files and make the code ourselves so that we can learn how to use the Leap Motion API. Both ways are feasible and will work great, but, you have to make yourself familiar with the LeapManager so as to know what exactly each function does, but if you do it using just the lib files and coding in all the gestures and such yourself, you will be able to have more freedom customizing the user experience. 

In this script, we start off by declaring our variables, as usual. This time, we will be using the Controller object, so don’t for to declare that and initialize it in our Start function. We will also need a variable to hold the bullet transform, and a bulletCount for the amount of bullets that are available. In our Start function, there are a few things that we will initialize. First, of course, is the Controller object. After that, we have to initialize our gestures that we will be using. Like so:

//enable the gestures that will be used
        controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
        controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
        controller.EnableGesture(Gesture.GestureType.TYPEINVALID);

	The gesture names “TYPEINVALID” is a default non-gesture gesture which basically states what happens if no initialized gesture is made. The other two are “TYPESCREENTAP” and “TYPESWIPE” which are, as the name states, a screen tapping motion and a swiping motion respectively. This is all you need to do in your Start function so as to let Unity know that you will be using these gestures. There are two move, which are “TYPECIRCLE” and “TYPEKEYTAP” which recognize circular motions and key tapping like motions. 
Now we take a look at our Update function, or as I like to call it, where the magic happens. Now that we have all those initialized, we need to look at another concept of the Leap Motion API and that is the Frame object. The Leap Motion controller recognizes and registers gestures frame by frame, which is why the code is included in the Update function rather than in the Start function, because we need it to keep registering and updating frame by frame. We declare a Frame object and initialize it, like so:

Frame frame = controller.Frame();

Now that we have that declared and initialized, we will use the frame object to grab the gesture data, which can only be accessed from the frame object, because they are registered frame by frame. I repeat this because it is a very important concept to learn and if you try to do this any other way IT WILL NOT WORK. The Leap Manager in the other script had frame objects and all that already created for us to use. We are doing it ourselves here so that we can be better programmers! Now we need to access the gestures, which we can do by using a “foreach” loop and going through the gestures and checking for each type of gesture that we declared and initialized earlier in our start function. The code will look like so:

foreach (Gesture gesture in frame.Gestures())
        {
            switch(gesture.Type)
            {
        
            case(Gesture.GestureType.TYPEINVALID):
            {
                Debug.Log("Invalid gesture recognized.");
                break;
            }
        
            //a simple motion of tapping at th screen
            case(Gesture.GestureType.TYPESCREENTAP):
            {
                Debug.Log("Screen tap gesture recognized.");
                if (bulletCount > 0){
                shootBullet();
                }
                break;
            }
            
            //a swipe of the finger or pointer in any direction
            case(Gesture.GestureType.TYPESWIPE):
            {
                Debug.Log("Swipe gesture recognized.");
                reloadSound.Play();
                bulletCount = 6;
                break;
            }
    
            default:
            {
                break;
            }
            }
        }

	Now as we can see, both the “TYPESCREENTAP” and “TYPESWIPE” cases have code inside them that runs when the gestures are detected. The screen tap gesture will first check to see if we have enough bullets to fire, if that is true, then it will call upon our shootBullet() function, which looks like this:

        public void shootBullet(){
        
        Instantiate(bullet, transform.position, transform.rotation);
        shootSound.Play();
        bulletCount -= 1;
        
    }

	The shootSound part is something optional, but really cool, that will add some sound effects to your game. Basically I declared two AudioSource variables at the top with the other variable declarations before the start functions:

	public AudioSource reloadSound;
    	public AudioSource shootSound;
	
	Each of them will carry a sound file for their own effect, both of which are in the github repository. So back to the function, it instiantiates the bullet in the direction and rotation that our shooter game object is facing (which could be actual crosshairs itself OR as I like to do this kind of thing, create an empty game object, point it to where you would like the shooting direction to be, for example on the other side of the crosshair and not towards the camera or the sky, and make it a child of the crosshair. This works perfectly well and is more efficient than having this script on the actual crosshair). The function then decreases the number of bullets by 1 and plays the shooting sound. 

	Our swipe gesture case will reload the weapon, doing the sound effect for that and adding 1 to our bulletCount. It’s pretty simple how this is done and you can do it too! Just declare what type of gesture you want to use in the Start function, and then do what we did in the Update function and add your code in there. There are other ways to make it more customizable for the user, like how far they can go a certain direction and what not, and that you can do by using the code from the “sample” codes in the SDK. 

	Right now, our game has a crosshair, a shooting object, bullets that fire when you tap at the screen (do not make contact with your screen, that is bad), and ammo that reloads when you swipe your finger! That’s awesome! But how will the user know how many bullets are left? What kind of game is this? NO GUI?! We are not bad game programmers, so let’s add some GUI elements that will appeal to our user and keep them informed. This can be easily done by putting an OnGUI function at the end of our script that we were just in (the shooterObject script):

       void OnGUI () {

        //convert the count of bullets to a string so as to be able to display it
        string bulletName = bulletCount.ToString();

        // Make a background box
        GUI.Box(new Rect(45,30,50,25), "Ammo");
        GUI.Box(new Rect(10,10,100,40), bulletName);
        
        //game instructions
        GUI.Box(new Rect(900,40,300,25), "Shoot the thingies!");
        GUI.Box(new Rect(900,60,300,25), "Screen Tap to shoot");
        GUI.Box(new Rect(900,20,300,25), "Swipe to reload");
        GUI.Box(new Rect(900,80,300,25), "Have fun!");
    }

	We do a couple of really simple things here in our OnGUI function. First of all, we cannot display int values in our GUI.Box so we have to convert our bulletCount to a string value before we can use it. Now that we have that, we create our GUI boxes and initialize new Rect’s in them to hold our information. The parameters of the Rect are as follows: (x position, y position, width, height). The top left corner is (0,0). So you have a general idea of where you want to put your Rect’s. Make one that just says “Ammo” and one right under it that actually shows the number of bullets you have left. The other 4 boxes are there just as instructions to the actual game.
	
	One last important thing about the bullets that you must now forget is to make sure our bullet knows what it’s doing! We can’t have it go on forever, then we will potentially have infinite amounts of bullets running around our game and that’s really bad for our processor. This script also makes sure that the bullet goes straight forward from the point it’s instantiated from. This is the ProjectileSpeed.js script. Unlike the leap motion integrated scripts, this one is in JavaScript. Why? Because the Leap Motion only works with C#, but for everything else we can still use JavaScript (or Boo, if you’re into it). We could write this in C# too, whatever your preference is. This is an extremely simple and basic script. It also makes sure that upon collision, the bullet will get destroyed so that it doesn’t go though things. 
	All you do is have this in the Update function:
	
time += Time.deltaTime;
transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

if (time > 3)
{

Destroy(gameObject);

}

Don’t forget to declare the time and speed variables at the top:

var speed = 20.0;
var time = 0.0;

Then one more function to check for the collision:

function OnCollisionEnter (projectileCollision : Collision)
{

Destroy(gameObject);

}

	Now you are all set! Your finger is now a weapon that shoots bullets! Well, only in this game project. Don’t actually try and shoot bullets out of your fingers, people might find that weird. But show them this game and show them how awesome you are for making this! 

	But wait, what are we shooting at? Oh right! Let’s add some moving targets! First we need the targets, so grab the target prefabs that my good friend Matthew Allen modeled himself from scratch. Now put one out in the scene in front of the crosshair where it can align with it. Make sure that it has a rigid body and a mesh collider. One simple script will make sure those suckers disappear when we shoot enough bullets at them, and it’s the targetHealth.js script. We first have to declare the health variable called targetHealth and give it a value:
	
	public var targetHealth = 2.0;

	After that, in our update function, we need to check if the health is equal to or less than zero, so as to know if the target is “dead”:
	
	if (targetHealth <= 0.0)
{

Destroy(gameObject);
}

One last function to add, and that would be the collision check function: 

function OnCollisionEnter(projectileCollision : Collision)
{

if (projectileCollision.transform.name == ("bullet(Clone)"))
{


targetHealth -=1;


}

}

	We name the colliding target “bullet(Clone)” instead of just “bullet” because that’s what they’re called in the hierarchy if they are instantiated from the same prefab, because clones of it are created. 

	One last cool trick we will look at is making some of the targets move left and right, or even up and down if you prefer that instead. This will add some challenge to the game and make it seem like an actual shooting range. For this, we will look at my script named targetMove.js. Again, this is in JavaScript, but you can write it in C# or Boo, according to your preference. We declare a few variables first to hold information on the x position of the target, it’s y position, and the maximum position we want to set for movement. 

	private var Xpos : float;
private var Ypos : float;
private var max : boolean;

After that, w have three more variables, but these are public variables, meaning we can see them and change them from the Unity interface instead of just the code. The first one is a Boolean variable named Vert, when that is true, the code will make the target move vertically, otherwise if it’s false, the code will make it move horizontally. The next two are maxAmount and step, which are useful to be changeable from the interface so as to test how far you wanna set the maximum amount and how fast you want it to step between extremes. The declarations look like so:

var Vert : boolean;
var maxAmount : int;
var step : float;

Our Update function is changed to a FixedUpdate because that handles this code better after testing. There are two pieces of control flow that will handle setting the max range of the target’s movement and one that will move the target. 

//set the max
	if (Vert) 
	{
		if (transform.position.y >= Ypos + maxAmount)
		{
		max = true;
		}
		else if (transform.position.y <= Ypos)
		{
		max = false;
		}
	}
	
	else 
	{
	
	if (transform.position.x >= Xpos + maxAmount)
		{
		max = true;
		}
		else if (transform.position.x <= Xpos)
		{
		max = false;
		}
	
	}

This is very simple code, it first checks to see if we are moving vertically or horizontally, and according to that, it changes the Y and X transforms respectively. After checking for the Boolean condition, it checks to see if the current position of the target and if it is at the maximum position, and if it is, then the Boolean max is set to true,  and if it is not at the maximum, then it is set to false. This Boolean variable will be used in the next piece of code that actually moves the target:

//moving the platform
	if (Vert){
		if(!max){
			transform.position.y +=step;
		}
		else 
		{
			transform.position.y -=step;
		}
		}
	else //horizontal movement 
	{
	if(!max){
			transform.position.x +=step;
		}
		else 
		{
			transform.position.x -=step;
		}
		
	}

This is also another simple piece of code. Like the other chunk of code we just looked at, it checks for the Vert Boolean, to see if it’s going to be changing the X or Y transform. After that, the max Boolean that we manipulated earlier will be used here, and if it is true, then you move in the opposite direction of where you are so that you can reach the other extreme. If it is false, then you keep moving in the positive direction. This is all that there is to it. Your targets now move based on your specifications. Play around with the maxAmount, Vert, and step variables and see what works best for you!

	And there you have it. You have no created a fully functional and fun Leap Motion Unity3D game. You learned how to integrate Leap using a prebuilt Leap Manager class, or just by simply using the libs and creating your own instances of the classes and making the objects work together to get your data. You learned how to make sure that the crosshair moves with your finger, and that it shoots when you do a screen tap motion and reloads when you swipe. You also learned how to set up targets and shoot them down. Enjoy the game! Hope this was educational, happy shooting! 
