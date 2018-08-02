using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour 
{
	private float nextFire = 0.0f;
	public float fireRate = 0.5f;
	public float speed;
	public float tilt;
    public float accelationThreshold;
	public Boundary boundary;
	public GameObject shot;
    public Transform shotSpawn;

	void Update() 
	{
		// if(Input.GetButton("Fire1") && Time.time > nextFire)
		if((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
		}
	}
	void FixedUpdate()
	{
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;
        if(SystemInfo.supportsAccelerometer)
        {
            moveHorizontal = Input.acceleration.x >= accelationThreshold ? 1 :
                             Input.acceleration.x <= -accelationThreshold ? -1 : 0;
            moveVertical = Input.acceleration.y >= accelationThreshold ? 1 :
                             Input.acceleration.y <= -accelationThreshold ? -1 : 0;
        }
        else
        {
		    moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        // 플레이어 이동
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement*speed;

		// 플레이어의 이동범위 설정
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		// 기울기 효과
		GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
}
