
// standard global variables
var container, scene, camera, renderer, uniforms;
var keyboard = new THREEx.KeyboardState();

// custom global variables
var video, videoImage, videoImageContext, videoTexture;

var textureList = [];

init();
animate();

// FUNCTIONS 		
function init() 
{
	// SCENE
	scene = new THREE.Scene();
	// CAMERA
	var SCREEN_WIDTH = window.innerWidth, SCREEN_HEIGHT = window.innerHeight;
	var VIEW_ANGLE = 45, ASPECT = SCREEN_WIDTH / SCREEN_HEIGHT, NEAR = 0.1, FAR = 1000;
	camera = new THREE.PerspectiveCamera(VIEW_ANGLE, ASPECT, NEAR, FAR);
	// RENDERER
	renderer = new THREE.WebGLRenderer({ antialias:true });
	renderer.setSize(SCREEN_WIDTH, SCREEN_HEIGHT);
	container = document.getElementById('ThreeJS');
	container.appendChild( renderer.domElement );
	// EVENTS
	THREEx.WindowResize(renderer, camera);
	
	///////////
	// VIDEO //
	///////////

	video = document.getElementById('monitor');
	
	videoImage = document.getElementById('videoImage');
	videoImageContext = videoImage.getContext('2d');
	videoImageContext.fillStyle = '#000000';
	videoImageContext.fillRect( 0, 0, videoImage.width, videoImage.height );

	videoTexture = new THREE.Texture(videoImage);
	videoTexture.minFilter = THREE.LinearFilter;
	videoTexture.magFilter = THREE.LinearFilter;

	uniforms = {
		"uResolution" : { type: "v2", value: new THREE.Vector2( SCREEN_WIDTH, SCREEN_HEIGHT ) },
		"uTexture" : { type: "t", value: videoTexture }
	};

	window.addEventListener('resize', function () {
		uniforms.uResolution.value = new THREE.Vector2( window.innerWidth, window.innerHeight )
	}, false);

	var movieMaterial = new THREE.ShaderMaterial({
    uniforms: uniforms,
    vertexShader: document.getElementById('vertexShader').textContent,
    fragmentShader: document.getElementById('fragmentShader').textContent
	});
	
	var movieGeometry = new THREE.PlaneGeometry(2, 2, 1, 1);
	var movieScreen = new THREE.Mesh( movieGeometry, movieMaterial );
	movieScreen.position.set(0, 0, 0);
	scene.add(movieScreen);
	
	camera.position.set(0, 0, 1);
	camera.lookAt(movieScreen.position);	


	///////////////////
	// TIME TEXTURES //
	///////////////////

	var sliceSize = 1;
	for (var i = 0; i < videoImage.height / sliceSize; ++i) {
		textureList.push([]);
	}
}

function animate () 
{
	requestAnimationFrame(animate);
	render();
}

function render () 
{	
	renderVideoTexture();
	renderer.render(scene, camera);
}

function renderVideoTexture () 
{
	if (video.readyState === video.HAVE_ENOUGH_DATA) {
		videoImageContext.drawImage(video, 0, 0, videoImage.width, videoImage.height);
		if (videoTexture) {
			videoTexture.needsUpdate = true;
		}
	}
}