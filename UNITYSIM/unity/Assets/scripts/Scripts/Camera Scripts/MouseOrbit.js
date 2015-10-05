var target2 : GameObject;
var target : Transform;
var distance = 10.0;

var xSpeed = 250.0;
var ySpeed = 120.0;

var yMinLimit = -20;
var yMaxLimit = 20;

private var x = 0.0;
private var y = 0.0;

@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   	if (GetComponent.<Rigidbody>())
		GetComponent.<Rigidbody>().freezeRotation = true;
}

function LateUpdate () {
    if (target) {
       // x += UnityEngine.Mathf.Round(Input.GetAxis("Window Shake X") * 10) * xSpeed * 0.002 ;
        x = target2.transform.rotation.eulerAngles.y ;
        y -= UnityEngine.Mathf.Round(Input.GetAxis("Window Shake Y") * 10) * ySpeed * 0.005;
 		
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
 		
 		if ( y < 5 ) y = 5;
        var rotation = Quaternion.Euler(y, x, 0);
        var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
        
        transform.rotation = rotation;
        transform.position = position;
    }
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}