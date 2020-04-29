using UnityEngine;
using System.Collections;

public class RayCasting 
{
	Camera _raycast_cam;
	float Ray_distance;
	public RayCasting(Camera cam,float ray_distance)
	{
		_raycast_cam = cam;
		Ray_distance = ray_distance;
		
	}
	
	public GameObject castRay()
	{
		
		 Ray ray ;
		
     	if(Application.isEditor || Application.isWebPlayer)
		{
			Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
			ray= _raycast_cam.ScreenPointToRay(position);
		}
		else 
		    ray= _raycast_cam.ScreenPointToRay(Input.touches[0].position);
		
		
	    	RaycastHit hit;
			
	
			Physics.Raycast(ray, out hit, Ray_distance);
	

		
				if(hit.transform != null)
				{
			   
			      return hit.transform.gameObject;
		         }
		        else
			    return null;
	}

	public RaycastHit castRayHit()
	{
		
		Ray ray ;
		
		if(Application.isEditor || Application.isWebPlayer)
		{
			Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
			ray= _raycast_cam.ScreenPointToRay(position);
		}
		else 
			ray= _raycast_cam.ScreenPointToRay(Input.touches[0].position);
		
		
		RaycastHit hit;
		
		Debug.DrawRay(ray.origin,ray.direction*Ray_distance,Color.blue);
		Physics.Raycast(ray, out hit, Ray_distance);

		return hit;

	}
	
	public GameObject castRay(Vector2 position)
	{
   		 Ray ray = _raycast_cam.ScreenPointToRay(position);
   			RaycastHit hit;

		Physics.Raycast(ray, out hit, Ray_distance);

	  
		if(hit.transform != null)
		{
	      return hit.transform.gameObject;
         }
        else
	    return null;
	}
	
	
	
}
