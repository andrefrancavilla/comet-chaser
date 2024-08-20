<---DOCUMENTATION--->

<---Getting Started--->
	To start using Instant Pooling, you can create a Pool System from the Menu Item 'Instant Tools/Instant Pooling/Create Pool System' or you can manually
	attach the InstantPool component (found in the 'Instant Tools/Instant Pooling/Scripts' directory) to a GameObject that already exists in the scene
	(although a dedicated Empty GameObject is recommended).

<---Configuring Object Pools--->
InstantPool.cs - InstantPool is the class that's responsible for the creation and configuration of all the object pools. There can only be 1 InstantPool present in each scene.
	If there are more than 1 InstantPool components present in the scene, you will likely encounter issues. Instant Pooling's main functionality relies on the pool list,
	situated in the InstantPool class/component. Every item in the pool list represents an object pool.

	Add Object Pools
	You can add a pool to the pool list and configure it using Instant Pool's Inspector UI, pressing the "Add Pool" button. By pressing the 'Add Pool' button, a new pool shows up on screen.
	Here you can assign a name*, a size and object type to the newly created pool. The object type refers to the GameObject (or Prefab) that will populate the object pool.	
	*NOTE: You must assign a unique name to the object pool. If there are duplicates, no object pool with such name will be created. The name is fundamental because
	it acts as an identifier that is used when you have to use the pooling system from your classes, which means that you can insert the object pool's name and obtain
	an object that belongs to that pool.
	
	Generate Object Pools
	After you add and configure all the object pools you want (you can have as many object pools you want, only hardware resources are your limit), 
	you have to press the "Generate Pools" button on the Inspector UI of the InstantPool component to load the object pools into the scene. 
	If you happen to modify anything about an object pool, all the modifications will apply	when you press the "Generate Pools" button.
	
	Remove Object Pools
	You can remove an object pool from the object pool list by pressing the "x" button that's on the right side of each object pool present in the InstantPool's Inspector UI, 
	next to the 'object' selection.	
	Reminder: all modifications, adds and removals that you do will take effect only after you press the "Generate Pools" button.

	Is Dynamic
	If this option is enabled on an object pool, the object's pool will automatically expand if the object request is superior to what the given pool size can offer. 
	Keep in mind that if the starting pool size is too small compared to the object request, you might encounter an initial performance drop upon the pool's expansion; 
	this is because the pool expands through the Instantiate method.

