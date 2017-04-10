# 3D-Snake
Technical Document
List of Scripts:
	GameManager: Singleton Monobehaviour
Complete Game state control. Pass Game state to all other manager objects by abstract class GameStateAbstract .
 

•	GameStateAbstract:  Inherit from MonoBehaviour
Subscribe/Unsubscribe Game state event to other modules.

	CameraManager: Inherit from GameStateAbstract
•	BlurOptimized: Blur the screen which is took from UnityStandardAssets.
•	CameraShake: To shake camera when GameOver reference from http://wiki.unity3d.com/index.php/Camera_Shake
Modified as per requirement

	GridSystemManager: Inherit from GameStateAbstract
•	Creates Grid by using Serialized class GridNode

	VFXManager: Inherit from GameStateAbstract
Based on the game state activate vfx.
•	VFXLineRenderer: Renders line based on Mouse/Touch swip
•	Enables fadeout head spin particles when gameover

	InputManager: Inherit from GameStateAbstract
Handles Keyboard /Mouse swip based on game state and flags in the inspector for keyboard and mose.
 


	ObstacleManager: Inherit from GameStateAbstract
Handles to spawn obstacles 
 


	UIManager: Inherit from GameStateAbstract
Switches UI canvas basesd on Game state
 

	SoundManager: Inherit from GameStateAbstract
Handles all sounds which is having List of Audio source with names
 
	poolManager: Singleton Monobehaviour
•	MeshMerger: Took script from the URL http://wiki.unity3d.com/index.php?title=MeshMerger and modified as requirement.
 

For Snake Control: created 2 scripts to handle snake movement from GameManager
	SnakeController – Handles initial position and holds snake head component.
	SnakeBlock – Handles to position the pieces of snake.

Other Helping Script:

	UIScoreComponent:For Pass Score to UI component from GameManager.
	ConstantsList: Holds all required constant variables of the game.

