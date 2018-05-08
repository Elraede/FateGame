using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class JumpSkill : Skill {
    public Rigidbody2D rigidBody;
    public bool skill1 = false;
    public Texture2D cursorImage;
    public Vector3 mousePosition = new Vector3();
    public Camera camera;
	// Use this for initialization
	void Start () {
        manaCost = 10;
        cooldown = new Cooldown();
        cooldown.coolDown = 6;
        isActive = false;
        isUsed = false;
    }

    private void FixedUpdate()
    {
    }
    // Update is called once per frame
    void Update () {


        if (Input.GetKeyDown(KeyCode.Alpha1) && isCooldown == false)
        {
            isActive = true;
            skill1 = true;
            camera.orthographicSize = 118;
        }

        if (isActive && CrossPlatformInputManager.GetButtonDown("Fire2"))
        {
            isActive = false;
            skill1 = false;
            camera.orthographicSize = 66.72548f;
        }
        if (skill1)
        {
            //Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                isCooldown = true;
                imageCooldown.fillAmount = 1;
                isUsed = true;
                Vector3 worldMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition = worldMousePos;
                skill1 = false;
                camera.orthographicSize = 66.72548f;
            }
        }
        if (isCooldown)
        {
            imageCooldown.fillAmount -= 1 / cooldown.coolDown * Time.deltaTime;

            if (imageCooldown.fillAmount <= 0)
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }
	}
}