PoolManager.cs - PoolManager is a class that can be accessed directly without the need to create an instance of it (because it's static). It contains all the methods 
needed to make use of the previously created object pools through script during runtime. (Details in the API section)


<---API--->
All of Instant Pooling's classes belong to the InstantTools.PoolSystem namespace. In order to use any of the code mentioned below, remember to include the namespace by writing "using InstantTools.PoolSystem;"
at the beginning of all your classes and components that will make use of Instant Pooling. In most cases, PoolManager will be the only class you'll be using, but you can make use of the InstantPool's
source code (described below) to have manual control over what you're doing.

PoolManager:
This class is a public static class that is accessible from any script you make, as long as you included the 'InstantTools.PoolSystem' namespace. It contains all of the methods
needed to delete and add object pools and fetch objects from a specific object pool defined by the poolName parameter. For organization purposes, they're divided into USAGE methods
and MODIFICATION methods. For basic functionality, you'll only need to make use of the USAGE methods.

Note: Since all the objects present inside the object pool are disabled by default, when you use GetObject or ActivateObject, the GameObject returned from these methods will
be automatically activated. Also, please note that to fetch an object from the object pool, the given poolName has to be exactly the same as the name of the pool you're looking for,
since it's case-sensitive.

-Method list:
	[USAGE]
	> GetObject(string poolName)
		|> Based on the inserted poolName parameter, this method will return an activated (but not yet utilized) GameObject that belongs to the specified pool.
			EXAMPLE ||> GameObject enemy = PoolManager.GetObject("Enemy 1");
					||> This will fetch an enemy that's present in the 'Enemy 1' object pool, activate it and assign it to the 'enemy' GameObject variable. 

	> ActivateObject(string poolName, Vector3 position, Quaternion rotation)
		|> Based on the inserted poolName, this method will return a GameObject that belongs to the specified object pool. 
		|> It will activate a GameObject that belongs to the specified pool, position it in the given position and assign it's rotation to the given rotation (position and rotation are specified 
		   in the method's parameters). 
		|> See this method as a shorthand as for example managing bullets, where it's necessary to position the bullet at the weapon's muzzle and rotate it towards where the player 
		   is aiming right in the moment when you want to shoot.
		   	EXAMPLE ||> GameObject projectile = PoolManager.ActivateObject("Shotgun Shells", projectileSpawn.transform.position, projectileSpawn.transform.rotation);
			   		||> This will fetch a projectile that's present within the 'Shotgun Shells' object pool, activate it, position it to the projectileSpawn's transform position and rotation
					    and assign it to the 'projectile' GameObject variable.
	

	[MODIFICATION]
	[The following methods are used mainly for more advanced scenarios, where you want to modify InstantPool's object pool list during runtime]

	> DeletePool(string poolname, bool autoUpdate)
		|> Based on the inserted poolName parameter, this method will destroy and remove the specified object pool from InstantPool's object pool list.
		|> If there are no object pools found, it will return an error message.
		|> autoUpdate defines if you want the changes to the pool system to be applied as soon as the wanted object pool is deleted. Set it to false if you want to manually handle
		   the object pool's regeneration. (detailed autoUpdate parameter functionality and example further down the documentation)
			EXAMPLE ||> PoolManager.DeletePool("Red projectile", true);
					||> This will delete the object pool named 'Red projectile' from the scene during runtime and automatically apply the changes made.

	> AddPool(string poolName, int poolSize, GameObject poolObject, bool isDynamic, bool autoUpdate)
		|> This method will add an object pool to InstantPool's object pool list. The object pool's name, size, object and dynamic settings are defined through this method's parameters.
		|> If there's already an object pool that contains the same name as the name defined in the poolName parameter, it will return an error message.
		|> autoUpdate defines if you want the changes to the pool system to be applied as soon as the wanted pool is added. Set it to false if you want to manually handle
		|> the object pool's regeneration. (detailed autoUpdate parameter functionality and example further down the documentation)
			EXAMPLE ||> PoolManager.AddPool("Brown projectile", 20, brownProjectile, true, true);
					||> This method will add an object pool named 'Brown projectile' that has a size of 20, populated by the 'brownProjectile' GameObject and that's dynamic and will automatically
					    apply all the changes made.
	
	-The use of autoUpdate-
	autoUpdate is a parameter that defines if the modification made to InstantPool's object pool list will be automatically applied. This means that if you set autoUpdate to true
	when you use DeletePool or AddPool, InstantPool's GeneratePools() method will automatically be called. It is handy in some cases to set the autoUpdate parameter to false
	if you are making multiple changes to the object pool during runtime in a very short period of time, because it avoids the regeneration of all the object pool's present in the
	object pool list every time you call the previously mentioned methods.
		EXAMPLE: Let's say that you want to replace a projectile with a more powerful projectile because the player got an upgrade. This will require 2 object pool modification methods,
				 so setting autoUpdate to false on the first method call would be considered a best practice for performance.
				 Here's some example code for the situation mentioned above:

				 PoolManager.DeletePool("Projectiles", false);
				 PoolManager.AddPool("Projectiles", 30, poweredProjectile, true, true);

				 This will delete the object pool named 'Projectiles' that was populated with the 'regularProjectile' GameObject, 
				 then add a new dynamic object pool with the same name and a size of 30, but populated with the 'poweredProjectile' GameObject. 
				 Since the autoUpdate parameter in the 'AddPool' method above is set to true, the modifications that were made by the DeletePool method will also be applied.


InstantPool:
As mentioned previously, InstantPool is the class that's responsible for the creation and configuration of all the object pools. It contains the object pool list on which the entire
InstantPooling tool relies upon.

-Method list:
	> GeneratePools()
		|> GeneratePools is the method that applies any modifications made to the object pool list. This method is automatically called when using PoolManager's MODIFICATION methods
		|> if 'autoUpdate' is set to true.
			EXAMPLE ||> FindObjectOfType<InstantPool>().GeneratePools();
					||> Since InstantPool is an actual component and there can be only 1 InstantPool component present in a scene at the same time, it's safe to use
					    FindObjectOfType<InstantPool>() to use it's methods.