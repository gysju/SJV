using UnityEngine;
using System.Collections;

public class Player : Entity {

	[Range(0.1f,1.0f)]
	public float Speed = 0.1f;

	[Range(1.0f,30.0f)]
	public float rotationSpeed = 2.0f;

    public static Player Instance { get; private set; }

    private Transform pivot;
	private Arm leftArm;
	private Arm rightArm;

	private float rightAxisH;
	private float rightAxisV;

    private float currentPosition; 

    protected override void Start () 
	{
        Instance = this;

        pivot = transform.FindChild("Pivot");
        rightArm = pivot.FindChild ("RightArm").GetComponent<Arm>();
		leftArm = pivot.FindChild("LeftArm").GetComponent<Arm>();
	}

	protected override void Update ()
	{
		rightAxisH = Input.GetAxis("HorizontalR") * Time.deltaTime;
		rightAxisV = Input.GetAxis("VerticalR") * Time.deltaTime;

		Move ();
		Rotation ();
		Attack();
	}

	void Rotation()
	{
        transform.LookAt(Vector3.zero);
        pivot.rotation = Quaternion.Euler (new Vector3(rightAxisV * rotationSpeed + pivot.rotation.eulerAngles.x, 
                                                       rightAxisH * rotationSpeed + pivot.rotation.eulerAngles.y, 
                                                       0.0f));
	}

	void Move()
	{
        if (Input.GetButton("LeftButton"))
        {
            currentPosition -= Speed * Time.deltaTime;
        }
        else if (Input.GetButton("RightButton"))
        {
            currentPosition += Speed * Time.deltaTime;
        }
        transform.position = Vector3.Lerp(BaliseManager.Instance.firstBalise.transform.position + new Vector3(0.0f, transform.position.y, 0.0f), 
                                          BaliseManager.Instance.secondeBalise.transform.position + new Vector3(0.0f, transform.position.y, 0.0f),
                                          currentPosition);
        CheckBalise();
    }

    void CheckBalise()
    {
        if (currentPosition < 0.0f)
        {
            currentPosition = 1.0f;
            BaliseManager.Instance.PreviousStep();
        }
        else if (currentPosition > 1.0f)
        {
            currentPosition = 0.0f;
            BaliseManager.Instance.NextStep();
        }
    }

    void Attack()
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			rightArm.Shoot(this);
		}
		if (Input.GetButtonDown ("Fire2")) 
		{
			leftArm.Shoot(this);
		}
	}

    protected override void OnDeathEnter()
    {
        base.OnDeathEnter();
    }

    protected override void OnDeathUpdate()
    {

    }

    protected override void OnDeathExit()
    {

    }
}
