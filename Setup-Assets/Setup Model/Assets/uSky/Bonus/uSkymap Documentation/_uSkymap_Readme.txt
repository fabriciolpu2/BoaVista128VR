uSkyMap for Ocean Material
===============================

SETUP IN NEW SCENE
----------------------

1)	Apply uSkymapRenderer component to uSkyManager gameobject.
	Note: both uSkyMapRenderer and uSkyManager component must be in the same gameobject.
	
	Menu : Component > uSky > uSkymap Renderer
	
2)	Load the Ocean prefab (scrawk's ocean) to the scene.

3)	Apply the Ocean Material to SkymapRenderer.	
	uSkymapRenderer will render and apply the uSkymap RenderTexture to the Ocean Material.

----------------------------------------------------------------------------------------
Note: Ocean Material requires additional attributes from the Scrawk's "Sky" script: 
	RenderTexture	_Transmittance	
	RenderTexture	_Irradiance

	RenderTexture	_Variance (Texture3D)
	
	Please assign requires textures to the ocean material to make the sun reflection works correctly. 
	
	About the ocean surface color, you may need to make it darker. The ocean color that in the webplayer demo is 
	R= 0 , G = 3 / 255, B = 8 / 255.